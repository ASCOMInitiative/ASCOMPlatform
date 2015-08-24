'---------------------------------------------------------------------
'   ==============
'   HandboxForm.vb
'   ==============
'
' ASCOM Dome Simulator hand box form
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 10-Sep-03 jab     label the traffic window
' 06-Dec-04 rbd     4.0.2 - New annunciator for SLEW, park and home now
'                   textual annunciator instead of LED.
' 21-Dec-04 rbd     4.0.3 - Increase journalling buffer size.
' 12-Jun-07 jab     cleaned up enabling of GUI buttons wrt Can flags
' 23-Jun-09 rbt     Port to Visual Basic .NET
' -----------------------------------------------------------------------------

Imports System.Windows.Forms
Imports ASCOM.DeviceInterface
Imports System.Globalization

<ComVisible(False)> _
Public Class HandboxForm
    Public ButtonState As Integer                         'Controls the dome slewing buttons

    'Friend Shared Sub Run()
    '    If g_handBox Is Nothing Then
    '        TL.LogMessage("HandboxForm.Run", "Handbox Variable is empty, Creating Form")
    '        g_handBox = New HandboxForm
    '        Application.Run(g_handBox)
    '    Else
    '        TL.LogMessage("HandboxForm.Run", "Handbox Variable is not empty, Doing nothing")
    '    End If
    'End Sub

