Imports System.Windows.Forms
Imports System.Runtime.InteropServices

<ComVisible(False)> _
Public Class SetupDialogForm

    ' Declare Form control arrays.
    Private m_arrNameTextBox As TextBoxArray
    Private m_arrOffsetTextBox As TextBoxArray
    Private m_arrColourPicBox As PictureBoxArray

    Private m_iSlots As Integer                 ' Current number of filter slots
  

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Create a tooltip object, and assign a few values
        Dim aTooltip As ToolTip = New ToolTip
        aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website")
        aTooltip.SetToolTip(chkImplementsNames, "Driver returns default names if cleared")
        aTooltip.SetToolTip(chkImplementsOffsets, "Driver raises an exception if cleared")

        ' Create control arrays for the filters (arrays make it easier to handle)
        m_arrNameTextBox = New TextBoxArray(Me)
        m_arrOffsetTextBox = New TextBoxArray(Me)
        m_arrColourPicBox = New PictureBoxArray(Me)

        ' Create the textbox and picture controls on the form
        For i As Integer = 0 To 7
            ' Add a new control
            m_arrNameTextBox.AddNewTextBox(False)
            ' Associate it with the TableLayout grid
            m_arrNameTextBox(i).Parent = TableLayoutPanel2
            m_arrNameTextBox(i).Width = 120
            ' Set the cell position
            TableLayoutPanel2.SetRow(m_arrNameTextBox(i), i)
            TableLayoutPanel2.SetColumn(m_arrNameTextBox(i), 1)

            ' Repeat for the other controls
            m_arrOffsetTextBox.AddNewTextBox(True)
            m_arrOffsetTextBox(i).Parent = TableLayoutPanel2
            TableLayoutPanel2.SetRow(m_arrOffsetTextBox(i), i)
            TableLayoutPanel2.SetColumn(m_arrOffsetTextBox(i), 2)

            m_arrColourPicBox.AddNewPictureBox()
            m_arrColourPicBox(i).Parent = TableLayoutPanel2
            TableLayoutPanel2.SetRow(m_arrColourPicBox(i), i)
            TableLayoutPanel2.SetColumn(m_arrColourPicBox(i), 3)
        Next

        ' Give 'em a bit of info...
        lblDriverInfo.Text = ProductName & _
            " Version " & ProductVersion

        EnableDisableControls()

    End Sub


    '
    ' Properties are used to setup the values on the form
    '
#Region "Public Properties"

    Public WriteOnly Property Slots() As Integer
        Set(ByVal value As Integer)
            cmbSlots.Text = CStr(value)
        End Set
    End Property



    Public WriteOnly Property Time() As Integer
        Set(ByVal value As Integer)
            ' We store the time in millisecs, convert to seconds for display
            cmbTime.Text = Format$(value / 1000, "0.0")
        End Set
    End Property


    Public WriteOnly Property Names() As String()
        Set(ByVal value As String())
            For i As Integer = 0 To 7
                m_arrNameTextBox(i).Text = value(i)
            Next i
        End Set
    End Property


    Public WriteOnly Property Offsets() As Integer()
        Set(ByVal value As Integer())
            For i As Integer = 0 To 7
                m_arrOffsetTextBox(i).Text = Format$(value(i), "0")     ' No decimal digits
            Next i
        End Set
    End Property


    Public WriteOnly Property Colours() As Drawing.Color()
        Set(ByVal value As Drawing.Color())
            For i As Integer = 0 To 7
                m_arrColourPicBox(i).BackColor = value(i)
            Next
        End Set
    End Property


    Public WriteOnly Property ImplementsNames() As Boolean
        Set(ByVal value As Boolean)
            chkImplementsNames.Checked = value
        End Set
    End Property


    Public WriteOnly Property ImplementsOffsets() As Boolean
        Set(ByVal value As Boolean)
            chkImplementsOffsets.Checked = value
        End Set
    End Property

#End Region

