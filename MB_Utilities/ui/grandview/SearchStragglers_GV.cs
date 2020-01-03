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
    public partial class SearchStragglers_GV : UserControl
    {
        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;
        private const int MISSING_LIST_INCORRECT = 3;

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
            for (int i=0; i < checkedListBox1.Items.Count; ++i)
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
                MessageBox.Show("Ready!!");
            }

            enableUI();
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
                case MISSING_LIST_INCORRECT:
                    MessageBox.Show("The missing list you chose is not the GV missing list.");
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
