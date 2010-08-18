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

namespace PyxisLE_Control
{
    public partial class Form1 : Form
    {
        Rotators RotatorMonitor;
        private Rotator myRotator;
        private delegate void UI_Update(string x);

        public Form1()
        {
            InitializeComponent();
            RotatorMonitor = new Rotators();
            ArrayList AllControls = new ArrayList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if ((RotatorMonitor.RotatorList.Count > 0))
            {
                myRotator = RotatorMonitor.RotatorList[0] as Rotator;
                myRotator.HomeFinished += new EventHandler(myRotator_HomeFinished);
                myRotator.MoveFinished += new EventHandler(myRotator_MoveFinished);
                myRotator.DeviceUnplugged += new EventHandler(myRotator_DeviceUnplugged);
                this.ReverseCB.Checked = myRotator.Reverse;
                this.RTL_CB.Checked = myRotator.ReturnToLastOnHome;
            }
            else
            {
                timer1.Enabled = true;
                foreach (Control x in this.Controls)
                {
                    try
                    {
                        if (x.GetType() != timer1.GetType())
                        {
                            x.Enabled = false;
                            Application.DoEvents();
                        }
                    }
                    catch { }
                }
            }
            

            
        }

        void myRotator_DeviceUnplugged(object sender, EventArgs e)
        {
            myRotator = null;
            this.Invoke(new UpdateUI(DisableAllControls));
            timer1.Enabled = true;
        }

        private void GetDeviceInfo()
        {
            this.listBox1.Items.Add("Attached Device Found ***************************:");
            this.listBox1.Items.Add("Firmware Version: " + myRotator.FirmwareVersion);
            this.listBox1.Items.Add("Device Type = " + myRotator.DeviceType);
            this.listBox1.Items.Add("Error State = " + myRotator.ErrorState);
            this.listBox1.Items.Add("Current Position = " + myRotator.CurrentPosition.ToString());
            this.listBox1.Items.Add("Target Position = " + myRotator.TargetPosition.ToString());
            this.listBox1.Items.Add("Zero Offset = " + myRotator.ZeroOffset.ToString());
            this.listBox1.Items.Add("Sky PA Offset = " + myRotator.CurrentSkyPA.ToString());
            this.listBox1.Items.Add("Steps per Revolution = " + myRotator.StepsPerRev.ToString());
            this.listBox1.Items.Add("Reverse Property = " + myRotator.Reverse.ToString());
            this.listBox1.Items.Add("Return To Last On Home = " + myRotator.ReturnToLastOnHome.ToString());
            this.listBox1.Items.Add("Serial Number = " + myRotator.SerialNumber);
        }

        private void GetInfo_BTN_Click(object sender, EventArgs e)
        {
            GetDeviceInfo();
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            int x = listBox1.Items.Count;
            listBox1.SelectedIndex = x - 1;
        }

        private void ReqMove_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                double pos = double.Parse(textBox1.Text);
                this.listBox1.Items.Add("Moving to: " + pos.ToString());
                ScrollToBottom();
                myRotator.ChangePosition(pos);    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBox1.Items.Add("Homing Device... " );
                ScrollToBottom();
                myRotator.Home();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoveTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ReqMove_BTN_Click(this, EventArgs.Empty);
            }
        }

        private void MoveTextBoxRelative_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                GoToRel_BTN_Click(this, EventArgs.Empty);
            }
        }

        private void GoToRel_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                double Distance = Double.Parse(textBox2.Text);
                myRotator.ChangePosition_Relative(Distance);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Halt_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                myRotator.Halt_Move();
                listBox1.Items.Add("Device Haulted");
                ScrollToBottom();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTheListBox(string text)
        {
            listBox1.Items.Add(text);
            ScrollToBottom();
        }

        void myRotator_HomeFinished(object sender, EventArgs e)
        {
            string msg = "Homing Completed!";
            this.Invoke(new UI_Update(UpdateTheListBox), new object[] { msg });
        }

        void myRotator_MoveFinished(object sender, EventArgs e)
        {
            string msg = "\nMove Completed!";
            this.Invoke(new UI_Update(UpdateTheListBox), new object[] { msg });
            msg = "Current Position = " + myRotator.CurrentPosition.ToString() + "°";

            this.Invoke(new UI_Update(UpdateTheListBox), new object[] { msg });
            msg = "Current Sky PA = " + myRotator.CurrentSkyPA.ToString() + "°";
            this.Invoke(new UI_Update(UpdateTheListBox), new object[] { msg });
        }

        private void ReverseCB_CheckedChanged(object sender, EventArgs e)
        {
            myRotator.Reverse = ReverseCB.Checked;
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            MouseEventArgs ClickedPt = e as MouseEventArgs;
            int x1 = 78;
            int y1 = 75;
            int x2 = ClickedPt.X;
            int y2 = ClickedPt.Y;
            int dx = x2 - x1;
            int dy = y2 - y1;
            double radians = Math.Atan2((double)dy, (double)dx);
            double angle = -(radians * (180 / Math.PI) - 180);
            angle = angle + 90;
            if (angle > 360) angle = (angle - 360);
            myRotator.ChangePosition(angle);
            
        }

        private void RTL_CB_CheckedChanged(object sender, EventArgs e)
        {
            myRotator.ReturnToLastOnHome = RTL_CB.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (myRotator == null)
                {
                    if (RotatorMonitor.RotatorList.Count > 0)
                    {
                        myRotator = RotatorMonitor.RotatorList[0] as Rotator;
                        myRotator.HomeFinished += new EventHandler(myRotator_HomeFinished);
                        myRotator.MoveFinished += new EventHandler(myRotator_MoveFinished);
                        myRotator.DeviceUnplugged += new EventHandler(myRotator_DeviceUnplugged);
                        this.Invoke(new UpdateUI(EnableAllControls));
                        timer1.Enabled = false;
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private delegate void UpdateUI();

        private void EnableAllControls()
        {
            try
            {
                foreach (Control x in this.Controls)
                {
                    if (x.GetType() != timer1.GetType())
                    {
                        x.Enabled = true;
                        Application.DoEvents();
                    }

                }
                this.Update();
                this.Refresh();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DisableAllControls()
        {
            try
            {
                foreach (Control x in this.Controls)
                {
                    if (x.GetType() != timer1.GetType())
                    {
                        x.Enabled = false;
                        x.Refresh();
                        x.Update();
                        Application.DoEvents();
                    }
                }
                this.Update();
                this.Refresh();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SetZOBTN_Click(object sender, EventArgs e)
        {
            myRotator.ZeroOffset = Int16.Parse(ZOff_TB.Text);
        }

        private void SetSkyPA_BTN_Click(object sender, EventArgs e)
        {
            double x = double.Parse(SkyPA_TB.Text);
            myRotator.CurrentSkyPA = x;
        }

       
    }
}
