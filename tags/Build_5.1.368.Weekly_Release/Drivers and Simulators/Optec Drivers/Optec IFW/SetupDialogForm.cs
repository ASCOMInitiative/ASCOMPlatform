using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace ASCOM.Optec_IFW
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        #region //Declarations
        private static object CommLock = new object();

        FilterWheel DriverInstance = new FilterWheel();
        #endregion

        public SetupDialogForm()
        {
            InitializeComponent();    
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DriverInstance.Connected = false;   //dissconnect the device to free it up for manual control
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

        private void SaveData_Btn_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Filter Names & Offsets Saved!\r\n\r\nNOTE: New names will not be displayed on the handbox until the device is homed.",
                "Data Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            cmdOK.Enabled = true;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            cmdOK.Enabled = false;
            this.Home_Btn.Enabled = false;


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

                #region Get Filter Names from the Device
                string[] names = new string[DeviceComm.NumOfFilters];

                names = DeviceComm.ReadAllNames();
                foreach (Control c in panel1.Controls)
                {
                    for (int i = 0; i < DeviceComm.NumOfFilters; i++)
                    {
                        if (c.Name.Contains((i + 1).ToString()) && c.Name.Contains("Filter"))
                        {
                            c.Text = names[i];
                            break;
                        }
                    }
                } 
                #endregion

                #region Get the Offset Values From Registry
                string[] OffsetValues = new string[DeviceComm.NumOfFilters];
                OffsetValues = DeviceComm.TryGetOffsets();


                try
                {
                    this.F1Offset_TB.Text = OffsetValues[0];
                    this.F2Offset_TB.Text = OffsetValues[1];
                    this.F3Offset_TB.Text = OffsetValues[2];
                    this.F4Offset_TB.Text = OffsetValues[3];
                    this.F5Offset_TB.Text = OffsetValues[4];
                    this.F6Offset_TB.Text = OffsetValues[5];
                    this.F7Offset_TB.Text = OffsetValues[6];
                    this.F8Offset_TB.Text = OffsetValues[7];
                    this.F9Offset_TB.Text = OffsetValues[8];
                }
                catch (Exception)
                {

                    //do nothing, this is where the program flows if you are using less than 9 filters
                } 
                #endregion

                this.Connect_BTN.Enabled = false;
                this.SaveData_Btn.Enabled = true;
                this.Home_Btn.Enabled = true;

            }
        }

        private void ComPort_Picker_ValueChanged(object sender, EventArgs e)
        {
            int PortNumber = (int)this.ComPort_Picker.Value;
            DeviceComm.SavePortNumber(PortNumber.ToString());
            DeviceComm.FilterWheelType = DeviceComm.TypesOfFWs.IFW;
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
            DeviceComm.FilterWheelType = DeviceComm.TypesOfFWs.IFW3;
        }

        private void SaveSettings()
        {
           
            #region //store the filter names to the device memory
            string[] Names = new string[DeviceComm.NumOfFilters];


            for (int i = 0; i < DeviceComm.NumOfFilters; i++)
            {
                foreach (Control c in this.panel1.Controls)
                {
                    if (c.Name.Contains("Filter") && c.Name.Contains((i + 1).ToString()))
                    {
                        Names[i] = c.Text;
                    }
                }
            }
            DeviceComm.StoreNames(Names); 
            #endregion

            #region Store the filter offsets in the registry
            int[] filteroffsets = new int[9];
            try
            {
                if (this.F1Offset_TB.Text == "") filteroffsets[0] = 0000;
                else filteroffsets[0] = int.Parse(this.F1Offset_TB.Text);

                if (this.F2Offset_TB.Text == "") filteroffsets[1] = 0000;
                else filteroffsets[1] = int.Parse(this.F2Offset_TB.Text);

                if (this.F3Offset_TB.Text == "") filteroffsets[2] = 0000;
                else filteroffsets[2] = int.Parse(this.F3Offset_TB.Text);

                if (this.F4Offset_TB.Text == "") filteroffsets[3] = 0000;
                else filteroffsets[3] = int.Parse(this.F4Offset_TB.Text);

                if (this.F5Offset_TB.Text == "") filteroffsets[4] = 0000;
                else filteroffsets[4] = int.Parse(this.F5Offset_TB.Text);

                if (this.F6Offset_TB.Text == "") filteroffsets[5] = 0000;
                else filteroffsets[5] = int.Parse(this.F6Offset_TB.Text);

                if (this.F7Offset_TB.Text == "") filteroffsets[6] = 0000;
                else filteroffsets[6] = int.Parse(this.F7Offset_TB.Text);

                if (this.F8Offset_TB.Text == "") filteroffsets[7] = 0000;
                else filteroffsets[7] = int.Parse(this.F8Offset_TB.Text);

                if (this.F9Offset_TB.Text == "") filteroffsets[8] = 0000;
                else filteroffsets[8] = int.Parse(this.F9Offset_TB.Text);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An illegal offset value has been entered. Make sure numbers only - " + Ex.Data + Ex.Message);
            }
            DeviceComm.StoreFilterOffsets(filteroffsets);
            #endregion

        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            if (DriverInstance.Connected) DriverInstance.Connected = false;

            AdvancedForm AForm = new AdvancedForm();
            AForm.COMPortString = "Note: Using COM Port: " + this.ComPort_Picker.Value.ToString()+ ". Selected on previous page.";
            AForm.CP = this.ComPort_Picker.Value.ToString();
            AForm.ShowDialog();
            foreach (Control C in this.panel1.Controls)
            {
                C.Enabled = false;
            }
            this.SaveData_Btn.Enabled = false;
            this.Home_Btn.Enabled = false;
            this.Connect_BTN.Enabled = true;
            this.cmdOK.Enabled = true;

           
        }

        private void Home_Btn_Click(object sender, EventArgs e)
        {
            DeviceComm.HomeDevice();
        }
    }
}