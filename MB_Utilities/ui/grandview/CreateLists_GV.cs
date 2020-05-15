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

namespace MB_Utilities.ui.grandview
{
    public partial class CreateLists_GV : UserControl
    {
        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;
        private const int MISSING_LIST_INCORRECT = 3;
        private const int MISSING_LIST_CANNOT_SAVE = 4;

        // state of log file
        private const int LOG_FILE_READY = 5;
        private const int LOG_FILE_PATH_EMPTY = 6;
        private const int LOG_FILE_NOT_FOUND = 7;

        // state of save folder
        private const int FOLDER_READY = 8;
        private const int FOLDER_PATH_EMPTY = 9;
        private const int FOLDER_NOT_FOUND = 10;


        public CreateLists_GV()
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

        private void saveFileToBTN_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = saveFileToPathField.Text;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFileToPathField.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void createListsBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            int missingListState = getMissingListState();
            int logFileState = getLogFileState();
            int folderState = getFolderState();
            if (missingListState != MISSING_LIST_READY)
            {
                showErrorMessage(missingListState);
            }
            else if (logFileState != LOG_FILE_READY)
            {
                showErrorMessage(logFileState);
            }
            else if (folderState != FOLDER_READY)
            {
                showErrorMessage(folderState);
            }
            else
            {
                List<Dictionary<string, string>> logFile = loadLogFile();
                List<Dictionary<string, string>> missingList = createMissingList(logFile);
                List<Dictionary<string, string>> voidedList = createVoidedList(logFile);

                List<string> subListIDs = new List<string>() { "ME", "PM", "TD" };
                List<SubList> subLists = createSubLists(subListIDs);
                List<Dictionary<string, string>> stragglerList = createStragglerList(subLists);

                bool docCreated = docCreated = createLists(missingList, voidedList, stragglerList);

                if (docCreated)
                {
                    // if lists are created and checkbox is marked, we can delete the rows from the missing list
                    if (deleteRowsCheckBox.Checked)
                    {
                        List<int> rowsToDelete = getRowsToDelete(stragglerList);
                        updateMissingList(subLists, rowsToDelete);
                    }

                    // output number on each list
                    missingTotalLabel.Text = "Missing Total: " + missingList.Count;
                    voidedTotalLabel.Text = "Voided Total: " + voidedList.Count;
                    stragglersTotalLabel.Text = "Straggler Total: " + stragglerList.Count;

                    MessageBox.Show("Lists created!!");
                }
                else
                {
                    MessageBox.Show("An error occurred so the lists were not created and the missing list was not updated. Try again.");
                }
            }

            enableUI();
        }


        /************* MISSING AND VOIDED LIST FUNCTIONS ******************/

        private List<Dictionary<string, string>> loadLogFile()
        {
            List<Dictionary<string, string>> logFile = new List<Dictionary<string, string>>();

            string[] lines = File.ReadAllLines(logFilePathField.Text).Where(line => line != "").ToArray();
            foreach (string line in lines)
            {
                string[] chartInfo = line.Split(',');
                string date = chartInfo[0];
                string chartNum = chartInfo[1];
                string lastName = chartInfo[2];
                string firstName = chartInfo[3];
                string logCode = chartInfo[4];
                Dictionary<string, string> patientInfo = new Dictionary<string, string>()
                {
                    {"date", date },
                    {"chartNum", chartNum },
                    {"lastName", lastName },
                    {"firstName", firstName },
                    {"logCode", logCode },
                    {"missing", " " }
                };
                logFile.Add(patientInfo);
            }

            // log file is not necessarily in number order so it must be sorted by chart num
            logFile = logFile.OrderBy(x => x["chartNum"]).ToList<Dictionary<string, string>>();
            return logFile;
        }

        private List<Dictionary<string, string>> createMissingList(List<Dictionary<string, string>> logFile)
        {
            List<Dictionary<string, string>> missingList = new List<Dictionary<string, string>>();
            foreach (var patientInfo in logFile)
            {
                // check if this chart is TD, must be modified before running
                if (patientInfo["logCode"] == "TD")
                {
                    missingList.Add(patientInfo);
                }
            }
            return missingList;
        }

