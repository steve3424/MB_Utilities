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
        private const int CREATE_LIST_WARNING = 1;

        // state of file
        private const int FILE_READY = 0;
        private const int FILE_PATH_EMPTY = 1;
        private const int FILE_NOT_FOUND = 2;
        private const int INCORRECT_FILE = 3;

        // state of folder
        private const int FOLDER_READY = 4;
        private const int FOLDER_PATH_EMPTY = 5;
        private const int FOLDER_NOT_FOUND = 6;
        private const int FOLDER_IS_EMPTY = 7;
        private const int CONTAINS_BAD_FILE = 8;

        private const int CANNOT_SAVE_MISSING_LIST = 9;


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
                    // continue with rename

                    List<string> subListIDs = new List<string>() { "ME", "PM", "SC", "WR", "TD" };
                    List<SubList> subLists = createSubLists(subListIDs);

                    foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        int chartNum = Int32.Parse(fileName);

                        string listContainingChart = searchSubLists(subLists, chartNum);
                        renameFile(listContainingChart, file);
                    }

                    MessageBox.Show("Rename complete!!");
                }
            }
            enableUI();
        }

        private void createStragglerListBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            if (showWarning(CREATE_LIST_WARNING) == DialogResult.Yes)
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
                    List<string> subListIDs = new List<string>() { "ME", "PM", "TD"};
                    List<SubList> subLists = createSubLists(subListIDs);
                    SortedDictionary<int, Tuple<int, string, DateTime>> stragglerList = createStragglerList(subLists);
                    List<int> rowsToDelete = getRowsToDelete(stragglerList);
                    updateMissingList(subLists, rowsToDelete);
                    outputStragglerList(stragglerList);
                }
            }
            enableUI();
        }



        /************* FILE/FOLDER CHECKS ******************/

        private int getFileState()
        {
            if (string.IsNullOrEmpty(missingListPathField.Text))
            {
                return FILE_PATH_EMPTY;
            }
            else if (!File.Exists(missingListPathField.Text))
            {
                return FILE_NOT_FOUND;
            }
            else if (!correctFile())
            {
                return INCORRECT_FILE;
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
                    newSubList.chartInfo = loadList(worksheet, newSubList);
                    newSubList.totalCharts = worksheet.Cells[newSubList.endRow, 3].GetValue<int>();

                    subLists.Add(newSubList);
                }
                // need to close worksheet ??
            }
            return subLists;
        }

        private string searchSubLists(List<SubList> subLists, int chartNum)
        {
            foreach (SubList subList in subLists)
            {
                if (subList.chartInfo.ContainsKey(chartNum))
                {
                    return subList.name;
                }
            }
            return null;
        }

        private SortedDictionary<int, Tuple<int, string, DateTime>> createStragglerList(List<SubList> subLists)
        {
            SortedDictionary<int, Tuple<int, string, DateTime>> stragglerList = new SortedDictionary<int, Tuple<int, string, DateTime>>();

            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int chartNum = Int32.Parse(fileName);

                foreach (SubList subList in subLists)
                {
                    if (subList.chartInfo.ContainsKey(chartNum))
                    {
                        stragglerList.Add(chartNum, subList.chartInfo[chartNum]);
                        subList.totalCharts -= 1;
                    }
                }
            }

            return stragglerList;
        }

        private Dictionary<int, Tuple<int, string, DateTime>> loadList(ExcelWorksheet worksheet, SubList subList)
        {
            Dictionary<int, Tuple<int, string, DateTime>> list = new Dictionary<int, Tuple<int, string, DateTime>>();

            for (int row = subList.startRow; row < subList.endRow; row++)
            {
                int chartNum = worksheet.Cells[row, 1].GetValue<int>();
                string patientName = worksheet.Cells[row, 2].GetValue<string>();
                DateTime date = worksheet.Cells[row, 3].GetValue<DateTime>();

                Tuple<int, string, DateTime> rowNameDate = new Tuple<int, string, DateTime>(row, patientName, date);
                list.Add(chartNum, rowNameDate);

                // maybe add exception handling for possible blank cells or cells with bad chart numbers ??
            }
            return list;
        }

        private void updateMissingList(List<SubList> subLists, List<int> rowsToDelete)
        {
            FileInfo path = new FileInfo(missingListPathField.Text);
            using (ExcelPackage package = new ExcelPackage(path))
            using (ExcelWorksheet worksheet = package.Workbook.Worksheets[1])
            {
                if (deleteRowsCheckBox.Checked)
                {
                    foreach (SubList subList in subLists)
                    {
                        int rowWithTotal = subList.endRow;
                        worksheet.Cells[rowWithTotal, 3].Value = subList.totalCharts;
                    }

                    foreach (int row in rowsToDelete)
                    {
                        worksheet.DeleteRow(row);
                    }
                }
                else
                {
                    foreach (int row in rowsToDelete)
                    {
                        worksheet.Row(row).Style.Font.Bold = true;
                    }
                }

                // need to close worksheet ??

                try
                {
                    package.Save();
                }
                catch (InvalidOperationException)
                {
                    showErrorMessage(CANNOT_SAVE_MISSING_LIST);
                }
            }
        }

        private void outputStragglerList(SortedDictionary<int, Tuple<int, string, DateTime>> stragglerList)
        {
            using (DataTable stragglerListDataTable = new DataTable())
            {
                stragglerListDataTable.Columns.Add("Chart");
                stragglerListDataTable.Columns.Add("Patient Name");
                stragglerListDataTable.Columns.Add("DOS");

                foreach (int chartNum in stragglerList.Keys)
                {
                    string patientName = stragglerList[chartNum].Item2;
                    string date = stragglerList[chartNum].Item3.ToShortDateString();

                    stragglerListDataTable.Rows.Add(chartNum, patientName, date);
                }
                stragglerListOutput.DataSource = stragglerListDataTable;
                stragglersTotalLabel.Text = "Total: " + stragglerList.Count.ToString();
            }
        }

        private List<int> getRowsToDelete(SortedDictionary<int, Tuple<int, string, DateTime>> stragglerList)
        {
            List<int> rowsToDelete = new List<int>();
            
            foreach (int stragglerKey in stragglerList.Keys)
            {
                rowsToDelete.Add(stragglerList[stragglerKey].Item1);
            }

            rowsToDelete.Sort();
            rowsToDelete.Reverse();

            return rowsToDelete;
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

        private void renameFile(string listContainingChart, string fileToRename)
        {
            // rules on how to rename file when found in a particular list
            switch(listContainingChart)
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

        

        /************* UTILITY FUNCTIONS ******************/

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
            createStragglerListBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            renameFilesBTN.Enabled = true;
            createStragglerListBTN.Enabled = true;
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
                case CREATE_LIST_WARNING:
                    string createListMessage = "This program will remove all of the charts from the missing list.\n\n" +
                "Make sure you copy the list once it is run because you will not be able to run it again.\n\n" +
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
                case FILE_PATH_EMPTY:
                    MessageBox.Show("Please select a file.");
                    return;
                case FILE_NOT_FOUND:
                    MessageBox.Show("The file you selected could not be found.");
                    return;
                case INCORRECT_FILE:
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
                case CANNOT_SAVE_MISSING_LIST:
                    MessageBox.Show("It looks like the missing list is open somewhere else.\n\n" +
                        "The straggler list will be created, but the missing list will not be modified.\n\n" +
                        "Close the missing list and try again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }



        /************* COPY OPERATION FUNCTIONS ******************/
        private void stragglerListOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && stragglerListOutput.Rows.Count != 0)
            {
                this.rightClickMenu.Show(this.stragglerListOutput, e.Location);
                this.rightClickMenu.Show(Cursor.Position);
            }
        }

        private void copyRightClickMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.stragglerListOutput.GetClipboardContent());
        }
    }
}
