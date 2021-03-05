using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.utils
{
    public enum State 
    {
        FOLDER_PATH_EMPTY,
        FOLDER_PATH_NOT_FOUND,
        FOLDER_HAS_NO_PDFS,

        FILE_PATH_EMPTY,
        FILE_PATH_NOT_FOUND,
        FILE_INCORRECT_CT,
        FILE_INCORRECT_GV,

        BAD_FILE_NAME,
        BAD_FILE_NAME_SKIP_BAD,
        BACKGROUND_WORKER_ERROR,

        CHECKBOX_NOT_CHECKED,

        READY
    }

    public class StateChecks
    {
        public static State performStateChecks(List<State> checksToDo, string path)
        {
            foreach (State stateCheck in checksToDo)
            {
                switch(stateCheck)
                {
                    case State.FOLDER_PATH_EMPTY:
                        {
                            if (String.IsNullOrEmpty(path))
                            {
                                return State.FOLDER_PATH_EMPTY;
                            }
                        }
                        break;
                    case State.FOLDER_PATH_NOT_FOUND:
                        {
                            if (!Directory.Exists(path))
                            {
                                return State.FOLDER_PATH_NOT_FOUND;
                            }
                        }
                        break;
                    case State.FOLDER_HAS_NO_PDFS:
                        {
                            if (!Directory.EnumerateFiles(path, "*.pdf").Any())
                            {
                                return State.FOLDER_HAS_NO_PDFS;
                            }
                        }
                        break;
                    case State.BAD_FILE_NAME:
                        {
                            if (!fileNamesAreGood(path))
                            {
                                return State.BAD_FILE_NAME;
                            }
                        }
                        break;
                    case State.BAD_FILE_NAME_SKIP_BAD:
                        {
                            if (!fileNamesAreGoodSkipBad(path))
                            {
                                return State.BAD_FILE_NAME_SKIP_BAD;
                            }
                        }
                        break;
                    case State.FILE_PATH_EMPTY:
                        {
                            if (String.IsNullOrEmpty(path))
                            {
                                return State.FILE_PATH_EMPTY;
                            }
                        }
                        break;
                    case State.FILE_PATH_NOT_FOUND:
                        {
                            if (!File.Exists(path))
                            {
                                return State.FILE_PATH_NOT_FOUND;
                            }
                        }
                        break;
                    case State.FILE_INCORRECT_CT:
                        {
                            if (!StateChecks.correctFileCT(path))
                            {
                                return State.FILE_INCORRECT_CT;
                            }
                        }
                        break;
                    case State.FILE_INCORRECT_GV:
                        {
                            if (!StateChecks.correctFileGV(path))
                            {
                                return State.FILE_INCORRECT_GV;
                            }
                        }
                        break;
                    case State.BACKGROUND_WORKER_ERROR:
                        {

                        }
                        break;
                }
            }

            return State.READY;
        }

        public static void showErrorMessage(State state, string path)
        {
            switch (state)
            {
                case State.FOLDER_PATH_EMPTY:
                    {
                        MessageBox.Show("Please select a folder");
                    } 
                    break;
                case State.FOLDER_PATH_NOT_FOUND:
                    {
                        MessageBox.Show("Could not find folder: " + path);
                    } 
                    break;
                case State.FOLDER_HAS_NO_PDFS:
                    {
                        MessageBox.Show("There were no pdf's in folder: " + path);
                    } 
                    break;
                case State.BAD_FILE_NAME:
                    {
                        MessageBox.Show("One or more files have a bad name in: " + path);
                    }
                    break;
                case State.BAD_FILE_NAME_SKIP_BAD:
                    {
                        MessageBox.Show("One or more files have a bad name in: " + path);
                    }
                    break;
                case State.FILE_PATH_EMPTY:
                    {
                        MessageBox.Show("Please select a file");
                    }
                    break;
                case State.FILE_PATH_NOT_FOUND:
                    {
                        MessageBox.Show("Could not find file: " + path);
                    }
                    break;
                case State.FILE_INCORRECT_CT:
                    {
                        MessageBox.Show("This doesn't look like the CT coding log file: " + path);
                    }
                    break;
                case State.FILE_INCORRECT_GV:
                    {
                        MessageBox.Show("This doesn't look like the GV coding log file: " + path);
                    }
                    break;
                case State.CHECKBOX_NOT_CHECKED:
                    {
                        MessageBox.Show("Please select a checkbox option!!!");
                    }
                    break;
                case State.BACKGROUND_WORKER_ERROR:
                    {
                        MessageBox.Show("There was an error when processing the files.");
                    }
                    break;
                default:
                    {
                        MessageBox.Show("An unspecified error occurred");
                    } 
                    break;
            }
        }

        private static bool fileNamesAreGood(string folderPath)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            FileInfo[] files = folder.GetFiles("*.pdf");
            foreach (FileInfo file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                try
                {
                    int chartNum = Int32.Parse(fileName);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is OverflowException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool fileNamesAreGoodSkipBad(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            FileInfo[] files = folder.GetFiles("*.pdf");
            foreach (FileInfo file in files)
            {
                // skip "- BAD" files
                // they will be NN later
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                if (fileName.Contains("BAD"))
                {
                    continue;
                }
                else
                {
                    try
                    {
                        int chartNum = Int32.Parse(fileName);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException || ex is OverflowException)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool correctFileCT(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            // get first 5 letters of file name
            string hospitalCode = "";
            if (fileName.Length >= 5)
            {
                for (int i = 0; i < 5; ++i)
                {
                    hospitalCode += fileName[i];
                }
            }

            if (hospitalCode == "1938L")
            {
                return true;
            }
            return false;
        }

        private static bool correctFileGV(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            // get first 5 letters of file name
            string hospitalCode = "";
            if (fileName.Length >= 5)
            {
                for (int i = 0; i < 5; ++i)
                {
                    hospitalCode += fileName[i];
                }
            }

            if (hospitalCode == "1950L")
            {
                return true;
            }
            return false;
        }
    }
}
