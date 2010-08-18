using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PyxisLE_Control
{
    public partial class SetSkyPA_Frm : Form
    {
        public SetSkyPA_Frm()
        {
            InitializeComponent();
        }

        private double oldValue = 0;
        private double newValue = 0;

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            double NewPA;
            TextBox sndr = sender as TextBox;
            try
            {
                NewPA =  double.Parse(sndr.Text);
                errorProvider1.Clear();
                if (NewPA > 360)
                {
                    errorProvider1.SetError(sndr, "Position angle can not exceed 360°");
                    e.Cancel = true;
                }
                if (NewPA < 0)
                {
                    errorProvider1.SetError(sndr, "Position angle must be positive");
                    e.Cancel = true;
                }

            }
            catch
            {
                errorProvider1.SetError(sndr, "Must be a number");
                e.Cancel = true;
            }

        }

        private void OK_BTN_Click(object sender, EventArgs e)
        {
            newValue = double.Parse(this.NewValue_TB.Text);
            this.Close();
        }

        private void Cancel_BTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetSkyPA_Frm_Load(object sender, EventArgs e)
        {
            this.CurrentSkyPA_LBL.Text = oldValue.ToString() + "°";
        }

        public double OldPAValue
        {
            set { oldValue = value; }
        }

        public double NewPAValue
        {
            get { return newValue; }
        }
    }
}
