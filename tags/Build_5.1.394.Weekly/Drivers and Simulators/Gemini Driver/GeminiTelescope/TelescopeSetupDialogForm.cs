using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{

    [ComVisible(false)]					// Form not registered for COM!
    public partial class TelescopeSetupDialogForm : Form
    {
        
        public TelescopeSetupDialogForm()
        {
            InitializeComponent();
            


            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }

            Version version = new Version(Application.ProductVersion);
            labelVersion.Text = "ASCOM Gemini Telescope .NET " + string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            labelTime.Text = "Time zone is " + localZone.StandardName;
            if (localZone.IsDaylightSavingTime(DateTime.Now))
            {
                labelTime.Text += " (currently DST)"; 
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
           
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
           
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

        #region Properties for Settings
        public string ComPort
        {
            get { return comboBoxComPort.SelectedItem.ToString(); }
            set { comboBoxComPort.SelectedItem = value; }
        }

        public string BaudRate
        {
            get { return comboBoxBaudRate.SelectedItem.ToString(); }
            set { comboBoxBaudRate.SelectedItem = value; }
        }
        public double Elevation
        {
            get 
            {
                double elevation;
                double.TryParse(textBoxElevation.Text, out elevation);
                return elevation;
                
            }
            set { textBoxElevation.Text = value.ToString(); }
        }
        public double Latitude
        {
            get
            {
                double lat=0;
                try
                {
                    lat = double.Parse(textBoxLatitudeDegrees.Text) + double.Parse(textBoxLatitudeMinutes.Text) / 60;
                    if (comboBoxLatitude.SelectedItem.ToString() == "S") { lat = -lat; }
                }
                catch { }
                return lat;
            }
            set
            {
                if (value < 0)
                {
                    comboBoxLatitude.SelectedIndex = 1;
                    value = -value;
                }
                else
                {
                    comboBoxLatitude.SelectedIndex = 0;
                }

                textBoxLatitudeDegrees.Text = ((int)value).ToString("00");
                textBoxLatitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00");
            }
        }
        public double Longitude
        {
            get
            {
                double log = 0;
                try
                {
                    log = double.Parse(textBoxLongitudeDegrees.Text) + double.Parse(textBoxLongitudeMinutes.Text) / 60;
                    if (comboBoxLongitude.SelectedItem.ToString() == "W") { log = -log; }
                }
                catch { }
                return log;
            }
            set
            {
                if (value < 0)
                {
                    value = -value;
                    comboBoxLongitude.SelectedIndex = 1;
                }else{
                    comboBoxLongitude.SelectedIndex = 0;
                }

                textBoxLongitudeDegrees.Text = ((int)value).ToString("000");
                textBoxLongitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00");
            }
        }
        public bool UseGeminiSite
        {
            get { return checkBoxUseGeminiSite.Checked; }
            set 
            { 
                checkBoxUseGeminiSite.Checked = value;
                if (value)
                {
                    comboBoxLatitude.Enabled = false;
                    textBoxLatitudeDegrees.Enabled = false;
                    textBoxLatitudeMinutes.Enabled = false;
                    comboBoxLongitude.Enabled = false;
                    textBoxLongitudeDegrees.Enabled = false;
                    textBoxLongitudeMinutes.Enabled = false;
                }
                else
                {
                    comboBoxLatitude.Enabled = true;
                    textBoxLatitudeDegrees.Enabled = true;
                    textBoxLatitudeMinutes.Enabled = true;
                    comboBoxLongitude.Enabled = true;
                    textBoxLongitudeDegrees.Enabled = true;
                    textBoxLongitudeMinutes.Enabled = true;
                }
            }
        }
        public bool UseGeminiTime
        {
            get { return checkBoxUseGeminiTime.Checked; }
            set { checkBoxUseGeminiTime.Checked = value; }
        }
        
        #endregion

        private void TelescopeSetupDialogForm_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);
        }

        private void buttonVirtualPort_Click(object sender, EventArgs e)
        {
            frmPassThroughPortSetup frm = new frmPassThroughPortSetup();
            DialogResult res = frm.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    GeminiHardware.PassThroughPortEnabled = frm.VirtualPortEnabled;
                    GeminiHardware.PassThroughComPort = frm.ComPort;
                    GeminiHardware.PassThroughBaudRate = int.Parse(frm.BaudRate);
                }
                catch 
                {
                    MessageBox.Show("Settings are invalid", SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            labelUtc.Text = DateTime.UtcNow.ToLongTimeString();
        }   
    }
}