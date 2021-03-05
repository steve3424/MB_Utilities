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


namespace MB_Utilities.ui.grandview
{
    public partial class SearchStragglers_GV : UserControl
    {
        public SearchStragglers_GV()
        {
            InitializeComponent();

            // check TD at start
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                string itemName = checkedListBox1.Items[i].ToString();
                if (itemName == "TD")
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
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

            List<State> missingListChecks = new List<State>() { State.FILE_PATH_EMPTY,
                                                                State.FILE_PATH_NOT_FOUND};
            List<State> folderChecks = new List<State>() { State.FOLDER_PATH_EMPTY,
                                                           State.FOLDER_PATH_NOT_FOUND};

            State missingListState = StateChecks.performStateChecks(missingListChecks, 
                                                                    missingListPathField.Text);
            State folderState = StateChecks.performStateChecks(folderChecks, saveFileToPathField.Text);
            State checkBoxState = (checkedListBox1.CheckedItems.Count > 0) ? State.READY : 
                                                                             State.CHECKBOX_NOT_CHECKED;
            if (missingListState != State.READY)
            {
                StateChecks.showErrorMessage(missingListState, missingListPathField.Text);
            }
            else if (folderState != State.READY)
            {
                StateChecks.showErrorMessage(folderState, saveFileToPathField.Text);
            }
            else if (checkBoxState != State.READY)
            {
                StateChecks.showErrorMessage(checkBoxState, null);
            }
            else
            {
                List<string> subListIDs = new List<string>();
                foreach (object checkedItem in checkedListBox1.CheckedItems)
                {
                    subListIDs.Add(checkedListBox1.GetItemText(checkedItem));
                }

                List<SubList> subLists = MissingList.createSubLists(subListIDs, missingListPathField.Text);
                createLogFile(subLists);

                MessageBox.Show("Done!!");
            }

            enableUI();
        }


        /************* UTILITY FUNCTIONS ******************/
        private void createLogFile(List<SubList> subLists)
        {
            using (StreamWriter sw = File.CreateText(saveFileToPathField.Text + "\\gv_stragglers.txt"))
            {
                foreach (SubList subList in subLists)
                {
                    // put each dictionary entry into list, then sort by "rowNum"
                    List<Dictionary<string, string>> stragglerList = new List<Dictionary<string, string>>();
                    foreach (string chartNum in subList.patientInfo.Keys)
                    {
                        stragglerList.Add(subList.patientInfo[chartNum]);
                    }
                    List<Dictionary<string, string>> sortedStragglerList = stragglerList.OrderBy(x => Int32.Parse(x["rowNum"]))
                                                                   .ToList<Dictionary<string, string>>();

                    // write each entry to file 
                    foreach (var straggler in sortedStragglerList)
                    {
                        string date = straggler["date"];
                        string chartNum = straggler["chartNum"];
                        string name = straggler["patientName"];
                        string logCode = "RG";

                        string lineToWrite = string.Join(",", date, chartNum, name, logCode);
                        sw.WriteLine(lineToWrite);
                    }
                }
            }
        }

        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            saveFileToBTN.Enabled = false;
            checkedListBox1.Enabled = false;
            createListsBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            saveFileToBTN.Enabled = true;
            checkedListBox1.Enabled = true;
            createListsBTN.Enabled = true;
        }
    }
}
