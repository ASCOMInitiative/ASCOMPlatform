Imports System

'Common constants for the ASCOM.Utilities namesapce

Module GlobalConstants
    Friend Const SERIAL_FILE_NAME_VARNAME As String = "SerTraceFile" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_AUTO_FILENAME As String = "C:\SerialTraceAuto.txt" 'Special value to indicate use of automatic trace filenames
    Friend Const SERIAL_DEFAULT_FILENAME As String = "C:\SerialTrace.txt" 'Default manual trace filename
    Friend Const SERIAL_DEBUG_TRACE_VARNAME As String = "SerDebugTrace" 'Constant naming the profile trace file variable name
    Friend Const SERIALPORT_COM_PORT_SETTINGS As String = "COMPortSettings"
    Friend Const SERIAL_FORCED_COMPORTS_VARNAME As String = SERIALPORT_COM_PORT_SETTINGS & "\ForceCOMPorts" 'Constant listing COM ports that will be forced to be present
    Friend Const SERIAL_IGNORE_COMPORTS_VARNAME As String = SERIALPORT_COM_PORT_SETTINGS & "\IgnoreCOMPorts" 'Constant listing COM ports that will be ignored if present

    'Utilities configuration constants
    Friend Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Friend Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
    Friend Const TRACE_UTIL As String = "Trace Util", TRACE_UTIL_DEFAULT As Boolean = False
    Friend Const TRACE_TIMER As String = "Trace Timer", TRACE_TIMER_DEFAULT As Boolean = False
    Friend Const SERIAL_TRACE_DEBUG As String = "Serial Trace Debug", SERIAL_TRACE_DEBUG_DEFAULT As Boolean = False
    Friend Const SIMULATOR_TRACE As String = "Trace Simulators", SIMULATOR_TRACE_DEFAULT As Boolean = False
    Friend Const DRIVERACCESS_TRACE As String = "Trace DriverAccess", DRIVERACCESS_TRACE_DEFAULT As Boolean = False
    Friend Const CHOOSER_USE_CREATEOBJECT As String = "Chooser Use CreateObject", CHOOSER_USE_CREATEOBJECT_DEFAULT As Boolean = False
    Friend Const ABANDONED_MUTEXT_TRACE As String = "Trace Abandoned Mutexes", ABANDONED_MUTEX_TRACE_DEFAULT As Boolean = False
    Friend Const ASTROUTILS_TRACE As String = "Trace Astro Utils", ASTROUTILS_TRACE_DEFAULT As Boolean = False
    Friend Const NOVAS_TRACE As String = "Trace NOVAS", NOVAS_TRACE_DEFAULT As Boolean = False
    Friend Const SERIAL_WAIT_TYPE As String = "Serial Wait Type", SERIAL_WAIT_TYPE_DEFAULT As ASCOM.Utilities.Serial.WaitType = Serial.WaitType.WaitForSingleObject

    Friend Const PROFILE_MUTEX_NAME As String = "ASCOMProfileMutex" 'Name and timout value for the Profile mutex than ensures only one profile action happens at a time
    Friend Const PROFILE_MUTEX_TIMEOUT As Integer = 5000

    ' Trace settings values, these are used to persist trace values on a per user basis
    Friend Const TRACE_TRANSFORM As String = "Trace Transform", TRACE_TRANSFORM_DEFAULT As Boolean = False
    Friend Const REGISTRY_UTILITIES_FOLDER As String = "Software\ASCOM\Utilities"

    'Settings for the ASCOM Windows event log
    Friend Const EVENT_SOURCE As String = "ASCOM Platform" 'Name of the the event source
    Friend Const EVENTLOG_NAME As String = "ASCOM" 'Name of the event log as it appears in Windows event viewer
    Friend Const EVENTLOG_MESSAGES As String = "ASCOM\EventLogMessages.txt"
    Friend Const EVENTLOG_ERRORS As String = "ASCOM\EventLogErrors.txt"

    'RegistryAccess constants
    Friend Const REGISTRY_ROOT_KEY_NAME As String = "SOFTWARE\ASCOM" 'Location of ASCOM profile in HKLM registry hive
    Friend Const REGISTRY_5_BACKUP_SUBKEY As String = "Platform5Original" 'Location that the original Plartform 5 Profile will be copied to before migrating the 5.5 Profile back to the registry
    Friend Const REGISTRY_55_BACKUP_SUBKEY As String = "Platform55Original" 'Location that the original Plartform 5.5 Profile will be copied to before removing Platform 5 and 5.5
    Friend Const PLATFORM_VERSION_NAME As String = "PlatformVersion"
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
    'These are public so that they can be used by applications to work directly with the returned XML
    Public Const XML_SUBKEYNAME_ELEMENTNAME As String = "SubKeyName"
    Public Const XML_DEFAULTVALUE_ELEMENTNAME As String = "DefaultValue"
    Public Const XML_NAME_ELEMENTNAME As String = "Name"
    Public Const XML_DATA_ELEMENTNAME As String = "Data"
    Public Const XML_SUBKEY_ELEMENTNAME As String = "SubKey"
    Public Const XML_VALUE_ELEMENTNAME As String = "Value"
    Public Const XML_VALUES_ELEMENTNAME As String = "Values"

    'Location of the lists of 32bit and 64bit only drivers and PlatformVersion exception lists
    Public Const DRIVERS_32BIT As String = "Drivers Not Compatible With 64bit Applications" ' 32bit only registry location
    Public Const DRIVERS_64BIT As String = "Drivers Not Compatible With 32bit Applications" ' 64bit only registry location
    Friend Const PLATFORM_VERSION_EXCEPTIONS As String = "ForcePlatformVersion"
    Friend Const PLATFORM_VERSION_SEPARATOR_EXCEPTIONS As String = "ForcePlatformVersionSeparator"

    Friend Const FORCE_SYSTEM_TIMER As String = "ForceSystemTimer" 'Location of executables for which we must force system timer rather than forms timer

    'Installer Variables
    Public Const PLATFORM_INSTALLER_PROPDUCT_CODE As String = "{8961E141-B307-4882-ABAD-77A3E76A40C1}" '{8961E141-B307-4882-ABAD-77A3E76A40C1}
    Public Const DEVELOPER_INSTALLER_PROPDUCT_CODE As String = "{4A195DC6-7DF9-459E-8F93-60B61EB45288}"

    'Contact driver author message
    Friend Const DRIVER_AUTHOR_MESSAGE_DRIVER As String = "Please contact the driver author and request an updated driver."
    Friend Const DRIVER_AUTHOR_MESSAGE_INSTALLER As String = "Please contact the driver author and request an updated installer."

    'Location of Platform version in Profile
    Friend Const PLATFORM_INFORMATION_SUBKEY As String = "Platform"
    Friend Const PLATFORM_VERSION As String = "Platform Version"
    Friend Const PLATFORM_VERSION_DEFAULT_BAD_VALUE As String = "0.0.0.0"

    ' Other constants
    Friend Const ABSOLUTE_ZERO_CELSIUS As Double = -273.15

    Friend Enum EventLogErrors As Integer
        EventLogCreated = 0
        ChooserFormLoad = 1
        MigrateProfileVersions = 2
        MigrateProfileRegistryKey = 3
        RegistryProfileMutexTimeout = 4
        XMLProfileMutexTimeout = 5
        XMLAccessReadError = 6
        XMLAccessRecoveryPreviousVersion = 7
        XMLAccessRecoveredOK = 8
        ChooserSetupFailed = 9
        ChooserDriverFailed = 10
        ChooserException = 11
        Chooser32BitOnlyException = 12
        Chooser64BitOnlyException = 13
        FocusSimulatorNew = 14
        FocusSimulatorSetup = 15
        TelescopeSimulatorNew = 16
        TelescopeSimulatorSetup = 17
        VB6HelperProfileException = 18
        DiagnosticsLoadException = 19
        DriverCompatibilityException = 20
        TimerSetupException = 21
        DiagnosticsHijackedCOMRegistration = 22
        UninstallASCOMInfo = 23
        UninstallASCOMError = 24
        ProfileExplorerException = 25
        InstallTemplatesInfo = 26
        InstallTemplatesError = 27
        TraceLoggerException = 28
        TraceLoggerMutexTimeOut = 29
        TraceLoggerMutexAbandoned = 30
        RegistryProfileMutexAbandoned = 31
    End Enum
End Module