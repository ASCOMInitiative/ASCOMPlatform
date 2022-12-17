using ASCOM.Common.Alpaca;

namespace ASCOM.DynamicRemoteClients
{
    public static class SharedConstants
    {
        // Client driver profile persistence constants
        public const string TRACE_LEVEL_PROFILENAME = "Trace Level"; public const bool CLIENT_TRACE_LEVEL_DEFAULT = true;
        public const string DEBUG_TRACE_PROFILENAME = "Include Debug Trace"; public const bool DEBUG_TRACE_DEFAULT = false;
        public const string LOCALHOST_NAME_IPV4 = "127.0.0.1";
        public const string STRONG_WILDCARD_NAME = "+";
        public const string VALID_HOST_NAME_REGEX = "^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\\-]*[A-Za-z0-9])$";
        public const string IPADDRESS_PROFILENAME = "IP Address"; public const string IPADDRESS_DEFAULT = LOCALHOST_NAME_IPV4;
        public const string PORTNUMBER_PROFILENAME = "Port Number"; public const decimal PORTNUMBER_DEFAULT = 11111;
        public const string REMOTE_DEVICE_NUMBER_PROFILENAME = "Remote Device Number"; public const decimal REMOTE_DEVICE_NUMBER_DEFAULT = 0;
        public const string SERVICE_TYPE_PROFILENAME = "Service Type"; public const string SERVICE_TYPE_DEFAULT = "http";
        public const string ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME = "Establish Connection Timeout"; public const int ESTABLISH_CONNECTION_TIMEOUT_DEFAULT = 2;
        public const string STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME = "Standard Device Response Timeout"; public const int STANDARD_SERVER_RESPONSE_TIMEOUT_DEFAULT = 10;
        public const string LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME = "Long Device Response Timeout"; public const int LONG_SERVER_RESPONSE_TIMEOUT_DEFAULT = 120;
        public const string USERNAME_PROFILENAME = "User Name"; public const string USERNAME_DEFAULT = "";
        public const string PASSWORD_PROFILENAME = "Password"; public const string PASSWORD_DEFAULT = "";
        public const string MANAGE_CONNECT_LOCALLY_PROFILENAME = "Manage Connect Locally"; public const bool MANAGE_CONNECT_LOCALLY_DEFAULT = false;
        public const string IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME = "Image Array Transfer Type"; public const ImageArrayTransferType IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT = DEFAULT_IMAGE_ARRAY_TRANSFER_TYPE;
        public const string IMAGE_ARRAY_COMPRESSION_PROFILENAME = "Image Array Compression"; public const ImageArrayCompression IMAGE_ARRAY_COMPRESSION_DEFAULT = DEFAULT_IMAGE_ARRAY_COMPRESSION;
        public const string UNIQUEID_PROFILENAME = "UniqueID"; public const string UNIQUEID_DEFAULT = "Unknown";
        public const string ENABLE_REDISCOVERY_PROFILENAME = "Enable Rediscovery"; public const bool ENABLE_REDISCOVERY_DEFAULT = true;
        public const string ENABLE_IPV4_DISCOVERY_PROFILENAME = "Enable IPv4 Discovery"; public const bool ENABLE_IPV4_DISCOVERY_DEFAULT = true;
        public const string ENABLE_IPV6_DISCOVERY_PROFILENAME = "Enable IPv6 Discovery"; public const bool ENABLE_IPV6_DISCOVERY_DEFAULT = true;
        public const string DISCOVERY_PORT_PROFILENAME = "Discovery Port"; public const int DISCOVERY_PORT_DEFAULT = 32227;

        // Trace logger naming template
        public const string TRACELOGGER_NAME_FORMAT_STRING = "AlpacaDynamic{0}.{1}";

        // Default image array transfer constants
        public const ImageArrayCompression DEFAULT_IMAGE_ARRAY_COMPRESSION = ImageArrayCompression.None;
        public const ImageArrayTransferType DEFAULT_IMAGE_ARRAY_TRANSFER_TYPE = ImageArrayTransferType.BestAvailable;
        public const string IMAGE_ARRAY_METHOD_NAME = "ImageArray";

        // Constants used by the generated dynamic client driver assembly
        public const string ALPACA_CLIENT_LOCAL_SERVER_PATH = @"\ASCOM\AlpacaDynamicClients\"; // Relative path from CommonFiles
        public const string ALPACA_CLIENT_LOCAL_SERVER = @"ASCOM.AlpacaClientLocalServer.exe"; // Name of the remote client local server application

        // Driver naming constants
        public const string DRIVER_DISPLAY_NAME = "ASCOM Alpaca Dynamic Client";
        public const string DRIVER_PROGID_BASE = "ASCOM.AlpacaDynamic";
        public const string NOT_CONNECTED_MESSAGE = "is not connected."; // This is appended to the driver display name + driver number and displayed when the driver is not connected

    }
}
