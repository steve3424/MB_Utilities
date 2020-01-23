using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MB_Utilities.utils
{
    class SubList
    {
        public string name;
        public int startRow;
        public int endRow;
        public int numPatients;
        public Dictionary<string, Dictionary<string, string>> patientInfo = new Dictionary<string, Dictionary<string, string>>();
    }
}
