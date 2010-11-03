using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Optec;
using System.IO.Ports;

namespace ASCOM.Pyxis
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            this.Activated += new EventHandler(SetupDialogForm_Activated);
        }

        void SetupDialogForm_Activated(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
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

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OptecPyxis.PortName = XMLSettings.SavedSerialPortName;
                OptecPyxis.Connect();
                updateFormDisplayNoInvoke();
                Application.DoEvents();
                while (OptecPyxis.IsMoving)
                {
                    Application.DoEvents();
                }
                updateFormDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
            finally{
            }
        }

        private void updateFormDisplayNoInvoke()
        {
            switch (OptecPyxis.CurrentDeviceState)
            {
                case OptecPyxis.DeviceStates.Connected:
                    Sleep_BTN.Enabled = true;
                    sleepToolStripMenuItem.Enabled = true;
                    Park_BTN.Enabled = true;
                    parkToolStripMenuItem.Enabled = true;
                    Home_Btn.Enabled = true;
                    homeToolStripMenuItem.Enabled = true;
                    Wake_BTN.Enabled = false;
                    wakeToolStripMenuItem.Enabled = false;
                    GoToPA_BTN.Enabled = true;
                    TargetPA_TB.Enabled = true;
                    ChangeDirection_BTN.Enabled = true;


                    connectToolStripMenuItem.Enabled = false;
                    disconnectToolStripMenuItem.Enabled = true;
                    CurrentPosition_LBL.Text = OptecPyxis.CurrentDevicePosition.ToString("000.00°");
                    AdjustedPosition_LBL.Text = OptecPyxis.CurrentAdjustedPA.ToString("000.00°");
                    FirmwareVer_LBL.Text = OptecPyxis.FirmwareVersion;
                    StatusLabel.Text = "Connected and Ready!";

                    break;
                case OptecPyxis.DeviceStates.Disconnected:
                    Sleep_BTN.Enabled = false;
                    sleepToolStripMenuItem.Enabled = false;
                    Park_BTN.Enabled = false;
                    parkToolStripMenuItem.Enabled = false;
                    Home_Btn.Enabled = false;
                    homeToolStripMenuItem.Enabled = false;
                    Wake_BTN.Enabled = false;
                    wakeToolStripMenuItem.Enabled = false;
                    GoToPA_BTN.Enabled = false;
                    TargetPA_TB.Enabled = false;
                    ChangeDirection_BTN.Enabled = false;

                    wakeToolStripMenuItem.Enabled = false;
                    connectToolStripMenuItem.Enabled = true;
                    disconnectToolStripMenuItem.Enabled = false;
                    CurrentPosition_LBL.Text = "?";
                    AdjustedPosition_LBL.Text = "?";
                    StatusLabel.Text = "Disconnected";
                    FirmwareVer_LBL.Text = "??";
                    break;
                case OptecPyxis.DeviceStates.InMotion:
                    Sleep_BTN.Enabled = false;
                    sleepToolStripMenuItem.Enabled = false;
                    Park_BTN.Enabled = false;
                    parkToolStripMenuItem.Enabled = false;
                    Home_Btn.Enabled = false;
                    homeToolStripMenuItem.Enabled = false;
                    Wake_BTN.Enabled = false;
                    wakeToolStripMenuItem.Enabled = false;
                    GoToPA_BTN.Enabled = false;
                    TargetPA_TB.Enabled = false;
                    ChangeDirection_BTN.Enabled = false;

                    connectToolStripMenuItem.Enabled = false;
                    disconnectToolStripMenuItem.Enabled = false;
                    CurrentPosition_LBL.Text = "Moving";
                    AdjustedPosition_LBL.Text = "Moving";
                    StatusLabel.Text = "Device is Moving or Homing";
                    break;
                case OptecPyxis.DeviceStates.Sleep:
                    Sleep_BTN.Enabled = false;
                    sleepToolStripMenuItem.Enabled = false;
                    Park_BTN.Enabled = false;
                    parkToolStripMenuItem.Enabled = false;
                    Home_Btn.Enabled = false;
                    homeToolStripMenuItem.Enabled = false;
                    Wake_BTN.Enabled = true;
                    wakeToolStripMenuItem.Enabled = true;
                    GoToPA_BTN.Enabled = false;
                    TargetPA_TB.Enabled = false;
                    ChangeDirection_BTN.Enabled = false;

                    connectToolStripMenuItem.Enabled = false;
                    disconnectToolStripMenuItem.Enabled = false;
                    StatusLabel.Text = "shhh... Rotator is Sleeping!";
                    break;     
            }
            Application.DoEvents();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OptecPyxis.Disconnect();
                updateFormDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void cOMPortToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem MainItem = sender as ToolStripMenuItem;
            MainItem.DropDownItems.Clear();
            foreach (string x in SerialPort.GetPortNames())
            {
                MainItem.DropDownItems.Add(x, null, ComPortName_Clicked);
            }
        }

        void ComPortName_Clicked(object sender, EventArgs e)
        {
            ToolStripMenuItem Sender = sender as ToolStripMenuItem ;
            XMLSettings.SavedSerialPortName = Sender.Text;
        }

        private void GoToPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OptecPyxis.CurrentDevicePosition = int.Parse(TargetPA_TB.Text);
                updateFormDisplayNoInvoke();
                while (OptecPyxis.IsMoving) { }
                updateFormDisplayNoInvoke();
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
            updateFormDisplayNoInvoke();
        }

        private void Home_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OptecPyxis.Home();
                updateFormDisplayNoInvoke();
                while (OptecPyxis.IsMoving) { }
                updateFormDisplayNoInvoke();
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

        private void Park_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                // Do something to park the device here...

                throw new NotImplementedException("Park Method");
                updateFormDisplayNoInvoke();
                while (OptecPyxis.IsMoving) { }
                updateFormDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Sleep_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                OptecPyxis.PutToSleep();
                updateFormDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Wake_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                OptecPyxis.Connect();
                updateFormDisplayNoInvoke();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeDirectionBtn_Click(object sender, EventArgs e)
        {
            if (OptecPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
            {
                MessageBox.Show("The device must be in the connected state and not moving or homing to perform this action.");
                return;
            }

            string currentDirString, newDirString;
            int newDir;
            try
            {
                if (OptecPyxis.getDirectionFlag() == OptecPyxis.CCW)
                {
                    currentDirString = "Counter-Clockwise";
                    newDirString = "Clockwise";
                    newDir = OptecPyxis.CW;
                }
                else
                {
                    currentDirString = "Clockwise";
                    newDirString = "Counter-Clockwise";
                    newDir = OptecPyxis.CCW;
                }
            }
            catch (Exception ex) 
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show("An error occured while attempting to get the current direction flag. " + 
                    ex.Message);
                return;
            }

            try
            {
                string msg = "The current default direction for increases in PA is set to "
                        + currentDirString + ". Would you like to change it to " + newDirString + "?";
                DialogResult result = MessageBox.Show(msg, "Change Direction?", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    OptecPyxis.setDefaultDirection(newDir);
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show("An error occured while attempting to set the current direction flag. " +
                    ex.Message);
                return;
            }
        }

        private void StepRate_BTN_Click(object sender, EventArgs e)
        {
            if (OptecPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
            {
                MessageBox.Show("The device must be in the connected state and not moving or homing to perform this action.");
                return;
            }
            StepRateForm frm = new StepRateForm();
            frm.rate = XMLSettings.StepRate;
            DialogResult result = frm.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    OptecPyxis.SetStepRate(frm.rate);
                    XMLSettings.StepRate = frm.rate;
                    frm.Dispose();
                    MessageBox.Show("Step rate update successfully!", "Success!");
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    MessageBox.Show("An error occurred while attempting to set a new step rate. " + 
                        ex.Message);
                }
            }
        }

        private void TargetPA_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GoToPA_BTN_Click(GoToPA_BTN, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void SetSkyPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (OptecPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Connected)
                {
                    MessageBox.Show("The device must be in the connected state and not moving or homing to perform this action.");
                    return;
                }
                SetSkyPAForm frm = new SetSkyPAForm();
                DialogResult result = frm.ShowDialog();
                double newPA = frm.PA;
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //Determine the offset
                    double offset = newPA - OptecPyxis.CurrentDevicePosition;
                    XMLSettings.SkyPAOffset = offset;
                    updateFormDisplayNoInvoke();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToAdjustedPA_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OptecPyxis.CurrentAdjustedPA = (int)double.Parse(this.AdjustedTargetPA_TB.Text);
                updateFormDisplayNoInvoke();
                while (OptecPyxis.IsMoving) { }
                updateFormDisplayNoInvoke();
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

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
           
 
        }

        


    }
}