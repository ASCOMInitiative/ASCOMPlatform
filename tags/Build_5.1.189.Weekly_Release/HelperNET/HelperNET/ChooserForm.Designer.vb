<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ChooserForm
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents picASCOM As System.Windows.Forms.PictureBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdProperties As System.Windows.Forms.Button
	Public WithEvents cbDriverSelector As System.Windows.Forms.ComboBox
	Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChooserForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.picASCOM = New System.Windows.Forms.PictureBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdProperties = New System.Windows.Forms.Button
        Me.cbDriverSelector = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ChooserMenu = New System.Windows.Forms.MenuStrip
        Me.MenuTrace = New System.Windows.Forms.ToolStripMenuItem
        Me.NormallyLeaveTheseDisabledToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.MenuUseTraceAutoFilenames = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuUseTraceManualFilename = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuSerialTraceEnabled = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuProfileTraceEnabled = New System.Windows.Forms.ToolStripMenuItem
        Me.SerialTraceFileName = New System.Windows.Forms.SaveFileDialog
        Me.MenuTransformTraceEnabled = New System.Windows.Forms.ToolStripMenuItem
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ChooserMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'picASCOM
        '
        Me.picASCOM.BackColor = System.Drawing.SystemColors.Control
        Me.picASCOM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.picASCOM.Image = CType(resources.GetObject("picASCOM.Image"), System.Drawing.Image)
        Me.picASCOM.Location = New System.Drawing.Point(15, 115)
        Me.picASCOM.Name = "picASCOM"
        Me.picASCOM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.picASCOM.Size = New System.Drawing.Size(48, 56)
        Me.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picASCOM.TabIndex = 5
        Me.picASCOM.TabStop = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(242, 144)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(79, 23)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(242, 115)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(79, 23)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdProperties
        '
        Me.cmdProperties.BackColor = System.Drawing.SystemColors.Control
        Me.cmdProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdProperties.Enabled = False
        Me.cmdProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProperties.Location = New System.Drawing.Point(242, 69)
        Me.cmdProperties.Name = "cmdProperties"
        Me.cmdProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdProperties.Size = New System.Drawing.Size(79, 23)
        Me.cmdProperties.TabIndex = 1
        Me.cmdProperties.Text = "&Properties..."
        Me.cmdProperties.UseVisualStyleBackColor = False
        '
        'cbDriverSelector
        '
        Me.cbDriverSelector.BackColor = System.Drawing.SystemColors.Window
        Me.cbDriverSelector.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbDriverSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDriverSelector.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cbDriverSelector.Location = New System.Drawing.Point(15, 71)
        Me.cbDriverSelector.Name = "cbDriverSelector"
        Me.cbDriverSelector.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbDriverSelector.Size = New System.Drawing.Size(214, 21)
        Me.cbDriverSelector.Sorted = True
        Me.cbDriverSelector.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(69, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(160, 54)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Click the logo to learn more about ASCOM, a set of standards for inter-operation " & _
            "of astronomy software."
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(12, 35)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(321, 31)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "<runtime>"
        '
        'ChooserMenu
        '
        Me.ChooserMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuTrace})
        Me.ChooserMenu.Location = New System.Drawing.Point(0, 0)
        Me.ChooserMenu.Name = "ChooserMenu"
        Me.ChooserMenu.Size = New System.Drawing.Size(333, 24)
        Me.ChooserMenu.TabIndex = 7
        Me.ChooserMenu.Text = "ChooserMenu"
        '
        'MenuTrace
        '
        Me.MenuTrace.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NormallyLeaveTheseDisabledToolStripMenuItem, Me.ToolStripSeparator1, Me.MenuUseTraceAutoFilenames, Me.MenuUseTraceManualFilename, Me.MenuSerialTraceEnabled, Me.MenuProfileTraceEnabled, Me.MenuTransformTraceEnabled})
        Me.MenuTrace.Name = "MenuTrace"
        Me.MenuTrace.Size = New System.Drawing.Size(68, 20)
        Me.MenuTrace.Text = "Transform"
        '
        'NormallyLeaveTheseDisabledToolStripMenuItem
        '
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Name = "NormallyLeaveTheseDisabledToolStripMenuItem"
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Size = New System.Drawing.Size(263, 22)
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Text = "Normally leave these disabled"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(260, 6)
        '
        'MenuUseTraceAutoFilenames
        '
        Me.MenuUseTraceAutoFilenames.Name = "MenuUseTraceAutoFilenames"
        Me.MenuUseTraceAutoFilenames.Size = New System.Drawing.Size(263, 22)
        Me.MenuUseTraceAutoFilenames.Text = "Use Automatic Serial Trace Filenames"
        '
        'MenuUseTraceManualFilename
        '
        Me.MenuUseTraceManualFilename.Name = "MenuUseTraceManualFilename"
        Me.MenuUseTraceManualFilename.Size = New System.Drawing.Size(263, 22)
        Me.MenuUseTraceManualFilename.Text = "Use a Manual Serial Trace Filename"
        '
        'MenuSerialTraceEnabled
        '
        Me.MenuSerialTraceEnabled.Name = "MenuSerialTraceEnabled"
        Me.MenuSerialTraceEnabled.Size = New System.Drawing.Size(263, 22)
        Me.MenuSerialTraceEnabled.Text = "Serial Trace Enabled"
        '
        'MenuProfileTraceEnabled
        '
        Me.MenuProfileTraceEnabled.Name = "MenuProfileTraceEnabled"
        Me.MenuProfileTraceEnabled.Size = New System.Drawing.Size(263, 22)
        Me.MenuProfileTraceEnabled.Text = "Profile Trace Enabled"
        '
        'MenuTransformTraceEnabled
        '
        Me.MenuTransformTraceEnabled.Name = "MenuTransformTraceEnabled"
        Me.MenuTransformTraceEnabled.Size = New System.Drawing.Size(263, 22)
        Me.MenuTransformTraceEnabled.Text = "Transform Trace Enabled"
        '
        'ChooserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(333, 181)
        Me.Controls.Add(Me.picASCOM)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdProperties)
        Me.Controls.Add(Me.cbDriverSelector)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.ChooserMenu)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MainMenuStrip = Me.ChooserMenu
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ChooserForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ASCOM <runtime> Chooser"
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ChooserMenu.ResumeLayout(False)
        Me.ChooserMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChooserMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuTrace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SerialTraceFileName As System.Windows.Forms.SaveFileDialog
    Friend WithEvents MenuUseTraceManualFilename As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuUseTraceAutoFilenames As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSerialTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuProfileTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NormallyLeaveTheseDisabledToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuTransformTraceEnabled As System.Windows.Forms.ToolStripMenuItem
#End Region
End Class