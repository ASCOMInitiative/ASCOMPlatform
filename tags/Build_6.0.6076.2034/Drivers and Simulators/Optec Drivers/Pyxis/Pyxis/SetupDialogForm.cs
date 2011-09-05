using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Optec;
using System.IO.Ports;
using System.Diagnostics;
using PyxisAPI;

namespace ASCOM.Pyxis
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private bool ExitPositionUpdaterLoop = true;
        private OptecPyxis myPyxis;
        private NewVersionChecker myNewVersionChecker = new NewVersionChecker();

        #region Form Methods

        public SetupDialogForm(OptecPyxis somePyxis)
        {
            InitializeComponent();
            myPyxis = somePyxis;
            myPyxis.MotionStarted += new EventHandler(OptecPyxis_MotionStarted);
            myPyxis.MotionCompleted += new EventHandler(OptecPyxis_MotionCompleted);
            myPyxis.ErrorOccurred += new EventHandler(OptecPyxis_ErrorCodeReceived);
            myPyxis.ConnectionEstablished += new EventHandler(OptecPyxis_ConnectionEstablished);
            myPyxis.ConnectionTerminated += new EventHandler(OptecPyxis_ConnectionTerminated);
            myPyxis.MotionHalted += new EventHandler(OptecPyxis_MotionHalted);

            myNewVersionChecker.NewVersionDetected += new EventHandler(myNewVersionChecker_NewVersionDetected);
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
  
        private void SetupDialogForm_Shown(object sender, EventArgs e)
        {
            try
            {
                UpdateFormConnectionTerminated();

                // Check for new updates
                myNewVersionChecker.CheckForNewVersion(
                    System.Reflection.Assembly.GetExecutingAssembly(),
                    "PyxisPrograms",
                    false,
                    this.Icon
                    );
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        #region Button Clicks / Control Events

        private void cmdOK_Click(object sender, EventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry. No documentation is available at this time.");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Check for new updates
                myNewVersionChecker.CheckForNewVersion(
                    System.Reflection.Assembly.GetExecutingAssembly(),
                    "PyxisPrograms",
                    true,
                    this.Icon
                    );
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
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

        private void Sleep_BTN_Click(object sender, EventArgs e)
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

        private void Wake_BTN_Click(object sender, EventArgs e)
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

        private delegate void DisplayUpdater();

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

        private delegate void PositionUpdater();

        private void UpdatePosition()
        {
            this.CurrentPosition_LBL.Text = myPyxis.CurrentAdjustedPA.ToString("000°");
        }

        private void UpdateFormConnectionEstablished()
        {
            propertyGrid1.SelectedObject = new PyxisPropertyGrid(myPyxis);
            Sleep_BTN.Enabled = true;
            sleepToolStripMenuItem.Enabled = true;
            SetSkyPA_BTN.Enabled = true;
            Park_BTN.Enabled = true;
            parkToolStripMenuItem.Enabled = true;
            Home_Btn.Enabled = true;
            homeToolStripMenuItem.Enabled = true;
            Wake_BTN.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            CurrentPosition_LBL.Text = myPyxis.CurrentAdjustedPA.ToString("000°");
            RelMoveBack_BTN.Enabled = true;
            RelMoveFwd_BTN.Enabled = true;
            RelativeIncrement_NUD.Enabled = true;
            propertyGrid1.Enabled = true;
            propertyGrid1.Refresh();
            GoToAdjustedPA_BTN.Enabled = true;
            AdjustedTargetPA_TB.Enabled = true;
            StatusLabel.Text = "Pyxis Connected and Ready for Action!";
        }

        private void UpdateFormConnectionTerminated()
        {
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            propertyGrid1.SelectedObject = null;
            Sleep_BTN.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            Wake_BTN.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            StatusLabel.Text = "Disconnected";
            propertyGrid1.Enabled = false;
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
            Sleep_BTN.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            Wake_BTN.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            propertyGrid1.Enabled = false;
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
            ExitPositionUpdaterLoop = true;
            Halt_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = true;
            RelMoveBack_BTN.Enabled = true;
            SetSkyPA_BTN.Enabled = true;
            Home_Btn.Enabled = true;
            Park_BTN.Enabled = true;
            Sleep_BTN.Enabled = true;
            RelMoveFwd_BTN.Enabled = true;
            GoToAdjustedPA_BTN.Enabled = true;
            AdjustedTargetPA_TB.Enabled = true;
            propertyGrid1.Enabled = true;
            propertyGrid1.Refresh();
            StatusLabel.Text = "Pyxis Ready for Action!";

        }

        private void UpdateFormMotionHalted()
        {
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false;
            Sleep_BTN.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = true;
            homeToolStripMenuItem.Enabled = true;
            Wake_BTN.Enabled = false;
            wakeToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = true;
            StatusLabel.Text = "Device has been Halted. HOME REQUIRED";
            propertyGrid1.Enabled = false;
            Halt_BTN.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            RelativeIncrement_NUD.Enabled = false;
            GoToAdjustedPA_BTN.Enabled = false;
            AdjustedTargetPA_TB.Enabled = false;
        }

        private void UpdateFormDeviceInSleepMode()
        {
            propertyGrid1.Enabled = false;
            Sleep_BTN.Enabled = false;
            sleepToolStripMenuItem.Enabled = false;
            Park_BTN.Enabled = false;
            parkToolStripMenuItem.Enabled = false;
            Home_Btn.Enabled = false;
            homeToolStripMenuItem.Enabled = false;
            Wake_BTN.Enabled = true;
            wakeToolStripMenuItem.Enabled = true;
            RelativeIncrement_NUD.Enabled = false;
            RelMoveBack_BTN.Enabled = false;
            RelMoveFwd_BTN.Enabled = false;
            Halt_BTN.Enabled = false;
            SetSkyPA_BTN.Enabled = false;

            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = false;
            StatusLabel.Text = "shhh... Rotator is Sleeping!";
            propertyGrid1.Refresh();
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
                if (result == DialogResult.Yes)
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
                myPyxis.HomeOnStart = value;
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