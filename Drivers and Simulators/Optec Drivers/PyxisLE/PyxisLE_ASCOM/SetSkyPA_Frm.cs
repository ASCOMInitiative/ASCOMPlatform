using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.PyxisLE_ASCOM
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
            this.NewValue_TB.Focus();
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
    }
}
