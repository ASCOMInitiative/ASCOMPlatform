'It is vital that this application by compiled as X86 as its job is to migrate the ASCOM Profile,
'which is stored in the 32bit part of the registry on 64bit systems. 
' Use of X86 allows the application transparent access to the 32bit registry.

Imports ASCOM.Utilities
Imports System.IO

Module MigrateProfile

    Sub Main()
        Dim args() As String
        Dim PR As Profile
        Dim Response As MsgBoxResult
        Dim BaseFolder As String
        Dim parmEraseOnly, parmForce, parmMigrateIfNeeded As Boolean

        Const ASCOM_DIRECTORY As String = "\ASCOM\Profile" 'Root directory within the supplied file system space

        Try

            args = System.Environment.GetCommandLineArgs

            'Initialise default values and process arguments
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
                        Case Else
                    End Select
                End If
            Next

            'Find the location of the All Users profile
            BaseFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & ASCOM_DIRECTORY
            Response = MsgBoxResult.Ok ' Inititalise to OK status

            If Not parmForce Then 'Deterine what to do
                If File.Exists(BaseFolder & "\Profile.xml") Then ' Profile already exists so check whether we really do want to migrate it!
                    If parmMigrateIfNeeded Then 'This is a setup run and profile is already migrated so no need to migrate again
                        Response = MsgBoxResult.Cancel
                    Else 'Manual run by user so ask what to to
                        If parmEraseOnly Then 'Ask the right question...
                            Response = MsgBox("Are you sure that you want to erase your migrated Profile, leaving your registry Profile intact?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
                        Else
                            Response = MsgBox("Your Profile has already been migrated and if you migrate again you will loose any changes you have made since it was originally migrated. Are you sure you wish to continue?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
                        End If
                    End If
                End If
            End If
            If Response = MsgBoxResult.Ok Then 'Migrate the profile; remove curent folder and copy from registry
                Try : Directory.Delete(BaseFolder, True) : Catch : End Try
                PR = Nothing 'Initialise to remove a compiler warning
                Try
                    PR = New Profile(True) 'Do not generate ProfileNotFound exception as we have'nt migrated yet!
                Catch Ex2 As Exception
                    MsgBox("MigrateProfile - Unexpected Ex2 Exception: " & Ex2.ToString)
                End Try

                If Not parmEraseOnly Then PR.MigrateProfile()

                PR.Dispose() 'Clean up profile object
                PR = Nothing
            End If
        Catch Ex1 As Exception
            MsgBox("MigrateProfile - Unexpected Ex1 Exception: " & Ex1.ToString)
        End Try


    End Sub

End Module
