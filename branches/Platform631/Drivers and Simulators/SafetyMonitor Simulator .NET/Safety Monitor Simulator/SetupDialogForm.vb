Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

<ComVisible(False)> _
Public Class SetupDialogForm

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim Prof As ASCOM.Utilities.Interfaces.IProfile 'Create a profile component
        Prof = New Profile
        Prof.DeviceType = SAFETY_MONITOR 'Set up for safety monitor device

        g_CanEmergencyShutdown = chkCanEmergencyShutdown.Checked 'Save checkbox values to variables
        g_CanIsGood = chkCanIsGood.Checked

        'Write values to profile store
        Prof.WriteValue(s_csDriverID, CAN_EMERGENCY_SHUTDOWN, g_CanEmergencyShutdown.ToString)
        Prof.WriteValue(s_csDriverID, CAN_IS_GOOD, g_CanIsGood.ToString)

        Prof.Dispose() 'Clean up profile component
        Prof = Nothing

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel 'Close without saving checkbox values
        Me.Close()
    End Sub

    Private Sub ShowAscomWebPage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick, PictureBox1.Click
        Try
            System.Diagnostics.Process.Start("http://ascom-standards.org/")
        Catch noBrowser As System.ComponentModel.Win32Exception
            If noBrowser.ErrorCode = -2147467259 Then
                MessageBox.Show(noBrowser.Message)
            End If
        Catch other As System.Exception
            MessageBox.Show(other.Message)
        End Try
    End Sub

    Private Sub SetupDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkCanEmergencyShutdown.Checked = g_CanEmergencyShutdown 'Set the checkbox values
        chkCanIsGood.Checked = g_CanIsGood
    End Sub
End Class
