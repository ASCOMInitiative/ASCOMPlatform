using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Utilities.Alpaca
{
    public static class Constants
    {
        public const string DISCOVERY_MESSAGE = "alpacadiscovery";
        public const string DISCOVERY_RESPONSE_STRING = "alpacaport";

        public const string TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE = "Trying to contact Alpaca management API";
        public const string FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE = "The Alpaca management API did not respond within the discovery response time";

        // Alpaca and ASCOM error number constants
        public const int ASCOM_ERROR_NUMBER_OFFSET = unchecked((int)0x80040000); // Offset value that relates the ASCOM Alpaca reserved error number range to the ASCOM COM HResult error number range
        public const int ASCOM_ERROR_NUMBER_BASE = unchecked((int)0x80040400); // Lowest ASCOM error number
        public const int ASCOM_ERROR_NUMBER_MAX = unchecked((int)0x80040FFF); // Highest ASCOM error number
        public const int ALPACA_ERROR_CODE_BASE = 0x400; // Start of the Alpaca error code range 0x400 to 0xFFF
        public const int ALPACA_ERROR_CODE_MAX = 0xFFF; // End of Alpaca error code range 0x400 to 0xFFF

        // Persistence and data schema constants
        public const int CURRENT_SCHEMA_VERSION = 1;
        public const string STATE_FILE_DIRECTORY = "ASCOM\\AlpacaDiscoveryClient"; // Relative to the user's app data directory
        public const string STATE_FILE_NAME = "configuration.json";
        public const bool DEFAULT_TRACE_STATE = true;
        public const Decimal DEFAULT_TIME_OUT = 2.0M;
        public const int DEFAULT_DISCOVERY_PORT = 32227;

        // Discovery timeout constants
        public const double DEFAULT_ALPACA_DISCOVERY_TIMEOUT = 2.0; // Default time out (seconds) time for a management API command
        public const double MINIMUM_TIME_OUT = 1.0; // Minimum allowable discovery timeout value

        public const int NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES = 2;
    }
}