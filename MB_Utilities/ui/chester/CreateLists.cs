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

using OfficeOpenXml;

namespace MB_Utilities.ui.chester
{
    public partial class CreateLists : UserControl
    {
        private const int DELETE_ROWS_WARNING = 0;

        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;
        private const int MISSING_LIST_INCORRECT = 3;

        // state of log file
        private const int FILE_READY = 4;
        private const int FILE_PATH_EMPTY = 5;
        private const int FILE_NOT_FOUND = 6;

        // state of folder
        private const int FOLDER_READY = 7;
        private const int FOLDER_PATH_EMPTY = 8;
        private const int FOLDER_NOT_FOUND = 9;
        private const int FOLDER_IS_EMPTY = 10;
        private const int CONTAINS_BAD_FILE = 11;

        private const int CANNOT_SAVE_MISSING_LIST = 12;

        public CreateLists()
        {
            InitializeComponent();
        }


        /************* BUTTON CLICK HANDLERS ******************/

        private void chooseMissingListBTN_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Sheet (.xlsx)|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    missingListPathField.Text = openFileDialog.FileName;
                }
            }
        }

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

        private void createListsBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            DialogResult warningSelection = DialogResult.Yes;
            if (deleteRowsCheckBox.Checked)
            {
                warningSelection = showWarning(DELETE_ROWS_WARNING);
            }

            if (warningSelection == DialogResult.Yes)
            {
                int missingListState = getMissingListState();
                int fileState = getFileState();
                int folderState = getFolderState();
                if (missingListState != MISSING_LIST_READY)
                {
                    showErrorMessage(missingListState);
                }
                else if (fileState != FILE_READY)
                {
                    showErrorMessage(fileState);
                }
                else if (folderState != FOLDER_READY)
                {
                    showErrorMessage(folderState);
                }
                else
                {
                    HashSet<int> fileNames = loadFileNames();
                    Dictionary<int, Dictionary<string, string>> logFile = loadLogFile();
                    SortedDictionary<int, Dictionary<string, string>> missingList = createMissingList(logFile, fileNames);
                    SortedDictionary<int, Dictionary<string, string>> voidedList = createVoidedList(logFile, fileNames);
                }
            }

            enableUI();
        }


        /************* MISSING AND VOIDED LIST FUNCTIONS ******************/

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
            foreach (string line in lines)
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


        /************* UTILITY FUNCTIONS ******************/
        private int getMissingListState()
        {
            if (string.IsNullOrEmpty(missingListPathField.Text))
            {
                return MISSING_LIST_PATH_EMPTY;
            }
            else if (!File.Exists(missingListPathField.Text))
            {
                return MISSING_LIST_NOT_FOUND;
            }
            else if (!correctMissingList())
            {
                return MISSING_LIST_INCORRECT;
            }
            return MISSING_LIST_READY;
        }

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

        private bool correctMissingList()
        {
            FileInfo missingListPath = new FileInfo(missingListPathField.Text);
            using (ExcelPackage packageMissing = new ExcelPackage(missingListPath))
            using (ExcelWorksheet worksheetMissing = packageMissing.Workbook.Worksheets[1])
            {
                string title = worksheetMissing.Cells[1, 1].GetValue<string>();
                if (title == "CT Unbilled Report")
                {
                    return true;
                }
            }
            return false;
        }

        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseLogFileBTN.Enabled = false;
            chooseFileFolderBTN.Enabled = false;
            createListsBTN.Enabled = false;
            deleteRowsCheckBox.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseLogFileBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            createListsBTN.Enabled = true;
            deleteRowsCheckBox.Enabled = true;
        }

        private DialogResult showWarning(int warning)
        {
            string title = "Warning";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            switch (warning)
            {
                case DELETE_ROWS_WARNING:
                    string createListMessage = "This program will remove all of the charts from the missing list.\n\n" +
                "Are you sure you are ready to continue?";
                    return MessageBox.Show(createListMessage, title, buttons);
                default:
                    return DialogResult.No;
            }
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case MISSING_LIST_PATH_EMPTY:
                    MessageBox.Show("Please select the CT missing list");
                    return;
                case MISSING_LIST_NOT_FOUND:
                    MessageBox.Show("The CT missing list could not be found.");
                    return;
                case MISSING_LIST_INCORRECT:
                    MessageBox.Show("The missing list you chose is not the CT missing list.");
                    return;
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
                case CANNOT_SAVE_MISSING_LIST:
                    MessageBox.Show("It looks like the missing list is open somewhere else.\n\n" +
                        "The missing list will not be updated.\n\n" +
                        "Close the missing list and try again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
