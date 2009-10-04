Imports System.IO
Imports ASCOM.Utilities
Imports System.Security.AccessControl
Imports System.Security.Principal

Module EraseProfile
    Sub Main()
        Const ASCOM_DIRECTORY As String = "\ASCOM" 'Root directory within the supplied file system space
        Const PROFILE_DIRECTORY As String = ASCOM_DIRECTORY & "\Profile"
        Dim ASCOMFolder, ProfileFolder As String
        Dim Response As MsgBoxResult
        Dim TL As New TraceLogger("", "EraseProfile")
        TL.Enabled = True
        TL.LogMessage("EraseProfile", "Program started")

        'Find the location of the All Users profile
        ASCOMFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & ASCOM_DIRECTORY
        ProfileFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & PROFILE_DIRECTORY
        TL.LogMessage("EraseProfile", "BaseFolder: " & ProfileFolder)

        Response = MsgBox("Are you sure you wish to erase the ASCOM Profile store?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
        If Response = MsgBoxResult.Ok Then
            TL.LogMessage("EraseProfile", "Received OK response")
            Try
                'If directory exists then erase it
                If Directory.Exists(ProfileFolder) Then
                    TL.LogMessage("EraseProfile", "Profile directory exists so erasing it")

                    'Set ACL Security
                    TL.LogMessage("EraseProfile", "Setting security on ASCOM Root directory")
                    Dim dInfo As New DirectoryInfo(ASCOMFolder)
                    Dim dSecurity As DirectorySecurity

                    Dim DomainSid As New SecurityIdentifier("S-1-0-0")
                    'Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule
                    Dim Ident As New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid)
                    TL.LogMessage("EraseProfile", "Creating security rules")
                    dSecurity = dInfo.GetAccessControl
                    dSecurity.AddAccessRule(New FileSystemAccessRule(Ident, FileSystemRights.Delete, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow))
                    dSecurity.AddAccessRule(New FileSystemAccessRule(Ident, FileSystemRights.DeleteSubdirectoriesAndFiles, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow))
                    TL.LogMessage("EraseProfile", "Added access rules")

                    dInfo.SetAccessControl(dSecurity)
                    TL.LogMessage("EraseProfile", "Successfully set security ACL!")
                    TL.LogMessage("EraseProfile", "Deleting folder...")
                    Directory.Delete(ProfileFolder, True)
                    TL.LogMessage("EraseProfile", "Folder deleted OK!")
                Else
                    TL.LogMessage("EraseProfile", "Profile directory does not exist so ignoring request")
                End If
            Catch ex As Exception
                TL.LogMessage("EraseProfile", "Exception: " & ex.ToString)
                MsgBox("EraseProfile - Unable to erase ASCOM Profile Exception: " & ex.ToString)
            End Try
        End If

        'Clean up trace logger
        TL.LogMessage("EraseProfile", "Program ended")
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing
    End Sub
End Module
