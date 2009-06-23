﻿Imports System.Windows.Forms

<ComVisible(False)> _
Public Class HandboxForm

#Region "Variables"

    ' button handling variables
    Private BtnState As Integer                         ' current state
    Private BtnDebounce As Integer                      ' hold key for fetch even of up occured
    Private downclick As Boolean                        ' button just went down - state unclear
    Private getting As Boolean                          ' timer going off - fetch in progress
    Private m_bConnected As Boolean                     ' Tracked connected state
    Private m_sPosition As Short                        ' Current filter position
    Private m_bMoving As Boolean                        ' FilterWheel in motion?
    Private m_iSlots As Integer                         ' Number of filter wheel positions
    Private m_asFilterNames(7) As String                  ' Array of filter name strings
    Private m_aiFocusOffsets(7) As Integer                ' Array of focus offsets
    Private m_acFilterColours(7) As System.Drawing.Color ' Array of filter colours
    Private m_iTimeInterval As Integer                  ' Time to move between filter positions (miilisecs)
    Private m_bImplementsNames As Boolean               ' Return filternames?
    Private m_bImplementsOffsets As Boolean             ' Return Offsets?

#End Region

#Region "Event Handlers"

    Private Sub Prev_Button_MouseDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.MouseDown
        downclick = True
        BtnState = 1
        BtnDebounce = 1
    End Sub

    Private Sub Prev_Button_MouseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.MouseUp
        BtnState = 0
        'race condition handling
        If getting Then _
            BtnDebounce = 0
        downclick = False
    End Sub

    Private Sub Next_Button_MouseDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.MouseDown
        downclick = True
        BtnState = 2
        BtnDebounce = 2
    End Sub

    Private Sub Next_Button_MouseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.MouseUp
        BtnState = 0
        'race condition handling
        If getting Then _
            BtnDebounce = 0
        downclick = False
    End Sub

    Private Sub picASCOM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picASCOM.Click
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


    Private Sub btnTraffic_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTraffic.Click
        If g_trafficDialog Is Nothing Then
            g_trafficDialog = New ShowTrafficForm
            'g_trafficDialog.TextOffset = 1425
        End If

        g_trafficDialog.Show()

    End Sub


    Private Sub btnConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        Me.btnConnect.Text = IIf(m_bConnected, "Connect", "Disconnect").ToString
        m_bConnected = Not m_bConnected
        If Not g_trafficDialog Is Nothing Then
            If g_trafficDialog.chkOther.Checked Then
                g_trafficDialog.TrafficLine("Connected = " & m_bConnected)
            End If
        End If
        UpdateLabels()
    End Sub

    ' Hmm, never gets called!
    ' !!!!!!!!!!!!!!!!!!!!!!!
    Private Sub HandboxForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' force clean state
        BtnState = 0
        BtnDebounce = 0
        getting = False
        downclick = False
        On Error Resume Next
        If Not g_trafficDialog Is Nothing Then _
            g_trafficDialog.Close()
        DoShutdown()              ' Saves parameters for next start
    End Sub


    Private Sub HandboxForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim aTooltip As ToolTip = New ToolTip
        aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website")
        aTooltip.SetToolTip(btnTraffic, "Monitor ASCOM API traffic")
        aTooltip.SetToolTip(btnConnect, "Connect/Disconnect filterwheel")
        aTooltip.SetToolTip(btnPrev, "Move position to previous filter")
        aTooltip.SetToolTip(btnNext, "Move position to next filter")
        BtnState = 0
        BtnDebounce = 0
        downclick = False
        getting = False

        UpdateLabels()

    End Sub

    Private Sub btnSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        DoSetup()
    End Sub


    '---------------------------------------------------------------------
    '
    ' timer_tick() - Called when timer in frmHandbox fires event
    '
    ' Implements handbox control.
    '---------------------------------------------------------------------
    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Dim button As Long

        ' Kept this timer based polling of the keys because I did think
        ' about implementing a filter movement animation. Ha!

        ' Only process the button if we are not already moving
        If Not m_bMoving Then

            button = ButtonState      ' Only fetch once due to debounce

            If button <> 0 Then                 ' Handbox buttons have priority

                Select Case button
                    Case 1                                  ' Prev button
                        m_sPosition = m_sPosition - CShort(1)
                    Case 2                                  ' Next button
                        m_sPosition = m_sPosition + CShort(1)
                End Select

                ' make sure position stays in range
                If m_sPosition > m_iSlots - 1 Then
                    m_sPosition = 0
                ElseIf m_sPosition < 0 Then
                    m_sPosition = CShort(m_iSlots - 1)
                End If

                If Not g_trafficDialog Is Nothing Then
                    If g_trafficDialog.chkMoving.Checked Then
                        g_trafficDialog.TrafficStart("Set Position=" & m_sPosition)
                    End If
                End If

                ' trigger the "motor"
                TimerMove.Interval = m_iTimeInterval
                TimerMove.Enabled = True
                m_bMoving = True
                Me.Moving = True

            End If
        End If

    End Sub

    '---------------------------------------------------------------------
    '
    ' timer_move() - Called when the move timer in frmHandbox fires event
    '
    ' Implements moving.
    '---------------------------------------------------------------------
    Private Sub TimerMove_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerMove.Tick
        ' disable the timer now it has fired
        TimerMove.Enabled = False

        '
        ' move complete, update everything
        '
        If m_bMoving Then
            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkMoving.Checked Then
                    g_trafficDialog.TrafficLine(" (move complete)")
                End If
            End If
            m_bMoving = False
        End If

        Me.Moving = False

    End Sub
