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
        Me.btnCOM = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuChooseDevice = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChooserToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChooserNETToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListAvailableCOMPortsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTrace = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLeaveUnset = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuUseTraceAutoFilenames = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuUseTraceManualFilename = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSerialTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuIncludeSerialTraceDebugInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuProfileTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuUtilTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTimerTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSimulatorTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDriverAccessTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTransformTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNovasTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAstroUtilsTraceEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuThrowAbandonedMutexExceptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.btnLastLog = New System.Windows.Forms.Button()
        Me.SerialTraceFileName = New System.Windows.Forms.SaveFileDialog()
        Me.SerialWaitTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWaitTypeSleep = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWaitTypeManualResetEvent = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWaitTypeWaitForSingleObject = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuChooseDevice, Me.mnuTools, Me.mnuTrace, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(530, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuChooseDevice
        '
        Me.mnuChooseDevice.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem, Me.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem})
        Me.mnuChooseDevice.Name = "mnuChooseDevice"
        Me.mnuChooseDevice.Size = New System.Drawing.Size(97, 20)
        Me.mnuChooseDevice.Text = "Choose Device"
        '
        'ChooseAndConnectToDevice64bitApplicationToolStripMenuItem
        '
        Me.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Name = "ChooseAndConnectToDevice64bitApplicationToolStripMenuItem"
        Me.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Text = "Choose and Connect to Device"
        '
        'ChooseAndConnectToDevice32bitApplicationToolStripMenuItem
        '
        Me.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Name = "ChooseAndConnectToDevice32bitApplicationToolStripMenuItem"
        Me.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Text = "Choose and Connect to Device (32bit application)"
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChooserToolStripMenuItem1, Me.ChooserNETToolStripMenuItem, Me.ListAvailableCOMPortsToolStripMenuItem})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(48, 20)
        Me.mnuTools.Text = "Tools"
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
        'ListAvailableCOMPortsToolStripMenuItem
        '
        Me.ListAvailableCOMPortsToolStripMenuItem.Name = "ListAvailableCOMPortsToolStripMenuItem"
        Me.ListAvailableCOMPortsToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.ListAvailableCOMPortsToolStripMenuItem.Text = "List Available COM Ports"
        '
        'mnuTrace
        '
        Me.mnuTrace.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLeaveUnset, Me.ToolStripSeparator1, Me.MenuUseTraceAutoFilenames, Me.MenuUseTraceManualFilename, Me.MenuSerialTraceEnabled, Me.MenuIncludeSerialTraceDebugInformation, Me.MenuProfileTraceEnabled, Me.MenuUtilTraceEnabled, Me.MenuTimerTraceEnabled, Me.MenuSimulatorTraceEnabled, Me.MenuDriverAccessTraceEnabled, Me.MenuTransformTraceEnabled, Me.MenuNovasTraceEnabled, Me.MenuAstroUtilsTraceEnabled, Me.MenuThrowAbandonedMutexExceptions, Me.SerialWaitTypeToolStripMenuItem})
        Me.mnuTrace.Name = "mnuTrace"
        Me.mnuTrace.Size = New System.Drawing.Size(48, 20)
        Me.mnuTrace.Text = "Trace"
        '
        'mnuLeaveUnset
        '
        Me.mnuLeaveUnset.Name = "mnuLeaveUnset"
        Me.mnuLeaveUnset.Size = New System.Drawing.Size(280, 22)
        Me.mnuLeaveUnset.Text = "Normally leave these options disabled"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(277, 6)
        '
        'MenuUseTraceAutoFilenames
        '
        Me.MenuUseTraceAutoFilenames.Name = "MenuUseTraceAutoFilenames"
        Me.MenuUseTraceAutoFilenames.Size = New System.Drawing.Size(280, 22)
        Me.MenuUseTraceAutoFilenames.Text = "Use Automatic Serial Trace Filenames"
        '
        'MenuUseTraceManualFilename
        '
        Me.MenuUseTraceManualFilename.Name = "MenuUseTraceManualFilename"
        Me.MenuUseTraceManualFilename.Size = New System.Drawing.Size(280, 22)
        Me.MenuUseTraceManualFilename.Text = "Use a Manual Serial Trace Filename"
        '
        'MenuSerialTraceEnabled
        '
        Me.MenuSerialTraceEnabled.Name = "MenuSerialTraceEnabled"
        Me.MenuSerialTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuSerialTraceEnabled.Text = "Serial Trace Enabled"
        '
        'MenuIncludeSerialTraceDebugInformation
        '
        Me.MenuIncludeSerialTraceDebugInformation.Name = "MenuIncludeSerialTraceDebugInformation"
        Me.MenuIncludeSerialTraceDebugInformation.Size = New System.Drawing.Size(280, 22)
        Me.MenuIncludeSerialTraceDebugInformation.Text = "Include Serial Trace Debug Information"
        '
        'MenuProfileTraceEnabled
        '
        Me.MenuProfileTraceEnabled.Name = "MenuProfileTraceEnabled"
        Me.MenuProfileTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuProfileTraceEnabled.Text = "Profile Trace Enabled"
        '
        'MenuUtilTraceEnabled
        '
        Me.MenuUtilTraceEnabled.Name = "MenuUtilTraceEnabled"
        Me.MenuUtilTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuUtilTraceEnabled.Text = "Util Trace Enabled"
        '
        'MenuTimerTraceEnabled
        '
        Me.MenuTimerTraceEnabled.Name = "MenuTimerTraceEnabled"
        Me.MenuTimerTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuTimerTraceEnabled.Text = "Timer Timer Enabled"
        '
        'MenuSimulatorTraceEnabled
        '
        Me.MenuSimulatorTraceEnabled.Name = "MenuSimulatorTraceEnabled"
        Me.MenuSimulatorTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuSimulatorTraceEnabled.Text = "Simulator Trace Enabled"
        '
        'MenuDriverAccessTraceEnabled
        '
        Me.MenuDriverAccessTraceEnabled.Name = "MenuDriverAccessTraceEnabled"
        Me.MenuDriverAccessTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuDriverAccessTraceEnabled.Text = "DriverAccess Trace Enabled"
        '
        'MenuTransformTraceEnabled
        '
        Me.MenuTransformTraceEnabled.Name = "MenuTransformTraceEnabled"
        Me.MenuTransformTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuTransformTraceEnabled.Text = "Transform Trace Enabled"
        '
        'MenuNovasTraceEnabled
        '
        Me.MenuNovasTraceEnabled.Name = "MenuNovasTraceEnabled"
        Me.MenuNovasTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuNovasTraceEnabled.Text = "NOVAS (Partial) Trace Enabled"
        '
        'MenuAstroUtilsTraceEnabled
        '
        Me.MenuAstroUtilsTraceEnabled.Name = "MenuAstroUtilsTraceEnabled"
        Me.MenuAstroUtilsTraceEnabled.Size = New System.Drawing.Size(280, 22)
        Me.MenuAstroUtilsTraceEnabled.Text = "AstroUtils Trace Enabled"
        '
        'MenuThrowAbandonedMutexExceptions
        '
        Me.MenuThrowAbandonedMutexExceptions.Name = "MenuThrowAbandonedMutexExceptions"
        Me.MenuThrowAbandonedMutexExceptions.Size = New System.Drawing.Size(280, 22)
        Me.MenuThrowAbandonedMutexExceptions.Text = "Throw Abandoned Mutex Exceptions"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'lblAction
        '
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(22, 236)
        Me.lblAction.MinimumSize = New System.Drawing.Size(380, 0)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAction.Size = New System.Drawing.Size(380, 13)
        Me.lblAction.TabIndex = 6
        Me.lblAction.Text = "Label1"
        Me.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'SerialWaitTypeToolStripMenuItem
        '
        Me.SerialWaitTypeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuWaitTypeManualResetEvent, Me.MenuWaitTypeSleep, Me.MenuWaitTypeWaitForSingleObject})
        Me.SerialWaitTypeToolStripMenuItem.Name = "SerialWaitTypeToolStripMenuItem"
        Me.SerialWaitTypeToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.SerialWaitTypeToolStripMenuItem.Text = "Serial Wait Type"
        '
        'MenuWaitTypeSleep
        '
        Me.MenuWaitTypeSleep.Name = "MenuWaitTypeSleep"
        Me.MenuWaitTypeSleep.Size = New System.Drawing.Size(182, 22)
        Me.MenuWaitTypeSleep.Text = "Sleep"
        '
        'MenuWaitTypeManualResetEvent
        '
        Me.MenuWaitTypeManualResetEvent.Name = "MenuWaitTypeManualResetEvent"
        Me.MenuWaitTypeManualResetEvent.Size = New System.Drawing.Size(182, 22)
        Me.MenuWaitTypeManualResetEvent.Text = "ManualResetEvent"
        '
        'MenuWaitTypeWaitForSingleObject
        '
        Me.MenuWaitTypeWaitForSingleObject.Name = "MenuWaitTypeWaitForSingleObject"
        Me.MenuWaitTypeWaitForSingleObject.Size = New System.Drawing.Size(182, 22)
        Me.MenuWaitTypeWaitForSingleObject.Text = "WaitForSingleObject"
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
    Friend WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooserToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooserNETToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents ListAvailableCOMPortsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLastLog As System.Windows.Forms.Button
    Friend WithEvents mnuChooseDevice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooseAndConnectToDevice64bitApplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTrace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLeaveUnset As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuUseTraceAutoFilenames As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuUseTraceManualFilename As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSerialTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuIncludeSerialTraceDebugInformation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuProfileTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuTransformTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuUtilTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuTimerTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSimulatorTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuDriverAccessTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuThrowAbandonedMutexExceptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAstroUtilsTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuNovasTraceEnabled As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChooseAndConnectToDevice32bitApplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SerialTraceFileName As System.Windows.Forms.SaveFileDialog
    Friend WithEvents SerialWaitTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWaitTypeSleep As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWaitTypeManualResetEvent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWaitTypeWaitForSingleObject As System.Windows.Forms.ToolStripMenuItem

End Class
