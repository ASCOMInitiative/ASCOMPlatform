using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TempCoeffWizard
{
    public partial class Step3Frm : Form
    {
        public bool ContinueWizard = false;
        public Step3Frm()
        {
            InitializeComponent();
        }

        private void Step3Frm_Load(object sender, EventArgs e)
        {
            label1.Text = "The calculated temperature compensation coefficient is " + WizardFocuser.Coefficient.ToString() + ".";
            ModeA_rb.Text = "Mode A - \"" + WizardFocuser.ModeA_Name + "\"" ;
            ModeB_rb.Text = "Mode B - \"" + WizardFocuser.ModeB_Name + "\"";
            ModeA_rb.Checked = true;
        }

        private void Abort_Btn_Click(object sender, EventArgs e)
        {
            DialogResult answer = new DialogResult();
            answer = MessageBox.Show("Are you sure you want to quit now? The temperature coefficient will NOT be stored.", "Abort Now?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (answer == DialogResult.Yes) Application.Exit();
        }

        private void Capture_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ModeA_rb.Checked)
                {
                    WizardFocuser.StoreCoefficient('A');
                }
                else if (ModeB_rb.Checked)
                {
                    WizardFocuser.StoreCoefficient('B');
                }
                else if (Both_rb.Checked)
                {
                    WizardFocuser.StoreCoefficient('A');
                    WizardFocuser.StoreCoefficient('B');
                }
                MessageBox.Show("Temperature coefficient stored successfully. The process is now complete and your focuser is ready for use!");
                ContinueWizard = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Step3Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ContinueWizard == false)
            {
                DialogResult answer = new DialogResult();
                answer = MessageBox.Show("This will abort the entire process. Are you sure you want to quit now?",
                    "Quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else e.Cancel = true;
            }

            
        }
    }
}
