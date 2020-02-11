using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            enableUI();
        }



        /************* UTILITY FUNCTIONS ******************/

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
