<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.btnCOM = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnCOM
        '
        Me.btnCOM.Location = New System.Drawing.Point(88, 66)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(110, 23)
        Me.btnCOM.TabIndex = 0
        Me.btnCOM.Text = "COM Diagnostics"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(205, 231)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnCOM)
        Me.Name = "Form1"
        Me.Text = "ASCOM Diagnostics"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button

End Class
