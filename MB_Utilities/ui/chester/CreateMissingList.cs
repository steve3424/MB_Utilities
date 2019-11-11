using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.controls.chester
{
    public partial class CreateMissingList : UserControl
    {
        // state of file
        private const int FILE_READY = 0;
        private const int FILE_PATH_EMPTY = 1;
        private const int FILE_NOT_FOUND = 2;

        // state of folder
        private const int FOLDER_READY = 3;
        private const int FOLDER_PATH_EMPTY = 4;
        private const int FOLDER_NOT_FOUND = 5;
        private const int FOLDER_IS_EMPTY = 6;
        private const int CONTAINS_BAD_FILE = 7;

        public CreateMissingList()
        {
            InitializeComponent();
        }


        /************* BUTTON CLICK HANDLERS ******************/

        private void chooseLogFileBTN_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Text File (.txt)|*.txt" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    logFilePathField.Text = openFileDialog.FileName;
                }
            }
        }

        private void chooseFileFolderBTN_Click(object sender, EventArgs e)
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

        private void createMissingListBTN_Click(object sender, EventArgs e)
        {
            // check state of file and folder before executing
            int fileState = getFileState();
            int folderState = getFolderState();
            if (fileState != FILE_READY)
            {
                showErrorMessage(fileState);
            }
            else if (folderState != FOLDER_READY)
            {
                showErrorMessage(folderState);
            }
            else
            {
                disableUI();

                HashSet<int> fileNames = loadFileNames();
                Dictionary<int, Dictionary<string, string>> logFile = loadLogFile();
                SortedDictionary<int, Dictionary<string, string>> missingList = createMissingList(logFile, fileNames);
                SortedDictionary<int, Dictionary<string, string>> voidedList = createVoidedList(logFile, fileNames);
                showMissingList(missingList);
                showVoidedList(voidedList);

                enableUI();
            }
        }



        /************* MAIN FUNCTIONS ******************/

        private HashSet<int> loadFileNames()
        {
            HashSet<int> fileNames = new HashSet<int>();
            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int chartNum = Int32.Parse(fileName);

                fileNames.Add(chartNum);
            }
            return fileNames;
        }

        private Dictionary<int, Dictionary<string, string>> loadLogFile()
        {
            Dictionary<int, Dictionary<string, string>> logFile = new Dictionary<int, Dictionary<string, string>>();

            string[] lines = File.ReadAllLines(logFilePathField.Text);
            foreach(string line in lines)
            {
                string[] chartInfo = line.Split(',');
                int chartNum = Int32.Parse(chartInfo[1]);
                string date = chartInfo[0];
                string lastName = chartInfo[2];
                string firstName = chartInfo[3];
                string logCode = chartInfo[4];
                Dictionary<string, string> info = new Dictionary<string, string>()
                {
                    {"date", date },
                    {"lastName", lastName },
                    {"firstName", firstName },
                    {"logCode", logCode },
                    {"missing", "" }
                };
                logFile.Add(chartNum, info);
            }
            return logFile;
        }

        private SortedDictionary<int, Dictionary<string, string>> createMissingList(Dictionary<int, Dictionary<string, string>> logFile, HashSet<int> fileNames)
        {
            SortedDictionary<int, Dictionary<string, string>> missingList = new SortedDictionary<int, Dictionary<string, string>>();
            foreach (int logNum in logFile.Keys)
            {
                if (!fileNames.Contains(logNum))
                {
                    // chart is missing AND is RG or already modified to TD
                    if (logFile[logNum]["logCode"] == "RG" || logFile[logNum]["logCode"] == "TD")
                    {
                        missingList.Add(logNum, logFile[logNum]);
                    }
                }
            }
            return missingList;
        }

        private SortedDictionary<int, Dictionary<string, string>> createVoidedList(Dictionary<int, Dictionary<string, string>> logFile, HashSet<int> fileNames)
        {
            SortedDictionary<int, Dictionary<string, string>> voidedList = new SortedDictionary<int, Dictionary<string, string>>();
            foreach (int logNum in logFile.Keys)
            {
                if (!fileNames.Contains(logNum))
                {
                    // log code CAN'T be RG or TD
                    if (!(logFile[logNum]["logCode"] == "RG") && !(logFile[logNum]["logCode"] == "TD"))
                    {
                        logFile[logNum]["missing"] = "-";
                        voidedList.Add(logNum, logFile[logNum]);
                    }
                }
                else if (logFile[logNum]["logCode"] != "RG")
                {
                    // chart is found, but is not a voided code
                    voidedList.Add(logNum, logFile[logNum]);
                }
            }
            return voidedList;
        }

        private void showMissingList(SortedDictionary<int, Dictionary<string, string>> missingList)
        {
            using (DataTable missingListTable = new DataTable())
            {
                missingListTable.Columns.Add("Chart #");
                missingListTable.Columns.Add("Patient Name");

                foreach (int chartNum in missingList.Keys)
                {
                    string patientName = string.Concat(missingList[chartNum]["lastName"], ", ", missingList[chartNum]["firstName"]);
                    missingListTable.Rows.Add(chartNum, patientName);
                }

                missingListOutput.DataSource = missingListTable;
                string missingTotal = missingList.Count.ToString();
                missingTotalLabel.Text = "Missing Total: " + missingTotal;
            }
        }

        private void showVoidedList(SortedDictionary<int, Dictionary<string, string>> voidedList)
        {
            using (DataTable voidedListTable = new DataTable())
            {
                voidedListTable.Columns.Add("Missing?");
                voidedListTable.Columns.Add("Chart #");
                voidedListTable.Columns.Add("Patient Name");
                voidedListTable.Columns.Add("Log Code");

                foreach (int chartNum in voidedList.Keys)
                {
                    string patientName = string.Concat(voidedList[chartNum]["lastName"], ", ", voidedList[chartNum]["firstName"]);
                    voidedListTable.Rows.Add(voidedList[chartNum]["missing"], chartNum, patientName, voidedList[chartNum]["logCode"]);
                }

                voidedListOutput.DataSource = voidedListTable;
                string voidedTotal = voidedList.Count.ToString();
                voidedTotalLabel.Text = "Voided Total: " + voidedTotal;
            }
        }


        /************* UTILITY FUNCTIONS ******************/

        private int getFileState()
        {
            if (string.IsNullOrEmpty(logFilePathField.Text))
            {
                return FILE_PATH_EMPTY;
            }
            else if (!File.Exists(logFilePathField.Text))
            {
                return FILE_NOT_FOUND;
            }
            return FILE_READY;
        }

        private int getFolderState()
        {
            if (string.IsNullOrEmpty(folderPathField.Text))
            {
                return FOLDER_PATH_EMPTY;
            }
            else if (!Directory.Exists(folderPathField.Text))
            {
                return FOLDER_NOT_FOUND;
            }
            else if (!Directory.EnumerateFiles(folderPathField.Text, "*.pdf").Any())
            {
                return FOLDER_IS_EMPTY;
            }
            else if (!fileNamesAreGood())
            {
                return CONTAINS_BAD_FILE;
            }
            return FOLDER_READY;
        }

        private bool fileNamesAreGood()
        {
            DirectoryInfo folder = new DirectoryInfo(folderPathField.Text);
            FileInfo[] files = folder.GetFiles("*.pdf");
            foreach (FileInfo file in files)
            {
                try
                {
                    string[] splitFileName = file.Name.Split('.');
                    int chartNum = Int32.Parse(splitFileName[0]);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is OverflowException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void disableUI()
        {
            chooseFileFolderBTN.Enabled = false;
            chooseFileFolderBTN.Enabled = false;
            createMissingListBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseFileFolderBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            createMissingListBTN.Enabled = true;
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case FILE_PATH_EMPTY:
                    MessageBox.Show("Please select a file.");
                    return;
                case FILE_NOT_FOUND:
                    MessageBox.Show("The file you selected could not be found.");
                    return;
                case FOLDER_PATH_EMPTY:
                    MessageBox.Show("Please select a folder.");
                    return;
                case FOLDER_NOT_FOUND:
                    MessageBox.Show("The folder you selected could not be found.");
                    return;
                case FOLDER_IS_EMPTY:
                    MessageBox.Show("There are no pdf files in the selected folder");
                    return;
                case CONTAINS_BAD_FILE:
                    MessageBox.Show("There was a problem with one or more file names. Please rename the files and try again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }



        /************* COPY OPERATION FUNCTIONS ******************/
        private void missingListOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && missingListOutput.Rows.Count > 0)
            {
                this.rightClickMenu.Show(this.missingListOutput, e.Location);
                this.rightClickMenu.Show(Cursor.Position);
            }
        }

        private void voidedListOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && voidedListOutput.Rows.Count > 0)
            {
                this.rightClickMenu.Show(this.voidedListOutput, e.Location);
                this.rightClickMenu.Show(Cursor.Position);
            }
        }

        private void copyRightClickMenuItem_Click(object sender, EventArgs e)
        {
            if (missingVoidedTabControl.SelectedIndex == 0)
            {
                Clipboard.SetDataObject(this.missingListOutput.GetClipboardContent());
            }
            else
            {
                Clipboard.SetDataObject(this.voidedListOutput.GetClipboardContent());
            }
        }
    }
}
