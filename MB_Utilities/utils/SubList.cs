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
        public int totalCharts;
        public Dictionary<int, Tuple<int, string, DateTime>> chartInfo = new Dictionary<int, Tuple<int, string, DateTime>>();
    }
}
