Friend Module Constants
    Public Const DISCOVERY_MESSAGE As String = "alpacadiscovery1" ' Fixed text alpacadiscovery plus a string version number of 1
    Public Const DISCOVERY_RESPONSE_STRING As String = "alpacaport"
    Public Const TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE As String = "Trying to contact Alpaca management API"
    Public Const ALPACA_DISCOVERY_IPV6_MULTICAST_ADDRESS As String = "ff12::a19aca"

    ' Alpaca and ASCOM error number constants
    Public Const FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE As String = "The Alpaca management API did not respond within the discovery response time"

    Public Const ASCOM_ERROR_NUMBER_OFFSET As Integer = &H80040000 ' Offset value that relates the ASCOM Alpaca reserved Error number range To the ASCOM COM HResult Error number range
    Public Const ASCOM_ERROR_NUMBER_BASE As Integer = &H80040400 ' Lowest ASCOM Error number
    Public Const ASCOM_ERROR_NUMBER_MAX As Integer = &H80040FFF ' Highest ASCOM Error number
    Public Const ALPACA_ERROR_CODE_BASE As Integer = &H400 ' Start of the Alpaca error code range 0x400 to 0xFFF
    Public Const ALPACA_ERROR_CODE_MAX As Integer = &HFFF ' End of Alpaca error code range 0x400 to 0xFFF

    ' Persistence and data schema constants
    Public Const CURRENT_SCHEMA_VERSION As Integer = 1
    Public Const STATE_FILE_DIRECTORY As String = "ASCOM\AlpacaDiscoveryClient" ' Relative to the user's app data directory
    Public Const STATE_FILE_NAME As String = "configuration.json"
    Public Const DEFAULT_TRACE_STATE As Boolean = True
    Public Const DEFAULT_TIME_OUT As Decimal = 2D
    Public Const DEFAULT_DISCOVERY_PORT As Integer = 32227

    ' Discovery timeout constants
    Public Const DEFAULT_ALPACA_DISCOVERY_TIMEOUT As Double = 2.0 ' Default time out (seconds) time for a management API command
    Public Const MINIMUM_TIME_OUT As Double = 1.0 ' Minimum allowable discovery timeout value
    Public Const NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES As Integer = 2
End Module
