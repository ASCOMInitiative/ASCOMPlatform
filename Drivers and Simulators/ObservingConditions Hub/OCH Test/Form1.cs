using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class Form1 : Form
    {
        private const string DEVICE_PROGID = "ASCOM.OCH.ObservingConditions";
        private ASCOM.DriverAccess.ObservingConditions driver = null;

        public Form1()
        {
            InitializeComponent();
            Properties.Settings.Default.DriverId = DEVICE_PROGID;
            labelDriverId.Text = DEVICE_PROGID;
            driver = new ASCOM.DriverAccess.ObservingConditions(DEVICE_PROGID);
            SetUIState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.ObservingConditions.Choose(DEVICE_PROGID);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
                LogMessage("Disconnected OK");
            }
            else
            {
                try
                {
                    txtStatus.Clear();
                    driver.Connected = true;
                    LogMessage("Connected OK");
                    LogMessage(driver.DriverInfo);
                    try { LogMessage("Average period: " + driver.AveragePeriod); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("CloudCover : " + driver.CloudCover); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("DewPoint : " + driver.DewPoint); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Humidity : " + driver.Humidity); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Pressure : " + driver.Pressure); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("RainRate : " + driver.RainRate); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("CloudCover Description : " + driver.SensorDescription("CloudCover")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("DewPoint Description : " + driver.SensorDescription("DewPoint")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Humidity Description : " + driver.SensorDescription("Humidity")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Pressure Description : " + driver.SensorDescription("Pressure")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("RainRate Description : " + driver.SensorDescription("RainRate")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyBrightness Description : " + driver.SensorDescription("SkyBrightness")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyQuality Description : " + driver.SensorDescription("SkyQuality")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkySeeing Description : " + driver.SensorDescription("SkySeeing")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyTemperature Description : " + driver.SensorDescription("SkyTemperature")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Temperature Description : " + driver.SensorDescription("Temperature")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindDirection Description : " + driver.SensorDescription("WindDirection")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindGust Description : " + driver.SensorDescription("WindGust")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindSpeed Description : " + driver.SensorDescription("WindSpeed")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyBrightness : " + driver.SkyBrightness); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyQuality : " + driver.SkyQuality); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkySeeing : " + driver.SkySeeing); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("SkyTemperature : " + driver.SkyTemperature); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("Temperature : " + driver.Temperature); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("TimeSinceLastUpdate : " + driver.TimeSinceLastUpdate("")); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindDirection : " + driver.WindDirection); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindGust : " + driver.WindGust); } catch (Exception ex) { LogMessage(ex.Message); }
                    try { LogMessage("WindSpeed : " + driver.WindSpeed); } catch (Exception ex) { LogMessage(ex.Message); }

                }
                catch (Exception ex)
                {
                    LogMessage("Failed to connect");
                    LogMessage("Exception: " + ex.Message);
                }
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
                LogMessage("Disconnected OK");
            }
            //driver.Dispose();

            Application.Exit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                //driver = new ASCOM.DriverAccess.ObservingConditions(Properties.Settings.Default.DriverId);
                if (driver.Connected) driver.Connected = false;
                SetUIState();
                driver.SetupDialog();
                //driver.Dispose();
                //driver = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