#Region "Helpers"

    '
    ' Make sure GUI elements are in sync with the number of slots
    '
    Private Sub EnableDisableControls()
        For i As Integer = 0 To 7
            If m_iSlots > i Then
                ' Enable controls
                ControlEnable(m_arrNameTextBox(i), chkImplementsNames.Checked)
                ControlEnable(m_arrOffsetTextBox(i), chkImplementsOffsets.Checked)
                ControlEnable(m_arrColourPicBox(i), True)
            Else
                ' Disable controls
                ControlEnable(m_arrNameTextBox(i), False)
                ControlEnable(m_arrOffsetTextBox(i), False)
                ControlEnable(m_arrColourPicBox(i), False)
            End If
        Next i

        Me.Refresh()

    End Sub

    '
    ' Enable/disable controls
    '
    Private Sub ControlEnable(ByVal control As Control, ByVal enabled As Boolean)
        If enabled Then
            control.Enabled = True
            control.ForeColor = Drawing.Color.Black
        Else
            control.Enabled = False
            control.ForeColor = Drawing.Color.DarkGray
        End If
    End Sub

#End Region

#Region "Event Handlers"


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim i As Integer

        ' Write all the settings to the registry
        m_iSlots = CInt(cmbSlots.Text)
        g_Profile.WriteValue(g_csDriverID, "Slots", m_iSlots.ToString)
        ' Convert secs to millisecs
        i = CInt(CDbl(cmbTime.Text) * 1000)
        g_Profile.WriteValue(g_csDriverID, "Time", i.ToString)
        For i = 0 To 7
            g_Profile.WriteValue(g_csDriverID, i.ToString, m_arrNameTextBox(i).Text, "FilterNames")
            g_Profile.WriteValue(g_csDriverID, i.ToString, m_arrOffsetTextBox(i).Text, "FocusOffsets")
            g_Profile.WriteValue(g_csDriverID, i.ToString, System.Drawing.ColorTranslator.ToWin32(m_arrColourPicBox(i).BackColor).ToString, "FilterColours")
        Next i
        g_Profile.WriteValue(g_csDriverID, "ImplementsNames", chkImplementsNames.Checked.ToString)
        g_Profile.WriteValue(g_csDriverID, "ImplementsOffsets", chkImplementsOffsets.Checked.ToString)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

    End Sub


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub


    Private Sub ShowAscomWebPage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picASCOM.DoubleClick, picASCOM.Click
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


    Private Sub cmbSlots_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSlots.KeyPress
        ' Only allow 0-9 and backspace
        If e.KeyChar < Microsoft.VisualBasic.ChrW(48) Or e.KeyChar > Microsoft.VisualBasic.ChrW(57) Then
            If Not e.KeyChar = Microsoft.VisualBasic.ChrW(8) Then e.Handled = True
        End If
    End Sub


    Private Sub cmbSlots_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSlots.SelectedValueChanged
        Dim i As Integer
        i = CInt("0" & cmbSlots.Text)    ' Make blanks = 0
        If i >= 1 And i < 9 Then
            m_iSlots = i
            EnableDisableControls()
        End If
    End Sub


    Private Sub cmbSlots_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmbSlots.Validating
        Dim i As Integer
        i = CInt("0" & cmbSlots.Text)    ' Make blanks = 0
        If i < 1 Or i > 8 Then
            e.Cancel = True
            MsgBox("Range of slot values is 1-8", MsgBoxStyle.Exclamation And MsgBoxStyle.OkOnly, "Input Error")
        End If
    End Sub


    Private Sub cmbTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTime.KeyPress
        ' Only allow one occurrence of the period
        If e.KeyChar = "." And InStr(cmbTime.Text, ".") > 0 Then
            e.Handled = True
            ' Now carry out numerals and period check
        ElseIf e.KeyChar < "." Or e.KeyChar > "9" Or e.KeyChar = "/" Then
            If Not e.KeyChar = Microsoft.VisualBasic.ChrW(8) Then e.Handled = True
        End If
    End Sub


    Private Sub cmbTime_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmbTime.Validating
        Dim i As Double

        i = CDbl("0" & cmbTime.Text)        ' Make blanks = 0
        If i < 0.1 Or i > 8 Then
            MsgBox("Range time values is 0.1-8.0", MsgBoxStyle.Exclamation And MsgBoxStyle.OkOnly, "Input Error")
            e.Cancel = True
        End If

    End Sub


    Private Sub chkImplementsNames_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImplementsNames.Click
        EnableDisableControls()
    End Sub


    Private Sub chkImplementsOffsets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImplementsOffsets.Click
        EnableDisableControls()
    End Sub


    Private Sub SetupDialogForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        EnableDisableControls()
    End Sub

#End Region

End Class
