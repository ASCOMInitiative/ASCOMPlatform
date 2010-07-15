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
    public partial class Step1Frm : Form
    {
        public bool ContinueWizard = false;

        public Step1Frm()
        {
            InitializeComponent();
        }

        private void Capture_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                WizardFocuser.myFocuser.Link = true;
                WizardFocuser.StartTime = DateTime.Now;
                WizardFocuser.StartTemp = WizardFocuser.myFocuser.Temperature;
                WizardFocuser.StartPos = WizardFocuser.myFocuser.Position;
                WizardFocuser.myFocuser.Link = false;
                ContinueWizard = true;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Wizard was unable to connect to device.\n" +
                    "Make sure you disconnect from the focuser in your auto-focusing applicaiton before continuing.");
            }
            
        }

        private void Step1Frm_Load(object sender, EventArgs e)
        {

        }

        private void Abort_Btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Step1Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ContinueWizard == false)
            {
                DialogResult answer = new DialogResult();
                answer = MessageBox.Show("This will abort the entire process. Are you sure you want to quit now?",
                    "Quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    //Do nothng just exit
                }
                else e.Cancel = true;
            }
            
        
        }
    }
}
