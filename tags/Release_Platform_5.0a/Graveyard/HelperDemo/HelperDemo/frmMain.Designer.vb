<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
		Me.cmdChoose = New System.Windows.Forms.Button
		Me.SuspendLayout()
		'
		'cmdChoose
		'
		Me.cmdChoose.Location = New System.Drawing.Point(12, 12)
		Me.cmdChoose.Name = "cmdChoose"
		Me.cmdChoose.Size = New System.Drawing.Size(163, 25)
		Me.cmdChoose.TabIndex = 0
		Me.cmdChoose.Text = "Show Telescope Chooser"
		Me.cmdChoose.UseVisualStyleBackColor = True
		'
		'frmMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(187, 47)
		Me.Controls.Add(Me.cmdChoose)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMain"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.Text = "VB.NET uses Helper"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents cmdChoose As System.Windows.Forms.Button

End Class
