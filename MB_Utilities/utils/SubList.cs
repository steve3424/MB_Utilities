using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.utils
{
    // name => log code ID (TD, NN, ME, etc)
    // startRow => row number on spreadsheet where first chart is on sublist
    // endRow => row number + 1 where last chart is on sublist
    // patientInfo => nested dictionary where key is chart number and secondary dictionary can have any info that you would want to load for the patient
    class SubList
    {
        public string name;
        public int startRow;
        public int endRow;
        public Dictionary<string, Dictionary<string, string>> patientInfo = new Dictionary<string, Dictionary<string, string>>();

        public void PrintSubList() {
            MessageBox.Show("name: " + name + "\n" +
                            "startRow: " + startRow + "\n" +
                            "endRow: " + endRow + "\n");

            foreach (string chartNum in patientInfo.Keys) {
                MessageBox.Show("chartNum: " + chartNum + "\n" +
                                "rowNum: " + patientInfo[chartNum]["rowNum"] + "\n" +
                                "patientName: " + patientInfo[chartNum]["patientName"] + "\n" +
                                "date: " + patientInfo[chartNum]["date"] + "\n");
            }
        }
    }
}
