using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace MB_Utilities.controls.chester
{
    public partial class ProcessFiles : UserControl
    {
        private Stopwatch mStopwatch = new Stopwatch();

        // state of directory
        private const int FOLDER_READY = 0;
        private const int PATH_IS_EMPTY = 1;
        private const int PATH_NOT_FOUND = 2;
        private const int FOLDER_IS_EMPTY = 3;

        private const int BACKGROUND_WORKER_ERROR = 4;

        // list of our physicians to search for
        List<string> physicianList = new List<string>
        {
            "Ader",
            "Arnone",
            "Benedict, John",
            "Clark",
            "Cohen, Megan",
            "Crofcheck",
            "Deardorff",
            "Dhargalkar",
            "Doherty, Laura",
            "Donatelli",
            "Fan",
            "Fisk",
            "Gaffney, Kevin",
            "Gagliardi, Shannon Tracey",
            "Gelman, Ricardo",
            "Gibbons, Ryan",
            "Goudouvas",
            "Henry, Michael Thomas",
            "Henry, Michael T",
            "Hooper",
            "Jeffrey, Chad",
            "Jeffery, Chad", // sometimes Chad Jeffery's name is spelled differently
            "Kane",
            "Kelton",
            "Kossey",
            "Landi",
            "Lehmann",
            "Lynch, Joseph Patrick",
            "Matsco",
            "McKinley",
            "McMullan",
            "Mihalakis",
            "Modi",
            "Packard",
            "Parvis",
            "Paterno, Amanda",
            "Purvis, Michele Ann",
            "Roe",
            "Ryan, Devon",
            "Sainval",
            "Trevisan",
            "Solimini, Elizabeth", // Elizabeth Trevisan name change
            "Ware",
        };

        public ProcessFiles()
        {
            InitializeComponent();
        }

        /************* BUTTON CLICK HANDLERS ******************/

        private void chooseFilesFolderBTN_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = folderPathField.Text;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPathField.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void processFilesBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            // check state of folder before running
            int folderState = getFolderState();
            if (folderState != FOLDER_READY)
            {
                showErrorMessage(folderState);
                enableUI();
            }
            else
            {
                mStopwatch.Start();

                FileInfo[] files = loadFiles();
                backgroundWorker1.RunWorkerAsync(files);
            }
        }

        /****************** BACKGROUND WORKER FUNCTIONS **************/
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            /// CANNOT ACCESS UI THREAD
            
            FileInfo[] files = e.Argument as FileInfo[];
            SearchFiles(files);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /// ACCESS UI THREAD HERE
            ///

            outputField.Text = "Searching Files... " + e.ProgressPercentage + "% complete";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /// ACCESS UI THREAD HERE

            mStopwatch.Stop();

            if (e.Error != null)
            {
                showErrorMessage(BACKGROUND_WORKER_ERROR);
                outputField.Text = "";
            }
            else
            {
                TimeSpan ts = mStopwatch.Elapsed;
                string searchTime = string.Format("{0:00}m:{1:00}s.{2:00}ms", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                outputField.Text += "\nSearch Time: " + searchTime;
            }

            mStopwatch.Reset();
            enableUI();
        }

        /*************** UTILITY FUNCTIONS ***********************/

        private void SearchFiles(FileInfo[] files)
        {
            /// CANNOT ACCESS UI THREAD
            /// CALLED FROM backgroundWorker1_DoWork()
            /// 

            // for updating background worker
            float numFiles = files.Length;
            float numFilesSearched = 0;

            foreach (FileInfo file in files)
            {
                string filePath = file.FullName;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordField.Text);
                ReaderProperties readerProperties = new ReaderProperties();
                readerProperties.SetPassword(passwordBytes);
                using (PdfReader reader = new PdfReader(filePath, readerProperties))
                using (PdfDocument document = new PdfDocument(reader))
                {
                    // flags
                    bool goodChart = isGoodChart(document);
                    bool eight_zero_three = is803(document.GetFirstPage());

                    document.Close();
                    if (!goodChart || eight_zero_three)
                    {
                        renameFile(file, goodChart, eight_zero_three);
                    }
                }

                // update background worker progress
                numFilesSearched++;
                int percentComplete = (int)((numFilesSearched / numFiles) * 100);
                backgroundWorker1.ReportProgress(percentComplete);
            }
        }

        private int getFolderState()
        {
            if (String.IsNullOrEmpty(folderPathField.Text))
            {
                return PATH_IS_EMPTY;
            }
            else if (!Directory.Exists(folderPathField.Text))
            {
                return PATH_NOT_FOUND;
            }
            else if (!Directory.EnumerateFiles(folderPathField.Text, "*.pdf").Any())
            {
                return FOLDER_IS_EMPTY;
            }
            return FOLDER_READY;
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case PATH_IS_EMPTY:
                    MessageBox.Show("Please select a folder.");
                    return;
                case PATH_NOT_FOUND:
                    MessageBox.Show("The folder you selected could not be found.");
                    return;
                case FOLDER_IS_EMPTY:
                    MessageBox.Show("There are no pdf files in the selected folder");
                    return;
                case BACKGROUND_WORKER_ERROR:
                    MessageBox.Show("There was an error when processing the files.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }

        private FileInfo[] loadFiles()
        {
            DirectoryInfo folder = new DirectoryInfo(folderPathField.Text);
            FileInfo[] files = folder.GetFiles("*.pdf");
            return files;
        }

        private void disableUI()
        {
            chooseFilesFolderBTN.Enabled = false;
            processFilesBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseFilesFolderBTN.Enabled = true;
            processFilesBTN.Enabled = true;
        }

        private bool isGoodChart(PdfDocument document)
        {
            /* Current strategy for categorizing chart as good or bad
             * is to check if "Physical Exam" and one of our doctors name is
             * present on the same page.
             * 
             * There are some cases that it can't handle but is around 99% accurate
             */

            int numPages = document.GetNumberOfPages();
            for (int pageNumber = 1; pageNumber <= numPages; pageNumber++)
            {
                PdfPage currentPage = document.GetPage(pageNumber);
                string pageContent = PdfTextExtractor.GetTextFromPage(currentPage);
                if (pageContent.Contains("Physical Exam") && physicianOnChart(pageContent))
                {
                    return true;
                }
            }

            return false;
        }

        private bool is803(PdfPage firstPage)
        {
            /* Currently it works out that if this string is present on the first page, then the chart is misnamed
             * This very well could change at any point
             */
            string firstPageContent = PdfTextExtractor.GetTextFromPage(firstPage);
            if (firstPageContent.Contains("80300"))
            {
                return true;
            }

            return false;
        }

        private bool physicianOnChart (string pageContent)
        {
            foreach (string physician in physicianList)
            {
                if (pageContent.Contains(physician))
                {
                    return true;
                }
            }
            return false;
        }

        private void renameFile(FileInfo file, bool goodChart, bool eight_zero_three)
        {
            string[] fileName = file.FullName.Split('.');
            string newFileName = fileName[0];

            if (eight_zero_three)
            {
                newFileName += " - 803";
            }

            if (!goodChart)
            {
                newFileName += " - BAD";
            }

            newFileName += "." + fileName[1];
            File.Move(file.FullName, newFileName);
        }
    }
}
