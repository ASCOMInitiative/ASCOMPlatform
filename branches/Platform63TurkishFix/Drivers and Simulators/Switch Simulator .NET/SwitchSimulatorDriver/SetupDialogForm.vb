Imports ASCOM.Utilities
Imports ASCOM.Interface
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

<ComVisible(False)> _
Public Class SetupDialogForm
    Private txtNames(NUM_SWITCHES - 1) As TextBox
    Private chkGet(NUM_SWITCHES - 1) As CheckBox
    Private chkSet(NUM_SWITCHES - 1) As CheckBox
    Private lblGet As Label
    Private lblSet As Label

    
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim i As Integer
        g_bZero = Me.chkZero.Checked
        g_iMaxSwitch = CShort(cbMaxSwitch.Text)
        For i = 0 To (NUM_SWITCHES - 1)
            g_sSwitchName(i) = txtNames(i).Text
            g_bCanGetSwitch(i) = chkGet(i).Checked
            g_bCanSetSwitch(i) = chkSet(i).Checked
        Next

        Using Prof = New ASCOM.Utilities.Profile() With {.DeviceType = "Switch"}
            Prof.WriteValue(s_csDriverID, "MaxSwitch", CStr(g_iMaxSwitch))
            Prof.WriteValue(s_csDriverID, "Zero", CStr(g_bZero))
            For i = 0 To (NUM_SWITCHES - 1)
                Prof.WriteValue(s_csDriverID, CStr(i), CStr(g_bCanGetSwitch(i)), "CanGetSwitch")
                Prof.WriteValue(s_csDriverID, CStr(i), CStr(g_bCanSetSwitch(i)), "CanSetSwitch")
                Prof.WriteValue(s_csDriverID, CStr(i), CStr(g_bSwitchState(i)), "SwitchState")
                Prof.WriteValue(s_csDriverID, CStr(i), g_sSwitchName(i), "SwitchName")
            Next i
        End Using
        


        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
        If frmHandbox Is Nothing Then
        Else

            frmHandbox.Visible = True
            frmHandbox.UpdateHandboxWindow()
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
        If frmHandbox Is Nothing Then
        Else

            frmHandbox.Visible = True
            frmHandbox.UpdateHandboxWindow()
        End If
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

    Private Sub SetupDialogForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Short
        Dim bVisible As Boolean
        Dim iTop As Integer
        Dim iLeft As Integer
        Dim iSpace As Integer

        If frmHandbox Is Nothing Then

        Else
            frmHandbox.Visible = False
        End If

        Me.cbMaxSwitch.SelectedIndex = Me.cbMaxSwitch.FindString(CStr(g_iMaxSwitch))

        Me.chkZero.Checked = CBool(IIf(g_bZero, 1, 0))

        iTop = 20
        iLeft = 20
        iSpace = 20
        For i = 0 To (NUM_SWITCHES - 1)
            txtNames(i) = New TextBox
            txtNames(i).Top = iTop + i * txtNames(i).Height
            txtNames(i).Left = iLeft
            Controls.Add(txtNames(i))
            txtNames(i).Text = g_sSwitchName(i)

            chkGet(i) = New CheckBox
            chkGet(i).Top = iTop + i * txtNames(i).Height
            chkGet(i).Left = txtNames(i).Right + iSpace
            chkGet(i).Text = ""
            chkGet(i).AutoSize = True
            Controls.Add(chkGet(i))
            chkGet(i).Checked = CBool(IIf(g_bCanGetSwitch(i), 1, 0))

            chkSet(i) = New CheckBox
            chkSet(i).Top = iTop + i * txtNames(i).Height
            chkSet(i).Left = chkGet(i).Left + 3 * iSpace
            chkSet(i).Text = ""
            chkSet(i).AutoSize = True
            Controls.Add(chkSet(i))

            chkGet(i).Checked = CBool(IIf(g_bCanGetSwitch(i), 1, 0))
            chkSet(i).Checked = CBool(IIf(g_bCanSetSwitch(i), 1, 0))

            bVisible = CBool(IIf(i <= g_iMaxSwitch, True, False))
            chkGet(i).Visible = bVisible
            chkSet(i).Visible = bVisible
            txtNames(i).Visible = bVisible
        Next
        chkGet(0).Visible = g_bZero
        chkSet(0).Visible = g_bZero
        txtNames(0).Visible = g_bZero

        lblGet = New Label
        lblGet.Top = 5
        lblGet.Left = txtNames(0).Right + iSpace
        lblGet.Text = "Get"
        lblGet.AutoSize = True
        Controls.Add(lblGet)

        lblSet = New Label
        lblSet.Top = 5
        lblSet.Left = chkGet(0).Left + 3 * iSpace
        lblSet.Text = "Set"
        lblSet.AutoSize = True
        Controls.Add(lblSet)
        lblDriverInfo.Text = My.Application.Info.Title & " " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Revision & vbCrLf & "Modified on "
    End Sub


    Private Sub cbMaxSwitch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbMaxSwitch.SelectedIndexChanged
        UpdateSetupWindow()
    End Sub

    Private Sub chkZero_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkZero.CheckStateChanged
        UpdateSetupWindow()
    End Sub
    Private Sub UpdateSetupWindow()
        Dim iMaxSwitch As Short ' temporary value, equivalent to g_iMaxSwitch
        Dim bZero As Boolean ' temporary value, equivalent to g_bZero
        Dim i As Integer
        Dim s As String
        Dim bVisible As Boolean
        s = Me.cbMaxSwitch.Text
        iMaxSwitch = CShort(s)
        For i = 0 To (NUM_SWITCHES - 1)

            bVisible = CBool(IIf(i <= iMaxSwitch, True, False))
            Try 'can be called before next objects are created
                chkGet(i).Visible = bVisible
                chkSet(i).Visible = bVisible
                txtNames(i).Visible = bVisible
            Catch
            End Try
        Next
        bZero = Me.chkZero.Checked
        Try 'can be called before next objects are created
            chkGet(0).Visible = bZero
            chkSet(0).Visible = bZero
            txtNames(0).Visible = bZero
        Catch
        End Try
    End Sub
End Class
