using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;


namespace ASCOM.OptecTCF_S
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private List<Control> TemperatureControls;
        private List<Control> ConnectionDependantControls;
        private static Object LockObject = new Object();
        private static string helpFilePath = "";

        public SetupDialogForm()
        {
            InitializeComponent();

            //Setup the lists for manipulating controls

            TemperatureControls = new List<Control>();
            TemperatureControls.Add(TempDRO_TB);
            TemperatureControls.Add(Temp_LBL);
            TemperatureControls.Add(TempCompMode_GB);

            ConnectionDependantControls = new List<Control>();
            ConnectionDependantControls.Add(Temp_LBL);
            ConnectionDependantControls.Add(TempDRO_TB);
            ConnectionDependantControls.Add(PosDRO_TB);
            ConnectionDependantControls.Add(Pos_LBL);
            ConnectionDependantControls.Add(In_BTN);
            ConnectionDependantControls.Add(Out_BTN);
            ConnectionDependantControls.Add(Center_Btn);
            ConnectionDependantControls.Add(StepSize_LBL);
            ConnectionDependantControls.Add(StepSize_NUD);
            ConnectionDependantControls.Add(TempDRO_TB);
            ConnectionDependantControls.Add(Temp_LBL);
            ConnectionDependantControls.Add(TempCompMode_GB);
            ConnectionDependantControls.Add(FocStatusControls_GB);

            Application.EnableVisualStyles();
            helpFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            if (helpFilePath.Contains("(x86)")) helpFilePath = helpFilePath.Substring(0, helpFilePath.Length - 6);
            helpFilePath += "\\Optec\\TCF-S\\Help Files\\TCFSHelp.chm";
            helpProvider1.HelpNamespace = helpFilePath;
            helpProvider1.SetShowHelp(this, true);
           
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            
            try
            {
                OptecFocuser.ConnectAndEnterSerialMode();
                if (DeviceSettings.ActiveTempCompMode == 'A') ModeA_RB.Checked = true;
                else ModeB_RB.Checked = true;
                updateFormPosition();
                if (DeviceSettings.TempProbePresent && (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)) 
                    PollingTemps = true;

            }
            catch (Exception)
            {
                //TODO: Add a statement here to update status label,
                string cap = "Could not connect to device\n Set the correct COM port here...";
                //ConnectionToolTip.SetToolTip(pictureBox2, cap);
                ConnectionToolTip.Show(cap, this, 5, 5, 5000);

            }
            finally
            {
                EnableDisableControls();
                Cursor = Cursors.Default;
                VersionCheckerBGWorker.RunWorkerAsync();
            }

        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisconnectProctedure();
            }
            catch { }
            finally
            {
                this.Dispose();
            }
        }

        private void EnableDisableControls()
        {
            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.Disconnected)
            {
                foreach (Control x in ConnectionDependantControls)
                {
                    x.Enabled = false;
                    x.Visible = true;
                }
                PowerLight.Visible = false;
                connectToolStripMenuItem.Enabled = true;
                EnterSleepModeToolStripMenuItem.Enabled = false;
                exitSleepModeToolStripMenuItem.Enabled = false;
                Sleeping_TB.Visible = false;
            }
            else
            {
                foreach (Control x in ConnectionDependantControls)
                {
                    x.Visible = true;
                    x.Enabled = true;
                }
                connectToolStripMenuItem.Enabled = false;
                PowerLight.Visible = true;
                if (!DeviceSettings.TempProbePresent)
                {
                    foreach (Control x in TemperatureControls)
                    {
                        x.Enabled = false;
                        x.Visible = true;
                    }
                }
                ModeAName_LB.Text = DeviceSettings.ModeA_Name;
                ModeBName_LB.Text = DeviceSettings.ModeB_Name;
                if (DeviceSettings.ActiveTempCompMode == 'A')
                {
                    ModeA_RB.Checked = true;
                }
                else ModeB_RB.Checked = true;
                exitSleepModeToolStripMenuItem.Enabled = false;
                EnterSleepModeToolStripMenuItem.Enabled = true;
                setupToolStripMenuItem.Enabled = true;
                Sleeping_TB.Visible = false;
            }
            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.Sleeping)
            {
                Sleeping_TB.Visible = true;
                exitSleepModeToolStripMenuItem.Enabled = true;
                EnterSleepModeToolStripMenuItem.Enabled = false;
                setupToolStripMenuItem.Enabled = false;
                foreach (Control x in ConnectionDependantControls)
                    x.Visible = false;
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void In_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                bool wasPollingTemps = false;
                if (PollingTemps)
                {
                    wasPollingTemps = true;
                    PollingTemps = false;
                }
                int DistToMove = Convert.ToInt32(StepSize_NUD.Value);
                int CurrentPos = OptecFocuser.GetPosition();
                if ((CurrentPos - DistToMove) < 1)
                {
                    Console.Beep();
                    return; // This means we can't move any further
                }
                else
                {
                    PosDRO_TB.Text = "MOVING";
                    Application.DoEvents();
                    int newPos = CurrentPos - DistToMove;
                    OptecFocuser.MoveFocus(newPos);
                    updateFormPosition();
                }
                if (wasPollingTemps) PollingTemps = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Moving focus IN.\n " + ex.ToString());
            }
        }

        private void Out_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                bool wasPollingTemps = false;
                if (PollingTemps)
                {
                    wasPollingTemps = true;
                    PollingTemps = false;
                }
                int DistToMove = Convert.ToInt32(StepSize_NUD.Value);
                int CurrentPos = OptecFocuser.GetPosition();
                if ((CurrentPos + DistToMove) > DeviceSettings.MaxStep)
                {
                    System.Media.SystemSounds.Beep.Play();
                    return; // This means we can't move any further
                }
                else
                {
                    PosDRO_TB.Text = "MOVING";
                    Application.DoEvents();
                    int newPos = CurrentPos + DistToMove;
                    OptecFocuser.MoveFocus(newPos);
                    updateFormPosition();
                }
                if (wasPollingTemps) PollingTemps = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Moving focus OUT.\n " + ex.ToString());
            }
        }

        private void Center_Btn_Click(object sender, EventArgs e)
        {
            bool wasPollingTemps = false;
            if (PollingTemps)
            {
                PollingTemps = false;
                wasPollingTemps = true;
            }
            int currentPos = OptecFocuser.GetPosition();
            int newPos = DeviceSettings.MaxStep / 2;
            int disttomove = currentPos - newPos;
            if (newPos.Equals(currentPos)) { /* Do nothing*/ }
            else
            {
                //Display MOVING
                PosDRO_TB.Text = "MOVING";
                Application.DoEvents();
                //Move focuser to center
                OptecFocuser.MoveFocus(newPos);
                //Update Form
                PosDRO_TB.Text = newPos.ToString().PadLeft(4, '0');
            }

            //Play a sound to let you know it's centered
            System.Media.SystemSounds.Asterisk.Play();
            if (wasPollingTemps) PollingTemps = true;
        }

        private void ModeA_RB_Click(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            RB.Checked = true;
            DeviceSettings.ActiveTempCompMode = 'A';
        }

        private void ModeB_RB_Click(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            RB.Checked = true;
            DeviceSettings.ActiveTempCompMode = 'B';
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

        private void BrowseToOptec(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.optecinc.com");
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

        private void temperatureCoefficientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowHelpFile("9");
        }

        private void ShowHelpFile()
        {
            //Help.ShowHelp(this, "TCFSHelp5100.chm");
            try
            {
                Help.ShowHelp(this, helpFilePath);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowHelpFile(string topicid)
        {
            //Help.ShowHelp(this, "TCFSHelp5100.chm");
            try
            {
                Help.ShowHelp(this, helpFilePath, HelpNavigator.TopicId, topicid);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PollingTemps) PollingTemps = false;
                this.Cursor = Cursors.WaitCursor;
                OptecFocuser.ConnectAndEnterSerialMode();
                EnableDisableControls();
                updateFormPosition();
                if (DeviceSettings.TempProbePresent) PollingTemps = true;
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Unable To Connect To Device. No Response Was Received.\n" +
                "Have you selected the correct COM port?\n" +
                "Is the serial cable plugged into the device?");
            }
            catch (Exception)
            {
                MessageBox.Show("An Unexpected Error Occured While Attempting To Connect.\n" +
                    "Is another program using the COM port?");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void deviceSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                StatusTimer.Enabled = false;
                if (PollingTemps) PollingTemps = false;
                SettingsForm x = new SettingsForm();
                x.ShowDialog();
                this.Cursor = Cursors.Default;
                Debug.WriteLine("Form is closed");
                //update the active temp comp mode RB

                if ((OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode) &&
                    DeviceSettings.TempProbePresent)
                {
                    Debug.WriteLine("Start Polling Temps");
                    PollingTemps = true;
                    Debug.WriteLine("Started Polling Temps");
                }
                else
                {
                    Debug.WriteLine(OptecFocuser.ConnectionState.ToString());
                    Debug.WriteLine(DeviceSettings.TempProbePresent.ToString());
                }

                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                {
                    updateFormPosition();
                }
                EnableDisableControls();
                StatusTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                EnableDisableControls();
            }
        }

#region Temperature Polling Methods

        private bool PollingTemps
       {
           get 
           {
               return (TempBGWorker.IsBusy && GetTemps && (!StoppedGettingTemps));
           }
           set
           {
               if (value)
               {
                   if (DeviceSettings.TempProbePresent == false)
                       throw new ASCOM.InvalidOperationException("Cannot poll for temperature when temp probe is disabled");
                   GetTemps = true;
                   StoppedGettingTemps = false;
                   if (!TempBGWorker.IsBusy) TempBGWorker.RunWorkerAsync();
                   else throw new ASCOM.InvalidOperationException("Attempted to get temps while background worker is already doing work");
               }
               else
               {
                   TempBGWorker.CancelAsync();
                   GetTemps = false;
                   while (!StoppedGettingTemps)
                   {
                       Application.DoEvents();
                   }
               }
           }
       }

        private void TempBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (GetTemps)
                {
                    if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                    {
                        double t = 0;
                        t = OptecFocuser.GetTemperature();
                        TempBGWorker.ReportProgress(1, t);
                    }
                }
            }
 
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("ER=1"))
                {
                    GetTemps = false;
                    StoppedGettingTemps = true;
                    MessageBox.Show("Temperature Probe Disconnected\n" + 
                    "Disconnect and reconnect to begin polling temperatures again.", "Temperature Probe Disconnected",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Invoke(new UpdateFormControlAcrossThread(ClearTempBox));

                }
                else
                {
                    //Disconnect the device and Enable/Disable the controls

                    Debug.WriteLine("An error occurred while getting temps" + ex.ToString());
                    
                    MessageBox.Show("Communication with the device has been broken\n" + 
                        "Check the cables and attempt to reconnect.", "Communication Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    GetTemps = false;
                    StoppedGettingTemps = true;
                    OptecFocuser.Disconnect();
                    this.Invoke(new UpdateFormControlAcrossThread(EnableDisableControls));
                }
            }

        }

        private delegate void UpdateFormControlAcrossThread();
        private void ClearTempBox()
        {
            TempDRO_TB.Text = "----°C";
        }

        private void TempBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                double temp = Convert.ToDouble(e.UserState);
                if (temp > 0) TempDRO_TB.Text = String.Format("{0:00.0}", temp) + "°C";
                else TempDRO_TB.Text = String.Format("{0:00.0}", temp) + "°";
            }
            catch { PollingTemps = false; }   //This occurs when the form has been closed

        }

        private void TempBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StoppedGettingTemps = true;
            
        }

        private void updateFormPosition()
        {
            if (PollingTemps)
            {
                PollingTemps = false;
                PosDRO_TB.Text = OptecFocuser.GetPosition().ToString().PadLeft(4, '0') ;
                PollingTemps = true;
            }
            else
            {
                PosDRO_TB.Text = OptecFocuser.GetPosition().ToString().PadLeft(4, '0') ;
            }
        }

