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
    public partial class SearchStragglers_GV : UserControl
    {
        public SearchStragglers_GV()
        {
            InitializeComponent();

            // check TD at start
            for (int i=0; i < checkedListBox1.Items.Count; ++i)
            {
                string itemName = checkedListBox1.Items[i].ToString();
                if (itemName == "TD")
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }
    }
}
