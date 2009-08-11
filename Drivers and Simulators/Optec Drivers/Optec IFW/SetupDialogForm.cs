using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Optec_IFW
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private static object CommLock = new object();
        FilterWheel DriverInstance = new FilterWheel();

        public SetupDialogForm()
        {
            InitializeComponent();    
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

        private void TestConnect_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                ASCOM.Helper.Serial test = new ASCOM.Helper.Serial();
                test.Port = 3;
                test.Speed = ASCOM.Helper.PortSpeed.ps19200;
                test.ReceiveTimeoutMs = 500;
                test.Connected = true;
                test.ClearBuffers();
                test.Transmit("WSMODE");
                MessageBox.Show(test.Receive());
            }
        }

        private void DissConnBtn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.DisconnectDevice();
            }
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.HomeDevice();
            }
            this.WheelId_TB.Text = DeviceComm.WheelID.ToString();
        }

        private void ReadNames_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
               // this.FilterNames_TB.Text = DeviceComm.ReadAllNames();
            }
            
        }

        private void CheckConn_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                if (DeviceComm.CheckForConnection()) this.ConnStatus_TB.Text = "Yes";
                else this.ConnStatus_TB.Text = "NO";
            }

        }

        private void GoTo_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.GoToPosition(Int32.Parse(this.GoToPos_CB.Text));
            }
        }

        private void SaveData_Btn_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            //load the com port if one has been saved
            if( Int32.Parse(DeviceComm.TryGetCOMPort())> 0)
            {
                this.ComPort_Picker.Value = Int32.Parse(DeviceComm.TryGetCOMPort());
            }
            //load the filter wheel type if one has been saved
            string FW_Type = DeviceComm.TryGetFilterWheelType();
            if ( FW_Type == DeviceComm.TypesOfFWs.IFW.ToString())
            {
                this.IFW_RB.Checked = true;
            }  
            else if (FW_Type == DeviceComm.TypesOfFWs.IFW3.ToString())
            {
                this.IFW3_RB.Checked = true;
            }   
        }

        private void Connect_BTN_Click(object sender, EventArgs e)
        {
            if (this.ComPort_Picker.Value < 1 )
            {
                MessageBox.Show("You must select a COM port first");
            }
            else if (!(IFW_RB.Checked || IFW3_RB.Checked))
            {
                MessageBox.Show("Please select which IFW model you have");
            }
            else
            {
                DriverInstance.Connected = true;
                //DeviceComm.HomeDevice();  //the device is homed immediatly after connection
                //char WheelID = DeviceComm.GetWheelIdentity();

                //Setup the form for the number of positions the wheel has

                #region Enable/Disable textbox's
                switch (DeviceComm.NumOfFilters)
                {
                    case 9:
                        //9 positions wheels
                        Filter7Name_TB.Enabled = true;
                        Filter8Name_TB.Enabled = true;
                        Filter9Name_TB.Enabled = true;
                        F7Offset_TB.Enabled = true;
                        F8Offset_TB.Enabled = true;
                        F9Offset_TB.Enabled = true;
                        goto case 6;
                    case 6:
                        //6 position wheels
                        Filter6Name_TB.Enabled = true;
                        F6Offset_TB.Enabled = true;
                        goto case 5;
                    case 5:
                        // 5 position wheels
                        Filter1Name_TB.Enabled = true;
                        Filter2Name_TB.Enabled = true;
                        Filter3Name_TB.Enabled = true;
                        Filter4Name_TB.Enabled = true;
                        Filter5Name_TB.Enabled = true;
                        F1Offset_TB.Enabled = true;
                        F2Offset_TB.Enabled = true;
                        F3Offset_TB.Enabled = true;
                        F4Offset_TB.Enabled = true;
                        F5Offset_TB.Enabled = true;
                        break;
                    default:
                        throw new Exception("Incorrect number of positions returned");
                }
                #endregion

                //Get the filter names from the Device
                string[] names = new string[DeviceComm.NumOfFilters];

                names = DeviceComm.ReadAllNames();
                foreach (Control c in panel1.Controls)
                {
                    for (int i = 0; i < DeviceComm.NumOfFilters; i++)
		            {
                        if (c.Name.Contains((i+1).ToString()) && c.Name.Contains("Filter"))
                        {
                            c.Text = names[i];
                            break;
                        }
		            }
                }
            }
        }

        private void ComPort_Picker_ValueChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void IFW_RB_CheckedChanged(object sender, EventArgs e)
        {
            
            if (IFW_RB.Checked == true) DeviceComm.SaveFilterWheelType(DeviceComm.TypesOfFWs.IFW.ToString());
            else if (IFW3_RB.Checked == true) DeviceComm.SaveFilterWheelType(DeviceComm.TypesOfFWs.IFW3.ToString());   
        }

        private void IFW3_RB_CheckedChanged(object sender, EventArgs e)
        {
            if (IFW_RB.Checked == true) DeviceComm.SaveFilterWheelType(DeviceComm.TypesOfFWs.IFW.ToString());
            else if (IFW3_RB.Checked == true) DeviceComm.SaveFilterWheelType(DeviceComm.TypesOfFWs.IFW3.ToString());   
        }

        private void SaveSettings()
        {
            int PortNumber = (int)this.ComPort_Picker.Value;
            DeviceComm.SavePortNumber(PortNumber.ToString());

            //store the filter names to the device memory


            //Store the filter offsets in the registry
            //float[] filteroffsets = new float[9];
            //filteroffsets[0] = float.Parse(this.F1Offset_TB.Text);
            //filteroffsets[1] = float.Parse(this.F1Offset_TB.Text);
            //filteroffsets[2] = float.Parse(this.F2Offset_TB.Text);
            //filteroffsets[3] = float.Parse(this.F3Offset_TB.Text);
            //filteroffsets[4] = float.Parse(this.F4Offset_TB.Text);
            //filteroffsets[5] = float.Parse(this.F5Offset_TB.Text);
            //filteroffsets[6] = float.Parse(this.F6Offset_TB.Text);
            //filteroffsets[7] = float.Parse(this.F7Offset_TB.Text);
            //filteroffsets[8] = float.Parse(this.F8Offset_TB.Text);
            //DeviceComm.StoreFilterOffsets(filteroffsets, this.IFW_RB.Checked);
            
            //Store Centering Values

        }





        




 

    }
}