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
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myRotator = RotatorMonitor.RotatorList[0] as Rotator;
            myRotator.HomeFinished += new EventHandler(myRotator_HomeFinished);
            myRotator.MoveFinished += new EventHandler(myRotator_MoveFinished);
            this.ReverseCB.Checked = myRotator.Reverse;
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

        private void MoveTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ReqMove_BTN_Click(this, EventArgs.Empty);
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
        }

        private void ReverseCB_CheckedChanged(object sender, EventArgs e)
        {
            myRotator.Reverse = ReverseCB.Checked;
        }

       
    }
}
