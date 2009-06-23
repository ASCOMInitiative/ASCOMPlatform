<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetupDialogForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
Me.OK_Button = New System.Windows.Forms.Button
Me.Cancel_Button = New System.Windows.Forms.Button
Me.Label1 = New System.Windows.Forms.Label
Me.PictureBox1 = New System.Windows.Forms.PictureBox
Me.TableLayoutPanel1.SuspendLayout()
CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
Me.SuspendLayout()
'
'TableLayoutPanel1
'
Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
Me.TableLayoutPanel1.ColumnCount = 2
Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
Me.TableLayoutPanel1.Location = New System.Drawing.Point(151, 113)
Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
Me.TableLayoutPanel1.RowCount = 1
Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
Me.TableLayoutPanel1.TabIndex = 0
'
'OK_Button
'
Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
Me.OK_Button.Location = New System.Drawing.Point(3, 3)
Me.OK_Button.Name = "OK_Button"
Me.OK_Button.Size = New System.Drawing.Size(67, 23)
Me.OK_Button.TabIndex = 0
Me.OK_Button.Text = "OK"
'
'Cancel_Button
'
Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
Me.Cancel_Button.Name = "Cancel_Button"
Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
Me.Cancel_Button.TabIndex = 1
Me.Cancel_Button.Text = "Cancel"
'
'Label1
'
Me.Label1.AutoSize = True
Me.Label1.Location = New System.Drawing.Point(12, 55)
Me.Label1.Name = "Label1"
Me.Label1.Size = New System.Drawing.Size(195, 13)
Me.Label1.TabIndex = 1
Me.Label1.Text = "Construct your driver's setup dialog here"
'
'PictureBox1
'
Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
Me.PictureBox1.Image = Global.ASCOM.$safeprojectname$.My.Resources.Resources.ASCOM
Me.PictureBox1.Location = New System.Drawing.Point(248, 12)
Me.PictureBox1.Name = "PictureBox1"
Me.PictureBox1.Size = New System.Drawing.Size(48, 56)
Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
Me.PictureBox1.TabIndex = 2
Me.PictureBox1.TabStop = False
'
'SetupDialogForm
'
Me.AcceptButton = Me.OK_Button
Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
Me.CancelButton = Me.Cancel_Button
Me.ClientSize = New System.Drawing.Size(309, 154)
Me.Controls.Add(Me.PictureBox1)
Me.Controls.Add(Me.Label1)
Me.Controls.Add(Me.TableLayoutPanel1)
Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
Me.MaximizeBox = False
Me.MinimizeBox = False
Me.Name = "SetupDialogForm"
Me.ShowInTaskbar = False
Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
Me.Text = "$safeprojectname$ Setup"
Me.TableLayoutPanel1.ResumeLayout(False)
CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
Me.ResumeLayout(False)
Me.PerformLayout()

End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
	Friend WithEvents Cancel_Button As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
 Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
