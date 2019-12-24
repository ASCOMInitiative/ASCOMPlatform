using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Forms;
using ASCOM.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace ASCOM.Remote
{
    public partial class ServerConfigurationForm : Form
    {
        TraceLoggerPlus TL;
        List<string> registeredDeviceTypes = new List<string>();
        // Create a dictionary to hold the current device instance numbers of every device type
        Dictionary<string, int> deviceNumberIndexes;
        internal static Dictionary<string, ConfiguredDevice> configuredDevices;
        private string serviceType;
        private string ipAddressString;
        private decimal portNumber;
        private string userName;
        private string password;

        public ServerConfigurationForm()
        {
            InitializeComponent();
        }

        public ServerConfigurationForm(TraceLoggerPlus Logger, string serviceType, string ipAddressString, decimal portNumber, string userName, string password) : this()
        {
            TL = Logger;
            TL.LogMessage("ServerConfigurationForm", "Form initialiser starting");
            deviceNumberIndexes = new Dictionary<string, int>();
            configuredDevices = new Dictionary<string, ConfiguredDevice>();
            this.serviceType = serviceType;
            this.ipAddressString = ipAddressString;
            this.portNumber = portNumber;
            this.userName = userName;
            this.password = password;
            TL.LogMessage("ServerConfigurationForm", "Form initialiser completed");
        }

        public void RecalculateDeviceNumbers()
        {
            TL.LogMessage("RecalculateDeviceNumbers", "Start of RecalculateDeviceNumbers");

            // Add a zero entry for each device type on this system
            deviceNumberIndexes.Clear();
            foreach (string device in registeredDeviceTypes)
            {
                deviceNumberIndexes.Add(device, 0);
            }
            TL.LogMessage(0, 0, 0, "RecalculateDeviceNumbers", "Initialised device numbers");

            foreach (string deviceType in registeredDeviceTypes)
            {
                TL.LogMessage(0, 0, 0, "RecalculateDeviceNumbers", "Processing device type: " + deviceType);
                SortedDictionary<string, ServedDeviceClient> servedDevices = new SortedDictionary<string, ServedDeviceClient>();
                foreach (ServedDeviceClient c in this.Controls.OfType<ServedDeviceClient>().Where(device => device.DeviceType == deviceType))
                {
                    servedDevices.Add(c.Name, c);
                }
                TL.LogMessage(0, 0, 0, "RecalculateDeviceNumbers", "Added served devices");
                Dictionary<string, string> x = new Dictionary<string, string>();

                foreach (KeyValuePair<string, ServedDeviceClient> item in servedDevices)
                {
                    TL.LogMessage(0, 0, 0, "RecalculateDeviceNumbers", "Processing item number: " + item.Value.Name + " ");
                    if (item.Value.DeviceType == deviceType)
                    {
                        TL.LogMessage(0, 0, 0, "RecalculateDeviceNumbers", "Setting " + deviceType + " item number: " + deviceNumberIndexes[deviceType].ToString());
                        item.Value.DeviceNumber = deviceNumberIndexes[deviceType];
                        deviceNumberIndexes[deviceType] += 1;
                    }
                }
            }

            TL.LogMessage("RecalculateDeviceNumbers", "End of RecalculateDeviceNumbers");

        }

        private void ServerConfigurationForm_Load(object sender, System.EventArgs e)
        {
            MessageBox.Show("Set configuration is not yet implemented", "Set Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnGetRemoteConfiguration_Click(object sender, EventArgs e)
        {
            TL.LogMessage("GetConfiguration", "Start of btnGetRemoteConfgiuration_Click");
            try
            {
                int clientNumber = 0;
                TL.LogMessage("GetConfiguration", "Connecting to device: " + ipAddressString + ":" + portNumber.ToString());

                string clientHostAddress = string.Format("{0}://{1}:{2}", serviceType, ipAddressString, portNumber.ToString());
                TL.LogMessage("GetConfiguration", "Client host address: " + clientHostAddress);

                RestClient client = new RestClient(clientHostAddress)
                {
                    PreAuthenticate = true
                };
                TL.LogMessage("GetConfiguration", "Creating Authenticator");
                client.Authenticator = new HttpBasicAuthenticator(userName, password);
                TL.LogMessage("GetConfiguration", "Setting timeout");
                RemoteClientDriver.SetClientTimeout(client, 10);

                string managementUri = string.Format("{0}{1}/{2}", SharedConstants.REMOTE_SERVER_MANAGEMENT_URL_BASE, SharedConstants.API_VERSION_V1, SharedConstants.REMOTE_SERVER_MANGEMENT_GET_CONFIGURATION);
                RestRequest request = new RestRequest(managementUri.ToLowerInvariant(), Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                request.AddParameter(SharedConstants.CLIENTID_PARAMETER_NAME, clientNumber.ToString());
                uint transaction = RemoteClientDriver.TransactionNumber();
                request.AddParameter(SharedConstants.CLIENTTRANSACTION_PARAMETER_NAME, transaction.ToString());

                TL.LogMessage("GetConfiguration", "Client Txn ID: " + transaction.ToString() + ", Sending command to remote server");
                IRestResponse response = client.Execute(request);
                string responseContent;
                if (response.Content.Length > 100) responseContent = response.Content.Substring(0, 100);
                else responseContent = response.Content;
                TL.LogMessage("GetConfiguration", string.Format("Response Status: '{0}', Response: {1}", response.StatusDescription, responseContent));

                if ((response.ResponseStatus == ResponseStatus.Completed) & (response.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    ConfigurationResponse configurationResponse = JsonConvert.DeserializeObject<ConfigurationResponse>(response.Content);
                    ConcurrentDictionary<string, ConfiguredDevice> configuration = configurationResponse.Value;
                    TL.LogMessage("GetConfiguration", "Number of device records: " + configuration.Count);

                    using (Profile profile = new Profile())
                    {
                        foreach (string deviceType in profile.RegisteredDeviceTypes)
                        {
                            TL.LogMessage("GetConfiguration", "Adding item: " + deviceType);
                            registeredDeviceTypes.Add(deviceType); // Remember the device types on this system
                        }

                        foreach (ServedDeviceClient item in this.Controls.OfType<ServedDeviceClient>())
                        {
                            TL.LogMessage(0, 0, 0, "GetConfiguration", "Starting Init");
                            item.InitUI(this, TL);
                            TL.LogMessage(0, 0, 0, "GetConfiguration", "Completed Init");
                            item.DeviceType = configuration[item.Name].DeviceType;
                            item.ProgID = configuration[item.Name].ProgID;
                            item.DeviceNumber = configuration[item.Name].DeviceNumber;
                            TL.LogMessage("GetConfiguration", "Completed");
                        }
                        TL.LogMessage("GetConfiguration", "Before RecalculateDevice Numbers");

                        RecalculateDeviceNumbers();
                        TL.LogMessage("GetConfiguration", "After RecalculateDevice Numbers");
                    }

                    // Handle exceptions received from the driver by the remote server
                    if (configurationResponse.DriverException != null)
                    {
                        TL.LogMessageCrLf("GetConfiguration", string.Format("Exception Message: {0}, Exception Number: 0x{1}", configurationResponse.ErrorMessage, configurationResponse.ErrorNumber.ToString("X8")));
                    }
                }
                else
                {
                    if (response.ErrorException != null)
                    {
                        TL.LogMessageCrLf("GetConfiguration", "RestClient exception: " + response.ErrorMessage + "\r\n " + response.ErrorException.ToString());
                        // throw new ASCOM.DriverException(string.Format("Communications exception: {0} - {1}", response.ErrorMessage, response.ResponseStatus), response.ErrorException);
                    }
                    else
                    {
                        TL.LogMessage("GetConfiguration" + " Error", string.Format("RestRequest response status: {0}, HTTP response code: {1}, HTTP response description: {2}", response.ResponseStatus.ToString(), response.StatusCode, response.StatusDescription));
                        // throw new ASCOM.DriverException("ServerConfigurationForm Error - Status: " + response.ResponseStatus + " " + response.StatusDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                TL.LogMessage("GetConfiguration", "Exception: " + ex.ToString());
            }

            TL.LogMessage("GetConfiguration", "End of btnGetRemoteConfgiuration_Click");
        }

        private void BtnReloadConfiguration_Click(object sender, EventArgs e)
        {
            TL.LogMessage("Reload", "Start of BtnReloadConfiguration_Click");
            try
            {
                int clientNumber = 0;
                TL.LogMessage("Reload", "Connecting to device: " + ipAddressString + ":" + portNumber.ToString());

                string clientHostAddress = string.Format("{0}://{1}:{2}", serviceType, ipAddressString, portNumber.ToString());
                TL.LogMessage("Reload", "Client host address: " + clientHostAddress);

                RestClient client = new RestClient(clientHostAddress)
                {
                    PreAuthenticate = true
                };
                TL.LogMessage("Reload", "Creating Authenticator");
                client.Authenticator = new HttpBasicAuthenticator(userName, password);
                TL.LogMessage("Reload", "Setting timeout");
                RemoteClientDriver.SetClientTimeout(client, 10);

                string managementUri = string.Format("{0}{1}/{2}", SharedConstants.REMOTE_SERVER_MANAGEMENT_URL_BASE, SharedConstants.API_VERSION_V1, SharedConstants.REMOTE_SERVER_MANGEMENT_GET_CONFIGURATION);
                RestRequest request = new RestRequest(managementUri.ToLowerInvariant(), Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                request.AddParameter(SharedConstants.CLIENTID_PARAMETER_NAME, clientNumber.ToString());
                uint transaction = RemoteClientDriver.TransactionNumber();
                request.AddParameter(SharedConstants.CLIENTTRANSACTION_PARAMETER_NAME, transaction.ToString());

                TL.LogMessage("Reload", "Client Txn ID: " + transaction.ToString() + ", Sending command to remote server");
                IRestResponse response = client.Execute(request);
                string responseContent;
                if (response.Content.Length > 100) responseContent = response.Content.Substring(0, 100);
                else responseContent = response.Content;
                TL.LogMessage("Reload", string.Format("Response Status: '{0}', Response: {1}", response.StatusDescription, responseContent));

                if ((response.ResponseStatus == ResponseStatus.Completed) & (response.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    ConfigurationResponse configurationResponse = JsonConvert.DeserializeObject<ConfigurationResponse>(response.Content);
                    ConcurrentDictionary<string, ConfiguredDevice> configuration = configurationResponse.Value;
                    TL.LogMessage("Reload", "Number of device records: " + configuration.Count);

                    // Handle exceptions received from the driver by the remote server
                    if (configurationResponse.DriverException != null)
                    {
                        TL.LogMessageCrLf("Reload", string.Format("Exception Message: {0}, Exception Number: 0x{1}", configurationResponse.ErrorMessage, configurationResponse.ErrorNumber.ToString("X8")));
                    }
                }
                else
                {
                    if (response.ErrorException != null)
                    {
                        TL.LogMessageCrLf("Reload", "RestClient exception: " + response.ErrorMessage + "\r\n " + response.ErrorException.ToString());
                        // throw new ASCOM.DriverException(string.Format("Communications exception: {0} - {1}", response.ErrorMessage, response.ResponseStatus), response.ErrorException);
                    }
                    else
                    {
                        TL.LogMessage("Reload" + " Error", string.Format("RestRequest response status: {0}, HTTP response code: {1}, HTTP response description: {2}", response.ResponseStatus.ToString(), response.StatusCode, response.StatusDescription));
                        // throw new ASCOM.DriverException("ServerConfigurationForm Error - Status: " + response.ResponseStatus + " " + response.StatusDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                TL.LogMessage("Reload", "Exception: " + ex.ToString());
            }

            TL.LogMessage("Reload", "End of BtnReloadConfiguration_Click");

        }
    }
}
