'Implements the filestore mechanic to store ASCOM profiles

'This implementation stores files in the All Users profile area that is accessible to everyone and so 
'creates a "per machine" store rather than a "per user" store.

Imports System.IO
Imports ASCOM.Utilities.Interfaces
Imports System.Security.AccessControl
Imports System.Security.Principal

Friend Class AllUsersFileSystemProvider

    Implements IFileStoreProvider
    Private Const ASCOM_DIRECTORY As String = "\ASCOM"
    Private Const PROFILE_DIRECTORY As String = ASCOM_DIRECTORY & "\Profile" 'Root directory within the supplied file system space

    Private BaseFolder, ASCOMFolder As String

#Region "New"
    Friend Sub New()
        'Find the location of the All Users profile
        ASCOMFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & ASCOM_DIRECTORY
        BaseFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & PROFILE_DIRECTORY
    End Sub
#End Region

#Region "IFileStore Implementation"
    Friend Sub SetSecurityACLs(ByVal p_tl As TraceLogger) Implements IFileStoreProvider.SetSecurityACLs
        Dim dInfo As New DirectoryInfo(ASCOMFolder) 'Apply to the ASCOM folder itself
        Dim dSecurity As DirectorySecurity

        'PWGS 5.5.2.0 Fix for users security group not being globally usable
        'Build a temp domainSID using the Null SID passed in as a SDDL string. The constructor will 
        'accept the traditional notation or the SDDL notation interchangeably.
        Dim DomainSid As New SecurityIdentifier("S-1-0-0")
        'Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule
        Dim Ident As New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid)

        p_tl.LogMessage("  SetSecurityACLs", "Retrieving access control")
        dSecurity = dInfo.GetAccessControl

        p_tl.LogMessage("  SetSecurityACLs", "Adding full control access rule")
        dSecurity.AddAccessRule(New FileSystemAccessRule(Ident, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))

        p_tl.LogMessage("  SetSecurityACLs", "Setting access control")
        dInfo.SetAccessControl(dSecurity)

        p_tl.LogMessage("  SetSecurityACLs", "Successfully set security ACL!")
    End Sub

    Friend ReadOnly Property Exists(ByVal p_FileName As String) As Boolean Implements IFileStoreProvider.Exists
        'Tests whether a file exists and returns a boolean value
        Get
            Try
                If File.Exists(CreatePath(p_FileName)) Then
                    Return True 'This was successful 
                Else
                    Return False 'No exception but no file returned so the file does not exist
                End If
            Catch ex As Exception
                MsgBox("Exists " & ex.ToString)
                Return False 'Exception so file doesn't exist
            End Try
        End Get
    End Property

    Friend Sub CreateDirectory(ByVal p_SubKeyName As String, ByVal p_TL As TraceLogger) Implements IFileStoreProvider.CreateDirectory
        'Creates a directory in the supplied path (p_SubKeyName)
        Try
            p_TL.LogMessage("  CreateDirectory", "Creating directory for: """ & p_SubKeyName & """")
            Directory.CreateDirectory(CreatePath(p_SubKeyName))
            p_TL.LogMessage("  CreateDirectory", "Created directory OK")
        Catch ex As Exception
            p_TL.LogMessage("FileSystem.CreateDirectory", "Exception: " & ex.ToString)
            MsgBox("CreateDirectory Exception: " & ex.ToString)
        End Try
    End Sub

    Friend Sub DeleteDirectory(ByVal p_SubKeyName As String) Implements IFileStoreProvider.DeleteDirectory
        'Deletes a directory specified by p_SubKeyName
        Directory.Delete(CreatePath(p_SubKeyName), True)
    End Sub

    Friend Sub EraseFileStore() Implements IFileStoreProvider.EraseFileStore
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you wish to erase the Utilities profile store?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.Utilities")
        If Response = MsgBoxResult.Ok Then
            Try : Directory.Delete(BaseFolder, True) : Catch : End Try
        End If
    End Sub

    Friend ReadOnly Property GetDirectoryNames(ByVal p_SubKeyName As String) As String() Implements IFileStoreProvider.GetDirectoryNames
        'Enumerates the sub directories within a directory specified by p_SubKeyName
        'Returns a string array of directory names
        Get
            Dim FullDirs() As String, ct As Integer, RelDirs() As String
            FullDirs = Directory.GetDirectories(CreatePath(p_SubKeyName))
            ct = 0
            For Each FullDir As String In FullDirs
                RelDirs = Split(FullDir, "\")
                FullDirs(ct) = RelDirs(RelDirs.Length - 1)
                ct += 1
            Next
            Return FullDirs
        End Get
    End Property

    Friend ReadOnly Property FullPath(ByVal p_FileName As String) As String Implements IFileStoreProvider.FullPath
        Get
            Return CreatePath(p_FileName)
        End Get
    End Property

    Friend ReadOnly Property BasePath() As String Implements IFileStoreProvider.BasePath
        Get
            Return BaseFolder
        End Get
    End Property

    Friend Sub Rename(ByVal p_CurrentName As String, ByVal p_NewName As String) Implements IFileStoreProvider.Rename
        File.Delete(CreatePath(p_NewName)) 'Make sure the target file doesn't exist
        File.Move(CreatePath(p_CurrentName), CreatePath(p_NewName))
    End Sub

    Friend Sub RenameDirectory(ByVal CurrentName As String, ByVal NewName As String) Implements IFileStoreProvider.RenameDirectory
        Try : Directory.Delete(CreatePath(NewName), True) : Catch : End Try 'Remove driectory if it already exists
        Dim DirInfo As New DirectoryInfo(CreatePath(CurrentName))
        DirInfo.MoveTo(CreatePath(NewName))
    End Sub

#End Region

#Region "Support Code"
    Private Function CreatePath(ByVal p_FileName As String) As String
        If Left(p_FileName, 1) <> "\" Then p_FileName = "\" & p_FileName
        Return BaseFolder & p_FileName
    End Function
#End Region

End Class