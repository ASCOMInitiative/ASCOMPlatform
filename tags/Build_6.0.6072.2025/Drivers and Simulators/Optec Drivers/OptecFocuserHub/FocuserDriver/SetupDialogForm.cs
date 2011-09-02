using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.OptecFocuserHubTools;
using System.IO.Ports;

namespace ASCOM.OptecFocuserHub
{
    [ComVisible(false)]					// Form not registered for COM!

    
    public partial class SetupDialogForm : Form
    {
        private bool LastConnectedState = SharedResources.SharedFocuserManager.Connected;
        private bool connectionMethodChanged = false;
        private bool nicknameChanged = false;
        private bool editingNickname = false;
        private bool editingDeviceType = false;
        private bool deviceTypeChanged = false;

        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            if (SharedResources.SharedFocuserManager.ConnectionMethod == ConnectionMethods.Serial)
                SerialRB.Checked = true;
            else if (SharedResources.SharedFocuserManager.ConnectionMethod == ConnectionMethods.WiredEthernet)
                EthernetRB.Checked = true;

            foreach (var x in SharedResources.SharedFocuserManager.FocuserTypes.Keys)
            {
                FocuserTypeCB.Items.Add(x);
            }

            
            

            connectionStateChanged();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (connectionMethodChanged)
            {
                if (EthernetRB.Checked)
                {
                    SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.WiredEthernet;
                    SharedResources.SharedFocuserManager.IPAddress = IPAddrTB.Text;
                    SharedResources.SharedFocuserManager.TCPIPPort = TcpipPortNumberTB.Text;
                }
                else if (SerialRB.Checked)
                {
                    SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.Serial;
                    SharedResources.SharedFocuserManager.COMPortName = ComPortNameCB.Text;
                }
            }

            
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

        private void SerialRB_CheckedChanged(object sender, EventArgs e)
        {
            connectionMethodChanged = true;
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                ComPortNameCB.Enabled = true;
                ComPortNameCB.Items.Clear();
                ComPortNameCB.Items.AddRange(SerialPort.GetPortNames());
                for (int i = 0; i < ComPortNameCB.Items.Count; i++)
                {
                    if (ComPortNameCB.Items[i].ToString() == SharedResources.SharedFocuserManager.COMPortName)
                    {
                        ComPortNameCB.SelectedIndex = i;
                        break;
                    }
                }

                IPAddrTB.Enabled = false;
                TcpipPortNumberTB.Enabled = false;
            }


        }

