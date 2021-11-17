using Microsoft.Win32;
using System.Runtime.Serialization.Formatters;

namespace ASCOM.DynamicRemoteClients
{
    public static class SharedConstants
    {
        // Alpaca and ASCOM error number constants
        public const int ASCOM_ERROR_NUMBER_OFFSET = unchecked((int)0x80040000); // Offset value that relates the ASCOM Alpaca reserved error number range to the ASCOM COM HResult error number range
        public const int ASCOM_ERROR_NUMBER_BASE = unchecked((int)0x80040400); // Lowest ASCOM error number
        public const int ASCOM_ERROR_NUMBER_MAX = unchecked((int)0x80040FFF); // Highest ASCOM error number
        public const int ALPACA_ERROR_CODE_BASE = 0x400; // Start of the Alpaca error code range 0x400 to 0xFFF
        public const int ALPACA_ERROR_CODE_MAX = 0xFFF; // End of Alpaca error code range 0x400 to 0xFFF

        // Regular expressions to validate IP addresses and host names
        public const string ValidIpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        public const string ValidHostnameRegex = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";

        // HTTP Parameter names shared by driver and remote device
        public const string RA_PARAMETER_NAME = "RightAscension";
        public const string DEC_PARAMETER_NAME = "Declination";
        public const string ALT_PARAMETER_NAME = "Altitude";
        public const string AZ_PARAMETER_NAME = "Azimuth";
        public const string AXIS_PARAMETER_NAME = "Axis";
        public const string RATE_PARAMETER_NAME = "Rate";
        public const string DIRECTION_PARAMETER_NAME = "Direction";
        public const string DURATION_PARAMETER_NAME = "Duration";
        public const string CLIENTID_PARAMETER_NAME = "ClientID";
        public const string CLIENTTRANSACTION_PARAMETER_NAME = "ClientTransactionID";
        public const string COMMAND_PARAMETER_NAME = "Command";
        public const string RAW_PARAMETER_NAME = "Raw";
        public const string LIGHT_PARAMETER_NAME = "Light";
        public const string ACTION_COMMAND_PARAMETER_NAME = "Action";
        public const string ACTION_PARAMETERS_PARAMETER_NAME = "Parameters";
        public const string ID_PARAMETER_NAME = "ID";
        public const string STATE_PARAMETER_NAME = "State";
        public const string NAME_PARAMETER_NAME = "Name";
        public const string VALUE_PARAMETER_NAME = "Value";
        public const string POSITION_PARAMETER_NAME = "Position";
        public const string SIDEOFPIER_PARAMETER_NAME = "SideOfPier";
        public const string UTCDATE_PARAMETER_NAME = "UTCDate";
        public const string SENSORNAME_PARAMETER_NAME = "SensorName";
        public const string BRIGHTNESS_PARAMETER_NAME = "Brightness";

        public const string ISO8601_DATE_FORMAT_STRING = "yyyy-MM-ddTHH:mm:ss.fffffff";

        // Dynamic client configuration constants
        public const int SOCKET_ERROR_MAXIMUM_RETRIES = 2; // The number of retries that the client will make when it receives a socket actively refused error from the remote device
        public const int SOCKET_ERROR_RETRY_DELAY_TIME = 1000; // The delay time (milliseconds) between socket actively refused retries

        // remote device setup form constants
        public const string LOCALHOST_NAME_IPV4 = "localhost";
        public const string LOCALHOST_ADDRESS_IPV4 = "127.0.0.1"; // Get the localhost loop back address
        public const string STRONG_WILDCARD_NAME = "+"; // Get the localhost loop back address

        // Constants shared by Remote Client Drivers and the ASCOM remote device
        public const string API_URL_BASE = "/api/"; // This constant must always be lower case to make the logic tests work properly 
        public const string API_VERSION_V1 = "v1"; // This constant must always be lower case to make the logic tests work properly

        // Client driver profile persistence constants
        public const string TRACE_LEVEL_PROFILENAME = "Trace Level"; public const bool CLIENT_TRACE_LEVEL_DEFAULT = true;
        public const string DEBUG_TRACE_PROFILENAME = "Include Debug Trace"; public const bool DEBUG_TRACE_DEFAULT = false;
        public const string IPADDRESS_PROFILENAME = "IP Address"; public const string IPADDRESS_DEFAULT = SharedConstants.LOCALHOST_ADDRESS_IPV4;
        public const string PORTNUMBER_PROFILENAME = "Port Number"; public const decimal PORTNUMBER_DEFAULT = 11111;
        public const string REMOTE_DEVICE_NUMBER_PROFILENAME = "Remote Device Number"; public const decimal REMOTE_DEVICE_NUMBER_DEFAULT = 0;
        public const string SERVICE_TYPE_PROFILENAME = "Service Type"; public const string SERVICE_TYPE_DEFAULT = "http";
        public const string ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME = "Establish Connection Timeout"; public const int ESTABLISH_CONNECTION_TIMEOUT_DEFAULT = 2;
        public const string STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME = "Standard Device Response Timeout"; public const int STANDARD_SERVER_RESPONSE_TIMEOUT_DEFAULT = 10;
        public const string LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME = "Long Device Response Timeout"; public const int LONG_SERVER_RESPONSE_TIMEOUT_DEFAULT = 120;
        public const string USERNAME_PROFILENAME = "User Name"; public const string USERNAME_DEFAULT = "";
        public const string PASSWORD_PROFILENAME = "Password"; public const string PASSWORD_DEFAULT = "";
        public const string MANAGE_CONNECT_LOCALLY_PROFILENAME = "Manage Connect Locally"; public const bool MANAGE_CONNECT_LOCALLY_DEFAULT = false;
        public const string IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME = "Image Array Transfer Type"; public const ASCOM.Common.Alpaca.ImageArrayTransferType IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT = DEFAULT_IMAGE_ARRAY_TRANSFER_TYPE;
        public const string IMAGE_ARRAY_COMPRESSION_PROFILENAME = "Image Array Compression"; public const ASCOM.Common.Alpaca.ImageArrayCompression IMAGE_ARRAY_COMPRESSION_DEFAULT = DEFAULT_IMAGE_ARRAY_COMPRESSION;
        public const string UNIQUEID_PROFILENAME = "UniqueID"; public const string UNIQUEID_DEFAULT = "Unknown";
        public const string ENABLE_REDISCOVERY_PROFILENAME = "Enable Rediscovery"; public const bool ENABLE_REDISCOVERY_DEFAULT = true;
        public const string ENABLE_IPV4_DISCOVERY_PROFILENAME = "Enable IPv4 Discovery"; public const bool ENABLE_IPV4_DISCOVERY_DEFAULT = true;
        public const string ENABLE_IPV6_DISCOVERY_PROFILENAME = "Enable IPv6 Discovery"; public const bool ENABLE_IPV6_DISCOVERY_DEFAULT = true;
        public const string DISCOVERY_PORT_PROFILENAME = "Discovery Port"; public const int DISCOVERY_PORT_DEFAULT = 32227;

