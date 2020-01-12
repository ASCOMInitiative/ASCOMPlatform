using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    public partial class CreateDeviceForm : Form
    {
        private string deviceType;
        private int deviceNumber;
        private string progId;
        private string localServerPath;
        private TraceLogger TL;

        public CreateDeviceForm()
        {
            InitializeComponent();
        }

        public CreateDeviceForm(string deviceTypeParameter, int deviceNumberParameter, string progIdParameter, string localServerPathParameter, TraceLogger TLParameter) : this()
        {
            // Save the supplied parameters
            deviceType = deviceTypeParameter;
            deviceNumber = deviceNumberParameter;
            progId = progIdParameter;
            localServerPath = localServerPathParameter;

            // Save trace logger and log the supplied parameters
            TL = TLParameter;
            TL.LogMessage("CreateDeviceForm", $"Parameters - Device type: { deviceType}, Device number: { deviceNumber}, ProgID: { progId}, Local server path: {localServerPath}");
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Check whether there is a device description
            if (!string.IsNullOrEmpty(TxtDeviceDescription.Text)) // There is a device description
            {
                // Create the new Alpaca client using the supplied parameters
                CreateAlpacaClients.CreateAlpacaClient(deviceType, deviceNumber, progId, TxtDeviceDescription.Text, localServerPath);

                // Create a pointer to the local server executable
                string localServerExe = $"{localServerPath}\\{CreateAlpacaClients.LOCALSERVER_EXE_NAME}";
                TL.LogMessage("CreateAlpacaClient", $"Alpaca local server exe name: {localServerExe}");

                // Run the local server to register the new drivers
                CreateAlpacaClients.RunLocalServer(localServerExe, "-regserver", TL);

                Application.Exit(); // Close the application
            }
            else // There is no device description
            {
                MessageBox.Show("The device description field must contain some text", "Validation Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TL.LogMessage("CreateAlpacaClient", $"User pressed the Cancel button so close this application without doing anything.");
            Application.Exit(); // Close the application
        }
    }
}