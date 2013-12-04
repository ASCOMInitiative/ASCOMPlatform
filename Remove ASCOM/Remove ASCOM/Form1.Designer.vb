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
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.txtWarning = New System.Windows.Forms.TextBox()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.LblAction = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(623, 437)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(227, 242)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(260, 76)
        Me.btnRemove.TabIndex = 2
        Me.btnRemove.Text = "Remove ASCOM"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'txtWarning
        '
        Me.txtWarning.BackColor = System.Drawing.Color.Red
        Me.txtWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWarning.ForeColor = System.Drawing.SystemColors.Window
        Me.txtWarning.Location = New System.Drawing.Point(12, 28)
        Me.txtWarning.Multiline = True
        Me.txtWarning.Name = "txtWarning"
        Me.txtWarning.ReadOnly = True
        Me.txtWarning.Size = New System.Drawing.Size(686, 120)
        Me.txtWarning.TabIndex = 3
        Me.txtWarning.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WARNING!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This tool will forcibly remove your entire ASCOM Platform including" & _
    " your drivers and Profile." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use it only as a last resort, when normal u" & _
    "ninstall fails!"
        Me.txtWarning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblResult
        '
        Me.lblResult.AutoSize = True
        Me.lblResult.Location = New System.Drawing.Point(9, 418)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(51, 13)
        Me.lblResult.TabIndex = 4
        Me.lblResult.Text = "LblStatus"
        '
        'LblAction
        '
        Me.LblAction.AutoSize = True
        Me.LblAction.Location = New System.Drawing.Point(9, 442)
        Me.LblAction.Name = "LblAction"
        Me.LblAction.Size = New System.Drawing.Size(51, 13)
        Me.LblAction.TabIndex = 5
        Me.LblAction.Text = "LblAction"
        '
        'Form1
        '
        Me.AcceptButton = Me.btnExit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(710, 472)
        Me.Controls.Add(Me.LblAction)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.txtWarning)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnExit)
        Me.Name = "Form1"
        Me.Text = "Remove ASCOM by Force!"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Private WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents txtWarning As System.Windows.Forms.TextBox
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents LblAction As System.Windows.Forms.Label

End Class
