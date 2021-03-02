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

/*
 * Thanks to Olivier Jacot-Descombes for his suggestion to load the spreadsheet lists into HashSets for faster lookups!! (https://stackoverflow.com/questions/58674525/is-it-better-practice-to-work-with-information-within-a-file-folder-or-to-load?answertab=votes#tab-top)
 */

namespace MB_Utilities.controls.chester
{
    public partial class ProcessStragglers : UserControl
    {
        private const int RENAME_WARNING = 0;

        public ProcessStragglers()
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

        private void renameFilesBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            if (showWarning(RENAME_WARNING) == DialogResult.Yes)
            {
                // check state of missing list and files in folder
                List<State> missingListChecks = new List<State>() { State.FILE_PATH_EMPTY,
                                                                    State.FILE_PATH_NOT_FOUND};
                List<State> folderChecks = new List<State>() { State.FOLDER_PATH_EMPTY,
                                                               State.FOLDER_PATH_NOT_FOUND,
                                                               State.FOLDER_HAS_NO_PDFS,
                                                               State.BAD_FILE_NAME};
                State missingListState = StateChecks.performStateChecks(missingListChecks, 
                                                                        missingListPathField.Text);
                State folderState = StateChecks.performStateChecks(folderChecks,
                                                                   folderPathField.Text);
                if (missingListState != State.READY)
                {
                    StateChecks.showErrorMessage(missingListState, missingListPathField.Text);
                }
                else if (folderState != State.READY)
                {
                    StateChecks.showErrorMessage(folderState, folderPathField.Text);
                }
                else
                {
                    List<string> subListIDs = new List<string>() { "ME", "NN", "PM", "SC", "SG", "WR", "TD" };
                    List<SubList> subLists = MissingList.createSubLists(subListIDs, missingListPathField.Text);
                    
                    foreach (string file in Directory.EnumerateFiles(folderPathField.Text, "*.pdf"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);

                        string listContainingChart = searchSubLists(subLists, fileName);
                        renameFile(listContainingChart, file);
                    }

                    MessageBox.Show("Rename complete!!");
                }
            }
            enableUI();
        }

        private string searchSubLists(List<SubList> subLists, string fileName)
        {
            foreach (SubList subList in subLists)
            {
                if (subList.patientInfo.ContainsKey(fileName))
                {
                    return subList.name;
                }
            }
            return null;
        }

        /************* UTILITY FUNCTIONS ******************/


        private void renameFile(string listContainingChart, string fileToRename)
        {
            // rules on how to rename file when found in a particular list
            if (listContainingChart == null)
            {
                appendToFileName("not on list", fileToRename);

            }
            else if (listContainingChart == "TD" || listContainingChart == "NN")
            {
                return;
            }
            else if (listContainingChart == "SC")
            {
                appendToFileName("SC save", fileToRename);
            }
            else
            {
                appendToFileName(listContainingChart, fileToRename);
            }
        }

        private void appendToFileName(string suffix, string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string newFileName = string.Concat(fileName, " ", suffix, extension);
            string newFilePath = Path.Combine(directory, newFileName);
            File.Move(filePath, newFilePath);
        }


        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseFileFolderBTN.Enabled = false;
            renameFilesBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseFileFolderBTN.Enabled = true;
            renameFilesBTN.Enabled = true;
        }

        private DialogResult showWarning(int warning)
        {
            string title = "Warning";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            switch(warning)
            {
                case RENAME_WARNING:
                    string renameMessage = "Before running this program make sure you have separated all of the straggler files into your selected folder.\n\n" +
                "If the charts for your day are also in this folder it will rename all of them, which you don't want.\n\n" +
                "Are you sure you are ready to continue?";
                    return MessageBox.Show(renameMessage, title, buttons);
                default:
                    return DialogResult.No;
            }
        }
    }
}
