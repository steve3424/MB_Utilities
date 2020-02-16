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

namespace MB_Utilities.ui.grandview
{
    public partial class CreateLists_GV : UserControl
    {
        private const int DELETE_ROWS_WARNING = 0;

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
                int logFileState = getLogFileState();
                if (missingListState != MISSING_LIST_READY)
                {
                    showErrorMessage(missingListState);
                }
                else if (logFileState != LOG_FILE_READY)
                {
                    showErrorMessage(logFileState);
                }
                else
                {
                    List<Dictionary<string, string>> logFile = loadLogFile();
                    List<Dictionary<string, string>> missingList = createMissingList(logFile);
                    List<Dictionary<string, string>> voidedList = createVoidedList(logFile);
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
            createListsBTN.Enabled = false;
            deleteRowsCheckBox.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseLogFileBTN.Enabled = true;
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
    }
}
