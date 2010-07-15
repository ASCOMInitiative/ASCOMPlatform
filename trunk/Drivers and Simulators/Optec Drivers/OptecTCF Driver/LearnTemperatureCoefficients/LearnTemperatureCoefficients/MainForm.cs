using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Interface;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace TempCoeffWizard
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }
        //public Focuser myFocuser;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string driverName = "ASCOM.OptecTCF_S.Focuser";
                WizardFocuser.myFocuser = new Focuser(driverName);
                //myFocuser = new Focuser(driverName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the Optec TCF-S focuser driver. Make sure that it is installed first\n" + ex.Message);
            }
        }

        private void Start_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                WizardFocuser.myFocuser.Link = true;
                if (!WizardFocuser.myFocuser.TempCompAvailable)
                    throw new ApplicationException("The temperature probe for your focuser is disabled.\n" +
                        "Use the Setup Dialog to enable the temperature probe before continuing.");
                double t = WizardFocuser.myFocuser.Temperature;
                int p = WizardFocuser.myFocuser.Position;
                WizardFocuser.myFocuser.Link = false;
                bool ContinueWizard = false;

                Step1Frm Step1Form = new Step1Frm();
                Step2Frm Step2Form;
                Step3Frm Step3Form;
                Step1Form.ShowDialog();
                if (Step1Form.ContinueWizard)
                {
                    Step1Form.Dispose();
                    Step2Form = new Step2Frm(ref ContinueWizard);
                    Step2Form.ShowDialog();

                    if (Step2Form.ContinueWizard)
                    {
                        Step2Form.Dispose();
                        Step3Form = new Step3Frm();
                        Step3Form.ShowDialog();
                        Step3Form.Dispose();
                        Application.Exit();
                    }
                }
                else Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (WizardFocuser.myFocuser.Link) WizardFocuser.myFocuser.Link = false;

            }
        }

        private void SetupDialog_Btn_Click(object sender, EventArgs e)
        {
            WizardFocuser.myFocuser.SetupDialog();
        }

        private void Abort_Btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

       
    }
}
