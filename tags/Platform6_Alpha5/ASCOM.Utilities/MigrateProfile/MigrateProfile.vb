'************************************************************************************************
'It is vital that this application by compiled as X86 as its job is to migrate the ASCOM Profile,
'which is stored in the 32bit part of the registry on 64bit systems. 
' Use of X86 allows the application transparent access to the 32bit registry.
'************************************************************************************************

Imports ASCOM.Utilities
Imports System.IO
Imports Microsoft.Win32
Imports System.Security.AccessControl
Imports System.Security.Principal

Module MigrateProfile

    Sub Main()
        Dim args() As String
        Dim PR As Profile
        'Dim parmEraseOnly, parmForce, 
        Dim parmMigrateIfNeeded, parmSavePlatformVersion, parmCreateRegistryKey As Boolean
        Dim Utl As Util, Key As RegistryKey, CurrentProfileVersion As String

        Const LAST_PROFILE_VALUE_NAME As String = "LastPlatformVersion"
        Try
            'Test whether we are a 32bit application. If not then migration will fail so throw an error!
            If Not ApplicationBits() = Bitness.Bits32 Then Throw New ASCOM.Utilities.Exceptions.ProfilePersistenceException("The MigrateProfile application must be compiled as 32bit, but it appears not to be. Please report this to the ASCOM Platform team")

            'Process command line arguments if supplied
            args = System.Environment.GetCommandLineArgs

            'Initialise default values
            'parmForce = False
            'parmEraseOnly = False
            parmMigrateIfNeeded = False
            parmCreateRegistryKey = False
            parmSavePlatformVersion = False
            For Each arg As String In args
                'Select Case UCase(Mid(arg, 2))
                Select Case UCase(Trim(arg))
                    'Case "FORCE" 'Migrate without prompts
                    '    parmForce = True
                    'Case "ERASE" 'Erase the new profile only, do not migrate
                    '    parmEraseOnly = True
                    Case "MIGRATEPROFILE", "MIGRATEPROFILE.EXE" 'Name of executable so do nothing!
                    Case "MIGRATEIFNEEDED" '
                        parmMigrateIfNeeded = True
                    Case "SAVEPLATFORMVERSION"
                        parmSavePlatformVersion = True
                    Case "CREATEREGISTRYKEY"
                        parmCreateRegistryKey = True
                    Case Else 'Ignore migrate profile 0th parameter, otherwise give error message
                        If InStr(UCase(Trim(arg)), "MIGRATEPROFILE") = 0 Then MsgBox("Migrate: Unrecognised parameter - """ & arg & """ CREATEREGISTRYKEY")
                End Select
            Next
            LogEvent("MigrateProfile", "CreateRegistryKey: " & parmCreateRegistryKey & ", SavePlatformVersion: " & parmSavePlatformVersion & ", MigrateifNeeded: " & parmMigrateIfNeeded, EventLogEntryType.Information, 0, Nothing)
            If Not (parmCreateRegistryKey Or parmMigrateIfNeeded Or parmSavePlatformVersion) Then
                MsgBox("MirateProfile: No parameter supplied, requires - SAVEPLATFORMVERSION CREATEREGISTRYKEY MIGRATEIFNEEDED, these can be used in combination")
            Else
                If parmSavePlatformVersion Then ' Save the platform version
                    Try
                        Utl = New Util 'Get a Util object

                        Key = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME) 'Create or open a registry key in which to save the value, use HKCU so we do have write accesto it by default
                        Key.SetValue(LAST_PROFILE_VALUE_NAME, Utl.PlatformVersion, RegistryValueKind.String) ' Save the value
                        Key.Close() 'flush the value to disk and close the key

                        Key = Nothing
                        Utl.Dispose() 'Dispose of the Util object
                        Utl = Nothing
                    Catch ex As Exception ' Something went horriby wrong so platform may not be installed or it is corrupted so do not create a saved value
                    End Try
                End If

                If parmCreateRegistryKey Then ' Create registy key
                    Try
                        SetRegistryACL()
                    Catch ex As Exception
                        MsgBox("Exception creating Key: " & ex.ToString)
                    End Try
                End If

                If parmMigrateIfNeeded Then 'Migrate the Profile if required
                    'Get the version of the Profile last in use
                    Key = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME) 'Create or open a registry key in which to save the value, use HKCU so we do have write accesto it by default
                    CurrentProfileVersion = Key.GetValue(LAST_PROFILE_VALUE_NAME, "Unknown") ' Read the value
                    Key.Close() 'flush the value to disk and close the key
                    Key = Nothing

                    PR = Nothing 'Initialise to remove a compiler warning
                    Try
                        PR = New Profile(True) 'Do not generate ProfileNotFound exception as we have'nt migrated yet!
                    Catch Ex2 As Exception
                        MsgBox("MigrateProfile - Unexpected profile creation exception: " & Ex2.ToString)
                    End Try

                    Try
                        'If Not parmEraseOnly Then PR.MigrateProfile(CurrentProfileVersion) ' Migrate the profile and set platform version
                        PR.MigrateProfile(CurrentProfileVersion) ' Migrate the profile and set platform version
                    Catch Ex2 As Exception
                        MsgBox("MigrateProfile - Unexpected migration exception: " & Ex2.ToString)
                    End Try

                    PR.Dispose() 'Clean up profile object
                    PR = Nothing
                End If
            End If
        Catch Ex1 As Exception
            MsgBox("MigrateProfile - Unexpected overall migration exception: " & Ex1.ToString)
        End Try
    End Sub

    Sub Rubbish()
        ' If Not parmForce Then 'Deterine what to do
        ' If File.Exists(BaseFolder & "\Profile.xml") Then ' Profile already exists so check whether we really do want to migrate it!
        ' If parmMigrateIfNeeded Then 'This is a setup run and profile is already migrated so no need to migrate again
        ' 'Try and create a profile object, then ensure its platform level is what we require
        ' PR = Nothing

        'Try
        ' PR = New Profile()
        ' PR.MigrateProfile(CurrentProfileVersion, PLATFORM_VERSION, True) ' Just set the platform version number
        ' Response = MsgBoxResult.Cancel 'All worked OK so set the flag to show we don't need to migrate the profile
        ' Catch ex As Exception
        ' 'Ignore errors as this just means that we do need to properly migrate
        ' Finally
        ' Try : PR.Dispose() : Catch : End Try
        ' PR = Nothing
        ' End Try
        ' Else 'Manual run by user so ask what to to
        ' If parmEraseOnly Then 'Ask the right question...
        ' Response = MsgBox("Are you sure that you want to erase your migrated Profile, leaving your registry Profile intact?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
        ' Else
        ' Response = MsgBox("Your Profile has already been migrated and if you migrate again you will loose any changes you have made since it was originally migrated. Are you sure you wish to continue?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
        ' End If
        ' End If
        ' End If
        ' End If

    End Sub

    Private Sub SetRegistryACL()
        'Subroutine to control the migration of a Platform 5.5 profile to Platform 6
        Dim swLocal As Stopwatch
        Dim Key As RegistryKey, KeySec As RegistrySecurity, RegAccessRule As RegistryAccessRule
        Dim DomainSid, Ident As SecurityIdentifier

        swLocal = Stopwatch.StartNew

        'TL.LogMessage("MigrateTo60", "Creating root key ""\""")
        Key = Registry.LocalMachine.CreateSubKey(REGISTRY_ROOT_KEY_NAME)

        'Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key
        'TL.LogMessage("MigrateTo60", "Creating security identifier")
        DomainSid = New SecurityIdentifier("S-1-0-0") 'Create a starting point domain SID
        Ident = New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid) 'Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule

        'TL.LogMessage("MigrateTo60", "Creating new ACL rule")
        RegAccessRule = New RegistryAccessRule(Ident, _
                                               RegistryRights.FullControl, _
                                               InheritanceFlags.ContainerInherit, _
                                               PropagationFlags.None, _
                                               AccessControlType.Allow) ' Create the new access permission rule

        'TL.LogMessage("MigrateTo60", "Retrieving current ACL rule")
        KeySec = Key.GetAccessControl() ' Get existing ACL rules on the key 
        'TL.LogMessage("MigrateTo60", "Adding new ACL rule")
        KeySec.AddAccessRule(RegAccessRule) 'Add the new rule to the existing rules
        'TL.LogMessage("MigrateTo60", "Setting new ACL rule")
        Key.SetAccessControl(KeySec) 'Apply the new rules to the Profile key

        'TL.LogMessage("MigrateTo60", "Flushing key")
        Key.Flush() 'Flush the key to make sure the permission is committed
        'TL.LogMessage("MigrateTo60", "Closing key")
        Key.Close() 'Close the key after migration

        swLocal.Stop() 'TL.LogMessage("MigrateTo60", "ElapsedTime " & swLocal.ElapsedMilliseconds & " milliseconds")
        swLocal = Nothing
    End Sub


End Module
