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
    Private m_bConnected As Boolean                     ' Tracked connected state
#Region "Public Properties and Methods"
    Public Property Connected() As Boolean
        Get
            Connected = m_bConnected
        End Get
        Set(ByVal value As Boolean)
            m_bConnected = value
            LabelButtons()
        End Set
    End Property
   
    Public Sub UpdateConfig()

    End Sub

    Public Sub DoSetup()
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


        Me.Visible = True
        Me.BringToFront()
    End Sub

    Public Sub RefreshLEDs()

        lblPARK.ForeColor = IIf(g_bAtPark, &HFF00&, &H404080)   ' Green
        lblHOME.ForeColor = IIf(g_bAtHome, &HFF00&, &H404080)   ' Green
        lblSLEW.ForeColor = IIf(HW_Slewing, &HFFFF&, &H404080)  ' Yellow

    End Sub
#End Region
#Region "Event Handlers"
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
#End Region

#Region "Private Methods"
    Private Sub LabelButtons()
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
        buttonslewaltitudeup.Enabled = g_bCanSetAltitude And g_bCanSetShutter
        buttonslewaltitudedown.Enabled = g_bCanSetAltitude And g_bCanSetShutter
        ButtonOpen.Enabled = g_bCanSetShutter
        ButtonClose.Enabled = g_bCanSetShutter
    End Sub
#End Region
End Class