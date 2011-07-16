using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optec_TCF_S_Focuser;
using System.Diagnostics;
using System.Reflection;
using Optec;

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
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
            }
            catch 
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.NewVersionBGWorker.IsBusy)
                {
                    string msg = "The Version Checker is currently busy. Please try again in a moment.";
                    MessageBox.Show(msg);
                }
                else
                {
                    if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.TCF_S))
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

        private void NewVersionBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Check For A newer verison of the driver
                EventLogger.LogMessage("Checking for application updates", TraceLevel.Info);
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.TCF_S))
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

        private void SetupDialog2_Shown(object sender, EventArgs e)
        {
            try
            {
                NewVersionBGWorker.RunWorkerAsync();
            }
            catch
            {
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abt = new AboutBox1();
            abt.ShowDialog();
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int i = asmpath.IndexOf("TCF-S");
            asmpath = asmpath.Substring(0, i + 5);
            asmpath += "\\Documentation\\TCF-S_Help.chm";
            //MessageBox.Show(asmpath);
            Process p = new Process();
            p.StartInfo.FileName = asmpath;
            p.Start();
        }

        private void PowerLight_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (myFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
                {
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
                }
                else
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
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
    }
}
