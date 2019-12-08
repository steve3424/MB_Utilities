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

namespace MB_Utilities.ui.grandview
{
    public partial class UpdateMissingList_GV : UserControl
    {
        // state of missing list
        private const int MISSING_LIST_READY = 0;
        private const int MISSING_LIST_PATH_EMPTY = 1;
        private const int MISSING_LIST_NOT_FOUND = 2;
        private const int MISSING_LIST_INCORRECT = 3;

        // state of unbilled report
        private const int UNBILLED_REPORT_READY = 4;
        private const int UNBILLED_REPORT_PATH_EMPTY = 5;
        private const int UNBILLED_REPORT_NOT_FOUND = 6;
        private const int UNBILLED_REPORT_INCORRECT = 7;
        private const int DATE_NOT_FOUND = 8;

        private const int CANNOT_SAVE_MISSING_LIST = 9;

        public UpdateMissingList_GV()
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

        private void chooseUnbilledReportBTN_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Sheet (.xlsx)|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    unbilledReportPathField.Text = openFileDialog.FileName;
                }
            }
        }

        private void updateMissingListBTN_Click(object sender, EventArgs e)
        {
            disableUI();

            int missingListState = getMissingListState();
            int unbilledReportState = getUnbilledReportState();
            if (missingListState != MISSING_LIST_READY)
            {
                showErrorMessage(missingListState);
            }
            else if (unbilledReportState != UNBILLED_REPORT_READY)
            {
                showErrorMessage(unbilledReportState);
            }
            else
            {

                List<Dictionary<string, string>> unbilledReportCharts = loadFromUnbilled();
                bool updateSuccessful = appendToMissingList(unbilledReportCharts);
                if (updateSuccessful)
                {
                    MessageBox.Show("Update Complete!");
                }
                else
                {
                    showErrorMessage(CANNOT_SAVE_MISSING_LIST);
                }
            }
            enableUI();
        }

        /************* MAIN FUNCTIONS ******************/
        private List<Dictionary<string, string>> loadFromUnbilled()
        {
            List<Dictionary<string, string>> unbilledReportCharts = new List<Dictionary<string, string>>();

            FileInfo unbilledReportPath = new FileInfo(unbilledReportPathField.Text);
            using (ExcelPackage packageUnbilled = new ExcelPackage(unbilledReportPath))
            using (ExcelWorksheet worksheetUnbilled = packageUnbilled.Workbook.Worksheets[1])
            {
                int startRow = findStartRowOfUnbilledReport(worksheetUnbilled);
                int endRow = findEndRowOfUnbilledReport(worksheetUnbilled, startRow);
                for (int i = startRow; i < endRow; i++)
                {
                    string currentCellValue = worksheetUnbilled.Cells[i, 1].GetValue<string>();
                    if (!string.IsNullOrEmpty(currentCellValue))
                    {
                        string chartNum = worksheetUnbilled.Cells[i, 1].GetValue<string>();
                        string patientName = worksheetUnbilled.Cells[i, 4].GetValue<string>();
                        string date = worksheetUnbilled.Cells[i, 7].GetValue<DateTime>().ToShortDateString();
                        string doctor = worksheetUnbilled.Cells[i, 8].GetValue<string>();
                        Dictionary<string, string> chartInfo = new Dictionary<string, string>()
                        {
                            {"chartNum", chartNum },
                            {"patientName", patientName },
                            {"date", date },
                            {"doctor", doctor}
                        };
                        unbilledReportCharts.Add(chartInfo);
                    }
                }
            }
            return unbilledReportCharts;
        }

        private bool appendToMissingList(List<Dictionary<string, string>> unbilledReportCharts)
        {
            FileInfo missingListPath = new FileInfo(missingListPathField.Text);
            using (ExcelPackage packageMissing = new ExcelPackage(missingListPath))
            using (ExcelWorksheet worksheetMissing = packageMissing.Workbook.Worksheets[1])
            {
                int insertRow = findEndRowOfMissingList(worksheetMissing);
                foreach (Dictionary<string, string> chartToAppend in unbilledReportCharts)
                {
                    worksheetMissing.InsertRow(insertRow, 1);
                    worksheetMissing.Cells[insertRow, 1].Value = chartToAppend["chartNum"];
                    worksheetMissing.Cells[insertRow, 2].Value = chartToAppend["patientName"];
                    worksheetMissing.Cells[insertRow, 3].Value = chartToAppend["date"];

                    // add doctor name if available
                    string doc = chartToAppend["doctor"];
                    if (string.IsNullOrEmpty(doc))
                    {
                        worksheetMissing.Cells[insertRow, 4].Value = "NO PHYSICIAL LISTED";
                        worksheetMissing.Cells[insertRow, 4].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheetMissing.Cells[insertRow, 4].Value = doc;
                    }

                    // format new row like the rest of the missing list
                    worksheetMissing.Row(insertRow).Style.Font.Size = 7.5F;
                    worksheetMissing.Row(insertRow).Style.Font.Name = "Verdana";
                    worksheetMissing.Row(insertRow).Height = 15;
                    worksheetMissing.Cells[insertRow, 1, insertRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheetMissing.Cells[insertRow, 1, insertRow, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    insertRow++;

                    // update total of missing list
                    worksheetMissing.Cells[insertRow, 3].Value = worksheetMissing.Cells[insertRow, 3].GetValue<int>() + 1;
                }

                try
                {
                    packageMissing.Save();
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
            return true;
        }

        private int findStartRowOfUnbilledReport(ExcelWorksheet worksheet)
        {
            int startRow = 13; // charts start at row 13 on Unbilled Report when run just for TD's
            string dateSelected = chooseDatePicker.Value.ToShortDateString();
            string dateOnReport = worksheet.Cells[startRow, 7].GetValue<DateTime>().ToShortDateString();
            while (dateOnReport != dateSelected)
            {
                startRow++;
                dateOnReport = worksheet.Cells[startRow, 7].GetValue<DateTime>().ToShortDateString();
            }
            return startRow;
        }

        private int findEndRowOfUnbilledReport(ExcelWorksheet worksheet, int startRow)
        {
            int endRow = startRow;
            string dateSelected = chooseDatePicker.Value.ToShortDateString();
            string dateOnReport = worksheet.Cells[endRow, 7].GetValue<DateTime>().ToShortDateString();
            string dateDefault = "1/1/0001";
            string endString = "TD - MISSING COMPLETE CHART Total:";
            string stringOnReport = worksheet.Cells[endRow, 3].GetValue<string>();
            while (dateOnReport == dateSelected)
            {
                endRow++;
                dateOnReport = worksheet.Cells[endRow, 7].GetValue<DateTime>().ToShortDateString();
                stringOnReport = worksheet.Cells[endRow, 3].GetValue<string>();
                while (dateOnReport == dateDefault && stringOnReport != endString)
                {
                    endRow++;
                    dateOnReport = worksheet.Cells[endRow, 7].GetValue<DateTime>().ToShortDateString();
                    stringOnReport = worksheet.Cells[endRow, 3].GetValue<string>();
                }
            }
            return endRow;
        }

        private int findEndRowOfMissingList(ExcelWorksheet worksheet)
        {
            int endRow = 13; // begin from start row of 13
            string endString = "TD - MISSING CHART Total:";
            string stringOnReport = worksheet.Cells[endRow, 1].GetValue<string>();
            while (stringOnReport != endString)
            {
                endRow++;
                stringOnReport = worksheet.Cells[endRow, 1].GetValue<string>();
            }
            return endRow;
        }


        /************* UTILITY FUNCTIONS ******************/

        private int getMissingListState()
        {
            if (string.IsNullOrEmpty(missingListPathField.Text))
            {
                return MISSING_LIST_PATH_EMPTY;
            }
            else if (!File.Exists(missingListPathField.Text))
            {
                return MISSING_LIST_NOT_FOUND;
            }
            /*
             * NOT YET IMPLEMENTED
            else if (!correctFile())
            {
                return MISSING_LIST_INCORRECT;
            }
            */
            return MISSING_LIST_READY;
        }

        private int getUnbilledReportState()
        {
            if (string.IsNullOrEmpty(unbilledReportPathField.Text))
            {
                return UNBILLED_REPORT_PATH_EMPTY;
            }
            else if (!File.Exists(unbilledReportPathField.Text))
            {
                return UNBILLED_REPORT_NOT_FOUND;
            }
            else if (!dateOnReport())
            {
                return DATE_NOT_FOUND;
            }
            /*
             * NOT YET IMPLEMENTED
            else if (!correctFile())
            {
                return UNBILLED_INCORRECT;
            }
            */
            return UNBILLED_REPORT_READY;
        }

        private bool dateOnReport()
        {
            FileInfo unbilledReportPath = new FileInfo(unbilledReportPathField.Text);
            using (ExcelPackage packageUnbilled = new ExcelPackage(unbilledReportPath))
            using (ExcelWorksheet worksheetUnbilled = packageUnbilled.Workbook.Worksheets[1])
            {
                int currentRow = 13;
                string dateSelected = chooseDatePicker.Value.ToShortDateString();
                string dateOnReport = worksheetUnbilled.Cells[currentRow, 7].GetValue<DateTime>().ToShortDateString();
                string endString = "TD - MISSING COMPLETE CHART Total:";
                string stringOnReport = worksheetUnbilled.Cells[currentRow, 3].GetValue<string>();
                while (stringOnReport != endString)
                {
                    if (dateOnReport == dateSelected)
                    {
                        return true;
                    }

                    currentRow++;
                    dateOnReport = worksheetUnbilled.Cells[currentRow, 7].GetValue<DateTime>().ToShortDateString();
                    stringOnReport = worksheetUnbilled.Cells[currentRow, 3].GetValue<string>();
                }
            }
            return false;
        }

        private void disableUI()
        {
            chooseMissingListBTN.Enabled = false;
            chooseUnbilledReportBTN.Enabled = false;
            chooseDatePicker.Enabled = false;
            updateMissingListBTN.Enabled = false;
        }

        private void enableUI()
        {
            chooseMissingListBTN.Enabled = true;
            chooseUnbilledReportBTN.Enabled = true;
            chooseDatePicker.Enabled = true;
            updateMissingListBTN.Enabled = true;
        }

        private void showErrorMessage(int error)
        {
            switch (error)
            {
                case MISSING_LIST_PATH_EMPTY:
                    MessageBox.Show("Please select the CT missing list");
                    return;
                case MISSING_LIST_NOT_FOUND:
                    MessageBox.Show("The CT missing list could not be found.");
                    return;
                case MISSING_LIST_INCORRECT:
                    MessageBox.Show("The missing list you chose is not the CT missing list.");
                    return;
                case UNBILLED_REPORT_PATH_EMPTY:
                    MessageBox.Show("Please select the CT unbilled report.");
                    return;
                case UNBILLED_REPORT_NOT_FOUND:
                    MessageBox.Show("The CT unbilled report could not be found.");
                    return;
                case UNBILLED_REPORT_INCORRECT:
                    MessageBox.Show("The unbilled report you chose is not the CT unbilled report.");
                    return;
                case DATE_NOT_FOUND:
                    MessageBox.Show("The date you've selected is not on the unbilled report. The missing list will not be updated.");
                    return;
                case CANNOT_SAVE_MISSING_LIST:
                    MessageBox.Show("It looks like the missing list is open somewhere else.\n\n" +
                        "The missing list will not be updated.\n\n" +
                        "Close the missing list and try again.");
                    return;
                default:
                    MessageBox.Show("An unspecified error occurred.");
                    return;
            }
        }
    }
}
