using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class LearnWizard1_5 : Form
    {
        public LearnWizard1_5()
        {
            InitializeComponent();
        }

        private void ModeSelected(object sender, EventArgs e)
        {
            Next_Btn.Enabled = true;
        }
    }
}
