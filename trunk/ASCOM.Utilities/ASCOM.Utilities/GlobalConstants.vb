'Common constants for the ASCOM.Utilities namesapce

Module GlobalConstants
    Friend Const PLATFORM_VERSION As String = "6.0" 'This is the master platform version and is set during profile migration

    Friend Const SERIAL_FILE_NAME_VARNAME As String = "SerTraceFile" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_AUTO_FILENAME As String = "C:\SerialTraceAuto.txt" 'Special value to indicate use of automatic trace filenames
    Friend Const SERIAL_DEFAULT_FILENAME As String = "C:\SerialTrace.txt" 'Default manual trace filename
    Friend Const SERIAL_DEBUG_TRACE_VARNAME As String = "SerDebugTrace" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_FORCED_COMPORTS_VARNAME As String = "ForcedCOMPorts" 'Constant listing COM ports that will be forced to be present
    Friend Const SERIAL_IGNORED_COMPORTS_VARNAME As String = "IgnoredCOMPorts" 'Constant listing COM ports that will be ignored if present

    'Utilities configuration constants
    Friend Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Friend Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
    Friend Const SERIAL_TRACE_DEBUG As String = "Serial Trace Debug", SERIAL_TRACE_DEBUG_DEFAULT As Boolean = False

    Friend Const PROFILE_MUTEX_NAME As String = "ASCOMProfileMutex" 'Name and timout value for the Profile mutex than ensures only one profile action happens at a time
    Friend Const PROFILE_MUTEX_TIMEOUT As Integer = 5000

    ' Trace settings values, these are used to persist trace values on a per user basis
    Friend Const TRACE_TRANSFORM As String = "Trace Transform", TRACE_TRANSFORM_DEFAULT As Boolean = False
    Friend Const REGISTRY_UTILITIES_FOLDER As String = "Software\ASCOM\Utilities"

    'Settings for the ASCOM Windows event log
    Friend Const EVENT_SOURCE As String = "ASCOM Platform" 'Name of the the event source
    Friend Const EVENTLOG_NAME As String = "ASCOM" 'Name of the event log as it appears in Windows event viewer

End Module