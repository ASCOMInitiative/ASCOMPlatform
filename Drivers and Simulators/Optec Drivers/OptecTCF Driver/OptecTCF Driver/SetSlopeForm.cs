using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class SetSlopeForm : Form
    {
        public SetSlopeForm()
        {
            InitializeComponent();
            this.label1.Text = "Enter Slope =";
        }
        public SetSlopeForm(int slope)
        {
            InitializeComponent();
            this.label1.Text = "Calculated Slope =";
        }

        private void SetSlopeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
