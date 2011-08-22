Imports System.Windows.Forms
Imports System.Runtime.InteropServices

<ComVisible(False)> _
Public Class SetupDialogForm

	Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        My.MySettings.Default.ComPort = ComboBoxComPort.SelectedItem
        My.MySettings.Default.Save()
        Me.Close()
	End Sub

	Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        My.MySettings.Default.Reload()
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

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Using serial As New ASCOM.Utilities.Serial
            ComboBoxComPort.Items.Clear()
            For Each item In serial.AvailableCOMPorts
                ComboBoxComPort.Items.Add(item)
                If item = My.MySettings.Default.ComPort Then
                    ComboBoxComPort.SelectedIndex = ComboBoxComPort.Items.Count - 1
                End If
            Next
        End Using
    End Sub
End Class
