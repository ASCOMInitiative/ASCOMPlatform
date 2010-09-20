using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Threading;


namespace ASCOM.OptecTCF_S
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private List<Control> TemperatureControls;
        private List<Control> ConnectionDependantControls;
        private static Object LockObject = new Object();
        private static string helpFilePath = "";
        private static bool TempControlsEnabled = true;
        private static int TempDelay = 1000;
        private Thread TempGetterThread;


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
            ConnectionDependantControls.Add(OUT_Btn);
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

                UpdatePositionTextBox();

                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                {
                    ThreadStart ts = new ThreadStart(TempGetter);
                    TempGetterThread = new Thread(ts);
                    TempGetterThread.Start();
                }
                      

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
                DisconnectProcedure();
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
                TempControlsEnabled = false;
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


                if (TempControlsEnabled) EnableTempControls(true);
                else EnableTempControls(false);
                    
                
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
                    UpdatePositionTextBox();
                }
     
 

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
                int DistToMove = Convert.ToInt32(StepSize_NUD.Value);
                int CurrentPos = OptecFocuser.GetPosition();
                if ((CurrentPos + DistToMove) > DeviceSettings.MaxStep)
                {
                    Console.Beep();
                    return; // This means we can't move any further
                }
                else
                {
                    PosDRO_TB.Text = "MOVING";                
                    Application.DoEvents();
                    int newPos = CurrentPos + DistToMove; 
                    OptecFocuser.MoveFocus(newPos);         
                    UpdatePositionTextBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Moving focus OUT.\n " + ex.ToString());
            }


        }

        private void Center_Btn_Click(object sender, EventArgs e)
        {
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
                this.Cursor = Cursors.WaitCursor;
                OptecFocuser.ConnectAndEnterSerialMode();
                EnableDisableControls();
                UpdatePositionTextBox();
                if (!TempGetterThread.IsAlive)
                {
                    ThreadStart ts = new ThreadStart(TempGetter);
                    TempGetterThread = new Thread(ts);
                    TempGetterThread.Start();
                }
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
                System.Threading.Thread.Sleep(150);
                SettingsForm x = new SettingsForm();
                x.ShowDialog();
                this.Cursor = Cursors.Default;
                Debug.WriteLine("Form is closed");
                //update the active temp comp mode RB

                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                {
                    UpdatePositionTextBox();
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

        private void TempGetter()
        {
            while (true)
            {
                try
                {
                    if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                    {
                        System.Threading.Thread.Sleep(TempDelay);
                        DateTime st = DateTime.Now;
                        double t = 0;
                        st = DateTime.Now;
                        
                        t = OptecFocuser.GetTemperature();
                         Debug.Print(st.Subtract(DateTime.Now).TotalSeconds.ToString());
                        this.Invoke(new DoubleDelegate(UpdateTempTextBox), new object[] { t });


                        if (!TempControlsEnabled) EnableTempControls(true);
                        
                    }

                }
                catch (ER1_DeviceException)
                {
                    if (!DeviceSettings.TempProbePresent)
                    {
                        if (TempControlsEnabled)
                        {
                            EnableTempControls(false);
                        }
                    }
                    else
                    {
                        if (TempControlsEnabled)
                        {
                            // This is the first time an error has occurred.
                            MessageBox.Show("The temperature probe is disconnected from the focuser. " +
                                "If you wish to continue reading temperatures plug the probe back into the focuser.");
                            EnableTempControls(false);
                            // Disable the temperature contorls
                        }

                    }
                }
                catch (ER4_DeviceException)
                {
                    if (!DeviceSettings.TempProbePresent) return;
                    else
                    {
                        if (TempControlsEnabled)
                        {
                            // This is the first time an error has occurred.
                            MessageBox.Show("The temperature probe is either disabled in firmware or disconnected from the focuser. " +
                                "If you wish to continue reading temperatures plug the probe back into the focuser and verify that it is enabled.");
                            EnableTempControls(false);
                            // Disable the temperature contorls
                        }
                    }
                }
                catch
                {
                    // Anything else just keep checking
                }
            }


        }

        private delegate void DoubleDelegate(double d);

        private void UpdateTempTextBox(double t)
        {
            if (t > 0) TempDRO_TB.Text = String.Format("{0:00.0}", t) + "°C";
            else TempDRO_TB.Text = String.Format("{0:00.0}", t) + "°";
        }

        private delegate void UpdateFormControlAcrossThread();

        private delegate void BoolDelegate(bool b);

        private void EnableTempControls(bool enable)
        {
            this.Invoke(new BoolDelegate(enableTempControlsInvoked), new object[] {enable});
        }

        private void enableTempControlsInvoked(bool enable)
        {
            if (enable)
            {
                foreach (Control c in TemperatureControls)
                {
                    c.Enabled = true;
                }
                TempControlsEnabled = true;
                TempDelay = 1000;
            }
            else
            {
                foreach (Control c in TemperatureControls)
                {
                    c.Enabled = false;
                }
                TempDRO_TB.Text = "----°C";
                TempControlsEnabled = false;
                TempDelay = 6000;
            }
        }

        private void UpdatePositionTextBox()
        {
            PosDRO_TB.Text = OptecFocuser.GetPosition().ToString().PadLeft(4, '0');      
        }

#endregion Temperature Polling Methods

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisconnectProcedure();
            EnableDisableControls();
        }

        private void DisconnectProcedure()
        {
            TempGetterThread.Abort();
            TempGetterThread.Join();
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
            AboutBox1 AboutBox = new AboutBox1();
            AboutBox.Show();
        }

        private void setStartPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StartPtForm SPF = new StartPtForm();
                SPF.ShowDialog();
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
                EndPointForm EPF = new EndPointForm();
                EPF.ShowDialog();
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
            TempCompTest_Form x = new TempCompTest_Form();
            x.ShowDialog();
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