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
    public partial class SetSkyPAForm : Form
    {
        public double PA;

        public SetSkyPAForm()
        {
            InitializeComponent();
        }

        private void Degrees_TB_Leave(object sender, EventArgs e)
        {
            TextBox Sender = sender as TextBox;
            double val = 0;
            if(double.TryParse(Sender.Text, out val))
            {
                if (val < 0 || val >= 360)
                {
                    // Do nothing and fall through to message box.
                }
                else
                {
                    TotalLBL.Text = ComputePA().ToString("000.00°");
                    return;
                }
                    
            }
            MessageBox.Show("Degree value must a numeric value between 0 and 359.99");
            Sender.Focus();
        }

        private void Min_TB_Leave(object sender, EventArgs e)
        {
            TextBox Sender = sender as TextBox;
            double val = 0;
            if (double.TryParse(Sender.Text, out val))
            {
                if (val < 0 || val >= 60)
                {
                    // Do nothing and fall through to message box.
                }
                else
                {
                    TotalLBL.Text = ComputePA().ToString("000.00°");
                    return;
                }

            }
            MessageBox.Show("Value must a numeric value between 0 and 59.99");
            Sender.Focus();
        }

        private void Sec_TB_Leave(object sender, EventArgs e)
        {
            TextBox Sender = sender as TextBox;
            double val = 0;
            if (double.TryParse(Sender.Text, out val))
            {
                if (val < 0 || val >= 60)
                {
                    // Do nothing and fall through to message box.
                }
                else
                {
                    TotalLBL.Text = ComputePA().ToString("000.00°");
                    return;
                }

            }
            MessageBox.Show("Value must a numeric value between 0 and 59.99");
            Sender.Focus();
        }

        private void OK_BTN_Click(object sender, EventArgs e)
        {
            // Verify the result is okay...
            if (ComputePA() >= 360)
            {
                MessageBox.Show("Sky PA must be in the range of 0 to 359.99");
                return;
            }

            PA = ComputePA();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private int ComputePA()
        {
            // Make sure all of the boxes have a value...
            if (Degrees_TB.Text.Length == 0) Degrees_TB.Text = "00";
            if (Min_TB.Text.Length == 0) Min_TB.Text = "00";
            if (Sec_TB.Text.Length == 0) Sec_TB.Text = "00";

            double deg = double.Parse(Degrees_TB.Text);
            double min = double.Parse(Min_TB.Text) / 60;
            double sec = double.Parse(Sec_TB.Text) / (60*60);

            double pa = deg + min + sec;

            return (int)pa;

            
        }

        private void SetSkyPAForm_Load(object sender, EventArgs e)
        {

        }
    }
}
