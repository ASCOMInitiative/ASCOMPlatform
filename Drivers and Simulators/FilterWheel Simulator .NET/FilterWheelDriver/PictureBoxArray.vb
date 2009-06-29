<ComVisible(False)> _
Public Class PictureBoxArray

    Inherits System.Collections.CollectionBase
    Private ReadOnly HostForm As System.Windows.Forms.Form


    Public Function AddNewPictureBox() As System.Windows.Forms.PictureBox
        ' Create a new instance of the Button class.
        Dim aPictureBox As New System.Windows.Forms.PictureBox()
        Dim aTooltip As New ToolTip()
        ' Add the button to the collection's internal list.
        Me.List.Add(aPictureBox)
        ' Add the button to the controls collection of the form 
        ' referenced by the HostForm field.
        HostForm.Controls.Add(aPictureBox)
        ' Set intial properties for the button object.
        aPictureBox.Top = Count * 25
        aPictureBox.Left = 100
        aPictureBox.BorderStyle = BorderStyle.FixedSingle
        aPictureBox.Tag = Me.Count
        aPictureBox.BackColor = Drawing.Color.DarkGray
        aTooltip.SetToolTip(aPictureBox, "Click to select colour")

        AddHandler aPictureBox.Click, AddressOf ClickHandler

        Return aPictureBox
    End Function

    Public Sub New(ByVal host As System.Windows.Forms.Form)
        HostForm = host
        'Me.AddNewPictureBox()
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As System.Windows.Forms.PictureBox
        Get
            Return CType(Me.List.Item(Index), System.Windows.Forms.PictureBox)
        End Get
    End Property

    Public Sub Remove()
        ' Check to be sure there is a PictureBox to remove.
        If Me.Count > 0 Then
            ' Remove the last button added to the array from the host form 
            ' controls collection. Note the use of the default property in 
            ' accessing the array.
            HostForm.Controls.Remove(Me(Me.Count - 1))
            Me.List.RemoveAt(Me.Count - 1)
        End If
    End Sub

    ' When the picturebox is clicked show a colour picker dialog
    ' set initially to the current colour. If OK clicked on colour picker
    ' set the current picture box background to the selected colour
    Public Sub ClickHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim colourDiag As Windows.Forms.ColorDialog = New Windows.Forms.ColorDialog
        colourDiag.AllowFullOpen = True
        colourDiag.AnyColor = True
        colourDiag.Color = CType(sender, System.Windows.Forms.PictureBox).BackColor
        If (colourDiag.ShowDialog() = DialogResult.OK) Then _
            CType(sender, System.Windows.Forms.PictureBox).BackColor = colourDiag.Color
    End Sub


End Class
