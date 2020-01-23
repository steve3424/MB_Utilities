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
using MB_Utilities.utils;

/*
 * Thanks to Olivier Jacot-Descombes for his suggestion to load the spreadsheet lists into HashSets for faster lookups!! (https://stackoverflow.com/questions/58674525/is-it-better-practice-to-work-with-information-within-a-file-folder-or-to-load?answertab=votes#tab-top)
 */

namespace MB_Utilities.controls.chester
{
    public partial class ProcessStragglers : UserControl
    {
        private const int RENAME_WARNING = 0;

        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;
        private const int MISSING_LIST_INCORRECT = 3;
        private const int MISSING_LIST_CANNOT_SAVE = 4;

        // state of folder
        private const int FOLDER_READY = 5;
        private const int FOLDER_PATH_EMPTY = 6;
        private const int FOLDER_NOT_FOUND = 7;
        private const int FOLDER_IS_EMPTY = 8;
        private const int CONTAINS_BAD_FILE = 9;

        public ProcessStragglers()
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

        private void renameFilesBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            if (showWarning(RENAME_WARNING) == DialogResult.Yes)
            {
                // check state of missing list and files in folder
                int missingListState = getMissingListState();
                int folderState = getFolderState();
                if (missingListState != MISSING_LIST_READY)
                {
                    showErrorMessage(missingListState);
                }
                else if (folderState != FOLDER_READY)
                {
                    showErrorMessage(folderState);
                }
                else
                {
                    List<string> subListIDs = new List<string>() { "ME", "PM", "SC", "WR", "TD" };
                    List<SubList> subLists = createSubLists(subListIDs);

                    foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);

                        string listContainingChart = searchSubLists(subLists, fileName);
                        renameFile(listContainingChart, file);
                    }

                    MessageBox.Show("Rename complete!!");
                }
            }
            enableUI();
        }
        



        /************* SUB LIST FUNCTIONS ******************/

        private List<SubList> createSubLists(List<string> subListsToCreate)
        {
            List<SubList> subLists = new List<SubList>();


            FileInfo path = new FileInfo(missingListPathField.Text);
            using (ExcelPackage package = new ExcelPackage(path))
            using (ExcelWorksheet worksheet = package.Workbook.Worksheets[1])
            {
                foreach (string subListID in subListsToCreate)
                {
                    SubList newSubList = new SubList();
                    newSubList.name = subListID;
                    newSubList.startRow = findStartOfList(worksheet, subListID);
                    newSubList.endRow = findEndOfList(worksheet, subListID);
                    newSubList.patientInfo = loadPatientInfo(worksheet, newSubList);
                    newSubList.numPatients = worksheet.Cells[newSubList.endRow, 3].GetValue<int>();

                    subLists.Add(newSubList);
                }
            }
            return subLists;
        }

        private Dictionary<string, Dictionary<string, string>> loadPatientInfo(ExcelWorksheet worksheet, SubList subList)
        {
            Dictionary<string, Dictionary<string, string>> patients = new Dictionary<string, Dictionary<string, string>>();

            int startRow = subList.startRow;
            int endRow = subList.endRow;
            for (int row = startRow; row < endRow; ++row)
            {
                string chartNum = worksheet.Cells[row, 1].GetValue<string>();
                string patientName = worksheet.Cells[row, 2].GetValue<string>();
                string date = worksheet.Cells[row, 3].GetValue<DateTime>().ToShortDateString();

                Dictionary<string, string> patientInfo = new Dictionary<string, string>() 
                {
                    { "rowNum", row.ToString()},
                    { "patientName", patientName},
                    { "date", date}
                };
                patients.Add(chartNum, patientInfo);

                // maybe add exception handling for possible blank cells or cells with bad chart numbers ??
            }
            return patients;
        }

        private string searchSubLists(List<SubList> subLists, string fileName)
        {
            foreach (SubList subList in subLists)
            {
                if (subList.patientInfo.ContainsKey(fileName))
                {
                    return subList.name;
                }
            }
            return null;
        }

        private int findStartOfList(ExcelWorksheet worksheet, string subListID)
        {
            int start = 1;
            string cellValue = worksheet.Cells[start, 1].GetValue<string>();
            while (!cellValue.Contains(subListID))
            {
                start++;
                cellValue = worksheet.Cells[start, 1].GetValue<string>();
                while (cellValue == null)
                {
                    start++;
                    cellValue = worksheet.Cells[start, 1].GetValue<string>();
                }
            }
            start += 2;
            return start;
        }

        private int findEndOfList(ExcelWorksheet worksheet, string subListID)
        {
            int end = 1;
            string cellValue = worksheet.Cells[end, 1].GetValue<string>();
            while (!cellValue.Contains(subListID) || !cellValue.Contains("Total:"))
            {
                end++;
                cellValue = worksheet.Cells[end, 1].GetValue<string>();
                while (cellValue == null)
                {
                    end++;
                    cellValue = worksheet.Cells[end, 1].GetValue<string>();
                }
            }
            return end;
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
            else if (!correctFile())
            {
                return MISSING_LIST_INCORRECT;
            }
            return MISSING_LIST_READY;
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

        private bool correctFile()
        {
            // open spreadsheet and check first cell for "CT Unbilled Report"
            FileInfo path = new FileInfo(missingListPathField.Text);
            using (ExcelPackage package = new ExcelPackage(path))
            using (ExcelWorksheet worksheet = package.Workbook.Worksheets[1])
            {
                if (worksheet.Cells[1, 1].GetValue<string>() != "CT Unbilled Report")
                {
                    return false;
                }
            }
            return true;
        }

        private void renameFile(string listContainingChart, string fileToRename)
        {
            // rules on how to rename file when found in a particular list
            switch (listContainingChart)
            {
                case "ME":
                    appendToFileName("ME", fileToRename);
                    return;
                case "PM":
                    appendToFileName("PM", fileToRename);
                    return;
                case "SC":
                    appendToFileName("SC", fileToRename);
                    return;
                case "WR":
                    appendToFileName("WR", fileToRename);
                    return;
                case "TD":
                    return;
                case null:
                    appendToFileName("not on list", fileToRename);
                    return;
            }
        }

        private void appendToFileName(string suffix, string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string newFileName = string.Concat(fileName, " ", suffix, extension);
            string newFilePath = Path.Combine(directory, newFileName);
            File.Move(filePath, newFilePath);
        }


        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseFileFolderBTN.Enabled = false;
            renameFilesBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            renameFilesBTN.Enabled = true;
        }

        private DialogResult showWarning(int warning)
        {
            string title = "Warning";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            switch(warning)
            {
                case RENAME_WARNING:
                    string renameMessage = "Before running this program make sure you have separated all of the straggler files into your selected folder.\n\n" +
                "If the charts for your day are also in this folder it will rename all of them, which you don't want.\n\n" +
                "Are you sure you are ready to continue?";
                    return MessageBox.Show(renameMessage, title, buttons);
                default:
                    return DialogResult.No;
            }
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case MISSING_LIST_PATH_EMPTY:
                    MessageBox.Show("Please select a file.");
                    return;
                case MISSING_LIST_NOT_FOUND:
                    MessageBox.Show("The file you selected could not be found.");
                    return;
                case MISSING_LIST_INCORRECT:
                    MessageBox.Show("This missing list is missing the header 'CT Unbilled Report'. Is this the correct missing list?");
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
                case MISSING_LIST_CANNOT_SAVE:
                    MessageBox.Show("It looks like the missing list is open somewhere else.\n\n" +
                        "The straggler list will be created, but the missing list will not be modified.\n\n" +
                        "Close the missing list and try again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
