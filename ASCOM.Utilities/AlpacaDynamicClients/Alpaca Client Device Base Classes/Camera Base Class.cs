using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using ASCOM.DeviceInterface;
using RestSharp;
using static ASCOM.DynamicRemoteClients.SharedConstants;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// ASCOM DynamicRemoteClients Camera base class.
    /// </summary>
    public class CameraBaseClass : ReferenceCountedObjectBase, ICameraV3
    {
        #region Variables and Constants

        // Constant to set the device type
        private const string DEVICE_TYPE = "Camera";

        // GetBase64Image constants
        private const string BASE64RESPONSE_COMMAND_NAME = "GetBase64Image";
        private const int BASE64RESPONSE_VERSION_NUMBER = 1;
        private const int BASE64RESPONSE_VERSION_POSITION = 0;
        private const int BASE64RESPONSE_OUTPUTTYPE_POSITION = 4;
        private const int BASE64RESPONSE_TRANSMISSIONTYPE_POSITION = 8;
        private const int BASE64RESPONSE_RANK_POSITION = 12;
        private const int BASE64RESPONSE_DIMENSION0_POSITION = 16;
        private const int BASE64RESPONSE_DIMENSION1_POSITION = 20;
        private const int BASE64RESPONSE_DIMENSION2_POSITION = 24;
        private const int BASE64RESPONSE_DATA_POSITION = 48;

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
        private bool? canGetBase64Image = null; // Indicator of whether the remote device supports GetBase64Image functionality
        private bool? canGetImageBytes = null; // Indicator of whether the remote device supports GetBase64Image functionality

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
        private ImageArrayTransferType imageArrayTransferType;
        private ImageArrayCompression imageArrayCompression;
        private string uniqueId;
        private bool enableRediscovery;
        private bool ipV4Enabled;
        private bool ipV6Enabled;
        private int discoveryPort;

        #endregion

        #region Initialiser

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraBaseClass"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public CameraBaseClass(string RequiredDriverNumber, string RequiredDriverDisplayName, string RequiredProgId)
        {
            try
            {
                // Initialise variables unique to this particular driver with values passed from the calling class
                DriverNumber = RequiredDriverNumber;
                DriverDisplayName = RequiredDriverDisplayName; // Driver description that displays in the ASCOM Chooser.
                DriverProgId = RequiredProgId;

                if (TL == null) TL = new TraceLoggerPlus("", string.Format(TRACELOGGER_NAME_FORMAT_STRING, DriverNumber, DEVICE_TYPE));
                DynamicClientDriver.ReadProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId,
                    ref traceState, ref debugTraceState, ref ipAddressString, ref portNumber, ref remoteDeviceNumber, ref serviceType, ref establishConnectionTimeout, ref standardDeviceResponseTimeout,
                    ref longDeviceResponseTimeout, ref userName, ref password, ref manageConnectLocally, ref imageArrayTransferType, ref imageArrayCompression, ref uniqueId, ref enableRediscovery, ref ipV4Enabled, ref ipV6Enabled, ref discoveryPort);
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Trace state: {0}, Debug Trace State: {1}, TraceLogger Debug State: {2}", traceState, debugTraceState, TL.DebugTraceState));
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Starting initialisation, Version: " + version.ToString());

                clientNumber = DynamicClientDriver.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This instance's unique client number: " + clientNumber);

                DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE,
                                                          standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery, ipV6Enabled, ipV6Enabled, discoveryPort);

                URIBase = string.Format("{0}{1}/{2}/{3}/", API_URL_BASE, API_VERSION_V1, DEVICE_TYPE, remoteDeviceNumber.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This devices's base URI: " + URIBase);
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Establish communications timeout: " + establishConnectionTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Standard device response timeout: " + standardDeviceResponseTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Long device response timeout: " + longDeviceResponseTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "User name: " + userName);
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Password is Null or Empty: {0}, Password is Null or White Space: {1}", string.IsNullOrEmpty(password), string.IsNullOrWhiteSpace(password)));
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Password length: {0}", password.Length));
                TL.LogMessage(clientNumber, DEVICE_TYPE, $"Image array transfer type: {imageArrayTransferType}");

                TL.LogMessage(clientNumber, DEVICE_TYPE, "Completed initialisation");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(clientNumber, DEVICE_TYPE, ex.ToString());
            }
        }

        #endregion

        #region Common properties and methods.

        public string Action(string actionName, string actionParameters)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.Action(clientNumber, client, URIBase, TL, actionName, actionParameters);
        }

        public void CommandBlind(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CommandBlind(clientNumber, client, URIBase, TL, command, raw);
        }

        public bool CommandBool(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.CommandBool(clientNumber, client, URIBase, TL, command, raw);
        }

        public string CommandString(string command, bool raw = false)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            return DynamicClientDriver.CommandString(clientNumber, client, URIBase, TL, command, raw);
        }

        public void Dispose()
        {
        }

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

        public string DriverVersion
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.DriverVersion(clientNumber, client, URIBase, TL);
            }
        }

        public short InterfaceVersion
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.InterfaceVersion(clientNumber, client, URIBase, TL);
            }
        }

        public string Name
        {
            get
            {
                string response = DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "Name", MemberTypes.Property);
                TL.LogMessage(clientNumber, "Name", response);
                return response;
            }
        }

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
                    setupForm.ImageArrayCompression = imageArrayCompression;
                    setupForm.DeviceType = DEVICE_TYPE;
                    setupForm.EnableRediscovery = enableRediscovery;
                    setupForm.IpV4Enabled = ipV4Enabled;
                    setupForm.IpV6Enabled = ipV6Enabled;
                    setupForm.DiscoveryPort = discoveryPort;

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
                        imageArrayCompression = setupForm.ImageArrayCompression;
                        enableRediscovery = setupForm.EnableRediscovery;
                        ipV4Enabled = setupForm.IpV4Enabled;
                        ipV6Enabled = setupForm.IpV6Enabled;
                        discoveryPort = setupForm.DiscoveryPort;

                        // Write the changed values to the Profile
                        TL.LogMessage(clientNumber, "SetupDialog", "Writing new values to profile");
                        DynamicClientDriver.WriteProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId, traceState, debugTraceState, ipAddressString, portNumber, remoteDeviceNumber, serviceType,
                            establishConnectionTimeout, standardDeviceResponseTimeout, longDeviceResponseTimeout, userName, password, manageConnectLocally, imageArrayTransferType, imageArrayCompression, uniqueId, enableRediscovery, ipV4Enabled, ipV6Enabled, discoveryPort);

                        // Establish new host and device parameters
                        TL.LogMessage(clientNumber, "SetupDialog", "Establishing new host and device parameters");
                        DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE,
                                                                  standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery, ipV4Enabled, ipV6Enabled, discoveryPort);
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

        public ArrayList SupportedActions
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.SupportedActions(clientNumber, client, URIBase, TL);
            }
        }

        #endregion

        #region ICameraV2 Implementation

        public void AbortExposure()
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "AbortExposure", MemberTypes.Method);
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { DIRECTION_PARAMETER_NAME, ((int)Direction).ToString(CultureInfo.InvariantCulture) },
                { DURATION_PARAMETER_NAME, Duration.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "PulseGuide", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void StartExposure(double Duration, bool Light)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { DURATION_PARAMETER_NAME, Duration.ToString(CultureInfo.InvariantCulture) },
                { LIGHT_PARAMETER_NAME, Light.ToString() }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "StartExposure", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void StopExposure()
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "StopExposure", MemberTypes.Method);
        }

        public short BinX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BinX", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "BinX", value, MemberTypes.Property);
            }
        }

        public short BinY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BinY", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "BinY", value, MemberTypes.Property);
            }
        }

        public CameraStates CameraState
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<CameraStates>(clientNumber, client, URIBase, TL, "CameraState", MemberTypes.Property);
            }
        }

        public int CameraXSize
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "CameraXSize", MemberTypes.Property);
            }
        }

        public int CameraYSize
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "CameraYSize", MemberTypes.Property);
            }
        }

        public bool CanAbortExposure
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanAbortExposure", MemberTypes.Property);
            }
        }

        public bool CanAsymmetricBin
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanAsymmetricBin", MemberTypes.Property);
            }
        }

        public bool CanGetCoolerPower
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanGetCoolerPower", MemberTypes.Property);
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanPulseGuide", MemberTypes.Property);
            }
        }

        public bool CanSetCCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetCCDTemperature", MemberTypes.Property);
            }
        }

        public bool CanStopExposure
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanStopExposure", MemberTypes.Property);
            }
        }

        public double CCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "CCDTemperature", MemberTypes.Property);
            }
        }

        public bool CoolerOn
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CoolerOn", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "CoolerOn", value, MemberTypes.Property);
            }
        }

        public double CoolerPower
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "CoolerPower", MemberTypes.Property);
            }
        }

        public double ElectronsPerADU
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ElectronsPerADU", MemberTypes.Property);
            }
        }

        public double FullWellCapacity
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "FullWellCapacity", MemberTypes.Property);
            }
        }

        public bool HasShutter
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "HasShutter", MemberTypes.Property);
            }
        }

        public double HeatSinkTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "HeatSinkTemperature", MemberTypes.Property);
            }
        }

        public object ImageArray
        {
            get
            {
                try
                {
                    // Special handling for GetBase64Image transfers
                    TL.LogMessage(clientNumber, DEVICE_TYPE, $"CameraBaseClass.ImageArray called - canGetBase64Image.HasValue: {canGetBase64Image.HasValue}, canGetImageBytes: {canGetImageBytes.HasValue} , imageArrayTransferType: {imageArrayTransferType}");

                    // Determine whether we need to find out whether Getbase64Image functionality is provided by this driver
                    if ((!canGetBase64Image.HasValue) & ((imageArrayTransferType == ImageArrayTransferType.GetBase64Image) | (imageArrayTransferType == ImageArrayTransferType.BestAvailable)))
                    {
                        // Try to get an answer from the device, if anything goes wrong assume that the feature is not available
                        try
                        {
                            // Initialise the supported flag to false
                            canGetBase64Image = false;
                            ArrayList supportedActions = this.SupportedActions;
                            foreach (string action in supportedActions)
                            {
                                // Set the supported flag true if the device advertises that it supports GetBase64Image
                                if (action.ToLowerInvariant() == GETBASE64IMAGE_ACTION_NAME.ToLowerInvariant()) canGetBase64Image = true;
                                if (action.ToLowerInvariant() == "getimagebytes") canGetImageBytes = true;
                                TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"CameraBaseClass.ImageArray Found action: {action}, canGetBase64Image: {canGetBase64Image.Value}");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Just log any errors but otherwise ignore them
                            TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Received an exception when trying to get the device's SupportedActions: {ex.Message}");
                        }
                    }
                    if (!canGetBase64Image.HasValue) canGetBase64Image = false; // Set false if we have no value at this point
                    if (!canGetImageBytes.HasValue) canGetImageBytes = false; // Set false if we have no value at this point

                    // Throw an exception if GetBase64Image mode is explicitly requested but the device does not support this mode
                    if (imageArrayTransferType == ImageArrayTransferType.GetBase64Image & !canGetBase64Image.Value) throw new InvalidOperationException("GetBase64Image transfer mode has been requested by the device does not support this mode.");

                    // Use a fast transfer mode if possible
                    if (canGetImageBytes.Value & imageArrayTransferType == ImageArrayTransferType.GetImageBytes)
                    {
                        Stopwatch swOverall = new Stopwatch();
                        swOverall.Start();

                        RestRequest request = new RestRequest((URIBase + "ImageArrayBytes").ToLowerInvariant(), Method.GET);
                        request.RequestFormat = DataFormat.None;
                        client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.None); // Prevent any decompression

                        // Add the transaction number and client ID parameters
                        uint transaction = DynamicClientDriver.TransactionNumber();
                        request.AddParameter(SharedConstants.CLIENTTRANSACTION_PARAMETER_NAME, transaction.ToString());
                        request.AddParameter(SharedConstants.CLIENTID_PARAMETER_NAME, clientNumber.ToString());

                        TL.LogMessage(clientNumber, "ImageArrayBytes", "Client Txn ID: " + transaction.ToString() + ", Sending command to remote device");
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        byte[] imageBytes = client.DownloadData(request, true);
                        sw.Stop();
                        TL.LogMessage(clientNumber, "ImageArrayBytes", $"Downloaded {imageBytes.Length} bytes in {sw.ElapsedMilliseconds}ms.");
                        return ConvertByteArray(swOverall, imageBytes);
                    }
                    else if (canGetBase64Image.Value)
                    {
                        Stopwatch sw = new Stopwatch();
                        Stopwatch swOverall = new Stopwatch();
                        swOverall.Start();
                        sw.Start();

                        // Call the GetBase64Image Action method to retrieve the image in base64 encoded form
                        string base64String = this.Action(BASE64RESPONSE_COMMAND_NAME, "");
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Received {base64String.Length} bytes in {sw.ElapsedMilliseconds}ms.");

                        sw.Restart();
                        byte[] base64ArrayByteArray = Convert.FromBase64String(base64String);
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Converted string to byte array in {sw.ElapsedMilliseconds}ms.");

                        return ConvertByteArray(swOverall, base64ArrayByteArray);
                    }
                    else
                    {
                        DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
                        return DynamicClientDriver.GetValue<Array>(clientNumber, client, URIBase, TL, "ImageArray", imageArrayTransferType, imageArrayCompression, MemberTypes.Property);
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf(clientNumber, "CameraBaseClass.ImageArray", $"CameraBaseClass.ImageArray exception: {ex}");

                    throw;
                }
            }
        }

        private object ConvertByteArray(Stopwatch swOverall, byte[] base64ArrayByteArray)
        {
            Stopwatch sw = new Stopwatch();

            // Set the array type, rank and dimensions
            int version = BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_VERSION_POSITION);
            ImageArrayElementTypes outputType = (ImageArrayElementTypes)BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_OUTPUTTYPE_POSITION);
            ImageArrayElementTypes transmissionType = (ImageArrayElementTypes)BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_TRANSMISSIONTYPE_POSITION);
            int rank = BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_RANK_POSITION);
            int dimension0 = BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_DIMENSION0_POSITION);
            int dimension1 = BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_DIMENSION1_POSITION);
            int dimension2 = BitConverter.ToInt32(base64ArrayByteArray, BASE64RESPONSE_DIMENSION2_POSITION);
            TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Version: {version}, Output Type: {outputType}, Transmission Type: {transmissionType}, Rank: {rank}, Dimension 0: {dimension0}, Dimension 1: {dimension1}, Dimension 2: {dimension2}");

            // Validate returned metadata values
            if (version != GETBASE64IMAGE_SUPPORTED_VERSION) throw new InvalidValueException($"GetBase64Image - The device returned an unsupported version: {version}, this Alpaca client supports version: {GETBASE64IMAGE_SUPPORTED_VERSION}");

            sw.Restart();
            // Convert the returned byte[] into the form that the client is expecting
            if ((outputType == ImageArrayElementTypes.Int32) & (transmissionType == ImageArrayElementTypes.Int16)) // Handle the special case where Int32 has been converted to Int16 for transmission
            {
                switch (rank)
                {
                    case 2: // Rank 2
                        short[,] short2dArray = new short[dimension0, dimension1];
                        Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, short2dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");

                        int[,] int2dArray = new int[dimension0, dimension1];
                        Parallel.For(0, short2dArray.GetLength(0) - 1, (i) =>
                        {
                            Parallel.For(0, short2dArray.GetLength(1) - 1, (j) =>
                            {
                                int2dArray[i, j] = short2dArray[i, j];
                            });
                        });
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"CONVERTED 2D INT16 ARRAY TO INT32 ARRAY - GetBase64Image time: {swOverall.ElapsedMilliseconds}ms, Input length: {short2dArray.Length}, Output length: {int2dArray.Length}");
                        return int2dArray;

                    case 3: // Rank 3
                        short[,,] short3dArray = new short[dimension0, dimension1, dimension2];
                        Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, short3dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");

                        int[,,] int3dArray = new int[dimension0, dimension1, dimension2];
                        Parallel.For(0, short3dArray.GetLength(2) - 1, (k) =>
                        {
                            Parallel.For(0, short3dArray.GetLength(1) - 1, (j) =>
                            {
                                Parallel.For(0, short3dArray.GetLength(0) - 1, (i) =>
                                {
                                    int3dArray[i, j, k] = short3dArray[i, j, k];
                                });
                            });
                        });
                        TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"CONVERTED 3D INT16 ARRAY TO INT32 ARRAY - GetBase64Image time: {swOverall.ElapsedMilliseconds}ms, Input length: {short3dArray.Length}, Output length: {int3dArray.Length}");
                        return int3dArray;

                    default:
                        throw new InvalidValueException($"CameraBaseClass.ImageArray - Returned array cannot be handled because it does not have a rank of 2 or 3. Returned array rank:{rank}.");
                }
            }
            else // Handle all other cases where the expected array type and the transmitted array type are the same
            {
                if (outputType == transmissionType) // Required and transmitted array element types are the same
                {
                    switch (outputType)
                    {
                        case ImageArrayElementTypes.Byte:
                            switch (rank)
                            {
                                case 2: // Rank 2
                                    byte[,] byte2dArray = new byte[dimension0, dimension1];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, byte2dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed byte[,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return byte2dArray;

                                case 3: // Rank 3
                                    byte[,,] byte3dArray = new byte[dimension0, dimension1, dimension2];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, byte3dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed byte[,,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return byte3dArray;

                                default:
                                    throw new InvalidValueException($"ImageArray - Returned byte array cannot be handled because it does not have a rank of 2 or 3. Returned array rank:{rank}.");
                            }

                        case ImageArrayElementTypes.Int16:
                            switch (rank)
                            {
                                case 2: // Rank 2
                                    short[,] short2dArray = new short[dimension0, dimension1];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, short2dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int16[,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return short2dArray;

                                case 3: // Rank 3
                                    short[,,] short3dArray = new short[dimension0, dimension1, dimension2];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, short3dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int16[,,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return short3dArray;

                                default:
                                    throw new InvalidValueException($"ImageArray - Returned Int16 array cannot be handled because it does not have a rank of 2 or 3. Returned array rank:{rank}.");
                            }

                        case ImageArrayElementTypes.Int32:
                            switch (rank)
                            {
                                case 2: // Rank 2
                                    int[,] int2dArray = new int[dimension0, dimension1];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, int2dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int32[,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return int2dArray;

                                case 3: // Rank 3
                                    int[,,] int3dArray = new int[dimension0, dimension1, dimension2];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, int3dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int32[,,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return int3dArray;

                                default:
                                    throw new InvalidValueException($"ImageArray - Returned Int32 array cannot be handled because it does not have a rank of 2 or 3. Returned array rank:{rank}.");
                            }

                        case ImageArrayElementTypes.Int64:
                            switch (rank)
                            {
                                case 2: // Rank 2
                                    Int64[,] int642dArray = new Int64[dimension0, dimension1];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, int642dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int64[,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return int642dArray;

                                case 3: // Rank 3
                                    Int64[,,] int643dArray = new Int64[dimension0, dimension1, dimension2];
                                    Buffer.BlockCopy(base64ArrayByteArray, BASE64RESPONSE_DATA_POSITION, int643dArray, 0, base64ArrayByteArray.Length - BASE64RESPONSE_DATA_POSITION);
                                    TL.LogMessage(clientNumber, "CameraBaseClass.ImageArray", $"Completed Int64[,,] block copy of {base64ArrayByteArray.Length} bytes in {sw.ElapsedMilliseconds}ms");
                                    return int643dArray;

                                default:
                                    throw new InvalidValueException($"ImageArray - Returned Int64 array cannot be handled because it does not have a rank of 2 or 3. Returned array rank:{rank}.");
                            }

                        default:
                            throw new InvalidValueException($"The device has returned an unsupported image array element type: {outputType}.");
                    }
                }
                else // An unsupported combination of array element types has been returned
                {
                    throw new InvalidValueException($"The device has returned an unsupported combination of Output type: {outputType} and Transmission type: {transmissionType}.");
                }
            }
        }

        public object ImageArrayVariant
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
                return DynamicClientDriver.ImageArrayVariant(clientNumber, client, URIBase, TL, imageArrayTransferType, imageArrayCompression);
            }
        }

        public bool ImageReady
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "ImageReady", MemberTypes.Property);
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "IsPulseGuiding", MemberTypes.Property);
            }
        }

        public double LastExposureDuration
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "LastExposureDuration", MemberTypes.Property);
            }
        }

        public string LastExposureStartTime
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "LastExposureStartTime", MemberTypes.Property);
            }
        }

        public int MaxADU
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "MaxADU", MemberTypes.Property);
            }
        }

        public short MaxBinX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "MaxBinX", MemberTypes.Property);
            }
        }

        public short MaxBinY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "MaxBinY", MemberTypes.Property);
            }
        }

        public int NumX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "NumX", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "NumX", value, MemberTypes.Property);
            }
        }

        public int NumY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "NumY", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "NumY", value, MemberTypes.Property);
            }
        }

        public double PixelSizeX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "PixelSizeX", MemberTypes.Property);
            }
        }

        public double PixelSizeY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "PixelSizeY", MemberTypes.Property);
            }
        }

        public double SetCCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SetCCDTemperature", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SetCCDTemperature", value, MemberTypes.Property);
            }
        }

        public int StartX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "StartX", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "StartX", value, MemberTypes.Property);
            }
        }

        public int StartY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "StartY", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "StartY", value, MemberTypes.Property);
            }
        }

        public short BayerOffsetX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BayerOffsetX", MemberTypes.Property);
            }
        }

        public short BayerOffsetY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BayerOffsetY", MemberTypes.Property);
            }
        }

        public bool CanFastReadout
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanFastReadout", MemberTypes.Property);
            }
        }

        public double ExposureMax
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureMax", MemberTypes.Property);
            }
        }

        public double ExposureMin
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureMin", MemberTypes.Property);
            }
        }

        public double ExposureResolution
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureResolution", MemberTypes.Property);
            }
        }

        public bool FastReadout
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "FastReadout", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "FastReadout", value, MemberTypes.Property);
            }
        }

        public short Gain
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "Gain", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "Gain", value, MemberTypes.Property);
            }
        }

        public short GainMax
        {
            get
            {
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "GainMax", MemberTypes.Property);
            }
        }

        public short GainMin
        {
            get
            {
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "GainMin", MemberTypes.Property);
            }
        }

        public ArrayList Gains
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                List<string> gains = DynamicClientDriver.GetValue<List<string>>(clientNumber, client, URIBase, TL, "Gains", MemberTypes.Property);
                TL.LogMessage(clientNumber, "Gains", string.Format("Returning {0} gains", gains.Count));

                ArrayList returnValues = new ArrayList();
                foreach (string gain in gains)
                {
                    returnValues.Add(gain);
                    TL.LogMessage(clientNumber, "Gains", string.Format("Returning gain: {0}", gain));
                }

                return returnValues;
            }
        }

        public short PercentCompleted
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "PercentCompleted", MemberTypes.Property);
            }
        }

        public short ReadoutMode
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "ReadoutMode", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "ReadoutMode", value, MemberTypes.Property);
            }
        }

        public ArrayList ReadoutModes
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                List<string> modes = DynamicClientDriver.GetValue<List<string>>(clientNumber, client, URIBase, TL, "ReadoutModes", MemberTypes.Property);
                TL.LogMessage(clientNumber, "ReadoutModes", string.Format("Returning {0} modes", modes.Count));

                ArrayList returnValues = new ArrayList();
                foreach (string gain in modes)
                {
                    returnValues.Add(gain);
                    TL.LogMessage(clientNumber, "ReadoutModes", string.Format("Returning mode: {0}", gain));
                }

                return returnValues;
            }
        }

        public string SensorName
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "SensorName", MemberTypes.Property);
            }
        }

        public SensorType SensorType
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<SensorType>(clientNumber, client, URIBase, TL, "SensorType", MemberTypes.Property);
            }
        }

        #endregion

        #region ICameraV3 implementation

        public int Offset
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "Offset", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "Offset", value, MemberTypes.Property);
            }
        }

        public int OffsetMax
        {
            get
            {
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "OffsetMax", MemberTypes.Property);
            }
        }

        public int OffsetMin
        {
            get
            {
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "OffsetMin", MemberTypes.Property);
            }
        }

        public ArrayList Offsets
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                List<string> offsets = DynamicClientDriver.GetValue<List<string>>(clientNumber, client, URIBase, TL, "Offsets", MemberTypes.Property);
                TL.LogMessage(clientNumber, "Offsets", string.Format("Returning {0} Offsets", offsets.Count));

                ArrayList returnValues = new ArrayList();
                foreach (string offset in offsets)
                {
                    returnValues.Add(offset);
                    TL.LogMessage(clientNumber, "Offsets", string.Format("Returning Offset: {0}", offset));
                }

                return returnValues;
            }
        }

        public double SubExposureDuration
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SubExposureDuration", MemberTypes.Property);
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SubExposureDuration", value, MemberTypes.Property);
            }
        }

        #endregion

    }
}