#End Region

#Region "Properties"

    Public Property Connected() As Boolean
        Get
            Connected = m_bConnected
        End Get
        Set(ByVal value As Boolean)
            m_bConnected = value
            UpdateLabels()
        End Set
    End Property

    Public Property Position() As Short
        Get
            Position = m_sPosition
        End Get
        Set(ByVal value As Short)
            Dim Time As Integer

            ' position range check
            If value > m_iSlots - 1 Or value < 0 Then
                If Not g_trafficDialog Is Nothing Then
                    If g_trafficDialog.chkPosition.Checked Then _
                        g_trafficDialog.TrafficEnd(" (aborting, range)")
                End If
                Throw New DriverException("Position: " & MSG_VAL_OUTOFRANGE, SCODE_VAL_OUTOFRANGE)
            End If

            ' do the move

            ' Find the shortest distance between two filter positions
            Time = Math.Min(Math.Abs(m_sPosition - value), m_iSlots - Math.Abs(m_sPosition - value)) * m_iTimeInterval
            TimerMove.Interval = Time
            TimerMove.Enabled = True
            m_sPosition = value

            ' trigger the "motor"
            m_bMoving = True

            ' update the handbox
            Me.Moving = True

        End Set
    End Property

    Public ReadOnly Property Slots() As Integer
        Get
            Slots = m_iSlots
        End Get
    End Property

    Public Property FilterNames() As String()
        Get
            Dim temp() As String
            temp = m_asFilterNames
            ReDim Preserve temp(m_iSlots - 1)
            FilterNames = temp
        End Get
        Set(ByVal value As String())
            m_asFilterNames = value
            UpdateLabels()
        End Set
    End Property

    Public Property FocusOffsets() As Integer()
        Get
            Dim temp() As Integer
            temp = m_aiFocusOffsets
            ' Only return an array with the values used
            ReDim Preserve temp(m_iSlots - 1)
            FocusOffsets = temp
        End Get
        Set(ByVal value As Integer())
            m_aiFocusOffsets = value
            UpdateLabels()
        End Set
    End Property

    Public Property FilterColour() As System.Drawing.Color()
        Get
            FilterColour = m_acFilterColours
        End Get
        Set(ByVal value As System.Drawing.Color())
            m_acFilterColours = value
            UpdateLabels()
        End Set
    End Property

    Public Property Moving() As Boolean
        Get
            Moving = m_bMoving
        End Get
        Set(ByVal value As Boolean)
            If value Then
                picFilterMoving.Visible = True
                lblPosition.Text = "moving"
                lblName.Text = ""
                lblOffset.Text = ""
            Else
                UpdateLabels()
            End If
        End Set
    End Property


    Public ReadOnly Property ButtonState() As Integer
        Get
            ' race condition check, ignore if down just occuring
            If downclick Then
                downclick = False
                ButtonState = 0     ' button not really down yet
                Exit Property
            End If

            ButtonState = BtnDebounce

            ' race condition, make sure not to return 2 presses
            getting = True
            If BtnState = 0 Then _
                BtnDebounce = 0
            getting = False
        End Get
    End Property

    Public Sub DoSetup()

        Dim SetupDialog As SetupDialogForm = New SetupDialogForm

        Timer.Enabled = False                         ' don't want races
        TimerMove.Enabled = False

        With SetupDialog
            .Slots = m_iSlots
            .Time = m_iTimeInterval
            .Names = m_asFilterNames
            .Offsets = m_aiFocusOffsets
            .Colours = m_acFilterColours
            .ImplementsNames = m_bImplementsNames
            .ImplementsOffsets = m_bImplementsOffsets
        End With

        Me.Visible = False                       ' May float over setup

        If SetupDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Read new values from registry
            UpdateConfig()
            ' Fix up position if required
            If m_sPosition >= m_iSlots Then m_sPosition = 0 ' Reduced the number of slots?
            ' Update Handbox
            UpdateLabels()
        End If

        Me.Visible = True
        Me.BringToFront()

        Timer.Interval = CInt(TIMER_INTERVAL * 1000)
        TimerMove.Interval = m_iTimeInterval * 1000
        Timer.Enabled = True

        SetupDialog = Nothing

    End Sub

    Public Sub UpdateConfig()
        ' Read all the values from the registry
        m_sPosition = CShort(g_Profile.GetValue(g_csDriverID, "Position"))
        m_iSlots = CInt(g_Profile.GetValue(g_csDriverID, "Slots"))
        m_iTimeInterval = CInt(g_Profile.GetValue(g_csDriverID, "Time"))
        m_bImplementsNames = CBool(g_Profile.GetValue(g_csDriverID, "ImplementsNames"))
        m_bImplementsOffsets = CBool(g_Profile.GetValue(g_csDriverID, "ImplementsOffsets"))
        For i As Integer = 0 To 7
            m_asFilterNames(i) = g_Profile.GetValue(g_csDriverID, i.ToString, "FilterNames")
            m_aiFocusOffsets(i) = CInt(g_Profile.GetValue(g_csDriverID, i.ToString, "FocusOffsets"))
            m_acFilterColours(i) = System.Drawing.ColorTranslator.FromWin32(CInt(g_Profile.GetValue(g_csDriverID, i.ToString, "FilterColours")))
        Next i
    End Sub