#endregion Temperature Polling Methods

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisconnectProctedure();
            EnableDisableControls();
        }

        private void DisconnectProctedure()
        {
            if(PollingTemps)
                PollingTemps = false;
            OptecFocuser.Disconnect();
        }

#region Status Bar Methods
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            this.StatusLabel.Text = StatusBar.GetNextMessage();
        }

        private void StatusLabel_Click(object sender, EventArgs e)
        {
            this.StatusLabel.Text = StatusBar.GetNextMessage();
        }

        private void StatusLabel_MouseEnter(object sender, EventArgs e)
        {
            StatusTimer.Enabled = false;
        }

        private void StatusLabel_MouseLeave(object sender, EventArgs e)
        {
            StatusTimer.Enabled = true;
        }

        private void StatusLabel_MouseHover(object sender, EventArgs e)
        {
            StatusBarToolTip.AutoPopDelay = 2500;
            StatusBarToolTip.ReshowDelay = 1000;
            StatusBarToolTip.SetToolTip(this.statusStrip1, "Click to see next message");
           
        }

        private void DeviceType_TB_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
            //SendKeys.Send(Keys.Tab.ToString());
        }
 #endregion Status Bar Methods

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool wasPollingTemps = false;
            if (PollingTemps)
            {
                wasPollingTemps = true;
                PollingTemps = false;
            }
            AboutBox1 AboutBox = new AboutBox1();
            AboutBox.Show();
            if (wasPollingTemps) PollingTemps = true;
        }

        private void setStartPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool wasPollingTemps = false;
                if (PollingTemps)
                {
                    wasPollingTemps = true;
                    PollingTemps = false;
                }
                StartPtForm SPF = new StartPtForm();
                SPF.ShowDialog();

                if (wasPollingTemps) PollingTemps = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void setEndPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool wasPollingTemps = false;
                if (PollingTemps)
                {
                    wasPollingTemps = true;
                    PollingTemps = false;
                }
                EndPointForm EPF = new EndPointForm();
                EPF.ShowDialog();

                if (wasPollingTemps) PollingTemps = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void putToSleepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.Sleeping)
            {
                MessageBox.Show("Device Is Already Sleeping!");
            }
            OptecFocuser.EnterSleepMode();
            EnableDisableControls();
            
        }

        private void exitSleepModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.Sleeping)
            {
                MessageBox.Show("Device is not in sleep mode");
            }
            OptecFocuser.WakeUpDevice();
            EnableDisableControls();
        }

        private void Test_Btn_Click(object sender, EventArgs e)
        {
            bool wasPollingTemps = false;

            if (PollingTemps)
            {
                wasPollingTemps = true;
                PollingTemps = false;
            }

            TempCompTest_Form x = new TempCompTest_Form();
            x.ShowDialog();

            if (wasPollingTemps) PollingTemps = true;
        }

        private void VersionCheckerBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Check For A newer verison of the driver
            if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.TCF_S))
            {
                //Found a VersionNumber, now check if it's newer
                Assembly asm = Assembly.GetExecutingAssembly();
                AssemblyName asmName = asm.GetName();
                NewVersionChecker.CompareToLatestVersion(asmName.Version);
                if (NewVersionChecker.NewerVersionAvailable)
                {
                    NewVersionFrm nvf = new NewVersionFrm(asmName.Version.ToString(), 
                        NewVersionChecker.NewerVersionNumber, NewVersionChecker.NewerVersionURL);
                    nvf.ShowDialog();
                }
            }
        }

        private void driverDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowHelpFile();
                //Used for displaying an HTML help file...
                //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //path += "\\HelpFiles_OptecTCFS_5100\\TCFHelp.htm";
                //MessageBox.Show(path);
                //System.Diagnostics.Process.Start(path);
            }

            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void SetupDialogForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpFile();
        }

      

        

    }

    class StatusBar
    {
        private static int CurrentMessage = 0;

        public static string GetNextMessage()
        {
            string MsgToPost = "";
            switch (CurrentMessage)
            {
                case 0:
                    MsgToPost = "Link State:   " + GetLinkState();
                    break;
                case 1:
                    MsgToPost = "COM Port:   " + GetComPort();
                    break;
                case 2: 
                    MsgToPost = "Temp Comp Mode:   " + GetActiveMode();
                    break;
                case 3:
                    MsgToPost = "Probe Enabled:   " + GetProbeEnabled();
                    break;
                case 4:
                    MsgToPost = "Device Type:   " + GetDeviceType();
                    break;
            }
            if (CurrentMessage == 4) CurrentMessage = 0;
            else CurrentMessage++;
            return MsgToPost;
        }
        
        private static string GetLinkState()
        {
            try
            {
                return OptecFocuser.ConnectionState.ToString();
            }
            catch
            {
                return "Not Available";
            }
        }

        private static string GetComPort()
        {
            try
            {
                return DeviceSettings.COMPort.ToString();
            }
            catch
            {
                return "Not Available";
            }
        }

        private static string GetActiveMode()
        {
            try
            {
                return DeviceSettings.ActiveTempCompMode.ToString();
            }
            catch
            {
                return "Not Available";
            }
        }

        private static string GetProbeEnabled()
        {
            try
            {
                return DeviceSettings.TempProbePresent.ToString();
            }
            catch
            {
                return "Not Available";
            }
        }

        private static string GetDeviceType()
        {
            try
            {
                return DeviceSettings.DeviceType.ToString().Replace('_','-');
            }
            catch
            {
                return "Not Available";
            }
        }
    }
}