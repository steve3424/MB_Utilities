﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB_Utilities
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void chesterCountyNavBTN_Click(object sender, EventArgs e)
        {
            titleMain.Hide();
            iconMain.Hide();
            gvMain1.Hide();

            ctMain1.Show();
        }

        private void grandviewNavBTN_Click(object sender, EventArgs e)
        {
            titleMain.Hide();
            iconMain.Hide();
            ctMain1.Hide();

            gvMain1.Show();
        }
    }
}
