<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagnosticsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DiagnosticsForm))
        Me.btnCOM = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.lblMessage = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblResult = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ChooserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChooserToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ChooserNETToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ConnectToDeviceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ListAvailableCOMPortsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblAction = New System.Windows.Forms.Label
        Me.btnLastLog = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
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
        Me.lblTitle.Location = New System.Drawing.Point(11, 41)
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
        Me.lblResult.Location = New System.Drawing.Point(2, 206)
        Me.lblResult.MinimumSize = New System.Drawing.Size(400, 0)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblResult.Size = New System.Drawing.Size(400, 13)
        Me.lblResult.TabIndex = 4
        Me.lblResult.Text = "Label1"
        Me.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChooserToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(530, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ChooserToolStripMenuItem
        '
        Me.ChooserToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChooserToolStripMenuItem1, Me.ChooserNETToolStripMenuItem, Me.ConnectToDeviceToolStripMenuItem, Me.ListAvailableCOMPortsToolStripMenuItem})
        Me.ChooserToolStripMenuItem.Name = "ChooserToolStripMenuItem"
        Me.ChooserToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ChooserToolStripMenuItem.Text = "Tools"
        '
        'ChooserToolStripMenuItem1
        '
        Me.ChooserToolStripMenuItem1.Name = "ChooserToolStripMenuItem1"
        Me.ChooserToolStripMenuItem1.Size = New System.Drawing.Size(245, 22)
        Me.ChooserToolStripMenuItem1.Text = "Telescope Chooser (using COM)"
        '
        'ChooserNETToolStripMenuItem
        '
        Me.ChooserNETToolStripMenuItem.Name = "ChooserNETToolStripMenuItem"
        Me.ChooserNETToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.ChooserNETToolStripMenuItem.Text = "Telescope Chooser (using .NET)"
        '
        'ConnectToDeviceToolStripMenuItem
        '
        Me.ConnectToDeviceToolStripMenuItem.Name = "ConnectToDeviceToolStripMenuItem"
        Me.ConnectToDeviceToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.ConnectToDeviceToolStripMenuItem.Text = "Connect to Device"
        '
        'ListAvailableCOMPortsToolStripMenuItem
        '
        Me.ListAvailableCOMPortsToolStripMenuItem.Name = "ListAvailableCOMPortsToolStripMenuItem"
        Me.ListAvailableCOMPortsToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.ListAvailableCOMPortsToolStripMenuItem.Text = "List Available COM Ports"
        '
        'lblAction
        '
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(12, 236)
        Me.lblAction.MinimumSize = New System.Drawing.Size(380, 0)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAction.Size = New System.Drawing.Size(380, 13)
        Me.lblAction.TabIndex = 6
        Me.lblAction.Text = "Label1"
        Me.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnLastLog
        '
        Me.btnLastLog.Enabled = False
        Me.btnLastLog.Location = New System.Drawing.Point(408, 173)
        Me.btnLastLog.Name = "btnLastLog"
        Me.btnLastLog.Size = New System.Drawing.Size(108, 23)
        Me.btnLastLog.TabIndex = 7
        Me.btnLastLog.Text = "View Last Log"
        Me.btnLastLog.UseVisualStyleBackColor = True
        '
        'DiagnosticsForm
        '
        Me.AcceptButton = Me.btnCOM
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(530, 266)
        Me.Controls.Add(Me.btnLastLog)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnCOM)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "DiagnosticsForm"
        Me.Text = "ASCOM Diagnostics"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ChooserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooserToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooserNETToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConnectToDeviceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents ListAvailableCOMPortsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLastLog As System.Windows.Forms.Button

End Class
