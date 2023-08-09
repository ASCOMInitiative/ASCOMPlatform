using ASCOM.Utilities;
using System;
using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Create device form
    /// </summary>
    public partial class CreateDeviceForm : Form
    {
        private string deviceType;
        private int deviceNumber;
        private string progId;
        private string localServerPath;
        private TraceLogger TL;

        /// <summary>
        /// Base initialiser for the create client form
        /// </summary>
        public CreateDeviceForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise a create client form with supplied parameters
        /// </summary>
        /// <param name="deviceTypeParameter">ASCOM device type e.g. Telescope, Focuser etc.</param>
        /// <param name="deviceNumberParameter">Dynamic client driver number</param>
        /// <param name="progIdParameter">Dynamic client ProgID</param>
        /// <param name="localServerPathParameter">Path to the local server that will register the driver</param>
        /// <param name="TLParameter">Trace logger with which to create log messages</param>
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

        /// <summary>
        /// OK button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Check whether there is a device description
            if (!string.IsNullOrEmpty(TxtDeviceDescription.Text)) // There is a device description
            {
                // Create the new Alpaca client using the supplied parameters
                CreateAlpacaClients.CreateAlpacaClient(deviceType, deviceNumber, progId, TxtDeviceDescription.Text, localServerPath);

                // Create a pointer to the local server executable
                string localServerExe = $"{localServerPath}{SharedConstants.ALPACA_CLIENT_LOCAL_SERVER}";
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

        /// <summary>
        /// Cancel button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TL.LogMessage("CreateAlpacaClient", $"User pressed the Cancel button so close this application without doing anything.");
            Application.Exit(); // Close the application
        }
    }
}