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


        private void NewValue_TB_Validating(object sender, CancelEventArgs e)
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
            CancelEventArgs c = new CancelEventArgs();
            NewValue_TB_Validating(this.NewValue_TB, c);
            if (c.Cancel) return;
            else
            {
                newValue = double.Parse(this.NewValue_TB.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }     
        }

        private void Cancel_BTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetSkyPA_Frm_Load(object sender, EventArgs e)
        {
            this.CurrentSkyPA_LBL.Text = oldValue.ToString("000.00°");
            this.Degree_TB.Focus();
            this.Degree_TB.SelectAll();
        }

        public double OldPAValue
        {
            set { oldValue = value; }
        }

        public double NewPAValue
        {
            get { return newValue; }
        }

        private void NewValue_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OK_BTN_Click(OK_BTN, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void Degree_TB_TextChanged(object sender, EventArgs e)
        {
            double deg = 0;
            double min = 0;
            double sec = 0;

            try
            {
                deg = (Degree_TB.TextLength > 0) ? double.Parse(Degree_TB.Text) : 0;
                min = (Min_TB.TextLength > 0) ? double.Parse(Min_TB.Text) : 0;
                sec = (Sec_TB.TextLength > 0) ? double.Parse(Sec_TB.Text) : 0;

                if (deg > 359.9999)
                    Degree_TB.Text = "359";
                if (min > 59.999) Min_TB.Text = "59";
                if (sec > 59.999) Sec_TB.Text = "59";

                double result = deg + (min / 60) + (sec / (60 * 60));
                NewValue_TB.Text = result.ToString("000.00");
            }
            catch
            {
                MessageBox.Show("Invalid value entered");
            }
        }

        private void Degree_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Char.IsLetter(e.KeyChar) || System.Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
            } 
        }

        private void Degree_TB_Enter(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.SelectAll();
        }
    }
}
