''' <summary>
''' Custom renderer for the Chooser tool strip
''' </summary>
Public Class ChooserCustomToolStripRenderer
    Inherits ToolStripProfessionalRenderer

    ''' <summary>
    ''' Prevent "selected "colour changes when hovering over disabled menu items 
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        If (e.Item.Enabled) Then
            MyBase.OnRenderMenuItemBackground(e)
        End If
    End Sub

    ''' <summary>
    ''' Respect the BackBolor property set for labels - without this they always appear with a grey background.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnRenderLabelBackground(e As ToolStripItemRenderEventArgs)
        If Not (e.Item.BackColor = Color.WhiteSmoke) Then
            Dim myBrush As SolidBrush = New SolidBrush(e.Item.BackColor)
            e.Graphics.FillRectangle(myBrush, e.Item.ContentRectangle)
            myBrush.Dispose()
        End If
    End Sub

End Class
