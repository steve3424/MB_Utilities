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


        List<String> log_codes_regular = new List<string>() { "RG" };
        List<String> log_codes_missing = new List<string>() { "TD", "NN" };
        List<String> log_codes_voided = new List<string>() { "LW", "PD", "NS" };

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

            List<State> missingListChecks = new List<State>() { State.FILE_PATH_EMPTY,
                                                                State.FILE_PATH_NOT_FOUND};
            List<State> logFileChecks = new List<State>() { State.FILE_PATH_EMPTY,
                                                                State.FILE_PATH_NOT_FOUND};
            List<State> folderChecks = new List<State>() { State.FOLDER_PATH_EMPTY,
                                                               State.FOLDER_PATH_NOT_FOUND,
                                                               State.FOLDER_HAS_NO_PDFS,
                                                               State.BAD_FILE_NAME_SKIP_BAD};
            State missingListState = StateChecks.performStateChecks(missingListChecks,
                                                                    missingListPathField.Text);
            State logFileState = StateChecks.performStateChecks(logFileChecks,
                                                                logFilePathField.Text);
            State folderState = StateChecks.performStateChecks(folderChecks,
                                                               folderPathField.Text);
            if (missingListState != State.READY)
            {
                StateChecks.showErrorMessage(missingListState, missingListPathField.Text);
            }
            else if (logFileState != State.READY)
            {
                StateChecks.showErrorMessage(logFileState, logFilePathField.Text);
            }
            else if (folderState != State.READY)
            {
                StateChecks.showErrorMessage(folderState, folderPathField.Text);
            }
            else
            {
                List<Dictionary<string, string>> logFile = loadLogFile();
                processNNCharts(ref logFile);

                HashSet<string> fileNames = loadFileNames();
                List<Dictionary<string, string>> missingList = createMissingList(logFile, fileNames);
                List<Dictionary<string, string>> voidedList = createVoidedList(logFile, fileNames);

                // create straggler list
                List<string> subListIDs = new List<string>() { "ME", "NN", "PM", "SG", "TD", "WR" };
                List<SubList> subLists = MissingList.createSubLists(subListIDs, missingListPathField.Text);
                List<Dictionary<string, string>> stragglerList = createStragglerList(subLists);

                // create word doc of lists
                bool docCreated = createWordDoc(missingList, voidedList, stragglerList);

                if (docCreated)
                {
                    // file to be used in UiPath
                    if (missingList.Count > 0 || stragglerList.Count > 0)
                    {
                        createUIPathList(missingList, stragglerList);
                    }

                    // output number on each list
                    int missingTotal = missingList.Count;
                    missingTotalLabel.Text = "Missing Total: " + missingTotal;
                    int voidedChartsNotMissing = 0;
                    foreach (var patientInfo in voidedList)
                    {
                        if (patientInfo["missing"] != "-")
                        {
                            voidedChartsNotMissing++;
                        }
                    }
                    voidedTotalLabel.Text = "Voided Total: " + voidedList.Count + " (" + voidedChartsNotMissing + ")";
                    stragglersTotalLabel.Text = "Straggler Total: " + stragglerList.Count;

                    MessageBox.Show("Lists created!!");
                }
                else
                {
                    MessageBox.Show("An error occurred so the lists were not created. Try again.");
                }
            }

            enableUI();
        }


        /************* MISSING AND VOIDED LIST FUNCTIONS ******************/

        private void processNNCharts(ref List<Dictionary<string, string>> logFile) 
        {
            // get all "- BAD" charts in a list
            List<string> NNCharts = new List<string>();
            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.Contains("BAD"))
                {
                    NNCharts.Add(fileName);
                }
            }

            // move files into don't needs folder
            foreach (string file in NNCharts)
            {
                string source = folderPathField.Text + "\\" + file + ".pdf";
                string dest = "S:\\MB Charts\\Chester County\\TempNewProcess\\possible signatures - DO NOT DELETE\\" + file + ".pdf";

                try 
                {
                    File.Move(source, dest);
                }
                catch (Exception e)
                {
                    MessageBox.Show(file + "\n\n" + e.ToString());
                }
                
            }

            // sanitize chart names
            for (int i = 0; i < NNCharts.Count; ++i)
            {
                string sanitized_name = NNCharts[i];

                do
                {
                    try
                    {
                        Int32.Parse(sanitized_name);
                        break;
                    }
                    catch (Exception)
                    {
                        sanitized_name = sanitized_name.Remove(sanitized_name.Length - 1);
                    }
                    
                } while (true);

                sanitized_name = sanitized_name.Trim(' ');
                
                // find this chart in logfile struct and change log code to NN
                foreach(var patientInfo in logFile)
                {
                    if (patientInfo["chartNum"] == sanitized_name) {
                        patientInfo["logCode"] = "NN";
                    }
                }
            }
        }

        private HashSet<string> loadFileNames()
        {
            HashSet<string> fileNames = new HashSet<string>();
            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                fileNames.Add(fileName);
            }
            return fileNames;
        }

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

        private List<Dictionary<string, string>> createMissingList(List<Dictionary<string, string>> logFile, HashSet<string> fileNames)
        {
            List<Dictionary<string, string>> missingList = new List<Dictionary<string, string>>();
            foreach (var patientInfo in logFile)
            {
                bool chart_is_missing = !fileNames.Contains(patientInfo["chartNum"]);
                bool chart_has_log_code = log_codes_missing.Contains(patientInfo["logCode"]) || log_codes_regular.Contains(patientInfo["logCode"]);
                if (chart_is_missing && chart_has_log_code)
                {
                    missingList.Add(patientInfo);
                }
            }
            return missingList;
        }

        private List<Dictionary<string, string>> createVoidedList(List<Dictionary<string, string>> logFile, HashSet<string> fileNames)
        {
            List<Dictionary<string, string>> voidedList = new List<Dictionary<string, string>>();
            foreach (var patientInfo in logFile)
            {
                if (log_codes_voided.Contains(patientInfo["logCode"]))
                {
                    if (fileNames.Contains(patientInfo["chartNum"]))
                    {
                        voidedList.Add(patientInfo);
                    }
                    else {
                        patientInfo["missing"] = "-";
                        voidedList.Add(patientInfo);
                    }
                }
            }
            return voidedList;
        }

        private void createUIPathList(List<Dictionary<string, string>> missingList, List<Dictionary<string, string>> stragglerList)
        {
            string savePath = folderPathField.Text + "\\ChartsToModify.txt";
            StreamWriter sw = new StreamWriter(savePath, false);

            foreach (var patientInfo in missingList)
            {
                if (patientInfo["logCode"] == "NN")
                {
                    sw.WriteLine(patientInfo["chartNum"] + "," + "NN - CHRT RECVD FROM FAC CHRT INCOM");
                }
                else {
                    sw.WriteLine(patientInfo["chartNum"] + "," + "TD - MISSING COMPLETE CHART");
                }
                
            }

            foreach (var patientInfo in stragglerList)
            {
                sw.WriteLine(patientInfo["chartNum"] + "," + "LS - LOCATED CHART SENT TO CODING");
            }

            sw.Flush();
            sw.Close();
        }


        private List<Dictionary<string, string>> createStragglerList(List<SubList> subLists)
        {
            List<Dictionary<string, string>> stragglerList = new List<Dictionary<string, string>>();

            foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                foreach (SubList subList in subLists)
                {
                    if (subList.patientInfo.ContainsKey(fileName))
                    {
                        stragglerList.Add(subList.patientInfo[fileName]);
                    }
                }
            }

            // sort by "date", then by "chartNum"
            List<Dictionary<string, string>> sortedStragglerList = stragglerList.OrderBy(x => x["chartNum"])
                                                                   .ThenBy(x => Convert.ToDateTime(x["date"]))
                                                                   .ToList<Dictionary<string, string>>();

            return sortedStragglerList;
        }

        private bool createWordDoc(List<Dictionary<string, string>> missingList, List<Dictionary<string, string>> voidedList, List<Dictionary<string, string>> stragglerList)
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
                        voidedTable.Cell(row, 1).Range.Text = patientInfo["missing"] + "  " + patientInfo["chartNum"];
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

                object fileName = folderPathField.Text + "\\accountability_list.docx";
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

        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseLogFileBTN.Enabled = false;
            chooseFileFolderBTN.Enabled = false;
            createListsBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseLogFileBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            createListsBTN.Enabled = true;
        }
    }
}
