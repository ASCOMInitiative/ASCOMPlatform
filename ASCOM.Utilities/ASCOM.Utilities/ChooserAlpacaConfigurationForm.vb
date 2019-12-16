Public Class ChooserAlpacaConfigurationForm
    Private chooserForm As ChooserForm

    ''' <summary>
    ''' Initialiser enabling the Chooser to pass in a reference to itself so that it's variables can be accessed
    ''' </summary>
    ''' <param name="chooser"></param>
    Friend Sub New(chooser As ChooserForm)

        ' This call is required by the designer.
        InitializeComponent()

        ' Save the supplied reference to the Chooser for use in the form load and OK button events
        chooserForm = chooser

    End Sub

    ''' <summary>
    ''' Form load event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ChooserAlpacaConfigurationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Initialise controls to the values held in the Chooser
        NumDiscoveryBroadcasts.Value = Convert.ToDecimal(chooserForm.AlpacaNumberOfBroadcasts)
        NumDiscoveryDuration.Value = Convert.ToDecimal(chooserForm.AlpacaTimeout)
        NumDiscoveryIpPort.Value = Convert.ToDecimal(chooserForm.AlpacaDiscoveryPort)
        ChkDNSResolution.Checked = chooserForm.AlpacaDnsResolution

    End Sub

    ''' <summary>
    ''' OK button event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click

        ' User clicked OK so save the new values to the Chooser's variables 
        chooserForm.AlpacaNumberOfBroadcasts = Convert.ToInt32(NumDiscoveryBroadcasts.Value)
        chooserForm.AlpacaTimeout = Convert.ToInt32(NumDiscoveryDuration.Value)
        chooserForm.AlpacaDiscoveryPort = Convert.ToInt32(NumDiscoveryIpPort.Value)
        chooserForm.AlpacaDnsResolution = ChkDNSResolution.Checked

        ' Indicate success so that he Chooser can persist the values
        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    ''' <summary>
    ''' Cancel button event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click

        ' User clicked Cancel so changes will not be made to the Chooser variables nor will they be persisted
        Me.DialogResult = DialogResult.Cancel
        Me.Close()

    End Sub
End Class