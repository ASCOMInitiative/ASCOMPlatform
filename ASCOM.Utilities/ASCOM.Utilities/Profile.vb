Option Strict On
Option Explicit On
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

''' <summary>
''' ASCOM Scope Driver Helper Registry Profile Object
''' </summary>
''' <remarks>
''' <para>This object provides facilities for registering ASCOM drivers with the Chooser, and for storing 
''' persistence information in a shared area of the file system.</para>
''' <para>Please code to the IProfile interface</para>
''' </remarks>
<ClassInterface(ClassInterfaceType.None), _
Guid("880840E2-76E6-4036-AD8F-60A326D7F9DA"), _
ComVisible(True)> _
Public Class Profile
    Implements IProfile, IProfileExtra, IDisposable
    '   ===========
    '   PROFILE.CLS
    '   ===========
    '
    ' Written:  21-Jan-01   Robert B. Denny <rdenny@dc3.com>
    '
    ' Edits:
    '
    ' When      Who     What
    ' --------- ---     --------------------------------------------------
    ' 25-Feb-09 pwgs    5.1.0 - Refactor for Utilities
    ' -----------------------------------------------------------------------------

    Private m_sDeviceType As String ' Device type specified by user
    Private ProfileStore As RegistryAccess
    Private TL As TraceLogger
    Private LastDriverID As String, LastResult As Boolean ' Cache values to improve IsRegistered performance


