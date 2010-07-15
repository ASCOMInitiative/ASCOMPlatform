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
    public partial class Step2Frm : Form
    {
        public bool ContinueWizard = false;

        public Step2Frm(ref bool ContinueWizard)
        {
            InitializeComponent();
        }

        private void Step2Frm_Load(object sender, EventArgs e)
        {
            StartDate_Lb.Text = WizardFocuser.StartTime.ToString();
            StartPos_Lb.Text = WizardFocuser.StartPos.ToString();
            StartTemp_Lb.Text = WizardFocuser.StartTemp.ToString() + "°C";
        }

        private void Abort_Btn_Click(object sender, EventArgs e)
        {
            DialogResult answer = new DialogResult();
            answer = MessageBox.Show("Are you sure you want to quit now? Your start point data will not be saved.", "Abort Now?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (answer == DialogResult.Yes) Application.Exit();
        }

        private void Capture_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                WizardFocuser.myFocuser.Link = true;
                WizardFocuser.EndPos = WizardFocuser.myFocuser.Position;
                WizardFocuser.EndTemp = WizardFocuser.myFocuser.Temperature;
                WizardFocuser.myFocuser.Link = false;
                WizardFocuser.CalculateCoefficient();
                ContinueWizard = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Step2Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ContinueWizard == false)
            {
                DialogResult answer = new DialogResult();
                answer = MessageBox.Show("This will abort the entire process. Are you sure you want to quit now?",
                    "Quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    //just close the form...
                }
                else e.Cancel = true;
            }
            
        }

        
    }
}