        private List<Dictionary<string, string>> createVoidedList(List<Dictionary<string, string>> logFile)
        {
            List<Dictionary<string, string>> voidedList = new List<Dictionary<string, string>>();
            foreach (var patientInfo in logFile)
            {
                // log codes other than RG or TD
                if (patientInfo["logCode"] != "RG" && patientInfo["logCode"] != "TD")
                {
                    voidedList.Add(patientInfo);
                }
            }
            return voidedList;
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
                    newSubList.patientInfo = loadPatientInfo(worksheet, newSubList);
                    newSubList.numPatients = newSubList.patientInfo.Count;

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

        private Dictionary<string, Dictionary<string, string>> loadPatientInfo(ExcelWorksheet worksheet, SubList subList)
        {
            // Only loads patient to sublist if the first cell of the row is underlined.
            // This how to mark which stragglers we want to send

            Dictionary<string, Dictionary<string, string>> patients = new Dictionary<string, Dictionary<string, string>>();

            int startRow = subList.startRow;
            int endRow = subList.endRow;
            for (int row = startRow; row < endRow; ++row)
            {
                if (worksheet.Cells[row, 1].Style.Font.UnderLine)
                {
                    string chartNum = worksheet.Cells[row, 1].GetValue<string>();
                    string patientName = worksheet.Cells[row, 2].GetValue<string>();
                    string date = worksheet.Cells[row, 3].GetValue<DateTime>().ToShortDateString();

                    Dictionary<string, string> patientInfo = new Dictionary<string, string>()
                    {
                        { "chartNum", chartNum },
                        { "rowNum", row.ToString()},
                        { "patientName", patientName},
                        { "date", date}
                    };
                    patients.Add(chartNum, patientInfo);
                }
            }
            return patients;
        }

        private List<Dictionary<string, string>> createStragglerList(List<SubList> subLists)
        {
            List<Dictionary<string, string>> stragglerList = new List<Dictionary<string, string>>();

            foreach (SubList subList in subLists)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> patient in subList.patientInfo)
                {
                    stragglerList.Add(patient.Value);
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
                int rowNumber = Int32.Parse(chartInfo["rowNum"]);
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
                foreach (SubList subList in subLists)
                {
                    // get total and subtract the number of patients in each sublist since each sublist only contains the patients we want to delete
                    int rowWithTotal = subList.endRow;
                    int total = worksheet.Cells[rowWithTotal, 3].GetValue<int>();
                    worksheet.Cells[rowWithTotal, 3].Value = total - subList.numPatients;
                }

                foreach (int row in rowsToDelete)
                {
                    worksheet.DeleteRow(row);
                }

                try
                {
                    package.Save();
                }
                catch (InvalidOperationException)
                {
                    showErrorMessage(MISSING_LIST_CANNOT_SAVE);
                }
            }
        }

