using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

using ASCOM.Utilities;
using ASCOM.DeviceInterface;

using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Http;

namespace ASCOM.DynamicRemoteClients
{
    public static class RemoteClientDriver
    {
        #region Private variables and constants

        //Private variables
        private static TraceLoggerPlus TLLocalServer;

        private static uint uniqueTransactionNumber = 0; // Unique number that increments on each call to TransactionNumber

        // Lock objects
        private readonly static object connectLockObject = new object();
        private readonly static object transactionCountlockObject = new object();

        private static ConcurrentDictionary<long, bool> connectStates;

        #endregion

        #region Initialiser

        /// <summary>
        /// Static initialiser to set up the objects we need at run time
        /// </summary>
        static RemoteClientDriver()
        {
            try
            {
                TLLocalServer = new TraceLoggerPlus("", "RemoteClientLocalServer")
                {
                    Enabled = false
                }; // Trace state is set in ReadProfile, immediately after being read from the Profile
                TLLocalServer.LogMessage("RemoteClient", $"Initialising - Version: { Assembly.GetEntryAssembly().GetName().Version}");

                connectStates = new ConcurrentDictionary<long, bool>();

                TLLocalServer.LogMessage("RemoteClient", "Initialisation complete.");
            }
            catch (Exception ex)
            {
                TLLocalServer.LogMessageCrLf("RemoteClient", ex.ToString());
                MessageBox.Show(ex.ToString(), "Error initialising the RemoteClient Telescope", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Utility code

        /// <summary>
        /// Returns a unique client number to the calling instance in the range 1::65536
        /// </summary>
        public static uint GetUniqueClientNumber()
        {
            uint randomvalue;

            using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
            {
                byte[] rno = new byte[5]; // Create a four byte array
                rg.GetBytes(rno); // Fill the array with four random bytes
                rno[2] = 0; // Zero the higher two bytes to limit the resulting integer to the range 0::65535
                rno[3] = 0;
                rno[4] = 0;
                randomvalue = BitConverter.ToUInt32(rno, 0) + 1; // Convert the bytes to an integer in the range 0::65535 and add 1 to get an integer in the range 1::65536
            }

            return randomvalue;
        }

        /// <summary>
        /// Returns a unique client number to the calling instance
        /// </summary>
        public static uint TransactionNumber()
        {
            lock (transactionCountlockObject)
            {
                uniqueTransactionNumber += 1;
            }
            return uniqueTransactionNumber;
        }

        /// <summary>
        /// Tests whether the hub is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the client making the call</param>
        /// <returns>Boolean true if the hub is already connected</returns>
        public static bool IsHardwareConnected(TraceLoggerPlus TL)
        {
            if (TL.DebugTraceState) TL.LogMessage("IsHardwareConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
            return connectStates.Count > 0;
        }

        /// <summary>
        /// Test whether a particular client is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the calling client</param>
        /// <returns></returns>
        public static bool IsClientConnected(uint clientNumber, TraceLoggerPlus TL)
        {
            foreach (KeyValuePair<long, bool> kvp in connectStates)
            {
                TL.LogMessage(clientNumber, "IsClientConnected", string.Format("This device ClientID is in the ConnectedStates list: {0}, Value: {1}", kvp.Key, kvp.Value));
            }

            TL.LogMessage(clientNumber, "IsClientConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + connectStates.ContainsKey(clientNumber).ToString());

            return connectStates.ContainsKey(clientNumber);
        }

        /// <summary>
        /// Returns the number of connected clients
        /// </summary>
        public static uint ConnectionCount(TraceLoggerPlus TL)
        {
            TL.LogMessage("ConnectionCount", connectStates.Count.ToString());
            return (uint)connectStates.Count;
        }

        /// <summary>
        /// Return name of current method
        /// </summary>
        /// <returns>Name of current method</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public static void SetClientTimeout(RestClient client, int serverResponseTimeout)
        {
            client.Timeout = serverResponseTimeout * 1000;
            client.ReadWriteTimeout = serverResponseTimeout * 1000;
        }

        public static void ConnectToRemoteServer(ref RestClient client, string ipAddressString, decimal portNumber, string serviceType, TraceLoggerPlus TL, uint clientNumber, string deviceType, int serverResponseTimeout, string userName, string password)
        {
            TL.LogMessage(clientNumber, deviceType, "Connecting to device: " + ipAddressString + ":" + portNumber.ToString());

            string clientHostAddress = string.Format("{0}://{1}:{2}", serviceType, ipAddressString, portNumber.ToString());
            TL.LogMessage(clientNumber, deviceType, "Client host address: " + clientHostAddress);

            if (client != null)
            {
                client.ClearHandlers();
            }

            client = new RestClient(clientHostAddress)
            {
                PreAuthenticate = true
            };
            TL.LogMessage(clientNumber, deviceType, "Creating Authenticator");
            client.Authenticator = new HttpBasicAuthenticator(userName.Unencrypt(TL), password.Unencrypt(TL)); // Need to decrypt the user name and password so the correct values are sent to the remote server
            SetClientTimeout(client, serverResponseTimeout);
        }

        #endregion

        #region Profile management
        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        public static void ReadProfile(uint clientNumber, TraceLoggerPlus TL, string deviceType, string driverProgID,
                                       ref bool traceState,
                                       ref bool debugTraceState,
                                       ref string ipAddressString,
                                       ref decimal portNumber,
                                       ref decimal remoteDeviceNumber,
                                       ref string serviceType,
                                       ref int establishConnectionTimeout,
                                       ref int standardServerResponseTimeout,
                                       ref int longServerResponseTimeout,
                                       ref string userName,
                                       ref string password,
                                       ref bool manageConnectLocally,
                                       ref SharedConstants.ImageArrayTransferType imageArrayTransferType,
                                       ref SharedConstants.ImageArrayCompression imageArrayCompression
                                       )
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = deviceType;

                // Initialise the logging trace state from the Profile
                traceState = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.TRACE_LEVEL_PROFILENAME, string.Empty, SharedConstants.CLIENT_TRACE_LEVEL_DEFAULT);
                TL.Enabled = traceState; // Set the logging state immediately after this has been retrieved from Profile

                // Initialise other variables from the Profile
                debugTraceState = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.DEBUG_TRACE_PROFILENAME, string.Empty, SharedConstants.DEBUG_TRACE_DEFAULT);
                ipAddressString = driverProfile.GetValue(driverProgID, SharedConstants.IPADDRESS_PROFILENAME, string.Empty, SharedConstants.IPADDRESS_DEFAULT);
                portNumber = GetDecimalValue(TL, driverProfile, driverProgID, SharedConstants.PORTNUMBER_PROFILENAME, string.Empty, SharedConstants.PORTNUMBER_DEFAULT);
                remoteDeviceNumber = GetDecimalValue(TL, driverProfile, driverProgID, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, string.Empty, SharedConstants.REMOTE_DEVICE_NUMBER_DEFAULT);
                serviceType = driverProfile.GetValue(driverProgID, SharedConstants.SERVICE_TYPE_PROFILENAME, string.Empty, SharedConstants.SERVICE_TYPE_DEFAULT);
                establishConnectionTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_DEFAULT);
                standardServerResponseTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.STANDARD_SERVER_RESPONSE_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.STANDARD_SERVER_RESPONSE_TIMEOUT_DEFAULT);
                longServerResponseTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.LONG_SERVER_RESPONSE_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.LONG_SERVER_RESPONSE_TIMEOUT_DEFAULT);
                userName = driverProfile.GetValue(driverProgID, SharedConstants.USERNAME_PROFILENAME, string.Empty, SharedConstants.USERNAME_DEFAULT);
                password = driverProfile.GetValue(driverProgID, SharedConstants.PASSWORD_PROFILENAME, string.Empty, SharedConstants.PASSWORD_DEFAULT);
                manageConnectLocally = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, string.Empty, SharedConstants.MANAGE_CONNECT_LOCALLY_DEFAULT);
                imageArrayTransferType = (SharedConstants.ImageArrayTransferType)GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, string.Empty, (int)SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT);
                imageArrayCompression = (SharedConstants.ImageArrayCompression)GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, string.Empty, (int)SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT);

                TL.DebugTraceState = debugTraceState; // Save the debug state for use when needed wherever the trace logger is used

                TL.LogMessage(clientNumber, "ReadProfile", string.Format("New values: Device IP: {0} {1}, Remote device number: {2}", ipAddressString, portNumber.ToString(), remoteDeviceNumber.ToString()));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public static void WriteProfile(uint clientNumber, TraceLoggerPlus TL, string deviceType, string driverProgID,
                                        bool traceState,
                                        bool debugTraceState,
                                        string ipAddressString,
                                        decimal portNumber,
                                        decimal remoteDeviceNumber,
                                        string serviceType,
                                        int establishConnectionTimeout,
                                        int standardServerResponseTimeout,
                                        int longServerResponseTimeout,
                                        string userName,
                                        string password,
                                        bool manageConnectLocally,
                                        SharedConstants.ImageArrayTransferType imageArrayTransferType,
                                        SharedConstants.ImageArrayCompression imageArrayCompression
                                        )
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = deviceType;

                // Save the variable state to the Profile
                driverProfile.WriteValue(driverProgID, SharedConstants.TRACE_LEVEL_PROFILENAME, traceState.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.DEBUG_TRACE_PROFILENAME, debugTraceState.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.IPADDRESS_PROFILENAME, ipAddressString.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.PORTNUMBER_PROFILENAME, portNumber.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, remoteDeviceNumber.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.SERVICE_TYPE_PROFILENAME, serviceType.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, establishConnectionTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.STANDARD_SERVER_RESPONSE_TIMEOUT_PROFILENAME, standardServerResponseTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.LONG_SERVER_RESPONSE_TIMEOUT_PROFILENAME, longServerResponseTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.USERNAME_PROFILENAME, userName.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.PASSWORD_PROFILENAME, password.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, manageConnectLocally.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, ((int)imageArrayTransferType).ToString());
                driverProfile.WriteValue(driverProgID, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, ((int)imageArrayCompression).ToString());

                TL.DebugTraceState = debugTraceState; // Save the new debug state for use when needed wherever the trace logger is used

                TL.LogMessage(clientNumber, "WriteProfile", string.Format("New values: Device IP: {0} {1}, Remote device number: {2}", ipAddressString, portNumber.ToString(), remoteDeviceNumber.ToString()));
            }
        }

        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static bool GetBooleanValue(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, bool defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetBooleanValue", $"{valueName} = '{profileValue}'");

            bool convertedOk = Boolean.TryParse(profileValue, out bool returnValue); // Try to convert the profile value to a boolean
            if (!convertedOk) // If unable to convert to a boolean, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetBooleanValue", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;
        }

        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static int GetInt32Value(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, int defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetInt32Value", $"{valueName} = '{profileValue}'");

            bool convertedOk = Int32.TryParse(profileValue, out int returnValue); // Try to convert the profile value to an int
            if (!convertedOk) // If unable to convert to a int, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetInt32Value", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;

        }
        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static decimal GetDecimalValue(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, decimal defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetDecimalValue", $"{valueName} = '{profileValue}'");

            bool convertedOk = Decimal.TryParse(profileValue, out decimal returnValue); // Try to convert the profile value to a decimal
            if (!convertedOk) // If unable to convert to a decimal, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetDecimalValue", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;
        }
        #endregion

        #region Remote access methods

        public static void CallMethodWithNoParameters(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        /// <summary>
        /// Overload used by methods other than ImageArray and ImageArrayVariant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static T GetValue<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method)
        {
            return GetValue<T>(clientNumber, client, URIBase, TL, method, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT, SharedConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT); // Set an arbitrary value for ImageArrayTransferType
        }

        /// <summary>
        /// Overload for use by the ImageArray and ImageArrayVariant methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="imageArrayTransferType"></param>
        /// <returns></returns>
        public static T GetValue<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, SharedConstants.ImageArrayTransferType imageArrayTransferType, SharedConstants.ImageArrayCompression imageArrayCompression)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            return SendToRemoteDriver<T>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, imageArrayTransferType, imageArrayCompression);
        }

        public static void SetBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, bool parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetInt(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, int parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetShort(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, double parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetDoubleWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, double parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.VALUE_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetBoolWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, bool parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.STATE_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static void SetStringWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, string parmeterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { SharedConstants.NAME_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT);
        }

        public static string GetStringIndexedString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, string parameterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.SENSORNAME_PARAMETER_NAME, parameterValue }
            };
            return SendToRemoteDriver<string>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET);
        }

        public static double GetStringIndexedDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, string parameterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.SENSORNAME_PARAMETER_NAME, parameterValue }
            };
            return SendToRemoteDriver<double>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET);
        }

        public static double GetShortIndexedDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDriver<double>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET);
        }

        public static bool GetShortIndexedBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDriver<bool>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET);
        }

        public static string GetShortIndexedString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDriver<string>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET);
        }

        /// <summary>
        /// Send a command to the remote server, retrying a given number of times if a socket exception is received
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="Parameters"></param>
        /// <param name="HttpMethod"></param>
        /// <returns></returns>
        public static T SendToRemoteDriver<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, Dictionary<string, string> Parameters, Method HttpMethod)
        {
            return SendToRemoteDriver<T>(clientNumber, client, URIBase, TL, method, Parameters, HttpMethod, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT, SharedConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT);
        }

        /// <summary>
        /// Send a command to the remote server, retrying a given number of times if a socket exception is received, specifying an image array transfer type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="Parameters"></param>
        /// <param name="HttpMethod"></param>
        /// <param name="imageArrayTransferType"></param>
        /// <returns></returns>
        public static T SendToRemoteDriver<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, Dictionary<string, string> Parameters, Method HttpMethod, SharedConstants.ImageArrayTransferType imageArrayTransferType, SharedConstants.ImageArrayCompression imageArrayCompression)
        {
            int retryCounter = 0; // Initialise the socket error retry counter
            Stopwatch sw = new Stopwatch(); // Stopwatch to time activities
            long lastTime = 0; // Holder for the accumulated elapsed time, used when reporting intermediate step timings
            Array remoteArray = null;

            sw.Start();

            do // Socket communications error retry loop
            {
                try
                {
                    const string LOG_FORMAT_STRING = "Client Txn ID: {0}, Server Txn ID: {1}, Value: {2}";

                    RestResponseBase restResponseBase = null; // This has to be the base class of the data type classes in order for exception and error responses to be handled generically
                    RestRequest request = new RestRequest((URIBase + method).ToLowerInvariant(), HttpMethod);
                    {
                        request.RequestFormat = DataFormat.Json;
                    };

                    // Set the default JSON compression behaviour to None
                    client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.None); // Prevent any decompression

                    // Apply appropriate headers to control image array transfer
                    if (typeof(T) == typeof(Array))
                    {
                        switch (imageArrayCompression)
                        {
                            case SharedConstants.ImageArrayCompression.None:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.None); // Prevent any decompression
                                break;
                            case SharedConstants.ImageArrayCompression.Deflate:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.Deflate); // Allow only Deflate decompression
                                break;
                            case SharedConstants.ImageArrayCompression.GZip:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.GZip); // Allow only GZip decompression
                                break;
                            case SharedConstants.ImageArrayCompression.GZipOrDeflate:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip); // Allow both Deflate and GZip decompression
                                break;
                            default:
                                throw new InvalidValueException($"Invalid image array compression type: {imageArrayCompression} - Correct this in the Remote Client setup dialogue.");
                        }

                        switch (imageArrayTransferType)
                        {
                            case SharedConstants.ImageArrayTransferType.JSON:
                                // No extra action because "accepts = application/json" will be applied automatically by the client
                                break;
                            case SharedConstants.ImageArrayTransferType.Base64HandOff:
                                request.AddHeader(SharedConstants.BASE64_HANDOFF_HEADER, SharedConstants.BASE64_HANDOFF_SUPPORTED);
                                break;
                            default:
                                throw new InvalidValueException($"Invalid image array transfer type: {imageArrayTransferType} - Correct this in the Remote Client setup dialogue.");
                        }
                    }

                    // Add the transaction number and client ID parameters
                    uint transaction = TransactionNumber();
                    request.AddParameter(SharedConstants.CLIENTTRANSACTION_PARAMETER_NAME, transaction.ToString());
                    request.AddParameter(SharedConstants.CLIENTID_PARAMETER_NAME, clientNumber.ToString());

                    // Add any supplied parameters to the request
                    foreach (KeyValuePair<string, string> parameter in Parameters)
                    {
                        request.AddParameter(parameter.Key, parameter.Value);
                    }

                    lastTime = sw.ElapsedMilliseconds;
                    // Call the remote device and get the response
                    if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, "Client Txn ID: " + transaction.ToString() + ", Sending command to remote server");
                    IRestResponse deviceJsonResponse = client.Execute(request);
                    long timeServerResponse = sw.ElapsedMilliseconds - lastTime;

                    string responseContent;
                    if (deviceJsonResponse.Content.Length > 1000) responseContent = deviceJsonResponse.Content.Substring(0, 1000);
                    else responseContent = deviceJsonResponse.Content;
                    TL.LogMessage(clientNumber, method, string.Format("Response Status: '{0}', Response: {1}", deviceJsonResponse.StatusDescription, responseContent));

                    if ((deviceJsonResponse.ResponseStatus == ResponseStatus.Completed) & (deviceJsonResponse.StatusCode == System.Net.HttpStatusCode.OK))
                    {
                        // GENERAL MULTI-DEVICE TYPES
                        if (typeof(T) == typeof(bool))
                        {
                            BoolResponse boolResponse = JsonConvert.DeserializeObject<BoolResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, boolResponse.ClientTransactionID, boolResponse.ServerTransactionID, boolResponse.Value.ToString())); //, boolResponse.Method));
                            if (CallWasSuccessful(TL, boolResponse)) return (T)((object)boolResponse.Value);
                            restResponseBase = (RestResponseBase)boolResponse;
                        }
                        if (typeof(T) == typeof(float))
                        {
                            // Handle float as double over the web, remembering to convert the returned value to float
                            DoubleResponse doubleResponse = JsonConvert.DeserializeObject<DoubleResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleResponse.ClientTransactionID, doubleResponse.ServerTransactionID, doubleResponse.Value.ToString())); //, doubleResponse.Method));
                            float floatValue = (float)doubleResponse.Value;
                            if (CallWasSuccessful(TL, doubleResponse)) return (T)((object)floatValue);
                            restResponseBase = (RestResponseBase)doubleResponse;
                        }
                        if (typeof(T) == typeof(double))
                        {
                            DoubleResponse doubleResponse = JsonConvert.DeserializeObject<DoubleResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleResponse.ClientTransactionID, doubleResponse.ServerTransactionID, doubleResponse.Value.ToString())); //, doubleResponse.Method));
                            if (CallWasSuccessful(TL, doubleResponse)) return (T)((object)doubleResponse.Value);
                            restResponseBase = (RestResponseBase)doubleResponse;
                        }
                        if (typeof(T) == typeof(string))
                        {
                            StringResponse stringResponse = JsonConvert.DeserializeObject<StringResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringResponse.ClientTransactionID, stringResponse.ServerTransactionID, stringResponse.Value.ToString())); //, stringResponse.Method));
                            if (CallWasSuccessful(TL, stringResponse)) return (T)((object)stringResponse.Value);
                            restResponseBase = (RestResponseBase)stringResponse;
                        }
                        if (typeof(T) == typeof(string[]))
                        {
                            StringArrayResponse stringArrayResponse = JsonConvert.DeserializeObject<StringArrayResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringArrayResponse.ClientTransactionID, stringArrayResponse.ServerTransactionID, stringArrayResponse.Value.Count())); //, stringArrayResponse.Method));
                            if (CallWasSuccessful(TL, stringArrayResponse)) return (T)((object)stringArrayResponse.Value);
                            restResponseBase = (RestResponseBase)stringArrayResponse;
                        }
                        if (typeof(T) == typeof(short))
                        {
                            ShortResponse shortResponse = JsonConvert.DeserializeObject<ShortResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortResponse.ClientTransactionID, shortResponse.ServerTransactionID, shortResponse.Value.ToString())); //, shortResponse.Method));
                            if (CallWasSuccessful(TL, shortResponse)) return (T)((object)shortResponse.Value);
                            restResponseBase = (RestResponseBase)shortResponse;
                        }
                        if (typeof(T) == typeof(int))
                        {
                            IntResponse intResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intResponse.ClientTransactionID, intResponse.ServerTransactionID, intResponse.Value.ToString())); //, intResponse.Method));
                            if (CallWasSuccessful(TL, intResponse)) return (T)((object)intResponse.Value);
                            restResponseBase = (RestResponseBase)intResponse;
                        }
                        if (typeof(T) == typeof(int[]))
                        {
                            IntArray1DResponse intArrayResponse = JsonConvert.DeserializeObject<IntArray1DResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArrayResponse.ClientTransactionID, intArrayResponse.ServerTransactionID, intArrayResponse.Value.Count())); //, intArrayResponse.Method));
                            if (CallWasSuccessful(TL, intArrayResponse)) return (T)((object)intArrayResponse.Value);
                            restResponseBase = (RestResponseBase)intArrayResponse;
                        }
                        if (typeof(T) == typeof(DateTime))
                        {
                            DateTimeResponse dateTimeResponse = JsonConvert.DeserializeObject<DateTimeResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, dateTimeResponse.ClientTransactionID, dateTimeResponse.ServerTransactionID, dateTimeResponse.Value.ToString())); //, dateTimeResponse.Method));
                            if (CallWasSuccessful(TL, dateTimeResponse)) return (T)((object)dateTimeResponse.Value);
                            restResponseBase = (RestResponseBase)dateTimeResponse;
                        }
                        if (typeof(T) == typeof(List<string>)) // Used for ArrayLists of string
                        {
                            StringListResponse stringListResponse = JsonConvert.DeserializeObject<StringListResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringListResponse.ClientTransactionID, stringListResponse.ServerTransactionID, stringListResponse.Value.Count.ToString())); //, stringListResponse.Method));
                            if (CallWasSuccessful(TL, stringListResponse)) return (T)((object)stringListResponse.Value);
                            restResponseBase = (RestResponseBase)stringListResponse;
                        }
                        if (typeof(T) == typeof(NoReturnValue)) // Used for Methods that have no response and Property Set members
                        {
                            MethodResponse deviceResponse = JsonConvert.DeserializeObject<MethodResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, deviceResponse.ClientTransactionID.ToString(), deviceResponse.ServerTransactionID.ToString(), "No response")); //, deviceResponse.Method));
                            if (CallWasSuccessful(TL, deviceResponse)) return (T)((object)new NoReturnValue());
                            restResponseBase = (RestResponseBase)deviceResponse;
                        }

                        // DEVICE SPECIFIC TYPES
                        if (typeof(T) == typeof(PierSide))
                        {
                            IntResponse pierSideResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, pierSideResponse.ClientTransactionID, pierSideResponse.ServerTransactionID, pierSideResponse.Value.ToString())); //, pierSideResponse.Method));
                            if (CallWasSuccessful(TL, pierSideResponse)) return (T)((object)pierSideResponse.Value);
                            restResponseBase = (RestResponseBase)pierSideResponse;
                        }
                        if (typeof(T) == typeof(ITrackingRates))
                        {
                            TrackingRatesResponse trackingRatesResponse = JsonConvert.DeserializeObject<TrackingRatesResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format("Trackingrates Count: {0} - Txn ID: {1}, Method: {1}", trackingRatesResponse.Value.Count.ToString(), trackingRatesResponse.ServerTransactionID.ToString())); //, trackingRatesResponse.Method));
                            List<DriveRates> rates = new List<DriveRates>();
                            DriveRates[] ratesArray = new DriveRates[trackingRatesResponse.Value.Count];
                            int i = 0;
                            foreach (DriveRates rate in trackingRatesResponse.Value)
                            {
                                TL.LogMessage(clientNumber, method, string.Format("Rate: {0}", rate.ToString()));
                                ratesArray[i] = rate;
                                i++;
                            }
                            TrackingRates trackingRates = new TrackingRates();
                            trackingRates.SetRates(ratesArray);
                            if (CallWasSuccessful(TL, trackingRatesResponse))
                            {
                                TL.LogMessage(clientNumber, method, string.Format("Returning {0} tracking rates to the client - now measured from trackingRates", trackingRates.Count));
                                return (T)((object)trackingRates);
                            }
                            TL.LogMessage(clientNumber, method, "trackingRatesResponse.DriverException is NOT NULL!");
                            restResponseBase = (RestResponseBase)trackingRatesResponse;
                        }
                        if (typeof(T) == typeof(EquatorialCoordinateType))
                        {
                            IntResponse equatorialCoordinateResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, equatorialCoordinateResponse.ClientTransactionID, equatorialCoordinateResponse.ServerTransactionID, equatorialCoordinateResponse.Value.ToString())); //, equatorialCoordinateResponse.Method));
                            if (CallWasSuccessful(TL, equatorialCoordinateResponse)) return (T)((object)equatorialCoordinateResponse.Value);
                            restResponseBase = (RestResponseBase)equatorialCoordinateResponse;
                        }
                        if (typeof(T) == typeof(AlignmentModes))
                        {
                            IntResponse alignmentModesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, alignmentModesResponse.ClientTransactionID, alignmentModesResponse.ServerTransactionID, alignmentModesResponse.Value.ToString())); //, alignmentModesResponse.Method));
                            if (CallWasSuccessful(TL, alignmentModesResponse)) return (T)((object)alignmentModesResponse.Value);
                            restResponseBase = (RestResponseBase)alignmentModesResponse;
                        }
                        if (typeof(T) == typeof(DriveRates))
                        {
                            IntResponse driveRatesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, driveRatesResponse.ClientTransactionID, driveRatesResponse.ServerTransactionID, driveRatesResponse.Value.ToString())); //, driveRatesResponse.Method));
                            if (CallWasSuccessful(TL, driveRatesResponse)) return (T)((object)driveRatesResponse.Value);
                            restResponseBase = (RestResponseBase)driveRatesResponse;
                        }
                        if (typeof(T) == typeof(SensorType))
                        {
                            IntResponse sensorTypeResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, sensorTypeResponse.ClientTransactionID, sensorTypeResponse.ServerTransactionID, sensorTypeResponse.Value.ToString())); //, sensorTypeResponse.Method));
                            if (CallWasSuccessful(TL, sensorTypeResponse)) return (T)((object)sensorTypeResponse.Value);
                            restResponseBase = (RestResponseBase)sensorTypeResponse;
                        }
                        if (typeof(T) == typeof(CameraStates))
                        {
                            IntResponse cameraStatesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, cameraStatesResponse.ClientTransactionID, cameraStatesResponse.ServerTransactionID, cameraStatesResponse.Value.ToString())); //, cameraStatesResponse.Method));
                            if (CallWasSuccessful(TL, cameraStatesResponse)) return (T)((object)cameraStatesResponse.Value);
                            restResponseBase = (RestResponseBase)cameraStatesResponse;
                        }
                        if (typeof(T) == typeof(ShutterState))
                        {
                            IntResponse domeShutterStateResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, domeShutterStateResponse.ClientTransactionID, domeShutterStateResponse.ServerTransactionID, domeShutterStateResponse.Value.ToString())); //, domeShutterStateResponse.Method));
                            if (CallWasSuccessful(TL, domeShutterStateResponse)) return (T)((object)domeShutterStateResponse.Value);
                            restResponseBase = (RestResponseBase)domeShutterStateResponse;
                        }
                        if (typeof(T) == typeof(IAxisRates))
                        {
                            AxisRatesResponse axisRatesResponse = JsonConvert.DeserializeObject<AxisRatesResponse>(deviceJsonResponse.Content);
                            AxisRates axisRates = new AxisRates((TelescopeAxes)(Convert.ToInt32(Parameters[SharedConstants.AXIS_PARAMETER_NAME])));
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, axisRatesResponse.ClientTransactionID.ToString(), axisRatesResponse.ServerTransactionID.ToString(), axisRatesResponse.Value.Count.ToString())); //, axisRatesResponse.Method));
                            foreach (RateResponse rr in axisRatesResponse.Value)
                            {
                                axisRates.Add(rr.Minimum, rr.Maximum, TL);
                                TL.LogMessage(clientNumber, method, string.Format("Found rate: {0} - {1}", rr.Minimum, rr.Maximum));
                            }

                            if (CallWasSuccessful(TL, axisRatesResponse)) return (T)((object)axisRates);
                            restResponseBase = (RestResponseBase)axisRatesResponse;
                        }
                        if (typeof(T) == typeof(Array)) // Used for Camera.ImageArray and Camera.ImageArrayVariant
                        {
                            // Include some debug logging
                            if (TL.DebugTraceState)
                            {
                                foreach (var header in deviceJsonResponse.Headers)
                                {
                                    TL.LogMessage(clientNumber, method, $"Response header {header.Name} = {header.Value}");
                                }
                            }

                            // Handle base64 hand-off image transfer mechanic
                            if (deviceJsonResponse.Headers.Any(t => t.Name.ToString() == SharedConstants.BASE64_HANDOFF_HEADER)) // Base64 format header is present so the server supports base64 serialised transfer
                            {
                                // De-serialise the JSON image array hand-off response 
                                sw.Restart(); // Clear and start the stopwatch
                                Base64ArrayHandOffResponse base64HandOffresponse = JsonConvert.DeserializeObject<Base64ArrayHandOffResponse>(deviceJsonResponse.Content);
                                SharedConstants.ImageArrayElementTypes arrayType = (SharedConstants.ImageArrayElementTypes)base64HandOffresponse.Type; // Extract the array type from the JSON response

                                TL.LogMessage(clientNumber, method, $"Base64 - Extracted array information in {sw.ElapsedMilliseconds}ms. Array Type: {arrayType}, Rank: {base64HandOffresponse.Rank}, Dimension 0 length: {base64HandOffresponse.Dimension0Length}, Dimension 1 length: {base64HandOffresponse.Dimension1Length}, Dimension 2 length: {base64HandOffresponse.Dimension2Length}");
                                sw.Restart();

                                TL.LogMessage(clientNumber, method, $"Base64 - Downloading base64 serialised image");

                                // Construct an HTTP request to get the logo
                                string base64Uri = (client.BaseUrl + URIBase.TrimStart('/') + method.ToLowerInvariant() + SharedConstants.BASE64_HANDOFF_FILE_DOWNLOAD_URI_EXTENSION).ToLowerInvariant(); // Create the download URI from the REST client elements
                                if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, $"Base64 URI: {base64Uri}");

                                // Create a handler that indicates the compression levels supported by this client
                                HttpClientHandler imageDownloadHandler = new HttpClientHandler();
                                switch (imageArrayCompression)
                                {
                                    case SharedConstants.ImageArrayCompression.None:
                                        imageDownloadHandler.AutomaticDecompression = DecompressionMethods.None;
                                        break;
                                    case SharedConstants.ImageArrayCompression.Deflate:
                                        imageDownloadHandler.AutomaticDecompression = DecompressionMethods.Deflate;
                                        break;
                                    case SharedConstants.ImageArrayCompression.GZip:
                                        imageDownloadHandler.AutomaticDecompression = DecompressionMethods.GZip;
                                        break;
                                    case SharedConstants.ImageArrayCompression.GZipOrDeflate:
                                        imageDownloadHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip; // Allow both Deflate and GZip decompression
                                        break;
                                    default:
                                        throw new InvalidValueException($"Unknown ImageArrayCompression value: {imageArrayCompression} - Can't proceed further!");
                                }

                                using (HttpClient httpClient = new HttpClient(imageDownloadHandler))
                                {
                                    string base64ArrayString = "";

                                    // Get an async stream
                                    Stream base64ArrayStream = httpClient.GetStreamAsync(base64Uri).Result;
                                    TL.LogMessage(clientNumber, method, $"Downloaded base64 stream obtained in {sw.ElapsedMilliseconds}ms"); sw.Restart();

                                    // Read the stream contents into a string variable
                                    using (StreamReader sr = new StreamReader(base64ArrayStream, System.Text.Encoding.ASCII, false))
                                    {
                                        base64ArrayString = sr.ReadToEnd();
                                    }

                                    TL.LogMessage(clientNumber, method, $"Read base64 string from stream ({base64ArrayString.Length} bytes) in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    try { TL.LogMessage(clientNumber, method, $"Base64 string start: {base64ArrayString.Substring(0, 300)}"); } catch { }
                                    try { TL.LogMessage(clientNumber, method, $"Base64 string end: {base64ArrayString.Substring(60000000, 300)}"); } catch { }

                                    // Convert the array from base64 encoding to a byte array
                                    byte[] base64ArrayByteArray = Convert.FromBase64String(base64ArrayString);
                                    TL.LogMessage(clientNumber, method, $"Converted base64 string of length {base64ArrayString.Length} to byte array of length {base64ArrayByteArray.Length} in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    string byteLine = "";
                                    try
                                    {
                                        for (int i = 0; i < 300; i++)
                                        {
                                            byteLine += base64ArrayByteArray[i].ToString() + " ";
                                        }
                                        TL.LogMessage(clientNumber, method, $"Converted base64 bytes: {byteLine}");
                                    }
                                    catch { }

                                    // Now create and populate an appropriate array to return to the client that mirrors the array type returned by the device
                                    switch (arrayType) // Handle the different array return types
                                    {
                                        case SharedConstants.ImageArrayElementTypes.Int:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new int[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new int[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length); // Copy the array bytes to the response array that will return to the client
                                            break;

                                        case SharedConstants.ImageArrayElementTypes.Short:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new short[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new short[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length); // Copy the array bytes to the response array that will return to the client
                                            break;

                                        case SharedConstants.ImageArrayElementTypes.Double:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new double[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new double[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length); // Copy the array bytes to the response array that will return to the client
                                            break;

                                        default:
                                            throw new InvalidOperationException("Image array element type" + arrayType + " is not supported.");
                                    }

                                    if (TL.DebugTraceState)
                                    {
                                        TL.LogMessage(clientNumber, method, $"Created and copied the array in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    }

                                    return (T)(object)remoteArray;
                                }
                            }

                            // Handle conventional JSON response with integer array elements individually serialised
                            else
                            {
                                sw.Restart(); // Clear and start the stopwatch
                                ImageArrayResponseBase responseBase = JsonConvert.DeserializeObject<ImageArrayResponseBase>(deviceJsonResponse.Content);
                                SharedConstants.ImageArrayElementTypes arrayType = (SharedConstants.ImageArrayElementTypes)responseBase.Type;
                                int arrayRank = responseBase.Rank;

                                // Include some debug logging
                                if (TL.DebugTraceState)
                                {
                                    TL.LogMessage(clientNumber, method, $"Extracted array type and rank by JsonConvert.DeserializeObject in {sw.ElapsedMilliseconds}ms. Type: {arrayType}, Rank: {arrayRank}, Response values - Type: {responseBase.Type}, Rank: {responseBase.Rank}");
                                }

                                sw.Restart(); // Clear and start the stopwatch
                                switch (arrayType) // Handle the different return types that may come from ImageArrayVariant
                                {
                                    case SharedConstants.ImageArrayElementTypes.Int:
                                        switch (arrayRank)
                                        {
                                            case 2:
                                                IntArray2DResponse intArray2DResponse = JsonConvert.DeserializeObject<IntArray2DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArray2DResponse.ClientTransactionID, intArray2DResponse.ServerTransactionID, intArray2DResponse.Rank.ToString())); //, intArray2DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)intArray2DResponse.Type).ToString()}, Rank: {intArray2DResponse.Rank}");
                                                if (CallWasSuccessful(TL, intArray2DResponse)) return (T)((object)intArray2DResponse.Value);
                                                restResponseBase = (RestResponseBase)intArray2DResponse;
                                                break;

                                            case 3:
                                                IntArray3DResponse intArray3DResponse = JsonConvert.DeserializeObject<IntArray3DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArray3DResponse.ClientTransactionID, intArray3DResponse.ServerTransactionID, intArray3DResponse.Rank.ToString())); //, intArray3DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)intArray3DResponse.Type).ToString()}, Rank: {intArray3DResponse.Rank}");
                                                if (CallWasSuccessful(TL, intArray3DResponse)) return (T)((object)intArray3DResponse.Value);
                                                restResponseBase = (RestResponseBase)intArray3DResponse;
                                                break;

                                            default:
                                                throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                        }
                                        break;

                                    case SharedConstants.ImageArrayElementTypes.Short:
                                        switch (arrayRank)
                                        {
                                            case 2:
                                                ShortArray2DResponse shortArray2DResponse = JsonConvert.DeserializeObject<ShortArray2DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortArray2DResponse.ClientTransactionID, shortArray2DResponse.ServerTransactionID, shortArray2DResponse.Rank.ToString())); //, shortArray2DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)shortArray2DResponse.Type).ToString()}, Rank: {shortArray2DResponse.Rank}");
                                                if (CallWasSuccessful(TL, shortArray2DResponse)) return (T)((object)shortArray2DResponse.Value);
                                                restResponseBase = (RestResponseBase)shortArray2DResponse;
                                                break;

                                            case 3:
                                                ShortArray3DResponse shortArray3DResponse = JsonConvert.DeserializeObject<ShortArray3DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortArray3DResponse.ClientTransactionID, shortArray3DResponse.ServerTransactionID, shortArray3DResponse.Rank.ToString())); //, shortArray3DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)shortArray3DResponse.Type).ToString()}, Rank: {shortArray3DResponse.Rank}");
                                                if (CallWasSuccessful(TL, shortArray3DResponse)) return (T)((object)shortArray3DResponse.Value);
                                                restResponseBase = (RestResponseBase)shortArray3DResponse;
                                                break;

                                            default:
                                                throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                        }
                                        break;

                                    case SharedConstants.ImageArrayElementTypes.Double:
                                        switch (arrayRank)
                                        {
                                            case 2:
                                                DoubleArray2DResponse doubleArray2DResponse = JsonConvert.DeserializeObject<DoubleArray2DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleArray2DResponse.ClientTransactionID, doubleArray2DResponse.ServerTransactionID, doubleArray2DResponse.Rank.ToString())); //, doubleArray2DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)doubleArray2DResponse.Type).ToString()}, Rank: {doubleArray2DResponse.Rank}");
                                                if (CallWasSuccessful(TL, doubleArray2DResponse)) return (T)((object)doubleArray2DResponse.Value);
                                                restResponseBase = (RestResponseBase)doubleArray2DResponse;
                                                break;

                                            case 3:
                                                DoubleArray3DResponse doubleArray3DResponse = JsonConvert.DeserializeObject<DoubleArray3DResponse>(deviceJsonResponse.Content);
                                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleArray3DResponse.ClientTransactionID, doubleArray3DResponse.ServerTransactionID, doubleArray3DResponse.Rank.ToString())); //, doubleArray3DResponse.Method));
                                                TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {((SharedConstants.ImageArrayElementTypes)doubleArray3DResponse.Type).ToString()}, Rank: {doubleArray3DResponse.Rank}");
                                                if (CallWasSuccessful(TL, doubleArray3DResponse)) return (T)((object)doubleArray3DResponse.Value);
                                                restResponseBase = (RestResponseBase)doubleArray3DResponse;
                                                break;

                                            default:
                                                throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                        }
                                        break;

                                    default:
                                        throw new InvalidOperationException("Image array element type" + arrayType + " is not supported.");
                                }
                            } // Remote server has used JSON encoding
                        }

                        // HANDLE COM EXCEPTIONS THROWN BY WINDOWS BASED DRIVERS RUNNING IN THE REMOTE SERVER
                        if (restResponseBase.DriverException != null)
                        {
                            TL.LogMessageCrLf(clientNumber, method, string.Format("Exception Message: \"{0}\", Exception Number: 0x{1}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber.ToString("X8")));
                            throw restResponseBase.DriverException;
                        }

                        // HANDLE ERRORS REPORTED BY ALPACA DEVICES THAT USE THE ERROR NUMBER AND ERROR MESSAGE FIELDS
                        if ((restResponseBase.ErrorMessage != "") || (restResponseBase.ErrorNumber != 0))
                        {
                            TL.LogMessageCrLf(clientNumber, method, string.Format("Received an Alpaca error - ErrorMessage: \"{0}\", ErrorNumber: 0x{1}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber.ToString("X8")));

                            // Handle ASCOM Alpaca reserved error numbers between 0x400 and 0xFFF by translating these to the COM HResult error number range: 0x80040400 to 0x80040FFF and throwing the translated value as an exception
                            if ((restResponseBase.ErrorNumber >= SharedConstants.ALPACA_ERROR_CODE_BASE) & (restResponseBase.ErrorNumber <= SharedConstants.ALPACA_ERROR_CODE_MAX)) // This error is within the ASCOM Alpaca reserved error number range
                            {
                                // Calculate the equivalent COM HResult error number from the supplied Alpaca error number so that comparison can be made with the original ASCOM COM exception HResult numbers that Windows clients expect in their exceptions
                                int ascomCOMErrorNumber = restResponseBase.ErrorNumber + SharedConstants.ASCOM_ERROR_NUMBER_OFFSET;
                                TL.LogMessageCrLf(clientNumber, method, string.Format("Received Alpaca error code: {0} (0x{0:X4}), the equivalent COM error HResult error code is {1} (0x{1:X8})", restResponseBase.ErrorNumber, ascomCOMErrorNumber));

                                // Now check whether the COM HResult matches any of the built-in ASCOM exception types. If so, we throw that exception type otherwise we throw a generic DriverException
                                if (ascomCOMErrorNumber == ErrorCodes.ActionNotImplementedException) // Handle ActionNotImplementedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca action not implemented error, throwing ActionNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ActionNotImplementedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidOperationException) // Handle InvalidOperationException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid operation error, throwing InvalidOperationException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new InvalidOperationException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidValue) // Handle InvalidValueException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid value error, throwing InvalidValueException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new InvalidValueException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidWhileParked) // Handle ParkedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid while parked error, throwing ParkedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ParkedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidWhileSlaved) // Handle SlavedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca invalid while slaved error, throwing SlavedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new SlavedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.NotConnected) // Handle NotConnectedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca not connected error, throwing NotConnectedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new NotConnectedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.NotImplemented) // Handle PropertyNotImplementedException and MethodNotImplementedException (both have the same error code)
                                {
                                    // Need to determine whether a property not implemented or a method not implemented exception is intended by examining the message text - ugh - but I can't see another way!
                                    if (restResponseBase.ErrorMessage.ToLowerInvariant().Contains("property"))  // Found the string "property" so assume a PropertyNotImplementedException is appropriate...
                                    {
                                        TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca property not implemented error, throwing PropertyNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                        throw new PropertyNotImplementedException(restResponseBase.ErrorMessage);
                                    }
                                    else // Otherwise assume that a MethodNotImplementedException is appropriate...
                                    {
                                        TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca method not implemented error, throwing MethodNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                        throw new MethodNotImplementedException(restResponseBase.ErrorMessage);
                                    }
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.ValueNotSet) // Handle ValueNotSetException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca value not set error, throwing ValueNotSetException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ValueNotSetException(restResponseBase.ErrorMessage);
                                }
                                else // The exception is inside the ASCOM Alpaca reserved range but is not one of those with their own specific exception types above, so wrap it in a DriverException and throw this to the client
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca undefined ASCOM error, throwing DriverException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new DriverException(restResponseBase.ErrorMessage, ascomCOMErrorNumber);
                                }
                            }
                            else // An exception has been thrown with an error number outside the ASCOM Alpaca reserved range, so wrap it in a DriverException and throw this to the client.
                            {
                                TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca error outside ASCOM reserved range, throwing DriverException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber));
                                throw new DriverException(restResponseBase.ErrorMessage, restResponseBase.ErrorNumber);
                            }
                        }

                        // Internal error if an unsupported type is requested - should only occur during development and not in production operation!
                        throw new InvalidOperationException("Type " + typeof(T).ToString() + " is not supported. You should never see this message, if you do, please report it on the ASCOM Talk Forum!");
                    }
                    else
                    {
                        if (deviceJsonResponse.ErrorException != null)
                        {
                            TL.LogMessageCrLf(clientNumber, method, "RestClient exception: " + deviceJsonResponse.ErrorMessage + "\r\n " + deviceJsonResponse.ErrorException.ToString());
                            throw new DriverException(string.Format("Communications exception: {0} - {1}", deviceJsonResponse.ErrorMessage, deviceJsonResponse.ResponseStatus), deviceJsonResponse.ErrorException);
                        }
                        else
                        {
                            TL.LogMessage(clientNumber, method + " Error", string.Format("RestRequest response status: {0}, HTTP response code: {1}, HTTP response description: {2}", deviceJsonResponse.ResponseStatus.ToString(), deviceJsonResponse.StatusCode, deviceJsonResponse.StatusDescription));
                            throw new DriverException(string.Format("Error calling method: {0}, HTTP Completion Status: {1}, Error Message:\r\n{2}", method, deviceJsonResponse.ResponseStatus, deviceJsonResponse.Content));
                        }
                    }
                }
                catch (Exception ex) // Process unexpected exceptions
                {
                    if (ex is System.Net.WebException) // Received a WebException, this could indicate that the remote server actively refused the connection so test for this and retry if appropriate
                    {
                        if (ex.InnerException != null) // First make sure the is an inner exception
                        {
                            if (ex.InnerException is System.Net.Sockets.SocketException) // There is an inner exception and it is a SocketException so apply the retry logic
                            {
                                retryCounter += 1; // Increment the retry counter
                                if (retryCounter <= SharedConstants.SOCKET_ERROR_MAXIMUM_RETRIES) // The retry count is less than or equal to the maximum allowed so retry the command
                                {
                                    TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                    if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "SocketException: " + ex.ToString());

                                    // Log that we are retrying the command and wait a short time in the hope that the transient condition clears
                                    TL.LogMessage(clientNumber, method, string.Format("Socket exception, retrying command - retry-count {0}/{1}", retryCounter, SharedConstants.SOCKET_ERROR_MAXIMUM_RETRIES));
                                    Thread.Sleep(SharedConstants.SOCKET_ERROR_RETRY_DELAY_TIME);
                                }
                                else // The retry count exceeds the maximum allowed so throw the exception to the client
                                {
                                    TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                    if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "SocketException: " + ex.ToString());
                                    throw;
                                }

                            }
                            else  // There is an inner exception but it is not a SocketException so log it and throw it  to the client
                            {
                                TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "WebException: " + ex.ToString());
                                throw;
                            }

                        }
                    }
                    else // Some other type of exception that isn't System.Net.WebException so log it and throw it to the client
                    {
                        TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                        if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "Exception: " + ex.ToString());
                        throw;
                    }
                }
            } while (true); // Execution will only reach here if a communications retry is required, all other conditions are handled by return statements or by throwing exceptions

            // Execution will never reach this point
        }

        /// <summary>
        /// Test whether an error occurred in the driver
        /// </summary>
        /// <param name="response">The driver's response </param>
        /// <returns>True if the call was successful otherwise returns false.</returns>
        private static bool CallWasSuccessful(TraceLogger TL, RestResponseBase response)
        {
            TL.LogMessage("CallWasSuccessful", string.Format("DriverException == null: {0}, ErrorMessage: {1}, ErrorNumber: 0x{2}", response.DriverException == null, response.ErrorMessage, response.ErrorNumber.ToString("X8")));
            if (response.DriverException != null) TL.LogMessage("CallWasSuccessfulEx", response.DriverException.ToString());
            if ((response.DriverException == null) & (response.ErrorMessage == "") & (response.ErrorNumber == 0))
            {
                TL.LogMessage("CallWasSuccessful", "Returning True");
                TL.BlankLine();
                return true; // All was OK so return true
            }
            else
            {
                TL.LogMessage("CallWasSuccessful", "Returning False");
                TL.BlankLine();
                return false; // Some sort of issue so return false
            }
        }



        #endregion

        #region ASCOM Common members

        public static string Action(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string actionName, string actionParameters)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.ACTION_COMMAND_PARAMETER_NAME, actionName },
                { SharedConstants.ACTION_PARAMETERS_PARAMETER_NAME, actionParameters }
            };
            string remoteString = SendToRemoteDriver<string>(clientNumber, client, URIBase, TL, "Action", Parameters, Method.PUT);

            TL.LogMessage(clientNumber, "Action", "Response: " + remoteString);
            return remoteString;
        }

        public static void CommandBlind(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.COMMAND_PARAMETER_NAME, command },
                { SharedConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            SendToRemoteDriver<NoReturnValue>(clientNumber, client, URIBase, TL, "CommandBlind", Parameters, Method.PUT);
            TL.LogMessage(clientNumber, "CommandBlind", "Completed OK");
        }

        public static bool CommandBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.COMMAND_PARAMETER_NAME, command },
                { SharedConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            bool remoteBool = SendToRemoteDriver<bool>(clientNumber, client, URIBase, TL, "CommandBool", Parameters, Method.PUT);

            TL.LogMessage(clientNumber, "CommandBool", remoteBool.ToString());
            return remoteBool;
        }

        public static string CommandString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { SharedConstants.COMMAND_PARAMETER_NAME, command },
                { SharedConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            string remoteString = SendToRemoteDriver<string>(clientNumber, client, URIBase, TL, "CommandString", Parameters, Method.PUT);

            TL.LogMessage(clientNumber, "CommandString", remoteString);
            return remoteString;
        }

        public static void Connect(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            if (TL.DebugTraceState) TL.LogMessage(clientNumber, "Connect", "Acquiring connection lock");
            lock (connectLockObject) // Ensure that only one connection attempt can happen at a time
            {
                TL.LogMessage(clientNumber, "Connect", "Has connection lock");
                if (IsClientConnected(clientNumber, TL)) // If we are already connected then just log this 
                {
                    TL.LogMessage(clientNumber, "Connect", "Already connected, just incrementing connection count.");
                }
                else // We are not connected so connect now
                {
                    try
                    {
                        TL.LogMessage(clientNumber, "Connect", "This is the first connection so set Connected to True");
                        SetBool(clientNumber, client, URIBase, TL, "Connected", true);
                        bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true);
                        TL.LogMessage(clientNumber, "Connect", "Successfully connected, AlreadyConnected: " + (!notAlreadyPresent).ToString() + ", number of connections: " + connectStates.Count);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf(clientNumber, "Connect", "Exception: " + ex.ToString());
                        throw;
                    }
                }
            }
        }

        public static string Description(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            return GetValue<string>(clientNumber, client, URIBase, TL, "Description");
        }

        public static void Disconnect(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {

            if (IsClientConnected(clientNumber, TL)) // If we are already connected then disconnect, otherwise ignore disconnect 
            {
                TL.LogMessage(clientNumber, "Disconnect", "We are connected, setting Connected to False on remote driver");
                SetBool(clientNumber, client, URIBase, TL, "Connected", false);
                bool successfullyRemoved = connectStates.TryRemove(clientNumber, out bool lastValue);
                TL.LogMessage("Disconnect", $"Set Connected to: False, Successfully removed: {successfullyRemoved}, previous value: {lastValue}");
            }
            else
            {
                TL.LogMessage("Disconnect", "Already disconnected, not sending command to driver");
            }
        }

        public static string DriverInfo(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            return GetValue<string>(clientNumber, client, URIBase, TL, "DriverInfo");
        }

        public static string DriverVersion(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            string remoteString = GetValue<string>(clientNumber, client, URIBase, TL, "DriverVersion");
            TL.LogMessage(clientNumber, "DriverVersion", remoteString);
            return remoteString;
        }

        public static short InterfaceVersion(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            short interfaceVersion = GetValue<short>(clientNumber, client, URIBase, TL, "InterfaceVersion");
            TL.LogMessage(clientNumber, "InterfaceVersion", interfaceVersion.ToString());
            return interfaceVersion;
        }

        public static ArrayList SupportedActions(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            List<string> supportedActions = GetValue<List<string>>(clientNumber, client, URIBase, TL, "SupportedActions");
            TL.LogMessage(clientNumber, "SupportedActions", string.Format("Returning {0} actions", supportedActions.Count));

            ArrayList returnValues = new ArrayList();
            foreach (string action in supportedActions)
            {
                returnValues.Add(action);
                TL.LogMessage(clientNumber, "SupportedActions", string.Format("Returning action: {0}", action));
            }

            return returnValues;
        }

        #endregion

        #region Complex Camera Properties

        public static int GetIntParameter(string parameterName, string response, TraceLoggerPlus TL)
        {
            const string COMMA = ",";

            string formattedParameterName = $"\"{parameterName}\":";

            int parameterTextStart = response.IndexOf(formattedParameterName, StringComparison.Ordinal);
            int parameterStart = parameterTextStart + formattedParameterName.Length;
            int parameterEnd = response.IndexOf(COMMA, parameterStart, StringComparison.Ordinal);
            string parameterString = response.Substring(parameterStart, parameterEnd - parameterStart);

            bool success = int.TryParse(parameterString, out int parameterValue);

            TL.LogMessage("GetIntParameter", $"ParameterTextStart: {parameterTextStart}, ParameterStart: {parameterStart}, PArameterEnd: {parameterEnd}, ParameterString: {parameterString}, ParameterValue: {parameterValue}, Success: {success}");

            return parameterValue;
        }

        public static object ImageArrayVariant(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, SharedConstants.ImageArrayTransferType imageArrayTransferType, SharedConstants.ImageArrayCompression imageArrayCompression)
        {
            Array returnArray;
            object[,] objectArray2D;
            object[,,] objectArray3D;

            returnArray = GetValue<Array>(clientNumber, client, URIBase, TL, "ImageArrayVariant", imageArrayTransferType, imageArrayCompression);

            string variantType = returnArray.GetType().Name;
            string elementType;
            switch (returnArray.Rank) // Process 2D and 3D variant arrays, all other types are unsupported
            {
                case 2: // 2D Array
                    elementType = returnArray.GetValue(0, 0).GetType().Name;
                    break;
                case 3: // 3D array
                    elementType = returnArray.GetValue(0, 0, 0).GetType().Name;
                    break;
                default:
                    throw new InvalidValueException("ReturnImageArray: Received an unsupported return array rank: " + returnArray.Rank);
            }

            TL.LogMessage(clientNumber, "ImageArrayVariant", $"Received {variantType} array of Rank {returnArray.Rank} with Length {returnArray.Length} and element type {elementType}");

            // convert to variant

            switch (returnArray.Rank)
            {
                case 2:
                    switch (variantType)
                    {
                        case "Int16[,]":
                            objectArray2D = new object[returnArray.GetLength(0), returnArray.GetLength(1)];
                            for (int i = 0; i < returnArray.GetLength(1); i++)
                            {
                                for (int j = 0; j < returnArray.GetLength(0); j++)
                                {
                                    objectArray2D[j, i] = ((short[,])returnArray)[j, i];
                                }
                            }
                            return objectArray2D;
                        case "Int32[,]":
                            objectArray2D = new object[returnArray.GetLength(0), returnArray.GetLength(1)];

                            for (int i = 0; i < returnArray.GetLength(1); i++)
                            {
                                for (int j = 0; j < returnArray.GetLength(0); j++)
                                {
                                    objectArray2D[j, i] = ((int[,])returnArray)[j, i];
                                }
                            }
                            return objectArray2D;
                        case "Double[,]":
                            objectArray2D = new object[returnArray.GetLength(0), returnArray.GetLength(1)];
                            for (int i = 0; i < returnArray.GetLength(1); i++)
                            {
                                for (int j = 0; j < returnArray.GetLength(0); j++)
                                {
                                    objectArray2D[j, i] = ((double[,])returnArray)[j, i];
                                }
                            }
                            return objectArray2D;

                        case "Object[,]":
                            TL.LogMessage(clientNumber, "ImageArrayVariant", $"Returning Object[,] array to client");
                            return returnArray;

                        default:
                            throw new InvalidValueException("Remote Driver Camera.ImageArrayVariant: Unsupported return array rank from RemoteClientDriver.GetValue<Array>: " + returnArray.Rank);
                    }
                case 3:
                    switch (variantType)
                    {
                        case "Int16[,,]":
                            objectArray3D = new object[returnArray.GetLength(0), returnArray.GetLength(1), 3];
                            for (int i = 0; i < returnArray.GetLength(1); i++)
                            {
                                for (int j = 0; j < returnArray.GetLength(0); j++)
                                {
                                    for (int k = 0; k < 3; k++)
                                        objectArray3D[j, i, k] = ((short[,,])returnArray)[j, i, k];
                                }
                            }
                            return objectArray3D;

                        case "Int32[,,]":
                            objectArray3D = new object[returnArray.GetLength(0), returnArray.GetLength(1), returnArray.GetLength(2)];
                            for (int k = 0; k < returnArray.GetLength(2); k++)
                            {
                                for (int j = 0; j < returnArray.GetLength(1); j++)
                                {
                                    for (int i = 0; i < returnArray.GetLength(0); i++)
                                    {
                                        objectArray3D[i, j, k] = ((int[,,])returnArray)[i, j, k];
                                    }
                                }
                            }
                            return objectArray3D;

                        case "Double[,,]":
                            objectArray3D = new object[returnArray.GetLength(0), returnArray.GetLength(1), 3];
                            for (int i = 0; i < returnArray.GetLength(1); i++)
                            {
                                for (int j = 0; j < returnArray.GetLength(0); j++)
                                {
                                    for (int k = 0; k < 3; k++)
                                        objectArray3D[j, i, k] = ((double[,,])returnArray)[j, i, k];
                                }
                            }
                            return objectArray3D;

                        case "Object[,,]":
                            TL.LogMessage(clientNumber, "ImageArrayVariant", $"Returning Object[,,] array to client");
                            return returnArray;

                        default:
                            throw new InvalidValueException("Remote Driver Camera.ImageArrayVariant: Unsupported return array rank from RemoteClientDriver.GetValue<Array>: " + returnArray.Rank);
                    }

                default:
                    throw new InvalidValueException("Remote Driver Camera.ImageArrayVariant: Unsupported return array rank from RemoteClientDriver.GetValue<Array>: " + returnArray.Rank);
            }
        }

        #endregion

    }
}
