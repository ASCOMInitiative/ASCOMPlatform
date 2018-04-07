'Class to read and write profile values to the registry

Imports System.Security.AccessControl
Imports System.Security.Principal
Imports Microsoft.Win32
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Environment

Friend Class RegistryAccess
    Implements IAccess, IDisposable

    Private ProfileRegKey As RegistryKey

    Private ProfileMutex As System.Threading.Mutex
    Private GotMutex As Boolean

    Private TL As TraceLogger
    Private disposedValue As Boolean = False        ' To detect redundant calls to IDisposable
    Private DisableTLOnExit As Boolean = True

    Private sw, swSupport As Stopwatch

    ''' <summary>
    ''' Enum containing all the possible registry access rights values. The buit-in RegistryRights enum only has a partial collection
    ''' and often returns values such as -1 or large positive and negative integer values when converted to a string
    ''' The Flags attribute ensures that the ToString operation returns an aggregate list of discrete values
    ''' </summary>
    <Flags()>
    Enum AccessRights
        Query = 1
        SetKey = 2
        CreateSubKey = 4
        EnumSubkey = 8
        Notify = &H10
        CreateLink = &H20
        Unknown40 = &H40
        Unknown80 = &H80

        Wow64_64Key = &H100
        Wow64_32Key = &H200
        Unknown400 = &H400
        Unknown800 = &H800
        Unknown1000 = &H1000
        Unknown2000 = &H2000
        Unknown4000 = &H4000
        Unknown8000 = &H8000

        StandardDelete = &H10000
        StandardReadControl = &H20000
        StandardWriteDAC = &H40000
        StandardWriteOwner = &H80000
        StandardSynchronize = &H100000
        Unknown200000 = &H200000
        Unknown400000 = &H400000
        AuditAccess = &H800000

        AccessSystemSecurity = &H1000000
        MaximumAllowed = &H2000000
        Unknown4000000 = &H4000000
        Unknown8000000 = &H8000000
        GenericAll = &H10000000
        GenericExecute = &H20000000
        GenericWrite = &H40000000
        GenericRead = &H80000000
    End Enum

#Region "New and IDisposable Support"
    Public Sub New()
        Me.New(False) 'Create but respect any exceptions thrown
    End Sub

    Sub New(ByVal TraceLoggerToUse As TraceLogger)
        ' This initiator is called by UninstallASCOM in order to pass in its TraceLogger so that all output appears in one file
        TL = TraceLoggerToUse
        DisableTLOnExit = False

        RunningVersions(TL)

        sw = New Stopwatch 'Create the stowatch instances
        swSupport = New Stopwatch
        ProfileMutex = New System.Threading.Mutex(False, PROFILE_MUTEX_NAME)

        ProfileRegKey = Nothing
    End Sub

    Sub New(ByVal p_CallingComponent As String)
        If p_CallingComponent.ToUpper = "UNINSTALLASCOM" Then 'Special handling for migration application
            TL = New TraceLogger("", "ProfileMigration") 'Create a new trace logger
            TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT) 'Get enabled / disabled state from the user registry

            RunningVersions(TL)

            sw = New Stopwatch 'Create the stowatch instances
            swSupport = New Stopwatch
            ProfileMutex = New System.Threading.Mutex(False, PROFILE_MUTEX_NAME)

            ProfileRegKey = Nothing
        Else
            NewCode(False) 'Normal behaviour so call common code respecting exceptions
        End If
    End Sub

    Sub New(ByVal p_IgnoreChecks As Boolean)
        NewCode(p_IgnoreChecks) 'Call the common code with the appropriate ignore flag
    End Sub

    ''' <summary>
    ''' Common code for the new method
    ''' </summary>
    ''' <param name="p_IgnoreChecks">If true, suppresses the exception normally thrown if a valid profile is not present</param>
    ''' <remarks></remarks>
    Sub NewCode(ByVal p_IgnoreChecks As Boolean)
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
            If DisableTLOnExit Then
                Try : TL.Enabled = False : Catch : End Try 'Clean up the logger
                Try : TL.Dispose() : Catch : End Try
                Try : TL = Nothing : Catch : End Try
            End If
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
            ReleaseProfileMutex("DeleteProfile")
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
            ReleaseProfileMutex("EnumKeys")
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
            ReleaseProfileMutex("EnumProfile")
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
            TL.LogMessage("  DefaultValue", "is nothing... " & (p_DefaultValue Is Nothing).ToString)
            RetVal = "" 'Initialise return value to null string
            Try
                RetVal = ProfileRegKey.OpenSubKey(CleanSubKey(p_SubKeyName)).GetValue(p_ValueName).ToString
                TL.LogMessage("  Value", """" & RetVal & """")
            Catch ex As NullReferenceException
                If Not (p_DefaultValue Is Nothing) Then 'We have been supplied a default value so set it and then return it
                    WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue)
                    RetVal = p_DefaultValue
                    TL.LogMessage("  Value", "Value not yet set, returning supplied default value: " & p_DefaultValue)
                Else
                    TL.LogMessage("  Value", "Value not yet set and no default value supplied, returning null string")
                End If
            Catch ex As Exception 'Any other exception
                If Not (p_DefaultValue Is Nothing) Then 'We have been supplied a default value so set it and then return it
                    WriteProfile(CleanSubKey(p_SubKeyName), CleanSubKey(p_ValueName), p_DefaultValue)
                    RetVal = p_DefaultValue
                    TL.LogMessage("  Value", "Key not yet set, returning supplied default value: " & p_DefaultValue)
                Else
                    TL.LogMessageCrLf("  Value", "Key not yet set and no default value supplied, throwing exception: " & ex.ToString)
                    Throw New ProfilePersistenceException("GetProfile Exception", ex)
                End If
            End Try

            sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Finally
            ReleaseProfileMutex("GetProfile")
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
            ReleaseProfileMutex("WriteProfile")
        End Try
    End Sub

    Friend Sub BackupProfile(ByVal CurrentPlatformVersion As String) Implements IAccess.MigrateProfile

        Try
            GetProfileMutex("BackupProfile", "")
            sw.Reset() : sw.Start() 'Start timing this call

            LogMessage("BackupProfile", "From platform: " & CurrentPlatformVersion & ", OS: " & [Enum].GetName(GetType(Bitness), OSBits()))

            Select Case CurrentPlatformVersion
                Case "" 'New installation
                    LogMessage("BackupProfile", "New installation so nothing to migrate")
                Case "4", "5" 'Currently on Platform 4 or 5 so Profile is in 32bit registry
                    'Profile just needs to be backed up 
                    LogMessage("BackupProfile", "Backing up Platform 5 Profile" & CurrentPlatformVersion)
                    Call Backup50() ' Take a backup copy to retore later
                Case "5.5" 'Currently on Platform 5.5 so Profile is in file system and there is some profile in the registry too
                    'Backup old 5.0 Profile and Copy 5.5 Profile to registry
                    Call Backup50()
                    Call Backup55()
                Case Else '6.0 or above, leave as is!
                    'Do nothing as Profile is already in the Registry
                    LogMessage("BackupProfile", "Profile reports previous Platform as " & CurrentPlatformVersion & " - no migration required")
            End Select

            sw.Stop() : LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Catch ex As Exception
            LogError("BackupProfile", "Exception: " & ex.ToString)
            Throw New ProfilePersistenceException("RegistryAccess.BackupProfile exception", ex)
        Finally
            ReleaseProfileMutex("BackupProfile")
        End Try
    End Sub

    Friend Sub RestoreProfile(ByVal CurrentPlatformVersion As String)

        Try
            GetProfileMutex("RestoreProfile", "")
            sw.Reset() : sw.Start() 'Start timing this call

            LogMessage("RestoreProfile", "From platform: " & CurrentPlatformVersion & ", OS: " & [Enum].GetName(GetType(Bitness), OSBits()))

            Select Case CurrentPlatformVersion
                Case "" 'New installation
                    LogMessage("RestoreProfile", "New installation so nothing to migrate")
                Case "4", "5" 'Currently on Platform 4 or 5 so Profile is in 32bit registry
                    'Profile just needs to be backed up 
                    LogMessage("RestoreProfile", "Restoring Platform 5 Profile" & CurrentPlatformVersion)
                    Call Restore50() ' Take a backup copy to retore later
                Case "5.5" 'Currently on Platform 5.5 so Profile is in file system and there is some profile in the registry too
                    'Backup old 5.0 Profile and Copy 5.5 Profile to registry
                    Call Restore55()
                Case Else '6.0 or above, leave as is!
                    'Do nothing as Profile is already in the Registry
                    LogMessage("RestoreProfile", "Profile reports previous Platform as " & CurrentPlatformVersion & " - no migration required")
            End Select

            'Make sure we have a valid key now that we have migrated the profile to the registry
            ProfileRegKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY)

            sw.Stop() : LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Catch ex As Exception
            LogError("RestoreProfile", "Exception: " & ex.ToString)
            Throw New ProfilePersistenceException("RegistryAccess.BackupProfile exception", ex)
        Finally
            ReleaseProfileMutex("RestoreProfile")
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
            ReleaseProfileMutex("GetProfile")
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
            ReleaseProfileMutex("SetProfile")
        End Try

    End Sub