        // Driver naming constants
        public const string DRIVER_DISPLAY_NAME = "ASCOM Alpaca Dynamic Client";
        public const string DRIVER_PROGID_BASE = "ASCOM.AlpacaDynamic";
        public const string NOT_CONNECTED_MESSAGE = "is not connected."; // This is appended to the driver display name + driver number and displayed when the driver is not connected
        public const string TRACELOGGER_NAME_FORMAT_STRING = "AlpacaDynamic{0}.{1}";

        // Enum to describe Camera.ImageArray and ImageArrayVCariant array types
        //public enum Base64HandoffElementTypes
        //{
        //    Unknown = 0,
        //    Int16 = 1,
        //    Int32 = 2,
        //    Double = 3,
        //    Float = 4,
        //    Byte = 5,
        //    Int64 = 6

        //}

        //// Enum used by the dynamic client to indicate what type of image array transfer should be used
        //public enum ImageArrayTransferType
        //{
        //    JSON = 0,
        //    Base64HandOff = 1,
        //    GetBase64Image = 2,
        //    GetImageBytes = 3,
        //    BestAvailable = 4
        //}

        // Enum used by the dynamic client to indicate what type of compression should be used in responses
        //public enum ImageArrayCompression
        //{
        //    None = 0,
        //    Deflate = 1,
        //    GZip = 2,
        //    GZipOrDeflate = 3
        //}

        // Default image array transfer constants
        public const ASCOM.Common.Alpaca.ImageArrayCompression DEFAULT_IMAGE_ARRAY_COMPRESSION = ASCOM.Common.Alpaca.ImageArrayCompression.None;
        public const ASCOM.Common.Alpaca.ImageArrayTransferType DEFAULT_IMAGE_ARRAY_TRANSFER_TYPE = ASCOM.Common.Alpaca.ImageArrayTransferType.BestAvailable;

        // Image array base64 hand-off and ImageBytes support constants
        public const string BASE64_HANDOFF_HEADER = "base64handoff"; // Name of HTTP header used to affirm binary serialisation support for image array data
        public const string BASE64_HANDOFF_SUPPORTED = "true"; // Value of HTTP header to indicate support for binary serialised image array data
        public const string BASE64_HANDOFF_FILE_DOWNLOAD_URI_EXTENSION = "base64"; // Addition to the ImageArray and ImageArrayVariant method names from which base64 serialised image files can be downloaded
        public const string GETBASE64IMAGE_ACTION_NAME = "GetBase64Image";
        public const int GETBASE64IMAGE_SUPPORTED_VERSION = 1;
        public const string IMAGE_ARRAY_METHOD_NAME = "ImageArray";

        // Basic mime-type values
        public const string ACCEPT_HEADER_NAME = "Accept"; // Name of HTTP header used to affirm ImageBytes support for image array data
        public const string CONTENT_TYPE_HEADER_NAME = "Content-Type"; // Name of HTTP header used to affirm the type of data returned by the device
        public const string IMAGE_BYTES_MIME_TYPE = "application/imagebytes"; // Image bytes mime type
        public const string APPLICATION_JSON_MIME_TYPE = "application/json"; // Application/json mime type
        public const string TEXT_JSON_MIME_TYPE = "text/json"; // Text/json mime type

        // Combined mime-type values
        public const string JSON_MIME_TYPES = APPLICATION_JSON_MIME_TYPE + ", " + TEXT_JSON_MIME_TYPE; // JSON mime types
        public const string IMAGE_BYTES_ACCEPT_HEADER = IMAGE_BYTES_MIME_TYPE + ", " + JSON_MIME_TYPES; // Value of HTTP header to indicate support for the GetImageBytes mechanic

        public const string DEVICE_NOT_CONFIGURED = "None"; // ProgID / UniqueID / device type value indicating no device configured

        // Constants used by the generated dynamic client driver assembly
        public const string ALPACA_CLIENT_LOCAL_SERVER_PATH = @"\ASCOM\AlpacaDynamicClients\"; // Relative path from CommonFiles
        public const string ALPACA_CLIENT_LOCAL_SERVER = @"ASCOM.AlpacaClientLocalServer.exe"; // Name of the remote client local server application

    }
}
