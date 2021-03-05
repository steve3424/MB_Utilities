using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MB_Utilities.utils
{
    class LogReportOnDemand
    {
        public static List<Dictionary<string, string>> loadLogFile(string path)
        {
            List<Dictionary<string, string>> logFile = new List<Dictionary<string, string>>();

            string[] lines = File.ReadAllLines(path).Where(line => line != "").ToArray();
            foreach (string line in lines)
            {
                string[] chartInfo = line.Split(',');
                string date = chartInfo[0];
                string chartNum = chartInfo[1];
                string lastName = chartInfo[2];
                string firstName = chartInfo[3];
                string logCode = chartInfo[4];
                Dictionary<string, string> patientInfo = new Dictionary<string, string>()
                {
                    {"date", date },
                    {"chartNum", chartNum },
                    {"lastName", lastName },
                    {"firstName", firstName },
                    {"logCode", logCode },
                    {"missing", " " }
                };
                logFile.Add(patientInfo);
            }

            // log file is not necessarily in number order so it must be sorted by chart num
            logFile = logFile.OrderBy(x => x["chartNum"]).ToList<Dictionary<string, string>>();
            return logFile;
        }
    }
}