#End Region

#Region "Support Functions"

    'log messages and send to screen when appropriate
    Private Sub LogMessage(ByVal section As String, ByVal logMessage As String)
        TL.LogMessageCrLf(section, logMessage) ' The CrLf version is used in order properly to format exception messages
        EventLogCode.LogEvent(section, logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.MigrateProfileRegistryKey, "")
    End Sub

    'log error messages and send to screen when appropriate
    Private Sub LogError(ByVal section As String, ByVal logMessage As String)
        TL.LogMessageCrLf(section, logMessage) ' The CrLf version is used in order properly to format exception messages
        EventLogCode.LogEvent(section, "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.MigrateProfileRegistryKey, logMessage)
    End Sub

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

        If ValueNames.Count = 0 Then ' No values found so add a single blank value in order to ensure that an empty key is recorded
            ProfileContents.SetValue("", "", SubKeyOffset)
        Else ' Some values exist so add them to the collection
            For Each ValueName As String In ValueNames
                Value = SKey.GetValue(ValueName).ToString
                ProfileContents.SetValue(ValueName, Value, SubKeyOffset)
            Next
        End If

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

    Private Sub CopyRegistry(ByVal FromKey As RegistryKey, ByVal ToKey As RegistryKey)
        'Subroutine used to recursively copy copy a registry Profile from one place to another
        Dim Value, ValueNames(), SubKeys() As String
        'Dim swLocal As Stopwatch
        Dim NewFromKey, NewToKey As RegistryKey
        Static RecurseDepth As Integer

        RecurseDepth += 1 'Increment the recursion depth indicator

        'swLocal = Stopwatch.StartNew
        LogMessage("CopyRegistry " & RecurseDepth.ToString, "Copy from: " & FromKey.Name & " to: " & ToKey.Name & " Number of values: " & FromKey.ValueCount.ToString & ", number of subkeys: " & FromKey.SubKeyCount.ToString)

        'First copy values from the from key to the to key
        ValueNames = FromKey.GetValueNames()
        For Each ValueName As String In ValueNames
            Value = FromKey.GetValue(ValueName, "").ToString
            ToKey.SetValue(ValueName, Value)
            LogMessage("CopyRegistry", ToKey.Name & " - """ & ValueName & """  """ & Value & """")
        Next

        'Now process the keys
        SubKeys = FromKey.GetSubKeyNames
        For Each SubKey As String In SubKeys
            Value = FromKey.OpenSubKey(SubKey).GetValue("", "").ToString
            'LogMessage("  CopyRegistry", "  Processing subkey: " & SubKey & " " & Value)
            NewFromKey = FromKey.OpenSubKey(SubKey) 'Create the new subkey and get a handle to it
            NewToKey = ToKey.CreateSubKey(SubKey) 'Create the new subkey and get a handle to it
            If Not Value = "" Then NewToKey.SetValue("", Value) 'Set the default value if present
            CopyRegistry(NewFromKey, NewToKey) 'Recursively process each key
        Next
        'swLocal.Stop() : LogMessage("  CopyRegistry", "  Completed subkey: " & FromKey.Name & " " & RecurseDepth.ToString & ",  Elapsed time: " & swLocal.ElapsedMilliseconds & " milliseconds")
        RecurseDepth -= 1 'Decrement the recursion depth counter
        'swLocal = Nothing
    End Sub

    Private Sub Backup50()
        Dim FromKey, ToKey As RegistryKey
        Dim swLocal As Stopwatch
        Dim PlatformVersion As String

        swLocal = Stopwatch.StartNew

        'FromKey = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME, True)
        FromKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, False, RegWow64Options.KEY_WOW64_32KEY)
        ToKey = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_5_BACKUP_SUBKEY)
        PlatformVersion = ToKey.GetValue("PlatformVersion", "").ToString ' Test whether we have already backed up the profile
        If String.IsNullOrEmpty(PlatformVersion) Then
            LogMessage("Backup50", "Backup PlatformVersion not found, backing up Profile 5 to " & REGISTRY_5_BACKUP_SUBKEY)
            CopyRegistry(FromKey, ToKey)
        Else
            LogMessage("Backup50", "Platform 5 backup found at HKCU\" & REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_5_BACKUP_SUBKEY & ", no further action taken.")
        End If

        FromKey.Close() 'Close the key after migration
        ToKey.Close()

        swLocal.Stop() : LogMessage("Backup50", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Friend Sub ListRegistryACLs(Key As RegistryKey, Description As String)
        Dim KeySec As RegistrySecurity, RuleCollection As AuthorizationRuleCollection

        LogMessage("ListRegistryACLs", Description & ", Key: " & Key.Name)
        KeySec = Key.GetAccessControl() ' Get existing ACL rules on the key 

        RuleCollection = KeySec.GetAccessRules(True, True, GetType(NTAccount)) 'Get the access rules

        For Each RegRule As RegistryAccessRule In RuleCollection 'Iterate over the rule set and list them
            LogMessage("ListRegistryACLs", RegRule.AccessControlType.ToString() & " " &
                                           RegRule.IdentityReference.ToString() & " " &
                                           CType(RegRule.RegistryRights, AccessRights).ToString() & " " &
                                           IIf(RegRule.IsInherited, "Inherited", "NotInherited").ToString() & " " &
                                           RegRule.InheritanceFlags.ToString() & " " &
                                           RegRule.PropagationFlags.ToString())
        Next
        TL.BlankLine()
    End Sub

    Friend Sub SetRegistryACL() 'ByVal CurrentPlatformVersion As String)
        'Subroutine to control the migration of a Platform 5.5 profile to Platform 6
        Dim Values As New Generic.SortedList(Of String, String)
        Dim swLocal As Stopwatch
        Dim Key As RegistryKey, KeySec As RegistrySecurity, RegAccessRule As RegistryAccessRule
        Dim DomainSid, Ident As SecurityIdentifier
        Dim RuleCollection As AuthorizationRuleCollection

        swLocal = Stopwatch.StartNew

        'Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key

        LogMessage("SetRegistryACL", "Creating security identifier")
        DomainSid = New SecurityIdentifier("S-1-0-0") 'Create a starting point domain SID
        Ident = New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid) 'Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule

        LogMessage("SetRegistryACL", "Creating FullControl ACL rule")
        RegAccessRule = New RegistryAccessRule(Ident,
                                               RegistryRights.FullControl,
                                               InheritanceFlags.ContainerInherit,
                                               PropagationFlags.None,
                                               AccessControlType.Allow) ' Create the new access permission rule

        ' List the ACLs on the standard registry keys before we do anything
        LogMessage("SetRegistryACL", "Listing base key ACLs")

        If ApplicationBits() = Bitness.Bits64 Then
            LogMessage("SetRegistryACL", "Listing base key ACLs in 64bit mode")
            ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT")
            ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE"), "HKEY_LOCAL_MACHINE\SOFTWARE")
            ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft"), "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft")
            ListRegistryACLs(OpenSubKey(Registry.LocalMachine, "SOFTWARE", True, RegWow64Options.KEY_WOW64_64KEY), "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node")
            ListRegistryACLs(OpenSubKey(Registry.LocalMachine, "SOFTWARE\Microsoft", True, RegWow64Options.KEY_WOW64_32KEY), "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft")
        Else
            LogMessage("SetRegistryACL", "Listing base key ACLS in 32bit mode")
            ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT")
            ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE"), "HKEY_LOCAL_MACHINE\SOFTWARE")
            ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft"), "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft")
        End If

        LogMessage("SetRegistryACL", "Creating root ASCOM key ""\""")
        Key = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY) ' Always create the key in the 32bit portion of the registry for backward compatibility

        LogMessage("SetRegistryACL", "Retrieving ASCOM key ACL rule")
        TL.BlankLine()
        KeySec = Key.GetAccessControl() ' Get existing ACL rules on the key 

        RuleCollection = KeySec.GetAccessRules(True, True, GetType(NTAccount)) 'Get the access rules

        For Each RegRule As RegistryAccessRule In RuleCollection 'Iterate over the rule set and list them
            LogMessage("SetRegistryACL Before", RegRule.AccessControlType.ToString() & " " &
                                                RegRule.IdentityReference.ToString() & " " &
                                                CType(RegRule.RegistryRights, AccessRights).ToString() & " " &
                                                IIf(RegRule.IsInherited, "Inherited", "NotInherited").ToString() & " " &
                                                RegRule.InheritanceFlags.ToString() & " " &
                                                RegRule.PropagationFlags.ToString())
        Next

        ' Ensure that the ACLs are canonical before writing them back.
        ' key.GetAccessControl sometimes provides ACLs in a non-canonical format, which causes failure when we write them back - UGH!

        TL.BlankLine()
        If KeySec.AreAccessRulesCanonical Then
            LogMessage("SetRegistryACL", "Current access rules on the ASCOM profile key are canonical, no fix-up action required")
        Else
            LogMessage("SetRegistryACL", "***** Current access rules on the ASCOM profile key are NOT canonical, fixing them")
            CanonicalizeDacl(KeySec) ' Ensure that the ACLs are canonical
            LogMessage("SetRegistryACL", "Access Rules are Canonical after fix: " & KeySec.AreAccessRulesCanonical)
        End If
        TL.BlankLine()

        ' List the rules post canonicalisation
        RuleCollection = KeySec.GetAccessRules(True, True, GetType(NTAccount)) 'Get the access rules
        For Each RegRule As RegistryAccessRule In RuleCollection 'Iterate over the rule set and list them
            LogMessage("SetRegistryACL Canon", RegRule.AccessControlType.ToString() & " " &
                                               RegRule.IdentityReference.ToString() & " " &
                                               CType(RegRule.RegistryRights, AccessRights).ToString() & " " &
                                               IIf(RegRule.IsInherited, "Inherited", "NotInherited").ToString() & " " &
                                               RegRule.InheritanceFlags.ToString() & " " &
                                               RegRule.PropagationFlags.ToString())
        Next

        LogMessage("SetRegistryACL", "Adding new ACL rule")
        KeySec.AddAccessRule(RegAccessRule) 'Add the new rule to the existing rules
        LogMessage("SetRegistryACL", "Access Rules are Canonical after adding full access rule: " & KeySec.AreAccessRulesCanonical)
        TL.BlankLine()
        RuleCollection = KeySec.GetAccessRules(True, True, GetType(NTAccount)) 'Get the access rules after adding the new one

        For Each RegRule As RegistryAccessRule In RuleCollection 'Iterate over the rule set and list them
            LogMessage("SetRegistryACL After", RegRule.AccessControlType.ToString() & " " &
                                               RegRule.IdentityReference.ToString() & " " &
                                               CType(RegRule.RegistryRights, AccessRights).ToString() & " " &
                                               IIf(RegRule.IsInherited, "Inherited", "NotInherited").ToString() & " " &
                                               RegRule.InheritanceFlags.ToString() & " " &
                                               RegRule.PropagationFlags.ToString())
        Next

        TL.BlankLine()
        LogMessage("SetRegistryACL", "Applying new ACL rule to the Profile key")
        Key.SetAccessControl(KeySec) 'Apply the new rules to the Profile key

        LogMessage("SetRegistryACL", "Flushing key")
        Key.Flush() 'Flush the key to make sure the permission is committed
        LogMessage("SetRegistryACL", "Closing key")
        Key.Close() 'Close the key after migration

        swLocal.Stop() : LogMessage("SetRegistryACL", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Friend Sub CanonicalizeDacl(objectSecurity As RegistrySecurity)

        ' A canonical ACL must have ACES sorted according to the following order:
        '   1. Access-denied on the object
        '   2. Access-denied on a child or property
        '   3. Access-allowed on the object
        '   4. Access-allowed on a child or property
        '   5. All inherited ACEs 
        Dim descriptor As New RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access))

        Dim implicitDenyDacl As New List(Of CommonAce)()
        Dim implicitDenyObjectDacl As New List(Of CommonAce)()
        Dim inheritedDacl As New List(Of CommonAce)()
        Dim implicitAllowDacl As New List(Of CommonAce)()
        Dim implicitAllowObjectDacl As New List(Of CommonAce)()
        Dim aceIndex As Integer = 0
        Dim newDacl As New RawAcl(descriptor.DiscretionaryAcl.Revision, descriptor.DiscretionaryAcl.Count)
        Dim count As Integer = 0 ' Counter for ACEs as they are processed

        Try

            If objectSecurity Is Nothing Then
                Throw New ArgumentNullException("objectSecurity")
            End If
            If objectSecurity.AreAccessRulesCanonical Then
                LogMessage("CanonicalizeDacl", "Rules are already canonical, no action taken")
                Return
            End If

            TL.BlankLine()
            LogMessage("CanonicalizeDacl", "***** Rules are not canonical, restructuring them *****")
            TL.BlankLine()

            For Each ace As CommonAce In descriptor.DiscretionaryAcl
                count += 1
                LogMessage("CanonicalizeDacl", "Processing ACE " & count)

                If (ace.AceFlags And AceFlags.Inherited) = AceFlags.Inherited Then
                    Try
                        LogMessage("CanonicalizeDacl", "Found Inherited Ace")
                        LogMessage("CanonicalizeDacl", "Found Inherited Ace,                  " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", "Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                        inheritedDacl.Add(ace)
                    Catch ex1 As Exception
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan")
                        LogError("CanonicalizeDacl", ex1.ToString())
                    End Try
                Else
                    Select Case ace.AceType
                        Case AceType.AccessAllowed
                            Try
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Allow")
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace,               Allow: " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                                implicitAllowDacl.Add(ace)
                            Catch ex1 As IdentityNotMappedException
                                LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan")
                                LogError("CanonicalizeDacl", ex1.ToString())
                            End Try
                            Exit Select

                        Case AceType.AccessDenied
                            Try
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace Deny")
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace,                Deny: " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                                implicitDenyDacl.Add(ace)
                            Catch ex1 As IdentityNotMappedException
                                LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan")
                                LogError("CanonicalizeDacl", ex1.ToString())
                            End Try
                            Exit Select

                        Case AceType.AccessAllowedObject
                            Try
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object Allow")
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object        Allow:" & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                                implicitAllowObjectDacl.Add(ace)
                            Catch ex1 As IdentityNotMappedException
                                LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan")
                                LogError("CanonicalizeDacl", ex1.ToString())
                            End Try
                            Exit Select

                        Case AceType.AccessDeniedObject
                            Try
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object Deny")
                                LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object         Deny: " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                                implicitDenyObjectDacl.Add(ace)
                                Exit Select
                            Catch ex1 As IdentityNotMappedException
                                LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan")
                                LogError("CanonicalizeDacl", ex1.ToString())
                            End Try
                    End Select
                End If
            Next

            TL.BlankLine()
            LogMessage("CanonicalizeDacl", "Rebuilding in correct order...")
            TL.BlankLine()

            'List the number of entries in each category
            LogMessage("CanonicalizeDacl", String.Format("There are {0} Implicit Deny ACLs", implicitDenyDacl.Count))
            LogMessage("CanonicalizeDacl", String.Format("There are {0} Implicit Deny Object ACLs", implicitDenyObjectDacl.Count))
            LogMessage("CanonicalizeDacl", String.Format("There are {0} Implicit Allow ACLs", implicitAllowDacl.Count))
            LogMessage("CanonicalizeDacl", String.Format("There are {0} Implicit Allow Object ACLs", implicitAllowObjectDacl.Count))
            LogMessage("CanonicalizeDacl", String.Format("There are {0} Inherited ACLs", inheritedDacl.Count))
            TL.BlankLine()

            ' Rebuild the access list in the correct order
            For Each ace As CommonAce In implicitDenyDacl
                Try
                    LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Deny Ace")
                    newDacl.InsertAce(aceIndex, ace)
                    aceIndex += 1
                    LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Deny Ace,         " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", " Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                Catch ex1 As IdentityNotMappedException
                    LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE")
                    LogError("CanonicalizeDacl", ex1.ToString())
                End Try
            Next

            For Each ace As CommonAce In implicitDenyObjectDacl
                Try
                    LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Deny Object Ace")
                    newDacl.InsertAce(aceIndex, ace)
                    aceIndex += 1
                    LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Deny Object Ace,  " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", " Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                Catch ex1 As IdentityNotMappedException
                    LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE")
                    LogError("CanonicalizeDacl", ex1.ToString())
                End Try
            Next

            For Each ace As CommonAce In implicitAllowDacl
                Try
                    LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Allow Ace")
                    newDacl.InsertAce(aceIndex, ace)
                    aceIndex += 1
                    LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Allow Ace,        " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", " Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                Catch ex1 As IdentityNotMappedException
                    LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE")
                    LogError("CanonicalizeDacl", ex1.ToString())
                End Try
            Next

            For Each ace As CommonAce In implicitAllowObjectDacl
                Try
                    LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Allow Object Ace")
                    newDacl.InsertAce(aceIndex, ace)
                    aceIndex += 1
                    LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Allow Object Ace, " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", " Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                Catch ex1 As IdentityNotMappedException
                    LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE")
                    LogError("CanonicalizeDacl", ex1.ToString())
                End Try
            Next

            For Each ace As CommonAce In inheritedDacl
                Try
                    LogMessage("CanonicalizeDacl", "Adding Inherited Ace")
                    newDacl.InsertAce(aceIndex, ace)
                    aceIndex += 1
                    LogMessage("CanonicalizeDacl", "Added Inherited Ace,                 " & IIf(ace.AceType = AceType.AccessAllowed, "Allow", " Deny").ToString() & ": " & ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() & " " & CType(ace.AccessMask, AccessRights).ToString().ToString() & " " & ace.AceFlags.ToString())
                Catch ex1 As IdentityNotMappedException
                    LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE")
                    LogError("CanonicalizeDacl", ex1.ToString())
                End Try
            Next

            'If aceIndex <> descriptor.DiscretionaryAcl.Count Then
            'System.Diagnostics.Debug.Fail("The DACL cannot be canonicalized since it would potentially result in a loss of information")
            'Return
            'End If

            descriptor.DiscretionaryAcl = newDacl
            objectSecurity.SetSecurityDescriptorSddlForm(descriptor.GetSddlForm(AccessControlSections.Access), AccessControlSections.Access)
        Catch ex2 As Exception
            LogError("CanonicalizeDacl", "Unexpected exception")
            LogError("CanonicalizeDacl", ex2.ToString())
            Throw
        End Try
    End Sub

    Private Sub Backup55()
        Dim Prof55 As XMLAccess
        Dim Key As RegistryKey

        LogMessage("Backup55", "Creating Profile 5.5 XMLAccess object")
        Prof55 = New XMLAccess()

        LogMessage("Backup55", "Opening " & REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_55_BACKUP_SUBKEY & " Registry Key")
        Key = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_55_BACKUP_SUBKEY)

        LogMessage("Backup55", "Backing up Platform 5.5 Profile")
        Copy55("", Prof55, Key)

        'Clean up objects
        Key.Flush()
        Key.Close()
        Key = Nothing
        Prof55.Dispose()
        Prof55 = Nothing

        LogMessage("Backup55", "Completed copying the Profile")

    End Sub

    Private Sub Copy55(ByVal CurrentSubKey As String, ByVal Prof55 As XMLAccess, ByVal RegistryTarget As RegistryKey)
        'Subroutine used to recursively copy copy the 5.5 XML profile to new 64bit registry profile
        Dim Values, SubKeys As Generic.SortedList(Of String, String)
        Dim swLocal As Stopwatch
        Static RecurseDepth As Integer

        RecurseDepth += 1 'Increment the recursion depth indicator

        swLocal = Stopwatch.StartNew
        LogMessage("Copy55ToRegistry " & RecurseDepth.ToString, "Starting key: " & CurrentSubKey)

        'First copy values from the from key to the to key
        Values = Prof55.EnumProfile(CurrentSubKey)
        For Each Value As Generic.KeyValuePair(Of String, String) In Values
            RegistryTarget.SetValue(Value.Key, Value.Value)
            LogMessage("  Copy55ToRegistry", "  Key: " & CurrentSubKey & " - """ & Value.Key & """  """ & Value.Value & """")
        Next
        RegistryTarget.Flush()

        'Now process the keys
        SubKeys = Prof55.EnumKeys(CurrentSubKey)
        Dim NewKey As RegistryKey
        For Each SubKey As Generic.KeyValuePair(Of String, String) In SubKeys
            LogMessage("  Copy55ToRegistry", "  Processing subkey: " & SubKey.Key & " " & SubKey.Value)
            NewKey = RegistryTarget.CreateSubKey(SubKey.Key) 'Create the new subkey and get a handle to it
            If Not SubKey.Value = "" Then NewKey.SetValue("", SubKey.Value) 'Set the default value if present
            Copy55(CurrentSubKey & "\" & SubKey.Key, Prof55, NewKey) 'Recursively process each key
            NewKey.Flush()
            NewKey.Close()
            NewKey = Nothing
        Next
        swLocal.Stop() : LogMessage("  Copy55ToRegistry", "  Completed subkey: " & CurrentSubKey & " " & RecurseDepth.ToString & ",  Elapsed time: " & swLocal.ElapsedMilliseconds & " milliseconds")
        RecurseDepth -= 1 'Decrement the recursion depth counter
        swLocal = Nothing
    End Sub

    Private Sub Restore50()
        Dim FromKey, ToKey As RegistryKey
        Dim swLocal As Stopwatch

        swLocal = Stopwatch.StartNew

        FromKey = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_5_BACKUP_SUBKEY)
        ToKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY)
        LogMessage("Restore50", "Restoring Profile 5 to " & ToKey.Name)
        CopyRegistry(FromKey, ToKey)
        FromKey.Close() 'Close the key after migration
        ToKey.Close()

        swLocal.Stop() : LogMessage("Restore50", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Private Sub Restore55()
        Dim FromKey, ToKey As RegistryKey
        Dim swLocal As Stopwatch

        swLocal = Stopwatch.StartNew

        FromKey = Registry.CurrentUser.OpenSubKey(REGISTRY_ROOT_KEY_NAME & "\" & REGISTRY_55_BACKUP_SUBKEY)
        ToKey = OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, True, RegWow64Options.KEY_WOW64_32KEY)
        LogMessage("Restore55", "Restoring Profile 5.5 to " & ToKey.Name)
        CopyRegistry(FromKey, ToKey)
        FromKey.Close() 'Close the key after migration
        ToKey.Close()

        swLocal.Stop() : LogMessage("Restore55", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub

    Private Sub GetProfileMutex(ByVal Method As String, ByVal Parameters As String)
        'Get the profile mutex or log an error and throw an exception that will terminate this profile call and return to the calling application
        Try
            'Try to acquire the mutex
            GotMutex = ProfileMutex.WaitOne(PROFILE_MUTEX_TIMEOUT, False)
            TL.LogMessage("GetProfileMutex", "Got Profile Mutex for " & Method)
            'Catch the AbandonedMutexException but not any others, these are passed to the calling routine
        Catch ex As System.Threading.AbandonedMutexException
            ' We've received this exception but it indicates an issue in a PREVIOUS thread not this one. Log it and we have also got the mutex; so continue!
            TL.LogMessage("GetProfileMutex", "***** WARNING ***** AbandonedMutexException in " & Method & ", parameters: " & Parameters)
            TL.LogMessageCrLf("AbandonedMutexException", ex.ToString)
            LogEvent("RegistryAccess", "AbandonedMutexException in " & Method & ", parameters: " & Parameters, EventLogEntryType.Error, EventLogErrors.RegistryProfileMutexAbandoned, ex.ToString)
            If GetBool(ABANDONED_MUTEXT_TRACE, ABANDONED_MUTEX_TRACE_DEFAULT) Then
                TL.LogMessage("RegistryAccess", "Throwing exception to application")
                LogEvent("RegistryAccess", "AbandonedMutexException in " & Method & ": Throwing exception to application", EventLogEntryType.Warning, EventLogErrors.RegistryProfileMutexAbandoned, Nothing)
                Throw 'Throw the exception in order to report it
            Else
                TL.LogMessage("RegistryAccess", "Absorbing exception, continuing normal execution")
                LogEvent("RegistryAccess", "AbandonedMutexException in " & Method & ": Absorbing exception, continuing normal execution", EventLogEntryType.Warning, EventLogErrors.RegistryProfileMutexAbandoned, Nothing)
                GotMutex = True 'Flag that we have got the mutex.
            End If
        End Try

        'Check whether we have the mutex, throw an error if not
        If Not GotMutex Then
            TL.LogMessage("GetProfileMutex", "***** WARNING ***** Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters)
            LogEvent(Method, "Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters, EventLogEntryType.Error, EventLogErrors.RegistryProfileMutexTimeout, Nothing)
            Throw New ProfilePersistenceException("Timed out waiting for Profile mutex in " & Method & ", parameters: " & Parameters)
        End If
    End Sub

    Sub ReleaseProfileMutex(ByVal Method As String)
        Try
            ProfileMutex.ReleaseMutex()
            TL.LogMessage("ReleaseProfileMutex", "Released Profile Mutex for " & Method)
        Catch ex As Exception
            TL.LogMessage("ReleaseProfileMutex", "Exception: " & ex.ToString)
            If GetBool(ABANDONED_MUTEXT_TRACE, ABANDONED_MUTEX_TRACE_DEFAULT) Then
                TL.LogMessage("ReleaseProfileMutex", "Release Mutex Exception in " & Method & ": Throwing exception to application")
                LogEvent("RegistryAccess", "Release Mutex Exception in " & Method & ": Throwing exception to application", EventLogEntryType.Error, EventLogErrors.RegistryProfileMutexAbandoned, ex.ToString)
                Throw 'Throw the exception in order to report it
            Else
                TL.LogMessage("ReleaseProfileMutex", "Release Mutex Exception in " & Method & ": Absorbing exception, continuing normal execution")
                LogEvent("RegistryAccess", "Release Mutex Exception in " & Method & ": Absorbing exception, continuing normal execution", EventLogEntryType.Error, EventLogErrors.RegistryProfileMutexAbandoned, ex.ToString)
            End If

        End Try
    End Sub

#Region "32/64bit registry access code"
    'There is a better way to achieve a 64bit registry view in framework 4. 
    'Only OpenSubKey is used in the code, the rest of these routines are support for OpenSubKey.
    'OpenSubKey should be replaced with Microsoft.Win32.RegistryKey.OpenBaseKey method

    '<Obsolete("Replace with Microsoft.Win32.RegistryKey.OpenBaseKey method in Framework 4", False)> _
    Friend Function OpenSubKey(ByVal ParentKey As RegistryKey, ByVal SubKeyName As String, ByVal Writeable As Boolean, ByVal Options As RegWow64Options) As RegistryKey
        Dim SubKeyHandle As Integer
        Dim Result As Integer

        If ParentKey Is Nothing OrElse GetRegistryKeyHandle(ParentKey).Equals(IntPtr.Zero) Then
            Throw New ProfilePersistenceException("OpenSubKey: Parent key is not open")
        End If

        Dim Rights As Integer = RegistryRights.ReadKey ' Or RegistryRights.EnumerateSubKeys Or RegistryRights.QueryValues Or RegistryRights.Notify
        If Writeable Then
            Rights = RegistryRights.WriteKey
            '                       hKey                             SubKey      Res lpClass     dwOpts samDesired     SecAttr      Handle        Disp
            Result = RegCreateKeyEx(GetRegistryKeyHandle(ParentKey), SubKeyName, 0, IntPtr.Zero, 0, Rights Or Options, IntPtr.Zero.ToInt32, SubKeyHandle, IntPtr.Zero.ToInt32)
        Else
            Result = RegOpenKeyEx(GetRegistryKeyHandle(ParentKey), SubKeyName, 0, Rights Or Options, SubKeyHandle)
        End If

        Select Case Result
            Case 0 'All OK so return result
                Return PointerToRegistryKey(CType(SubKeyHandle, IntPtr), Writeable, False, Options) ' Now pass the options as well for Framework 4 compatibility
            Case 2 'Key not found so return nothing
                Throw New ProfilePersistenceException("Cannot open key " & SubKeyName & " as it does not exist - Result: 0x" & Hex(Result))
            Case Else 'Some other error so throw an error
                Throw New ProfilePersistenceException("OpenSubKey: Exception encountered opening key - Result: 0x" & Hex(Result))
        End Select

    End Function

    Private Function PointerToRegistryKey(ByVal hKey As System.IntPtr, ByVal writable As Boolean, ByVal ownsHandle As Boolean, ByVal options As RegWow64Options) As Microsoft.Win32.RegistryKey
        ' Create a SafeHandles.SafeRegistryHandle from this pointer - this is a private class
        Dim privateConstructors, publicConstructors As System.Reflection.BindingFlags
        Dim safeRegistryHandleType As System.Type
        Dim safeRegistryHandleConstructorTypes As System.Type() = {GetType(System.IntPtr), GetType(System.Boolean)}
        Dim safeRegistryHandleConstructor As System.Reflection.ConstructorInfo
        Dim safeHandle As System.Object
        Dim registryKeyType As System.Type
        Dim registryKeyConstructor As System.Reflection.ConstructorInfo
        Dim result As Microsoft.Win32.RegistryKey

        publicConstructors = System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Public
        privateConstructors = System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic
        safeRegistryHandleType = GetType(Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle")
        safeRegistryHandleConstructor = safeRegistryHandleType.GetConstructor(publicConstructors, Nothing, safeRegistryHandleConstructorTypes, Nothing)
        safeHandle = safeRegistryHandleConstructor.Invoke(New System.Object() {hKey, ownsHandle})

        ' Create a new Registry key using the private constructor using the safeHandle - this should then behave like a .NET natively opened handle and disposed of correctly
        If System.Environment.Version.Major >= 4 Then ' Deal with MS having added a new parameter to the RegistryKey private costructor!!
            Dim RegistryViewType As System.Type = GetType(Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.RegistryView") ' This is the new parameter type
            Dim registryKeyConstructorTypes As System.Type() = {safeRegistryHandleType, GetType(System.Boolean), RegistryViewType} 'Add the extra paraemter to the list of parameter types
            registryKeyType = GetType(Microsoft.Win32.RegistryKey)
            registryKeyConstructor = registryKeyType.GetConstructor(privateConstructors, Nothing, registryKeyConstructorTypes, Nothing)
            result = DirectCast(registryKeyConstructor.Invoke(New Object() {safeHandle, writable, CInt(options)}), Microsoft.Win32.RegistryKey) ' Version 4 and later
        Else ' Only two paraemters for Frameworks 3.5 and below
            Dim registryKeyConstructorTypes As System.Type() = {safeRegistryHandleType, GetType(System.Boolean)}
            registryKeyType = GetType(Microsoft.Win32.RegistryKey)
            registryKeyConstructor = registryKeyType.GetConstructor(privateConstructors, Nothing, registryKeyConstructorTypes, Nothing)
            result = DirectCast(registryKeyConstructor.Invoke(New Object() {safeHandle, writable}), Microsoft.Win32.RegistryKey) ' Version 3.5 and earlier
        End If
        Return result
    End Function

    Private Function GetRegistryKeyHandle(ByVal RegisteryKey As RegistryKey) As IntPtr
        ' Basis from http://blogs.msdn.com/cumgranosalis/archive/2005/12/09/Win64RegistryPart1.aspx
        Dim Type As Type = Type.GetType("Microsoft.Win32.RegistryKey")
        Dim Info As Reflection.FieldInfo = Type.GetField("hkey", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)

        Dim Handle As Runtime.InteropServices.SafeHandle = CType(Info.GetValue(RegisteryKey), Runtime.InteropServices.SafeHandle)

        Return Handle.DangerousGetHandle
    End Function

    Friend Enum RegWow64Options As Integer
        ' Basis from: http://www.pinvoke.net/default.aspx/advapi32/RegOpenKeyEx.html
        None = 0
        KEY_WOW64_64KEY = &H100
        KEY_WOW64_32KEY = &H200
    End Enum

    Private Declare Auto Function RegOpenKeyEx Lib "advapi32.dll" (ByVal hKey As IntPtr,
                                                                   ByVal lpSubKey As String,
                                                                   ByVal ulOptions As Integer,
                                                                   ByVal samDesired As Integer,
                                                                   ByRef phkResult As Integer) As Integer


    Private Declare Auto Function RegCreateKeyEx Lib "advapi32.dll" (ByVal hKey As IntPtr,
                                                                     ByVal lpSubKey As String,
                                                                     ByVal Reserved As Integer,
                                                                     ByVal lpClass As IntPtr,
                                                                     ByVal dwOptions As Integer,
                                                                     ByVal samDesired As Integer,
                                                                     ByVal lpSecurityAttributes As Integer,
                                                                     ByRef phkResult As Integer,
                                                                     ByVal lpdwDisposition As Integer) As Integer

#End Region

#End Region

End Class
