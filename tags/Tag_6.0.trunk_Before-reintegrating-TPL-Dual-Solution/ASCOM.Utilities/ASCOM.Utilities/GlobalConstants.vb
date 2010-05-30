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

    'RegistryAccess constants
    Friend Const REGISTRY_ROOT_KEY_NAME As String = "SOFTWARE\ASCOM" 'Location of ASCOM profile in HKLM registry hive
    Friend Const REGISTRY_BACKUP_SUBKEY As String = "Platform5Original" 'Location that the original Plartform 5 Profile will be copied to before migrating the 5.5 Profile back to the registry

    'XML constants used by XMLAccess and RegistryAccess classes
    Friend Const COLLECTION_DEFAULT_VALUE_NAME As String = "***** DefaultValueName *****" 'Name identifier label
    Friend Const COLLECTION_DEFAULT_UNSET_VALUE As String = "===== ***** UnsetValue ***** =====" 'Value identifier label
    Friend Const VALUES_FILENAME As String = "Profile.xml" 'Name of file to contain profile xml information
    Friend Const VALUES_FILENAME_ORIGINAL As String = "ProfileOriginal.xml" 'Name of file to contain original profile xml information
    Friend Const VALUES_FILENAME_NEW As String = "ProfileNew.xml" 'Name of file to contain original profile xml information

    Friend Const PROFILE_NAME As String = "Profile" 'Name of top level XML element
    Friend Const SUBKEY_NAME As String = "SubKey" 'Profile subkey element name
    Friend Const DEFAULT_ELEMENT_NAME As String = "DefaultElement" 'Default value label
    Friend Const VALUE_ELEMENT_NAME As String = "Element" 'Profile value element name
    Friend Const NAME_ATTRIBUTE_NAME As String = "Name" 'Profile value name attribute
    Friend Const VALUE_ATTRIBUTE_NAME As String = "Value" 'Profile element value attribute

    'XML constants used by ASCOMProfile class to serialise and de-serialise a profile
    'These are public so that they can be used by applications to wrok directly with the returned XML
    Public Const XML_SUBKEYNAME_ELEMENTNAME As String = "SubKeyName"
    Public Const XML_DEFAULTVALUE_ELEMENTNAME As String = "DefaultValue"
    Public Const XML_NAME_ELEMENTNAME As String = "Name"
    Public Const XML_DATA_ELEMENTNAME As String = "Data"
    Public Const XML_SUBKEY_ELEMENTNAME As String = "SubKey"
    Public Const XML_VALUE_ELEMENTNAME As String = "Value"
    Public Const XML_VALUES_ELEMENTNAME As String = "Values"

End Module