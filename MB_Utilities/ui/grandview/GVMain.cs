﻿using System;
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
            updateMissingList_GV1.Show();
        }

        private void updateMissingListNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = updateMissingListNavBTN.Top;

            hideAllControls();
            updateMissingList_GV1.Show();
        }

        private void createMissingNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = createMissingNavBTN.Top;

            hideAllControls();
            createMissingList_GV1.Show();
        }

        private void hideAllControls()
        {
            updateMissingList_GV1.Hide();
            createMissingList_GV1.Hide();
        }
    }
}