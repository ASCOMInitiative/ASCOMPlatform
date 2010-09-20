'************************************************************************************************
'It is vital that this application by compiled as X86 as its job is to migrate the ASCOM Profile,
'which is stored in the 32bit part of the registry on 64bit systems. 
' Use of X86 allows the application transparent access to the 32bit registry.
'************************************************************************************************

Imports ASCOM.Utilities
Imports System.IO
Imports Microsoft.Win32

Module MigrateProfile

    Sub Main()
        Dim args() As String
        Dim PR As Profile
        Dim parmEraseOnly, parmForce, parmMigrateIfNeeded, parmSavePlatformVersion As Boolean
        Dim Utl As Util, Key As RegistryKey, CurrentProfileVersion As String

        Const LAST_PROFILE_VALUE_NAME As String = "LastPlatformVersion"
        Try
            'Test whether we are a 32bit application. If not then migration will fail so throw an error!
            If Not ApplicationBits() = Bitness.Bits32 Then Throw New ASCOM.Utilities.Exceptions.ProfilePersistenceException("The MigrateProfile application must be compiled as 32bit, but it appears not to be. Please report this to the ASCOM Platform team")

            'Process command line arguments if supplied
            args = System.Environment.GetCommandLineArgs

            'Initialise default values
            parmForce = False
            parmEraseOnly = False
            parmMigrateIfNeeded = False
            For Each arg As String In args
                If Len(arg) > 1 Then
                    Select Case UCase(Mid(arg, 2))
                        Case "FORCE" 'Migrate without prompts
                            parmForce = True
                        Case "ERASE" 'Erase the new profile only, do not migrate
                            parmEraseOnly = True
                        Case "MIGRATEIFNEEDED" '
                            parmMigrateIfNeeded = True
                        Case "SAVEPLATFORMVERSION"
                            parmSavePlatformVersion = True
                        Case Else
                    End Select
                End If
            Next

            LogEvent("MigrateProfile", "Force: " & parmForce & ", Erase: " & parmEraseOnly & ", MigrateifNeeded: " & parmMigrateIfNeeded & ", SavePlatformVersion: " & parmSavePlatformVersion, EventLogEntryType.Information, 0, Nothing)

            If parmSavePlatformVersion Then ' Just save the platform version
                Utl = New Util 'Get a Util object

                Key = Registry.CurrentUser.CreateSubKey(REGISTRY_ROOT_KEY_NAME) 'Create or open a registry key in which to save the value, use HKCU so we do have write accesto it by default
                Key.SetValue(LAST_PROFILE_VALUE_NAME, Utl.PlatformVersion, RegistryValueKind.String) ' Save the value
                Key.Close() 'flush the value to disk and close the key

                Key = Nothing
                Utl.Dispose() 'Dispose of the Util object
                Utl = Nothing
            Else 'Migrate the Profile if required
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
                    If Not parmEraseOnly Then PR.MigrateProfile(CurrentProfileVersion) ' Migrate the profile and set platform version
                Catch Ex2 As Exception
                    MsgBox("MigrateProfile - Unexpected migration exception: " & Ex2.ToString)
                End Try

                PR.Dispose() 'Clean up profile object
                PR = Nothing
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
End Module
