using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Reflection;
using Optec;

namespace ASCOM.PyxisLE_ASCOM
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        Rotators RotatorManager;
        private PyxisLE_API.Rotator myRotator;
        private Utilities.Profile myProfile = new Utilities.Profile();
        private string selectedSerialNumber = "0";
        private Thread MotionMonitorThread;
        private ArrayList ControlList = new ArrayList();

        public SetupDialogForm()
        {
      
            InitializeComponent();
            RotatorManager = new Rotators();
            RotatorManager.RotatorAttached += new EventHandler(RotatorManager_DeviceListChanged);
            RotatorManager.RotatorRemoved += new EventHandler(RotatorManager_DeviceListChanged);
            myProfile.DeviceType = "Rotator";
            ThreadStart ts = new ThreadStart(MotionMonitor);
            MotionMonitorThread = new Thread(ts);
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            EventLogger.LoggingLevel = XmlSettings.LoggingLevel;

            ControlList.Add(CurrentPA_LBL);
            ControlList.Add(label2);
            ControlList.Add(label3);
            ControlList.Add(label4);
            ControlList.Add(label5);
            ControlList.Add(textBox2);
            ControlList.Add(SetSkyPA_Btn);
            ControlList.Add(HomeBTN);
            ControlList.Add(RelativeForward_BTN);
            ControlList.Add(RelativeReverse_BTN);
            ControlList.Add(Relative_NUD);
            ControlList.Add(AbsoluteMove_TB);
            ControlList.Add(label3);
            ControlList.Add(AbsoluteMove_BTN);
            ControlList.Add(Park_BTN);

            UpdateDeviceList();



        }

        void RotatorManager_DeviceListChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NoParmDel(UpdateDeviceList));
            }
            else UpdateDeviceList();
        }

        private delegate void NoParmDel();

        private void UpdateDeviceList()
        {
            try
            {
                this.AttachedDevices_CB.DataSource = null;
                this.AttachedDevices_CB.Items.Clear();
                this.AttachedDevices_CB.DataSource = RotatorManager.RotatorList;
                this.AttachedDevices_CB.DisplayMember = "SerialNumber";
                this.AttachedDevices_CB.ValueMember = "SerialNumber";
                this.AttachedDevices_CB.SelectedIndex = 0;
                this.AttachedDevices_CB_SelectionChangeCommitted(this.AttachedDevices_CB, EventArgs.Empty);                
            }
            catch (Exception)
            {
                // An exception gets thrown if no devices attached.
            }

            if (AttachedDevices_CB.Items.Count == 0)
            {
                EnableDisableControls(false);
            }
            else
            {
                EnableDisableControls(true);
                if (AttachedDevices_CB.Items.Count > 1)
                {
                    string PreferredSN = myProfile.GetValue(Rotator.s_csDriverID, "SelectedSerialNumber", "", "0");
                    int i = 0;
                    foreach (PyxisLE_API.Rotator r in AttachedDevices_CB.Items)
                    {
                        if (r.SerialNumber == PreferredSN)
                        {
                            AttachedDevices_CB.SelectedIndex = i;
                            AttachedDevices_CB_SelectionChangeCommitted(AttachedDevices_CB, EventArgs.Empty);
                            return;
                        }
                        i++;
                    }
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (myRotator == null)
            {
                string msg = "A rotator has not been selected! Press Cancel to quit.";
                MessageBox.Show(msg);
                return;
            }
            else if (myRotator.ErrorState != 0)
            {
                string msg = "The selected Rotator has an error state set. See status below.";
                MessageBox.Show(msg);
                return;
            }
            else if (!myRotator.IsHomed)
            {
                string msg = "The selected Rotator is not homed. Please home the device before continuing.";
                MessageBox.Show(msg);
                return;
            }


            RotatorManager = null;
            myProfile.WriteValue(Rotator.s_csDriverID, "SelectedSerialNumber", selectedSerialNumber);
            myProfile.Dispose();
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            myRotator = null;
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

        

        private void EnableDisableControls(bool enable)
        {
            foreach (Control c in ControlList)
            {
                c.Enabled = enable;
            }
        }

        private void AttachedDevices_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox sndr = sender as ComboBox;
            string newSN = sndr.SelectedValue.ToString();

            if ((newSN != "0") && (newSN != ""))
            {
                selectedSerialNumber = newSN;
            }

            try
            {
                myRotator = sndr.SelectedItem as PyxisLE_API.Rotator;
                UpdateSkyPALabel();
                
            }
            catch
            {
            }
        }

        private void HomeBTN_Click(object sender, EventArgs e)
        {
            if (myRotator == null)
            {
                MessageBox.Show("No rotator has been selected");
                return;
            }
            else if (myRotator.ErrorState != 0)
            {
                MessageBox.Show("An error code has been set in the rotators firmware. See status below.");
            }

            myRotator.Home();
            MotionMonitor();
        }

        private void UpdateSkyPALabel()
        {
            this.CurrentPA_LBL.Text = myRotator.CurrentSkyPA.ToString("000.00°");
        }

        private void SetSkyPA_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                checkRotatorReady();
                string show = myProfile.GetValue(Rotator.s_csDriverID, "ShowSetPAWarning",
                        "", (true).ToString());
                if (show == true.ToString())
                {
                    Warning1Form w = new Warning1Form();
                    DialogResult r = w.ShowDialog();
                    if (r != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }


                SetSkyPA_Frm frm = new SetSkyPA_Frm();
                frm.OldPAValue = myRotator.CurrentSkyPA;
                DialogResult result = frm.ShowDialog();
                double NewOffset = 0;
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                    NewOffset = frm.NewPAValue - myRotator.CurrentDevicePA;
                    myRotator.SkyPAOffset = NewOffset;
                    UpdateSkyPALabel();
                    Application.DoEvents();
                }

                frm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void AbsoluteMove_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                checkRotatorReady();
                double pos = double.Parse(AbsoluteMove_TB.Text);
                StartAMove(pos);
                if (pos == 360) AbsoluteMove_TB.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }

        }

        private delegate void DelNoParms();

        private void MotionMonitor()
        {
            System.Threading.Thread.Sleep(100);
   
            while (myRotator.IsMoving || myRotator.IsHoming)
            {
                System.Threading.Thread.Sleep(50);
          
                this.Invoke(new DelNoParms(UpdateSkyPALabel));
                Application.DoEvents();
            }
            this.Invoke(new DelNoParms(UpdateSkyPALabel));
            Application.DoEvents();
        }

        private void StartAMove(double newpos)
        {
            myRotator.CurrentSkyPA = newpos;
            string msg = "Moving rotator to Sky Position Angle " + newpos.ToString("0.00°");
           
            if (MotionMonitorThread.IsAlive) return;
            else
            {
                ThreadStart ts = new ThreadStart(MotionMonitor);
                MotionMonitorThread = new Thread(ts);
                MotionMonitorThread.Start();
            }
        }

        private void AbsoluteMove_TB_Validating(object sender, CancelEventArgs e)
        {
            double NewPA;
            TextBox sndr = sender as TextBox;
            try
            {
                NewPA = double.Parse(sndr.Text);
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

        private void RelativeForward_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                checkRotatorReady();
                double increment = (double)Relative_NUD.Value;
                if (increment == 0) return;
                double NewPositon = myRotator.CurrentSkyPA + increment;
                if (NewPositon > 360) NewPositon = NewPositon - 360;
                if (NewPositon < 0) NewPositon = NewPositon + 360;
                StartAMove(NewPositon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                checkRotatorReady();
                double increment = (double)Relative_NUD.Value;
                if (increment == 0) return;
                double NewPositon = myRotator.CurrentSkyPA - increment;
                if (NewPositon > 360) NewPositon = NewPositon - 360;
                if (NewPositon < 0) NewPositon = NewPositon + 360;
                StartAMove(NewPositon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void checkRotatorReady()
        {
            if (myRotator == null)
            {
                string msg = "A rotator has not been selected. This operation can not be processed.";
                throw new ASCOM.DriverException(msg);
            }
            else if (myRotator.ErrorState != 0)
            {
                string msg = "The selected Rotator has an error state set. See status below.";
                throw new ASCOM.DriverException(msg);
            }
            else if (!myRotator.IsHomed)
            {
                string msg = "The selected Rotator is not homed. Please home the device before continuing.";
                throw new ASCOM.DriverException(msg);
            }
        }

        private void advancedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                checkRotatorReady();
                AdvancedForm frm = new AdvancedForm(myRotator);
                frm.ShowDialog();
                frm.Dispose();
                //EnableControls();
                MotionMonitor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }

        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                int i = asmpath.IndexOf("PyxisLE");
                asmpath = asmpath.Substring(0, i + 7);
                string fname = "Pyxis LE Help.chm";
                asmpath += "\\Documentation\\" + fname;
                //MessageBox.Show(asmpath);
                Process p = new Process();
                p.StartInfo.FileName = asmpath;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
                throw;
            }
        }

        private void StatusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (myRotator == null)
            {
                UpdateStatusLabel("No Rotator Conntected");
            }
            else
            {
                if (myRotator.ErrorState == 1)
                {
                    UpdateStatusLabel("ATTN: 12VDC Disconnected");
                }
                else if (myRotator.ErrorState != 0)
                {
                    UpdateStatusLabel(myRotator.GetErrorMessage(myRotator.ErrorState));
                }
                else if (!myRotator.IsHomed)
                {
                    UpdateStatusLabel("ATTN: HOME REQUIRED");
                }
                else UpdateStatusLabel("Rotator Ready!");
            }
        }

        private delegate void StatusLabelUpdater(string s);

        private void UpdateStatusLabel(string s)
        {
            try
            {
                if (this != null)
                    this.Invoke(new StatusLabelUpdater(usl), s);
            }
            catch { }
            
        }

        private void usl(string s)
        {
            this.StatusLabel.Text = s;
            Application.DoEvents();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StatusCheckTimer.Enabled = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abb = new AboutBox1();
            abb.ShowDialog();
            abb.Dispose();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (VersionCheckerBGWorker.IsBusy)
                {
                    string msg = "The Version Checker is currently busy. Please try again in a moment.";
                    MessageBox.Show(msg);
                }
                else
                {
                    if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.PyxisLE))
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
                        else MessageBox.Show("Congratulations! You have the most recent version of this program!\n" +
                            "This version number is " + asmName.Version.ToString(), "No Update Needed! - V" +
                            asmName.Version.ToString());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to connect to the server www.optecinc.com",
                    "Version Information Unavailable");
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void VersionCheckerBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Check For A newer verison of the driver
                EventLogger.LogMessage("Checking for application updates", TraceLevel.Info);
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.PyxisLE))
                {
                    //Found a VersionNumber, now check if it's newer
                    Assembly asm = Assembly.GetExecutingAssembly();
                    AssemblyName asmName = asm.GetName();
                    NewVersionChecker.CompareToLatestVersion(asmName.Version);
                    if (NewVersionChecker.NewerVersionAvailable)
                    {
                        EventLogger.LogMessage("This application is NOT the most recent version available", TraceLevel.Warning);
                        NewVersionFrm nvf = new NewVersionFrm(asmName.Version.ToString(),
                            NewVersionChecker.NewerVersionNumber, NewVersionChecker.NewerVersionURL);
                        nvf.ShowDialog();
                    }
                    else
                    {
                        EventLogger.LogMessage("This application is the most recent version available", TraceLevel.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            } // Just ignore all errors. They mean the computer isn't connected to internet.
        }

        private void Park_BTN_Click(object sender, EventArgs e)
        {
            ParkRotator();
        }

        private void parkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParkRotator();
        }

        private void ParkRotator()
        {
            try
            {
                if (myRotator == null)
                {
                    return;
                }
                else if (myRotator.IsMoving || myRotator.IsHoming)
                {
                    MessageBox.Show("Please wait until device has finished its current operation.", "Device Busy");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                myRotator.CurrentDevicePA = (double)XmlSettings.ParkPosition;
                while (myRotator.IsMoving)
                {
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetupDialogForm_Shown(object sender, EventArgs e)
        {
            // Check for updates now
            try
            {
                if (XmlSettings.CheckForUpdates)
                {
                    VersionCheckerBGWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void AbsoluteMove_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                AbsoluteMove_BTN_Click(this.AbsoluteMove_BTN, EventArgs.Empty);
        }
    }
}