        private void EthernetRB_CheckedChanged(object sender, EventArgs e)
        {
            connectionMethodChanged = true;
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                IPAddrTB.Enabled = true;
                IPAddrTB.Text = SharedResources.SharedFocuserManager.IPAddress;
                TcpipPortNumberTB.Enabled = true;
                TcpipPortNumberTB.Text = SharedResources.SharedFocuserManager.TCPIPPort;

                ComPortNameCB.Enabled = false;              
            }
        }

        private void NicknameTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Don't allow < or > in name
            if (e.KeyChar == (char)Keys.Back) return;
            if (e.KeyChar == '<' || e.KeyChar == '>') e.Handled = true;
            // Don't allow more than 16 characters
            if (((TextBox)(sender)).Text.Length == 16) e.Handled = true;
            

            nicknameChanged = true;
        }

        private void ConnectBTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectionMethodChanged)
                {
                    if (EthernetRB.Checked)
                    {
                        SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.WiredEthernet;
                        SharedResources.SharedFocuserManager.IPAddress = IPAddrTB.Text;
                        SharedResources.SharedFocuserManager.TCPIPPort = TcpipPortNumberTB.Text;
                    }
                    else if (SerialRB.Checked)
                    {
                        SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.Serial;
                        SharedResources.SharedFocuserManager.COMPortName = ComPortNameCB.Text;
                    }
                    connectionMethodChanged = false;
                }
                SharedResources.SharedFocuserManager.Connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Attempt Failed");
            }
        }

        private void DisconnectBTN_Click(object sender, EventArgs e)
        {
            try
            {
                SharedResources.SharedFocuserManager.Connected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Disconnecting");
            }
        }

        private delegate void formUpdater();
        private void ConnectionMonitor_Tick(object sender, EventArgs e)
        {
            if (SharedResources.SharedFocuserManager.Connected != LastConnectedState)
            {
                LastConnectedState = SharedResources.SharedFocuserManager.Connected;
                this.Invoke(new formUpdater(connectionStateChanged));
            }
        }

        private void connectionStateChanged()
        {
            if (LastConnectedState == false)
            {   // Connection terminated
                ConnectBTN.Enabled = true;
                DisconnectBTN.Enabled = false;
                NicknameTB.Text = "";
                PowerLight.Image = Properties.Resources.GreyLight;
                ConnectionSetupGB.Enabled = true;
                FocuserTypeCB.Text = "";
                groupBox2.Enabled = false;
            }
            else
            {   // Connection Established
                ConnectBTN.Enabled = false;
                DisconnectBTN.Enabled = true;
                NicknameTB.Text = SharedResources.SharedFocuserManager.Focuser1.Nickname;
                PowerLight.Image = Properties.Resources.RedLight;
                ConnectionSetupGB.Enabled = false;
                FocuserTypeCB.SelectedIndex = SharedResources.SharedFocuserManager.Focuser1.DeviceType - 'A';
                LEDTrackbar.Value = SharedResources.SharedFocuserManager.Focuser1.LEDBrightness;
                BacklashCompCB.Checked = SharedResources.SharedFocuserManager.Focuser1.BacklashCompEnabled;
                BacklashCompStepsNUD.Value = (decimal)SharedResources.SharedFocuserManager.Focuser1.BacklashCompSteps;
              
                groupBox2.Enabled = true;
            }
        }

      

        private void ChangeNicknameBTN_Click(object sender, EventArgs e)
        {
            
            if (editingNickname == false)
            {
                editingNickname = true;                
                ChangeFocuserTypeBTN.Enabled = false;
             
                cmdCancel.Enabled = false;
                OkBTN.Enabled = false;
                NicknameTB.ReadOnly = false;
                DisconnectBTN.Enabled = false;
                ChangeNicknameBTN.BackColor = Color.LightGreen;
            }
            else
            {
                if(nicknameChanged)
                {
                    try{ SharedResources.SharedFocuserManager.Focuser1.Nickname = NicknameTB.Text;}
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                    nicknameChanged = false;
                }
                editingNickname = false;
                ChangeFocuserTypeBTN.Enabled = true;
               
                DisconnectBTN.Enabled = true;
                cmdCancel.Enabled = true;
                OkBTN.Enabled = true;
                NicknameTB.ReadOnly = true;
                ChangeNicknameBTN.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void DeviceTypeChangeBtn_Click(object sender, EventArgs e)
        {
            if (editingDeviceType == false)
            {
                editingDeviceType = true;
                MessageBox.Show("Warning: Selecting a wrong focuser type for an attached focuser could cause damage to the device." + 
                    " Also, the focuser hub will automatically perform a reset after the device type has been changed.", "Warning");
                ChangeFocuserTypeBTN.BackColor = Color.LightGreen;
                ChangeNicknameBTN.Enabled = false;
                
                cmdCancel.Enabled = false;
                OkBTN.Enabled = false;
                DisconnectBTN.Enabled = false;
                FocuserTypeCB.Enabled = true;
            }
            else
            {
                if (deviceTypeChanged)
                {
                    char newType;
                    if (SharedResources.SharedFocuserManager.FocuserTypes.TryGetValue(FocuserTypeCB.Text, out newType) == false)
                    {
                        MessageBox.Show("Unacceptable device type selected.", "Error");
                    }
                    else
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;
                            SharedResources.SharedFocuserManager.Focuser1.DeviceType = newType;
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
                        finally { this.Cursor = Cursors.Default; }
                    }
                    deviceTypeChanged = false;
                }
                editingDeviceType = false;
                ChangeNicknameBTN.Enabled = true;
                
                cmdCancel.Enabled = true;
                OkBTN.Enabled = true;
                DisconnectBTN.Enabled = true;
                FocuserTypeCB.Enabled = false;
                ChangeFocuserTypeBTN.BackColor = System.Drawing.SystemColors.Control;
            }
            
        }

        private void FocuserTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceTypeChanged = true;
        }

        private void LEDTrackbar_Scroll(object sender, EventArgs e)
        {
            SharedResources.SharedFocuserManager.Focuser1.LEDBrightness = LEDTrackbar.Value;
        }

        private void BacklashCompCB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SharedResources.SharedFocuserManager.Focuser1.BacklashCompEnabled = BacklashCompCB.Checked;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void BacklashCompStepsNUD_ValueChanged(object sender, EventArgs e)
        {
            SharedResources.SharedFocuserManager.Focuser1.BacklashCompSteps = (int)BacklashCompStepsNUD.Value;
        }

        private void SetupTempCompBtn_Click(object sender, EventArgs e)
        {
            TempCompSetupForm tcsform = new TempCompSetupForm();
            tcsform.ShowDialog();
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SharedResources.SharedFocuserManager.Focuser1.Home();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

       

        
        

        

    }

    

}