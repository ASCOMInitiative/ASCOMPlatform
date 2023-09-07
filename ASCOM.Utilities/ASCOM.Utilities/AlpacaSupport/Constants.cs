
namespace ASCOM.Utilities
{
    internal static class Constants
    {
        public const string DISCOVERY_MESSAGE = "alpacadiscovery1"; // Fixed text alpacadiscovery plus a string version number of 1
        public const string DISCOVERY_RESPONSE_STRING = "alpacaport";
        public const string TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE = "Trying to contact Alpaca management API";
        public const string ALPACA_DISCOVERY_IPV6_MULTICAST_ADDRESS = "ff12::a1:9aca";

        // Alpaca and ASCOM error number constants
        public const string FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE = "The Alpaca management API did not respond within the discovery response time";

        public const int ASCOM_ERROR_NUMBER_OFFSET = int.MinValue + 0x00040000; // Offset value that relates the ASCOM Alpaca reserved Error number range To the ASCOM COM HResult Error number range
        public const int ASCOM_ERROR_NUMBER_BASE = int.MinValue + 0x00040400; // Lowest ASCOM Error number
        public const int ASCOM_ERROR_NUMBER_MAX = int.MinValue + 0x00040FFF; // Highest ASCOM Error number
        public const int ALPACA_ERROR_CODE_BASE = 0x400; // Start of the Alpaca error code range 0x400 to 0xFFF
        public const int ALPACA_ERROR_CODE_MAX = 0xFFF; // End of Alpaca error code range 0x400 to 0xFFF

        // Persistence and data schema constants
        public const int CURRENT_SCHEMA_VERSION = 1;
        public const string STATE_FILE_DIRECTORY = @"ASCOM\AlpacaDiscoveryClient"; // Relative to the user's app data directory
        public const string STATE_FILE_NAME = "configuration.json";
        public const bool DEFAULT_TRACE_STATE = true;
        public const decimal DEFAULT_TIME_OUT = 2m;
        public const int DEFAULT_DISCOVERY_PORT = 32227;

        // Discovery timeout constants
        public const double DEFAULT_ALPACA_DISCOVERY_TIMEOUT = 2.0d; // Default time out (seconds) time for a management API command
        public const double MINIMUM_TIME_OUT = 1.0d; // Minimum allowable discovery timeout value
        public const int NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES = 2;
        public const double MINIMUM_TIME_REMAINING_TO_UNDERTAKE_DNS_RESOLUTION = 0.1d; // Minimum discovery time (seconds) that must remain if a DNS IP to host name resolution is to be attempted
    }
}