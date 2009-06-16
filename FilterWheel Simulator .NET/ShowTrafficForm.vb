Option Explicit On
Imports System.Windows.Forms

<ComVisible(False)> _
Public Class ShowTrafficForm

    Private Const LOGLENGTH As Integer = 2000
    Private m_iTextOffset As Integer
    Private m_bDisable As Boolean
    Private m_bBOL As Boolean


    Public Sub New()
        Dim e As System.EventArgs = System.EventArgs.Empty

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim aTooltip As ToolTip = New ToolTip
        aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website")

        Clear_Button_Click(Me, e)
        m_bDisable = False
        Me.btnDisable.Text = "Disable"

    End Sub

#Region "Event Handlers"


    Private Sub Disable_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisable.Click
        m_bDisable = Not m_bDisable
        Me.btnDisable.Text = IIf(m_bDisable, "Enable", "Disable").ToString
    End Sub

    Private Sub Clear_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        m_bBOL = True
        txtTraffic.Text = ""
    End Sub

#End Region

#Region "Properties"


    Public Sub TrafficChar(ByVal val As String)

        If m_bDisable Then _
            Exit Sub

        If m_bBOL Then
            m_bBOL = False
            txtTraffic.Text = Strings.Right(txtTraffic.Text & val, LOGLENGTH)
        Else
            txtTraffic.Text = Strings.Right(txtTraffic.Text & " " & val, LOGLENGTH)
        End If

        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficLine(ByVal val As String)

        If m_bDisable Then _
            Exit Sub

        If m_bBOL Then
            val = val & vbCrLf
        Else
            val = vbCrLf & val & vbCrLf
        End If

        m_bBOL = True

        txtTraffic.Text = Strings.Right(txtTraffic.Text & val, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficStart(ByVal val As String)

        If m_bDisable Then _
            Exit Sub

        If Not m_bBOL Then _
            val = vbCrLf & val

        m_bBOL = False

        txtTraffic.Text = Strings.Right(txtTraffic.Text & val, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficEnd(ByVal val As String)

        If m_bDisable Then _
            Exit Sub

        val = val & vbCrLf

        m_bBOL = True

        txtTraffic.Text = Strings.Right(txtTraffic.Text & val, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    'Public WriteOnly Property TextOffset() As Integer
    '    Set(ByVal value As Integer)
    '        m_iTextOffset = value
    '        txtTraffic.Height = Height - m_iTextOffset
    '    End Set
    'End Property

#End Region

End Class
