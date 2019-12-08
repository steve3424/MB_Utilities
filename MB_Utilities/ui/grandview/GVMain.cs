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

            // hide all controls
            // show initial control
        }

        private void updateMissingListNavBTN_Click(object sender, EventArgs e)
        {
            navSelectedPanel.Top = updateMissingListNavBTN.Top;

            // hide all controls
            // show updateMissingList control
        }
    }
}
