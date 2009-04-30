Option Strict On
Option Explicit On
Imports System.IO
Imports ASCOM.HelperNET.Interfaces
Friend Class AllUsersFileSystemProvider
    'Implements the filestore mechanic to store ASCOM profiles

    'This implementation stores files in the All Users profile area that is accessible to everyone and so 
    'creates a "per machine" store rather than a "per user" store.

    Implements IFileStoreProvider

    Private Const ASCOM_DIRECTORY As String = "\ASCOM\Profile" 'Root directory within the supplied file system space

    Private BaseFolder As String

#Region "New"
    Friend Sub New()
        'Find the location of the All Users profile
        BaseFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & ASCOM_DIRECTORY
    End Sub
#End Region

#Region "IFileStore Implementation"
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

    Friend Sub CreateDirectory(ByVal p_SubKeyName As String) Implements IFileStoreProvider.CreateDirectory
        'Creates a directory in the supplied path (p_SubKeyName)
        Directory.CreateDirectory(CreatePath(p_SubKeyName))
    End Sub

    Friend Sub DeleteDirectory(ByVal p_SubKeyName As String) Implements IFileStoreProvider.DeleteDirectory
        'Deletes a directory specified by p_SubKeyName
        Directory.Delete(CreatePath(p_SubKeyName), True)
    End Sub

    Friend Sub EraseFileStore() Implements IFileStoreProvider.EraseFileStore
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you wish to erase the HelperNET profile store?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.HelperNET")
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