        private bool createLists(List<Dictionary<string, string>> missingList, List<Dictionary<string, string>> voidedList, List<Dictionary<string, string>> stragglerList)
        {
            try
            {
                object missing = System.Reflection.Missing.Value;
                Word.Application application = new Word.Application();
                Word.Document document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                string dateOfService = " ";

                int missingRows = missingList.Count;
                int missingCols = 2;
                if (missingRows > 0)
                {
                    // get date of service
                    DateTime date = DateTime.ParseExact(missingList[0]["date"], "yyyyMMdd", null);
                    dateOfService = date.ToString(@"MM-dd-yy");

                    // create table
                    Word.Paragraph missingTitle = document.Content.Paragraphs.Add(ref missing);
                    missingTitle.Range.Text = "Missing " + dateOfService;
                    missingTitle.Range.Font.Name = "calibri";
                    missingTitle.Range.Font.Size = 16;
                    missingTitle.Range.Font.Bold = 1;
                    missingTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    missingTitle.Range.InsertParagraphAfter();

                    Word.Table missingTable = document.Tables.Add(missingTitle.Range, missingRows, missingCols);

                    int row = 1;
                    foreach (var patientInfo in missingList)
                    {
                        // add text
                        missingTable.Cell(row, 1).Range.Text = patientInfo["chartNum"];
                        missingTable.Cell(row, 2).Range.Text = patientInfo["lastName"] + ", " + patientInfo["firstName"];

                        // format table
                        missingTable.Rows[row].Range.Font.Bold = 0;
                        missingTable.Rows[row].Range.Font.Size = 12;
                        missingTable.Rows[row].Range.Font.Name = "calibri";
                        missingTable.Rows[row].SetHeight(17.0f, Word.WdRowHeightRule.wdRowHeightExactly);
                        for (int i = 1; i <= missingCols; ++i)
                        {
                            missingTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }

                        row++;
                    }
                    missingTable.Columns.AutoFit();
                }

                int voidedRows = voidedList.Count;
                int voidedCols = 3;
                if (voidedRows > 0)
                {
                    // get date of service again in case missing list was empty
                    DateTime date = DateTime.ParseExact(voidedList[0]["date"], "yyyyMMdd", null);
                    dateOfService = date.ToString(@"MM-dd-yy");

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
                    foreach (var patientInfo in voidedList)
                    {
                        // add text
                        voidedTable.Cell(row, 1).Range.Text = patientInfo["chartNum"];
                        voidedTable.Cell(row, 2).Range.Text = patientInfo["lastName"] + ", " + patientInfo["firstName"];
                        voidedTable.Cell(row, 3).Range.Text = patientInfo["logCode"];

                        // format table
                        voidedTable.Rows[row].Range.Font.Bold = 0;
                        voidedTable.Rows[row].Range.Font.Size = 12;
                        voidedTable.Rows[row].Range.Font.Name = "calibri";
                        voidedTable.Rows[row].SetHeight(17.0f, Word.WdRowHeightRule.wdRowHeightExactly);
                        for (int i = 1; i <= voidedCols; ++i)
                        {
                            voidedTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }
                        voidedTable.Cell(row, 3).Range.Font.Bold = 1;

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
                        stragglerTable.Rows[row].SetHeight(17.0f, Word.WdRowHeightRule.wdRowHeightExactly);
                        for (int i = 1; i <= stragglerCols; ++i)
                        {
                            stragglerTable.Cell(row, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        }

                        row++;
                    }
                    stragglerTable.Columns.AutoFit();
                }

                object fileName = saveFileToPathField.Text + "\\file_lists.docx";
                int fileNum = 1;
                while (File.Exists(fileName.ToString()))
                {
                    fileName = saveFileToPathField.Text + "\\file_lists_" + fileNum.ToString() + ".docx";
                    fileNum++;
                }
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

        private int getLogFileState()
        {
            if (string.IsNullOrEmpty(logFilePathField.Text))
            {
                return LOG_FILE_PATH_EMPTY;
            }
            else if (!File.Exists(logFilePathField.Text))
            {
                return LOG_FILE_NOT_FOUND;
            }
            return LOG_FILE_READY;
        }

        private int getFolderState()
        {
            if (string.IsNullOrEmpty(saveFileToPathField.Text))
            {
                return FOLDER_PATH_EMPTY;
            }
            else if (!Directory.Exists(saveFileToPathField.Text))
            {
                return FOLDER_NOT_FOUND;
            }
            return FOLDER_READY;
        }

        private bool correctMissingList()
        {
            FileInfo missingListPath = new FileInfo(missingListPathField.Text);
            using (ExcelPackage packageMissing = new ExcelPackage(missingListPath))
            using (ExcelWorksheet worksheetMissing = packageMissing.Workbook.Worksheets[1])
            {
                string title = worksheetMissing.Cells[1, 1].GetValue<string>();
                if (title == "GV Unbilled Report")
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
            saveFileToBTN.Enabled = false;
            createListsBTN.Enabled = false;
            deleteRowsCheckBox.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseLogFileBTN.Enabled = true;
            saveFileToBTN.Enabled = true;
            createListsBTN.Enabled = true;
            deleteRowsCheckBox.Enabled = true;
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case MISSING_LIST_PATH_EMPTY:
                    MessageBox.Show("Please select the GV missing list");
                    return;
                case MISSING_LIST_NOT_FOUND:
                    MessageBox.Show("The GV missing list could not be found.");
                    return;
                case MISSING_LIST_INCORRECT:
                    MessageBox.Show("The missing list you chose is not the GV missing list.");
                    return;
                case LOG_FILE_PATH_EMPTY:
                    MessageBox.Show("Please select a GV log file.");
                    return;
                case LOG_FILE_NOT_FOUND:
                    MessageBox.Show("The log file you selected could not be found.");
                    return;
                case FOLDER_PATH_EMPTY:
                    MessageBox.Show("Please select a folder.");
                    return;
                case FOLDER_NOT_FOUND:
                    MessageBox.Show("The folder you selected could not be found.");
                    return;
                case MISSING_LIST_CANNOT_SAVE:
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
