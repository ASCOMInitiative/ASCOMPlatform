using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.PyxisLE_ASCOM
{
    public partial class Warning1Form : Form
    {
        public Warning1Form()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Utilities.Profile p = new Utilities.Profile();
            p.DeviceType = "Rotator";
            p.WriteValue(Rotator.s_csDriverID, "ShowSetPAWarning", false.ToString());
        }
    }
}
