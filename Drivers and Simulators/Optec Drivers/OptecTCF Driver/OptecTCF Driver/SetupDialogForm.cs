using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace ASCOM.OptecTCF_Driver
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void chooseCOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            COMPortForm CPFrm = new COMPortForm();
            CPFrm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 AboxForm = new AboutBox1();
            AboxForm.ShowDialog();  
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                UpdateControls();

            }
            catch
            {
                UpdateControls(); ;
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                UpdateControls();
            }
            catch(Exception Ex)
            {
                UpdateControls();
                DialogResult Result;
                Result = MessageBox.Show("Could not connect to device.\n" + 
                            "This may result from not selecting the correct COM port.\n" + 
                            "Would you like to see the exception data?", 
                            "Connection Failed" ,MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes)
                    MessageBox.Show("Error Message: \n" + Ex.ToString());
                else
                {
                    //don't display anything....
                }
            }
        }
        internal void UpdateControls()
        {
            if (DeviceComm.GetConnectionState())
            {
                StatusLabel.Text = "Connected successfully!";
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
            }
            else
            {
                StatusLabel.Text = "Device is not connected";
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceComm.Disconnect();
            UpdateControls();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeviceComm.Disconnect();
        }

       
        private void CancelOperations( SetupDialogForm setupDialogForm)
        {
            MessageBox.Show("You hit cancel");
        }

        private void wizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////////// DEFINE VARIABLES /////////////////////////////////////////
            char Mode = 'A';
            int FirstPos = 0;
            double FirstTemp = 0;
            int SecondPos = 0;
            double SecondTemp = 0;



            ////////// CONNECT TO DEVICE ////////////////////////////////////////
            try
            {
                DeviceComm.Connect();
                UpdateControls();
            }
            catch
            {
                MessageBox.Show("Failed to connect to device. \nThe wizard can not start until the device is connected.");
                return;
            }
            //////////BEGIN WIZARD - LOAD STEP 1 ////////////////////////////////
            DialogResult Wiz1Result = new DialogResult();
            LearnWizard1 Wizard1 = new LearnWizard1();
            Wiz1Result = Wizard1.ShowDialog();
            if (Wiz1Result == DialogResult.Cancel)
            {
                CancelOperations(this);
                Wizard1.Dispose();
                Wizard1 = null;
                return;
            }
            else if (Wiz1Result == DialogResult.OK)
            {
                Wizard1.Dispose();
                Wizard1 = null;
                goto Step1_5;
            }
            /////////LOAD STEP 1_5 - GET MODE  A OR B ////////////////////////////
            Step1_5:
            DialogResult Wiz1_5Result = new DialogResult();
            LearnWizard1_5  Wizard1_5 = new LearnWizard1_5();
            Wiz1_5Result = Wizard1_5.ShowDialog();
            if (Wiz1_5Result == DialogResult.Cancel)
            {
                CancelOperations(this);
                Wizard1_5.Dispose();
                Wizard1_5 = null;
                return;
            }
            else if (Wiz1_5Result == DialogResult.OK)
            {
                if (Wizard1_5.ModeA_RB.Checked) Mode = 'A';
                if (Wizard1_5.ModeB_RB.Checked) Mode = 'B';
                Wizard1_5.Dispose();
                Wizard1_5 = null;
                goto Step2;
            }

            ////////LOAD STEP 2 //////////////////////////////////////////////////////////
            Step2:
            DialogResult Wiz2Result = new DialogResult();
            LearnWizard2  Wizard2 = new LearnWizard2();
            Wiz2Result = Wizard2.ShowDialog();
            if (Wiz2Result == DialogResult.Cancel)
            {
                CancelOperations(this);
                Wizard2.Dispose();
                Wizard2 = null;
                return;
            }
            else if (Wiz2Result == DialogResult.OK)
            {
                FirstPos = int.Parse(Wizard2.Position_LB.Text);
                FirstTemp = double.Parse(Wizard2.Temp_LB.Text);
                Wizard2.Dispose();
                Wizard2 = null;
                goto Step3;
            }
            ///////// LOAD STEP 3 ////////////////////////////////////////////////////////
            Step3:
            DialogResult Wiz3Result = new DialogResult();
            LearnWizard3 Wizard3 = new LearnWizard3();
            Wizard3.FirstPos_LB.Text = FirstPos.ToString();
            Wizard3.FirstTemp_Lb.Text = FirstTemp.ToString();
            Wiz3Result = Wizard3.ShowDialog();
            if (Wiz3Result == DialogResult.Cancel)
            {
                CancelOperations(this);
                Wizard3.Dispose();
                Wizard3 = null;
                return;
            }
            else if (Wiz3Result == DialogResult.OK)
            {
                SecondPos = int.Parse(Wizard3.Position_LB.Text);
                SecondTemp = double.Parse(Wizard3.Temp_LB.Text);
                
                Wizard3.Dispose();
                Wizard3 = null;
                try
                {
                    SaveSlope(Mode, FirstTemp, SecondTemp, FirstPos, SecondPos);
                }
                catch(DivideByZeroException)
                {
                    MessageBox.Show("No temperature change has occured. Can not compute slope");
                    return;
                }
                goto Step4;
            }
            ////////////////LOAD STEP 4 - FINISHED ////////////////////////////////////////
            Step4:
            LearnWizard4 Wizard4 = new LearnWizard4(); 
            char sign = DeviceComm.GetSlopeSign(Mode);
            string slope = DeviceComm.GetLearnedSlope(Mode).ToString();
            Wizard4.Description_TB.Text += "New Slope for Mode " + Mode.ToString() + " = " + sign.ToString() + slope + "\n";
            Wizard4.ShowDialog();
            return;


            ///////////////CANCEL OPERATIONS ///////////////////////////////////////////////
            //Wizard1.Dispose();
            //Wizard1 = null;
            //Wizard1_5.Dispose();
            //Wizard1_5 = null;
            //Wizard2.Dispose();
            //Wizard2 = null;
            //Wizard3.Dispose();
            //Wizard3 = null;


        }

        private bool SaveSlope(char Mode, double FirstTemp, double SecondTemp, int FirstPos, int SecondPos)
        {
            
            if (Math.Abs(FirstTemp - SecondTemp) < 5)
            {
                MessageBox.Show("WARNING: Temperature difference should be at least 5° for an accurate time constant.");
            }
            if (FirstPos == SecondPos)
            {
                MessageBox.Show("Could not save time constant to device. Both focuser positions can not be the same.\n" );
                return false;
            }
            

            //Calculate Slope - Slope = Steps per Degree
            int slope = (FirstPos - SecondPos) /  (Convert.ToInt32(SecondTemp) - Convert.ToInt32(FirstTemp));
            char SlopeSign;
            if (slope < 0) SlopeSign = '-';
            else SlopeSign = '+';

            slope = Math.Abs(slope);

            if (slope < 2 || slope > 999)
            {
                MessageBox.Show("Could not save time constant to device. TC must be between 2 and 99.\n"
                    + "Calculated TC = " + slope.ToString() + ".\n");
                return false;
            }

            DeviceComm.LoadSlope(slope, Mode);
            DeviceComm.SetSlopeSign(SlopeSign, Mode);
            return true;
        }

        private void firstPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                SetStartPtForm SSPFrm = new SetStartPtForm();
                SSPFrm.ShowDialog();
                SSPFrm.Dispose();
                SSPFrm = null;
            }
            catch
            {
                MessageBox.Show("Unable to connect to device. \nHave you selected the right COM port?");
            }
            
        }

        private void endPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                SetEndPtForm SEPFrm = new SetEndPtForm();
                SEPFrm.ShowDialog();
                SEPFrm.Dispose();
                SEPFrm = null;
            }
            catch
            {
                MessageBox.Show("Unable to connect to device. \nHave you selected the right COM port?");
            }
        }



    }
}