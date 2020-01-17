using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.ui.chester
{
    public partial class CTMain : UserControl
    {
        public CTMain()
        {
            InitializeComponent();

            hideAllControls();
            processFiles1.Show();
        }

        private void processFilesNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = processFilesNavBTN.Top;

            hideAllControls();
            processFiles1.Show();
        }

        private void processStragglersNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = processStragglersNavBTN.Top;

            hideAllControls();
            processStragglers1.Show();
        }

        private void createListsNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = createListsNavBTN.Top;

            hideAllControls();
            createLists1.Show();
        }

        private void updateMissingNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = updateMissingNavBTN.Top;

            hideAllControls();
            updateMissingList1.Show();
        }

        private void loadDaysNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = loadDaysNavBTN.Top;

            hideAllControls();
            loadDays1.Show();
        }

        private void hideAllControls()
        {
            processFiles1.Hide();
            processStragglers1.Hide();
            createLists1.Hide();
            updateMissingList1.Hide();
            loadDays1.Hide();
        }
    }
}
