using ASCOM.DriverAccess;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class Form1 : Form
    {
        private const string DEVICE_PROGID = "ASCOM.Simulator.ObservingConditions";
        private ASCOM.DriverAccess.ObservingConditions driver = null;
        private System.Windows.Forms.Timer refreshTimer;

        public Form1()
        {
            InitializeComponent();
            Properties.Settings.Default.DriverId = DEVICE_PROGID;
            labelDriverId.Text = DEVICE_PROGID;
            driver = new ASCOM.DriverAccess.ObservingConditions(DEVICE_PROGID);
            SetUIState();
        }

        #region Event handlers

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Toggle the connected state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToggleConnected_Click(object sender, EventArgs e)
        {
            if (IsConnected) // Currently connected so disconnect
            {
                if (!(refreshTimer is null))
                    StopTimer();

                driver.Connected = false;
                LogMessage("Disconnected OK");
            }
            else // Currently not connected so connect
            {
                try
                {
                    txtStatus.Clear();
                    driver.Connected = true;
                    if (!IsConnected) LogMessage("Connected OK");
                    DisplayProperties();
                }
                catch (Exception ex)
                {
                    LogMessage("Failed to connect");
                    LogMessage("Exception: " + ex.ToString());
                }
            }
            SetUIState();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
                LogMessage("Disconnected OK");

            }

            Application.Exit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                if (driver.Connected) driver.Connected = false;
                SetUIState();
                driver.SetupDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "OCH Test.btnSetup_Click");
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            txtStatus.Clear();
            DisplayProperties();
        }

        private void buttonAutoRefresh_Click(object sender, EventArgs e)
        {
            if (refreshTimer is null)
                StartTimer();
            else
                StopTimer();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            txtStatus.Clear();
            DisplayProperties();
        }

        #endregion

        #region Support code

        private void StartTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 1000; // 1 second refresh interval
            refreshTimer.Start();
            refreshTimer.Tick += RefreshTimer_Tick;
            buttonAutoRefresh.Text = "Stop Refresh";
        }

        private void StopTimer()
        {
            refreshTimer.Tick -= RefreshTimer_Tick;
            refreshTimer.Stop();
            refreshTimer.Dispose();
            refreshTimer = null;
            buttonAutoRefresh.Text = "Auto Refresh";
        }

        private void SetUIState()
        {
            buttonToggleConnected.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonToggleConnected.Text = IsConnected ? "Disconnect" : "Connect";
            buttonRefresh.Enabled = IsConnected;
            buttonAutoRefresh.Enabled = IsConnected;
            btnSetup.Enabled = !IsConnected;
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }

        private void LogMessage(string message)
        {
            if (txtStatus.Text == "") txtStatus.Text = message;
            else txtStatus.Text = txtStatus.Text + "\r\n" + message;
        }

        private void DisplayProperties()
        {
            LogMessage(DateTime.Now.ToString());
            LogMessage(driver.DriverInfo);
            ListProperty("AveragePeriod");
            ListProperty("CloudCover");
            ListProperty("DewPoint");
            ListProperty("Humidity");
            ListProperty("Pressure");
            ListProperty("RainRate");
            ListProperty("SkyBrightness");
            ListProperty("SkyQuality");
            ListProperty("SkyTemperature");
            ListProperty("StarFWHM");
            ListProperty("Temperature");
            ListProperty("WindDirection");
            ListProperty("WindGust");
            ListProperty("WindSpeed");
            LogMessage(" ");

            DisplaySensorDescription("CloudCover");
            DisplaySensorDescription("DewPoint");
            DisplaySensorDescription("Humidity");
            DisplaySensorDescription("Pressure");
            DisplaySensorDescription("RainRate");
            DisplaySensorDescription("SkyBrightness");
            DisplaySensorDescription("SkyQuality");
            DisplaySensorDescription("SkyTemperature");
            DisplaySensorDescription("StarFWHM");
            DisplaySensorDescription("Temperature");
            DisplaySensorDescription("WindDirection");
            DisplaySensorDescription("WindGust");
            DisplaySensorDescription("WindSpeed");
            LogMessage(" ");

            try { LogMessage("TimeSinceLastUpdate : " + driver.TimeSinceLastUpdate("").ToString("0.00")); } catch (Exception ex) { LogMessage(ex.Message); }
            DisplayTimeSinceLastUpdate("CloudCover");
            DisplayTimeSinceLastUpdate("DewPoint");
            DisplayTimeSinceLastUpdate("Humidity");
            DisplayTimeSinceLastUpdate("Pressure");
            DisplayTimeSinceLastUpdate("RainRate");
            DisplayTimeSinceLastUpdate("SkyBrightness");
            DisplayTimeSinceLastUpdate("SkyQuality");
            DisplayTimeSinceLastUpdate("SkyTemperature");
            DisplayTimeSinceLastUpdate("StarFWHM");
            DisplayTimeSinceLastUpdate("Temperature");
            DisplayTimeSinceLastUpdate("WindDirection");
            DisplayTimeSinceLastUpdate("WindGust");
            DisplayTimeSinceLastUpdate("WindSpeed");
            LogMessage(" ");

        }

        private void DisplaySensorDescription(string sensor)
        {
            try
            {
                LogMessage(sensor + " Description : " + driver.SensorDescription(sensor));
            }
            catch (MethodNotImplementedException)
            {
                LogMessage(sensor + " is not implemented");
            }
            catch (InvalidOperationException)
            {
                LogMessage("Invalid operation, " + sensor + " is not ready");
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
        }

        private void DisplayTimeSinceLastUpdate(string sensor)
        {
            try
            {
                LogMessage(sensor + " Time since last update: " + driver.TimeSinceLastUpdate(sensor).ToString("0.00"));
            }
            catch (MethodNotImplementedException)
            {
                LogMessage(sensor + " is not implemented");
            }
            catch (InvalidOperationException)
            {
                LogMessage("Invalid operation, " + sensor + " is not ready");
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
        }

        private void ListProperty(string PropertyName)
        {
            try
            {
                Type type = typeof(ObservingConditions);
                PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                var observingConditionsValue = (double)propertyInfo.GetValue(driver, null);
                LogMessage(PropertyName + " : " + observingConditionsValue.ToString("0.000"));
            }

            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is PropertyNotImplementedException) LogMessage(PropertyName + " - This property is not implemented");
                else if (ex.InnerException is InvalidOperationException) LogMessage("Invalid operation, " + PropertyName + " is not ready");
                else if (ex.InnerException is DriverAccessCOMException)
                {
                    if (((DriverAccessCOMException)ex.InnerException).ErrorCode == ErrorCodes.InvalidOperationException)
                        LogMessage("Invalid operation (COM), " + PropertyName + " is not ready");
                }
                else LogMessage(ex.ToString());
            }
            catch (PropertyNotImplementedException)
            {
                LogMessage(PropertyName + " - This property is not implemented");
            }
            catch (InvalidOperationException)
            {
                LogMessage(PropertyName + " - Invalid operation at this time");
            }
            catch (Exception ex)
            {
                LogMessage(PropertyName + " " + ex.ToString());
                if (ex.InnerException != null)
                {
                    LogMessage("   " + ex.InnerException.Message);
                }
            }

        }

        #endregion

    }
}
