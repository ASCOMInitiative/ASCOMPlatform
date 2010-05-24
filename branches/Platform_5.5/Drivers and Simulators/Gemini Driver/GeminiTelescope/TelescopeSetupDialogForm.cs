using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{

    [ComVisible(false)]					// Form not registered for COM!
    public partial class TelescopeSetupDialogForm : Form
    {
        private string m_GpsComPort;
        private string m_GpsBaudRate;
        private bool m_GpsUpdateClock;

        private bool m_DoneInitialize;

        private bool m_UseSpeech = false;
        private double m_SaveLatitude;
        private double m_SaveLongitude;
        private int m_SaveUTCOffset;

        public string[] TimeZones = {
            "UTC-11",
            "UTC-10 AM HST",
            "UTC-9  AM YST",
            "UTC-8  AM PST/YDT",
            "UTC-7  AM MST/PDT",
            "UTC-6  AM CST/MDT",
            "UTC-5  AM EST/CDT",
            "UTC-4  AM AST/EDT",
            "UTC-3  AM ADT",
            "UTC-2",
            "UTC-1  AZOT",
            "UTC+0  GMT",
            "UTC+1  CET, BST",
            "UTC+2  CEST, EET",
            "UTC+3  EEST, MSK",
            "UTC+4 ",
            "UTC+5",
            "UTC+6",
            "UTC+7",
            "UTC+8  AU WST",
            "UTC+9 ",
            "UTC+10 AEST",
            "UTC+11 AEDT",
            "UTC+12",
            "UTC+13",
            "UTC+14"
        };
        
        public bool UseSpeech
        {
            get { return chkVoice.Checked; }
            set { m_UseSpeech = value; }
        }
        private string m_SpeechVoice = "";

        public string SpeechVoice
        {
            get { return m_SpeechVoice; }
            set { m_SpeechVoice = value; }
        }
        private Speech.SpeechType m_SpeechFlags = 0;

        internal Speech.SpeechType SpeechFlags
        {
            get { return m_SpeechFlags; }
            set { m_SpeechFlags = value; }
        }

        internal bool AllowPortScan
        {
            get { return chkPortScan.Checked; }
            set { chkPortScan.Checked = value; }
        }

        public TelescopeSetupDialogForm()
        {
            m_DoneInitialize = false;

            InitializeComponent();           

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }

            string[] joys = Joystick.JoystickNames;
            if (joys != null)
            {
                foreach (string s in Joystick.JoystickNames)
                {
                    cmbJoystick.Items.Add(s);
                }
                cmbJoystick.SelectedItem = GeminiHardware.JoystickName;

                chkJoystick.CheckState = GeminiHardware.UseJoystick ? CheckState.Checked : CheckState.Unchecked;
                if (!chkJoystick.Checked) cmbJoystick.Enabled = false;
                chkJoystick.BackColor = Color.Transparent;
            }
            else
            {   // no joysticks detected
                chkJoystick.CheckState = CheckState.Unchecked;
                chkJoystick.Enabled = false;
                cmbJoystick.Enabled = false;
                chkJoystick.BackColor= Color.FromArgb(64, 64, 64);

            }

            chkVoice.Checked = GeminiHardware.UseSpeech;
            m_SpeechVoice = GeminiHardware.SpeechVoice;
            m_SpeechFlags = GeminiHardware.SpeechFilter;

            Version version = new Version(Application.ProductVersion);
            labelVersion.Text = "ASCOM Gemini Telescope .NET " + string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            
            labelTime.Text = "Time zone is " + (localZone.IsDaylightSavingTime(DateTime.Now)? localZone.DaylightName : localZone.StandardName);

            foreach (string tz in TimeZones)
            {
                int idx = comboBoxTZ.Items.Add(tz);
            }
            
            m_DoneInitialize = true;
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
        public GeminiHardware.GeminiBootMode BootMode
        {
            get
            {
                if (radioButtonPrompt.Checked)
                {
                    return GeminiHardware.GeminiBootMode.Prompt;
                }
                else if (radioButtonColdStart.Checked)
                {
                    return GeminiHardware.GeminiBootMode.ColdStart;
                }
                else if (radioButtonWarmStart.Checked)
                {
                    return GeminiHardware.GeminiBootMode.WarmStart;
                }
                else if (radioButtonWarmRestart.Checked)
                {
                    return GeminiHardware.GeminiBootMode.WarmRestart;
                }
                else
                {
                    return GeminiHardware.GeminiBootMode.Prompt;
                }
            }
            set
            {
                switch (value)
                {
                    case GeminiHardware.GeminiBootMode.Prompt:
                        radioButtonPrompt.Checked = true;
                        break;
                    case GeminiHardware.GeminiBootMode.ColdStart:
                        radioButtonColdStart.Checked = true;
                        break;
                    case GeminiHardware.GeminiBootMode.WarmStart:
                        radioButtonWarmStart.Checked = true;
                        break;
                    case GeminiHardware.GeminiBootMode.WarmRestart:
                        radioButtonWarmRestart.Checked = true;
                        break;
                    default:
                        radioButtonPrompt.Checked = true;
                        break;
                }
            }
        }
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

        public string GpsComPort
        {
            get { return m_GpsComPort; }
            set { m_GpsComPort = value; }
        }
        public bool GpsUpdateClock
        {
            get { return m_GpsUpdateClock; }
            set { m_GpsUpdateClock = value; }
        }

        public string GpsBaudRate
        {
            get { return m_GpsBaudRate; }
            set { m_GpsBaudRate = value; }
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
                double lat=-900;
                try
                {
                    lat = double.Parse(textBoxLatitudeDegrees.Text) + double.Parse(textBoxLatitudeMinutes.Text) / 60;
                    if (comboBoxLatitude.SelectedItem.ToString() == "S") { lat = -lat; }

                    if (lat < -90 || lat > 90) lat = -900;
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
                double log = -900;
                try
                {
                    log = double.Parse(textBoxLongitudeDegrees.Text) + double.Parse(textBoxLongitudeMinutes.Text) / 60;
                    if (comboBoxLongitude.SelectedItem.ToString() == "W") { log = -log; }

                    if (log < -180 || log > 180) log = -900;
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

        public SiteInfo[] Sites
        {
            get
            {
                SiteInfo[] inf = new SiteInfo[4];
                return inf;
            }
            set
            {
                int count = 0;
                foreach (SiteInfo inf in value)
                {

                    if (inf != null && !string.IsNullOrEmpty(inf.Name))
                        comboBoxSites.Items.Add(inf);

                    count ++;
                }
            }
        }


        public int TZ
        {
            get
            {
                if (comboBoxTZ.SelectedIndex >= 0) return comboBoxTZ.SelectedIndex - 11;
                else return (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
            }
            set
            {
                if (value < -11 || value > 14) value = (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
                comboBoxTZ.SelectedIndex = value + 11;
            }
        }

        public bool UseDriverSite
        {
            get { return checkBoxUseDriverSite.Checked; }
            set 
            { 
                checkBoxUseDriverSite.Checked = value;
                if (!value)
                {
                    comboBoxLatitude.Enabled = false;
                    textBoxLatitudeDegrees.ReadOnly = true;
                    textBoxLatitudeMinutes.ReadOnly = true;
                    comboBoxLongitude.Enabled = false;
                    textBoxLongitudeDegrees.ReadOnly = true;
                    textBoxLongitudeMinutes.ReadOnly = true;
                }
                else
                {
                    comboBoxLatitude.Enabled = true;
                    textBoxLatitudeDegrees.ReadOnly = false;
                    textBoxLatitudeMinutes.ReadOnly = false;
                    comboBoxLongitude.Enabled = true;
                    textBoxLongitudeDegrees.ReadOnly = false;
                    textBoxLongitudeMinutes.ReadOnly = false;
                }
            }
        }

        public int TraceLevel
        {
            get { return cbLogging.SelectedIndex-1; }
            set
            {
                cbLogging.SelectedIndex = 0;
                try
                {
                    cbLogging.SelectedIndex = value+1;
                }
                catch { }
            }
        }

        public bool UseDriverTime
        {
            get { return checkBoxUseDriverTime.Checked; }
            set { checkBoxUseDriverTime.Checked = value; }
        }

        public bool ShowHandbox
        {
            get { return checkBoxShowHandbox.Checked; }
            set { checkBoxShowHandbox.Checked = value; }
        }

        public bool AsyncPulseGuide
        {
            get { return chkAsyncPulseGuide.Checked; }
            set { chkAsyncPulseGuide.Checked = value; }
        }

        public bool ReportPierSide
        {
            get { return chkPierSide.Checked; }
            set { chkPierSide.Checked = value; }
        }

        #endregion

        private void TelescopeSetupDialogForm_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);
            m_SaveLatitude = Latitude;
            m_SaveLongitude = Longitude;
            m_SaveUTCOffset = TZ;
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
                    MessageBox.Show(Resources.SomeInvalidSettings, SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            labelUtc.Text = DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString();
            pbSetSiteNow.Visible = GeminiHardware.Connected;
            pbSetTimeNow.Visible = GeminiHardware.Connected;

        }

        private void checkBoxUseGeminiSite_CheckedChanged(object sender, EventArgs e)
        {
            UseDriverSite = checkBoxUseDriverSite.Checked;
        }

        private void buttonGps_Click(object sender, EventArgs e)
        {
            frmGps gpsForm = new frmGps();

            gpsForm.ComPort = m_GpsComPort;
            gpsForm.BaudRate = m_GpsBaudRate;
            gpsForm.UpdateClock = m_GpsUpdateClock;

            DialogResult ans = gpsForm.ShowDialog(this);
            if (ans == DialogResult.OK)
            {
                try
                {
                    m_GpsBaudRate = gpsForm.BaudRate;
                    m_GpsComPort = gpsForm.ComPort;
                    m_GpsUpdateClock = gpsForm.UpdateClock;
                    if (m_GpsUpdateClock) checkBoxUseDriverTime.Checked = false;
                    if (gpsForm.Latitude != 0 && gpsForm.Longitude != 0)
                    {
                        Latitude = gpsForm.Latitude;
                        Longitude = gpsForm.Longitude;
                        checkBoxUseDriverSite.Checked = false;
                    }
                    if (gpsForm.Elevation != SharedResources.INVALID_DOUBLE.ToString()) Elevation = double.Parse(gpsForm.Elevation);
                }
                catch
                {
                }
            }
        }

        private void pbGeminiSettings_Click(object sender, EventArgs e)
        {
            frmAdvancedSettings settings = new frmAdvancedSettings();
            DialogResult re = settings.ShowDialog();
        }

        private void chkJoystick_CheckedChanged(object sender, EventArgs e)
        {
            cmbJoystick.Enabled = chkJoystick.Checked;
        }


        public bool UseJoystick
        {
            get { return chkJoystick.Checked; }
        }

        public string JoystickName
        {
            get {
                if (cmbJoystick.SelectedItem!=null)
                    return cmbJoystick.SelectedItem.ToString();
                return string.Empty;
            }
        }

        private void btnJoysticConfig_Click(object sender, EventArgs e)
        {
            frmJoystickConfig frm = new frmJoystickConfig();
            frm.JoystickName = (cmbJoystick.SelectedIndex >= 0 ? (string)cmbJoystick.SelectedItem : "");
            DialogResult res = frm.ShowDialog(this);
            if (res == DialogResult.OK)
                frm.PersistProfile(true);
        }

       

        private void checkBoxUseDriverTime_CheckedChanged(object sender, EventArgs e)
        {
        }

        

        private void pbSetTimeNow_Click_1(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                try
                {
                    GeminiHardware.UTCDate = DateTime.UtcNow;
                }
                catch
                {
                    MessageBox.Show(Resources.FailedSetTime,SharedResources.TELESCOPE_DRIVER_NAME); 
                }
            }
        }

        private void pbSetSiteNow_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                if (Longitude != -900 && Latitude != -900)
                {
                    try
                    {
                        GeminiHardware.SetLatitude(Latitude);
                        GeminiHardware.SetLongitude(Longitude);
                        GeminiHardware.UTCOffset = -TZ;
                    }
                    catch
                    {
                        MessageBox.Show(Resources.InvalidSite,SharedResources.TELESCOPE_DRIVER_NAME); 
                    }
                }
                else { MessageBox.Show(Resources.InvalidSite, SharedResources.TELESCOPE_DRIVER_NAME); }
            }
        }

        private void chkVoice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVoice.Checked && m_DoneInitialize)
            {
                frmVoice frm = new frmVoice();
                frm.Flags = m_SpeechFlags;
                frm.Voice = m_SpeechVoice;
                DialogResult res = frm.ShowDialog(this);
                if (res == DialogResult.Cancel) chkVoice.Checked = false;
                else
                {
                    m_SpeechVoice = frm.Voice;
                    m_SpeechFlags = frm.Flags;
                }
                m_UseSpeech = chkVoice.Checked;
            }
        }

        private void pbSiteConfig_Click(object sender, EventArgs e)
        {
            if (comboBoxSites.SelectedIndex >= 0 && comboBoxSites.SelectedIndex < 5)
                if (GeminiHardware.SetSiteNumber(comboBoxSites.SelectedIndex))
                {
                    Longitude = GeminiHardware.Longitude;
                    Latitude = GeminiHardware.Latitude;
                    TZ = -GeminiHardware.UTCOffset;
                }
                else if (comboBoxSites.SelectedIndex == 4)  //restore original values
                {
                    GeminiHardware.Longitude = m_SaveLongitude;
                    GeminiHardware.Latitude = m_SaveLatitude;
                    GeminiHardware.UTCOffset = -m_SaveUTCOffset;

                    Longitude = GeminiHardware.Longitude;
                    Latitude = GeminiHardware.Latitude;
                    TZ = -GeminiHardware.UTCOffset;
                }

        }

        private void chkAsyncPulseGuide_CheckedChanged(object sender, EventArgs e)
        {
        }

    }


    public class SiteInfo
    {
        public string Name {get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int UTCOffset { get; set; }
    }

}