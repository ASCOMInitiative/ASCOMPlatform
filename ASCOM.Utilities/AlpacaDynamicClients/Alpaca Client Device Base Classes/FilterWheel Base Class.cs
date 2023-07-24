using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASCOM.Common.Alpaca;
using ASCOM.DeviceInterface;
using RestSharp;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// ASCOM DynamicRemoteClients FilterWheel base class
    /// </summary>
    public class FilterWheelBaseClass : ReferenceCountedObjectBase, IFilterWheelV2
    {
        #region Variables and Constants

        // Constant to set the device type
        private const string DEVICE_TYPE = "FilterWheel";

        // Instance specific variables
        private TraceLoggerPlus TL; // Private variable to hold the trace logger object
        private string DriverNumber; // This driver's number in the series 1, 2, 3...
        private string DriverDisplayName; // Driver description that displays in the ASCOM Chooser.
        private string DriverProgId; // Drivers ProgID
        private SetupDialogForm setupForm; // Private variable to hold an instance of the Driver's setup form when invoked by the user
        private RestClient client; // Client to send and receive REST style messages to / from the remote device
        private uint clientNumber; // Unique number for this driver within the locaL server, i.e. across all drivers that the local server is serving
        private bool clientIsConnected;  // Connection state of this driver
        private string URIBase; // URI base unique to this driver

        // Connect / Disconnect emulation variables
        bool connecting;
        Exception connectException;

        // Variables to hold values that can be configured by the user through the setup form
        private bool traceState = true;
        private bool debugTraceState = true;
        private string ipAddressString;
        private decimal portNumber;
        private decimal remoteDeviceNumber;
        private string serviceType;
        private int establishConnectionTimeout;
        private int standardDeviceResponseTimeout;
        private int longDeviceResponseTimeout;
        private string userName;
        private string password;
        private bool manageConnectLocally;
        private ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType;
        private ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression;
        private string uniqueId;
        private bool enableRediscovery;
        private bool ipV4Enabled;
        private bool ipV6Enabled;
        private int discoveryPort;
        private bool trustUserGeneratedSslCertificates;

        #endregion

        #region Initialiser

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterWheelBaseClass"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public FilterWheelBaseClass(string RequiredDriverNumber, string RequiredDriverDisplayName, string RequiredProgId)
        {
            try
            {
                // Initialise variables unique to this particular driver with values passed from the calling class
                DriverNumber = RequiredDriverNumber;
                DriverDisplayName = RequiredDriverDisplayName; // Driver description that displays in the ASCOM Chooser.
                DriverProgId = RequiredProgId;

                if (TL == null) TL = new TraceLoggerPlus("", string.Format(SharedConstants.TRACELOGGER_NAME_FORMAT_STRING, DriverNumber, DEVICE_TYPE));
                DynamicClientDriver.ReadProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId,
                    ref traceState, ref debugTraceState, ref ipAddressString, ref portNumber, ref remoteDeviceNumber, ref serviceType, ref establishConnectionTimeout, ref standardDeviceResponseTimeout,
                    ref longDeviceResponseTimeout, ref userName, ref password, ref manageConnectLocally, ref imageArrayTransferType, ref imageArrayCompression, ref uniqueId, ref enableRediscovery,
                    ref ipV4Enabled, ref ipV6Enabled, ref discoveryPort, ref trustUserGeneratedSslCertificates);

                Version version = Assembly.GetEntryAssembly().GetName().Version;
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Starting initialisation, Version: " + version.ToString());

                clientNumber = DynamicClientDriver.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This instance's unique client number: " + clientNumber);

                DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE,
                                                          standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery, ipV4Enabled, ipV6Enabled, discoveryPort, trustUserGeneratedSslCertificates);

                URIBase = string.Format("{0}{1}/{2}/{3}/", AlpacaConstants.API_URL_BASE, AlpacaConstants.API_VERSION_V1, DEVICE_TYPE, remoteDeviceNumber.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This devices's base URI: " + URIBase);
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Establish communications timeout: " + establishConnectionTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Standard device response timeout: " + standardDeviceResponseTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Long device response timeout: " + longDeviceResponseTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "User name: " + userName);
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Password is Null or Empty: {0}, Password is Null or White Space: {1}", string.IsNullOrEmpty(password), string.IsNullOrWhiteSpace(password)));
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Password length: {0}", password.Length));

                TL.LogMessage(clientNumber, DEVICE_TYPE, "Completed initialisation");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(clientNumber, DEVICE_TYPE, ex.ToString());
            }
        }

        #endregion

        #region Common properties and methods.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="actionParameters"></param>
        /// <returns></returns>
        public string Action(string actionName, string actionParameters)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.Action(clientNumber, client, URIBase, TL, actionName, actionParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="raw"></param>
        public void CommandBlind(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CommandBlind(clientNumber, client, URIBase, TL, command, raw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public bool CommandBool(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.CommandBool(clientNumber, client, URIBase, TL, command, raw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public string CommandString(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.CommandString(clientNumber, client, URIBase, TL, command, raw);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected
        {
            get
            {
                return clientIsConnected;
            }
            set
            {
                clientIsConnected = value;
                if (manageConnectLocally)
                {
                    TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("The Connected property is being managed locally so the new value '{0}' will not be sent to the remote device", value));
                }
                else // Send the command to the remote device
                {
                    DynamicClientDriver.SetClientTimeout(client, establishConnectionTimeout);
                    if (value) DynamicClientDriver.Connect(clientNumber, client, URIBase, TL);
                    else DynamicClientDriver.Disconnect(clientNumber, client, URIBase, TL);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                string response = DynamicClientDriver.Description(clientNumber, client, URIBase, TL);
                TL.LogMessage(clientNumber, "Description", response);
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DriverInfo
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                string response = $"ASCOM Dynamic Driver v{version} - REMOTE DEVICE: {DynamicClientDriver.DriverInfo(clientNumber, client, URIBase, TL)}";
                TL.LogMessage(clientNumber, "DriverInfo", response);
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DriverVersion
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.DriverVersion(clientNumber, client, URIBase, TL);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.InterfaceVersion(clientNumber, client, URIBase, TL);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                string response = DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "Name", MemberTypes.Property);
                TL.LogMessage(clientNumber, "Name", response);
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetupDialog()
        {
            TL.LogMessage(clientNumber, "SetupDialog", "Connected: " + clientIsConnected.ToString());
            if (clientIsConnected)
            {
                MessageBox.Show("Simulator is connected, setup parameters cannot be changed, please press OK");
            }
            else
            {
                TL.LogMessage(clientNumber, "SetupDialog", "Creating setup form");
                using (setupForm = new SetupDialogForm(TL))
                {
                    // Pass the setup dialogue data into the form
                    setupForm.DriverDisplayName = DriverDisplayName;
                    setupForm.TraceState = traceState;
                    setupForm.DebugTraceState = debugTraceState;
                    setupForm.ServiceType = serviceType;
                    setupForm.IPAddressString = ipAddressString;
                    setupForm.PortNumber = portNumber;
                    setupForm.RemoteDeviceNumber = remoteDeviceNumber;
                    setupForm.EstablishConnectionTimeout = establishConnectionTimeout;
                    setupForm.StandardTimeout = standardDeviceResponseTimeout;
                    setupForm.LongTimeout = longDeviceResponseTimeout;
                    setupForm.UserName = userName;
                    setupForm.Password = password;
                    setupForm.ManageConnectLocally = manageConnectLocally;
                    setupForm.ImageArrayTransferType = imageArrayTransferType;
                    setupForm.DeviceType = DEVICE_TYPE;
                    setupForm.EnableRediscovery = enableRediscovery;
                    setupForm.IpV4Enabled = ipV4Enabled;
                    setupForm.IpV6Enabled = ipV6Enabled;
                    setupForm.DiscoveryPort = discoveryPort;
                    setupForm.TrustUserGeneratedSslCertificates = trustUserGeneratedSslCertificates;

                    TL.LogMessage(clientNumber, "SetupDialog", "Showing Dialogue");
                    var result = setupForm.ShowDialog();
                    TL.LogMessage(clientNumber, "SetupDialog", "Dialogue closed");
                    if (result == DialogResult.OK)
                    {
                        TL.LogMessage(clientNumber, "SetupDialog", "Dialogue closed with OK status");

                        // Retrieve revised setup data from the form
                        traceState = setupForm.TraceState;
                        debugTraceState = setupForm.DebugTraceState;
                        serviceType = setupForm.ServiceType;
                        ipAddressString = setupForm.IPAddressString;
                        portNumber = setupForm.PortNumber;
                        remoteDeviceNumber = setupForm.RemoteDeviceNumber;
                        establishConnectionTimeout = (int)setupForm.EstablishConnectionTimeout;
                        standardDeviceResponseTimeout = (int)setupForm.StandardTimeout;
                        longDeviceResponseTimeout = (int)setupForm.LongTimeout;
                        userName = setupForm.UserName;
                        password = setupForm.Password;
                        manageConnectLocally = setupForm.ManageConnectLocally;
                        imageArrayTransferType = setupForm.ImageArrayTransferType;
                        enableRediscovery = setupForm.EnableRediscovery;
                        ipV4Enabled = setupForm.IpV4Enabled;
                        ipV6Enabled = setupForm.IpV6Enabled;
                        discoveryPort = setupForm.DiscoveryPort;
                        trustUserGeneratedSslCertificates= setupForm.TrustUserGeneratedSslCertificates;

                        // Write the changed values to the Profile
                        TL.LogMessage(clientNumber, "SetupDialog", "Writing new values to profile");
                        DynamicClientDriver.WriteProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId, traceState, debugTraceState, ipAddressString, portNumber, remoteDeviceNumber, serviceType,
                            establishConnectionTimeout, standardDeviceResponseTimeout, longDeviceResponseTimeout, userName, password, manageConnectLocally, imageArrayTransferType, imageArrayCompression, uniqueId, enableRediscovery, 
                            ipV4Enabled, ipV6Enabled, discoveryPort, trustUserGeneratedSslCertificates);

                        // Establish new host and device parameters
                        TL.LogMessage(clientNumber, "SetupDialog", "Establishing new host and device parameters");
                        DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE,
                                                                  standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery, ipV4Enabled, ipV6Enabled, discoveryPort, trustUserGeneratedSslCertificates);
                    }
                    else TL.LogMessage(clientNumber, "SetupDialog", "Dialogue closed with Cancel status");
                }
                if (!(setupForm == null))
                {
                    setupForm.Dispose();
                    setupForm = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList SupportedActions
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.SupportedActions(clientNumber, client, URIBase, TL);
            }
        }
        #endregion

        #region IFilterWheel implementation

        /// <summary>
        /// 
        /// </summary>
        /// <asdasd></asdasd>
        /// <qweqwe></qweqwe>
        public int[] FocusOffsets
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int[]>(clientNumber, client, URIBase, TL, "FocusOffsets", MemberTypes.Property);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] Names
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<string[]>(clientNumber, client, URIBase, TL, "Names", MemberTypes.Property);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public short Position
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "Position", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "Position", value, MemberTypes.Property);
            }
        }

        #endregion

        #region IFilterWheelV3 implementation

        public void Connect()
        {
            // Call the device's Connect method if this is a Platform 7 or later device, otherwise simulate the connect call
            if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE, InterfaceVersion)) // We are presenting a Platform 7 or later device
            {
                TL.LogMessage("Connect", "Issuing Connect command");
                DynamicClientDriver.SetClientTimeout(client, establishConnectionTimeout);
                DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Connect", MemberTypes.Method);
            }

            // Platform 6 or earlier so emulate the capability
            TL.LogMessage("Connect", "Emulating Connect command for Platform 6 driver");

            // Set Connecting to true and clear any previous exception
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to True
            Task connectingTask = Task.Factory.StartNew(() =>
            {
                // Ensure that no exceptions can escape
                try
                {
                    // Set Connected True
                    TL.LogMessage("Connect", "About to set Connected True");
                    Connected = true;
                    TL.LogMessage("Connect", "Connected Set True OK");
                }
                catch (Exception ex)
                {
                    // Something went wrong so log the issue and save the exception
                    TL.LogMessage("Connect", $"Connected threw an exception: {ex.Message}");
                    connectException = ex;
                }
                // Ensure that Connecting is always set False at the end of the task
                finally
                {
                    TL.LogMessage("Connect", "Setting Connecting to False");
                    connecting = false;
                }
            });
        }

        public void Disconnect()
        {
            // Call the device's Disconnect method if this is a Platform 7 or later device, otherwise simulate the connect call
            if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE, InterfaceVersion)) // We are presenting a Platform 7 or later device
            {
                TL.LogMessage("Disconnect", "Issuing Disconnect command");
                DynamicClientDriver.SetClientTimeout(client, establishConnectionTimeout);
                DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Disconnect", MemberTypes.Method);
            }

            // Platform 6 or earlier so emulate the capability
            TL.LogMessage("Disconnect", "Emulating Disconnect command for Platform 6 driver");

            // Set Connecting to true and clear any previous exception
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to True
            Task connectingTask = Task.Factory.StartNew(() =>
            {
                // Ensure that no exceptions can escape
                try
                {
                    // Set Connected True
                    TL.LogMessage("Disconnect", "About to set Connected False");
                    Connected = false;
                    TL.LogMessage("Disconnect", "Connected Set False OK");
                }
                catch (Exception ex)
                {
                    // Something went wrong so log the issue and save the exception
                    TL.LogMessage("Disconnect", $"Connected threw an exception: {ex.Message}");
                    connectException = ex;
                }
                // Ensure that Connecting is always set False at the end of the task
                finally
                {
                    TL.LogMessage("Disconnect", "Setting Connecting to False");
                    connecting = false;
                }
            });
        }

        public bool Connecting
        {
            get
            {
                // Call the device's Connecting method if this is a Platform 7 or later device, otherwise return False
                if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE, InterfaceVersion)) // We are presenting a Platform 7 or later device
                {
                    TL.LogMessage("Connecting Get", "Issuing Connecting command");
                    DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                    return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "Connecting", MemberTypes.Property);
                }

                // Platform 6 or earlier device
                // If Connected or disconnected threw an exception, throw this to the client
                if (!(connectException is null))
                {
                    TL.LogMessage("Connecting Get", $"Throwing exception from Connected to the client: {connectException.Message}\r\n{connectException}");
                    throw connectException;
                }

                // Platform 6 or earlier device so always return false.
                return false;
            }
        }

        public ArrayList DeviceState
        {
            get
            {
                // Call the device's DeviceState method if this is a Platform 7 or later device, otherwise simulate the DeviceState method
                if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE, InterfaceVersion)) // We are presenting a Platform 7 or later device
                {
                    try
                    {
                        DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                        return DynamicClientDriver.DeviceState(clientNumber, client, URIBase, TL);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessage("DeviceState Get", "Received exception: " + ex.Message);
                        throw;
                    }
                }
                else // Platform 6 or earlier device so return an empty list.
                {
                    return new ArrayList();
                }
            }
        }

        #endregion

    }
}