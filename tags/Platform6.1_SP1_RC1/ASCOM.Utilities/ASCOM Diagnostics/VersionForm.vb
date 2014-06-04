Imports ASCOM.Utilities
Imports Microsoft.Win32

Public Class VersionForm

    Private Sub VersionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ProfileKey As RegistryKey

        Try

            Using Profile As New RegistryAccess
                ProfileKey = Profile.OpenSubKey(Registry.LocalMachine, "SOFTWARE\ASCOM\Platform", False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)

                NameLbl.Text = ProfileKey.GetValue("Platform Name", "Unknown Name")
                Version.Text = ProfileKey.GetValue("Platform Version", "Unknown Version")

                ProfileKey.Close()
            End Using

        Catch ex As Exception
            LogEvent("DiagnosticsVersionForm", "Exception", EventLogEntryType.Error, EventLogErrors.DiagnosticsLoadException, ex.ToString)
            MsgBox("Unexpected exception loading Version Form: " & ex.ToString, MsgBoxStyle.Critical, "Version load error")
        End Try

    End Sub
End Class