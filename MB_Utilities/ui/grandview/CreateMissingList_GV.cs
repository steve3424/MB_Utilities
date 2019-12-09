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

namespace MB_Utilities.ui.grandview
{
    public partial class CreateMissingList_GV : UserControl
    {
        // state of file
        private const int FILE_READY = 0;
        private const int FILE_PATH_EMPTY = 1;
        private const int FILE_NOT_FOUND = 2;

        public CreateMissingList_GV()
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

        private void createMissingListBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            // check state of file and folder before executing
            int fileState = getFileState();
            if (fileState != FILE_READY)
            {
                showErrorMessage(fileState);
            }
            else
            {
                Dictionary<int, Dictionary<string, string>> logFile = loadLogFile();
                SortedDictionary<int, Dictionary<string, string>> missingList = createMissingList(logFile);
                SortedDictionary<int, Dictionary<string, string>> voidedList = createVoidedList(logFile);
                showMissingList(missingList);
                showVoidedList(voidedList);
            }
            enableUI();
        }



        /************* MAIN FUNCTIONS ******************/

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
                    {"logCode", logCode }
                };
                logFile.Add(chartNum, info);
            }
            return logFile;
        }

        private SortedDictionary<int, Dictionary<string, string>> createMissingList(Dictionary<int, Dictionary<string, string>> logFile)
        {
            SortedDictionary<int, Dictionary<string, string>> missingList = new SortedDictionary<int, Dictionary<string, string>>();
            foreach (int logNum in logFile.Keys)
            {
                // charts marked TD go on missing list
                if (logFile[logNum]["logCode"] == "TD")
                {
                    missingList.Add(logNum, logFile[logNum]);
                }
            }
            return missingList;
        }

        private SortedDictionary<int, Dictionary<string, string>> createVoidedList(Dictionary<int, Dictionary<string, string>> logFile)
        {
            SortedDictionary<int, Dictionary<string, string>> voidedList = new SortedDictionary<int, Dictionary<string, string>>();
            foreach (int logNum in logFile.Keys)
            {
                // log codes other than RG or TD
                if (!(logFile[logNum]["logCode"] == "RG") && !(logFile[logNum]["logCode"] == "TD"))
                {
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
                voidedListTable.Columns.Add("Chart #");
                voidedListTable.Columns.Add("Patient Name");
                voidedListTable.Columns.Add("Log Code");

                foreach (int chartNum in voidedList.Keys)
                {
                    string patientName = string.Concat(voidedList[chartNum]["lastName"], ", ", voidedList[chartNum]["firstName"]);
                    voidedListTable.Rows.Add(chartNum, patientName, voidedList[chartNum]["logCode"]);
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

        private void disableUI()
        {
            chooseLogFileBTN.Enabled = false;
            createMissingListBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseLogFileBTN.Enabled = true;
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
