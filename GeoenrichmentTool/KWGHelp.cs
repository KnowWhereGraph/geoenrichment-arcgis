using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class KWGHelp : Form
    {
        public KWGHelp(string help)
        {
            InitializeComponent();
            helpText.Text = help;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}
