Public Class CheckedMessageBox
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Initialise the state of the suppress dialogue checkbox
        ChkDoNotShowAgain.Checked = GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT)
        Me.CenterToParent()

    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ChkDoNotShowAgain_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDoNotShowAgain.CheckedChanged
        'The checkbox has been clicked so record the new value
        SetName(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, ChkDoNotShowAgain.Checked.ToString)
    End Sub
End Class