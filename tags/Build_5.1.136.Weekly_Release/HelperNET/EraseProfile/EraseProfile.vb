Imports System.IO
Public Class EraseProfile

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Const ASCOM_DIRECTORY As String = "\ASCOM\Profile" 'Root directory within the supplied file system space
        Dim BaseFolder As String

        'Find the location of the All Users profile
        BaseFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & ASCOM_DIRECTORY
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you wish to erase the HelperNET profile store?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Critical, "ASCOM.HelperNET")
        If Response = MsgBoxResult.Ok Then
            Try : Directory.Delete(BaseFolder, True) : Catch : End Try
        End If
        End
    End Sub
End Class
