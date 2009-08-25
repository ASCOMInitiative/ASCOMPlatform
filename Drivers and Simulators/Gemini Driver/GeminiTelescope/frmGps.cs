using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace ASCOM.GeminiTelescope
{
    public delegate void MessageDelegate(string message);
    public delegate void FormDelegate(string latitude, string longitude);

    public partial class frmGps : Form
    {
        private SerialPort comPort = new SerialPort();
        private NmeaInterpreter interpreter = new NmeaInterpreter();

        
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

            comPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(comPort_DataReceived);

            comboBoxComPort.Items.Add("");
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }
        }

        private void frmGps_Load(object sender, EventArgs e)
        {
            interpreter.PositionReceived += new NmeaInterpreter.PositionReceivedEventHandler(interpreter_PositionReceived);
            interpreter.DateTimeChanged +=new NmeaInterpreter.DateTimeChangedEventHandler(interpreter_DateTimeChanged);
        }
        private void ProcessForm(string latitude, string longitude)
        {
            labelLatitude.Text = "Latitude: " + latitude;
            labelLongitude.Text = "Longitude: " + longitude;
        }
        private void interpreter_PositionReceived(string latitude, string longitude)
        {
            FormDelegate message = new FormDelegate(ProcessForm);
            this.Invoke(message, new Object[] { latitude, longitude });
            
        }
        private void interpreter_DateTimeChanged(System.DateTime dateTime)
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
        public double Latitude
        {
            get
            {
                double lat = 0;
                try
                {
                    
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

                }
                catch { }
                return log;
            }
        }

        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                MessageDelegate message = new MessageDelegate(ProcessMessage);
                //read data waiting in the buffer
                if (comPort.IsOpen)
                {
                    string str = comPort.ReadLine();
                    message.Invoke(str);
                }
            }
            catch { }
            
        }
        private void ProcessMessage(string message)
        {
            try
            {
                interpreter.Parse(message.Substring(0, message.Length - 2));
            }
            catch { }
        }
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (buttonQuery.Text == "Query")
            {
                try
                {
                    if (comboBoxComPort.SelectedItem.ToString() != "")
                    {
                        if (comPort.IsOpen == false)
                        {
                            comPort.PortName = comboBoxComPort.SelectedItem.ToString();
                            comPort.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                            comPort.DataBits = 8;
                            comPort.Parity = Parity.None;
                            comPort.StopBits = StopBits.One;
                            comPort.Handshake = Handshake.None;

                            

                            comPort.Open();

                            comPort.DtrEnable = true;
                            comPort.RtsEnable = true;
                        }
                        buttonQuery.Text = "Stop";
                    }
                    else { MessageBox.Show("Select Com Port"); }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                
            }
            else
            {
                try
                {
                    buttonQuery.Text = "Query";
                    if (comPort.IsOpen == true) comPort.Close();
                    
                }
                catch { }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (comPort.IsOpen == true) comPort.Close();

            }
            catch { }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                if (comPort.IsOpen == true) comPort.Close();

            }
            catch { }
        }

    }
}
