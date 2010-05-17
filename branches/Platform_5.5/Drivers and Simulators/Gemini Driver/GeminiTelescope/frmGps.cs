//tabs=4
// --------------------------------------------------------------------------------
//
// GPS query window
//
// Description:	
//
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//              (pk)  Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-JUL-2009	rbt	1.0.0	Initial implementation
// 29-MAR-2010  pk  1.0.3   Changed GPS Lat/Long/Elevation data to proper local
//                          decimal separator
// 16-MAY-2010  mc  1.0.7   Added EventHandlers for InvalidData and DataTimeout
// --------------------------------------------------------------------------------
//
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
    public delegate void StatusDelegate(string status, Boolean blankFields, int icon);
    public delegate void TimeUpdateDelegate(DateTime tm);

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
            labelStatus.Text = Resources.Status + ": ";
            labelLatitude.Text = Resources.Latitude + ": ";
            labelLongitude.Text = Resources.Longitude + ": ";
            labelElevation.Text = Resources.Elevation + ": ";

        }

        private void frmGps_Load(object sender, EventArgs e)
        {
            interpreter.PositionReceived += new NmeaInterpreter.PositionReceivedEventHandler(interpreter_PositionReceived);
            interpreter.DateTimeChanged +=new NmeaInterpreter.DateTimeChangedEventHandler(interpreter_DateTimeChanged);
            interpreter.FixLost += new NmeaInterpreter.FixLostEventHandler(interpreter_FixLost);
            interpreter.FixObtained += new NmeaInterpreter.FixObtainedEventHandler(interpreter_FixObtained);
            interpreter.InvalidData += new NmeaInterpreter.InvalidDataEventHandler(interpreter_InvalidData);
            interpreter.DataTimeout += new NmeaInterpreter.DataTimeoutEventHandler(interpreter_DataTimeout);

        }
        private void ProcessForm(string latitude, string longitude, string elevation)
        {
            

            m_Latitude = latitude.Substring(1);
            m_Longitude = longitude.Substring(1);

            // GPS data contains '.' as the decimal separator. To make ASCOM conversion functions work for the current locale,
            // need to replace '.' with the correct local decimal separator [pk: 2010-03-29]
            string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (sep != ".")
            {
                m_Latitude = m_Latitude.Replace(".", sep);
                m_Longitude = m_Longitude.Replace(".", sep);                
                elevation = elevation.Replace(".", sep);
            }

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
            this.BeginInvoke(message, new Object[] { latitude, longitude, elevation });          
        }

        private void ProcessStatus(string status, Boolean blankFields, int icon)
        {
            labelStatus.Text = Resources.Status + ": " + status;
            if (blankFields)
            {
                labelLatitude.Text = Resources.Latitude + ": ";
                labelLongitude.Text = Resources.Longitude + ": ";
                labelElevation.Text = Resources.Elevation + ": ";
            }
            if (icon == 1) //not connected
            {
                pictureBox1.Image = Resources.no_satellite;
            }
            else if (icon == 2)
            {
                pictureBox1.Image = Resources.no_fix_satellite;
            }
            else if (icon == 3)
            {
                pictureBox1.Image = Resources.satellite;
            }

        }


        private void setTime(System.DateTime dateTime)
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

        private void interpreter_DateTimeChanged(System.DateTime dateTime)
        {
            this.BeginInvoke(new TimeUpdateDelegate(setTime), dateTime);
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
                        interpreter.Connected = true;
                        buttonQuery.Text = Resources.Stop;
                        labelStatus.Text = Resources.Status + ": " + Resources.WaitingForData;                  
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                
            }
            else
            {
                try
                {
                    buttonQuery.Text = Resources.Query;
                    interpreter.Connected = false;
                    pictureBox1.Image = global::ASCOM.GeminiTelescope.Properties.Resources.no_satellite;
                    labelStatus.Text = Resources.Status + ": ";
                }
                catch { }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {

                interpreter.Connected = false;

            }
            catch { }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                interpreter.Connected = false;

            }
            catch { }
        }

        private void interpreter_FixLost()
        {
            StatusDelegate message = new StatusDelegate(ProcessStatus);
            this.BeginInvoke(message, new Object[] { global::ASCOM.GeminiTelescope.Properties.Resources.GPSNoFix, true, 2 });
        }
        private void interpreter_FixObtained()
        {
            StatusDelegate message = new StatusDelegate(ProcessStatus);
            this.BeginInvoke(message, new Object[] { global::ASCOM.GeminiTelescope.Properties.Resources.DataOK, false, 3 });
        }

        private void interpreter_InvalidData()
        {
            StatusDelegate message = new StatusDelegate(ProcessStatus);
            this.BeginInvoke(message, new Object[] { global::ASCOM.GeminiTelescope.Properties.Resources.InvalidDataReceived, true, 2 });
        }

        private void interpreter_DataTimeout()
        {
            StatusDelegate message = new StatusDelegate(ProcessStatus);
            this.BeginInvoke(message, new Object[] { global::ASCOM.GeminiTelescope.Properties.Resources.WaitingForData, true, 1 });
        }

        private void frmGps_FormClosing(object sender, FormClosingEventArgs e)
        {
            interpreter.Connected = false;
            interpreter = null;

        }

    }
}
