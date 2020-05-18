using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.utils
{
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
