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
using Word = Microsoft.Office.Interop.Word;

using OfficeOpenXml;
using MB_Utilities.utils;

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
                    // create missing and voided lists
                    HashSet<int> fileNames = loadFileNames();
                    Dictionary<int, Dictionary<string, string>> logFile = loadLogFile();
                    SortedDictionary<int, Dictionary<string, string>> missingList = createMissingList(logFile, fileNames);
                    SortedDictionary<int, Dictionary<string, string>> voidedList = createVoidedList(logFile, fileNames);

                    // create straggler list
                    List<string> subListIDs = new List<string>() { "ME", "PM", "TD" };
                    List<SubList> subLists = createSubLists(subListIDs);
                    List<Dictionary<string, string>> stragglerList = createStragglerList(subLists);

                    // create word doc of lists
                    bool docCreated = createLists(missingList, voidedList, stragglerList);
                    
                    if (docCreated)
                    {
                        List<int> rowsToDelete = getRowsToDelete(stragglerList);
                        updateMissingList(subLists, rowsToDelete);

                        createTextFile(missingList);

                        missingTotalLabel.Text = "Missing Total: " + missingList.Keys.Count;
                        voidedTotalLabel.Text = "Voided Total: " + voidedList.Keys.Count;
                        stragglersTotalLabel.Text = "Straggler Total: " + stragglerList.Count;

                        MessageBox.Show("Lists created!!");
                    }
                    else
                    {
                        MessageBox.Show("An error occurred so the lists were not created and the missing list was not updated. Try again.");
                    }
                }
            }

            enableUI();
        }

        private bool createLists(SortedDictionary<int, Dictionary<string, string>> missingList, SortedDictionary<int, Dictionary<string, string>> voidedList, List<Dictionary<string, string>> stragglerList)
        {
            // get date of service
            DateTime date = DateTime.ParseExact(missingList[missingList.Keys.First()]["date"], "yyyyMMdd", null);
            string dateOfService = date.ToString(@"MM-dd-yy");

            try
            {
                object missing = System.Reflection.Missing.Value;
                Word.Application application = new Word.Application();
                Word.Document document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                int missingRows = missingList.Keys.Count;
                int missingCols = 2;
                if (missingRows > 0)
                {
                    // create missing list table
                    Word.Paragraph missingTitle = document.Content.Paragraphs.Add(ref missing);
                    missingTitle.Range.Text = "Missing " + dateOfService;
                    missingTitle.Range.Font.Name = "calibri";
                    missingTitle.Range.Font.Size = 16;
                    missingTitle.Range.Font.Bold = 1;
                    missingTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    missingTitle.Range.InsertParagraphAfter();

                    Word.Table missingTable = document.Tables.Add(missingTitle.Range, missingRows, missingCols);

                    int row = 1;
                    foreach (int chartNum in missingList.Keys)
                    {
                        // add text
                        missingTable.Cell(row, 1).Range.Text = chartNum.ToString();
                        missingTable.Cell(row, 2).Range.Text = missingList[chartNum]["lastName"] + ", " + missingList[chartNum]["firstName"];

                        // format table
                        missingTable.Rows[row].Range.Font.Bold = 0;
                        missingTable.Rows[row].Range.Font.Size = 12;
                        missingTable.Rows[row].Range.Font.Name = "calibri";
                        for (int i = 1; i <= missingCols; ++i)
                        {
                            missingTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }

                        row++;
                    }
                    missingTable.Columns.AutoFit();
                }


                int voidedRows = voidedList.Keys.Count;
                int voidedCols = 4;
                if (voidedRows > 0)
                {
                    // create voided list table
                    Word.Paragraph voidedTitle = document.Content.Paragraphs.Add(ref missing);
                    voidedTitle.Range.Text = "\nVoided " + dateOfService;
                    voidedTitle.Range.Font.Name = "calibri";
                    voidedTitle.Range.Font.Size = 16;
                    voidedTitle.Range.Font.Bold = 1;
                    voidedTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    voidedTitle.Range.InsertParagraphAfter();

                    Word.Table voidedTable = document.Tables.Add(voidedTitle.Range, voidedRows, voidedCols);

                    int row = 1;
                    foreach (int chartNum in voidedList.Keys)
                    {
                        // add text
                        voidedTable.Cell(row, 1).Range.Text = voidedList[chartNum]["missing"];
                        voidedTable.Cell(row, 2).Range.Text = chartNum.ToString();
                        voidedTable.Cell(row, 3).Range.Text = voidedList[chartNum]["lastName"] + ", " + voidedList[chartNum]["firstName"];
                        voidedTable.Cell(row, 4).Range.Text = voidedList[chartNum]["logCode"];

                        // format table
                        voidedTable.Rows[row].Range.Font.Bold = 0;
                        voidedTable.Rows[row].Range.Font.Size = 12;
                        voidedTable.Rows[row].Range.Font.Name = "calibri";
                        for (int i = 1; i <= voidedCols; ++i)
                        {
                            voidedTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }
                        voidedTable.Cell(row, 4).Range.Font.Bold = 1;

                        row++;
                    }
                    voidedTable.Columns.AutoFit();
                }

                int stragglerRows = stragglerList.Count;
                int stragglerCols = 3;
                if (stragglerRows > 0)
                {
                    // create straggler list table
                    Word.Paragraph stragglerTitle = document.Content.Paragraphs.Add(ref missing);
                    stragglerTitle.Range.Text = "\nStragglers " + dateOfService;
                    stragglerTitle.Range.Font.Name = "calibri";
                    stragglerTitle.Range.Font.Size = 16;
                    stragglerTitle.Range.Font.Bold = 1;
                    stragglerTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    stragglerTitle.Range.InsertParagraphAfter();

                    Word.Table stragglerTable = document.Tables.Add(stragglerTitle.Range, stragglerRows, stragglerCols);

                    int row = 1;
                    foreach (var patient in stragglerList)
                    {
                        // add text
                        stragglerTable.Cell(row, 1).Range.Text = patient["chartNum"];
                        stragglerTable.Cell(row, 2).Range.Text = patient["patientName"];
                        stragglerTable.Cell(row, 3).Range.Text = patient["date"];

                        // format table
                        stragglerTable.Rows[row].Range.Font.Bold = 0;
                        stragglerTable.Rows[row].Range.Font.Size = 12;
                        stragglerTable.Rows[row].Range.Font.Name = "calibri";
                        for (int i = 1; i <= stragglerCols; ++i)
                        {
                            stragglerTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }

                        row++;
                    }
                    stragglerTable.Columns.AutoFit();
                }

                object fileName = folderPathField.Text + "\\file_lists.docx";
                document.SaveAs2(ref fileName);
                document.Close();
                application.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
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

        private void createTextFile(SortedDictionary<int, Dictionary<string, string>> missingList)
        {
            // creates text file of missing charts to use in UiPath

            string savePath = folderPathField.Text + "\\missing_list.txt";
            StreamWriter sw = new StreamWriter(savePath, false);

            foreach (int chartNum in missingList.Keys)
            {
                sw.WriteLine(chartNum);
            }
            sw.Flush();
            sw.Close();
        }


        /************* STRAGGLER LIST FUNCTIONS ******************/

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
                    newSubList.chartInfo = loadChartInfo(worksheet, newSubList);
                    newSubList.totalCharts = worksheet.Cells[newSubList.endRow, 3].GetValue<int>();

                    subLists.Add(newSubList);
                }
            }
            return subLists;
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

        private Dictionary<int, Dictionary<string, string>> loadChartInfo(ExcelWorksheet worksheet, SubList subList)
        {
            Dictionary<int, Dictionary<string, string>> chartInfo = new Dictionary<int, Dictionary<string, string>>();

            for (int row = subList.startRow; row < subList.endRow; row++)
            {
                int chartNum = worksheet.Cells[row, 1].GetValue<int>();
                string patientName = worksheet.Cells[row, 2].GetValue<string>();
                string date = worksheet.Cells[row, 3].GetValue<DateTime>().ToShortDateString();

                Dictionary<string, string> chartRowNameDate = new Dictionary<string, string>()
                {
                    { "chartNum", chartNum.ToString() },
                    { "rowNumber", row.ToString()},
                    { "patientName", patientName},
                    { "date", date}
                };
                chartInfo.Add(chartNum, chartRowNameDate);

                // maybe add exception handling for possible blank cells or cells with bad chart numbers ??
            }
            return chartInfo;
        }

        private List<Dictionary<string, string>> createStragglerList(List<SubList> subLists)
        {
            List<Dictionary<string, string>> stragglerList = new List<Dictionary<string, string>>();

            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int chartNum = Int32.Parse(fileName);

                foreach (SubList subList in subLists)
                {
                    if (subList.chartInfo.ContainsKey(chartNum))
                    {
                        stragglerList.Add(subList.chartInfo[chartNum]);
                        subList.totalCharts -= 1;
                    }
                }
            }

            // sort by "date", then by "chartNum"
            List<Dictionary<string, string>> sortedStragglerList = stragglerList.OrderBy(x => Convert.ToDateTime(x["date"]))
                                                                   .ThenBy(x => x["chartNum"])
                                                                   .ToList<Dictionary<string, string>>();

            return sortedStragglerList;
        }

        private List<int> getRowsToDelete(List<Dictionary<string, string>> stragglerList)
        {
            List<int> rowsToDelete = new List<int>();

            foreach (var chartInfo in stragglerList)
            {
                int rowNumber = Int32.Parse(chartInfo["rowNumber"]);
                rowsToDelete.Add(rowNumber);
            }

            rowsToDelete.Sort();
            rowsToDelete.Reverse();

            return rowsToDelete;
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
                        "Your lists have been created, but the missing list was not updated.\n\n" +
                        "Either update the missing list manually or close it and run the program again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
