using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities.ui.grandview
{
    public partial class GVMain : UserControl
    {
        public GVMain()
        {
            InitializeComponent();

            hideAllControls();
            searchStragglers_GV1.Show();
        }

        private void createListsNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = createListsNavBTN.Top;

            hideAllControls();
            createLists_GV1.Show();

        }

        private void searchStragglersNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = searchStragglersNavBTN.Top;

            hideAllControls();
            searchStragglers_GV1.Show();
        }

        private void updateMissingListNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = updateMissingListNavBTN.Top;

            hideAllControls();
            updateMissingList_GV1.Show();
        }

        private void loadDaysNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = loadDaysNavBTN.Top;

            hideAllControls();
            loadDays_GV1.Show();
        }

        private void hideAllControls()
        {
            searchStragglers_GV1.Hide();
            createLists_GV1.Hide();
            updateMissingList_GV1.Hide();
            loadDays_GV1.Hide();
        }
    }
}