#Region "Public Properties and Methods"

    Public Shared Sub UpdateConfig()
        Using p As New Utilities.Profile
            p.DeviceType = "Dome"


            p.WriteValue(ID, "OCDelay", CStr(g_dOCDelay))
            p.WriteValue(ID, "SetPark", CStr(g_dSetPark))
            p.WriteValue(ID, "SetHome", CStr(g_dSetHome))
            p.WriteValue(ID, "AltRate", CStr(g_dAltRate))
            p.WriteValue(ID, "AzRate", CStr(g_dAzRate))
            p.WriteValue(ID, "StepSize", CStr(g_dStepSize))
            p.WriteValue(ID, "MaxAlt", CStr(g_dMaxAlt))
            p.WriteValue(ID, "MinAlt", CStr(g_dMinAlt))
            p.WriteValue(ID, "StartShutterError", CStr(g_bStartShutterError))
            p.WriteValue(ID, "SlewingOpenClose", CStr(g_bSlewingOpenClose))
            p.WriteValue(ID, "NonFragileAtHome", CStr(Not g_bStandardAtHome))
            p.WriteValue(ID, "NonFragileAtPark", CStr(Not g_bStandardAtPark))

            p.WriteValue(ID, "DomeAz", CStr(g_dDomeAz), "State")
            p.WriteValue(ID, "DomeAlt", CStr(g_dDomeAlt), "State")

            p.WriteValue(ID, "ShutterState", CStr(g_eShutterState), "State")

            p.WriteValue(ID, "CanFindHome", CStr(g_bCanFindHome), "Capabilities")
            p.WriteValue(ID, "CanPark", CStr(g_bCanPark), "Capabilities")
            p.WriteValue(ID, "CanSetAltitude", CStr(g_bCanSetAltitude), "Capabilities")
            p.WriteValue(ID, "CanSetAzimuth", CStr(g_bCanSetAzimuth), "Capabilities")
            p.WriteValue(ID, "CanSetPark", CStr(g_bCanSetPark), "Capabilities")
            p.WriteValue(ID, "CanSetShutter", CStr(g_bCanSetShutter), "Capabilities")
            p.WriteValue(ID, "CanSyncAzimuth", CStr(g_bCanSyncAzimuth), "Capabilities")
        End Using
    End Sub

    Public Sub DoSetup()
        Timer1.Enabled = False

        Using SetupDialog As SetupDialogForm = New SetupDialogForm

            With SetupDialog
                .OCDelay = g_dOCDelay
                .AltRate = g_dAltRate
                .AzRate = g_dAzRate
                .StepSize = g_dStepSize
                .MaxAlt = g_dMaxAlt
                .MinAlt = g_dMinAlt
                .Park = g_dSetPark
                .Home = g_dSetHome
                .chkStartShutterError.Checked = IIf(g_bStartShutterError, 1, 0)
                .chkSlewingOpenClose.Checked = IIf(g_bSlewingOpenClose, 1, 0)
                .chkNonFragileAtHome.Checked = IIf(g_bStandardAtHome, 0, 1)   ' Reversed
                .chkNonFragileAtPark.Checked = IIf(g_bStandardAtPark, 0, 1)   ' Reversed

                .chkCanFindHome.Checked = IIf(g_bCanFindHome, 1, 0)
                .chkCanPark.Checked = IIf(g_bCanPark, 1, 0)
                .chkCanSetAltitude.Checked = IIf(g_bCanSetAltitude, 1, 0)
                .chkCanSetAzimuth.Checked = IIf(g_bCanSetAzimuth, 1, 0)
                .chkCanSetShutter.Checked = IIf(g_bCanSetShutter, 1, 0)
                .chkCanSetPark.Checked = IIf(g_bCanSetPark, 1, 0)
                .chkCanSyncAzimuth.Checked = IIf(g_bCanSyncAzimuth, 1, 0)
            End With

            Me.Visible = False                       ' May float over setup

            If SetupDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                With SetupDialog
                    g_dOCDelay = .OCDelay
                    g_dAltRate = .AltRate
                    g_dAzRate = .AzRate
                    g_dStepSize = .StepSize
                    g_dMaxAlt = .MaxAlt
                    g_dMinAlt = .MinAlt
                    g_dSetPark = .Park
                    g_dSetHome = .Home
                    g_bStartShutterError = (.chkStartShutterError.Checked)
                    g_bSlewingOpenClose = (.chkSlewingOpenClose.Checked)
                    g_bStandardAtHome = Not (.chkNonFragileAtHome.Checked)    ' Reversed
                    g_bStandardAtPark = Not (.chkNonFragileAtPark.Checked)    ' Reversed

                    g_bCanFindHome = (.chkCanFindHome.Checked)
                    g_bCanPark = (.chkCanPark.Checked)
                    g_bCanSetAltitude = (.chkCanSetAltitude.Checked)
                    g_bCanSetAzimuth = (.chkCanSetAzimuth.Checked)
                    g_bCanSetShutter = (.chkCanSetShutter.Checked)
                    g_bCanSetPark = (.chkCanSetPark.Checked)
                    g_bCanSyncAzimuth = (.chkCanSyncAzimuth.Checked)
                End With
                

                UpdateConfig()
            End If
        End Using

        ' range the shutter
        If g_eShutterState = ShutterState.shutterOpen Then
            If g_dMinAlt > g_dDomeAlt Then HW_MoveShutter(g_dMinAlt)
            If g_dMaxAlt < g_dDomeAlt Then HW_MoveShutter(g_dMaxAlt)
        End If

        ' check for home / park changes (standard: state is fragile, can override with position semantics)
        If g_bStandardAtHome Then
            If g_dDomeAz <> g_dSetHome Then g_bAtHome = False ' Fragile (standard)
        Else
            g_bAtHome = HW_AtHome                                   ' Position (non-standard)
        End If

        If g_bStandardAtPark Then
            If g_dDomeAz <> g_dSetPark Then g_bAtPark = False ' Fragile (standard)
        Else
            g_bAtPark = HW_AtPark                                   ' Position (non-standard)
        End If

        ' update handbox displays


        LabelButtons()
        RefreshLeds()

        Timer1.Enabled = g_bConnected

        Me.Visible = True
        Me.BringToFront()
    End Sub

    Public Sub RefreshLeds()

        lblPARK.ForeColor = IIf(g_bAtPark, System.Drawing.Color.Green, System.Drawing.Color.Red)   ' Green
        lblHOME.ForeColor = IIf(g_bAtHome, System.Drawing.Color.Green, System.Drawing.Color.Red)   ' Green
        lblSlew.ForeColor = IIf(HW_Slewing, System.Drawing.Color.Yellow, System.Drawing.Color.Red)  ' Yellow

    End Sub
    Public Sub LabelButtons()
        If g_dDomeAz = INVALID_COORDINATE Then
            txtDomeAz.Text = "---.-"
        Else

            txtDomeAz.Text = Format$(AzScale(g_dDomeAz), "000.0")
        End If

        If g_bCanPark Then
            If g_dSetPark = INVALID_COORDINATE Then
                ButtonPark.Enabled = False
                ButtonPark.Text = "Park"
            Else
                ButtonPark.Enabled = True
                ButtonPark.Text = "Park: " & Format$(g_dSetPark, "000.0") & "°"
            End If
        Else
            ButtonPark.Enabled = False
            ButtonPark.Text = "Park"
        End If

        ButtonHome.Enabled = g_bCanFindHome

        ButtonGoto.Enabled = g_bCanSetAzimuth
        ButtonSync.Enabled = g_bCanSyncAzimuth
        txtNewAz.Enabled = g_bCanSetAzimuth Or g_bCanSyncAzimuth

        ButtonClockwise.Enabled = g_bCanSetAzimuth
        ButtonStepClockwise.Enabled = g_bCanSetAzimuth
        ButtonCounterClockwise.Enabled = g_bCanSetAzimuth
        ButtonStepCounterClockwise.Enabled = g_bCanSetAzimuth
        ButtonSlewAltitudeUp.Enabled = g_bCanSetAltitude And g_bCanSetShutter
        ButtonSlewAltitudeDown.Enabled = g_bCanSetAltitude And g_bCanSetShutter
        ButtonOpen.Enabled = g_bCanSetShutter
        ButtonClose.Enabled = g_bCanSetShutter

        If g_dDomeAz = INVALID_COORDINATE Then
            txtDomeAz.Text = "---.-"
        Else

            txtDomeAz.Text = Format$(AzScale(g_dDomeAz), "000.0")
        End If

        If g_dDomeAlt = INVALID_COORDINATE Or Not g_bCanSetShutter Then
            txtShutter.Text = "----"
        Else
            Select Case g_eShutterState
                Case ShutterState.shutterOpen
                    If g_bCanSetAltitude Then
                        txtShutter.Text = Format$(g_dDomeAlt, "0.0")
                    Else
                        txtShutter.Text = "Open"
                    End If
                Case ShutterState.shutterClosed : txtShutter.Text = "Closed"
                Case ShutterState.shutterOpening : txtShutter.Text = "Opening"
                Case ShutterState.shutterClosing : txtShutter.Text = "Closing"
                Case ShutterState.shutterError : txtShutter.Text = "Error"
            End Select
        End If

    End Sub
