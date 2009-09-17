Option Strict On
Option Explicit On
Imports System
Imports System.Collections
Imports ASCOM.HelperNET.Interfaces

''' <summary>
''' ASCOM Scope Driver Helper Registry Profile Object
''' </summary>
''' <remarks>
''' <para>This object provides facilities for registering ASCOM drivers with the Chooser, and for storing 
''' persistence information in a shared area of the file system.</para>
''' <para>Please code to the IProfile interface</para>
''' </remarks>
Public Class Profile
    Implements IProfile, IDisposable
    '---------------------------------------------------------------------
    ' Copyright © 2000-2002 SPACE.com Inc., New York, NY
    '
    ' Permission is hereby granted to use this Software for any purpose
    ' including combining with commercial products, creating derivative
    ' works, and redistribution of source or binary code, without
    ' limitation or consideration. Any redistributed copies of this
    ' Software must include the above Copyright Notice.
    '
    ' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
    ' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
    ' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
    '---------------------------------------------------------------------
    '
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
    ' 25-Feb-09 pwgs    5.1.0 - Refactor for Helper.NET
    ' -----------------------------------------------------------------------------

    Private m_sDeviceType As String ' Device type specified by user
    Private ProfileStore As IAccess
    Private Tl As Interfaces.ITraceLogger


#Region "New and IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    Public Sub New()
        MyBase.New()
        ProfileStore = New XMLAccess(ERR_SOURCE_PROFILE) 'Get access to the profile store
        m_sDeviceType = "Telescope"
        Tl = New TraceLogger("", "ProfileNET")
        Tl.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT) 'Get enabled / disabled state from the user registry
        Tl.LogMessage("New", "Trace logger created OK")
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
            If Not (Tl Is Nothing) Then
                Tl.Enabled = False
                Tl.Dispose()
                Tl = Nothing
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

