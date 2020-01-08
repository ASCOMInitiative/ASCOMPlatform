using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

using ASCOM.DeviceInterface;
using RestSharp;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// ASCOM Remote Telescope base class
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
        private RestClient client; // Client to send and receive REST style messages to / from the remote server
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
        private int standardServerResponseTimeout;
        private int longServerResponseTimeout;
        private string userName;
        private string password;
        private bool manageConnectLocally;
        private SharedConstants.ImageArrayTransferType imageArrayTransferType;
        private SharedConstants.ImageArrayCompression imageArrayCompression;

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
                RemoteClientDriver.ReadProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId,
                    ref traceState, ref debugTraceState, ref ipAddressString, ref portNumber, ref remoteDeviceNumber, ref serviceType, ref establishConnectionTimeout, ref standardServerResponseTimeout, 
                    ref longServerResponseTimeout, ref userName, ref password, ref manageConnectLocally, ref imageArrayTransferType, ref imageArrayCompression);

                Version version = Assembly.GetEntryAssembly().GetName().Version;
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Starting initialisation, Version: " + version.ToString());

                clientNumber = RemoteClientDriver.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This instance's unique client number: " + clientNumber);

                RemoteClientDriver.ConnectToRemoteServer(ref client, ipAddressString, portNumber, serviceType, TL, clientNumber, DEVICE_TYPE, standardServerResponseTimeout, userName, password);

                URIBase = string.Format("{0}{1}/{2}/{3}/", SharedConstants.API_URL_BASE, SharedConstants.API_VERSION_V1, DEVICE_TYPE, remoteDeviceNumber.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "This devices's base URI: " + URIBase);
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Establish communications timeout: " + establishConnectionTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Standard server response timeout: " + standardServerResponseTimeout.ToString());
                TL.LogMessage(clientNumber, DEVICE_TYPE, "Long server response timeout: " + longServerResponseTimeout.ToString());
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
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            return RemoteClientDriver.Action(clientNumber, client, URIBase, TL, actionName, actionParameters);
        }

        public void CommandBlind(string command, bool raw = false)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CommandBlind(clientNumber, client, URIBase, TL, command, raw);
        }

        public bool CommandBool(string command, bool raw = false)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            return RemoteClientDriver.CommandBool(clientNumber, client, URIBase, TL, command, raw);
        }

        public string CommandString(string command, bool raw = false)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            return RemoteClientDriver.CommandString(clientNumber, client, URIBase, TL, command, raw);
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
                    TL.LogMessage(clientNumber, DEVICE_TYPE, string.Format("The Connected property is being managed locally so the new value '{0}' will not be sent to the remote server", value));
                }
                else // Send the command to the remote server
                {
                    RemoteClientDriver.SetClientTimeout(client, establishConnectionTimeout);
                    if (value) RemoteClientDriver.Connect(clientNumber, client, URIBase, TL);
                    else RemoteClientDriver.Disconnect(clientNumber, client, URIBase, TL);
                }
            }
        }

        public string Description
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                string response = string.Format("{0} REMOTE DRIVER: {1}", DriverDisplayName, RemoteClientDriver.Description(clientNumber, client, URIBase, TL));
                TL.LogMessage(clientNumber, "Description", response);
                return response;
            }
        }

        public string DriverInfo
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string remoteString = RemoteClientDriver.DriverInfo(clientNumber, client, URIBase, TL);
                string response = string.Format("{0} Version {1}, REMOTE DRIVER: {2}", DriverDisplayName, version.ToString(), remoteString);
                TL.LogMessage(clientNumber, "DriverInfo", response);
                return response;
            }
        }

        public string DriverVersion
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.DriverVersion(clientNumber, client, URIBase, TL);
            }
        }

        public short InterfaceVersion
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.InterfaceVersion(clientNumber, client, URIBase, TL);
            }
        }

        public string Name
        {
            get
            {
                string remoteString = RemoteClientDriver.GetValue<string>(clientNumber, client, URIBase, TL, "Name");
                string response = string.Format("{0} REMOTE DRIVER: {1}", DriverDisplayName, remoteString);
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
                    setupForm.StandardTimeout = standardServerResponseTimeout;
                    setupForm.LongTimeout = longServerResponseTimeout;
                    setupForm.UserName = userName;
                    setupForm.Password = password;
                    setupForm.ManageConnectLocally = manageConnectLocally;
                    setupForm.ImageArrayTransferType = imageArrayTransferType;
                    setupForm.DeviceType = DEVICE_TYPE;

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
                        standardServerResponseTimeout = (int)setupForm.StandardTimeout;
                        longServerResponseTimeout = (int)setupForm.LongTimeout;
                        userName = setupForm.UserName;
                        password = setupForm.Password;
                        manageConnectLocally = setupForm.ManageConnectLocally;
                        imageArrayTransferType = setupForm.ImageArrayTransferType;

                        // Write the changed values to the Profile
                        TL.LogMessage(clientNumber, "SetupDialog", "Writing new values to profile");
                        RemoteClientDriver.WriteProfile(clientNumber, TL, DEVICE_TYPE, DriverProgId,
                             traceState, debugTraceState, ipAddressString, portNumber, remoteDeviceNumber, serviceType, establishConnectionTimeout, standardServerResponseTimeout, longServerResponseTimeout, userName, password, manageConnectLocally, imageArrayTransferType, imageArrayCompression);

                        // Establish new host and device parameters
                        TL.LogMessage(clientNumber, "SetupDialog", "Establishing new host and device parameters");
                        RemoteClientDriver.ConnectToRemoteServer(ref client, ipAddressString, portNumber, serviceType, TL, clientNumber, DEVICE_TYPE, standardServerResponseTimeout, userName, password);
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
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.SupportedActions(clientNumber, client, URIBase, TL);
            }
        }

        #endregion

        #region ITelescope Implementation
        public void AbortSlew()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "AbortSlew");
            TL.LogMessage(clientNumber, "AbortSlew", "Slew aborted OK");
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<AlignmentModes>(clientNumber, client, URIBase, TL, "AlignmentMode");
            }
        }

        public double Altitude
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Altitude");
            }
        }

        public double ApertureArea
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ApertureArea");
            }
        }

        public double ApertureDiameter
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "ApertureDiameter");
            }
        }

        public bool AtHome
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "AtHome");
            }
        }

        public bool AtPark
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "AtPark");
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) }
            };
            return RemoteClientDriver.SendToRemoteDriver<IAxisRates>(clientNumber, client, URIBase, TL, "AxisRates", Parameters, Method.GET);
        }

        public double Azimuth
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Azimuth");
            }
        }

        public bool CanFindHome
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanFindHome");
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) }
            };
            return RemoteClientDriver.SendToRemoteDriver<bool>(clientNumber, client, URIBase, TL, "CanMoveAxis", Parameters, Method.GET);
        }

        public bool CanPark
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanPark");
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanPulseGuide");
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetDeclinationRate");
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetGuideRates");
            }
        }

        public bool CanSetPark
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetPark");
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetPierSide");
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetRightAscensionRate");
            }
        }

        public bool CanSetTracking
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSetTracking");
            }
        }

        public bool CanSlew
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlew");
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAltAz");
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAltAzAsync");
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSlewAsync");
            }
        }

        public bool CanSync
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSync");
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanSyncAltAz");
            }
        }

        public bool CanUnpark
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "CanUnpark");
            }
        }

        public double Declination
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "Declination");
            }
        }

        public double DeclinationRate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "DeclinationRate");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "DeclinationRate", value);
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            return RemoteClientDriver.SendToRemoteDriver<PierSide>(clientNumber, client, URIBase, TL, "DestinationSideOfPier", Parameters, Method.GET);
        }

        public bool DoesRefraction
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "DoesRefraction");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetBool(clientNumber, client, URIBase, TL, "DoesRefraction", value);
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<EquatorialCoordinateType>(clientNumber, client, URIBase, TL, "EquatorialSystem");
            }
        }

        public void FindHome()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "FindHome");
            TL.LogMessage(clientNumber, "FindHome", "Home found OK");
        }

        public double FocalLength
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "FocalLength");
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "GuideRateDeclination");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "GuideRateDeclination", value);
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "GuideRateRightAscension");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "GuideRateRightAscension", value);
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "IsPulseGuiding");
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AXIS_PARAMETER_NAME, ((int)Axis).ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.RATE_PARAMETER_NAME, Rate.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "MoveAxis", Parameters, Method.PUT);
        }

        public void Park()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Park");
            TL.LogMessage(clientNumber, "Park", "Parked OK");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.DIRECTION_PARAMETER_NAME, ((int)Direction).ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.DURATION_PARAMETER_NAME, Duration.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "PulseGuide", Parameters, Method.PUT);
        }

        public double RightAscension
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "RightAscension");
            }
        }

        public double RightAscensionRate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "RightAscensionRate");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "RightAscensionRate", value);
            }
        }

        public void SetPark()
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SetPark");
            TL.LogMessage(clientNumber, "SetPark", "Park set OK");
        }

        public PierSide SideOfPier
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<PierSide>(clientNumber, client, URIBase, TL, "SideOfPier");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
                Dictionary<string, string> Parameters = new Dictionary<string, string>
                {
                    { SharedConstants.SIDEOFPIER_PARAMETER_NAME, ((int)value).ToString(CultureInfo.InvariantCulture) }
                };
                RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SideOfPier", Parameters, Method.PUT);
            }
        }

        public double SiderealTime
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiderealTime");
            }
        }

        public double SiteElevation
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteElevation");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteElevation", value);
            }
        }

        public double SiteLatitude
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteLatitude");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteLatitude", value);
            }
        }

        public double SiteLongitude
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "SiteLongitude");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "SiteLongitude", value);
            }
        }

        public short SlewSettleTime
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<short>(clientNumber, client, URIBase, TL, "SlewSettleTime");
            }
            set
            {
                RemoteClientDriver.SetShort(clientNumber, client, URIBase, TL, "SlewSettleTime", value);
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToAltAz", Parameters, Method.PUT);
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToAltAzAsync", Parameters, Method.PUT);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToCoordinates", Parameters, Method.PUT);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SlewToCoordinatesAsync", Parameters, Method.PUT);
        }

        public void SlewToTarget()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SlewToTarget");
            TL.LogMessage(clientNumber, "SlewToTarget", "Slew completed OK");
        }

        public void SlewToTargetAsync()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SlewToTargetAsync");
            TL.LogMessage(clientNumber, "SlewToTargetAsync", "Slew completed OK");
        }

        public bool Slewing
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "Slewing");
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.AZ_PARAMETER_NAME, Azimuth.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.ALT_PARAMETER_NAME, Altitude.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SyncToAltAz", Parameters, Method.PUT);
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.RA_PARAMETER_NAME, RightAscension.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.DEC_PARAMETER_NAME, Declination.ToString(CultureInfo.InvariantCulture) }
            };
            RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "SyncToCoordinates", Parameters, Method.PUT);
        }

        public void SyncToTarget()
        {
            RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "SyncToTarget");
            TL.LogMessage(clientNumber, "SyncToTarget", "Slew completed OK");
        }

        public double TargetDeclination
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "TargetDeclination");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "TargetDeclination", value);
            }
        }

        public double TargetRightAscension
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<double>(clientNumber, client, URIBase, TL, "TargetRightAscension");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetDouble(clientNumber, client, URIBase, TL, "TargetRightAscension", value);
            }
        }

        public bool Tracking
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<bool>(clientNumber, client, URIBase, TL, "Tracking");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetBool(clientNumber, client, URIBase, TL, "Tracking", value);
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<DriveRates>(clientNumber, client, URIBase, TL, "TrackingRate");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                RemoteClientDriver.SetInt(clientNumber, client, URIBase, TL, "TrackingRate", (int)value);
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<ITrackingRates>(clientNumber, client, URIBase, TL, "TrackingRates");
            }
        }

        public DateTime UTCDate
        {
            get
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                return RemoteClientDriver.GetValue<DateTime>(clientNumber, client, URIBase, TL, "UTCDate");
            }
            set
            {
                RemoteClientDriver.SetClientTimeout(client, standardServerResponseTimeout);
                Dictionary<string, string> Parameters = new Dictionary<string, string>();
                string utcDateString = value.ToString(SharedConstants.ISO8601_DATE_FORMAT_STRING) + "Z";
                Parameters.Add(SharedConstants.UTCDATE_PARAMETER_NAME, utcDateString);
                TL.LogMessage(clientNumber, "UTCDate", "Sending date string: " + utcDateString);
                RemoteClientDriver.SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "UTCDate", Parameters, Method.PUT);
            }
        }

        public void Unpark()
        {
            RemoteClientDriver.SetClientTimeout(client, longServerResponseTimeout);
            RemoteClientDriver.CallMethodWithNoParameters(clientNumber, client, URIBase, TL, "Unpark");
            TL.LogMessage(clientNumber, "Unpark", "Unparked OK");
        }

        #endregion

    }
}
