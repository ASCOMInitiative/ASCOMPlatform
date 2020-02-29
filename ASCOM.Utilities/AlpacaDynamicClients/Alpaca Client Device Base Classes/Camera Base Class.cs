using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using ASCOM.DeviceInterface;
using RestSharp;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// ASCOM DynamicRemoteClients Camera base class.
    /// </summary>
    public class CameraBaseClass : ReferenceCountedObjectBase, ICameraV2
    {
        #region Variables and Constants

        // Constant to set the device type
        private const string DEVICE_TYPE = "Camera";

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
        private SharedConstants.ImageArrayTransferType imageArrayTransferType;
        private SharedConstants.ImageArrayCompression imageArrayCompression;
        private string uniqueId;
        private bool enableRediscovery;

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

                if (TL == null) TL = new TraceLoggerPlus("", string.Format(SharedConstants.TRACELOGGER_NAME_FORMAT_STRING, DriverNumber, DEVICE_TYPE));
                DynamicClientDriver.ReadProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId,
                    ref traceState, ref debugTraceState, ref ipAddressString, ref portNumber, ref remoteDeviceNumber, ref serviceType, ref establishConnectionTimeout, ref standardDeviceResponseTimeout,
                    ref longDeviceResponseTimeout, ref userName, ref password, ref manageConnectLocally, ref imageArrayTransferType, ref imageArrayCompression, ref uniqueId, ref enableRediscovery);
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("Trace state: {0}, Debug Trace State: {1}, TraceLogger Debug State: {2}", traceState, debugTraceState, TL.DebugTraceState));
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Starting initialisation, Version: " + version.ToString());

                clientNumber = DynamicClientDriver.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This instance's unique client number: " + clientNumber);

                DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE, 
                                                          standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery);

                URIBase = string.Format("{0}{1}/{2}/{3}/", SharedConstants.API_URL_BASE, SharedConstants.API_VERSION_V1, DEVICE_TYPE, remoteDeviceNumber.ToString());
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
                string response = string.Format("{0} REMOTE DEVICE: {1}", DriverDisplayName, DynamicClientDriver.Description(clientNumber, client, URIBase, TL));
                TL.LogMessage(clientNumber, "Description", response);
                return response;
            }
        }

        public string DriverInfo
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string remoteString = DynamicClientDriver.DriverInfo(clientNumber, client, URIBase, TL);
                string response = string.Format("{0} Version {1}, REMOTE DEVICE: {2}", DriverDisplayName, version.ToString(), remoteString);
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
                string remoteString = DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "Name");
                string response = string.Format("{0} REMOTE DEVICE: {1}", DriverDisplayName, remoteString);
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
                        enableRediscovery=setupForm.EnableRediscovery ;

                        // Write the changed values to the Profile
                        TL.LogMessage(clientNumber, "SetupDialog", "Writing new values to profile");
                        DynamicClientDriver.WriteProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId, traceState, debugTraceState, ipAddressString, portNumber, remoteDeviceNumber, serviceType,
                            establishConnectionTimeout, standardDeviceResponseTimeout, longDeviceResponseTimeout, userName, password, manageConnectLocally, imageArrayTransferType, imageArrayCompression, uniqueId, enableRediscovery);

                        // Establish new host and device parameters
                        TL.LogMessage(clientNumber, "SetupDialog", "Establishing new host and device parameters");
                        DynamicClientDriver.ConnectToRemoteDevice(ref client, ipAddressString, portNumber, establishConnectionTimeout, serviceType, TL, clientNumber, DriverProgId, DEVICE_TYPE,
                                                                  standardDeviceResponseTimeout, userName, password, uniqueId, enableRediscovery);
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
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "AbortExposure");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.DIRECTION_PARAMETER_NAME, ((int)Direction).ToString() },
                { SharedConstants.DURATION_PARAMETER_NAME, Duration.ToString() }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "PulseGuide", Parameters, Method.PUT);
        }

        public void StartExposure(double Duration, bool Light)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.DURATION_PARAMETER_NAME, Duration.ToString() },
                { SharedConstants.LIGHT_PARAMETER_NAME, Light.ToString() }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "StartExposure", Parameters, Method.PUT);
        }

        public void StopExposure()
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "StopExposure");
        }

        public short BinX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BinX");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "BinX", value);
            }
        }

        public short BinY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BinY");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "BinY", value);
            }
        }

        public CameraStates CameraState
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<CameraStates>(clientNumber, client, URIBase, TL, "CameraState");
            }
        }

        public int CameraXSize
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "CameraXSize");
            }
        }

        public int CameraYSize
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "CameraYSize");
            }
        }

        public bool CanAbortExposure
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanAbortExposure");
            }
        }

        public bool CanAsymmetricBin
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanAsymmetricBin");
            }
        }

        public bool CanGetCoolerPower
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanGetCoolerPower");
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanPulseGuide");
            }
        }

        public bool CanSetCCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetCCDTemperature");
            }
        }

        public bool CanStopExposure
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanStopExposure");
            }
        }

        public double CCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "CCDTemperature");
            }
        }

        public bool CoolerOn
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CoolerOn");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "CoolerOn", value);
            }
        }

        public double CoolerPower
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "CoolerPower");
            }
        }

        public double ElectronsPerADU
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ElectronsPerADU");
            }
        }

        public double FullWellCapacity
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "FullWellCapacity");
            }
        }

        public bool HasShutter
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "HasShutter");
            }
        }

        public double HeatSinkTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "HeatSinkTemperature");
            }
        }

        public object ImageArray
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<Array>(clientNumber, client, URIBase, TL, "ImageArray", imageArrayTransferType, imageArrayCompression);
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
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "ImageReady");
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "IsPulseGuiding");
            }
        }

        public double LastExposureDuration
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "LastExposureDuration");
            }
        }

        public string LastExposureStartTime
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "LastExposureStartTime");
            }
        }

        public int MaxADU
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "MaxADU");
            }
        }

        public short MaxBinX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "MaxBinX");
            }
        }

        public short MaxBinY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "MaxBinY");
            }
        }

        public int NumX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "NumX");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "NumX", value);
            }
        }

        public int NumY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "NumY");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "NumY", value);
            }
        }

        public double PixelSizeX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "PixelSizeX");
            }
        }

        public double PixelSizeY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "PixelSizeY");
            }
        }

        public double SetCCDTemperature
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SetCCDTemperature");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SetCCDTemperature", value);
            }
        }

        public int StartX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "StartX");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "StartX", value);
            }
        }

        public int StartY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<int>(clientNumber, client, URIBase, TL, "StartY");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "StartY", value);
            }
        }

        public short BayerOffsetX
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BayerOffsetX");
            }
        }

        public short BayerOffsetY
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "BayerOffsetY");
            }
        }

        public bool CanFastReadout
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanFastReadout");
            }
        }

        public double ExposureMax
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureMax");
            }
        }

        public double ExposureMin
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureMin");
            }
        }

        public double ExposureResolution
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ExposureResolution");
            }
        }

        public bool FastReadout
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "FastReadout");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "FastReadout", value);
            }
        }

        public short Gain
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "Gain");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "Gain", value);
            }
        }

        public short GainMax
        {
            get
            {
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "GainMax");
            }
        }

        public short GainMin
        {
            get
            {
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "GainMin");
            }
        }

        public ArrayList Gains
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                List<string> gains = DynamicClientDriver.GetValue<List<string>>(clientNumber, client, URIBase, TL, "Gains");
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
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "PercentCompleted");
            }
        }

        public short ReadoutMode
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "ReadoutMode");
            }

            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "ReadoutMode", value);
            }
        }

        public ArrayList ReadoutModes
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                List<string> modes = DynamicClientDriver.GetValue<List<string>>(clientNumber, client, URIBase, TL, "ReadoutModes");
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
                return DynamicClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "SensorName");
            }
        }

        public SensorType SensorType
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<SensorType>(clientNumber, client, URIBase, TL, "SensorType");
            }
        }

        #endregion

    }
}
