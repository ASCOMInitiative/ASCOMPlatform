<ComVisible(False)> _
Public Class TextBoxArray
    Inherits System.Collections.CollectionBase
    Private ReadOnly HostForm As System.Windows.Forms.Form



    Public Function AddNewTextBox(ByVal bOffsetBox As Boolean) As System.Windows.Forms.TextBox
        ' Create a new instance of the Button class.
        Dim aTextBox As New System.Windows.Forms.TextBox()
        ' Add the button to the collection's internal list.
        Me.List.Add(aTextBox)
        ' Add the button to the controls collection of the form 
        ' referenced by the HostForm field.
        HostForm.Controls.Add(aTextBox)
        ' Set intial properties for the button object.
        aTextBox.Top = Count * 25
        aTextBox.Left = 100
        aTextBox.Tag = Me.Count
        aTextBox.Text = "TextBox " & Me.Count.ToString

        If bOffsetBox Then _
            AddHandler aTextBox.KeyPress, AddressOf KeyPressHandler

        Return aTextBox
    End Function

    Public Sub New(ByVal host As System.Windows.Forms.Form)
        HostForm = host
        'Me.AddNewTextBox()
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As System.Windows.Forms.TextBox
        Get
            Return CType(Me.List.Item(Index), System.Windows.Forms.TextBox)
        End Get
    End Property

    Public Sub Remove()
        ' Check to be sure there is a textbox to remove.
        If Me.Count > 0 Then
            ' Remove the last button added to the array from the host form 
            ' controls collection. Note the use of the default property in 
            ' accessing the array.
            HostForm.Controls.Remove(Me(Me.Count - 1))
            Me.List.RemoveAt(Me.Count - 1)
        End If
    End Sub


    Public Sub KeyPressHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim key As System.Windows.Forms.KeyPressEventArgs

        key = CType(e, System.Windows.Forms.KeyPressEventArgs)
        If key.KeyChar < "0" Or key.KeyChar > "9" Then
            If Not key.KeyChar = Microsoft.VisualBasic.ChrW(8) Then key.Handled = True
        End If
    End Sub

End Class
