using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

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

    class MissingList
    {
        // string that indicates the end of the UnbilledRegularReport
        public static string EOF = "1/1";

        // sublist header and footer strings
        public static Dictionary<string, string> header_strings = new Dictionary<string, string>() {
            {"ME", "ME - MISSING EXAM OR PHYS NOTES"},
            {"PM", "PM - PROCEDURE NOTE MISSING"},
            {"SC", "SC - MISSING SIGNATURE BUT CODED"},
            {"SG", "SG - MISSING SIGNATURE"},
            {"TD", "TD - MISSING COMPLETE CHART"},
            {"WR", "need to implement"}
        };

        public static Dictionary<string, string> footer_strings = new Dictionary<string, string>() {
            {"ME", "ME - MISSING EXAM OR PHYS NOTES Total:"},
            {"PM", "PM - PROCEDURE NOTE MISSING Total:"},
            {"SC", "SC - MISSING SIGNATURE BUT CODED Total:"},
            {"SG", "SG - MISSING SIGNATURE Total:"},
            {"TD", "TD - MISSING COMPLETE CHART Total:"},
            {"WR", "need to implement"}
        };

        public static List<SubList> createSubLists(List<string> subListsToCreate, string missingListPathField)
        {
            List<SubList> subLists = new List<SubList>();

            FileInfo path = new FileInfo(missingListPathField);
            using (ExcelPackage package = new ExcelPackage(path))
            using (ExcelWorksheet worksheet = package.Workbook.Worksheets[1])
            {
                foreach (string subListID in subListsToCreate)
                {
                    SubList newSubList = new SubList();
                    newSubList.name = subListID;
                    newSubList.startRow = findStartOfList(worksheet, subListID);
                    newSubList.endRow = findEndOfList(worksheet, subListID);
                    newSubList.patientInfo = loadPatientInfo(worksheet, newSubList.startRow, newSubList.endRow);

                    subLists.Add(newSubList);
                }
            }
            return subLists;
        }

        /*
         * Header string is in col B (2)
         * EOF string is in col N (14)
         * charts begin 2 rows after header string
         */
        public static int findStartOfList(ExcelWorksheet worksheet, string subListID)
        {
            string cellValue = null;
            string header = header_strings[subListID];
            string eof_check = null;
            int start = 0;
            while ((cellValue != header) && (eof_check != EOF))
            {
                ++start;
                cellValue = worksheet.Cells[start, 2].GetValue<string>();
                eof_check = worksheet.Cells[start, 14].GetValue<string>();
            }

            if (eof_check == EOF)
            {
                start = 0;
            }
            else
            {
                start += 2;
            }

            return start;
        }

        public static int findEndOfList(ExcelWorksheet worksheet, string subListID)
        {
            string cellValue = null;
            string footer = footer_strings[subListID];
            string eof_check = null;
            int end = 0;
            bool footerFound = false;
            while (!footerFound && (eof_check != EOF))
            {
                ++end;
                cellValue = worksheet.Cells[end, 2].GetValue<string>();
                eof_check = worksheet.Cells[end, 14].GetValue<string>();
                if (!string.IsNullOrEmpty(cellValue) && !string.IsNullOrWhiteSpace(cellValue))
                {
                    footerFound = cellValue.Contains(footer);
                }
            }

            if (eof_check == EOF)
            {
                end = 0;
            }

            return end;
        }

        public static Dictionary<string, Dictionary<string, string>> loadPatientInfo(ExcelWorksheet worksheet, int startRow, int endRow)
        {
            Dictionary<string, Dictionary<string, string>> patients = new Dictionary<string, Dictionary<string, string>>();

            for (int row = startRow; row < endRow; ++row)
            {
                string chartNum = worksheet.Cells[row, 1].GetValue<string>();
                if (string.IsNullOrEmpty(chartNum) || string.IsNullOrWhiteSpace(chartNum))
                {
                    continue;
                }
                string isUnderlined = worksheet.Cells[row, 1].Style.Font.UnderLine ? "isUnderlined" : "";
                string patientName = worksheet.Cells[row, 3].GetValue<string>();
                string date = worksheet.Cells[row, 7].GetValue<DateTime>().ToShortDateString();

                Dictionary<string, string> patientInfo = new Dictionary<string, string>()
                {
                    { "rowNum", row.ToString()},
                    { "patientName", patientName},
                    { "date", date},
                    { "chartNum", chartNum },
                    { "isUnderlined", isUnderlined}
                };
                patients.Add(chartNum, patientInfo);
            }
            return patients;
        }
    }
}
