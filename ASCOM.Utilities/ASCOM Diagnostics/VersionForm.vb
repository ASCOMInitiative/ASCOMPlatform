Imports ASCOM.Utilities
Imports Microsoft.Win32

Public Class VersionForm

    Private Sub VersionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim InstallInformation As SortedList(Of String, String)

        Try
            InstallInformation = DiagnosticsForm.GetInstallInformation(PLATFORM_INSTALLER_PROPDUCT_CODE, False, True, False) 'Retrieve the current install information

            NameLbl.Text = InstallInformation.Item(DiagnosticsForm.INST_DISPLAY_NAME)
            Version.Text = InstallInformation.Item(DiagnosticsForm.INST_DISPLAY_VERSION)

        Catch ex As Exception
            LogEvent("DiagnosticsVersionForm", "Exception", EventLogEntryType.Error, EventLogErrors.DiagnosticsLoadException, ex.ToString)
            MsgBox("Unexpected exception loading Version Form: " & ex.ToString, MsgBoxStyle.Critical, "Version load error")
        End Try

    End Sub
End Class