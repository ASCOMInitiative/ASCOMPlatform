using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optec;
using System.IO.Ports;
using System.Diagnostics;
using PyxisAPI;

namespace Pyxis_Rotator_Control
{
    public partial class MainForm : Form
    {
        private bool ExitPositionUpdaterLoop = true;
        private NewVersionChecker myNewVersionChecker = new NewVersionChecker();
        private OptecPyxis myPyxis = new OptecPyxis();

        #region Form Methods

        public MainForm()
        {
            InitializeComponent();

            myPyxis.MotionStarted += new EventHandler(OptecPyxis_MotionStarted);
            myPyxis.MotionCompleted += new EventHandler(OptecPyxis_MotionCompleted);
            myPyxis.ErrorOccurred += new EventHandler(OptecPyxis_ErrorCodeReceived);
            myPyxis.ConnectionEstablished += new EventHandler(OptecPyxis_ConnectionEstablished);
            myPyxis.ConnectionTerminated += new EventHandler(OptecPyxis_ConnectionTerminated);
            myPyxis.MotionHalted += new EventHandler(OptecPyxis_MotionHalted);

            myNewVersionChecker.NewVersionDetected += new EventHandler(myNewVersionChecker_NewVersionDetected);

            skyPADisplayToolStripMenuItem.Checked = Properties.Settings.Default.ViewSkyPADisplay;
            rotatorDiagramToolStripMenuItem.Checked = Properties.Settings.Default.ViewRotatorDiagram;
            absoluteMoveControlsToolStripMenuItem.Checked = Properties.Settings.Default.ViewAbsoluteMoveControls;
            relativeMoveControlsToolStripMenuItem.Checked = Properties.Settings.Default.ViewRelativeMoveControls;
            alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop;

            skyPADisplayToolStripMenuItem.CheckedChanged += new EventHandler(ViewSettingItem_CheckedChanged);
            rotatorDiagramToolStripMenuItem.CheckedChanged += new EventHandler(ViewSettingItem_CheckedChanged);
            absoluteMoveControlsToolStripMenuItem.CheckedChanged += new EventHandler(ViewSettingItem_CheckedChanged);
            relativeMoveControlsToolStripMenuItem.CheckedChanged += new EventHandler(ViewSettingItem_CheckedChanged);
            alwaysOnTopToolStripMenuItem.CheckedChanged +=new EventHandler(alwaysOnTopToolStripMenuItem_CheckedChanged);

            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
        }

        void myNewVersionChecker_NewVersionDetected(object sender, EventArgs e)
        {
            try
            {
                Form f = sender as Form;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            myPyxis.ConnectionTerminated -= new EventHandler(OptecPyxis_ConnectionTerminated);
            myPyxis.MotionStarted -= new EventHandler(OptecPyxis_MotionStarted);
            myPyxis.MotionCompleted -= new EventHandler(OptecPyxis_MotionCompleted);
            myPyxis.ErrorOccurred -= new EventHandler(OptecPyxis_ErrorCodeReceived);
            myPyxis.ConnectionEstablished -= new EventHandler(OptecPyxis_ConnectionEstablished);
            myPyxis.MotionHalted -= new EventHandler(OptecPyxis_MotionHalted);

            myNewVersionChecker.NewVersionDetected -= new EventHandler(myNewVersionChecker_NewVersionDetected);

            try
            {
                myPyxis.Disconnect();
            }
            catch { }
            base.OnFormClosing(e);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                UpdateFormConnectionTerminated();
                UpdateFormSize();

                myNewVersionChecker.CheckForNewVersion(System.Reflection.Assembly.GetExecutingAssembly(),
                    "PyxisPrograms",
                    false,
                    Properties.Resources.Rotator);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Button Clicks / Control Events

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.Connect();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void addEditRemoveInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstanceSetupForm frm = new InstanceSetupForm(this.myPyxis);
            frm.ShowDialog();
        }

        private void parkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Park_BTN_Click(this, EventArgs.Empty);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 x = new AboutBox1();
            x.ShowDialog();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                myNewVersionChecker.CheckForNewVersion(
                        System.Reflection.Assembly.GetExecutingAssembly(),
                        "PyxisPrograms",
                        true,
                        Properties.Resources.Rotator);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Error");
            }
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry. No documentation is available at this time.", "No Docs");
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.Disconnect();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void PowerLight_Pic_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (myPyxis.CurrentDeviceState == OptecPyxis.DeviceStates.Disconnected)
                    myPyxis.Connect();
                else
                    myPyxis.Disconnect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cOMPortToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            string currentName = myPyxis.SavedSerialPortName;
            ToolStripMenuItem MainItem = sender as ToolStripMenuItem;
            MainItem.DropDownItems.Clear();
            foreach (string x in SerialPort.GetPortNames())
            {
                ToolStripItem addedItem = MainItem.DropDownItems.Add(x, null, ComPortName_Clicked);
            }

