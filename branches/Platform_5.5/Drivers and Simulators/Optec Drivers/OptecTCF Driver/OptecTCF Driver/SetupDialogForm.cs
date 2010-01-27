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
        private delegate void UpdatePosDisplayHandler(string s);
        private delegate void UpdateTempDisplayHandler(double temp);
        private static Object LockObject = new Object();
        private int CurrentPos;
        private int DesiredPos;
        private int MaxPos;

        private enum SDConnectionStates
        {
            Disconnected,
            Connected,
            ConnectedWithTP,
            ConnectedWithRemote,
            InAutoMode
        }
        
        public SetupDialogForm()
        {
            InitializeComponent();
            CurrentPos = 000;
            DesiredPos = 000;
            MaxPos = 0;
        }

        public void ConnectForSetup()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {

                lock (LockObject)
                {
                    DeviceComm.Connect();
                    string[] received = {"", ""};
                    try
                    {
                        received = DeviceComm.GetFVandDT();
                        DeviceSettings.SetFirmwareVersion(received[0]);
                        DeviceSettings.SetDeviceType(received[1]);
                    }
                    catch
                    {
                        //do nothing
                    }
                    //if (received[0] == "" && received[1] = "")
                    //{

                    //}

                    if (DeviceSettings.GetDeviceType() == "?")
                    {
                        MessageBox.Show("Please Select A Device Type First...");
                        DeviceComm.Disconnect();
                        UpdateControls();
                        return;
                    }
                    MaxPos = DeviceSettings.GetMaxStep();
                    int p = DeviceComm.Position;
                    CurrentPos = DesiredPos = p;
                    Pos_TB.Text = p.ToString();
                    double t = DeviceComm.Temperature;
                    Temp_TB.Text = t.ToString() + "°C";
                }

                this.ModeAName_TB.Text = DeviceSettings.GetModeName('A');
                this.ModeBName_TB.Text = DeviceSettings.GetModeName('B');

                char Mode = DeviceSettings.GetActiveMode();
                if (Mode == 'A')
                {
                    ModeA_RB.Checked = true;
                }
                else if (Mode == 'B')
                {
                    ModeB_RB.Checked = true;
                }
                this.backgroundWorkerTemp.RunWorkerAsync();
                this.backgroundWorkerPos.RunWorkerAsync();
                Timer_Temp.Enabled = true;

            }
            catch (Exception Ex)
            {  
                throw new DriverException("Error in ConnectForSetup", Ex);
            }
            finally
            {
                UpdateControls();
                this.Cursor = Cursors.Default;
            }
                
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
                ConnectForSetup();
                UpdateControls();
            }
            catch(Exception Ex)
            {
                if (Ex.InnerException.Message.Contains("ER=1"))
                {
                    MessageBox.Show("Warning: Temperature Probe Not Detected");
                    Timer_Temp.Enabled = false;
                    DeviceSettings.TempProbePresent = false;
                    UpdateControls();
                }
                else
                {
                    //Do nothing here because we still want the form to open even if
                    //if is unable to connect. The exception should be displayed if
                    //connection attempt fails after "Connect" is pressed in the menu.
                }
            }

        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectForSetup();
                DeviceSettings.TempProbePresent = true;
            }
            catch(Exception Ex)
            {
                if (Ex.InnerException.Message.Contains("ER=1"))
                {
                    if (DeviceSettings.TempProbePresent)
                    {
                        UpdateControls();
                    }
                    else
                    {
                        MessageBox.Show("Warning: No temperature probe detected.", "No Temp Probe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DeviceSettings.TempProbePresent = false;
                        UpdateControls();
                    }
                }
                else
                {
                    UpdateControls();
                    DialogResult Result;
                    Result = MessageBox.Show("Could not connect to device.\n" +
                                "This may result from not selecting the correct COM port.\n" + 
                                "Would you like to see the detailed error text?",
                                "Connection Failed", MessageBoxButtons.YesNo);
                    if (Result == DialogResult.Yes)
                        MessageBox.Show("Error Message: \n" + Ex.InnerException.InnerException.Message);
                    else
                    {
                        //don't display anything....
                    }
                }
            }
        }

        internal void UpdateControls()
        {
           
            if (DeviceComm.GetConnectionState())
            {
                //CONNECTED
                ConnectedForControls = true;
                PowerLight.Visible = true;
                StatusLabel.Text = "Connected successfully!";
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
                string DT = DeviceSettings.GetDeviceType();
                DeviceType_CB.Text = DT;
                DeviceType_CB.Enabled = false;
                DeviceType_LB.Enabled = false;

                foreach (Control x in FocStatusControls.Controls)
                {
                    x.Enabled = true; 
                }
                foreach (Control x in TempCompMode_GB.Controls)
                {
                    x.Enabled = true;
                }
                
                if (DeviceSettings.TempProbePresent)
                {
                    Temp_TB.Enabled = true;
                    Temp_LBL.Enabled = true;
                    firstPointToolStripMenuItem.Enabled = true;
                    endPointToolStripMenuItem.Enabled = true;
                }
                else
                {
                    Temp_TB.Enabled = false;
                    Temp_LBL.Enabled = false;
                    firstPointToolStripMenuItem.Enabled = false;
                    endPointToolStripMenuItem.Enabled = false;
                }
                learnToolStripMenuItem.Enabled = true;
            }
            else
            {
                //NOT CONNECTED
                PowerLight.Visible = false;
                ConnectedForControls = false;
                StatusLabel.Text = "Device is not connected";
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
                DeviceType_CB.Text = "Not Connected";
                DeviceType_CB.Enabled = false;
                DeviceType_LB.Enabled = false;

                foreach (Control x in FocStatusControls.Controls)
                {
                    x.Enabled = false;
                    PowerLight.Visible = false;
                }
                foreach (Control x in TempCompMode_GB.Controls)
                {
                    x.Enabled = false;
                }
                learnToolStripMenuItem.Enabled = false;
            }
            DeviceType_LB.ForeColor = System.Drawing.SystemColors.ControlText;
            DeviceType_LB.BackColor = System.Drawing.SystemColors.Control;
            DeviceType_LB.Font = new Font(DeviceType_LB.Font, FontStyle.Regular);
            
            
         

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DeviceComm.Disconnect();
                Timer_Temp.Enabled = false;
                UpdateControls();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                    ConnectForSetup();
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
                    Timer_Temp.Enabled = false;
                    if (!ConnectedForControls)
                    {
                        DeviceComm.Connect();
                    }
                    SetStartPtForm SSPFrm = new SetStartPtForm();
                    SSPFrm.ShowDialog();
                    SSPFrm.Dispose();
                    SSPFrm = null;
                    Timer_Temp.Enabled = true;
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Unable to connect to device. \nHave you selected the right COM port?\n" + 
                    "Exception Data: " + Ex.ToString());

            }
            
        }

        private void endPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lock(LockObject)
                {
                    if (!ConnectedForControls)
                    {
                        DeviceComm.Connect();
                    }
                    Timer_Temp.Enabled = false;
                    SetEndPtForm SEPFrm = new SetEndPtForm();  
                    SEPFrm.ShowDialog();
                    SEPFrm.Dispose();
                    SEPFrm = null;
                    Timer_Temp.Enabled = true;
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
                MessageBox.Show("Names Saved!");
            }
            else
            {
                MessageBox.Show("NOTE: Names will not be saved until you press DONE");
                EditNames_Btn.Text = "DONE";
                ModeAName_TB.ReadOnly = false;
                ModeBName_TB.ReadOnly = false;
            }
        }

        private void backgroundWorkerTemp_DoWork(object sender, DoWorkEventArgs e)
        {
            ///this updates the temperature display
            double temp = 0;
                if (ConnectedForControls && this.Visible)
                {
                    try
                    {
                        lock (LockObject)
                        {
                            temp = DeviceComm.Temperature;
                            this.BeginInvoke(new UpdateTempDisplayHandler(UpdateTempDisplay), new Object[] { temp });
                        }
                    }
                    catch (Exception Ex)
                    {

                        try
                        {
                            if (Ex.InnerException.Message.Contains("ER=1"))
                            {
                                StatusLabel.Text = "Temp Probe Not Found";
                                MessageBox.Show("Warning: Temperature Probe Not Detected.");
                                lock (LockObject)
                                {
                                    this.BeginInvoke(new UpdateTempDisplayHandler(UpdateTempDisplay), new Object[] { -9999 });
                                }
                                Timer_Temp.Enabled = false;
                                DeviceSettings.TempProbePresent = false;
                                UpdateControls();
                            }
                            else
                            {
                                MessageBox.Show("An error occured while trying to communicate with the device.\n" +
                                    "Check the connection cables and try to reconnect");
                                DeviceComm.Disconnect();
                                UpdateControls();
                            }
                        }
                        catch 
                        {
                            
                            
                        }
                    }
                }
        }

        private void UpdateTempDisplay(double temp)
        {
            if (this.Visible)
            {
                if (temp == -9999)
                {
                    this.Temp_TB.Text = "??????";
                }
                else
                {
                    this.Temp_TB.Text = temp.ToString() + "°C";
                }
            }
        }     

        private void backgroundWorkerPos_DoWork(object sender, DoWorkEventArgs e)
        {
            
            if ((ConnectedForControls && this.Visible) && (CurrentPos != DesiredPos))
            {
                try
                {
                    lock (LockObject)
                    {
                        this.BeginInvoke(new UpdatePosDisplayHandler(UpdatePosDisplay), new Object[] { "MOVING"} );
                        DeviceComm.MoveFocus(DesiredPos);
                        CurrentPos = DesiredPos;
                        this.BeginInvoke(new UpdatePosDisplayHandler(UpdatePosDisplay), new Object[]{CurrentPos.ToString()});
                    }
                }
                catch { }
            }
            
        }

        private void UpdatePosDisplay(string pos)
        {
            if (this.Visible)
            {
                this.Pos_TB.Text = pos;
            }
        }

        private void In_BTN_Click(object sender, EventArgs e)
        {
            lock (LockObject)
            {
                DesiredPos = DesiredPos - Convert.ToInt32(Increment_NUD.Value);
                if (DesiredPos < 1) DesiredPos = 1;
            }
            if (!backgroundWorkerPos.IsBusy)
            {
                backgroundWorkerPos.RunWorkerAsync();
            }
            
        }

        private void Out_BTN_Click(object sender, EventArgs e)
        {
            lock (LockObject)
            {
                DesiredPos = DesiredPos + Convert.ToInt32(Increment_NUD.Value);
                if (DesiredPos > MaxPos) DesiredPos = MaxPos;
            }
            if (!backgroundWorkerPos.IsBusy)
            {
                backgroundWorkerPos.RunWorkerAsync();
            }
            
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
            try
            {
                lock (LockObject)
                {
                    Timer_Temp.Enabled = false;
                    if (!ConnectedForControls)
                    {
                        DeviceComm.Connect();
                    }
                    SetSlopeForm SSFrm = new SetSlopeForm();
                    SSFrm.ShowDialog();
                    SSFrm.Dispose();
                    SSFrm = null;
                    Timer_Temp.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Failed to connect to device. \nMake sure a COM port has been selected.");
                return;
            }
        }

        private void displayToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                lock (LockObject)
                {
                    if (!ConnectedForControls)
                    {
                        ConnectForSetup();
                    }
                    DisplayTempCoEffs Frm = new DisplayTempCoEffs();
                    Frm.ShowDialog();
                    UpdateControls();
                }
            }
            catch (Exception)
            {
                
                MessageBox.Show("Failed to connect to device. \nMake sure a COM port has been selected.");
                return;
            }
        }

        private void Timer_Temp_Tick(object sender, EventArgs e)
        
        {
            if (!backgroundWorkerTemp.IsBusy)
            {
                backgroundWorkerTemp.RunWorkerAsync();
                UpdateControls();
            }
        }

        private void DeviceType_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConnectedForControls)
            {
                return;
            }
            if (DeviceType_CB.Text != "Cancel")
            {
                try
                {
                    try
                    {
                        DeviceComm.Connect();
                    }
                    catch
                    {
                        throw new InvalidOperationException("Could not connect to the device");
                    }
                    StatusLabel.Text = "Connected To Device";
                    DeviceSettings.SetDeviceType(DeviceType_CB.Text);
                    StatusLabel.Text = "Device Type Setting Stored In Software";
                    DeviceComm.SetDeviceType();
                    StatusLabel.Text = "Device Type Change Complete!";
                    DeviceComm.Disconnect();
                    MessageBox.Show("You must cycle power to the device in order to finish this process.");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Unable to set device type.\n" + Ex.ToString());
                }
            }

            DeviceType_CB.Enabled = false;
            DeviceType_LB.Enabled = false;
            DeviceType_LB.ForeColor = System.Drawing.SystemColors.ControlText;
            DeviceType_LB.BackColor = System.Drawing.SystemColors.Control;
            DeviceType_LB.Font = new Font(DeviceType_LB.Font, FontStyle.Regular);
           
        }

        private void chooseDeviceTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConnectedForControls)
            {
                MessageBox.Show("You must disconnect from the device before the Device Type can be changed");
                return;
            }
            else
            {
                if (DeviceSettings.PortSelected)
                {
                    StatusLabel.Text = "Select a Device Type";
                    DeviceType_CB.Enabled = true;
                    DeviceType_LB.Enabled = true;
                    DeviceType_LB.ForeColor = Color.Red;
                    DeviceType_LB.BackColor = Color.Blue;
                }
                else
                {
                    MessageBox.Show("You must choose a COM Port first.");
                }
            }
            
        }
     
        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (DeviceSettings.GetDeviceType().Contains("TCF") && DeviceSettings.PortSelected) this.Dispose();
        }

        private void DeviceType_CB_Validating(object sender, CancelEventArgs e)
        {
            if (!DeviceType_CB.Text.Contains("TCF"))
            {
                errorProviderDT.SetError(DeviceType_CB, "Select a device type");
            }
        }

        private void DeviceType_CB_Validated(object sender, EventArgs e)
        {
            errorProviderDT.SetError(DeviceType_CB, "");
        }

        private void Center_Btn_Click(object sender, EventArgs e)
        {
            lock (LockObject)
            {
                DesiredPos = DeviceSettings.GetMaxStep() / 2;
                if (DesiredPos < 1) DesiredPos = 1;
            }
            if (!backgroundWorkerPos.IsBusy)
            {
                backgroundWorkerPos.RunWorkerAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeviceComm.EnterTempCompMode(true);
        }

       
        
    }
}