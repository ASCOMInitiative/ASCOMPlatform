using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASCOM.Common.Alpaca;
using ASCOM.DeviceInterface;
using RestSharp;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// ASCOM DynamicRemoteClients Telescope base class
    /// </summary>
    public class TelescopeBaseClass : ReferenceCountedObjectBase, ITelescopeV3
    {
        #region Variables and Constants

        // Constant to set the device type
        private const string DEVICE_TYPE = "Telescope";

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
        /// Initializes a new instance of the <see cref="TelescopeBaseClass"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public TelescopeBaseClass(string RequiredDriverNumber, string RequiredDriverDisplayName, string RequiredProgId)
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
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("User name is Null or Empty: {0}, User name is Null or White Space: {1}", string.IsNullOrEmpty(userName), string.IsNullOrWhiteSpace(userName)));
                TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("User name length: {0}", password.Length));
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

        public ArrayList SupportedActions
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.SupportedActions(clientNumber, client, URIBase, TL);
            }
        }

        #endregion

        #region ITelescope Implementation
        public void AbortSlew()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "AbortSlew", MemberTypes.Method);
            TL.LogMessage(clientNumber, "AbortSlew", "Slew aborted OK");
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<AlignmentModes>(clientNumber, client, URIBase, TL, "AlignmentMode", MemberTypes.Property);
            }
        }

        public double Altitude
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Altitude", MemberTypes.Property);
            }
        }

        public double ApertureArea
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ApertureArea", MemberTypes.Property);
            }
        }

        public double ApertureDiameter
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ApertureDiameter", MemberTypes.Property);
            }
        }

        public bool AtHome
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "AtHome", MemberTypes.Property);
            }
        }

        public bool AtPark
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "AtPark", MemberTypes.Property);
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) }
            };
            return DynamicClientDriver.SendToRemoteDevice<IAxisRates>(clientNumber, client, URIBase, TL, "AxisRates", Parameters, Method.GET,MemberTypes.Method);
        }

        public double Azimuth
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Azimuth", MemberTypes.Property);
            }
        }

        public bool CanFindHome
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanFindHome", MemberTypes.Property);
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) }
            };
            return DynamicClientDriver.SendToRemoteDevice<bool>(clientNumber, client, URIBase, TL, "CanMoveAxis", Parameters, Method.GET, MemberTypes.Method);
        }

        public bool CanPark
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanPark", MemberTypes.Property);
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

        public bool CanSetDeclinationRate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetDeclinationRate", MemberTypes.Property);
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetGuideRates", MemberTypes.Property);
            }
        }

        public bool CanSetPark
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetPark", MemberTypes.Property);
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetPierSide", MemberTypes.Property);
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetRightAscensionRate", MemberTypes.Property);
            }
        }

        public bool CanSetTracking
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetTracking", MemberTypes.Property);
            }
        }

        public bool CanSlew
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlew", MemberTypes.Property);
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAltAz", MemberTypes.Property);
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAltAzAsync", MemberTypes.Property);
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAsync", MemberTypes.Property);
            }
        }

        public bool CanSync
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSync", MemberTypes.Property);
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSyncAltAz", MemberTypes.Property);
            }
        }

        public bool CanUnpark
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanUnpark", MemberTypes.Property);
            }
        }

        public double Declination
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Declination", MemberTypes.Property);
            }
        }

        public double DeclinationRate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "DeclinationRate", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "DeclinationRate", value, MemberTypes.Property);
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            return DynamicClientDriver.SendToRemoteDevice<PierSide>(clientNumber, client, URIBase, TL, "DestinationSideOfPier", Parameters, Method.GET, MemberTypes.Method);
        }

        public bool DoesRefraction
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "DoesRefraction", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "DoesRefraction", value, MemberTypes.Property);
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<EquatorialCoordinateType>(clientNumber, client, URIBase, TL, "EquatorialSystem", MemberTypes.Property);
            }
        }

        public void FindHome()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "FindHome", MemberTypes.Method);
            TL.LogMessage(clientNumber, "FindHome", "Home found OK");
        }

        public double FocalLength
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "FocalLength", MemberTypes.Property);
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "GuideRateDeclination", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "GuideRateDeclination", value, MemberTypes.Property);
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "GuideRateRightAscension", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "GuideRateRightAscension", value, MemberTypes.Property);
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

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.RATE_PARAMETER_NAME, Rate.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "MoveAxis", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void Park()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Park", MemberTypes.Method);
            TL.LogMessage(clientNumber, "Park", "Parked OK");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.DIRECTION_PARAMETER_NAME, ((int)Direction).ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.DURATION_PARAMETER_NAME, Duration.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "PulseGuide", Parameters, Method.PUT, MemberTypes.Method);
        }

        public double RightAscension
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "RightAscension", MemberTypes.Property);
            }
        }

        public double RightAscensionRate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "RightAscensionRate", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "RightAscensionRate", value, MemberTypes.Property);
            }
        }

        public void SetPark()
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SetPark", MemberTypes.Method);
            TL.LogMessage(clientNumber, "SetPark", "Park set OK");
        }

        public PierSide SideOfPier
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<PierSide>(clientNumber, client, URIBase, TL, "SideOfPier", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
                Dictionary<string, string> Parameters = new Dictionary<string, string>
                {
                    { AlpacaConstants.SIDEOFPIER_PARAMETER_NAME, ((int)value).ToString(CultureInfo.InvariantCulture) }
                };
                DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SideOfPier", Parameters, Method.PUT, MemberTypes.Property);
            }
        }

        public double SiderealTime
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiderealTime", MemberTypes.Property);
            }
        }

        public double SiteElevation
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteElevation", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteElevation", value, MemberTypes.Property);
            }
        }

        public double SiteLatitude
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteLatitude", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteLatitude", value, MemberTypes.Property);
            }
        }

        public double SiteLongitude
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteLongitude", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteLongitude", value, MemberTypes.Property);
            }
        }

        public short SlewSettleTime
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "SlewSettleTime", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetShort(clientNumber, client, URIBase, TL, "SlewSettleTime", value, MemberTypes.Property);
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToAltAz", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToAltAzAsync", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToCoordinates", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToCoordinatesAsync", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SlewToTarget()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SlewToTarget", MemberTypes.Method);
            TL.LogMessage(clientNumber, "SlewToTarget", "Slew completed OK");
        }

        public void SlewToTargetAsync()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SlewToTargetAsync", MemberTypes.Method);
            TL.LogMessage(clientNumber, "SlewToTargetAsync", "Slew completed OK");
        }

        public bool Slewing
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "Slewing", MemberTypes.Property);
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SyncToAltAz", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "SyncToCoordinates", Parameters, Method.PUT, MemberTypes.Method);
        }

        public void SyncToTarget()
        {
            DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SyncToTarget", MemberTypes.Method);
            TL.LogMessage(clientNumber, "SyncToTarget", "Slew completed OK");
        }

        public double TargetDeclination
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "TargetDeclination", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "TargetDeclination", value, MemberTypes.Property);
            }
        }

        public double TargetRightAscension
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "TargetRightAscension", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetDouble(clientNumber, client, URIBase, TL, "TargetRightAscension", value, MemberTypes.Property);
            }
        }

        public bool Tracking
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "Tracking", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetBool(clientNumber, client, URIBase, TL, "Tracking", value, MemberTypes.Property);
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<DriveRates>(clientNumber, client, URIBase, TL, "TrackingRate", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                DynamicClientDriver.SetInt(clientNumber, client, URIBase, TL, "TrackingRate", (int)value, MemberTypes.Property);
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<ITrackingRates>(clientNumber, client, URIBase, TL, "TrackingRates", MemberTypes.Property);
            }
        }

        public DateTime UTCDate
        {
            get
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                return DynamicClientDriver.GetValue<DateTime>(clientNumber, client, URIBase, TL, "UTCDate", MemberTypes.Property);
            }
            set
            {
                DynamicClientDriver.SetClientTimeout(client, standardDeviceResponseTimeout);
                Dictionary<string, string> Parameters = new Dictionary<string, string>();
                string utcDateString = value.ToString(AlpacaConstants.ISO8601_DATE_FORMAT_STRING) + "Z";
                Parameters.Add(AlpacaConstants.UTCDATE_PARAMETER_NAME, utcDateString);
                TL.LogMessage(clientNumber, "UTCDate", "Sending date string: " + utcDateString);
                DynamicClientDriver.SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "UTCDate", Parameters, Method.PUT, MemberTypes.Property);
            }
        }

        public void Unpark()
        {
            DynamicClientDriver.SetClientTimeout(client, longDeviceResponseTimeout);
            DynamicClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Unpark", MemberTypes.Method);
            TL.LogMessage(clientNumber, "Unpark", "Unparked OK");
        }

        #endregion

        #region ITelescopeV4 implementation

        public void Connect()
        {
            // Call the device's Connect method if this is a Platform 7 or later device, otherwise simulate the connect call
            if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE.ToDeviceType(), InterfaceVersion)) // We are presenting a Platform 7 or later device
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
            if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE.ToDeviceType(), InterfaceVersion)) // We are presenting a Platform 7 or later device
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
                if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE.ToDeviceType(), InterfaceVersion)) // We are presenting a Platform 7 or later device
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

                // No exception so return emulated state
                return connecting;
            }
        }

        public ArrayList DeviceState
        {
            get
            {
                // Call the device's DeviceState method if this is a Platform 7 or later device, otherwise simulate the DeviceState method
                if (DeviceCapabilities.HasConnectAndDeviceState(DEVICE_TYPE.ToDeviceType(), InterfaceVersion)) // We are presenting a Platform 7 or later device
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
