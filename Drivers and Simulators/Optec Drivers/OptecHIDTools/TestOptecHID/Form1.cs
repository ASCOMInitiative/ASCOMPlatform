using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OptecHIDTools;


namespace TestOptecHID
{
    public partial class Form1 : Form
    {
        //HIDMonitor myHID = null;
        public Form1()
        {
            InitializeComponent();
            HIDMonitor.HIDAttached += new EventHandler(OptecHID_NewHIDAttached);
            
        }

        void OptecHID_NewHIDAttached(object sender, EventArgs e)
        {

            this.BeginInvoke(new UpdateFormThread(RefreshDeviceList), new object[]{});
         
        }

        private delegate void UpdateFormThread();

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DateTime startTime = DateTime.Now;
                
        //        //myHID.GetDeviceList();
                
        //        //List<USBDevice> MatchingDevices = myHID.GetMatchingDevices();

        //        //foreach (USBDevice x in MatchingDevices)
        //        //{
        //        //    listBox1.Items.Add(x.VID_Hex + " --- " + x.PID_Hex);
        //        //}
                
        //        //myHID.DeviceChangeNotifier.DeviceAttached += new EventHandler(myHID_DeviceAttached);
        //        //OptecHID.SelectedDeviceAttached += new EventHandler(myHID_SelectedDeviceAttached);

        //        //myHID.SelectedDevice = MatchingDevices[0];


        //        OutputReport ReportToSend = new OutputReport();
        //        ReportToSend.DataWithoutID = new byte[] {0};
        //        ReportToSend.ReportID = 2;
        //        bool s = myHID.SelectedDevice.SendReport_Control(ReportToSend);
        //        listBox1.Items.Add("Send Success = " + s.ToString());
        //        System.Threading.Thread.Sleep(1000);
        //        ReportToSend.DataWithoutID = new byte[] { 1 };
        //        s = myHID.SelectedDevice.SendReport_Control(ReportToSend);
        //        listBox1.Items.Add("Send Success = " + s.ToString());
        //        byte[] test = myHID.SelectedDevice.RequestInputReport_Control(5);
        //        if (test != null)
        //        {
        //            string x = "";
        //            foreach (Byte z in test)
        //            {
        //                x += z.ToString() + "-";
        //            }
        //            listBox1.Items.Add(x);
        //        }

        //        FeatureReport fr = new FeatureReport();
        //        fr.ReportID = 6;
        //        fr.ExpectedResponse1 = new Byte[] { 1 };
        //        fr.ExpectedResponse2 = new Byte[] { 0 };
        //        bool success = false;
        //        for (int b = 1; b < 255; b++)
        //        {
        //            fr.DataToSend = new Byte[] { Convert.ToByte(b) };
        //            success = myHID.SelectedDevice.ProcessFeatureReport(fr);
        //            if (!success) break;
        //        }
        //        for (int b = 1; b < 255; b++)
        //        {
        //            fr.DataToSend = new Byte[] { Convert.ToByte(255-b) };
        //            success = myHID.SelectedDevice.ProcessFeatureReport(fr);
        //            if (!success) break;
        //        }

                
        //        listBox1.Items.Add("Feature Success = " + success.ToString());

        //        DateTime EndTime = DateTime.Now;
        //        TimeSpan TestDuration = EndTime.Subtract(startTime);
        //        string time = TestDuration.Hours + "h:" + TestDuration.Minutes + "m:" + TestDuration.Seconds + "s:" + TestDuration.Milliseconds + "ms" ;
        //        listBox1.Items.Add( "Completed after " + time);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        void DeviceAttached(object sender, EventArgs e)
        {
            MessageBox.Show("Device Attached");
        }

        void DeviceRemoved(object sender, EventArgs e)
        {
            MessageBox.Show("Device Removed");
        }

      

        void myHID_SelectedDeviceAttached(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDeviceList();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //myHID.UnRegisterForNotificaitons();
                //myHID.CloseStream();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // myHID.CloseStream();
        }

        private void Connect_Btn_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            //USBDevice x = (USBDevice)comboBox1.SelectedItem;
            //int i = comboBox1.SelectedIndex;
            
           // myHID = new HIDMonitor(i);
           // MessageBox.Show(myHID.SelectedDevice.DeviceIsAttached.ToString());
            //myHID.SelectedDevice.DeviceAttached += new EventHandler(SelectedDevice_DeviceAttached);
            //myHID.SelectedDevice.DeviceRemoved += new EventHandler(SelectedDevice_DeviceRemoved);
           // List<HID> dl = HIDMonitor.GetDeviceList();
        }

        void SelectedDevice_DeviceRemoved(object sender, EventArgs e)
        {
            MessageBox.Show("Selected Device Removed");
        }

        void SelectedDevice_DeviceAttached(object sender, EventArgs e)
        {
            MessageBox.Show("The Selected Device was attached.");
        }

        private void RefreshDeviceList()
        {
            this.comboBox1.Items.Clear();
            List<HID> dl = HIDMonitor.DetectedHIDs;
            comboBox1.DisplayMember = "SerialNumber";
            foreach (HID d in dl)
                comboBox1.Items.Add(d);
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void GetInputReport_Click(object sender, EventArgs e)
        {
            //byte[] bytes = myHID.SelectedDevice.RequestInputReport_Control(10);
            //string st = "";
            //if (bytes != null)
            //{
            //    foreach (Byte x in bytes)
            //    {
            //        if (x.ToString() == "255")
            //        {
            //            st += "TRUE" + ",";
            //        }
            //        else  st += x.ToString() + ",";
            //    }
            //    listBox1.Items.Add(st);
            //}

        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            //// Setup Feature Report to start Homing
            //FeatureReport HomeReport = new FeatureReport();
            //HomeReport.ReportID = 21;
            //HomeReport.ExpectedResponse1 = new byte[] { 255 };
            //HomeReport.ExpectedResponse2 = new byte[] { 0 };

            //// Send and Receive Feature Report
            //bool success = myHID.SelectedDevice.ProcessFeatureReport(HomeReport);
            //if (success) listBox1.Items.Add("Homing Started");
            //else listBox1.Items.Add("Homing Failed to Start");

            //// Now Wait until we detect that motion has stopped.
        }



        private void button3_Click(object sender, EventArgs e)
        {
            //byte[] bytes = myHID.SelectedDevice.RequestInputReport_Control(11);
            //string st = "";
            //if (bytes != null)
            //{
            //    foreach (Byte x in bytes)
            //    {
            //        st += x.ToString() + ",";
            //    }
            //    listBox1.Items.Add(st);
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //// Setup Feature Report to start Homing
            //FeatureReport MoveReport = new FeatureReport();
            //MoveReport.ReportID = 20;
            //MoveReport.DataToSend = new Byte[] { Convert.ToByte(textBox1.Text) };
            //MoveReport.ExpectedResponse1 = new byte[] { 255 };
            //MoveReport.ExpectedResponse2 = new byte[] { 0 };

            //// Send and Receive Feature Report
            //bool success = myHID.SelectedDevice.ProcessFeatureReport(MoveReport);
            //if (success) listBox1.Items.Add("Move Started");
            //else listBox1.Items.Add("Move Failed to Start");

            //// Now Wait until we detect that motion has stopped.
        }

    }
}
