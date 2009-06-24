'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
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

<ComVisible(False)> _
Public Class HandboxForm
    Public BtnState As Integer                         'Controls the dome slewing buttons
#Region "Public Properties and Methods"

    Public Sub UpdateConfig()
        g_Profile.WriteValue(ID, "OCDelay", CStr(g_dOCDelay))
        g_Profile.WriteValue(ID, "SetPark", CStr(g_dSetPark))
        g_Profile.WriteValue(ID, "SetHome", CStr(g_dSetHome))
        g_Profile.WriteValue(ID, "AltRate", CStr(g_dAltRate))
        g_Profile.WriteValue(ID, "AzRate", CStr(g_dAzRate))
        g_Profile.WriteValue(ID, "StepSize", CStr(g_dStepSize))
        g_Profile.WriteValue(ID, "MaxAlt", CStr(g_dMaxAlt))
        g_Profile.WriteValue(ID, "MinAlt", CStr(g_dMinAlt))
        g_Profile.WriteValue(ID, "StartShutterError", CStr(g_bStartShutterError))
        g_Profile.WriteValue(ID, "SlewingOpenClose", CStr(g_bSlewingOpenClose))
        g_Profile.WriteValue(ID, "NonFragileAtHome", CStr(Not g_bStandardAtHome))
        g_Profile.WriteValue(ID, "NonFragileAtPark", CStr(Not g_bStandardAtPark))

        g_Profile.WriteValue(ID, "DomeAz", CStr(g_dDomeAz), "State")
        g_Profile.WriteValue(ID, "DomeAlt", CStr(g_dDomeAlt), "State")

        g_Profile.WriteValue(ID, "ShutterState", CStr(g_eShutterState), "State")

        g_Profile.WriteValue(ID, "CanFindHome", CStr(g_bCanFindHome), "Capabilities")
        g_Profile.WriteValue(ID, "CanPark", CStr(g_bCanPark), "Capabilities")
        g_Profile.WriteValue(ID, "CanSetAltitude", CStr(g_bCanSetAltitude), "Capabilities")
        g_Profile.WriteValue(ID, "CanSetAzimuth", CStr(g_bCanSetAzimuth), "Capabilities")
        g_Profile.WriteValue(ID, "CanSetPark", CStr(g_bCanSetPark), "Capabilities")
        g_Profile.WriteValue(ID, "CanSetShutter", CStr(g_bCanSetShutter), "Capabilities")
        g_Profile.WriteValue(ID, "CanSyncAzimuth", CStr(g_bCanSyncAzimuth), "Capabilities")
    End Sub

    Public Sub DoSetup()
        g_timer.Enabled = False

        Dim SetupDialog As SetupDialogForm = New SetupDialogForm

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
        RefreshLEDs()

        g_timer.Enabled = g_bConnected

        Me.Visible = True
        Me.BringToFront()
    End Sub

    Public Sub RefreshLEDs()

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
    Private Sub HandboxForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub HandboxForm_Unload() Handles MyBase.Disposed
        If Not g_trafficDialog Is Nothing Then _
            g_trafficDialog.Close()
        g_Profile.WriteValue(g_csDriverID, "Left", Me.Left.ToString)
        g_Profile.WriteValue(g_csDriverID, "Top", Me.Top.ToString)
    End Sub
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
        If g_show Is Nothing Then _
           g_show = New ShowTrafficForm

        g_show.Text = "Dome Simulator ASCOM Traffic"
        g_show.Show()
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

        If g_bCanFindHome Then _
            HW_FindHome()
    End Sub
    Private Sub ButtonGoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGoto.Click
        Dim Az As Double

        Az = INVALID_COORDINATE
        Try
            Az = Double.Parse(txtNewAz.Text)
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
        Try
            Az = Double.Parse(txtNewAz.Text)
        Catch ex As Exception

        End Try


        If Az < -360 Or Az > 360 Then
            MessageBox.Show("Input value must be between" & _
                vbCrLf & "+/- 360")
            Return
        End If

        Az = AzScale(Az)


        HW_Sync(Az)
    End Sub
    Private Sub ButtonOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOpen.Click
        If g_eShutterState = ShutterState.shutterError Then

            MessageBox.Show("Shutter must be Closed to clear the error.")
            Return

        End If

        BtnState = 7
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

        BtnState = 5
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

        BtnState = 6
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click


        BtnState = 8
    End Sub

    Private Sub ButtonSlewStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSlewStop.Click
        BtnState = 10
    End Sub

    Private Sub ButtonClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClockwise.Click
        BtnState = 1
    End Sub

    Private Sub ButtonCounterClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCounterClockwise.Click
        BtnState = 3
    End Sub

    Private Sub ButtonStepClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStepClockwise.Click
        BtnState = 2
    End Sub

    Private Sub ButtonStepCounterClockwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStepCounterClockwise.Click
        BtnState = 4
    End Sub
#End Region





End Class