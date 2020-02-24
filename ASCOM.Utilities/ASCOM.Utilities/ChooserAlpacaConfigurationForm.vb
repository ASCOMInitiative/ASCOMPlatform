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
        ChkListAllDiscoveredDevices.Checked = chooserForm.AlpacaShowDiscoveredDevices
        ChkShowDeviceDetails.Checked = chooserForm.AlpacaShowDeviceDetails
        NumExtraChooserWidth.Value = Convert.ToDecimal(chooserForm.AlpacaChooserIncrementalWidth)
        ChkShowCreateNewAlpacaDriverMessage.Checked = Not GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT)

        ' Set the IP v4 / v6 radio boxes
        If chooserForm.AlpacaUseIpV4 And chooserForm.AlpacaUseIpV6 Then '// Both Then IPv4 And v6 are enabled so Set the "both" button
            RadIpV4AndV6.Checked = True
        Else ' Only one of v4 Or v6 Is enabled so set accordingly 
            RadIpV4.Checked = chooserForm.AlpacaUseIpV4
            RadIpV6.Checked = chooserForm.AlpacaUseIpV6
        End If

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
        chooserForm.AlpacaShowDiscoveredDevices = ChkListAllDiscoveredDevices.Checked
        chooserForm.AlpacaShowDeviceDetails = ChkShowDeviceDetails.Checked
        chooserForm.AlpacaChooserIncrementalWidth = Convert.ToInt32(NumExtraChooserWidth.Value)
        SetName(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, (Not ChkShowCreateNewAlpacaDriverMessage.Checked).ToString())

        ' Set the IP v4 And v6 variables as necessary
        If (RadIpV4.Checked) Then  ' The Then IPv4 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            chooserForm.AlpacaUseIpV4 = True
            chooserForm.AlpacaUseIpV6 = False
        End If

        If (RadIpV6.Checked) Then  ' The Then IPv6 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            chooserForm.AlpacaUseIpV4 = False
            chooserForm.AlpacaUseIpV6 = True
        End If

        If (RadIpV4AndV6.Checked) Then ' The Then IPv4 And IPV6 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            chooserForm.AlpacaUseIpV4 = True
            chooserForm.AlpacaUseIpV6 = True
        End If

        ' Indicate success so that the Chooser can persist the values
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