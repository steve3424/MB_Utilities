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
        // string and column that indicate the end of the UnbilledRegularReport
        string EOF = "1/1";

        // sublist header and footer strings
        Dictionary<string, string> header_strings = new Dictionary<string, string>() {
            {"ME", "ME - MISSING EXAM OR PHYS NOTES"},
            {"PM", "PM - PROCEDURE NOTE MISSING"},
            {"SC", "SC - MISSING SIGNATURE BUT CODED"},
            {"SG", "SG - MISSING SIGNATURE"},
            {"TD", "TD - MISSING COMPLETE CHART"},
            {"WR", "need to implement"}
        };

        Dictionary<string, string> footer_strings = new Dictionary<string, string>() {
            {"ME", "ME - MISSING EXAM OR PHYS NOTES Total:"},
            {"PM", "PM - PROCEDURE NOTE MISSING Total:"},
            {"SC", "SC - MISSING SIGNATURE BUT CODED Total:"},
            {"SG", "SG - MISSING SIGNATURE Total:"},
            {"TD", "TD - MISSING COMPLETE CHART Total:"},
            {"WR", "need to implement"}
        };

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
                    List<string> subListIDs = new List<string>() { "ME", "PM", "SC", "SG", "WR", "TD" };
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

        // TODO: Maybe check newSubList.startRow to see if list was found and only search for endRow if it was
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
                    newSubList.patientInfo = loadPatientInfo(worksheet, newSubList.startRow, newSubList.endRow);

                    subLists.Add(newSubList);
                }
            }
            return subLists;
        }

        /**
         *  @RETURN =   5 rows ahead of where the header string was found (starts from 1 not 0)
         *              0 if list was not found
         *  @worksheet =    unbilled missing list containing all of the lists to be searched
         *  @subListID =    2 letter log code indicating list to be searched
         *  
         *  REQUIRES =  header strings to be in column B(2)
         *              EOF string to be in column P(16)
         *              first chart # to be 5 rows after header string
         */
         // TODO: Is the first chart # always 5 rows after header string ???
        private int findStartOfList(ExcelWorksheet worksheet, string subListID)
        {
            string cellValue = null;
            string header = header_strings[subListID];
            string eof_check = null;
            int start = 0;
            while ((cellValue != header) && (eof_check != EOF)) {
                ++start;
                cellValue = worksheet.Cells[start, 2].GetValue<string>();
                eof_check = worksheet.Cells[start, 16].GetValue<string>();
            }

            if (eof_check == EOF)
            {
                start = 0;
            }
            else {
                start += 5;
            }

            return start;
        }

        /**
         *  @RETURN =   row on spreadsheet where footer string was found
         *              0 if list was not found
         *  @worksheet =    unbilled missing list containing all of the lists to be searched
         *  @subListID =    2 letter log code indicating list to be searched
         *  
         *  REQUIRES =  footer strings to be in column C(3)
         *              EOF string to be in column P(16)
         */

        private int findEndOfList(ExcelWorksheet worksheet, string subListID)
        {
            string cellValue = null;
            string footer = footer_strings[subListID];
            string eof_check = null;
            int end = 0;
            while ((cellValue != footer) && (eof_check != EOF))
            {
                ++end;
                cellValue = worksheet.Cells[end, 3].GetValue<string>();
                eof_check = worksheet.Cells[end, 16].GetValue<string>();
            }

            if (eof_check == EOF)
            {
                end = 0;
            }

            return end;
        }

        /**
         *  @RETURN =   nested dictionary of chart numbers and patient info
         *
         *  @worksheet =    unbilled missing list containing all of the lists to be searched
         *  @startRow =     beginning of subList
         *  @endRow =       end of subList
         *  
         *  REQUIRES =  chart number to be in column A(1)
         *              patient name to be in column D(4)
         *              date of service to be in column G(7)
         *              any empty list that is passed in to have startRow and endRow both be 0
         */
         // TODO: I don't think I need anything other than the chart number for the Process Stragglers step
        private Dictionary<string, Dictionary<string, string>> loadPatientInfo(ExcelWorksheet worksheet, int startRow, int endRow)
        {
            Dictionary<string, Dictionary<string, string>> patients = new Dictionary<string, Dictionary<string, string>>();

            for (int row = startRow; row < endRow; ++row)
            {
                string chartNum = worksheet.Cells[row, 1].GetValue<string>();
                if (string.IsNullOrEmpty(chartNum) || string.IsNullOrWhiteSpace(chartNum)) {
                    continue;
                }
                string patientName = worksheet.Cells[row, 4].GetValue<string>();
                string date = worksheet.Cells[row, 7].GetValue<DateTime>().ToShortDateString();

                Dictionary<string, string> patientInfo = new Dictionary<string, string>() 
                {
                    { "rowNum", row.ToString()},
                    { "patientName", patientName},
                    { "date", date}
                };
                patients.Add(chartNum, patientInfo);
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
            /*
            else if (!correctFile())
            {
                return MISSING_LIST_INCORRECT;
            }
            */
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
                    appendToFileName("SC save", fileToRename);
                    return;
                case "SG":
                    appendToFileName("SG", fileToRename);
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
