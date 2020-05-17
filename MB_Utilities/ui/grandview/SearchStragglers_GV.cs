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

        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;

        // state of save folder
        private const int FOLDER_READY = 4;
        private const int FOLDER_PATH_EMPTY = 5;
        private const int FOLDER_NOT_FOUND = 6;

        // check box states
        private const int CHECKBOX_READY = 7;
        private const int CHECKBOX_NOT_SELECTED = 8;


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

            int missingListState = getMissingListState();
            int folderState = getFolderState();
            int checkBoxState = getCheckBoxState();
            if (missingListState != MISSING_LIST_READY)
            {
                showErrorMessage(missingListState);
            }
            else if (folderState != FOLDER_READY)
            {
                showErrorMessage(folderState);
            }
            else if (checkBoxState != CHECKBOX_READY)
            {
                showErrorMessage(checkBoxState);
            }
            else
            {
                List<string> subListIDs = new List<string>();
                foreach (object checkedItem in checkedListBox1.CheckedItems)
                {
                    subListIDs.Add(checkedListBox1.GetItemText(checkedItem));
                }

                List<SubList> subLists = createSubLists(subListIDs);
                createLogFile(subLists);

                MessageBox.Show("Done!!");
            }

            enableUI();
        }

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
            while ((cellValue != header) && (eof_check != EOF))
            {
                ++start;
                cellValue = worksheet.Cells[start, 2].GetValue<string>();
                eof_check = worksheet.Cells[start, 16].GetValue<string>();
            }

            if (eof_check == EOF)
            {
                start = 0;
            }
            else
            {
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
                if (string.IsNullOrEmpty(chartNum) || string.IsNullOrWhiteSpace(chartNum))
                {
                    continue;
                }
                string patientName = worksheet.Cells[row, 4].GetValue<string>();
                string date = worksheet.Cells[row, 7].GetValue<DateTime>().ToShortDateString();

                Dictionary<string, string> patientInfo = new Dictionary<string, string>()
                {
                    { "rowNum", row.ToString()},
                    { "patientName", patientName},
                    { "date", date},
                    { "chartNum", chartNum }
                };
                patients.Add(chartNum, patientInfo);
            }
            return patients;
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

        private int getCheckBoxState()
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                return CHECKBOX_READY;
            }
            return CHECKBOX_NOT_SELECTED;
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
                case FOLDER_PATH_EMPTY:
                    MessageBox.Show("Please select a folder.");
                    return;
                case FOLDER_NOT_FOUND:
                    MessageBox.Show("The folder you selected could not be found.");
                    return;
                case CHECKBOX_NOT_SELECTED:
                    MessageBox.Show("Please select at least one list to search.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
