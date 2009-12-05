Imports ASCOM.Utilities
Imports ASCOM.Interface
Imports System.Windows.Forms
Public Class HandBoxForm
    Private lblSwitchNumber(NUM_SWITCHES - 1) As Label
    Private txtNames(NUM_SWITCHES - 1) As TextBox
    Private Led(NUM_SWITCHES - 1) As RadioButton
    Private cmdButton(NUM_SWITCHES - 1) As Button
    Private Sub ClickCmdButton(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim Prof As Interfaces.IProfile = New ASCOM.HelperNET.Profile
        Dim Prof As Interfaces.IProfile = New ASCOM.Utilities.Profile
        Dim btn As Button
        Dim s As String
        Dim i As Integer
        Dim res As Integer
        btn = CType(sender, Button)

        s = btn.Text
        For i = 0 To (NUM_SWITCHES - 1)
            res = String.Compare(s, g_sSwitchName(i))
            If res = 0 Then
                g_bSwitchState(i) = Not g_bSwitchState(i)
                Led(i).Checked = g_bSwitchState(i)
                Prof.DeviceType = "Switch"
                Prof.WriteValue(s_csDriverID, CStr(i), CStr(g_bSwitchState(i)), "SwitchState")
                Exit For
            End If
        Next
    End Sub
    Private Sub HandBoxForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Short
        Dim iTop As Integer
        Dim iLeft As Integer
        Dim iSpace As Integer
        iTop = 20
        iLeft = 100
        iSpace = 20
        For i = 0 To (NUM_SWITCHES - 1)

            cmdButton(i) = New Button
            cmdButton(i).Top = iTop + i * cmdButton(i).Height
            cmdButton(i).Left = iLeft + iSpace
            cmdButton(i).AutoSize = True
            Controls.Add(cmdButton(i))
            AddHandler cmdButton(i).Click, AddressOf Me.ClickCmdButton

            lblSwitchNumber(i) = New Label
            lblSwitchNumber(i).Top = 3 + iTop + i * cmdButton(i).Height
            lblSwitchNumber(i).Left = 5
            lblSwitchNumber(i).Text = "Switch number " & CStr(i)
            lblSwitchNumber(i).AutoSize = False
            Controls.Add(lblSwitchNumber(i))
            'txtNames(i) = New TextBox
            'txtNames(i).Top = iTop + i * txtNames(i).Height
            'txtNames(i).Left = iLeft
            'Controls.Add(txtNames(i))
            'txtNames(i).Text = g_sSwitchName(i)

            Led(i) = New RadioButton
            Led(i).Top = 3 + iTop + i * cmdButton(i).Height
            Led(i).Left = cmdButton(i).Right + iSpace
            Led(i).Text = ""
            Led(i).AutoSize = True
            Led(i).AutoCheck = False
            Controls.Add(Led(i))
        Next

        UpdateHandboxWindow()

    End Sub
    Public Sub UpdateHandboxWindow()
        Dim i As Integer
        Dim bVisible As Boolean
        For i = 0 To (NUM_SWITCHES - 1)
            bVisible = CBool(IIf(i <= g_iMaxSwitch, True, False))

            cmdButton(i).Text = g_sSwitchName(i)
            cmdButton(i).Visible = bVisible
            cmdButton(i).Enabled = g_bCanSetSwitch(i)
            Led(i).Checked = g_bSwitchState(i)


            Led(i).Visible = bVisible

            Led(i).Checked = g_bSwitchState(i)
        Next
        Led(0).Visible = g_bZero
        cmdButton(0).Visible = g_bZero
    End Sub

    Private Sub btnSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog()
    End Sub
End Class