            foreach (ToolStripMenuItem tsmi in MainItem.DropDownItems)
            {
                if (tsmi.Text == currentName)
                {
                    tsmi.Checked = true;
                    break;
                }
            }
        }

        private void ComPortName_Clicked(object sender, EventArgs e)
        {
            if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Disconnected)
            {
                MessageBox.Show("You must disconnect before you can change the COM Port!");
                return;
            }
            ToolStripMenuItem Sender = sender as ToolStripMenuItem;
            myPyxis.SavedSerialPortName = Sender.Text;
        }

        private void instancesToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            string currentName = myPyxis.CurrentInstance;
            ToolStripMenuItem MainItem = sender as ToolStripMenuItem;
            MainItem.DropDownItems.Clear();
            foreach (string x in myPyxis.SettingsInstanceNames)
            {
                ToolStripItem addedItem = MainItem.DropDownItems.Add(x, null, InstanceName_Clicked);
            }

            foreach (ToolStripMenuItem tsmi in MainItem.DropDownItems)
            {
                if (tsmi.Text == currentName)
                {
                    tsmi.Checked = true;
                    break;
                }
            }
        }

        private void InstanceName_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Disconnected)
                {
                    MessageBox.Show("You must disconnect before you can change the Profile Instance!");
                    return;
                }
                ToolStripMenuItem Sender = sender as ToolStripMenuItem;
                myPyxis.CurrentInstance = Sender.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RotatorDiagram_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (myPyxis.CurrentDeviceState == OptecPyxis.DeviceStates.Disconnected) return;
                else if (myPyxis.IsHoming) return;

                PictureBox sndr = sender as PictureBox;
                double center_x = sndr.Size.Width / 2;
                double center_y = sndr.Size.Height / 2;

                double x_zero = 0;
                double y_zero = -((double)center_y * 0.72D);

                double RotationAngle_deg = (double)myPyxis.CurrentAdjustedPA;

                if (myPyxis.Reverse)
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
            if (myPyxis.IsHoming || myPyxis.IsMoving)
            {
                MessageBox.Show(
                 "Please wait until current move/home is finished.");
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

                if (myPyxis.Reverse)
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

                myPyxis.CurrentAdjustedPA = (int)angle;
            }
        }

        private void AdjustedTargetPA_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GoToAdjustedPA_BTN_Click(GoToAdjustedPA_BTN, EventArgs.Empty);
                e.Handled = true;
                AdjustedTargetPA_TB.SelectAll();
            }
        }

        private void AdjustedTargetPA_TB_Enter(object sender, EventArgs e)
        {
            AdjustedTargetPA_TB.SelectAll();
        }

        private void AdjustedTargetPA_TB_Click(object sender, EventArgs e)
        {
            AdjustedTargetPA_TB.SelectAll();
        }

        private void GoToAdjustedPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.CurrentAdjustedPA = (int)double.Parse(this.AdjustedTargetPA_TB.Text);

            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RelMoveFwd_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int increment = (int)RelativeIncrement_NUD.Value;
                int newPos = myPyxis.CurrentAdjustedPA + increment;
                newPos %= 360;
                myPyxis.CurrentAdjustedPA = newPos;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RelMoveBack_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int increment = (int)RelativeIncrement_NUD.Value;
                int newPos = myPyxis.CurrentAdjustedPA - increment;
                newPos %= 360;
                myPyxis.CurrentAdjustedPA = newPos;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Halt_BTN_Click(object sender, EventArgs e)
        {
            if (myPyxis.IsMoving || myPyxis.IsHoming)
            {
                myPyxis.HaltMove();
            }  
        }

        private void Home_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.Home();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void sleepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.PutToSleep();
                UpdateFormDeviceInSleepMode();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void wakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.Connect();
                UpdateFormConnectionEstablished();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void advancedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
                {
                    MessageBox.Show("Pyxis must be connected in order to access its settings");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                while (myPyxis.IsMoving)
                {
                    // Wait until device is not moving...
                }

                AdvancedSettingsForm frm = new AdvancedSettingsForm(myPyxis);
                frm.ShowDialog();

                // Update the rotator diagram incase it changed...
                if (myPyxis.Reverse) RotatorDiagram.Image = Properties.Resources.Rotator_REV;
                else RotatorDiagram.Image = Properties.Resources.Rotator_FWD;

                UpdatePosition();
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

        private void SetSkyPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
                {
                    MessageBox.Show("The device must be in the connected state and not moving or homing to perform this action.");
                    return;
                }
                SetSkyPAForm frm = new SetSkyPAForm();
                DialogResult result = frm.ShowDialog();
                double newPA = frm.PA;
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //Set the new offset...
                    myPyxis.RedefineSkyPAOffset(newPA);

                    // Update the form controls
                    UpdateFormMotionCompleted();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Park_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.ParkRotator();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message, "Attention");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void ViewSettingItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ViewSkyPADisplay = skyPADisplayToolStripMenuItem.Checked;
            Properties.Settings.Default.ViewRotatorDiagram = rotatorDiagramToolStripMenuItem.Checked;
            Properties.Settings.Default.ViewAbsoluteMoveControls = absoluteMoveControlsToolStripMenuItem.Checked;
            Properties.Settings.Default.ViewRelativeMoveControls = relativeMoveControlsToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
            UpdateFormSize();
        }

        private void alwaysOnTopToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AlwaysOnTop = alwaysOnTopToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skyPADisplayToolStripMenuItem.Checked = true;
            rotatorDiagramToolStripMenuItem.Checked = true;
            absoluteMoveControlsToolStripMenuItem.Checked = true;
            relativeMoveControlsToolStripMenuItem.Checked = true;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region OptecPyxis Event Handlers

        void OptecPyxis_MotionHalted(object sender, EventArgs e)
        {
            this.Invoke(new DisplayUpdater(UpdateFormMotionHalted));
        }

        void OptecPyxis_MotionStarted(object sender, EventArgs e)
        {
            this.Invoke(new DisplayUpdater(UpdateFormMotionStarted));
        }

        void OptecPyxis_ConnectionTerminated(object sender, EventArgs e)
        {
            this.Invoke(new DisplayUpdater(UpdateFormConnectionTerminated));
        }

        void OptecPyxis_ConnectionEstablished(object sender, EventArgs e)
        {
            this.Invoke(new DisplayUpdater(UpdateFormConnectionEstablished));
        }

        void OptecPyxis_ErrorCodeReceived(object sender, EventArgs e)
        {
            MessageBox.Show(sender.ToString(), "Error");
        }

        void OptecPyxis_MotionCompleted(object sender, EventArgs e)
        {
            this.Invoke(new DisplayUpdater(UpdateFormMotionCompleted));
        }

        #endregion

        #region Update UI Methods

        private delegate void PositionUpdater();

        private delegate void DisplayUpdater();

        private void UpdatePosition()
        {
            this.CurrentPosition_LBL.Text = myPyxis.CurrentAdjustedPA.ToString("000°");
            RotatorDiagram.Refresh();
        }

        private void UpdateFormSize()
        {
            // Adjust the form size and sizes of tablelayout cells to adjust for viewed objects.
            float skyPADisplayHeight = 55;
            float rotatorDiagramHeight = 244;
            float absoluteControlHeight = 84;
            float relativeControlHeight = 70;

            float formStartHeight = 93;

            if (Properties.Settings.Default.ViewSkyPADisplay)
            {
                tableLayoutPanel1.RowStyles[0].Height = skyPADisplayHeight;
                formStartHeight += skyPADisplayHeight;
                SkyPA_Panel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[0].Height = 0;
                SkyPA_Panel.Visible = false;
            }

            if (Properties.Settings.Default.ViewRotatorDiagram)
            {
                tableLayoutPanel1.RowStyles[1].Height = rotatorDiagramHeight;
                formStartHeight += rotatorDiagramHeight;
                RotatorDiagram_Panel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[1].Height = 0;
                RotatorDiagram_Panel.Visible = false;
            }

            if (Properties.Settings.Default.ViewAbsoluteMoveControls)
            {
                tableLayoutPanel1.RowStyles[2].Height = absoluteControlHeight;
                formStartHeight += absoluteControlHeight;
                AbsoluteMovePanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[2].Height = 0;
                AbsoluteMovePanel.Visible = false;

            }

            if (Properties.Settings.Default.ViewRelativeMoveControls)
            {
                tableLayoutPanel1.RowStyles[3].Height = relativeControlHeight;
                formStartHeight += relativeControlHeight;
                RelativeMovePanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[3].Height = 0;
                RelativeMovePanel.Visible = false;
            }

            this.Height = (int)formStartHeight;
            this.Refresh();
            Application.DoEvents();
        }

        private void UpdateFormConnectionEstablished()
        {
            PowerLight_Pic.Image = Properties.Resources.RedLight;
            if (myPyxis.Reverse) RotatorDiagram.Image = Properties.Resources.Rotator_REV;
            else RotatorDiagram.Image = Properties.Resources.Rotator_FWD;
            sleepToolStripMenuItem.Enabled = true;
            SetSkyPA_BTN.Enabled = true;
            Park_BTN.Enabled = true;
            parkToolStripMenuItem.Enabled = true;
            Home_Btn.Enabled = true;
            homeToolStripMenuItem.Enabled = true;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            CurrentPosition_LBL.Text = myPyxis.CurrentAdjustedPA.ToString("000°");
            RelMoveBack_BTN.Enabled = true;
            RelMoveFwd_BTN.Enabled = true;
            RelativeIncrement_NUD.Enabled = true;
            GoToAdjustedPA_BTN.Enabled = true;
            AdjustedTargetPA_TB.Enabled = true;
            StatusLabel.Text = "Pyxis Connected and Ready for Action!";
            RotatorDiagram.Visible = true;
            RotatorDiagram.Enabled = true;
            RotatorDiagram.Refresh();
        }

        private void UpdateFormConnectionTerminated()
        {
            PowerLight_Pic.Image = Properties.Resources.GreyLight;
            RotatorDiagram.Enabled = false;
            RotatorDiagram.Image = Properties.Resources.Rotator_ERROR;
            RotatorDiagram.Refresh();
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false; 
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            StatusLabel.Text = "Disconnected";
            Halt_BTN.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = false;
            GoToAdjustedPA_BTN.Enabled = false;
            AdjustedTargetPA_TB.Enabled = false;
            CurrentPosition_LBL.Text = "000°";
        }

        private void UpdateFormMotionStarted()
        {
            Halt_BTN.Enabled = true;
            sleepToolStripMenuItem.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = false;
            GoToAdjustedPA_BTN.Enabled = false;
            AdjustedTargetPA_TB.Enabled = false;
            if (myPyxis.IsMoving) StatusLabel.Text = "Moving to PA: " + myPyxis.AdjustedTargetPosition.ToString() + "...";
            else if (myPyxis.IsHoming) StatusLabel.Text = "Pyxis is Homing";
            StartMotionBackgroundWorker();
        }

        private void UpdateFormMotionCompleted()
        {
            if (myPyxis.Reverse) RotatorDiagram.Image = Properties.Resources.Rotator_REV;
            else RotatorDiagram.Image = Properties.Resources.Rotator_FWD;
            RotatorDiagram.Enabled = true;
            ExitPositionUpdaterLoop = true;
            Halt_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = true;
            RelMoveBack_BTN.Enabled = true;
            SetSkyPA_BTN.Enabled = true;
            Home_Btn.Enabled = true;
            Park_BTN.Enabled = true;
            RelMoveFwd_BTN.Enabled = true;
            GoToAdjustedPA_BTN.Enabled = true;
            AdjustedTargetPA_TB.Enabled = true;
            sleepToolStripMenuItem.Enabled = true;
            homeToolStripMenuItem.Enabled = true;
            parkToolStripMenuItem.Enabled = true;
            UpdatePosition();
            StatusLabel.Text = "Pyxis Ready for Action!";
            
        }

        private void UpdateFormMotionHalted()
        {
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = true;
            homeToolStripMenuItem.Enabled = true;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = true;
            RotatorDiagram.Enabled = false;
            RotatorDiagram.Image = Properties.Resources.Rotator_ERROR;
            StatusLabel.Text = "Device has been Halted. HOME REQUIRED";
            Halt_BTN.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = false;
            GoToAdjustedPA_BTN.Enabled = false;
            AdjustedTargetPA_TB.Enabled = false;
        }

        private void UpdateFormDeviceInSleepMode()
        {
            RotatorDiagram.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            wakeToolStripMenuItem.Enabled = true;
            RelativeIncrement_NUD.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = false;
            StatusLabel.Text = "shhh... Rotator is Sleeping!";
        }

        private void StartMotionBackgroundWorker()
        {
            if (this.PositionUpdateBGWorker.IsBusy) return;
            else PositionUpdateBGWorker.RunWorkerAsync();
        }

        private void PositionUpdateBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (myPyxis.IsHoming)
            {
                this.Invoke(new PositionUpdater(UpdatePosition));
                Debug.Print("Background worker exited because device is homing");
                return;
            }
            else
            {
                ExitPositionUpdaterLoop = false;
                DateTime LastPositionChange = DateTime.Now;
                double LastPosition = 0;
                double CurrentPosition = 1;

                // Calculate Maximum time for one degree
                double degreeMaxTime = (1 / myPyxis.SlewRate) * 10;

                while (DateTime.Now.Subtract(LastPositionChange).TotalSeconds < degreeMaxTime)
                {
                    CurrentPosition = myPyxis.CurrentAdjustedPA;
                    if (CurrentPosition != LastPosition)
                    {
                        this.Invoke(new PositionUpdater(UpdatePosition));
                        LastPositionChange = DateTime.Now;
                        LastPosition = CurrentPosition;

                    }
                    if (ExitPositionUpdaterLoop) break;
                }

                Debug.Print("Background worker detected move has finished");
            }
        }

        #endregion

        

    }

    class PyxisPropertyGrid
    {
        private OptecPyxis myPyxis;
        public PyxisPropertyGrid(OptecPyxis somePyxis)
        {
            myPyxis = somePyxis;
            if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
            {
                throw new ApplicationException("You must connect to the Pyxis before you can access its settings.");
            }
        }

        [DisplayName("COM Port")]
        [Category("Device Settings")]
        [Description("The currently selected COM Port")]
        public string PortName
        {
            get
            {
                return myPyxis.SavedSerialPortName;
            }
        }

        [DisplayName("Current Pos. Angle")]
        [Category("Device Properties")]
        [Description("The current position angle of the rotator")]
        public int CurrentPosition
        {
            get
            {
                CheckIfConnected();
                return myPyxis.CurrentAdjustedPA;
            }
        }

        [DisplayName("Reverse")]
        [Category("Device Settings")]
        [Description("Select if the direction for positive moves is reversed or normal.")]
        public bool Reverse
        {
            get
            {
                CheckIfConnected();
                return myPyxis.Reverse;
            }
            set
            {
                try { CheckIfConnected(); }
                catch { return; }

                DialogResult result = MessageBox.Show("The rotator must be at its home position " +
                    "in order to change the reverse property. If the device is not at the home position " +
                    "it will move there automatically. Would you like to continue changing this property?",
                    "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if(result== DialogResult.Yes)
                    myPyxis.Reverse = value;
            }
        }

        [DisplayName("Step Time( msec)")]
        [Category("Device Settings")]
        [Description("Set the delay time between each step of the stepper motor. Note: Shorter delays will reduce torque.")]
        public int StepTime
        {
            get
            {
                CheckIfConnected();
                return myPyxis.StepTime;
            }
            set
            {
                try { CheckIfConnected(); }
                catch { return; }
                myPyxis.SetStepTime(value);
            }
        }

        [DisplayName("Slew Rate (°/sec)")]
        [Category("Device Settings")]
        [Description("The rotation rate of the device based on the device Resolution and Step Time properties")]
        public double SlewRate
        {
            get
            {
                CheckIfConnected();
                return myPyxis.SlewRate;
            }
        }

        [DisplayName("Resolution (steps/Rev)")]
        [Category("Device Settings")]
        [Description("The number of stepper motor steps per one revolution of the rotator")]
        public int Resolution
        {
            get
            {
                CheckIfConnected();
                return myPyxis.Resolution;
            }

        }

        [DisplayName("Device Type")]
        [Category("Device Settings")]
        [Description("Select which type of Pyxis is connected")]
        public OptecPyxis.DeviceTypes DeviceType
        {
            get
            {
                CheckIfConnected();
                return myPyxis.DeviceType;
            }
        }

        [DisplayName("Firmware Version")]
        [Category("Device Properties")]
        [Description("Displays the current firmware version loaded into the connected device")]
        public string FirmwareVersion
        {
            get
            {
                CheckIfConnected();
                return myPyxis.FirmwareVersion;
            }
        }

        [DisplayName("Home On Start")]
        [Category("Device Settings")]
        [Description("Select whether a 3 inch Pyxis will home on start or retain its last position")]
        public bool HomeOnStart
        {
            get
            {
                CheckIfConnected();
                return myPyxis.HomeOnStart;
            }
            set
            {
                try { CheckIfConnected(); }
                catch { return; }
                myPyxis.SetHomeOnStart(value);
            }
        }

        [DisplayName("Park Position (°)")]
        [Category("Application Settings")]
        [Description("Enter the position the rotator will travel to when parking")]
        public int ParkPosition
        {
            get
            {
                return myPyxis.ParkPosition;
            }
            set { myPyxis.ParkPosition = value; }

        }



        private void CheckIfConnected()
        {
            switch (myPyxis.CurrentDeviceState)
            {
                case OptecPyxis.DeviceStates.Connected:
                    return;
                case OptecPyxis.DeviceStates.Disconnected:
                    throw new Exception("Not Connected");
                case OptecPyxis.DeviceStates.Sleep:
                    throw new Exception("Sleeping");
            }
        }
    }
}
