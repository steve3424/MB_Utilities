﻿using System;
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

                List<string> subListIDs = new List<string>() { "ME", "PM", "SG", "TD", "WR" };
                List<SubList> subLists = MissingList.createSubLists(subListIDs, missingListPathField.Text);
                List<Dictionary<string, string>> stragglerList = createStragglerList(subLists);

                bool docCreated = docCreated = createLists(missingList, voidedList, stragglerList);

                if (docCreated)
                {
                    // missing list file to be used in UiPath
                    if (stragglerList.Count > 0)
                    {
                        createTextFile(stragglerList);
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


        private void createTextFile(List<Dictionary<string, string>> stragglerList)
        {
            // creates text file of missing charts to use in UiPath

            string savePath = saveFileToPathField.Text + "\\LS_ChartsToModify.txt";
            StreamWriter sw = new StreamWriter(savePath, false);

            foreach (var patientInfo in stragglerList)
            {
                sw.WriteLine(patientInfo["chartNum"] + "," + "LS - LOCATED CHART SENT TO CODING");
            }

            sw.Flush();
            sw.Close();
        }


        /************* CREATE LIST FUNCTIONS ******************/

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

        private List<Dictionary<string, string>> createStragglerList(List<SubList> subLists)
        {
            List<Dictionary<string, string>> stragglerList = new List<Dictionary<string, string>>();

            foreach (SubList subList in subLists)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> patient in subList.patientInfo)
                {
                    if (!string.IsNullOrEmpty(patient.Value["isUnderlined"]))
                    {
                        stragglerList.Add(patient.Value);
                    }
                }
            }

            // sort by "date", then by "chartNum"
            List<Dictionary<string, string>> sortedStragglerList = stragglerList.OrderBy(x => Convert.ToDateTime(x["date"]))
                                                                   .ThenBy(x => x["chartNum"])
                                                                   .ToList<Dictionary<string, string>>();
            return sortedStragglerList;
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

                object fileName = saveFileToPathField.Text + "\\accountability_list.docx";
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

        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseLogFileBTN.Enabled = false;
            saveFileToBTN.Enabled = false;
            createListsBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseLogFileBTN.Enabled = true;
            saveFileToBTN.Enabled = true;
            createListsBTN.Enabled = true;
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
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
