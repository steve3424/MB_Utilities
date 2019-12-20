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
    public partial class LoadDays_GV : UserControl
    {
        // state of file
        private const int FILE_READY = 0;
        private const int FILE_PATH_EMPTY = 1;
        private const int FILE_NOT_FOUND = 2;

        // state of save folder
        private const int FOLDER_READY = 3;
        private const int FOLDER_PATH_EMPTY = 4;
        private const int FOLDER_NOT_FOUND = 5;

        public LoadDays_GV()
        {
            InitializeComponent();
        }

        private void chooseCodingLogFileBTN_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Text File (.txt)|*.txt" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    codingLogFilePathField.Text = openFileDialog.FileName;
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

            int fileState = getFileState();
            int folderState = getFolderState();
            if (fileState != FILE_READY)
            {
                showErrorMessage(fileState);
            }
            else if (folderState != FOLDER_READY)
            {
                showErrorMessage(folderState);
            }
            else
            {
                string[] codingLog = File.ReadAllLines(codingLogFilePathField.Text);
                List<string> missing = loadList(codingLog, "NNY");
                List<string> voided = loadList(codingLog, "NYN");
                List<string> stragglers = loadList(codingLog, "NNN");

                outputFile(missing, voided, stragglers);
                MessageBox.Show("Done!!");
            }

            enableUI();
        }

        private List<string> loadList(string[] codingLog, string code)
        {
            List<string> patients = new List<string>();
            foreach (string line in codingLog)
            {
                if (codeMatches(line, code))
                {
                    string lastName = getLastName(line);
                    string firstName = getFirstName(line);
                    patients.Add(lastName + ", " + firstName);
                }
            }
            return patients;
        }

        private void outputFile(List<string> missing, List<string> voided, List<string> stragglers)
        {
            using (StreamWriter sw = File.CreateText(saveFileToPathField.Text + "\\helper_file.txt"))
            {
                sw.WriteLine("**** MISSING ****");
                foreach (string name in missing)
                {
                    sw.WriteLine(name);
                }

                sw.WriteLine("\n**** VOIDED ****");
                foreach (string name in voided)
                {
                    sw.WriteLine(name);
                }

                sw.WriteLine("\n**** STRAGGLERS ****");
                foreach (string name in stragglers)
                {
                    sw.WriteLine(name);
                }
            }
        }

        private bool codeMatches(string line, string code)
        {
            string[] splitLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int index = splitLine.Length - 1;
            DateTime dateValue; // for checking if there is a date at the end of this line
            if (DateTime.TryParse(splitLine[splitLine.Length - 1], out dateValue))
            {
                index--;
            }
            string lineCode = splitLine[index];
            return lineCode == code;
        }

        private string getLastName(string line)
        {
            // assumes multi-part last name is only separated by 1 space

            string lastName = "";

            string[] splitLine = line.Split(' ');
            string dateAndLastName = splitLine[4];

            // add first part
            for (int i = 11; i < dateAndLastName.Length; i++)
            {
                lastName += dateAndLastName[i];
            }

            // check next sections of line for other parts of last name
            int index = 8;
            string currentString = splitLine[index];
            while (!string.IsNullOrWhiteSpace(currentString))
            {
                lastName += " " + currentString;
                index++;
                currentString = splitLine[index];
            }

            return lastName;
        }

        private string getFirstName(string line)
        {
            // assumes multi-part last and first names are only separated by 1 space
            string firstName = "";

            string[] splitLine = line.Split(' ');
            int index = 7; // start at section with last name

            // move past every section of last name
            while (!string.IsNullOrWhiteSpace(splitLine[index]))
            {
                index++;
            }

            // find first part of first name
            while (string.IsNullOrWhiteSpace(splitLine[index]))
            {
                index++;
            }
            firstName += splitLine[index];

            // get ladder parts of first name
            index++;
            while (!string.IsNullOrWhiteSpace(splitLine[index]))
            {
                firstName += " " + splitLine[index];
                index++;
            }
            return firstName;
        }

        private int getFileState()
        {
            if (string.IsNullOrEmpty(codingLogFilePathField.Text))
            {
                return FILE_PATH_EMPTY;
            }
            else if (!File.Exists(codingLogFilePathField.Text))
            {
                return FILE_NOT_FOUND;
            }
            return FILE_READY;
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
            chooseCodingLogFileBTN.Enabled = false;
            saveFileToBTN.Enabled = false;
            createListsBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseCodingLogFileBTN.Enabled = true;
            saveFileToBTN.Enabled = true;
            createListsBTN.Enabled = true;
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
