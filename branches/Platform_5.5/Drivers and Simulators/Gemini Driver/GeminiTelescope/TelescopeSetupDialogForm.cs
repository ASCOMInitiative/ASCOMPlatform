using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.GeminiTelescope.Properties;
using System.Linq;
using System.Collections;

namespace ASCOM.GeminiTelescope
{

    [ComVisible(false)]					// Form not registered for COM!
    public partial class TelescopeSetupDialogForm : Form
    {
        private bool m_DoneInitialize;

        private bool m_UseSpeech = false;
        private double m_SaveLatitude;
        private double m_SaveLongitude;
        private int m_SaveUTCOffset;

        private string m_PreviousComPort = null;

        private ArrayList m_OpticsInfos = new ArrayList();

        private bool m_SelectedOpticChanged = false;

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
            InitializeLocal();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }

            // if a network connection is available, add 'Ethernet' for Gemini II 
            if (System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Any(
                x => x.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up))
            {
                comboBoxComPort.Items.Add("Ethernet");
            }

            string[] joys = Joystick.JoystickNames;
            if (joys != null)
            {
                foreach (string s in Joystick.JoystickNames)
                {
                    cmbJoystick.Items.Add(s);
                }
                cmbJoystick.SelectedItem = GeminiHardware.Instance.JoystickName;

                chkJoystick.CheckState = GeminiHardware.Instance.UseJoystick ? CheckState.Checked : CheckState.Unchecked;
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

            chkVoice.Checked = GeminiHardware.Instance.UseSpeech;
            m_SpeechVoice = GeminiHardware.Instance.SpeechVoice;
            m_SpeechFlags = GeminiHardware.Instance.SpeechFilter;

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
       
        public GeminiHardwareBase.GeminiBootMode BootMode
        {
            get
            {
                if (radioButtonPrompt.Checked)
                {
                    return GeminiHardwareBase.GeminiBootMode.Prompt;
                }
                else if (radioButtonColdStart.Checked)
                {
                    return GeminiHardwareBase.GeminiBootMode.ColdStart;
                }
                else if (radioButtonWarmStart.Checked)
                {
                    return GeminiHardwareBase.GeminiBootMode.WarmStart;
                }
                else if (radioButtonWarmRestart.Checked)
                {
                    return GeminiHardwareBase.GeminiBootMode.WarmRestart;
                }
                else
                {
                    return GeminiHardwareBase.GeminiBootMode.Prompt;
                }
            }
            set
            {
                switch (value)
                {
                    case GeminiHardwareBase.GeminiBootMode.Prompt:
                        radioButtonPrompt.Checked = true;
                        break;
                    case GeminiHardwareBase.GeminiBootMode.ColdStart:
                        radioButtonColdStart.Checked = true;
                        break;
                    case GeminiHardwareBase.GeminiBootMode.WarmStart:
                        radioButtonWarmStart.Checked = true;
                        break;
                    case GeminiHardwareBase.GeminiBootMode.WarmRestart:
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
        public void AddOpticsInfo(OpticsInfo oi)
        {
            m_OpticsInfos.Add(oi);
            comboBoxSavedOptics.Items.Add(oi.Name);
        }
        public void RemoveOpticsInfo(int i)
        {
            m_OpticsInfos.RemoveAt(i);
        }
        public OpticsInfo GetOpticsInfo(int i)
        {
            return (OpticsInfo)m_OpticsInfos[i];
        }
        public int SelectedOptic
        {
            get { return comboBoxSavedOptics.SelectedIndex; }
            set 
            {
                try
                {
                    comboBoxSavedOptics.SelectedIndex = value;
                    OpticsInfo oi = (OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex];

                    if (oi.UnitOfMeasure == "inches")
                    {
                        radioButtonInches.Checked = true;
                        textBoxAperture.Text = (oi.ApertureDiameter / 25.4).ToString();
                        textBoxFocalLength.Text = (oi.FocalLength / 25.4).ToString();
                    }
                    else
                    {
                        radioButtonmillimeters.Checked = true;
                        textBoxAperture.Text = oi.ApertureDiameter.ToString();
                        textBoxFocalLength.Text = oi.FocalLength.ToString();
                    }
                }
                catch 
                { 
                    m_OpticsInfos.Clear();
                    OpticsInfo oi = new OpticsInfo();
                    oi.Name = "";
                    oi.FocalLength = 0;
                    oi.UnitOfMeasure = "millimeters";
                    oi.ApertureDiameter = 0;
                    oi.ObstructionDiameter = 0;
                    m_OpticsInfos.Add(oi);
                    comboBoxSavedOptics.SelectedIndex = 0;
                }
            }
        }
        public ArrayList OpticsInfos
        {
            get { return m_OpticsInfos; }
            set 
            { 
                m_OpticsInfos = value;
                comboBoxSavedOptics.Items.Clear();
                for (int i=0; i<m_OpticsInfos.Count; i++)
                {
                    comboBoxSavedOptics.Items.Add(((OpticsInfo)m_OpticsInfos[i]).Name);
                }
            }
        }
        public void ClearOpticsInfos()
        {
            m_OpticsInfos.Clear();
            comboBoxSavedOptics.Items.Clear();
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

        public bool PrecisionPulseGuide
        {
            get { return chkPrecisionPulse.Checked; }
            set { chkPrecisionPulse.Checked = value; }
        }

        #endregion

        private void TelescopeSetupDialogForm_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);
            m_SaveLatitude = Latitude;
            m_SaveLongitude = Longitude;
            m_SaveUTCOffset = TZ;

            // Gemini L5 and above: support only precision pulse guiding:
            if (GeminiHardware.Instance.GeminiLevel > 4)
            {
                this.chkPrecisionPulse.Checked = true;
                this.chkAsyncPulseGuide.Checked = true;
                this.chkAsyncPulseGuide.Enabled = false;
                this.chkPrecisionPulse.Enabled = false;
            }
        }

        private void buttonVirtualPort_Click(object sender, EventArgs e)
        {
            frmPassThroughPortSetup frm = new frmPassThroughPortSetup();
            DialogResult res = frm.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    GeminiHardware.Instance.PassThroughPortEnabled = frm.VirtualPortEnabled;
                    GeminiHardware.Instance.PassThroughComPort = frm.ComPort;
                    GeminiHardware.Instance.PassThroughBaudRate = int.Parse(frm.BaudRate);
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
            pbSetSiteNow.Visible = GeminiHardware.Instance.Connected;
            pbSetTimeNow.Visible = GeminiHardware.Instance.Connected;

        }

        private void checkBoxUseGeminiSite_CheckedChanged(object sender, EventArgs e)
        {
            UseDriverSite = checkBoxUseDriverSite.Checked;
        }

        private void buttonGps_Click(object sender, EventArgs e)
        {
            frmGps gpsForm = new frmGps();

            gpsForm.ComPort = GeminiHardware.Instance.GpsComPort;
            gpsForm.BaudRate = GeminiHardware.Instance.GpsBaudRate.ToString();
            gpsForm.UpdateClock = GeminiHardware.Instance.GpsUpdateClock;

            DialogResult ans = gpsForm.ShowDialog(this);
            if (ans == DialogResult.OK)
            {
                try
                {
                    string error = "";
                    int gpsBaudRate;
                    if (!int.TryParse(gpsForm.BaudRate, out gpsBaudRate))
                        error += Resources.GPS + " " + Resources.BaudRate + ", ";
                    else
                        GeminiHardware.Instance.GpsBaudRate = gpsBaudRate;
                    try { GeminiHardware.Instance.GpsComPort = gpsForm.ComPort; }
                    catch { error += Resources.GPS + " " + Resources.COMport + ", "; }
                    GeminiHardware.Instance.GpsUpdateClock = gpsForm.UpdateClock;

                    if (gpsForm.UpdateClock) checkBoxUseDriverTime.Checked = false;
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
            if (GeminiHardware.Instance.Connected)
            {
                try
                {
                    GeminiHardware.Instance.UTCDate = DateTime.UtcNow;
                }
                catch
                {
                    MessageBox.Show(Resources.FailedSetTime,SharedResources.TELESCOPE_DRIVER_NAME); 
                }
            }
        }

        private void pbSetSiteNow_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                if (Longitude != -900 && Latitude != -900)
                {
                    try
                    {
                        GeminiHardware.Instance.SetLatitude(Latitude);
                        GeminiHardware.Instance.SetLongitude(Longitude);
                        GeminiHardware.Instance.UTCOffset = -TZ;
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
                if (GeminiHardware.Instance.SetSiteNumber(comboBoxSites.SelectedIndex))
                {
                    Longitude = GeminiHardware.Instance.Longitude;
                    Latitude = GeminiHardware.Instance.Latitude;
                    TZ = -GeminiHardware.Instance.UTCOffset;
                }
                else if (comboBoxSites.SelectedIndex == 4)  //restore original values
                {
                    GeminiHardware.Instance.Longitude = m_SaveLongitude;
                    GeminiHardware.Instance.Latitude = m_SaveLatitude;
                    GeminiHardware.Instance.UTCOffset = -m_SaveUTCOffset;

                    Longitude = GeminiHardware.Instance.Longitude;
                    Latitude = GeminiHardware.Instance.Latitude;
                    TZ = -GeminiHardware.Instance.UTCOffset;
                }

        }

        private void chkAsyncPulseGuide_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkPortScan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxComPort.Text == "Ethernet" && m_PreviousComPort!=null)
            {
                Speech.SayIt(Resources.EthernetSettings, Speech.SpeechType.Command);

                //configure ethernet connection for Gemini II
                frmNetworkSettings net = new frmNetworkSettings();
                DialogResult res = net.ShowDialog(this);
            }
            m_PreviousComPort = comboBoxComPort.Text;

            comboBoxBaudRate.Enabled = (comboBoxComPort.Text != "Ethernet");
        }

        private void chkPierSide_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pbEditSavedOptics_Click(object sender, EventArgs e)
        {
            frmOpticsSetup opticsForm = new frmOpticsSetup();
            opticsForm.OpticInfos = m_OpticsInfos;
            DialogResult ans = opticsForm.ShowDialog();
            if (ans == DialogResult.OK)
            {
                OpticsInfos = opticsForm.OpticInfos;
            }
        }

        private void comboBoxSavedOptics_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SelectedOpticChanged = true;

            OpticsInfo oi = (OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex];
            if (oi.UnitOfMeasure == "inches")
            {
                radioButtonInches.Checked = true;
                textBoxAperture.Text = (oi.ApertureDiameter / 25.4).ToString();
                textBoxFocalLength.Text = (oi.FocalLength / 25.4).ToString();
                textBoxObstruction.Text = (oi.ObstructionDiameter / 25.4).ToString();
            }
            else
            {
                radioButtonmillimeters.Checked = true;
                textBoxAperture.Text = oi.ApertureDiameter.ToString();
                textBoxFocalLength.Text = oi.FocalLength.ToString();
                textBoxObstruction.Text = oi.ObstructionDiameter.ToString();
            }

            m_SelectedOpticChanged = false;
        }

        private void textBoxFocalLength_TextChanged(object sender, EventArgs e)
        {
            if (!m_SelectedOpticChanged && textBoxFocalLength.Text != "")
            {
                if (radioButtonInches.Checked) ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).FocalLength = (double.Parse(textBoxFocalLength.Text)) * 25.4;
                else ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).FocalLength = double.Parse(textBoxFocalLength.Text);
            }
        }

        private void textBoxAperture_TextChanged(object sender, EventArgs e)
        {
            if (!m_SelectedOpticChanged && textBoxAperture.Text != "")
            {
                if (radioButtonInches.Checked) ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).ApertureDiameter = (double.Parse(textBoxAperture.Text)) * 25.4;
                else ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).ApertureDiameter = double.Parse(textBoxAperture.Text);
            }
        }

        private void radioButtonInches_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_SelectedOpticChanged)
            {
                if (radioButtonInches.Checked) ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).UnitOfMeasure = "inches";
                else ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).UnitOfMeasure = "millimeters";
            }
        }

        private void textBoxFocalLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;

            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (Char.IsDigit(e.KeyChar))
            {
            }
            else if (e.KeyChar == (char)8)
            {
            }
            else if (e.KeyChar.ToString() == Keys.Delete.ToString()) { }
            else
            {
                e.Handled = true;
            }
        }

        private void textBoxAperture_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;

            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (Char.IsDigit(e.KeyChar))
            {
            }
            else if (e.KeyChar == (char)8)
            {
            }
            else if (e.KeyChar.ToString() == Keys.Delete.ToString()) { }
            else
            {
                e.Handled = true;
            }
        }

        private void textBoxObstruction_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;

            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (Char.IsDigit(e.KeyChar))
            {
            }
            else if (e.KeyChar == (char)8)
            {
            }
            else if (e.KeyChar.ToString() == Keys.Delete.ToString()) { }
            else
            {
                e.Handled = true;
            }
        }

        private void textBoxObstruction_TextChanged(object sender, EventArgs e)
        {
            if (!m_SelectedOpticChanged && textBoxObstruction.Text != "")
            {

                if (radioButtonInches.Checked) ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).ObstructionDiameter = (double.Parse(textBoxObstruction.Text)) * 25.4;
                else ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).ObstructionDiameter = double.Parse(textBoxObstruction.Text);
                if (radioButtonInches.Checked) ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).UnitOfMeasure = "inches";
                else ((OpticsInfo)m_OpticsInfos[comboBoxSavedOptics.SelectedIndex]).UnitOfMeasure = "millimeters";
            }
        }

    }


    public class SiteInfo
    {
        public string Name {get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int UTCOffset { get; set; }
    }

    public class OpticsInfo
    {
        public string Name { get; set; }
        public double FocalLength { get; set; }
        public double ApertureDiameter { get; set; }
        public double ObstructionDiameter { get; set; }
        public string UnitOfMeasure { get; set; }
    }

}