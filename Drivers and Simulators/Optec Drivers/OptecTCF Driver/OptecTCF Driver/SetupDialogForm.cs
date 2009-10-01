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
        private bool ConnectedForControls = false;
        private delegate void UpdateDisplayHandler(int pos, double temp);
        private static Object LockObject = new Object();
        private int CurrentPos = 000;
        private int DesiredPos = 000;
        private int MaxPos = 0;

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
            lock (LockObject)
            {
                AboutBox1 AboxForm = new AboutBox1();
                AboxForm.ShowDialog();
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                MaxPos = DeviceComm.GetMaxStep();
                UpdateControls();
                this.ModeAName_TB.Text = DeviceSettings.GetModeName('A');
                this.ModeBName_TB.Text = DeviceSettings.GetModeName('B');
                

            }
            catch
            {
                UpdateControls(); ;
            }
            char Mode = DeviceSettings.GetActiveMode();
            if (Mode == 'A')
            {
                ModeA_RB.Checked = true;
            }
            else if (Mode == 'B')
            {
                ModeB_RB.Checked = true;
            }
            this.backgroundWorker1.RunWorkerAsync();
            this.backgroundWorker2.RunWorkerAsync();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                MaxPos = DeviceComm.GetMaxStep();
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
                ConnectedForControls = true;
                StatusLabel.Text = "Connected successfully!";
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
                foreach (Control x in FocStatusControls.Controls)
                {
                    x.Enabled = true;
                    PowerLight.Visible = true;
                }
            }
            else
            {
                ConnectedForControls = false;
                StatusLabel.Text = "Device is not connected";
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
                foreach (Control x in FocStatusControls.Controls)
                {
                    x.Enabled = false;
                    PowerLight.Visible = false;
                }
            }
         

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceComm.Disconnect();
            UpdateControls();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (LockObject)
            {
                DeviceComm.Disconnect();
            }
        }
       
        private void CancelOperations( SetupDialogForm setupDialogForm)
        {
            MessageBox.Show("You hit cancel");
        }

        private void wizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LockObject)
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
                LearnWizard1_5 Wizard1_5 = new LearnWizard1_5();
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
                LearnWizard2 Wizard2 = new LearnWizard2();
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
                    catch (DivideByZeroException)
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

            DeviceComm.SetSlope(slope, Mode);
            DeviceComm.SetSlopeSign(SlopeSign, Mode);
            return true;
        }

        private void firstPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                lock (LockObject)
                {
                    DeviceComm.Connect();
                    SetStartPtForm SSPFrm = new SetStartPtForm();
                    SSPFrm.ShowDialog();
                    SSPFrm.Dispose();
                    SSPFrm = null;
                }
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
                lock(LockObject)
                {
                    DeviceComm.Connect();
                    SetEndPtForm SEPFrm = new SetEndPtForm();
                    SEPFrm.ShowDialog();
                    SEPFrm.Dispose();
                    SEPFrm = null;
                }
            }
            catch
            {
                MessageBox.Show("Unable to connect to device. \nHave you selected the right COM port?");
            }
        }

        private void EditNames_Btn_Click(object sender, EventArgs e)
        {
            if (EditNames_Btn.Text == "DONE")
            {
                DeviceSettings.SetModeNames(ModeAName_TB.Text, ModeBName_TB.Text);
                EditNames_Btn.Text = "Edit Names";
                ModeAName_TB.ReadOnly = true;
                ModeBName_TB.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("NOTE: Names will not be saved until you press DONE");
                EditNames_Btn.Text = "DONE";
                ModeAName_TB.ReadOnly = false;
                ModeBName_TB.ReadOnly = false;
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ///this continually updates the display
            int pos = 0;
            double temp = 0;
            while (true)
            {
                if (ConnectedForControls && this.Visible)
                {
                    try
                    {
                        lock (LockObject)
                        {

                            pos = DeviceComm.GetPosition();
                            CurrentPos = pos;
                            temp = DeviceComm.GetTemperaterature();
                            this.BeginInvoke(new UpdateDisplayHandler(UpdateDisplay), new Object[] { pos, temp });
                        }
                    }
                    catch { }
                }
            }
        }

        private void UpdateDisplay(int pos, double temp)
        {
            if (this.Visible)
            {
                //this.Pos_TB.Text = DesiredPos.ToString();
                if (DesiredPos != CurrentPos)
                {
                    if (DesiredPos != 0)
                    {
                        this.Pos_TB.Text = "MOVING";
                        this.Temp_TB.Text = temp.ToString() + " °C";
                    }
                    else
                    {
                        this.Pos_TB.Text = "------";
                        this.Temp_TB.Text = temp.ToString() + " °C";
                    }
                }
                else
                {
                    
                    this.Pos_TB.Text = pos.ToString();
                    this.Temp_TB.Text = temp.ToString() + " °C";
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if ((ConnectedForControls && this.Visible) && (CurrentPos != DesiredPos))
                {
                    try
                    {
                        lock (LockObject)
                        {
                            if (DesiredPos == 0)
                            {
                                DesiredPos = CurrentPos;
                            }
                            DeviceComm.MoveFocus(DesiredPos);
                        }
                    }
                    catch { }
                }
            }
        }

        private void In_BTN_Click(object sender, EventArgs e)
        {
            DesiredPos = DesiredPos - Convert.ToInt32(Increment_NUD.Value);
            if (DesiredPos < 1) DesiredPos = 1;
        }

        private void Out_BTN_Click(object sender, EventArgs e)
        {
            DesiredPos = DesiredPos + Convert.ToInt32(Increment_NUD.Value);
            if (DesiredPos > MaxPos) DesiredPos = MaxPos;
        }

        private void ModeRBChecked_Changed(object sender, EventArgs e)
        {
            if(ModeA_RB.Checked)
            {
                DeviceSettings.SetActiveMode('A');
            }
            else if (ModeB_RB.Checked)
            {
                DeviceSettings.SetActiveMode('B');
            }
        }

        private void manuallyEnterSlopeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LockObject)
            {
                DeviceComm.Connect();
                SetSlopeForm SSFrm = new SetSlopeForm();
                SSFrm.ShowDialog();
            }
        }

        private void displayToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            lock (LockObject)
            {
                DeviceComm.Connect();
                DisplayTempCoEffs Frm = new DisplayTempCoEffs();
                Frm.ShowDialog();
            }
        }

       

    }
}