#End Region

#Region "helpers"

    Private Sub UpdateLabels()
        lblPosition.Text = m_sPosition.ToString
        lblName.Text = m_asFilterNames(m_sPosition)
        lblOffset.Text = m_aiFocusOffsets(m_sPosition).ToString
        picFilterMoving.Visible = m_bMoving
        picFilterStop.BackColor = m_acFilterColours(m_sPosition)
        If m_bConnected Then
            btnConnect.Text = "Connected"
            btnConnect.BackColor = System.Drawing.Color.DarkGreen
        Else
            btnConnect.Text = "Disconnected"
            btnConnect.BackColor = System.Drawing.Color.Maroon
        End If
        btnNext.Enabled = m_bConnected
        btnPrev.Enabled = m_bConnected
    End Sub



    Sub DoShutdown()

        m_bMoving = False
        m_bConnected = False

        Timer.Enabled = False

        g_Profile.WriteValue(g_csDriverID, "Position", m_sPosition.ToString)

        Me.Visible = True
        Me.WindowState = FormWindowState.Normal
        g_Profile.WriteValue(g_csDriverID, "Left", Me.Left.ToString)
        g_Profile.WriteValue(g_csDriverID, "Top", Me.Top.ToString)

    End Sub


    '---------------------------------------------------------------------
    '
    ' Min() - returns the smallest of two numbers
    '
    '---------------------------------------------------------------------
    Private Function Min(ByVal a As VariantType, ByVal b As VariantType) As Object
        Min = IIf(a <= b, a, b)
    End Function

#End Region


End Class
