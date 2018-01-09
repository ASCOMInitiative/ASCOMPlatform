'   ===========
'   HandboxForm.vb
'   ===========
'
' ASCOM Jornaling form
'
' Written:  31-May-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-may-03 jab     Initial edit
' 17-Jun-17 jab     Modified for general use (LOGLENGTH, TextOffset)
' 10-Sep-03 jab     make resize more robust
' 21-Dec-04 rbd     Increase LOGLENGTH from 2000 to 10000
' 23-Jun-09 rbt     Port to Visual Basic .NET
' -----------------------------------------------------------------------------

Imports System.Windows.Forms

<ComVisible(False)> _
Public Class ShowTrafficForm
#Region "Properties"
    Private Const LOGLENGTH As Integer = 2000
    'Private m_iTextOffset As Integer
    Private m_bDisable As Boolean
    Private m_bBOL As Boolean

    Public Sub TrafficChar(ByVal value As String)

        If m_bDisable Then _
            Exit Sub

        If m_bBOL Then
            m_bBOL = False
            txtTraffic.Text = Strings.Right(txtTraffic.Text & value, LOGLENGTH)
        Else
            txtTraffic.Text = Strings.Right(txtTraffic.Text & " " & value, LOGLENGTH)
        End If

        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficLine(ByVal value As String)

        If m_bDisable Then _
            Exit Sub

        If m_bBOL Then
            value = value & vbCrLf
        Else
            value = vbCrLf & value & vbCrLf
        End If

        m_bBOL = True

        txtTraffic.Text = Strings.Right(txtTraffic.Text & value, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficStart(ByVal value As String)

        If m_bDisable Then _
            Exit Sub

        If Not m_bBOL Then _
            value = vbCrLf & value

        m_bBOL = False

        txtTraffic.Text = Strings.Right(txtTraffic.Text & value, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

    Public Sub TrafficEnd(ByVal value As String)

        If m_bDisable Then _
            Exit Sub

        value = value & vbCrLf

        m_bBOL = True

        txtTraffic.Text = Strings.Right(txtTraffic.Text & value, LOGLENGTH)
        txtTraffic.SelectionStart = Len(txtTraffic.Text)

    End Sub

#End Region


    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        m_bBOL = True
        txtTraffic.Text = ""
    End Sub

    Private Sub ButtonDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDisable.Click
        m_bDisable = Not m_bDisable
        Me.ButtonDisable.Text = IIf(m_bDisable, "Enable", "Disable")
    End Sub

    Private Sub ShowTrafficForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_bBOL = True
        txtTraffic.Text = ""
        m_bDisable = False
        ButtonDisable.Text = "Disable"

    End Sub
End Class