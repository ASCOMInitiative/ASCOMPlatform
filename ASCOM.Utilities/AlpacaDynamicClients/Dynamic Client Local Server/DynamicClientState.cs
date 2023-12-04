using ASCOM.Com;
using ASCOM.Common;
using ASCOM.Common.Alpaca;
using System;

namespace ASCOM.DynamicClients
{
    /// <summary>
    /// Class holding the state of this driver
    /// </summary>
    public class DynamicClientState
    {
        /// <summary>
        /// Initialise this instance using values from the given ProgId
        /// </summary>
        /// <param name="ProgId">The device's ProgId</param>
        /// <param name="DeviceType">The ASCOM device type</param>
        public DynamicClientState(string progId, DeviceTypes deviceType, string driverDisplayName)
        {
            // Save the provided ProgId and device type for use when saving values to the Profile
            ProgId = progId;
            DeviceType = deviceType;

            // Get current state values from the Profile using default values if required
            TraceState = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.TRACE_LEVEL_PROFILENAME, TraceState.ToString()));
            DebugTraceState = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.DEBUG_TRACE_PROFILENAME, DebugTraceState.ToString()));
            IpAddressString = Profile.GetValue(DeviceType, ProgId, SharedConstants.IPADDRESS_PROFILENAME, IpAddressString);
            PortNumber = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.PORTNUMBER_PROFILENAME, PortNumber.ToString()));
            RemoteDeviceNumber = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, RemoteDeviceNumber.ToString()));
            ServiceType = Profile.GetValue(DeviceType, ProgId, SharedConstants.SERVICE_TYPE_PROFILENAME, ServiceType.ToString()).ToLowerInvariant() == "https" ? ServiceType.Https : ServiceType.Http;
            EstablishConnectionTimeout = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, EstablishConnectionTimeout.ToString()));
            StandardDeviceResponseTimeout = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, StandardDeviceResponseTimeout.ToString()));
            LongDeviceResponseTimeout = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, LongDeviceResponseTimeout.ToString()));
            UserName = Profile.GetValue(DeviceType, ProgId, SharedConstants.USERNAME_PROFILENAME, UserName);
            Password = Profile.GetValue(DeviceType, ProgId, SharedConstants.PASSWORD_PROFILENAME, Password);
            ManageConnectLocally = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, ManageConnectLocally.ToString()));
            ImageArrayTransferType = (ImageArrayTransferType)Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, ((int)ImageArrayTransferType).ToString()));
            ImageArrayCompression = (ImageArrayCompression)Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, ((int)ImageArrayCompression).ToString()));
            UniqueId = Profile.GetValue(DeviceType, ProgId, SharedConstants.UNIQUEID_PROFILENAME, UniqueId.ToString());
            EnableRediscovery = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.ENABLE_REDISCOVERY_PROFILENAME, EnableRediscovery.ToString()));
            IpV4Enabled = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.ENABLE_IPV4_DISCOVERY_PROFILENAME, IpV4Enabled.ToString()));
            IpV6Enabled = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.ENABLE_IPV6_DISCOVERY_PROFILENAME, IpV6Enabled.ToString()));
            DiscoveryPort = Convert.ToInt32(Profile.GetValue(DeviceType, ProgId, SharedConstants.DISCOVERY_PORT_PROFILENAME, DiscoveryPort.ToString()));
            TrustUserGeneratedSslCertificates = Convert.ToBoolean(Profile.GetValue(DeviceType, ProgId, SharedConstants.TRUST_UNSIGNED_SSL_CERTIFICATES_PROFILENAME, TrustUserGeneratedSslCertificates.ToString()));
            DriverDisplayName = driverDisplayName;
        }

        // Properties representing values that can be configured by the user through the setup form
        public bool TraceState { get; set; } = SharedConstants.CLIENT_TRACE_LEVEL_DEFAULT;
        public bool DebugTraceState { get; set; } = SharedConstants.DEBUG_TRACE_DEFAULT;
        public string IpAddressString { get; set; } = SharedConstants.IPADDRESS_DEFAULT;
        public int PortNumber { get; set; } = SharedConstants.PORTNUMBER_DEFAULT;
        public int RemoteDeviceNumber { get; set; } = SharedConstants.REMOTE_DEVICE_NUMBER_DEFAULT;
        public ServiceType ServiceType { get; set; } = SharedConstants.SERVICE_TYPE_DEFAULT;
        public int EstablishConnectionTimeout { get; set; } = SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_DEFAULT;
        public int StandardDeviceResponseTimeout { get; set; } = SharedConstants.STANDARD_SERVER_RESPONSE_TIMEOUT_DEFAULT;
        public int LongDeviceResponseTimeout { get; set; } = SharedConstants.LONG_SERVER_RESPONSE_TIMEOUT_DEFAULT;
        public string UserName { get; set; } = SharedConstants.USERNAME_DEFAULT;
        public string Password { get; set; } = SharedConstants.PASSWORD_DEFAULT;
        public bool ManageConnectLocally { get; set; } = SharedConstants.MANAGE_CONNECT_LOCALLY_DEFAULT;
        public ImageArrayTransferType ImageArrayTransferType { get; set; } = SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT;
        public ImageArrayCompression ImageArrayCompression { get; set; } = SharedConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT;
        public string UniqueId { get; set; } = SharedConstants.UNIQUEID_DEFAULT;
        public bool EnableRediscovery { get; set; } = SharedConstants.ENABLE_REDISCOVERY_DEFAULT;
        public bool IpV4Enabled { get; set; } = SharedConstants.ENABLE_IPV4_DISCOVERY_DEFAULT;
        public bool IpV6Enabled { get; set; } = SharedConstants.ENABLE_IPV6_DISCOVERY_DEFAULT;
        public int DiscoveryPort { get; set; } = SharedConstants.DISCOVERY_PORT_DEFAULT;
        public bool TrustUserGeneratedSslCertificates { get; set; } = SharedConstants.TRUST_UNSIGNED_CERTIFICATES_DEFAULT;

        // Additional Properties
        public DeviceTypes DeviceType { get; set; } = DeviceTypes.Video; // Deliberately initialised to a value that is invalid for Alpaca to detect failure to initialise later in the client!

        public uint ClientId { get; set; } = (uint)new Random().Next(1,65536);  // Creates a random client ID in the range 1::65535

        public string ProgId { get; set; }

        public string DriverDisplayName { get; set; } = "Default display name - this should never be visible!";

        /// <summary>
        /// Persist state to the device's profile
        /// </summary>
        public void PersistState()
        {
            Profile.SetValue(DeviceType, ProgId, SharedConstants.TRACE_LEVEL_PROFILENAME, TraceState.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.DEBUG_TRACE_PROFILENAME, DebugTraceState.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.IPADDRESS_PROFILENAME, IpAddressString);
            Profile.SetValue(DeviceType, ProgId, SharedConstants.PORTNUMBER_PROFILENAME, PortNumber.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, RemoteDeviceNumber.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.SERVICE_TYPE_PROFILENAME, ServiceType.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, EstablishConnectionTimeout.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, StandardDeviceResponseTimeout.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, LongDeviceResponseTimeout.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.USERNAME_PROFILENAME, UserName);
            Profile.SetValue(DeviceType, ProgId, SharedConstants.PASSWORD_PROFILENAME, Password);
            Profile.SetValue(DeviceType, ProgId, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, ManageConnectLocally.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, ((int)ImageArrayTransferType).ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, ((int)ImageArrayCompression).ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.UNIQUEID_PROFILENAME, UniqueId.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.ENABLE_REDISCOVERY_PROFILENAME, EnableRediscovery.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.ENABLE_IPV4_DISCOVERY_PROFILENAME, IpV4Enabled.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.ENABLE_IPV6_DISCOVERY_PROFILENAME, IpV6Enabled.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.DISCOVERY_PORT_PROFILENAME, DiscoveryPort.ToString());
            Profile.SetValue(DeviceType, ProgId, SharedConstants.TRUST_UNSIGNED_SSL_CERTIFICATES_PROFILENAME, TrustUserGeneratedSslCertificates.ToString());

            // DriverDisplayName is not persisted because it is already held in the Profile in the default key field
        }
    }
}
