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

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
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
            UpdateDeviceList();


            
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
            myRotator.Home();
            MotionMonitor();
        }

        private void UpdateSkyPALabel()
        {
            this.CurrentPA_LBL.Text = myRotator.CurrentSkyPA.ToString("000.00°");
        }

        private void SetSkyPA_Btn_Click(object sender, EventArgs e)
        {
            string show = myProfile.GetValue(Rotator.s_csDriverID, "ShowSetPAWarning", 
                "", (true).ToString());
            if(show == true.ToString())
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

        private void AbsoluteMove_BTN_Click(object sender, EventArgs e)
        {
            double pos = double.Parse(AbsoluteMove_TB.Text);
            StartAMove(pos);
            if (pos == 360) AbsoluteMove_TB.Text = "0";

        }

        private delegate void DelNoParms();

        private void MotionMonitor()
        {
            System.Threading.Thread.Sleep(100);
   
            while (myRotator.IsMoving || myRotator.IsHoming)
            {
                System.Threading.Thread.Sleep(25);
          
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
            double increment = (double)Relative_NUD.Value;
            if (increment == 0) return;
            double NewPositon = myRotator.CurrentSkyPA + increment;
            if (NewPositon > 360) NewPositon = NewPositon - 360;
            if (NewPositon < 0) NewPositon = NewPositon + 360;
            StartAMove(NewPositon);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double increment = (double)Relative_NUD.Value;
            if (increment == 0) return;
            double NewPositon = myRotator.CurrentSkyPA - increment;
            if (NewPositon > 360) NewPositon = NewPositon - 360;
            if (NewPositon < 0) NewPositon = NewPositon + 360;
            StartAMove(NewPositon);
        }

        private void advancedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AdvancedForm frm = new AdvancedForm(myRotator);
                frm.ShowDialog();
                frm.Dispose();
                //EnableControls();
                MotionMonitor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}