#Region "New and IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Create a new Profile object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        ProfileStore = New RegistryAccess(ERR_SOURCE_PROFILE) 'Get access to the profile store
        m_sDeviceType = "Telescope"
        TL = New TraceLogger("", "Profile")
        TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT) 'Get enabled / disabled state from the user registry
        TL.LogMessage("New", "Trace logger created OK")
    End Sub

    ''' <summary>
    ''' Create a new profile object ignoring profile not found exceptions if generated
    ''' </summary>
    ''' <param name="IgnoreExceptions">Ignore ProfileNotFound exceptions</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal IgnoreExceptions As Boolean)
        MyBase.New()
        ProfileStore = New RegistryAccess(IgnoreExceptions) 'Get access to the profile store
        m_sDeviceType = "Telescope"
        TL = New TraceLogger("", "Profile")
        TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT) 'Get enabled / disabled state from the user registry
        TL.LogMessage("New", "Trace logger created OK - Ignoring any ProfileNotFound exceptions")
    End Sub

    ''' <summary>
    ''' Disposes of resources used by the profile object - called by IDisposable interface
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If

            If Not (ProfileStore Is Nothing) Then
                ProfileStore.Dispose()
                ProfileStore = Nothing
            End If
            If Not (TL Is Nothing) Then
                TL.Enabled = False
                TL.Dispose()
                TL = Nothing
            End If

        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of resources used by the profile object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "IProfile Implementation"
    ''' <summary>
    ''' The type of ASCOM device for which profile data and registration services are provided 
    ''' (String, default = "Telescope")
    ''' </summary>
    ''' <value>String describing the type of device being accessed</value>
    ''' <returns>String describing the type of device being accessed</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown on setting the device type to empty string</exception>
    ''' <remarks></remarks>
    Public Property DeviceType() As String Implements IProfile.DeviceType
        Get
            Return m_sDeviceType
        End Get
        Set(ByVal Value As String)
            TL.LogMessage("DeviceType Set", Value.ToString)
            If Value = "" Then Throw New Exceptions.InvalidValueException(MSG_ILLEGAL_DEVTYPE) 'Err.Raise(SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE)
            m_sDeviceType = Value
        End Set
    End Property

    ''' <summary>
    ''' List the device types registered in the Profile store
    ''' </summary>
    ''' <value>List of registered device types</value>
    ''' <returns>A sorted string array of device types</returns>
    ''' <remarks>Use this to find the types of device that are registered in the Profile store.
    ''' <para>Device types are returned without the suffix " Devices" that is used in key names within the 
    ''' profile store; this allows direct use of returned device types inside For Each loops as shown in 
    ''' the Profile code example.</para>
    ''' </remarks>
    Public ReadOnly Property RegisteredDeviceTypes() As String() Implements IProfile.RegisteredDeviceTypes
        Get
            Dim RootKeys As Generic.SortedList(Of String, String)
            Dim RegDevs As New Generic.List(Of String), DType As String
            Dim RetVal() As String

            RootKeys = ProfileStore.EnumKeys("") ' Get root Keys
            TL.LogMessage("RegisteredDeviceTypes", "Found " & RootKeys.Count & " values")
            For Each kvp As Generic.KeyValuePair(Of String, String) In RootKeys
                TL.LogMessage("RegisteredDeviceTypes", "  " & kvp.Key & " " & kvp.Value)
                If Right(kvp.Key, 8) = " Drivers" Then
                    DType = Left(kvp.Key, Len(kvp.Key) - 8)
                    TL.LogMessage("RegisteredDeviceTypes", "    Adding: " & DType)
                    RegDevs.Add(DType) 'Only add keys that contain drivers
                End If
            Next
            RegDevs.Sort() 'Sort the list into alphabetical order
            ReDim RetVal(RegDevs.Count - 1)
            RegDevs.CopyTo(RetVal) 'Copy values to array

            Return RetVal

        End Get
    End Property

    ''' <summary>
    ''' List the devices of a given device type that are registered in the Profile store
    ''' </summary>
    ''' <param name="DeviceType">Type of devices to list</param>
    ''' <returns>An ArrayList of installed devices and associated device descriptions</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Throw if the supplied DeviceType is empty string or 
    ''' null value.</exception>
    ''' <remarks>
    ''' Use this to find all the registerd devices of a given type that are in the Profile store
    ''' <para>If a DeviceType is supplied, where no device of that type has been registered before on this system,
    ''' an empty list will be returned</para>
    ''' </remarks>
    Public Function RegisteredDevices(ByVal DeviceType As String) As ArrayList Implements IProfile.RegisteredDevices
        Dim RegDevs As Generic.SortedList(Of String, String) = Nothing
        Dim RetVal As New ArrayList
        If String.IsNullOrEmpty(DeviceType) Then ' Null value and empty string are invalid DeviceTypes
            TL.LogMessage("RegisteredDevices", "Empty string or Nothing supplied as DeviceType")
            Throw New Exceptions.InvalidValueException("Empty string or Nothing supplied as DeviceType")
        End If
        Try
            RegDevs = ProfileStore.EnumKeys(DeviceType & " Drivers") ' Get Key-Class pairs
        Catch ex As System.NullReferenceException 'Catch exception thrown if the Deviceype is an invalid value
            TL.LogMessage("RegisteredDevices", "WARNING: there are no devices of type: """ & DeviceType & """ registered on this system")
            RegDevs = New Generic.SortedList(Of String, String) 'Return an empty list
        End Try
        TL.LogMessage("RegisteredDevices", "Device type: " & DeviceType & " - found " & RegDevs.Count & " devices")
        For Each kvp As Generic.KeyValuePair(Of String, String) In RegDevs
            TL.LogMessage("RegisteredDevices", "  " & kvp.Key & " - " & kvp.Value)
            RetVal.Add(New KeyValuePair(kvp.Key, kvp.Value))
        Next
        Return RetVal
    End Function

    ''' <summary>
    ''' Confirms whether a specific driver is registered ort unregistered in the profile store
    ''' </summary>
    ''' <param name="DriverID">String reprsenting the device's ProgID</param>
    ''' <returns>Boolean indicating registered or unregisteredstate of the supplied DriverID</returns>
    ''' <remarks></remarks>
    Public Function IsRegistered(ByVal DriverID As String) As Boolean Implements IProfile.IsRegistered
        Return Me.IsRegisteredPrv(DriverID, False)
    End Function


    ''' <summary>
    ''' Registers a supplied DriverID and associates a descriptive name with the device
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to register</param>
    ''' <param name="DescriptiveName">Descriptive name of the device</param>
    ''' <remarks>Does nothing if already registered, so safe to call on driver load.</remarks>
    Public Sub Register(ByVal DriverID As String, ByVal DescriptiveName As String) Implements IProfile.Register
        'Register a driver
        If Not Me.IsRegistered(DriverID) Then
            TL.LogMessage("Register", "Registering " & DriverID)
            ProfileStore.CreateKey(MakeKey(DriverID, ""))
            ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName)
            LastDriverID = "" 'Clear this value so that the next next IsRegistered test doesn't use a now invalid cached value
        Else
            TL.LogMessage("Register", DriverID & " already registered")
        End If
    End Sub

    ''' <summary>
    ''' Remove all data for the given DriverID from the registry.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to unregister</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks><para>This deletes the entire device profile tree, including the DriverID root key.</para>
    ''' <para>Unregister must only be called if the driver has already been registered. If you are not sure 
    ''' use the IsRegistered function to check the status and only unregister if the driver is registered.</para>
    ''' </remarks>
    Public Sub Unregister(ByVal DriverID As String) Implements IProfile.Unregister
        'Unregister a driver
        TL.LogMessage("Unregister", DriverID)

        CheckRegistered(DriverID)
        TL.LogMessage("Unregister", "Unregistering " & DriverID)

        LastDriverID = "" 'Clear this value so that the next next IsRegistered test doesn't use a now invalid cached value
        ProfileStore.DeleteKey(MakeKey(DriverID, ""))
    End Sub

    ''' <summary>
    ''' Retrieve a string value from the profile using the supplied subkey for the given Driver ID 
    ''' and variable name. Set and return the default value if the requested variable name has not yet been set.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="SubKey">Subkey from the profile root from which to read the value</param>
    ''' <param name="DefaultValue">Default value to be used if there is no value currently set</param>
    ''' <returns>Retrieved variable value</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>
    ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
    ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
    ''' <para>If a default value is supplied and the value is not already present in the profile store,
    ''' the default value will be set in the profile store and then returned as the value of the 
    ''' DriverID/SubKey/Name. If the default value is set to null (C#) or Nothing (VB) then no value will
    ''' be set in the profile and an empty string will be returned as the value of the 
    ''' DriverID/SubKey/Name.</para>
    ''' </remarks>
    Public Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String, ByVal DefaultValue As String) As String Implements IProfile.GetValue
        'Get a profile value
        Dim Rtn As String
        TL.LogMessage("GetValue", "Driver: " & DriverID & " Name: " & Name & " Subkey: """ & SubKey & """" & " DefaultValue: """ & DefaultValue & """")

        CheckRegistered(DriverID)
        Rtn = ProfileStore.GetProfile(MakeKey(DriverID, SubKey), Name, DefaultValue)
        TL.LogMessage("  GetValue", "  " & Rtn)

        Return Rtn
    End Function

    ''' <summary>
    ''' Writes a string value to the profile using the supplied subkey for the given Driver ID and variable name.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="Value">The string value to be written</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <exception cref="Exceptions.RestrictedAccessException">Thrown if Name and SubKey are both empty strings. This 
    ''' value is reserved for the device description as it appears in Chooser and is set by Profile.Register</exception>
    ''' <remarks></remarks>
    Public Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String) Implements IProfile.WriteValue
        'Create or update a profile value
        TL.LogMessage("WriteValue", "Driver: " & DriverID & " Name: " & Name & " Value: " & Value & " Subkey: """ & SubKey & """")
        If Value Is Nothing Then
            TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string")
            Value = ""
        End If

        CheckRegistered(DriverID)
        If Name = "" And SubKey = "" Then
            'Err.Raise(SCODE_ILLEGAL_REGACC, ERR_SOURCE_PROFILE, MSG_ILLEGAL_REGACC)
            Throw New Exceptions.RestrictedAccessException("The device default value is protected as it contains the device descriptio and is set by Profile.Register")
        End If
        ProfileStore.WriteProfile(MakeKey(DriverID, SubKey), Name, Value)
    End Sub

    ''' <summary>
    ''' Return a list of the (unnamed and named variables) under the given DriverID and subkey.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <returns>An ArrayList of KeyValuePairs</returns>
    ''' <remarks>The returned object contains entries for each value. For each entry, 
    ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
    ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
    ''' Key property set to an empty string.
    ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
    '''  </remarks>
    Public Overloads Function Values(ByVal DriverID As String, ByVal SubKey As String) As ArrayList Implements IProfile.Values
        Dim RetVal As New ArrayList
        'Return a hashtable of all values in a given key
        Dim Vals As Generic.SortedList(Of String, String)
        TL.LogMessage("Values", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        Vals = ProfileStore.EnumProfile(MakeKey(DriverID, SubKey))
        TL.LogMessage("  Values", "  Returning " & Vals.Count & " values")
        For Each kvp As Generic.KeyValuePair(Of String, String) In Vals
            TL.LogMessage("  Values", "  " & kvp.Key & " = " & kvp.Value)
            RetVal.Add(New KeyValuePair(kvp.Key, kvp.Value))
        Next
        Return RetVal
    End Function

    ''' <summary>
    ''' Delete the value from the registry. Name may be an empty string for the unnamed value. Value will be deleted from the subkey supplied.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
    Public Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String) Implements IProfile.DeleteValue
        'Delete a value
        TL.LogMessage("DeleteValue", "Driver: " & DriverID & " Name: " & Name & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        ProfileStore.DeleteProfile(MakeKey(DriverID, SubKey), Name)
    End Sub

    ''' <summary>
    ''' Create a registry key for the given DriverID.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>If the SubKey argument contains a \ separated path, the intermediate keys will be created if needed. </remarks>
    Public Sub CreateSubKey(ByVal DriverID As String, ByVal SubKey As String) Implements IProfile.CreateSubKey
        'Create a subkey
        TL.LogMessage("CreateSubKey", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        ProfileStore.CreateKey(MakeKey(DriverID, SubKey))
    End Sub

    ''' <summary>
    ''' Return a list of the sub-keys under the given DriverID
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <returns>An ArrayList of key-value pairs</returns>
    ''' <remarks>The returned object contains entries for each sub-key. For each KeyValuePair in the list, 
    ''' the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
    ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
    ''' </remarks>
    Public Overloads Function SubKeys(ByVal DriverID As String, ByVal SubKey As String) As ArrayList Implements IProfile.SubKeys
        Dim RetVal As New ArrayList, SKeys As Generic.SortedList(Of String, String)

        TL.LogMessage("SubKeys", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        If DriverID <> "" Then CheckRegistered(DriverID)
        SKeys = ProfileStore.EnumKeys(MakeKey(DriverID, SubKey))
        TL.LogMessage("  SubKeys", "  Returning " & SKeys.Count & " subkeys")
        For Each kvp As Generic.KeyValuePair(Of String, String) In SKeys
            TL.LogMessage("  SubKeys", "  " & kvp.Key & " = " & kvp.Value)
            RetVal.Add(New KeyValuePair(kvp.Key, kvp.Value))
        Next
        Return RetVal
    End Function

    ''' <summary>
    ''' Delete a registry key for the given DriverID. SubKey may contain \ separated path to key to be deleted.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>The sub-key and all data and keys beneath it are deleted.</remarks>
    Public Sub DeleteSubKey(ByVal DriverID As String, ByVal SubKey As String) Implements IProfile.DeleteSubKey
        'Delete a subkey
        TL.LogMessage("DeleteSubKey", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        ProfileStore.DeleteKey(MakeKey(DriverID, SubKey))
    End Sub

    ''' <summary>
    ''' Read an entire device profile and return it as an XML encoded string
    ''' </summary>
    ''' <param name="DriverId">The ProgID of the device</param>
    ''' <returns>Device profile encoded in XML</returns>
    ''' <remarks>Please see the code examples in this help file for a description of how to use this method.
    '''<para>The format of the returned XML is shown below. The SubKey element repeats for as many subkeys as are present while the Value element with its
    ''' Name and Data memebers repeats as often as there are values in a particular subkey.</para>
    ''' <para><pre>
    ''' &lt;?xml version="1.0" encoding="utf-8" ?&gt; 
    ''' &lt;ASCOMProfileAL&gt;
    ''' &lt;SubKey&gt;
    ''' &lt;SubKeyName /&gt; 
    ''' &lt;DefaultValue&gt;Default text value&lt;/DefaultValue&gt; 
    '''    &lt;Values&gt;
    '''      &lt;Value&gt;
    '''        &lt;Name&gt;Valuename 1&lt;/Name&gt; 
    '''        &lt;Data&gt;False&lt;/Data&gt; 
    '''      &lt;/Value&gt;
    '''      &lt;Value&gt;
    '''        &lt;Name&gt;Valuename 2&lt;/Name&gt; 
    '''        &lt;Data&gt;True&lt;/Data&gt; 
    '''      &lt;/Value&gt;
    '''    &lt;/Values&gt;
    '''  &lt;/SubKey&gt;
    '''  &lt;SubKey&gt;
    '''    &lt;SubKeyName&gt;Settings&lt;/SubKeyName&gt; 
    '''    &lt;DefaultValue /&gt; 
    '''    &lt;Values&gt;
    '''      &lt;Value&gt;
    '''        &lt;Name&gt;Valuename 3&lt;/Name&gt; 
    '''        &lt;Data&gt;1&lt;/Data&gt; 
    '''      &lt;/Value&gt;
    '''      &lt;Value&gt;
    '''        &lt;Name&gt;Valuename 4&lt;/Name&gt; 
    '''        &lt;Data&gt;53.4217&lt;/Data&gt; 
    '''      &lt;/Value&gt;
    '''    &lt;/Values&gt;
    '''  &lt;/SubKey&gt;
    ''' &lt;/ASCOMProfileAL>
    ''' </pre></para></remarks>
    Public Function GetProfileXML(ByVal DriverId As String) As String Implements IProfile.GetProfileXML
        Dim RetVal As String, CurrentProfile As ASCOMProfile, XMLProfileBytes() As Byte
        Dim XMLSer As New XmlSerializer(GetType(ASCOMProfile))
        Dim UTF8 As New UTF8Encoding

        TL.LogMessage("GetProfileXML", "Driver: " & DriverId)
        CheckRegistered(DriverId)

        CurrentProfile = ProfileStore.GetProfile(MakeKey(DriverId, ""))
        Using MemStream As New MemoryStream 'Create a memory stream to receive the serialised ProfileKey
            XMLSer.Serialize(MemStream, CurrentProfile) 'Serialise the ProfileKey object to XML held in the memory stream
            XMLProfileBytes = MemStream.ToArray() 'Retrieve the serialised unicode XML characters as a byte array
            RetVal = UTF8.GetString(XMLProfileBytes) 'Convert the byte array into a UTF8 character string
        End Using

        TL.LogMessageCrLf("  GetProfileXML", vbCrLf & RetVal)
        Return RetVal
    End Function

    ''' <summary>
    ''' Set an entire device profile from an XML encoded string
    ''' </summary>
    ''' <param name="DriverId">The ProgID of the device</param>
    ''' <param name="XmlProfile">An XML encoding of the profile</param>
    ''' <remarks>Please see the code examples in this help file for a description of how to use this method. See GetProfileXML for a 
    ''' description of the XML format.</remarks>
    Public Sub SetProfileXML(ByVal DriverId As String, ByVal XmlProfile As String) Implements IProfile.SetProfileXML
        Dim NewProfileContents As ASCOMProfile, XMLProfileBytes() As Byte
        Dim XMLSer As New XmlSerializer(GetType(ASCOMProfile))
        Dim UTF8 As New UTF8Encoding

        TL.LogMessageCrLf("SetProfileXML", "Driver: " & DriverId & vbCrLf & XmlProfile)
        CheckRegistered(DriverId)

        XMLProfileBytes = UTF8.GetBytes(XmlProfile) 'Convert the UTF8 XML string into a byte array
        Using MemStream As New MemoryStream(XMLProfileBytes) 'Present the UTF8 string byte array as a memory stream
            NewProfileContents = CType(XMLSer.Deserialize(MemStream), ASCOMProfile) 'Deserialise the stream to a ProfileKey object holding the new set of values
        End Using

        ProfileStore.SetProfile(MakeKey(DriverId, ""), NewProfileContents)
        TL.LogMessage("  SetProfileXML", "  Complete")
    End Sub

#End Region

#Region "IProfileExtra Implementation"
    ''' <summary>
    ''' Read an entire device profile and return it as an ASCOMProfile class instance
    ''' </summary>
    ''' <param name="DriverId">The ProgID of the device</param>
    ''' <returns>Device profile represented as a recusrive class</returns>
    ''' <remarks>Please see the code examples in this help file for a description of how to use this method.</remarks>
    Public Function GetProfile(ByVal DriverId As String) As ASCOMProfile Implements IProfileExtra.GetProfile
        Dim RetVal As ASCOMProfile
        TL.LogMessage("GetProfile", "Driver: " & DriverId)
        CheckRegistered(DriverId)
        RetVal = ProfileStore.GetProfile(MakeKey(DriverId, ""))
        TL.LogMessageCrLf("  GetProfile", "Complete")
        Return RetVal
    End Function

    ''' <summary>
    ''' Set an entire device profile from an ASCOMProfile class instance
    ''' </summary>
    ''' <param name="DriverId">The ProgID of the device</param>
    ''' <param name="NewProfileKey">A class representing the profile</param>
    ''' <remarks>Please see the code examples in this help file for a description of how to use this method.</remarks>
    Public Sub SetProfile(ByVal DriverId As String, ByVal NewProfileKey As ASCOMProfile) Implements IProfileExtra.SetProfile
        TL.LogMessage("SetProfile", "Driver: " & DriverId)
        CheckRegistered(DriverId)
        ProfileStore.SetProfile(MakeKey(DriverId, ""), NewProfileKey)
        TL.LogMessage("  SetProfile", "  Complete")
    End Sub

    ''' <summary>
    ''' Migrate the ASCOM profile from registry to file store
    ''' </summary>
    ''' <param name="CurrentPlatformVersion">The platform version number of the current profile store beig migrated</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never), _
    ComVisible(False)> _
    Public Sub MigrateProfile(ByVal CurrentPlatformVersion As String) Implements IProfileExtra.MigrateProfile
        TL.Enabled = True 'Force tracing on when migrating a profile
        TL.LogMessage("MigrateProfile", "Migrating profile from Platform " & CurrentPlatformVersion & " to Platform " & PLATFORM_VERSION)
        Try
            ProfileStore.MigrateProfile(CurrentPlatformVersion)
            TL.LogMessage("MigrateProfile", "Successfully migrated Profile")
            TL.LogMessage("MigrateProfile", "Setting Platform version string to: " & PLATFORM_VERSION)
            ProfileStore.WriteProfile("", "PlatformVersion", PLATFORM_VERSION) 'Set the platform version in the ASCOM root key
            TL.LogMessage("MigrateProfile", "Successfully set PlatformVersion to: " & PLATFORM_VERSION)
            TL.LogMessage("MigrateProfile", "Completed migration")
        Catch ex As Exception
            TL.LogMessageCrLf("MigrateProfile", "Exception: " & ex.ToString)
            Throw New ASCOM.Utilities.Exceptions.ProfilePersistenceException("Profile.MigrateProfileException", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Delete the value from the registry. Name may be an empty string for the unnamed value. 
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key.
    ''' <para>This overload is not available through COM, please use 
    ''' "DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
    ''' with SubKey set to empty string achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String) Implements IProfileExtra.DeleteValue
        Me.DeleteValue(DriverID, Name, "")
    End Sub

    ''' <summary>
    ''' Retrieve a string value from the profile for the given Driver ID and variable name
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <returns>Retrieved variable value</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>
    ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
    ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
    ''' <para>This overload is not available through COM, please use 
    ''' "GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
    ''' with SubKey set to empty string achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String) As String Implements IProfileExtra.GetValue
        Return Me.GetValue(DriverID, Name, "", Nothing)
    End Function

    ''' <summary>
    ''' Retrieve a string value from the profile using the supplied subkey for the given Driver ID and variable name.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="SubKey">Subkey from the profile root from which to read the value</param>
    ''' <returns>Retrieved variable value</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>
    ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
    ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String) As String Implements IProfileExtra.GetValue
        Return Me.GetValue(DriverID, Name, SubKey, Nothing)
    End Function

    ''' <summary>
    ''' Return a list of the sub-keys under the root of the given DriverID
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <returns>An ArrayList of key-value pairs</returns>
    ''' <remarks>The returned object contains entries for each sub-key. For each KeyValuePair in the list, 
    ''' the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
    ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function SubKeys(ByVal DriverID As String) As ArrayList Implements IProfileExtra.SubKeys
        Return Me.SubKeys(DriverID, "")
    End Function

    ''' <summary>
    ''' Return a list of the (unnamed and named variables) under the root of the given DriverID.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <returns>An ArrayList of KeyValuePairs</returns>
    ''' <remarks>The returned object contains entries for each value. For each entry, 
    ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
    ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
    ''' Key property set to an empty string.
    ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
    '''  </remarks>
    <ComVisible(False)> _
    Public Overloads Function Values(ByVal DriverID As String) As ArrayList Implements IProfileExtra.Values
        Return Me.Values(DriverID, "")
    End Function

    ''' <summary>
    ''' Writes a string value to the profile using the given Driver ID and variable name.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="Value">The string value to be written</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>
    ''' This overload is not available through COM, please use 
    ''' "WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String)"
    ''' with SubKey set to empty string achieve this effect.
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String) Implements IProfileExtra.WriteValue
        Me.WriteValue(DriverID, Name, Value, "")
    End Sub
#End Region

#Region "Support code"
    Private Function IsRegisteredPrv(ByVal DriverID As String, ByVal Indent As Boolean) As Boolean
        'Confirm that the specified driver is registered
        Dim keys As Generic.SortedList(Of String, String)
        Dim IndStr As String = ""

        If Indent Then IndStr = "  "

        If DriverID = LastDriverID Then
            TL.LogMessage(IndStr & "IsRegistered", IndStr & DriverID.ToString & " - using cached value: " & LastResult)
            Return LastResult
        End If
        TL.LogStart(IndStr & "IsRegistered", IndStr & DriverID.ToString & " ")

        IsRegisteredPrv = False ' Assume failure
        If DriverID = "" Then
            TL.LogFinish("Null string so exiting False")
            Exit Function ' Nothing is a failure
        End If

        Try
            keys = ProfileStore.EnumKeys(MakeKey("", ""))
            If keys.ContainsKey(DriverID) Then
                TL.LogFinish("Key " & DriverID & " found")
                IsRegisteredPrv = True ' Found it
            Else
                TL.LogFinish("Key " & DriverID & " not found")
            End If
        Catch ex As Exception
            TL.LogFinish("Exception: " & ex.ToString)
        End Try

        LastDriverID = DriverID
        LastResult = IsRegisteredPrv
    End Function

    Private Function MakeKey(ByVal BaseKey As String, ByVal SubKey As String) As String
        'Create a full path to a subkey given the driver name and type 
        MakeKey = m_sDeviceType & " Drivers"
        If BaseKey <> "" Then MakeKey = MakeKey & "\" & BaseKey ' Allow blank BaseKey (See SubKeys())
        If SubKey <> "" Then MakeKey = MakeKey & "\" & SubKey
        Return MakeKey
    End Function

    Private Sub CheckRegistered(ByVal DriverID As String)
        'Confirm that a given driver exists
        TL.LogMessage("  CheckRegistered", """" & DriverID & """")
        If Not Me.IsRegisteredPrv(DriverID, True) Then
            TL.LogMessage("  CheckRegistered", "Driver is not registered")
            If DriverID = "" Then
                TL.LogMessage("  CheckRegistered", "Throwing illegal driver ID exception")
                'Err.Raise(SCODE_ILLEGAL_DRIVERID, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DRIVERID)
                Throw New Exceptions.InvalidValueException(MSG_ILLEGAL_DRIVERID)
            Else
                TL.LogMessage("  CheckRegistered", "Throwing driver is not registered exception")
                'Err.Raise(SCODE_DRIVER_NOT_REG, ERR_SOURCE_PROFILE, "DriverID " & DriverID & " is not registered.")
                Throw New Exceptions.DriverNotRegisteredException("DriverID " & DriverID & " is not registered.")
            End If
        Else
            TL.LogMessage("  CheckRegistered", "Driver is registered")
        End If
    End Sub
#End Region

End Class