#Region "Profile Implementation"
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
            Tl.LogMessage("DeviceType Set", Value.ToString)
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
    ReadOnly Property RegisteredDeviceTypes() As Generic.List(Of String) Implements IProfile.RegisteredDeviceTypes
        Get
            Dim RootKeys As Generic.SortedList(Of String, String)
            Dim Retval As Generic.List(Of String), DType As String
            Retval = New Generic.List(Of String)
            RootKeys = ProfileStore.EnumKeys("") ' Get root Keys
            Tl.LogMessage("RegisteredDeviceTypes", "Found " & RootKeys.Count & " values")
            For Each kvp As Generic.KeyValuePair(Of String, String) In RootKeys
                Tl.LogMessage("RegisteredDeviceTypes", "  " & kvp.Key & " " & kvp.Value)
                If Right(kvp.Key, 8) = " Drivers" Then
                    DType = Left(kvp.Key, Len(kvp.Key) - 8)
                    Tl.LogMessage("RegisteredDeviceTypes", "    Adding: " & DType)
                    Retval.Add(DType) 'Only add keys that contain drivers

                End If
            Next
            Retval.Sort() 'Sort the list into alphabetical order
            Return Retval

        End Get
    End Property

    ''' <summary>
    ''' List the devices of a given device type that are registered in the Profile store
    ''' </summary>
    ''' <param name="DeviceType">Type of devices to list</param>
    ''' <value>List of registered devices</value>
    ''' <returns>A sorted list of installed devices and associated device descriptions</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Throw if the supplied DeviceType is empty string or 
    ''' null value.</exception>
    ''' <remarks>
    ''' Use this to find all the registerd devices of a given type that are in the Profile store
    ''' <para>If a DeviceType is supplied, where no device of that type has been registered before on this system,
    ''' an empty list will be returned</para>
    ''' </remarks>
    ReadOnly Property RegisteredDevices(ByVal DeviceType As String) As Generic.SortedList(Of String, String) Implements IProfile.RegisteredDevices
        Get
            Dim Retval As Generic.SortedList(Of String, String) = Nothing
            If String.IsNullOrEmpty(DeviceType) Then ' Null value and empty string are invalid DeviceTypes
                Tl.LogMessage("RegisteredDevices", "Empty string or Nothing supplied as DeviceType")
                Throw New Exceptions.InvalidValueException("Empty string or Nothing supplied as DeviceType")
            End If
            Try
                Retval = ProfileStore.EnumKeys(DeviceType & " Drivers") ' Get Key-Class pairs
            Catch ex As System.IO.DirectoryNotFoundException 'Catch exception thrown if the Deviceype is an invalid value
                Tl.LogMessage("RegisteredDevices", "WARNING: there are no devices of type: """ & DeviceType & """ registered on this system")
                Retval = New Generic.SortedList(Of String, String) 'Return an empty list
            End Try
            Tl.LogMessage("RegisteredDevices", "Device type: " & DeviceType & " - found " & Retval.Count & " devices")
            For Each kvp As Generic.KeyValuePair(Of String, String) In Retval
                Tl.LogMessage("RegisteredDevices", "  " & kvp.Key & " - " & kvp.Value)
            Next
            Return Retval
        End Get
    End Property

    ''' <summary>
    ''' Confirms whether a specific driver is registered ort unregistered in the profile store
    ''' </summary>
    ''' <param name="DriverID">String reprsenting the device's ProgID</param>
    ''' <returns>Boolean indicating registered or unregisteredstate of the supplied DriverID</returns>
    ''' <remarks></remarks>
    Public Function IsRegistered(ByVal DriverID As String) As Boolean Implements IProfile.IsRegistered
        Return Me.IsRegisteredPrv(DriverID, False)
    End Function
    Private Function IsRegisteredPrv(ByVal DriverID As String, ByVal Indent As Boolean) As Boolean
        'Confirm that the specified driver is registered
        Dim keys As Generic.SortedList(Of String, String)
        Dim IndStr As String = ""
        If Indent Then IndStr = "  "
        IsRegisteredPrv = False ' Assume failure
        Tl.LogStart(IndStr & "IsRegistered", IndStr & DriverID.ToString & " ")
        If DriverID = "" Then
            Tl.LogFinish("Null string so exiting False")
            Exit Function ' Nothing is a failure
        End If

        Try
            keys = ProfileStore.EnumKeys(MakeKey("", ""))
            If keys.ContainsKey(DriverID) Then
                Tl.LogFinish("Key " & DriverID & " found")
                IsRegisteredPrv = True ' Found it
            Else
                Tl.LogFinish("Key " & DriverID & " not found")
            End If
        Catch ex As Exception
            Tl.LogFinish("Exception: " & ex.ToString)
        End Try
        Return IsRegisteredPrv
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
            Tl.LogMessage("Register", "Registering " & DriverID)
            ProfileStore.CreateKey(MakeKey(DriverID, ""))
            ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName)
        Else
            Tl.LogMessage("Register", DriverID & " already registered")
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
        Tl.LogMessage("Unregister", DriverID)

        CheckRegistered(DriverID)
        Tl.LogMessage("Unregister", "Unregistering " & DriverID)

        ProfileStore.DeleteKey(MakeKey(DriverID, ""))
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
    ''' </remarks>
    Public Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String) As String Implements IProfile.GetValue
        Return Me.GetValue(DriverID, Name, "")
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
    Public Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String) As String Implements IProfile.GetValue
        'Get a profile value
        Dim Rtn As String
        Tl.LogMessage("GetValue", "Driver: " & DriverID & " Name: " & Name & " Subkey: """ & SubKey & """")

        CheckRegistered(DriverID)
        Rtn = ProfileStore.GetProfile(MakeKey(DriverID, SubKey), Name)
        Tl.LogMessage("  GetValue", "  " & Rtn)

        Return Rtn
    End Function

    ''' <summary>
    ''' Writes a string value to the profile using the given Driver ID and variable name.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <param name="Value">The string value to be written</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks></remarks>
    Public Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String) Implements IProfile.WriteValue
        Me.WriteValue(DriverID, Name, Value, "")
    End Sub

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
        Tl.LogMessage("WriteValue", "Driver: " & DriverID & " Name: " & Name & " Value: " & Value & " Subkey: """ & SubKey & """")
        If Value Is Nothing Then Tl.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string")

        CheckRegistered(DriverID)
        If Name = "" And SubKey = "" Then
            'Err.Raise(SCODE_ILLEGAL_REGACC, ERR_SOURCE_PROFILE, MSG_ILLEGAL_REGACC)
            Throw New Exceptions.RestrictedAccessException("The device default value is protected as it contains the device descriptio and is set by Profile.Register")
        End If
        ProfileStore.WriteProfile(MakeKey(DriverID, SubKey), Name, Value)
    End Sub

    ''' <summary>
    ''' Return a list of the (unnamed and named variables) under the given DriverID.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <returns>Generic Sorted List of KeyValuePairs</returns>
    ''' <remarks>The returned object contains entries for each value. For each entry, 
    ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
    ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
    ''' Key property set to an empty string. </remarks>
    Public Overloads Function Values(ByVal DriverID As String) As Generic.SortedList(Of String, String) Implements IProfile.Values
        Return Me.Values(DriverID, "")
    End Function

    ''' <summary>
    ''' Return a list of the (unnamed and named variables) under the given DriverID subkey
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
    ''' <returns>Generic Sorted List of KeyValuePairs</returns>
    ''' <remarks>The returned object contains entries for each value. For each entry, 
    ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
    ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
    ''' Key property set to an empty string. </remarks>
    Public Overloads Function Values(ByVal DriverID As String, ByVal SubKey As String) As Generic.SortedList(Of String, String) Implements IProfile.Values
        'Return a hashtable of all values in a given key
        Dim Rtn As Generic.SortedList(Of String, String)
        Tl.LogMessage("Values", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        Rtn = ProfileStore.EnumProfile(MakeKey(DriverID, SubKey))
        Tl.LogMessage("  Values", "  Returning " & Rtn.Count & " values")
        For Each kvp As Generic.KeyValuePair(Of String, String) In Rtn
            Tl.LogMessage("  Values", "  " & kvp.Key & " = " & kvp.Value)
        Next

        Return Rtn
    End Function

    ''' <summary>
    ''' Delete the value from the registry. Name may be an empty string for the unnamed value. 
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <param name="Name">Name of the variable whose value is retrieved</param>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
    Public Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String) Implements IProfile.DeleteValue
        Me.DeleteValue(DriverID, Name, "")
    End Sub

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
        Tl.LogMessage("DeleteValue", "Driver: " & DriverID & " Name: " & Name & " Subkey: """ & SubKey & """")
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
        Tl.LogMessage("CreateSubKey", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        ProfileStore.CreateKey(MakeKey(DriverID, SubKey))
    End Sub

    ''' <summary>
    ''' Return a list of the sub-keys and their default values under the given DriverID.
    ''' </summary>
    ''' <param name="DriverID">ProgID of the device to read from</param>
    ''' <returns>Generic Sorted List of key-value pairs</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>The returned Generic.SortedList object contains entries for each sub-key. For each KeyValuePair 
    ''' in the list, the Key property is the sub-key name, and the Value property is the value. The unnamed 
    ''' ("default") value for that key is also returned.</remarks>
    Public Overloads Function SubKeys(ByVal DriverID As String) As Collections.Generic.SortedList(Of String, String) Implements IProfile.SubKeys
        Return Me.SubKeys(DriverID, "")
    End Function

    ''' <summary>
    ''' Return a list of the sub-keys and their default values under the given DriverID and sub-key
    ''' </summary>
    ''' <param name="DriverID">ProgID of the driver</param>
    ''' <param name="SubKey">Subkey from the profile root in which to search for subkeys</param>
    ''' <returns>Generic Sorted List of key-value pairs</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
    ''' <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
    ''' <remarks>The returned Generic.SortedList object contains entries for each sub-key. For each KeyValuePair 
    ''' in the list, the Key property is the sub-key name, and the Value property is the value. The unnamed 
    ''' ("default") value for that key is also returned.</remarks>
    Public Overloads Function SubKeys(ByVal DriverID As String, ByVal SubKey As String) As Collections.Generic.SortedList(Of String, String) Implements IProfile.SubKeys
        'Return a hashtable of subkeys in a given key
        Dim Rtn As Generic.SortedList(Of String, String)

        Tl.LogMessage("SubKeys", "Driver: " & DriverID & " Subkey: """ & SubKey & """")

        If DriverID <> "" Then CheckRegistered(DriverID)

        Rtn = ProfileStore.EnumKeys(MakeKey(DriverID, SubKey))
        Tl.LogMessage("  SubKeys", "  Returning " & Rtn.Count & " subkeys")
        For Each kvp As Generic.KeyValuePair(Of String, String) In Rtn
            Tl.LogMessage("  SubKeys", "  " & kvp.Key & " = " & kvp.Value)
        Next
        Return Rtn
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
        Tl.LogMessage("DeleteSubKey", "Driver: " & DriverID & " Subkey: """ & SubKey & """")
        CheckRegistered(DriverID)
        ProfileStore.DeleteKey(MakeKey(DriverID, SubKey))
    End Sub
#End Region

#Region "Support code"
    Private Function MakeKey(ByVal BaseKey As String, ByVal SubKey As String) As String
        'Create a full path to a subkey given the driver name and type 
        MakeKey = m_sDeviceType & " Drivers"
        If BaseKey <> "" Then MakeKey = MakeKey & "\" & BaseKey ' Allow blank BaseKey (See SubKeys())
        If SubKey <> "" Then MakeKey = MakeKey & "\" & SubKey
        Return MakeKey
    End Function

    Private Sub CheckRegistered(ByVal DriverID As String)
        'Confirm that a given driver exists
        Tl.LogMessage("  CheckRegistered", """" & DriverID & """")
        If Not Me.IsRegisteredPrv(DriverID, True) Then
            Tl.LogMessage("  CheckRegistered", "Driver is not registered")
            If DriverID = "" Then
                Tl.LogMessage("  CheckRegistered", "Throwing illegal driver ID exception")
                'Err.Raise(SCODE_ILLEGAL_DRIVERID, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DRIVERID)
                Throw New Exceptions.InvalidValueException(MSG_ILLEGAL_DRIVERID)
            Else
                Tl.LogMessage("  CheckRegistered", "Throwing driver is not registered exception")
                'Err.Raise(SCODE_DRIVER_NOT_REG, ERR_SOURCE_PROFILE, "DriverID " & DriverID & " is not registered.")
                Throw New Exceptions.DriverNotRegisteredException("DriverID " & DriverID & " is not registered.")
            End If
        Else
            Tl.LogMessage("  CheckRegistered", "Driver is registered")
        End If
    End Sub
#End Region

End Class