#End Region

#Region "Event Handlers"

    Private Sub HandboxForm_Unload() Handles Me.FormClosing
        If Not g_TrafficForm Is Nothing Then
            g_TrafficForm.Close()
            g_TrafficForm.Dispose()
            g_TrafficForm = Nothing
        End If
        Using profile As New Utilities.Profile
            profile.DeviceType = "Dome"
            profile.WriteValue(g_csDriverID, "Left", Me.Left.ToString(CultureInfo.InvariantCulture))
            profile.WriteValue(g_csDriverID, "Top", Me.Top.ToString(CultureInfo.InvariantCulture))
        End Using
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub picASCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picASCOM.Click
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

    Private Sub ButtonSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSetup.Click
        DoSetup()
    End Sub

    Private Sub ButtonTraffic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTraffic.Click
        If g_TrafficForm Is Nothing Then g_TrafficForm = New ShowTrafficForm

        g_TrafficForm.Text = "Dome Simulator ASCOM Traffic"
        g_TrafficForm.Show()
    End Sub

    Private Sub ButtonPark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPark.Click
        If g_bAtPark Then
            MessageBox.Show("Already parked.")
            Exit Sub
        End If

        If g_bCanPark Then
            If g_dSetPark < -360 Or g_dSetPark > 360 Then
                MessageBox.Show("Park location must be between +/- 360." & vbCrLf & _
                    "Click on [Setup] to change it.")
                Exit Sub
            End If

            HW_Park()
        End If
    End Sub

    Private Sub ButtonHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonHome.Click
        If g_bAtHome Then
            MessageBox.Show("Already at home.")
            Exit Sub
        End If

        If g_bCanFindHome Then HW_FindHome()
    End Sub
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ButtonGoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGoto.Click
        Dim Az As Double

        Az = INVALID_COORDINATE
        Try
            Az = Double.Parse(txtNewAz.Text, CultureInfo.CurrentCulture)
        Catch ex As Exception

        End Try

        If Az < -360 Or Az > 360 Then
            MessageBox.Show("Input value must be between" & _
                vbCrLf & "+/- 360")
            Return
        End If

        Az = AzScale(Az)
        HW_Move(Az)

    End Sub

    Private Sub ButtonSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSync.Click
        Dim Az As Double

        Az = INVALID_COORDINATE
        If Double.TryParse(txtNewAz.Text, NumberStyles.Number, CultureInfo.CurrentCulture, Az) Then
            If Az < -360 Or Az > 360 Then
                MessageBox.Show("Input value must be between" & _
                    vbCrLf & "+/- 360")
                Return
            End If
            Az = AzScale(Az)
            HW_Sync(Az)
        End If
    End Sub
    Private Sub ButtonOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOpen.Click
        If g_eShutterState = ShutterState.shutterError Then
            MessageBox.Show("Shutter must be Closed to clear the error.")
            Return
        End If
        ButtonState = 7
    End Sub

    Private Sub ButtonSlewAltitudeUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSlewAltitudeUp.Click
        If g_eShutterState = ShutterState.shutterError Then
            MessageBox.Show("Shutter must be Closed to clear the error.")
            Return
        End If

        If g_eShutterState = ShutterState.shutterClosed Then
            MessageBox.Show("Shutter must be open first.")
            Return
        End If
        ButtonState = 5
    End Sub

    Private Sub ButtonSlewAltitudeDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSlewAltitudeDown.Click
        If g_eShutterState = ShutterState.shutterError Then
            MessageBox.Show("Shutter must be Closed to clear the error.")
            Return
        End If

        If g_eShutterState = ShutterState.shutterClosed Then
            MessageBox.Show("Shutter must be open first.")
            Return
        End If
        ButtonState = 6
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        ButtonState = 8
    End Sub

    Private Sub ButtonSlewStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSlewStop.Click
        ButtonState = 10
    End Sub

    Private Sub ButtonClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClockwise.Click
        ButtonState = 1
    End Sub

    Private Sub ButtonCounterClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCounterClockwise.Click
        ButtonState = 3
    End Sub

    Private Sub ButtonStepClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStepClockwise.Click
        ButtonState = 2
    End Sub

    Private Sub ButtonStepCounterClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStepCounterClockwise.Click
        ButtonState = 4
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

        Dim slew As Double
        Dim distance As Double

        ' Handle hand-box state first
        If Me.ButtonState <> 0 Then
            Select Case (Me.ButtonState)
                Case 1 ' Go clockwise
                    HW_Run(True)
                Case 2 ' step clockwise
                    HW_Move(AzScale(g_dDomeAz + g_dStepSize))
                Case 3 ' Go counter clockwise
                    HW_Run(False)
                Case 4 ' step counter clockwise
                    HW_Move(AzScale(g_dDomeAz - g_dStepSize))
                Case 5 ' shutter up
                    If g_eShutterState = ShutterState.shutterOpen Then HW_MoveShutter(g_dMaxAlt)
                Case 6 ' shutter down
                    If g_eShutterState = ShutterState.shutterOpen Then HW_MoveShutter(g_dMinAlt)
                Case 7 ' shutter open
                    If g_eShutterState = ShutterState.shutterClosed Then HW_OpenShutter()
                Case 8 ' shutter close
                    If (g_eShutterState = ShutterState.shutterOpen) Or (g_eShutterState = ShutterState.shutterError) Then HW_CloseShutter()
                Case Else ' other - halt
                    HW_Halt()
            End Select

            Me.ButtonState = 0
        End If

        ' Azimuth slew simulation
        If g_eSlewing <> Going.slewNowhere Then
            slew = g_dAzRate * TIMER_INTERVAL
            If g_eSlewing > Going.slewCW Then
                distance = g_dTargetAz - g_dDomeAz
                If distance < 0 Then _
                    slew = -slew
                If distance > 180 Then _
                    slew = -slew
                If distance < -180 Then _
                    slew = -slew
            Else
                distance = slew * 2
                slew = slew * g_eSlewing
            End If

            ' Are we there yet ?
            If System.Math.Abs(distance) < System.Math.Abs(slew) Then
                g_dDomeAz = g_dTargetAz
                If Not g_TrafficForm Is Nothing Then
                    If g_TrafficForm.chkSlew.Checked Then _
                        g_TrafficForm.TrafficLine("(Slew complete)")
                End If

                ' Handle standard (fragile) and non-standard park/home changes
                If g_bStandardAtHome Then
                    If g_eSlewing = Going.slewHome Then g_bAtHome = True ' Fragile (standard)
                Else
                    g_bAtHome = HW_AtHome                               ' Position (non-standard)
                End If

                If g_bStandardAtPark Then
                    If g_eSlewing = Going.slewPark Then g_bAtPark = True ' Fragile (standard)
                Else
                    g_bAtPark = HW_AtPark                               ' Position (non-standard)
                End If

                g_eSlewing = Going.slewNowhere
            Else
                g_dDomeAz = AzScale(g_dDomeAz + slew)
            End If
        End If

        ' shutter altitude control simulation
        If (g_dDomeAlt <> g_dTargetAlt) And g_eShutterState = ShutterState.shutterOpen Then
            slew = g_dAltRate * TIMER_INTERVAL
            distance = g_dTargetAlt - g_dDomeAlt
            If distance < 0 Then _
                slew = -slew

            ' Are we there yet ?
            If System.Math.Abs(distance) < System.Math.Abs(slew) Then
                g_dDomeAlt = g_dTargetAlt
                If Not g_TrafficForm Is Nothing Then
                    If g_TrafficForm.chkShutter.Checked Then _
                        g_TrafficForm.TrafficLine("(Shutter complete)")
                End If
            Else
                g_dDomeAlt = g_dDomeAlt + slew
            End If
        End If

        ' shutter open/close simulation
        If g_dOCProgress > 0 Then
            g_dOCProgress = g_dOCProgress - TIMER_INTERVAL
            If g_dOCProgress <= 0 Then
                If g_eShutterState = ShutterState.shutterOpening Then
                    g_eShutterState = ShutterState.shutterOpen
                Else
                    g_eShutterState = ShutterState.shutterClosed
                End If
                If Not g_TrafficForm Is Nothing Then
                    If g_TrafficForm.chkShutter.Checked Then _
                        g_TrafficForm.TrafficLine("(Shutter complete)")
                End If
            End If
        End If

        If g_dDomeAz = INVALID_COORDINATE Then
            Me.txtDomeAz.Text = "---.-"
        Else

            Me.txtDomeAz.Text = Format$(AzScale(g_dDomeAz), "000.0")
        End If
        'Shutter = g_dDomeAlt
        If g_dDomeAlt = INVALID_COORDINATE Or Not g_bCanSetShutter Then
            Me.txtShutter.Text = "----"
        Else
            Select Case g_eShutterState
                Case ShutterState.shutterOpen
                    If g_bCanSetAltitude Then
                        Me.txtShutter.Text = Format$(g_dDomeAlt, "0.0")
                    Else
                        Me.txtShutter.Text = "Open"
                    End If
                Case ShutterState.shutterClosed : Me.txtShutter.Text = "Closed"
                Case ShutterState.shutterOpening : Me.txtShutter.Text = "Opening"
                Case ShutterState.shutterClosing : Me.txtShutter.Text = "Closing"
                Case ShutterState.shutterError : Me.txtShutter.Text = "Error"
            End Select
        End If
        Me.RefreshLeds()
    End Sub

#End Region

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Using profile As New Utilities.Profile
            profile.DeviceType = "Dome"

            ' Set handbox screen position
            Try
                Me.Left = CInt(profile.GetValue(g_csDriverID, "Left", "", "100"))
                Me.Top = CInt(profile.GetValue(g_csDriverID, "Top", "", "100"))
            Catch ex As Exception

            End Try


            ' Fix bad positions (which shouldn't ever happen, ha ha)
            If Me.Left < 0 Then
                Me.Left = 100
                profile.WriteValue(g_csDriverID, "Left", Me.Left.ToString(CultureInfo.InvariantCulture))
            End If
            If Me.Top < 0 Then
                Me.Top = 100
                profile.WriteValue(g_csDriverID, "Top", Me.Top.ToString(CultureInfo.InvariantCulture))
            End If
        End Using

        Timer1.Interval = TIMER_INTERVAL * 1000
        Timer1.Enabled = True

        Me.LabelButtons()
        Me.RefreshLeds()

    End Sub
End Class