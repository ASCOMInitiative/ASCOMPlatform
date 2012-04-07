Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities
Imports ASCOM.TEMPLATEDEVICENAME

<ComVisible(False)> _
Public Class SetupDialogForm
    Dim driverProfile As Profile

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click ' OK button event handler
        ' Persist new values of user settings to the ASCOM profile
        driverProfile.WriteValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.comPortProfileName, textBox1.Text)
        driverProfile.WriteValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.traceLevelProfileName, chkTrace.Checked.ToString())
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click 'Cancel button event handler
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ShowAscomWebPage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick, PictureBox1.Click
        ' Click on ASCOM logo event handler
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

    Private Sub SetupDialogForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ' Form load event handler
        ' Retrieve current values of user settings from the ASCOM Profile 
        driverProfile = New Profile()
        driverProfile.DeviceType = "TEMPLATEDEVICECLASS"
        textBox1.Text = driverProfile.GetValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.comPortProfileName, "", TEMPLATEDEVICECLASS.comPortDefault)
        chkTrace.Checked = Convert.ToBoolean(driverProfile.GetValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.traceLevelProfileName, "", TEMPLATEDEVICECLASS.traceLevelDefault))
    End Sub

    Private Sub SetupDialogForm_Closed(sender As System.Object, e As System.EventArgs) Handles MyBase.FormClosed 'Form closed event handler
        driverProfile.Dispose()
        driverProfile = Nothing
    End Sub

End Class
