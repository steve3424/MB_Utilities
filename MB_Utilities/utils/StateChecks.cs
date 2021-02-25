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
        FOLDER_EMPTY,
        FOLDER_READY,

        FILE_PATH_EMPTY,
        FILE_PATH_NOT_FOUND,
        FILE_INCORRECT,
        FILE_READY,

        BAD_FILE_NAME,
        BACKGROUND_WORKER_ERROR
    }

    public class StateChecks
    {
        public static State getFolderState(string folder_path)
        {
            if (String.IsNullOrEmpty(folder_path))
            {
                return State.FOLDER_PATH_EMPTY;
            }
            else if (!Directory.Exists(folder_path))
            {
                return State.FOLDER_PATH_NOT_FOUND;
            }
            else if (!Directory.EnumerateFiles(folder_path, "*.pdf").Any())
            {
                return State.FOLDER_EMPTY;
            }
            else
            {
                return State.FOLDER_READY;
            }
        }

        public static void showErrorMessage(State state, string path)
        {
            switch (state)
            {
                case State.FOLDER_PATH_EMPTY:
                    {
                        MessageBox.Show("Please select a folder");
                    } break;
                case State.FOLDER_PATH_NOT_FOUND:
                    {
                        MessageBox.Show("Could not find folder: " + path);
                    } break;
                case State.FOLDER_EMPTY:
                    {
                        MessageBox.Show("There were no pdf's in folder: " + path);
                    } break;
                case State.BACKGROUND_WORKER_ERROR:
                    {
                        MessageBox.Show("There was an error when processing the files.");
                    } break;
                default:
                    {
                        MessageBox.Show("An unspecified error occurred");
                    } break;
            }
        }
    }
}
