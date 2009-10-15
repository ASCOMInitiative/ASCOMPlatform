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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btnCOM = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.lblMessage = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblResult = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCOM
        '
        Me.btnCOM.Location = New System.Drawing.Point(408, 202)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(110, 23)
        Me.btnCOM.TabIndex = 0
        Me.btnCOM.Text = "Run Diagnostics"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(408, 231)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(110, 23)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(12, 109)
        Me.lblMessage.MinimumSize = New System.Drawing.Size(505, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(505, 13)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Message"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(13, 13)
        Me.lblTitle.MinimumSize = New System.Drawing.Size(505, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(505, 19)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Text = "ASCOM Diagnostics"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblResult
        '
        Me.lblResult.AutoSize = True
        Me.lblResult.Location = New System.Drawing.Point(2, 207)
        Me.lblResult.MinimumSize = New System.Drawing.Size(400, 0)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblResult.Size = New System.Drawing.Size(400, 13)
        Me.lblResult.TabIndex = 4
        Me.lblResult.Text = "Label1"
        Me.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form1
        '
        Me.AcceptButton = Me.btnCOM
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(530, 266)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnCOM)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "ASCOM Diagnostics"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblResult As System.Windows.Forms.Label

End Class
