using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optec_TCF_S_Focuser;
using System.Diagnostics;
using System.Reflection;

namespace ASCOM.OptecTCF_S
{
    public partial class SetupDialog2 : Form
    {
        OptecFocuser myFocuser;

        public SetupDialog2(OptecFocuser myFoc)
        {
            InitializeComponent();
            myFocuser = myFoc;
        }

        private void SetupDialog2_Load(object sender, EventArgs e)
        {
            // Populate the COM Port Combobox
            refreshCOMPortNames();
            propertyGrid1.SelectedObject = myFocuser;

            // This section moves the splitter to higher location
            Type propertygridtype = propertyGrid1.GetType();
            FieldInfo y = propertygridtype.GetField("gridView",
                BindingFlags.NonPublic | BindingFlags.Instance);
            y.FieldType.GetMethod("MoveSplitterTo",
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(y.GetValue(propertyGrid1), new object[] { 175 });
            
            myFocuser.DeviceStatusChanged += new EventHandler(myFocuser_DeviceStatusChanged);
            myFocuser.ErrorOccurred += new EventHandler(myFocuser_ErrorOccurred);
        }

        void myFocuser_ErrorOccurred(object sender, EventArgs e)
        {
            Exception ex = sender as Exception;
            this.Invoke(new StatusLabelUpdater(DisplayErrorMessage), ex.Message);
        }

        void myFocuser_DeviceStatusChanged(object sender, EventArgs e)
        {
            try
            {
                string msg;
                if (myFocuser.ConnectionState == OptecFocuser.ConnectionStates.Disconnected)
                {
                    msg = "Device is Not Connected";
                }
                else msg = "Device is Connected";
                this.Invoke(new StatusLabelUpdater(UpdateStatusLabel), msg);
            }
            catch
            {
            }
        }

        private void DisplayErrorMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private delegate void StatusLabelUpdater(string status);

        private void UpdateStatusLabel(string status)
        {
            this.toolStripStatusLabel1.Text = status;
        }

        private void refreshCOMPortNames()
        {
            // Populate the COM Port Combobox
            comboBox1.SelectionChangeCommitted -= new EventHandler(comboBox1_SelectionChangeCommitted);
            this.comboBox1.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            int i = 0;
            foreach (string n in comboBox1.Items)
            {
                if (n == myFocuser.SavedSerialPortName)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
                else
                {
                    comboBox1.Text = string.Empty;
                }
                i++;
            }
            comboBox1.SelectionChangeCommitted += new EventHandler(comboBox1_SelectionChangeCommitted);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x219)
            {
                Debug.Print("Device Change Notification");
                refreshCOMPortNames();
            }
            base.WndProc(ref m);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(myFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
            {
                MessageBox.Show("You must disconnect before you can change the serial port.");
                refreshCOMPortNames();
            }
            ComboBox cb = sender as ComboBox;

            myFocuser.SavedSerialPortName = cb.SelectedItem as string;

            propertyGrid1.Update();
            propertyGrid1.Refresh();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (myFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
                {
                    MessageBox.Show("Already Connected!");
                    return;
                }
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                propertyGrid1.Refresh();
                PowerLight.Image = Properties.Resources.RedLight;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
                propertyGrid1.Refresh();
                PowerLight.Image = Properties.Resources.GreyLight;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetupDialog2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                myFocuser.DeviceStatusChanged -= new EventHandler(myFocuser_DeviceStatusChanged);
                myFocuser.ErrorOccurred -= new EventHandler(myFocuser_ErrorOccurred);
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
            }
            catch 
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
