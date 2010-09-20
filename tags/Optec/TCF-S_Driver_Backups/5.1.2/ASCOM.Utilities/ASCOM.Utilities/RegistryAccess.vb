'Class to read and write profile values to the registry

Imports System.Security.AccessControl
Imports System.Security.Principal
Imports Microsoft.Win32
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

Friend Class RegistryAccess
    Implements IAccess, IDisposable

    Private ProfileRegKey As RegistryKey

    Private ProfileMutex As System.Threading.Mutex
    Private GotMutex As Boolean

    Private TL As TraceLogger
    Private disposedValue As Boolean = False        ' To detect redundant calls to IDisposable

    Private sw, swSupport As Stopwatch

#Region "New and IDisposable Support"
    Public Sub New()
        Me.New(False) 'Create but respect any exceptions thrown
    End Sub

    Sub New(ByVal p_CallingComponent As String)
        Me.New(False)
    End Sub

    Sub New(ByVal p_IgnoreChecks As Boolean)
        Dim PlatformVersion As String

        TL = New TraceLogger("", "RegistryAccess") 'Create a new trace logger
        TL.Enabled = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT) 'Get enabled / disabled state from the user registry
        RunningVersions(TL) ' Include version information

        sw = New Stopwatch 'Create the stowatch instances
        swSupport = New Stopwatch
        ProfileMutex = New System.Threading.Mutex(False, PROFILE_MUTEX_NAME)

        Try
            ProfileRegKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY)
            PlatformVersion = GetProfile("\", "PlatformVersion")
            'OK, no exception so assume that we are initialised
        Catch ex As System.ComponentModel.Win32Exception 'This occurs when the key does not exist and is OK if we are ignoring checks
            If p_IgnoreChecks Then
                ProfileRegKey = Nothing
            Else
                TL.LogMessageCrLf("RegistryAccess.New - Profile not found in registry at HKLM\" & REGISTRY_ROOT_KEY_NAME, ex.ToString)
                Throw New ProfilePersistenceException("RegistryAccess.New - Profile not found in registry at HKLM\" & REGISTRY_ROOT_KEY_NAME, ex)
            End If
        Catch ex As Exception
            If p_IgnoreChecks Then ' Ignore all checks
                TL.LogMessageCrLf("RegistryAccess.New IgnoreCheks is true so ignoring exception:", ex.ToString)
            Else
                TL.LogMessageCrLf("RegistryAccess.New Unexpected exception:", ex.ToString)
                Throw New ProfilePersistenceException("RegistryAccess.New - Unexpected exception", ex)
            End If
        End Try
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Try : TL.Enabled = False : Catch : End Try 'Clean up the logger
            Try : TL.Dispose() : Catch : End Try
            Try : TL = Nothing : Catch : End Try
            Try : sw.Stop() : Catch : End Try 'Clean up the stopwatches
            Try : sw = Nothing : Catch : End Try
            Try : swSupport.Stop() : Catch : End Try
            Try : swSupport = Nothing : Catch : End Try
            Try : ProfileMutex.Close() : Catch : End Try
            Try : ProfileMutex = Nothing : Catch : End Try
            Try : ProfileRegKey.Close() : Catch : End Try
            Try : ProfileRegKey = Nothing : Catch : End Try
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
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

#Region "IAccess Implementation"

    Friend Sub CreateKey(ByVal p_SubKeyName As String) Implements IAccess.CreateKey
        'Create a key

        Try
            GetProfileMutex("CreateKey", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("CreateKey", "SubKey: """ & p_SubKeyName & """")

            p_SubKeyName = Trim(p_SubKeyName) 'Normalise the string:
            Select Case p_SubKeyName
                Case ""  'Null path so do nothing
                Case Else 'Create the subkey
                    ProfileRegKey.CreateSubKey(CleanSubKey(p_SubKeyName))
                    ProfileRegKey.Flush()
            End Select
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
    End Sub

    Friend Sub DeleteKey(ByVal p_SubKeyName As String) Implements IAccess.DeleteKey
        'Delete a key
        Try
            GetProfileMutex("DeleteKey", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("DeleteKey", "SubKey: """ & p_SubKeyName & """")

            Try : ProfileRegKey.DeleteSubKeyTree(CleanSubKey(p_SubKeyName)) : ProfileRegKey.Flush() : Catch : End Try 'Remove it if at all possible but don't throw any errors
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
    End Sub

    Friend Sub RenameKey(ByVal OriginalSubKeyName As String, ByVal NewSubKeyName As String) Implements IAccess.RenameKey
        'Rename a key by creating a copy of the original key with the new name then deleting the original key
        'Throw New MethodNotImplementedException("RegistryAccess:RenameKey " & OriginalSubKeyName & " to " & NewSubKeyName)
        Dim SubKey As RegistryKey, Values As Generic.SortedList(Of String, String)
        SubKey = ProfileRegKey.OpenSubKey(CleanSubKey(NewSubKeyName))
        If SubKey Is Nothing Then 'Keydoes not exist so create it
            CreateKey(NewSubKeyName)
            Values = EnumProfile(OriginalSubKeyName)
            For Each Value As Generic.KeyValuePair(Of String, String) In Values
                WriteProfile(NewSubKeyName, Value.Key, Value.Value)
            Next
            DeleteKey(OriginalSubKeyName)
        Else ' Key already exists so throw an exception
            Throw New ProfilePersistenceException("Key " & NewSubKeyName & " already exists")
        End If
    End Sub

    Friend Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) Implements IAccess.DeleteProfile
        'Delete a value from a key

        Try
            GetProfileMutex("DeleteProfile", p_SubKeyName & " " & p_ValueName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("DeleteProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """")

            Try 'Remove value if it exists
                ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName), True).DeleteValue(p_ValueName)
                ProfileRegKey.Flush()
            Catch
                TL.LogMessage("DeleteProfile", "  Value did not exist")
            End Try
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
    End Sub

    Friend Function EnumKeys(ByVal p_SubKeyName As String) As Collections.Generic.SortedList(Of String, String) Implements IAccess.EnumKeys
        'Return a sorted list of subkeys
        Dim RetValues As New Generic.SortedList(Of String, String)
        Dim SubKeys() As String, Value As String
        Try
            GetProfileMutex("EnumKeys", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("EnumKeys", "SubKey: """ & p_SubKeyName & """")

            SubKeys = ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName)).GetSubKeyNames()

            For Each SubKey As String In SubKeys 'Process each key in trun
                Try 'If there is an error reading the data don't include in the returned list
                    'Create the new subkey and get a handle to it
                    Select Case p_SubKeyName
                        Case "", "\"
                            Value = ProfileRegKey.OpenSubKey(CleanSubKey(SubKey)).GetValue("", "").ToString
                        Case Else
                            Value = ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName) & "\" & SubKey).GetValue("", "").ToString
                    End Select
                    RetValues.Add(SubKey, Value) 'Add the Key name and default value to the hashtable
                Catch ex As Exception
                    TL.LogMessageCrLf("", "Read exception: " & ex.ToString)
                    Throw New ProfilePersistenceException("RegistryAccess.EnumKeys exception", ex)
                End Try
            Next
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
        For Each kvp As Generic.KeyValuePair(Of String, String) In RetValues
            TL.LogMessage("", "Found: " & kvp.Key & " " & kvp.Value)
        Next
        Return RetValues
    End Function

    Friend Function EnumProfile(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String) Implements IAccess.EnumProfile
        'Returns a sorted list of key values
        Dim RetValues As New Generic.SortedList(Of String, String)
        Dim Values() As String

        Try
            GetProfileMutex("EnumProfile", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("EnumProfile", "SubKey: """ & p_SubKeyName & """")

            Values = ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName)).GetValueNames
            For Each Value As String In Values
                RetValues.Add(Value, ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName)).GetValue(Value).ToString) 'Add the Key name and default value to the hashtable
            Next

            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
        Return RetValues
    End Function

    Friend Overloads Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_DefaultValue As String) As String Implements IAccess.GetProfile
        'Read a single value from a key
        Dim RetVal As String

        Try
            GetProfileMutex("GetProfile", p_SubKeyName & " " & p_ValueName & " " & p_DefaultValue)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("GetProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """" & """ DefaultValue: """ & p_DefaultValue & """")

            RetVal = "" 'Initialise return value to null string
            Try
                RetVal = ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName)).GetValue(p_ValueName).ToString
            Catch ex As NullReferenceException
                If Not (p_DefaultValue Is Nothing) Then 'We have been supplied a default value so set it and then return it
                    WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue)
                    RetVal = p_DefaultValue
                    TL.LogMessage("GetProfile", "Value not yet set, returning supplied default value: " & p_DefaultValue)
                Else
                    TL.LogMessage("GetProfile", "Value not yet set and no default value supplied, returning null string")
                End If
            Catch ex As Exception 'Any other exception
                If Not (p_DefaultValue Is Nothing) Then 'We have been supplied a default value so set it and then return it
                    WriteProfile(CleanSubKey(p_SubKeyName), CleanSubKey(p_ValueName), p_DefaultValue)
                    RetVal = p_DefaultValue
                    TL.LogMessage("GetProfile", "Key not yet set, returning supplied default value: " & p_DefaultValue)
                Else
                    TL.LogMessageCrLf("GetProfile", "Key not yet set and no default value supplied, throwing exception: " & ex.ToString)
                    Throw New ProfilePersistenceException("GetProfile Exception", ex)
                End If
            End Try

            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ProfileMutex.ReleaseMutex()
        End Try

        Return RetVal
    End Function

    Friend Overloads Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) As String Implements IAccess.GetProfile
        Return Me.GetProfile(p_SubKeyName, p_ValueName, Nothing)
    End Function

    Friend Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String) Implements IAccess.WriteProfile
        'Write a single value to a key

        Try
            GetProfileMutex("WriteProfile", p_SubKeyName & " " & p_ValueName & " " & p_ValueData)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("WriteProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """ Value: """ & p_ValueData & """")

            If p_SubKeyName = "" Then
                ProfileRegKey.SetValue(p_ValueName, p_ValueData, RegistryValueKind.String)
            Else
                ProfileRegKey.CreateSubKey(CleanSubKey(p_SubKeyName)).SetValue(p_ValueName, p_ValueData, RegistryValueKind.String)
            End If
            ProfileRegKey.Flush()

            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Catch ex As Exception 'Any other exception
            TL.LogMessageCrLf("WriteProfile", "Exception: " & ex.ToString)
            Throw New ProfilePersistenceException("RegistryAccess.WriteProfile exception", ex)
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
    End Sub

    Friend Sub MigrateProfile(ByVal CurrentPlatformVersion As String) Implements IAccess.MigrateProfile
        Dim LogEnabled As Boolean
        Dim Prof55 As XMLAccess
        Dim Key As RegistryKey

        Try
            GetProfileMutex("MigrateProfile", "")
            sw.Reset() : sw.Start() 'Start timing this call

            'Force logging to be enabled for this...
            LogEnabled = TL.Enabled 'Save logging state
            TL.Enabled = True
            RunningVersions(TL) 'Capture debug date in case logging wasn't initially enabled

            TL.LogMessage("MigrateProfile", "From platform: " & CurrentPlatformVersion & ", OS: " & [Enum].GetName(GetType(Bitness), OSBits()))

            Select Case CurrentPlatformVersion
                Case "4", "5" 'Currently on Platform 4 or 5 so Profile is in 32bit registry
                    'Profile is already in correct place so leave as is and just set the security ACL
                    Call SetRegistryACL(CurrentPlatformVersion)
                    TL.LogMessage("MigrateProfile", "The Profile is already in the correct place from Platform " & CurrentPlatformVersion)
                Case "5.5" 'Currently on Platform 5.5 so migrate Profile is in file system
                    'Backup old 5.0 Profile and Copy 5.5 Profile to registry
                    Call Backup50()
                    Call SetRegistryACL(CurrentPlatformVersion)
                    TL.LogMessage("MigrateProfile", "Creating Profile 5.5 XMLAccess object")
                    Prof55 = New XMLAccess()
                    TL.LogMessage("MigrateProfile", "OPening " & REGISTRY_ROOT_KEY_NAME & " Registry Key")
                    Key = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME, True)
                    TL.LogMessage("MigrateProfile", "Copying Profile 5.5 to Profile 6")
                    Copy55To60("", Prof55, Key)
                Case "Unknown", "" ' Don't know current platform
                    Throw New ProfilePersistenceException("Unable to determine current Platform version: """ & CurrentPlatformVersion & """ - " & "OS has bit size: " & [Enum].GetName(GetType(Bitness), OSBits()))
                Case Else '6.0 or above, leave as is!
                    'Do nothing as Profile is already in the Registry
                    TL.LogMessage("MigrateProfile", "Profile reports previous Platform as " & CurrentPlatformVersion & " - no migration required")
            End Select

            'Make sure we have a valid key now that we have migrated the profile to the registry
            ProfileRegKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY)

            'Restore original logging state
            TL.Enabled = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT) 'Get enabled / disabled state from the user registry
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
            TL.Enabled = LogEnabled 'Restore logging state
        Catch ex As Exception
            TL.LogMessageCrLf("MigrateProfile", "Exception: " & ex.ToString)
            Throw New ProfilePersistenceException("RegistryAccess.MigrateProfile exception", ex)
        Finally
            ProfileMutex.ReleaseMutex()
        End Try
    End Sub

    Friend Overloads Function GetProfile(ByVal p_SubKeyName As String) As ASCOMProfile Implements IAccess.GetProfile
        Dim ProfileContents As New ASCOMProfile

        Try
            GetProfileMutex("GetProfile", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("GetProfile", "SubKey: """ & p_SubKeyName & """")

            GetSubKey(p_SubKeyName, "", ProfileContents) 'Read the requested profile into a ProfileKey object
            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds ")

        Finally
            ProfileMutex.ReleaseMutex()
        End Try

        Return ProfileContents
    End Function

    Friend Sub SetProfile(ByVal p_SubKeyName As String, ByVal p_ProfileKey As ASCOMProfile) Implements IAccess.SetProfile
        Dim SKey As RegistryKey
        Try
            GetProfileMutex("SetProfile", p_SubKeyName)
            sw.Reset() : sw.Start() 'Start timing this call
            TL.LogMessage("SetProfile", "SubKey: """ & p_SubKeyName & """")

            For Each ProfileSubkey As String In p_ProfileKey.ProfileValues.Keys
                TL.LogMessage("SetProfile", "Received SubKey: " & ProfileSubkey)

                For Each value As String In p_ProfileKey.ProfileValues.Item(ProfileSubkey).Keys
                    TL.LogMessage("SetProfile", "  Received value: " & value & " = " & p_ProfileKey.ProfileValues.Item(ProfileSubkey).Item(value))
                Next
            Next

            For Each SubKey As String In p_ProfileKey.ProfileValues.Keys
                If p_SubKeyName = "" Then
                    If SubKey = "" Then
                        SKey = ProfileRegKey
                    Else
                        SKey = ProfileRegKey.CreateSubKey(CleanSubKey(SubKey))
                    End If
                Else
                    If SubKey = "" Then
                        SKey = ProfileRegKey.CreateSubKey(p_SubKeyName)
                    Else
                        SKey = ProfileRegKey.CreateSubKey(p_SubKeyName & "\" & CleanSubKey(SubKey))
                    End If
                End If

                For Each value As String In p_ProfileKey.ProfileValues.Item(SubKey).Keys
                    SKey.SetValue(value, p_ProfileKey.ProfileValues.Item(SubKey).Item(value), RegistryValueKind.String)
                Next
                SKey.Flush()
            Next
            ProfileRegKey.Flush()

            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds ")

        Finally
            ProfileMutex.ReleaseMutex()
        End Try

    End Sub
#End Region

#Region "Support Functions"

    Private Sub GetSubKey(ByVal BaseSubKey As String, ByVal SubKeyOffset As String, ByRef ProfileContents As ASCOMProfile)
        Dim RetVal As New ASCOMProfile
        Dim ValueNames(), SubKeyNames(), Value As String
        Dim SKey As RegistryKey

        BaseSubKey = CleanSubKey(BaseSubKey)
        SubKeyOffset = CleanSubKey(SubKeyOffset)

        If BaseSubKey = "" Then
            If SubKeyOffset = "" Then
                SKey = ProfileRegKey
            Else
                SKey = ProfileRegKey.OpenSubKey(SubKeyOffset)
            End If
        Else
            If SubKeyOffset = "" Then
                SKey = ProfileRegKey.OpenSubKey(BaseSubKey)
            Else
                SKey = ProfileRegKey.OpenSubKey(BaseSubKey & "\" & SubKeyOffset)
            End If
        End If

        ValueNames = SKey.GetValueNames

        For Each ValueName As String In ValueNames
            Value = SKey.GetValue(ValueName).ToString
            ProfileContents.SetValue(ValueName, Value, SubKeyOffset)
        Next

        SubKeyNames = SKey.GetSubKeyNames

        For Each SubKeyName As String In SubKeyNames
            GetSubKey(BaseSubKey, SubKeyOffset & "\" & CleanSubKey(SubKeyName), ProfileContents)
        Next
    End Sub

    Private Function CleanSubKey(ByVal SubKey As String) As String
        'Remove leading "\" if it exists as this is not logal in a subkey name. "\" in the middle of a subkey name is legal however
        If Left(SubKey, 1) = "\" Then Return Mid(SubKey, 2)
        Return SubKey
    End Function

    Private Sub Backup50()
        Dim FromKey, ToKey As RegistryKey
        Dim swLocal As Stopwatch
        swLocal = Stopwatch.StartNew

        FromKey = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME, True)
        ToKey = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_BACKUP_SUBKEY)

        TL.LogMessage("Backup50", "Backing up Profile 5 to " & REGISTRY_BACKUP_SUBKEY)
        Copy50(FromKey, ToKey)

        FromKey.Close() 'Close the key after migration
        ToKey.Close()

        swLocal.Stop() : TL.LogMessage("Backup50", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Private Sub Copy50(ByVal FromKey As RegistryKey, ByVal ToKey As RegistryKey)
        'Subroutine used to recursively copy copy the 5.0 registry Profile from one place to another
        Dim Value, ValueNames(), SubKeys() As String
        Dim swLocal As Stopwatch
        Dim NewFromKey, NewToKey As RegistryKey
        Static RecurseDepth As Integer

        RecurseDepth += 1 'Increment the recursion depth indicator

        swLocal = Stopwatch.StartNew
        TL.LogMessage("Copy50 " & RecurseDepth.ToString, "Copy from: " & FromKey.Name & " to: " & ToKey.Name)

        'First copy values from the from key to the to key
        ValueNames = FromKey.GetValueNames()
        For Each ValueName As String In ValueNames
            Value = FromKey.GetValue(ValueName, "").ToString
            ToKey.SetValue(ValueName, Value)
            TL.LogMessage("  Copy50", "  FromKey: " & FromKey.Name & " - """ & ValueName & """  """ & Value & """")
        Next

        'Now process the keys
        SubKeys = FromKey.GetSubKeyNames
        For Each SubKey As String In SubKeys
            If SubKey <> REGISTRY_BACKUP_SUBKEY Then 'Copy all keys except the backup key itself! Without this we would have infinite recursion...
                Value = FromKey.OpenSubKey(SubKey).GetValue("", "").ToString
                TL.LogMessage("  Copy50", "  Processing subkey: " & SubKey & " " & Value)
                NewFromKey = FromKey.OpenSubKey(SubKey) 'Create the new subkey and get a handle to it
                NewToKey = ToKey.CreateSubKey(SubKey) 'Create the new subkey and get a handle to it
                If Not Value = "" Then NewToKey.SetValue("", Value) 'Set the default value if present
                Copy50(NewFromKey, NewToKey) 'Recursively process each key
            Else
                TL.LogMessage("  Copy50", "  Skipping subkey: " & SubKey)
            End If
        Next
        swLocal.Stop() : TL.LogMessage("  Copy50", "  Completed subkey: " & FromKey.Name & " " & RecurseDepth.ToString & ",  Elapsed time: " & swLocal.ElapsedMilliseconds & " milliseconds")
        RecurseDepth -= 1 'Decrement the recursion depth counter
        swLocal = Nothing
    End Sub

    Private Sub SetRegistryACL(ByVal CurrentPlatformVersion As String)
        'Subroutine to control the migration of a Platform 5.5 profile to Platform 6
        Dim Values As New Generic.SortedList(Of String, String)
        Dim swLocal As Stopwatch
        Dim Key As RegistryKey, KeySec As RegistrySecurity, RegAccessRule As RegistryAccessRule
        Dim DomainSid, Ident As SecurityIdentifier

        swLocal = Stopwatch.StartNew

        TL.LogMessage("MigrateTo60", "Creating root key ""\""")
        Key = Registry.LocalMachine.CreateSubKey(REGISTRY_ROOT_KEY_NAME)

        'Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key
        TL.LogMessage("MigrateTo60", "Creating security identifier")
        DomainSid = New SecurityIdentifier("S-1-0-0") 'Create a starting point domain SID
        Ident = New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid) 'Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule

        TL.LogMessage("MigrateTo60", "Creating new ACL rule")
        RegAccessRule = New RegistryAccessRule(Ident, _
                                               RegistryRights.FullControl, _
                                               InheritanceFlags.ContainerInherit, _
                                               PropagationFlags.None, _
                                               AccessControlType.Allow) ' Create the new access permission rule

        TL.LogMessage("MigrateTo60", "Retrieving current ACL rule")
        KeySec = Key.GetAccessControl() ' Get existing ACL rules on the key 
        TL.LogMessage("MigrateTo60", "Adding new ACL rule")
        KeySec.AddAccessRule(RegAccessRule) 'Add the new rule to the existing rules
        TL.LogMessage("MigrateTo60", "Setting new ACL rule")
        Key.SetAccessControl(KeySec) 'Apply the new rules to the Profile key

        TL.LogMessage("MigrateTo60", "Flushing key")
        Key.Flush() 'Flush the key to make sure the permission is committed
        TL.LogMessage("MigrateTo60", "Closing key")
        Key.Close() 'Close the key after migration

        swLocal.Stop() : TL.LogMessage("MigrateTo60", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Private Sub Copy55To60(ByVal CurrentSubKey As String, ByVal Prof55 As XMLAccess, ByVal Prof6 As RegistryKey)
        'Subroutine used to recursively copy copy the 5.5 XML profile to new 64bit registry profile
        Dim Values, SubKeys As Generic.SortedList(Of String, String)
        Dim swLocal As Stopwatch
        Static RecurseDepth As Integer

        RecurseDepth += 1 'Increment the recursion depth indicator

        swLocal = Stopwatch.StartNew
        TL.LogMessage("Copy55To60 " & RecurseDepth.ToString, "Starting key: " & CurrentSubKey)

        'First copy values from the from key to the to key
        Values = Prof55.EnumProfile(CurrentSubKey)
        For Each Value As Generic.KeyValuePair(Of String, String) In Values
            Prof6.SetValue(Value.Key, Value.Value)
            TL.LogMessage("  Copy55To60", "  Key: " & CurrentSubKey & " - """ & Value.Key & """  """ & Value.Value & """")
        Next

        'Now process the keys
        SubKeys = Prof55.EnumKeys(CurrentSubKey)
        Dim NewKey As RegistryKey
        For Each SubKey As Generic.KeyValuePair(Of String, String) In SubKeys
            TL.LogMessage("  Copy55To60", "  Processing subkey: " & SubKey.Key & " " & SubKey.Value)
            NewKey = Prof6.CreateSubKey(SubKey.Key) 'Create the new subkey and get a handle to it
            If Not SubKey.Value = "" Then NewKey.SetValue("", SubKey.Value) 'Set the default value if present
            Copy55To60(CurrentSubKey & "\" & SubKey.Key, Prof55, NewKey) 'Recursively process each key
        Next
        swLocal.Stop() : TL.LogMessage("  Copy55To60", "  Completed subkey: " & CurrentSubKey & " " & RecurseDepth.ToString & ",  Elapsed time: " & swLocal.ElapsedMilliseconds & " milliseconds")
        RecurseDepth -= 1 'Decrement the recursion depth counter
        swLocal = Nothing
    End Sub

    Private Sub GetProfileMutex(ByVal Method As String, ByVal Parameters As String)
        'Get the profile mutex or log an error and throw an exception that will terminate this profile call and return to the calling application
        GotMutex = ProfileMutex.WaitOne(PROFILE_MUTEX_TIMEOUT, False)
        If Not GotMutex Then
            TL.LogMessage("GetProfileMutex", "***** WARNING ***** Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters)
            LogEvent(Method, "Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters, EventLogEntryType.Error, 0, Nothing)
            Throw New ProfilePersistenceException("Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters)
        End If
    End Sub

#Region "32/64bit registry access code"
    'There is a better way to achieve a 64bit registry view in framework 4. 
    'Only OpenSubKey is used in the code, the rest of these routines are support for OpenSubKey.
    'OpenSubKey should be replaced with Microsoft.Win32.RegistryKey.OpenBaseKey method

    '<Obsolete("Replace with Microsoft.Win32.RegistryKey.OpenBaseKey method in Framework 4", False)> _
    Friend Function OpenSubKey(ByVal ParentKey As Microsoft.Win32.RegistryKey, ByVal SubKeyName As String, ByVal Writeable As Boolean, ByVal Options As RegWow64Options) As Microsoft.Win32.RegistryKey
        Dim SubKeyHandle As System.Int32
        Dim Result As System.Int32

        If ParentKey Is Nothing OrElse GetRegistryKeyHandle(ParentKey).Equals(System.IntPtr.Zero) Then
            Throw New ProfilePersistenceException("OpenSubKey: Parent key is not open")
        End If

        Dim Rights As System.Int32 = RegistryRights.ReadKey ' Or RegistryRights.EnumerateSubKeys Or RegistryRights.QueryValues Or RegistryRights.Notify
        If Writeable Then
            Rights = RegistryRights.WriteKey
        End If

        Result = RegOpenKeyEx(GetRegistryKeyHandle(ParentKey), SubKeyName, 0, Rights Or Options, SubKeyHandle)

        Select Case Result
            Case 0 'All OK so return result
                Return PointerToRegistryKey(CType(SubKeyHandle, System.IntPtr), Writeable, False)
            Case 2 'Key not found so return nothing
                Throw New ProfilePersistenceException("Cannot open key " & SubKeyName & " as it does not exist - Result: 0x" & Hex(Result))
            Case Else 'Some other error so throw an error
                Throw New System.ComponentModel.Win32Exception(Result, "OpenSubKey: Exception encountered opening key - Result: 0x" & Hex(Result))
        End Select


        If Result <> 0 Then
        End If

    End Function

    Private Function PointerToRegistryKey(ByVal hKey As System.IntPtr, ByVal writable As Boolean, ByVal ownsHandle As Boolean) As Microsoft.Win32.RegistryKey
        ' Create a SafeHandles.SafeRegistryHandle from this pointer - this is a private class
        Dim privateConstructors As System.Reflection.BindingFlags
        Dim safeRegistryHandleType As System.Type
        Dim safeRegistryHandleConstructorTypes As System.Type() = {GetType(System.IntPtr), GetType(System.Boolean)}
        Dim safeRegistryHandleConstructor As System.Reflection.ConstructorInfo
        Dim safeHandle As System.Object
        Dim registryKeyType As System.Type
        Dim registryKeyConstructor As System.Reflection.ConstructorInfo
        Dim result As Microsoft.Win32.RegistryKey

        privateConstructors = System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic
        safeRegistryHandleType = GetType(Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle")
        safeRegistryHandleConstructor = safeRegistryHandleType.GetConstructor(privateConstructors, Nothing, safeRegistryHandleConstructorTypes, Nothing)
        safeHandle = safeRegistryHandleConstructor.Invoke(New System.Object() {hKey, ownsHandle})

        ' Create a new Registry key using the private constructor using the safeHandle - this should then behave like a .NET natively opened handle and disposed of correctly
        Dim registryKeyConstructorTypes As System.Type() = {safeRegistryHandleType, GetType(System.Boolean)}
        registryKeyType = GetType(Microsoft.Win32.RegistryKey)
        registryKeyConstructor = registryKeyType.GetConstructor(privateConstructors, Nothing, registryKeyConstructorTypes, Nothing)
        result = DirectCast(registryKeyConstructor.Invoke(New Object() {safeHandle, writable}), Microsoft.Win32.RegistryKey)

        Return result
    End Function

    Private Function GetRegistryKeyHandle(ByVal RegisteryKey As Microsoft.Win32.RegistryKey) As System.IntPtr
        ' Basis from http://blogs.msdn.com/cumgranosalis/archive/2005/12/09/Win64RegistryPart1.aspx
        Dim Type As System.Type = System.Type.GetType("Microsoft.Win32.RegistryKey")
        Dim Info As System.Reflection.FieldInfo = Type.GetField("hkey", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)

        Dim Handle As System.Runtime.InteropServices.SafeHandle = CType(Info.GetValue(RegisteryKey), System.Runtime.InteropServices.SafeHandle)
        Dim RealHandle As System.IntPtr = Handle.DangerousGetHandle

        Return Handle.DangerousGetHandle
    End Function

    Friend Enum RegWow64Options As System.Int32
        ' Basis from: http://www.pinvoke.net/default.aspx/advapi32/RegOpenKeyEx.html
        None = 0
        KEY_WOW64_64KEY = &H100
        KEY_WOW64_32KEY = &H200
    End Enum

    Private Declare Auto Function RegOpenKeyEx Lib "advapi32.dll" (ByVal hKey As System.IntPtr, ByVal lpSubKey As System.String, ByVal ulOptions As System.Int32, ByVal samDesired As System.Int32, ByRef phkResult As System.Int32) As System.Int32
#End Region
#End Region

End Class
