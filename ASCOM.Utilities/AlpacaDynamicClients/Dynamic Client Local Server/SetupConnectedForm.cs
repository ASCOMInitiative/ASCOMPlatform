using ASCOM.Common;
using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.DynamicClients
{
    public partial class SetupConnectedForm : Form
    {
        private ILogger TL;
        public SetupConnectedForm(ILogger TL)
        {
            InitializeComponent();
            this.TL = TL;
            TL.LogMessage(LogLevel.Debug, "SetupConnected", $"Connected-setup form created");
        }

        public string HostIpAddress { get; set; }

        public string DeviceType { get; set; }

        public int DeviceNumber { get; set; }

        private void BtnSetupUrlMain_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{HostIpAddress}/setup";
                TL.LogMessage(LogLevel.Debug, "MainSetupURL", $"{setupUrl}");

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = setupUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                TL.LogMessage(LogLevel.Debug, "MainSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSetupUrlDevice_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{HostIpAddress}/setup/v1/{DeviceType.ToLowerInvariant()}/{DeviceNumber}/setup";
                TL.LogMessage(LogLevel.Debug, "DeviceSetupURL", $"{setupUrl}");

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = setupUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                TL.LogMessage(LogLevel.Debug, "ASCOMSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
