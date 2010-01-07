using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    
    public delegate void FormDelegate(string latitude, string longitude, string elevation);

    public partial class frmGps : Form
    {
        
        private NmeaInterpreter interpreter = new NmeaInterpreter();
        private string m_Latitude;
        private string m_Longitude;
        private string m_Elevation;
        
        public struct SystemTime

        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        };

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        public frmGps()
        {
            InitializeComponent();

            

            comboBoxComPort.Items.Add("");
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }
            buttonQuery.Text = Resources.Query;
        }

        private void frmGps_Load(object sender, EventArgs e)
        {
            interpreter.PositionReceived += new NmeaInterpreter.PositionReceivedEventHandler(interpreter_PositionReceived);
            interpreter.DateTimeChanged +=new NmeaInterpreter.DateTimeChangedEventHandler(interpreter_DateTimeChanged);
            interpreter.FixLost += new NmeaInterpreter.FixLostEventHandler(interpreter_FixLost);
            interpreter.FixObtained += new NmeaInterpreter.FixObtainedEventHandler(interpreter_FixObtained);
        }
        private void ProcessForm(string latitude, string longitude, string elevation)
        {
            

            m_Latitude = latitude.Substring(1);
            m_Longitude = longitude.Substring(1);

            if (latitude.Substring(0, 1) == "S") m_Latitude = "-" + m_Latitude;
            if (longitude.Substring(0, 1) == "W") m_Longitude = "-" + m_Longitude;

            try
            {
                labelLatitude.Text = Resources.Latitude + ": " + GeminiHardware.m_Util.DegreesToDMS(GeminiHardware.m_Util.DMSToDegrees(m_Latitude));
                labelLongitude.Text = Resources.Longitude + ": " + GeminiHardware.m_Util.DegreesToDMS(GeminiHardware.m_Util.DMSToDegrees(m_Longitude));
            }
            catch { }
            
            if (elevation != SharedResources.INVALID_DOUBLE.ToString()) labelElevation.Text = Resources.Elevation + ": " + elevation;

            m_Elevation = elevation;
            
        }
        private void interpreter_PositionReceived(string latitude, string longitude, string elevation)
        {
            FormDelegate message = new FormDelegate(ProcessForm);
            this.Invoke(message, new Object[] { latitude, longitude, elevation });
            
        }
        private void interpreter_DateTimeChanged(System.DateTime dateTime)
        {
            if (checkBoxUpdateClock.Checked)
            {
                SystemTime updatedTime = new SystemTime();
                updatedTime.Year = (ushort)dateTime.ToUniversalTime().Year;
                updatedTime.Month = (ushort)dateTime.ToUniversalTime().Month;
                updatedTime.Day = (ushort)dateTime.ToUniversalTime().Day;
                updatedTime.Hour = (ushort)dateTime.ToUniversalTime().Hour;
                updatedTime.Minute = (ushort)dateTime.ToUniversalTime().Minute;
                updatedTime.Second = (ushort)dateTime.ToUniversalTime().Second;
                Win32SetSystemTime(ref updatedTime);
            }
        }
        public double Latitude
        {
            get
            {
                double lat = 0;
                try
                {
                    lat = GeminiHardware.m_Util.DMSToDegrees(m_Latitude);
                }
                catch { }
                return lat;
            }
        }
        public double Longitude
        {
            get
            {
                double log = 0;
                try
                {
                    log = GeminiHardware.m_Util.DMSToDegrees(m_Longitude);
                }
                catch { }
                return log;
            }
        }
        public string Elevation { get { return m_Elevation; } }

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
        public bool UpdateClock
        {
            get { return checkBoxUpdateClock.Checked; }
            set { checkBoxUpdateClock.Checked = value; }
        }
        
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (buttonQuery.Text == Resources.Query)
            {
                if (comboBoxComPort.SelectedItem == null) { MessageBox.Show(Resources.SelectCOMPort); return; }
                try
                {
                   
                        interpreter.ComPort = comboBoxComPort.SelectedItem.ToString();
                        interpreter.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                        interpreter.Conneced = true;
                        buttonQuery.Text = Resources.Stop;
                   
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                
            }
            else
            {
                try
                {
                    buttonQuery.Text = Resources.Query;
                    interpreter.Conneced = false;
                    
                }
                catch { }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {

                interpreter.Conneced = false;

            }
            catch { }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                interpreter.Conneced = false;

            }
            catch { }
        }

        private void interpreter_FixLost()
        {
            this.pictureBox1.Image = global::ASCOM.GeminiTelescope.Properties.Resources.no_satellite;
        }
        private void interpreter_FixObtained()
        {
            this.pictureBox1.Image = global::ASCOM.GeminiTelescope.Properties.Resources.satellite;
        }
    }
}
