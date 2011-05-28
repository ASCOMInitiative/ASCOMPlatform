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

        private ArrayList ControlsDisabledOnDisconnect = new ArrayList();

        private enum DisplayStates { Disconected, Connected, NoPower, NotHomed, Errored, Unset }

        private DisplayStates currentState = DisplayStates.Unset;

        private const string NC_msg = "No connected Pyxis LE rotators are connected to the PC";
        private bool LastConnectedState = true;
        private const int SKY_PA_HEIGHT = 43;
        private const int ROTATOR_DIAGRAM_HEIGHT = 230;
        private const int HOME_BUTTON_HEIGHT = 43;
        private const int ABSOLUTE_HEIGHT = 43;
        private const int RELATIVE_HEIGHT = 60;
        private double lastMoveTo = -9999999;
        private Thread MotionMonitorThread;

        #region Form Methods

        public MainForm()
        {
            InitializeComponent();

            ControlsDisabledOnDisconnect.Add(SkyPA_TB);
            ControlsDisabledOnDisconnect.Add(label1);
            ControlsDisabledOnDisconnect.Add(SetPA_BTN);
            ControlsDisabledOnDisconnect.Add(HomeBTN);
            ControlsDisabledOnDisconnect.Add(HomeDev_LBL);
            ControlsDisabledOnDisconnect.Add(RelativeForward_BTN);
            ControlsDisabledOnDisconnect.Add(RelativeReverse_Btn);
            ControlsDisabledOnDisconnect.Add(Relative_NUD);
            ControlsDisabledOnDisconnect.Add(AbsoluteMove_TB);
            ControlsDisabledOnDisconnect.Add(label3);
            ControlsDisabledOnDisconnect.Add(AbsoluteMove_BTN);
            ControlsDisabledOnDisconnect.Add(RelMoveLbl);
            ControlsDisabledOnDisconnect.Add(IncLabel);
            ControlsDisabledOnDisconnect.Add(RotatorDiagram);
            ControlsDisabledOnDisconnect.Add(Park_BTN);
           // ControlsDisabledOnDisconnect.Add(parkToolStripMenuItem);

            
            this.alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop;
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                EventLogger.LoggingLevel = XmlSettings.LoggingLevel;

                RotatorMonitor = new Rotators();
                RotatorMonitor.RotatorAttached += new EventHandler(RotatorListChanged);
                RotatorMonitor.RotatorRemoved += new EventHandler(RotatorListChanged);

                // Set the checked property for the tool strip menu items
                skyPADisplayToolStripMenuItem.Checked = Properties.Settings.Default.ShowSkyPA;
                rotatorDiagramToolStripMenuItem.Checked = Properties.Settings.Default.ShowRotatorDiagram;
                homeButtonToolStripMenuItem.Checked = Properties.Settings.Default.ShowHomeButton;
                absoluteMoveControlsToolStripMenuItem.Checked = Properties.Settings.Default.ShowAbsoluteMove;
                relativeMoveControlsToolStripMenuItem.Checked = Properties.Settings.Default.ShowRelativeMove;
                alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop;

                // set the event handlers for the tool strip menu items
                skyPADisplayToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                rotatorDiagramToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                homeButtonToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                absoluteMoveControlsToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                relativeMoveControlsToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                showAllToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                alwaysOnTopToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItemClicked);
                updateFormSize();

                myRotator = FindMyDevice();

                ThreadStart ts = new ThreadStart(MotionMonitor);
                MotionMonitorThread = new Thread(ts);

                StatusLabel.Text = (myRotator != null) ? "Connected to Pyxis LE with serial number: " + myRotator.SerialNumber :
                    StatusLabel.Text = "Searching for Pyxis LE...";

                updateDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                ExternalControlTimer.Enabled = true;

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

        #endregion

        #region Rotator Event Methods

        void RotatorListChanged(object sender, EventArgs e)
        {
            try
            {
                if (myRotator != null)
                {
                    if (myRotator.IsAttached) return;
                    else myRotator = null;

                }
                else  // myRotator = NULL
                {
                    myRotator = FindMyDevice();
                }
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Form Properties

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

        #endregion

        private void MotionMonitor()
        {
            try
            {
                System.Threading.Thread.Sleep(100);
                if (myRotator == null) return;
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
                this.Invoke(new DelNoParms(DisableHaltBtn));
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void DisableHaltBtn()
        {
            this.Halt_BTN.Enabled = false;
            this.Halt_BTN.Font = new Font(Halt_BTN.Font, FontStyle.Regular);
            this.Halt_BTN.BackColor = System.Drawing.SystemColors.Control;
            this.haltToolStripMenuItem.Enabled = false;
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
                Trace.WriteLine("Clicked " + newpos.ToString());
                myRotator.CurrentSkyPA = newpos;
                string msg = "Moving rotator to Sky Position Angle " + newpos.ToString("0.00°");
                this.Invoke(new SingleStringDelegate(SetStatusLabelText), new object[] { msg });
                if (MotionMonitorThread.IsAlive) return;
                else
                {
                    Halt_BTN.BackColor = Color.OrangeRed;
                    Halt_BTN.Font = new Font(Halt_BTN.Font, FontStyle.Bold);
                    Halt_BTN.Enabled = true;
                    this.haltToolStripMenuItem.Enabled = true;
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
                if (myRotator == null)
                {

                    return;
                }
                if (myRotator.ErrorState != 0) return;
                if (myRotator.IsHoming == false && myRotator.IsHomed == false) return;
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
                EventLogger.LogMessage(ex);
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
                if (angle == lastMoveTo) return;
                else lastMoveTo = angle;
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
            try
            {
                if (myRotator.IsHoming)
                {
                    MessageBox.Show("Please wait for the home procedure to finish before performing this operation.");
                    return;
                }
                double pos = double.Parse(AbsoluteMove_TB.Text);
                StartAMove(pos);
                if (pos == 360) AbsoluteMove_TB.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            NumericUpDown sndr = Relative_NUD as NumericUpDown;
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
            try
            {
                if (myRotator.IsHoming)
                {
                    MessageBox.Show("Please wait for the home procedure to finish before performing this operation.");
                    return;
                }
                double increment = (double)Relative_NUD.Value;
                if (increment == 0) return;
                double NewPositon = myRotator.CurrentSkyPA + increment;
                if (NewPositon > 360) NewPositon = NewPositon - 360;
                if (NewPositon < 0) NewPositon = NewPositon + 360;
                if (NewPositon == myRotator.CurrentDevicePA) return;
                StartAMove(NewPositon);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void RelativeReverse_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (myRotator.IsHoming)
                {
                    MessageBox.Show("Please wait for the home procedure to finish before performing this operation.");
                    return;
                }
                double increment = (double)Relative_NUD.Value;
                if (increment == 0) return;
                double NewPositon = myRotator.CurrentSkyPA - increment;
                if (NewPositon > 360) NewPositon = NewPositon - 360;
                if (NewPositon < 0) NewPositon = NewPositon + 360;
                StartAMove(NewPositon);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void HomeBTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (myRotator == null) throw new ApplicationException("No Rotator connected");
                if (myRotator.ErrorState == 1) throw new ApplicationException("A Home procedure cannot be performed at this time. The device does not have 12VDC power.");
                if(myRotator.IsMoving) myRotator.Halt_Move();
                System.Threading.Thread.Sleep(100);
                if (myRotator.ErrorState != 0) myRotator.ClearErrorState();
                myRotator.Home();
                StatusLabel.Text = "Homing device...";
                MotionMonitor();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
               // throw;
            }
        }

        private void SetPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (myRotator == null) throw new ApplicationException("Rotator is not connected");
                else if (myRotator.ErrorState == 1) throw new ApplicationException("12VDC Power is disconnected. Operation can not be processed.");
                else if (myRotator.ErrorState != 0) throw new ApplicationException(myRotator.GetErrorMessage(myRotator.ErrorState));
                else if (!myRotator.IsHomed) throw new ApplicationException("Please Home the rotator first.");
                else
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
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
        }

#endregion

#region ToolStripMenu Clicks

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotatorMonitor = null;
            this.Close();
        }

        private void ViewToolStripMenuItemClicked(object sender, EventArgs e)
        {
            if(sender.GetType() != typeof(ToolStripMenuItem)) return;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Tag.ToString())
            {
                case "SkyPADisplay":
                    Properties.Settings.Default.ShowSkyPA = item.Checked;
                    break;
                case "RotatorDiagram":
                    Properties.Settings.Default.ShowRotatorDiagram = item.Checked;
                    break;
                case "HomeButton":
                    Properties.Settings.Default.ShowHomeButton = item.Checked;
                    break;
                case "AbsoluteMoveControls":
                    Properties.Settings.Default.ShowAbsoluteMove = item.Checked;
                    break;
                case "RelativeMoveControls":
                    Properties.Settings.Default.ShowRelativeMove = item.Checked;
                    break;
                case "ShowAll":

                    Properties.Settings.Default.ShowSkyPA = true;
                    Properties.Settings.Default.ShowRotatorDiagram = true;
                    Properties.Settings.Default.ShowHomeButton = true;
                    Properties.Settings.Default.ShowAbsoluteMove = true;
                    Properties.Settings.Default.ShowRelativeMove = true;

                    skyPADisplayToolStripMenuItem.Checked = true;
                    rotatorDiagramToolStripMenuItem.Checked = true;
                    homeButtonToolStripMenuItem.Checked = true;
                    absoluteMoveControlsToolStripMenuItem.Checked = true;
                    relativeMoveControlsToolStripMenuItem.Checked = true;


                    break;
                case "AlwaysOnTop":
                    Properties.Settings.Default.AlwaysOnTop = item.Checked;
                    break;
                default: throw new ApplicationException("Invalid menu item clicked.");
            }
            Properties.Settings.Default.Save();
            updateDisplayNoInvoke();

        }

        private void advancedSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (myRotator != null)
                {
                    if (myRotator.ErrorState == 1)
                    {
                        throw new ApplicationException("The requested action cannot be processed. Please connect 12VDC to the rotator device.");
                    }
                    else if (!myRotator.IsHomed)
                    {
                        throw new ApplicationException("The device must be homed before the requested action can be processed.");
                    }
                    else if (myRotator.ErrorState != 0)
                    {
                         throw new ApplicationException(myRotator.GetErrorMessage(myRotator.ErrorState));
                    }
                    AdvancedForm frm = new AdvancedForm(myRotator);
                    frm.ShowDialog();
                    frm.Dispose();
                    updateDisplayNoInvoke();
                    MotionMonitor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }

        private void UpdateDisplay()
        {
            if (this.InvokeRequired)
                this.Invoke(new DelNoParms(updateDisplayNoInvoke));
            else updateDisplayNoInvoke();
        }

        private void updateDisplayNoInvoke()
        {
            updateFormSize();

            DisplayStates lastDisplayState = currentState;

            // First set the form state
            if (myRotator == null)
                currentState = DisplayStates.Disconected;
            else if (myRotator.ErrorState == 1)
                currentState = DisplayStates.NoPower;
            else if (myRotator.ErrorState != 0)
                currentState = DisplayStates.Errored;
            else if (myRotator.ErrorState == 0)
            {
                if (myRotator.IsHomed || myRotator.IsHoming)
                    currentState = DisplayStates.Connected;
                else 
                    currentState = DisplayStates.NotHomed;
            }

            // Set the display based on the state
            switch (currentState)
            {
                case DisplayStates.Connected:
                    if (lastDisplayState != DisplayStates.Connected)
                    {
                        RotatorDiagram.Visible = true;
                        foreach (Control x in this.ControlsDisabledOnDisconnect)
                        {
                            x.Enabled = true;
                        }
                    }
                    this.RotatorDiagram.Image = (myRotator.Reverse) ?
                            Properties.Resources.Rotator_REV : Properties.Resources.Rotator_FWD;
                    if (myRotator.IsMoving) this.StatusLabel.Text = "Rotating...";
                    else if (myRotator.IsHoming) this.StatusLabel.Text = "Homing...";
                    else this.StatusLabel.Text = STATUS_IDLE_MESSAGE;
                    break;

                case DisplayStates.Disconected:
                    if (lastDisplayState != DisplayStates.Disconected)
                    {
                        foreach (Control x in this.ControlsDisabledOnDisconnect)
                        {
                            x.Enabled = false;
                        }
                        this.StatusLabel.Text = "No Rotator Found";

                    }
                    RotatorDiagram.Visible = false;
                    break;

                case DisplayStates.Errored:
                    if (lastDisplayState != DisplayStates.Errored)
                    {
                        RotatorDiagram.Visible = true;
                        this.RotatorDiagram.Image = Properties.Resources.Rotator_ERROR;
                        foreach (Control x in this.ControlsDisabledOnDisconnect)
                        {
                            x.Enabled = false;
                        }
                    }
                    this.StatusLabel.Text = myRotator.GetErrorMessage(myRotator.ErrorState);
                    break;

                case DisplayStates.NoPower:
                    if (lastDisplayState != DisplayStates.NoPower)
                    {
                        RotatorDiagram.Visible = true;
                        this.RotatorDiagram.Image = Properties.Resources.Rotator_ERROR;
                        foreach (Control x in this.ControlsDisabledOnDisconnect)
                        {
                            x.Enabled = false;
                        }
                        this.StatusLabel.Text = "12VDC Power Disconnected!";
                    }
                    break;

                case DisplayStates.NotHomed:
                    if (lastDisplayState != DisplayStates.NotHomed)
                    {
                        RotatorDiagram.Visible = true;
                        this.RotatorDiagram.Image = Properties.Resources.Rotator_ERROR;
                        foreach (Control x in this.ControlsDisabledOnDisconnect)
                        {
                            x.Enabled = false;
                        }
                        HomeBTN.Enabled = true;
                        HomeDev_LBL.Enabled = true;
                        this.StatusLabel.Text = "HOME REQUIRED!";
                    }
                    break;
            }

            // Update the sky pa
            if(myRotator != null) UpdateSkyPATextbox();
            RotatorDiagram.Refresh(); 
        }

        private void updateFormSize()
        {
            int formSize = 95;

            // Adjust for Sky PA Display
            if (Properties.Settings.Default.ShowSkyPA)
            {
                formSize += SKY_PA_HEIGHT;
                tableLayoutPanel1.RowStyles[0].Height = SKY_PA_HEIGHT;
                SkyPAPanel.Visible = true;
            }
            else
            {
                SkyPAPanel.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            // Adjust for Rotator Diagram
            if (Properties.Settings.Default.ShowRotatorDiagram)
            {
                formSize += ROTATOR_DIAGRAM_HEIGHT;
                tableLayoutPanel1.RowStyles[1].Height = ROTATOR_DIAGRAM_HEIGHT;
                RotatorDiagram.Visible = true; RotatorDiagram.Enabled = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[1].Height = 0;
                RotatorDiagram.Visible = false; RotatorDiagram.Enabled = false;
            }

            // Adjust for Home Controls
            if (Properties.Settings.Default.ShowHomeButton)
            {
                formSize += HOME_BUTTON_HEIGHT;
                tableLayoutPanel1.RowStyles[2].Height = HOME_BUTTON_HEIGHT;
                HomePanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[2].Height = 0;
                HomePanel.Visible = false;
            }

            // Adjust for Absolute Move Controls
            if (Properties.Settings.Default.ShowAbsoluteMove)
            {
                formSize += ABSOLUTE_HEIGHT;
                tableLayoutPanel1.RowStyles[3].Height = ABSOLUTE_HEIGHT;
                AbsPanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[3].Height = 0;
                AbsPanel.Visible = false;
            }

            // Adjust for Relative Move Controls
            if (Properties.Settings.Default.ShowRelativeMove)
            {
                formSize += RELATIVE_HEIGHT;
                tableLayoutPanel1.RowStyles[4].Height = RELATIVE_HEIGHT;
                RelativePanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[4].Height = 0;
                RelativePanel.Visible = false;
            }

            // Adjust the main form size
            this.MinimumSize = new Size(314, formSize + 10);
            this.Size = new Size(314, formSize + 10);
            
            Application.DoEvents();
        }

#endregion

#region Status Label Updates

        private const string STATUS_IDLE_MESSAGE = "Pyxis LE - Ready for action!";

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
                UpdateDisplay(); 
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
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

        private void haltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(myRotator!= null && myRotator.IsMoving) myRotator.Halt_Move();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Halt_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (myRotator != null && myRotator.IsMoving)
                {
                    Button b = sender as Button;
                    if (myRotator != null)
                        myRotator.Halt_Move();
                    b.BackColor = System.Drawing.SystemColors.Control;
                    Halt_BTN.Font = new Font(Halt_BTN.Font, FontStyle.Regular);
                    b.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void deviceDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                int i = asmpath.IndexOf("PyxisLE");
                asmpath = asmpath.Substring(0, i + 7);
                string fname = Properties.Settings.Default.HelpFileName;
                asmpath += "\\Documentation\\" + fname;
                //MessageBox.Show(asmpath);
                Process p = new Process();
                p.StartInfo.FileName = asmpath;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Attention");
            }
        }

        private void parkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParkRotator();
        }

        private void Park_BTN_Click(object sender, EventArgs e)
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
                if (MotionMonitorThread.IsAlive) return;
                else
                {
                    ThreadStart ts = new ThreadStart(MotionMonitor);
                    MotionMonitorThread = new Thread(ts);
                    MotionMonitorThread.Start();
                }

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

    }
}
