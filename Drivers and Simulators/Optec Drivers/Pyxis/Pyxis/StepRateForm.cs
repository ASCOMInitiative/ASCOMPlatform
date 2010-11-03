using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Pyxis
{
    public partial class StepRateForm : Form
    {
        public int rate = 0;

        public StepRateForm()
        {
            InitializeComponent();
        }

        private void StepRateForm_Load(object sender, EventArgs e)
        {
            CurrentRateLBL.Text += rate.ToString();
        }

        private void SetBTN_Click(object sender, EventArgs e)
        {
            rate = (int)Rate_NUD.Value;
        }
    }
}
