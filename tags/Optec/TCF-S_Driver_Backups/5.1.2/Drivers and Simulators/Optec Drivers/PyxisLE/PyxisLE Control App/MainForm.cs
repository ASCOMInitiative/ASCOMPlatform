using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;
using System.Collections;
using System.Threading;
using Optec;
using System.Diagnostics;
using System.Reflection;


namespace PyxisLE_Control
{
    public partial class MainForm : Form
    {

        private Rotators RotatorMonitor;
        private Rotator myRotator;
        private ArrayList ControlList = new ArrayList();
        private const string NC_msg = "No connected Pyxis LE rotators are connected to the PC";
        private bool LastConnectedState = true;
        private bool LastPowerStateConnected = true;
        private const int NORMAL_HEIGHT = 628;
        private const int SKY_PA_HEIGHT = 43;
        private const int ROTATOR_DIAGRAM_HEIGHT = 315;
        private const int HOME_BUTTON_HEIGHT = 43;
        private const int ABSOLUTE_HEIGHT = 43;
        private const int RELATIVE_HEIGHT = 75;


        private const int SHORT_HEIGHT = 290;
        private Thread MotionMonitorThread;

        public MainForm()
        {
            InitializeComponent();
            EventLogger.LoggingLevel = Properties.Settings.Default.LastTraceLevel;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ControlList.Add(SkyPA_TB);
            ControlList.Add(label1);
            ControlList.Add(SetPA_BTN);
            ControlList.Add(HomeBTN);
            ControlList.Add(HomeDev_LBL);
            ControlList.Add(RelativeForward_BTN);
            ControlList.Add(RelativeReverse_Btn);
            ControlList.Add(Relative_NUD);
            ControlList.Add(AbsoluteMove_TB);
            ControlList.Add(label3);
            ControlList.Add(AbsoluteMove_BTN);
            
            
            RotatorMonitor = new Rotators();
            RotatorMonitor.RotatorAttached += new EventHandler(RotatorListChanged);
            RotatorMonitor.RotatorRemoved += new EventHandler(RotatorListChanged);
            myRotator = FindMyDevice();

            ThreadStart ts = new ThreadStart(MotionMonitor);
            MotionMonitorThread = new Thread(ts);

            if (myRotator != null)
            {
                StatusLabel.Text = "Connected to Pyxis LE with serial number: " + myRotator.SerialNumber;
                EnableControls();
            }
            else
            {
 
                StatusLabel.Text = "Searching for Pyxis LE...";
                DisableControls();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ExternalControlTimer.Enabled = true;
            if (Properties.Settings.Default.CheckForUpdates)
            {
                VersionCheckerBGWorker.RunWorkerAsync();
            }
        }

        void RotatorListChanged(object sender, EventArgs e)
        {
            try
            {
                if (myRotator != null)
                {
                    if (myRotator.IsAttached) return;
                    else
                    {
                        DisableControls();
                        myRotator = null;
                    }
                }
                else  // myRotator = NULL
                {
                    myRotator = FindMyDevice();
                    if (myRotator != null) EnableControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool Connected
        {
            get
            {
                if(LastConnectedState == false)
                {
                    myRotator = FindMyDevice();
                }
                
                if (myRotator == null) return false;
                if (!myRotator.IsAttached)
                {
                    myRotator = null;
                    return false;
                }
                return true;
                
            }
        }

        private void MotionMonitor()
        {
            try
            {
                System.Threading.Thread.Sleep(100);
                string msg = "";
                if (myRotator.IsHoming) msg = "Homing Complete!";
                else if (myRotator.IsMoving) msg = "Move Completed!";
                while (myRotator.IsMoving || myRotator.IsHoming)
                {
                    System.Threading.Thread.Sleep(25);
                    this.Invoke(new DelNoParms(RotatorDiagram.Refresh));
                    this.Invoke(new DelNoParms(UpdateSkyPATextbox));
                    Application.DoEvents();
                }
                this.Invoke(new DelNoParms(RotatorDiagram.Refresh));
                this.Invoke(new DelNoParms(UpdateSkyPATextbox));
                this.Invoke(new SingleStringDelegate(SetStatusLabelText), new object[] { msg });

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void StartAMove(double newpos)
        {
            try
            {
                if (myRotator.ErrorState == 1)
                {
                    EventLogger.LogMessage("Device had Error State = 1 (No Power) when move was attempted.", System.Diagnostics.TraceLevel.Warning);
                    myRotator.ClearErrorState();
                    if (myRotator.ErrorState != 0) throw new ApplicationException("Connect 12V power to Pyxis LE");
                }
                else if (myRotator.ErrorState != 0)
                {
                    throw new ApplicationException("Rotator has firmware error code set: " +
                        myRotator.GetErrorMessage(myRotator.ErrorState));
                }
                myRotator.CurrentSkyPA = newpos;
                string msg = "Moving rotator to Sky Position Angle " + newpos.ToString("0.00°");
                this.Invoke(new SingleStringDelegate(SetStatusLabelText), new object[] { msg });
                if (MotionMonitorThread.IsAlive) return;
                else
                {
                    ThreadStart ts = new ThreadStart(MotionMonitor);
                    MotionMonitorThread = new Thread(ts);
                    MotionMonitorThread.Start();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

#region Enable/Disable Controls Methods

        private void EnableControls()
        {
            try
            {
                this.BeginInvoke(new DelNoParms(enableControls));
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                DisableControls();
                throw;
            }
        }

        private void enableControls()
        {
            try
            {
                StatusLabel.Text = "Connected to Pyxis LE with serial number: " + myRotator.SerialNumber;
                foreach (Control x in ControlList)
                {
                    x.Enabled = true;
                }
                SkyPA_TB.Text = myRotator.CurrentSkyPA.ToString("000.00°");
                Degree_LBL.Visible = true;

                // Set the Forward or Reverse image for the RotatorDiagram.
                if (myRotator.Reverse == false)
                {
                    RotatorDiagram.Image = Properties.Resources.Rotator_FWD;
                }
                else RotatorDiagram.Image = Properties.Resources.Rotator_REV;

                // Adjust form for Show/Hide settings.
                int FormHeight = NORMAL_HEIGHT;

                // SKY PA Area
                if (Properties.Settings.Default.ShowSkyPA)
                {
                    tableLayoutPanel1.RowStyles[0].Height = SKY_PA_HEIGHT;
                    SkyPAPanel.Visible = true;
                }
                else
                {
                    FormHeight -= SKY_PA_HEIGHT;
                    tableLayoutPanel1.RowStyles[0].Height = 0;
                    SkyPAPanel.Visible = false;
                }

                // Rotator Diagram
                if (Properties.Settings.Default.ShowRotatorDiagram)
                {
                    tableLayoutPanel1.RowStyles[1].Height = ROTATOR_DIAGRAM_HEIGHT;
                    RotatorDiagram.Enabled = true;
                    RotatorDiagram.Visible = true;
                }
                else
                {
                    tableLayoutPanel1.RowStyles[1].Height = 0;
                    RotatorDiagram.Enabled = false;
                    RotatorDiagram.Visible = false;
                    FormHeight -= ROTATOR_DIAGRAM_HEIGHT;
                }

                // Home Button and Label
                if (Properties.Settings.Default.ShowHomeButton)
                {
                    tableLayoutPanel1.RowStyles[2].Height = HOME_BUTTON_HEIGHT;
                    HomePanel.Visible = true;
                }
                else
                {
                    FormHeight -= HOME_BUTTON_HEIGHT;
                    tableLayoutPanel1.RowStyles[2].Height = 0;
                    HomePanel.Visible = false;
                }

                // Absolute Move controls
                if (Properties.Settings.Default.ShowAbsoluteMove)
                {
                    tableLayoutPanel1.RowStyles[3].Height = ABSOLUTE_HEIGHT;
                    AbsPanel.Visible = true;
                }
                else
                {
                    FormHeight -= ABSOLUTE_HEIGHT;
                    tableLayoutPanel1.RowStyles[3].Height = 0;
                    AbsPanel.Visible = false;
                }
                // Relative Move Controls.
                if (Properties.Settings.Default.ShowRelativeMove)
                {
                    tableLayoutPanel1.RowStyles[4].Height = RELATIVE_HEIGHT;
                    RelativePanel.Visible = true;
                }
                else
                {
                    FormHeight -= RELATIVE_HEIGHT;
                    tableLayoutPanel1.RowStyles[4].Height = 0;
                    RelativePanel.Visible = false;
                }
                Height = FormHeight;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                disableControls();
            }

        }

        private void DisableControls()
        {
            try
            {
                this.BeginInvoke(new DelNoParms(disableControls));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void disableControls()
        {
            StatusLabel.Text = "Searching for Pyxis LE...";
            foreach (Control x in ControlList)
            {
                x.Enabled = false;
            }
            SkyPA_TB.Text = "";
            Degree_LBL.Visible = false;
            RotatorDiagram.Visible = false;
            RotatorDiagram.Enabled = false;

        }

        private Rotator FindMyDevice()
        {
            Rotator r = null;
            if (RotatorMonitor.RotatorList.Count > 0)
            {
                r = RotatorMonitor.RotatorList[0] as Rotator;
            }
            return r;
        }

        private delegate void DelNoParms();

 #endregion

#region UpdateDisplay Methods

        private void RotatorDiagram_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.ShowRotatorDiagram == false) return;
                if (myRotator == null || myRotator.IsAttached == false) return;
                PictureBox sndr = sender as PictureBox;
                double center_x = sndr.Size.Width / 2;
                double center_y = sndr.Size.Height / 2;

                double x_zero = 0;
                double y_zero = -((double)center_y * 0.72D);

                double RotationAngle_deg = myRotator.CurrentSkyPA;

                if (myRotator.Reverse)
                {
                    if (RotationAngle_deg == 0) { }
                    else if (RotationAngle_deg < 180)
                    {
                        RotationAngle_deg = 180 + (180 - RotationAngle_deg);
                    }
                    else if (RotationAngle_deg > 180)
                    {
                        RotationAngle_deg = 180 - (RotationAngle_deg - 180);
                    }
                }
                double RotationAngle_Rad = RotationAngle_deg * (Math.PI / 180);
                double x_rotated = x_zero * Math.Cos(RotationAngle_Rad) + y_zero * Math.Sin(RotationAngle_Rad);


                double y_rotated = (-(x_zero) * Math.Sin(RotationAngle_Rad)) + y_zero * Math.Cos(RotationAngle_Rad);

                Graphics g = e.Graphics;
                Pen p = new Pen(Color.OrangeRed, 3);
                g.DrawLine(p, new Point((int)center_x - 1, (int)center_y), new Point((int)(x_rotated + center_x - 1), (int)(y_rotated + center_y)));

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void RotatorDiagram_Click(object sender, EventArgs e)
        {
            if (myRotator.IsHoming)
            {
                MessageBox.Show(
                 "Please wait until home procedure is finished.");
                return;
            }
            MouseEventArgs ClickedPt = e as MouseEventArgs;
            double x1 = RotatorDiagram.Size.Width / 2;
            double y1 = RotatorDiagram.Size.Height / 2;
            double x2 = ClickedPt.X;
            double y2 = ClickedPt.Y;
            double dx = x2 - x1;
            double dy = y2 - y1;

            // Make sure it is within the blue border
            double DistFromCenter = Math.Sqrt((Math.Abs(dx) * Math.Abs(dx)) + (Math.Abs(dy) * Math.Abs(dy)));
            if (((DistFromCenter / y1) <= .76) && ((DistFromCenter / y1) >= .45))
            {
                double radians = Math.Atan2(dy, dx);
                double angle = -(radians * (180 / Math.PI) - 180);
                angle = angle + 90;
                if (angle > 360) angle = (angle - 360);

                if (myRotator.Reverse)
                {
                    if (angle == 0) { }
                    else if (angle < 180)
                    {
                        angle = 180 + (180 - angle);
                    }
                    else if (angle > 180)
                    {
                        angle = 180 - (angle - 180);
                    }
                }
                StartAMove(angle);
            }
        }

        private void UpdateSkyPATextbox()
        {
            this.SkyPA_TB.Text = myRotator.CurrentSkyPA.ToString("000.00°");
        }

#endregion

#region Form Button Clicks

        private void AbsoluteMove_BTN_Click(object sender, EventArgs e)
        {

            double pos = double.Parse(AbsoluteMove_TB.Text);
            StartAMove(pos);
            if (pos == 360) AbsoluteMove_TB.Text = "0";
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

        private void AbsoluteMove_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CancelEventArgs c = new CancelEventArgs();
                AbsoluteMove_TB_Validating(AbsoluteMove_TB, c);
                if (c.Cancel == false)
                {
                    AbsoluteMove_BTN_Click(this.AbsoluteMove_BTN, EventArgs.Empty);
                    e.Handled = true;
                }
            }

        }

        private void RelativeIncrement_TB_Validating(object sender, CancelEventArgs e)
        {
            double NewIncrement;
            TextBox sndr = sender as TextBox;
            try
            {
                NewIncrement = double.Parse(sndr.Text);
                errorProvider1.Clear();
                if (NewIncrement >= 360)
                {
                    errorProvider1.SetError(sndr, "Increment must be less than 360°");
                    e.Cancel = true;
                }
                if (NewIncrement < 0)
                {
                    errorProvider1.SetError(sndr, "Increment must be greater than 0");
                    e.Cancel = true;
                }

            }
            catch
            {
                errorProvider1.SetError(sndr, "Must be a number");

            }

        }

        private void RelativeForward_BTN_Click(object sender, EventArgs e)
        {
            double increment = (double)Relative_NUD.Value;
            if (increment == 0) return;
            double NewPositon = myRotator.CurrentSkyPA + increment;
            if (NewPositon > 360) NewPositon = NewPositon - 360;
            if (NewPositon < 0) NewPositon = NewPositon + 360;
            StartAMove(NewPositon);
        }

        private void RelativeReverse_Btn_Click(object sender, EventArgs e)
        {
            double increment = (double)Relative_NUD.Value;
            if (increment == 0) return;
            double NewPositon = myRotator.CurrentSkyPA - increment;
            if (NewPositon > 360) NewPositon = NewPositon - 360;
            if (NewPositon < 0) NewPositon = NewPositon + 360;
            StartAMove(NewPositon);
        }

        private void HomeBTN_Click(object sender, EventArgs e)
        {
            try
            {
                myRotator.Home();
                StatusLabel.Text = "Homing device...";
                MotionMonitor();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void SetPA_BTN_Click(object sender, EventArgs e)
        {
            SetSkyPA_Frm frm = new SetSkyPA_Frm();
            frm.OldPAValue = myRotator.CurrentSkyPA;
            DialogResult result = frm.ShowDialog();
            double NewOffset = 0;
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                NewOffset = frm.NewPAValue - myRotator.CurrentDevicePA;

                myRotator.SkyPAOffset = NewOffset;
                RotatorDiagram.Refresh();
                UpdateSkyPATextbox();
                this.StatusLabel.Text = "Sky PA set to " + frm.NewPAValue.ToString() + "°";
                Application.DoEvents();
            }

            frm.Dispose();
        }

#endregion

#region ToolStripMenu Clicks

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotatorMonitor = null;
            this.Close();
        }

        private void showDeviceDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowRotatorDiagram = !Properties.Settings.Default.ShowRotatorDiagram;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void advancedSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AdvancedForm frm = new AdvancedForm(myRotator);
                frm.ShowDialog();
                frm.Dispose();
                enableControls();
                MotionMonitor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void ShowHideSkyPAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSkyPA = !Properties.Settings.Default.ShowSkyPA;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void showHideHomeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowHomeButton = !Properties.Settings.Default.ShowHomeButton;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void showHideAbsoluteMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowAbsoluteMove = !Properties.Settings.Default.ShowAbsoluteMove;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void showHideRelativeMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowRelativeMove = !Properties.Settings.Default.ShowRelativeMove;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSkyPA = true;
            Properties.Settings.Default.ShowRotatorDiagram = true;
            Properties.Settings.Default.ShowHomeButton = true;
            Properties.Settings.Default.ShowAbsoluteMove = true;
            Properties.Settings.Default.ShowRelativeMove = true;
            Properties.Settings.Default.Save();
            if (Connected) EnableControls();
            else DisableControls();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }


#endregion

#region Status Label Updates

        private const string STATUS_IDLE_MESSAGE = "Pyxis LE - Ready for action!";

        private void StatusLabel_TextChanged(object sender, EventArgs e)
        {
            ToolStripStatusLabel sndr = sender as ToolStripStatusLabel;
            if (sndr.Text != STATUS_IDLE_MESSAGE)
            {
                if (StatusLabelTimer.Enabled)
                {
                    StatusLabelTimer.Stop();
                }
                StatusLabelTimer.Enabled = true;
            }
        }

        private void StatusLabelTimer_Tick(object sender, EventArgs e)
        {

            if (myRotator != null)
            {
                if (myRotator.IsMoving || myRotator.IsHoming)
                {
                    StatusLabelTimer.Enabled = true;
                }
                else
                {
                    this.Invoke(new SingleStringDelegate(SetStatusLabelText), new object[] { STATUS_IDLE_MESSAGE });
                    StatusLabelTimer.Enabled = false;
                } 
            }
        }

        private delegate void SingleStringDelegate(string s);

        private void SetStatusLabelText(string status)
        {
            this.StatusLabel.Text = status;
        }

#endregion

        private void ExternalControlTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (myRotator != null)
                {
                    if (myRotator.IsAttached)
                    {
                        this.Invoke(new DelNoParms(UpdateSkyPATextbox));

                        if (RotatorDiagram.Visible && RotatorDiagram.Enabled)
                        {
                            if (myRotator.ErrorState == 1)
                            {
                                try
                                {
                                    if (LastPowerStateConnected)
                                    {
                                        LastPowerStateConnected = false;
                                        MessageBox.Show("12V Power has been disconnected for the rotator. Please reconnect it now.");
                                        EventLogger.LogMessage("ExtCtrlTmr found Frmwr error code 1 set (No 12V power).", System.Diagnostics.TraceLevel.Warning);
                                        myRotator.ClearErrorState(); 
                                    }
                                    else
                                    {
                                        
                                        myRotator.ClearErrorState();
                                        LastPowerStateConnected = true;
                                        EventLogger.LogMessage("Restoring trace level", TraceLevel.Info);
                                        EventLogger.LoggingLevel = Properties.Settings.Default.LastTraceLevel;
                                        MessageBox.Show("Power has just been restored to the device. If you have a heavy load on the rotator it would be wise to re-home now.");
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                    if (EventLogger.LoggingLevel != TraceLevel.Off)
                                    {
                                        EventLogger.LogMessage("Temporarily disabling logging while waiting for power to be reconnected.", TraceLevel.Info);
                                        EventLogger.LoggingLevel = TraceLevel.Off;
                                    }
                                    
                                }
                            }
                            this.Invoke(new DelNoParms(RotatorDiagram.Refresh));
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExternalControlTimer.Stop();
            ExternalControlTimer.Enabled = false;
        }

        private void VersionCheckerBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
           try
            {
                //Check For A newer verison of the driver
                EventLogger.LogMessage("Checking for application updates", TraceLevel.Info);
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.HSFWControl))
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
                    if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.HSFWControl))
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

    }
}
