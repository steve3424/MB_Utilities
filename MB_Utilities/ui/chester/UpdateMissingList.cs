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

namespace MB_Utilities.controls.chester
{
    public partial class UpdateMissingList : UserControl
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

        private const int CANNOT_SAVE_MISSING_LIST = 8;

        public UpdateMissingList()
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
            int missingListState = getMissingListState();
            int unbilledReportState = getUnbilledReportState();
            if (missingListState != MISSING_LIST_READY)
            {
                // showErrorMessage(missingListState);
            }
            else if (unbilledReportState != UNBILLED_REPORT_READY)
            {
                // showErrorMessage(missingListState);
            }
            else
            {
                /*
                if (!selectedDateIsOnReport())
                {
                    showErrorMessage(DATE_NOT_FOUND);
                }
                else 
                {
                    // put everything in here
                }
                */

                disableUI();
                
                FileInfo unbilledReportPath = new FileInfo(unbilledReportPathField.Text);
                FileInfo missingListPath = new FileInfo(missingListPathField.Text);
                using (ExcelPackage packageUnbilled = new ExcelPackage(unbilledReportPath))
                using (ExcelWorksheet worksheetUnbilled = packageUnbilled.Workbook.Worksheets[1])
                using (ExcelPackage packageMissing = new ExcelPackage(missingListPath))
                using (ExcelWorksheet worksheetMissing = packageMissing.Workbook.Worksheets[1])
                {
                    List<Dictionary<string, string>> unbilledReportCharts = loadFromUnbilled(worksheetUnbilled);
                    appendToMissingList(unbilledReportCharts, worksheetMissing);

                    try 
                    {
                        packageMissing.Save();
                        MessageBox.Show("Update Complete!");
                    }
                    catch (InvalidOperationException)
                    {
                        showErrorMessage(CANNOT_SAVE_MISSING_LIST);
                    }
                }
                   
                enableUI();
            }
        }

        /************* MAIN FUNCTIONS ******************/
        private List<Dictionary<string, string>> loadFromUnbilled(ExcelWorksheet worksheet)
        {
            List<Dictionary<string, string>> unbilledReportCharts = new List<Dictionary<string, string>>();

            int startRow = findStartRowOfUnbilledReport(worksheet);
            int endRow = findEndRowOfUnbilledReport(worksheet);
            for (int i = startRow; i < endRow; i++)
            {
                string currentCell = worksheet.Cells[i, 1].GetValue<string>();
                if (!string.IsNullOrEmpty(currentCell))
                {
                    int chartNum = worksheet.Cells[i, 1].GetValue<int>();
                    string patientName = worksheet.Cells[i, 4].GetValue<string>();
                    string date = worksheet.Cells[i, 7].GetValue<string>();
                    Dictionary<string, string> chartInfo = new Dictionary<string, string>()
                    {
                        {"chartNum", chartNum.ToString() },
                        {"patientName", patientName },
                        {"date", date }
                    };
                    unbilledReportCharts.Add(chartInfo);
                }
            }
            return unbilledReportCharts;
        }

        private void appendToMissingList(List<Dictionary<string, string>> unbilledReportCharts, ExcelWorksheet worksheet)
        {
            FileInfo missingListPath = new FileInfo(missingListPathField.Text);
            int insertionRow = findEndRowOfMissingList(worksheet);
            foreach (Dictionary<string, string> chartToAppend in unbilledReportCharts)
            {
                worksheet.InsertRow(insertionRow, 1);
                worksheet.Cells[insertionRow, 1].Value = chartToAppend["chartNum"];
                worksheet.Cells[insertionRow, 2].Value = chartToAppend["patientName"];
                worksheet.Cells[insertionRow, 3].Value = chartToAppend["date"];

                // format new row like the rest of the missing list
                worksheet.Row(insertionRow).Style.Font.Size = 7.5F;
                worksheet.Row(insertionRow).Style.Font.Name = "Verdana";
                worksheet.Row(insertionRow).Height = 15;
                worksheet.Cells[insertionRow, 1, insertionRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[insertionRow, 1, insertionRow, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                // update total of missing list
                worksheet.Cells[insertionRow, 3].Value = worksheet.Cells[insertionRow, 3].GetValue<int>() + 1;

                insertionRow++;
            }
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

        private int findEndRowOfUnbilledReport(ExcelWorksheet worksheet)
        {
            int endRow = 13; // begin from start row of 13
            string endString = "TD - MISSING COMPLETE CHART Total:";
            string stringOnReport = worksheet.Cells[endRow, 3].GetValue<string>();
            while (stringOnReport != endString)
            {
                endRow++;
                stringOnReport = worksheet.Cells[endRow, 3].GetValue<string>();
            }
            return endRow;
        }

        private int findEndRowOfMissingList(ExcelWorksheet worksheet)
        {
            int endRow = 13; // begin from start row of 13
            string endString = "TD - MISSING COMPLETE CHART Total:";
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
            /*
             * NOT YET IMPLEMENTED
            else if (!correctFile())
            {
                return UNBILLED_INCORRECT;
            }
            */
            return UNBILLED_REPORT_READY;
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
