<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ChooserForm
#Region "Windows Form Designer generated code "
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
    Public WithEvents picASCOM As System.Windows.Forms.PictureBox
	Public WithEvents BtnCancel As System.Windows.Forms.Button
	Public WithEvents BtnOK As System.Windows.Forms.Button
	Public WithEvents BtnProperties As System.Windows.Forms.Button
	Public WithEvents CmbDriverSelector As System.Windows.Forms.ComboBox
	Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChooserForm))
        Me.picASCOM = New System.Windows.Forms.PictureBox()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnProperties = New System.Windows.Forms.Button()
        Me.CmbDriverSelector = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ChooserMenu = New System.Windows.Forms.MenuStrip()
        Me.MnuTrace = New System.Windows.Forms.ToolStripMenuItem()
        Me.NormallyLeaveTheseDisabledToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuSerialTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuProfileTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRegistryTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuUtilTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSimulatorTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDriverAccessTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTransformTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNovasTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAstroUtilsTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCacheTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEarthRotationDataFormTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuAlpaca = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuDiscoverNow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuEnableDiscovery = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuDisableDiscovery = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuConfigureChooser = New System.Windows.Forms.ToolStripMenuItem()
        Me.SerialTraceFileName = New System.Windows.Forms.SaveFileDialog()
        Me.LblAlpacaDiscovery = New System.Windows.Forms.Label()
        Me.AlpacaStatus = New System.Windows.Forms.PictureBox()
        Me.DividerLine = New System.Windows.Forms.Panel()
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ChooserMenu.SuspendLayout()
        CType(Me.AlpacaStatus, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.SystemColors.Control
        Me.BtnCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnCancel.Location = New System.Drawing.Point(242, 144)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BtnCancel.Size = New System.Drawing.Size(79, 23)
        Me.BtnCancel.TabIndex = 4
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'BtnOK
        '
        Me.BtnOK.BackColor = System.Drawing.SystemColors.Control
        Me.BtnOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnOK.Enabled = False
        Me.BtnOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnOK.Location = New System.Drawing.Point(242, 115)
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BtnOK.Size = New System.Drawing.Size(79, 23)
        Me.BtnOK.TabIndex = 3
        Me.BtnOK.Text = "&OK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'BtnProperties
        '
        Me.BtnProperties.BackColor = System.Drawing.SystemColors.Control
        Me.BtnProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnProperties.Enabled = False
        Me.BtnProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnProperties.Location = New System.Drawing.Point(242, 69)
        Me.BtnProperties.Name = "BtnProperties"
        Me.BtnProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BtnProperties.Size = New System.Drawing.Size(79, 23)
        Me.BtnProperties.TabIndex = 1
        Me.BtnProperties.Text = "&Properties..."
        Me.BtnProperties.UseVisualStyleBackColor = False
        '
        'CmbDriverSelector
        '
        Me.CmbDriverSelector.BackColor = System.Drawing.SystemColors.Window
        Me.CmbDriverSelector.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmbDriverSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbDriverSelector.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CmbDriverSelector.Location = New System.Drawing.Point(15, 71)
        Me.CmbDriverSelector.Name = "CmbDriverSelector"
        Me.CmbDriverSelector.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmbDriverSelector.Size = New System.Drawing.Size(214, 21)
        Me.CmbDriverSelector.Sorted = True
        Me.CmbDriverSelector.TabIndex = 0
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
        Me.Label1.Text = "Click the logo to learn more about ASCOM, a set of standards for inter-operation " &
    "of astronomy software."
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(12, 24)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(321, 42)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "Line 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line 2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line 3"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChooserMenu
        '
        Me.ChooserMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuTrace, Me.MnuAlpaca})
        Me.ChooserMenu.Location = New System.Drawing.Point(0, 0)
        Me.ChooserMenu.Name = "ChooserMenu"
        Me.ChooserMenu.Size = New System.Drawing.Size(333, 24)
        Me.ChooserMenu.TabIndex = 7
        Me.ChooserMenu.Text = "ChooserMenu"
        '
        'MnuTrace
        '
        Me.MnuTrace.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NormallyLeaveTheseDisabledToolStripMenuItem, Me.ToolStripSeparator1, Me.MenuSerialTraceEnabled, Me.MenuProfileTraceEnabled, Me.MenuRegistryTraceEnabled, Me.MenuUtilTraceEnabled, Me.MenuSimulatorTraceEnabled, Me.MenuDriverAccessTraceEnabled, Me.MenuTransformTraceEnabled, Me.MenuNovasTraceEnabled, Me.MenuAstroUtilsTraceEnabled, Me.MenuCacheTraceEnabled, Me.MenuEarthRotationDataFormTraceEnabled})
        Me.MnuTrace.Name = "MnuTrace"
        Me.MnuTrace.Size = New System.Drawing.Size(46, 20)
        Me.MnuTrace.Text = "Trace"
        '
        'NormallyLeaveTheseDisabledToolStripMenuItem
        '
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Name = "NormallyLeaveTheseDisabledToolStripMenuItem"
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Size = New System.Drawing.Size(282, 22)
        Me.NormallyLeaveTheseDisabledToolStripMenuItem.Text = "Normally leave these disabled"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(279, 6)
        '
        'MenuSerialTraceEnabled
        '
        Me.MenuSerialTraceEnabled.Name = "MenuSerialTraceEnabled"
        Me.MenuSerialTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuSerialTraceEnabled.Text = "Serial Trace Enabled"
        '
        'MenuProfileTraceEnabled
        '
        Me.MenuProfileTraceEnabled.Name = "MenuProfileTraceEnabled"
        Me.MenuProfileTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuProfileTraceEnabled.Text = "Profile Trace Enabled"
        '
        'MenuRegistryTraceEnabled
        '
        Me.MenuRegistryTraceEnabled.Name = "MenuRegistryTraceEnabled"
        Me.MenuRegistryTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuRegistryTraceEnabled.Text = "Registry Trace Enabled"
        '
        'MenuUtilTraceEnabled
        '
        Me.MenuUtilTraceEnabled.Name = "MenuUtilTraceEnabled"
        Me.MenuUtilTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuUtilTraceEnabled.Text = "Util Trace Enabled"
        '
        'MenuSimulatorTraceEnabled
        '
        Me.MenuSimulatorTraceEnabled.Name = "MenuSimulatorTraceEnabled"
        Me.MenuSimulatorTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuSimulatorTraceEnabled.Text = "Simulator Trace Enabled"
        '
        'MenuDriverAccessTraceEnabled
        '
        Me.MenuDriverAccessTraceEnabled.Name = "MenuDriverAccessTraceEnabled"
        Me.MenuDriverAccessTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuDriverAccessTraceEnabled.Text = "DriverAccess Trace Enabled"
        '
        'MenuTransformTraceEnabled
        '
        Me.MenuTransformTraceEnabled.Name = "MenuTransformTraceEnabled"
        Me.MenuTransformTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuTransformTraceEnabled.Text = "Transform Trace Enabled"
        '
        'MenuNovasTraceEnabled
        '
        Me.MenuNovasTraceEnabled.Name = "MenuNovasTraceEnabled"
        Me.MenuNovasTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuNovasTraceEnabled.Text = "NOVAS (Partial) Trace Enabled"
        '
        'MenuAstroUtilsTraceEnabled
        '
        Me.MenuAstroUtilsTraceEnabled.Name = "MenuAstroUtilsTraceEnabled"
        Me.MenuAstroUtilsTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuAstroUtilsTraceEnabled.Text = "AstroUtils Trace Enabled"
        '
        'MenuCacheTraceEnabled
        '
        Me.MenuCacheTraceEnabled.Name = "MenuCacheTraceEnabled"
        Me.MenuCacheTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuCacheTraceEnabled.Text = "Cache Trace Enabled"
        '
        'MenuEarthRotationDataFormTraceEnabled
        '
        Me.MenuEarthRotationDataFormTraceEnabled.Name = "MenuEarthRotationDataFormTraceEnabled"
        Me.MenuEarthRotationDataFormTraceEnabled.Size = New System.Drawing.Size(282, 22)
        Me.MenuEarthRotationDataFormTraceEnabled.Text = "Earth Rotation Data Form Trace Enabled"
        '
        'MnuAlpaca
        '
        Me.MnuAlpaca.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator3, Me.MnuDiscoverNow, Me.ToolStripSeparator4, Me.MnuEnableDiscovery, Me.MnuDisableDiscovery, Me.MnuConfigureChooser})
        Me.MnuAlpaca.Name = "MnuAlpaca"
        Me.MnuAlpaca.Size = New System.Drawing.Size(55, 20)
        Me.MnuAlpaca.Text = "Alpaca"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(177, 6)
        '
        'MnuDiscoverNow
        '
        Me.MnuDiscoverNow.Name = "MnuDiscoverNow"
        Me.MnuDiscoverNow.Size = New System.Drawing.Size(180, 22)
        Me.MnuDiscoverNow.Text = "Discover Now"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(177, 6)
        '
        'MnuEnableDiscovery
        '
        Me.MnuEnableDiscovery.Name = "MnuEnableDiscovery"
        Me.MnuEnableDiscovery.Size = New System.Drawing.Size(180, 22)
        Me.MnuEnableDiscovery.Text = "Enable DIscovery"
        '
        'MnuDisableDiscovery
        '
        Me.MnuDisableDiscovery.Name = "MnuDisableDiscovery"
        Me.MnuDisableDiscovery.Size = New System.Drawing.Size(180, 22)
        Me.MnuDisableDiscovery.Text = "Disable Discovery"
        '
        'MnuConfigureChooser
        '
        Me.MnuConfigureChooser.Name = "MnuConfigureChooser"
        Me.MnuConfigureChooser.Size = New System.Drawing.Size(180, 22)
        Me.MnuConfigureChooser.Text = "Configure Chooser"
        '
        'LblAlpacaDiscovery
        '
        Me.LblAlpacaDiscovery.AutoSize = True
        Me.LblAlpacaDiscovery.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.LblAlpacaDiscovery.Location = New System.Drawing.Point(211, 5)
        Me.LblAlpacaDiscovery.Name = "LblAlpacaDiscovery"
        Me.LblAlpacaDiscovery.Size = New System.Drawing.Size(90, 13)
        Me.LblAlpacaDiscovery.TabIndex = 9
        Me.LblAlpacaDiscovery.Text = "Alpaca Discovery"
        '
        'AlpacaStatus
        '
        Me.AlpacaStatus.BackColor = System.Drawing.Color.CornflowerBlue
        Me.AlpacaStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AlpacaStatus.Location = New System.Drawing.Point(305, 8)
        Me.AlpacaStatus.Name = "AlpacaStatus"
        Me.AlpacaStatus.Size = New System.Drawing.Size(16, 8)
        Me.AlpacaStatus.TabIndex = 10
        Me.AlpacaStatus.TabStop = False
        '
        'DividerLine
        '
        Me.DividerLine.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.DividerLine.Location = New System.Drawing.Point(15, 102)
        Me.DividerLine.Name = "DividerLine"
        Me.DividerLine.Size = New System.Drawing.Size(306, 1)
        Me.DividerLine.TabIndex = 11
        '
        'ChooserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(333, 181)
        Me.Controls.Add(Me.DividerLine)
        Me.Controls.Add(Me.LblAlpacaDiscovery)
        Me.Controls.Add(Me.AlpacaStatus)
        Me.Controls.Add(Me.picASCOM)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnOK)
        Me.Controls.Add(Me.BtnProperties)
        Me.Controls.Add(Me.CmbDriverSelector)
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
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ASCOM <runtime> Chooser"
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ChooserMenu.ResumeLayout(False)
        Me.ChooserMenu.PerformLayout()
        CType(Me.AlpacaStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChooserMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents MnuTrace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SerialTraceFileName As System.Windows.Forms.SaveFileDialog
    Friend WithEvents MenuSerialTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuProfileTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NormallyLeaveTheseDisabledToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuTransformTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuUtilTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSimulatorTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuDriverAccessTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAstroUtilsTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuNovasTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuCacheTraceEnabled As ToolStripMenuItem
    Friend WithEvents MenuEarthRotationDataFormTraceEnabled As ToolStripMenuItem
    Friend WithEvents MnuAlpaca As ToolStripMenuItem
    Friend WithEvents MnuEnableDiscovery As ToolStripMenuItem
    Friend WithEvents MnuDiscoverNow As ToolStripMenuItem
    Friend WithEvents MnuConfigureChooser As ToolStripMenuItem
    Friend WithEvents LblAlpacaDiscovery As Label
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents MnuDisableDiscovery As ToolStripMenuItem
    Friend WithEvents AlpacaStatus As PictureBox
    Friend WithEvents MenuRegistryTraceEnabled As ToolStripMenuItem
    Friend WithEvents DividerLine As Panel
#End Region
End Class