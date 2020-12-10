<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EarthRotationDataForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EarthRotationDataForm))
        Me.CmbDataSource = New System.Windows.Forms.ComboBox()
        Me.GrpManualUpdate = New System.Windows.Forms.GroupBox()
        Me.LblManualDeltaUT1 = New System.Windows.Forms.Label()
        Me.LblManualLeapSeconds = New System.Windows.Forms.Label()
        Me.TxtManualLeapSeconds = New System.Windows.Forms.TextBox()
        Me.TxtManualDeltaUT1 = New System.Windows.Forms.TextBox()
        Me.GrpOnDemandAndAutomaticUpdateConfiguration = New System.Windows.Forms.GroupBox()
        Me.LblRunStatus = New System.Windows.Forms.Label()
        Me.TxtRunStatus = New System.Windows.Forms.TextBox()
        Me.BtnRunAutomaticUpdate = New System.Windows.Forms.Button()
        Me.LblTraceEnabled = New System.Windows.Forms.Label()
        Me.ChkTraceEnabled = New System.Windows.Forms.CheckBox()
        Me.TxtTraceFilePath = New System.Windows.Forms.TextBox()
        Me.BtnSetTraceDirectory = New System.Windows.Forms.Button()
        Me.LblAutoSeconds = New System.Windows.Forms.Label()
        Me.TxtLastRun = New System.Windows.Forms.TextBox()
        Me.LblLastRun = New System.Windows.Forms.Label()
        Me.LblAutoTimeout = New System.Windows.Forms.Label()
        Me.LblAutoDataSource = New System.Windows.Forms.Label()
        Me.TxtDownloadTimeout = New System.Windows.Forms.TextBox()
        Me.LblNextLeapSecondsDate = New System.Windows.Forms.Label()
        Me.LblNextLeapSeconds = New System.Windows.Forms.Label()
        Me.TxtNextLeapSeconds = New System.Windows.Forms.TextBox()
        Me.TxtNextLeapSecondsDate = New System.Windows.Forms.TextBox()
        Me.LblAutoRepeatFrequency = New System.Windows.Forms.Label()
        Me.LblAutoDownloadTime = New System.Windows.Forms.Label()
        Me.CmbScheduleRepeat = New System.Windows.Forms.ComboBox()
        Me.DateScheduleRun = New System.Windows.Forms.DateTimePicker()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CmbUpdateType = New System.Windows.Forms.ComboBox()
        Me.GrpUpdateType = New System.Windows.Forms.GroupBox()
        Me.FolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.GrpStatus = New System.Windows.Forms.GroupBox()
        Me.LblNow = New System.Windows.Forms.Label()
        Me.TxtNow = New System.Windows.Forms.TextBox()
        Me.TxtEffectiveDeltaUT1 = New System.Windows.Forms.TextBox()
        Me.TxtEffectiveLeapSeconds = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GrpScheduleTime = New System.Windows.Forms.GroupBox()
        Me.BtnHelp = New System.Windows.Forms.Button()
        Me.GrpManualUpdate.SuspendLayout()
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpUpdateType.SuspendLayout()
        Me.GrpStatus.SuspendLayout()
        Me.GrpScheduleTime.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmbDataSource
        '
        Me.CmbDataSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbDataSource.FormattingEnabled = True
        Me.CmbDataSource.Location = New System.Drawing.Point(202, 36)
        Me.CmbDataSource.Name = "CmbDataSource"
        Me.CmbDataSource.Size = New System.Drawing.Size(250, 21)
        Me.CmbDataSource.TabIndex = 2
        '
        'GrpManualUpdate
        '
        Me.GrpManualUpdate.Controls.Add(Me.LblManualDeltaUT1)
        Me.GrpManualUpdate.Controls.Add(Me.LblManualLeapSeconds)
        Me.GrpManualUpdate.Controls.Add(Me.TxtManualLeapSeconds)
        Me.GrpManualUpdate.Controls.Add(Me.TxtManualDeltaUT1)
        Me.GrpManualUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpManualUpdate.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpManualUpdate.Location = New System.Drawing.Point(14, 113)
        Me.GrpManualUpdate.Name = "GrpManualUpdate"
        Me.GrpManualUpdate.Size = New System.Drawing.Size(434, 93)
        Me.GrpManualUpdate.TabIndex = 3
        Me.GrpManualUpdate.TabStop = False
        Me.GrpManualUpdate.Text = "Specified values"
        '
        'LblManualDeltaUT1
        '
        Me.LblManualDeltaUT1.AutoSize = True
        Me.LblManualDeltaUT1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblManualDeltaUT1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblManualDeltaUT1.Location = New System.Drawing.Point(71, 64)
        Me.LblManualDeltaUT1.Name = "LblManualDeltaUT1"
        Me.LblManualDeltaUT1.Size = New System.Drawing.Size(117, 13)
        Me.LblManualDeltaUT1.TabIndex = 15
        Me.LblManualDeltaUT1.Text = "UT1 - UTC (Delta UT1)"
        '
        'LblManualLeapSeconds
        '
        Me.LblManualLeapSeconds.AutoSize = True
        Me.LblManualLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblManualLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblManualLeapSeconds.Location = New System.Drawing.Point(55, 38)
        Me.LblManualLeapSeconds.Name = "LblManualLeapSeconds"
        Me.LblManualLeapSeconds.Size = New System.Drawing.Size(133, 13)
        Me.LblManualLeapSeconds.TabIndex = 16
        Me.LblManualLeapSeconds.Text = "TAI - UTC (Leap Seconds)"
        '
        'TxtManualLeapSeconds
        '
        Me.TxtManualLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtManualLeapSeconds.Location = New System.Drawing.Point(195, 35)
        Me.TxtManualLeapSeconds.Name = "TxtManualLeapSeconds"
        Me.TxtManualLeapSeconds.Size = New System.Drawing.Size(45, 20)
        Me.TxtManualLeapSeconds.TabIndex = 8
        '
        'TxtManualDeltaUT1
        '
        Me.TxtManualDeltaUT1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtManualDeltaUT1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TxtManualDeltaUT1.Location = New System.Drawing.Point(195, 61)
        Me.TxtManualDeltaUT1.Name = "TxtManualDeltaUT1"
        Me.TxtManualDeltaUT1.Size = New System.Drawing.Size(100, 20)
        Me.TxtManualDeltaUT1.TabIndex = 10
        '
        'GrpOnDemandAndAutomaticUpdateConfiguration
        '
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblRunStatus)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.TxtRunStatus)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.BtnRunAutomaticUpdate)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblTraceEnabled)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.ChkTraceEnabled)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.TxtTraceFilePath)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.BtnSetTraceDirectory)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblAutoSeconds)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.TxtLastRun)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblLastRun)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblAutoTimeout)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.LblAutoDataSource)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.CmbDataSource)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(Me.TxtDownloadTimeout)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Location = New System.Drawing.Point(14, 238)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Name = "GrpOnDemandAndAutomaticUpdateConfiguration"
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Size = New System.Drawing.Size(698, 228)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.TabIndex = 7
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.TabStop = False
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.Text = "On Demand and Automatic Update Configuration"
        '
        'LblRunStatus
        '
        Me.LblRunStatus.AutoSize = True
        Me.LblRunStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRunStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblRunStatus.Location = New System.Drawing.Point(123, 198)
        Me.LblRunStatus.Name = "LblRunStatus"
        Me.LblRunStatus.Size = New System.Drawing.Size(73, 13)
        Me.LblRunStatus.TabIndex = 27
        Me.LblRunStatus.Text = "Update status"
        '
        'TxtRunStatus
        '
        Me.TxtRunStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRunStatus.Location = New System.Drawing.Point(202, 195)
        Me.TxtRunStatus.Name = "TxtRunStatus"
        Me.TxtRunStatus.ReadOnly = True
        Me.TxtRunStatus.Size = New System.Drawing.Size(321, 20)
        Me.TxtRunStatus.TabIndex = 26
        '
        'BtnRunAutomaticUpdate
        '
        Me.BtnRunAutomaticUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRunAutomaticUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnRunAutomaticUpdate.Location = New System.Drawing.Point(550, 167)
        Me.BtnRunAutomaticUpdate.Name = "BtnRunAutomaticUpdate"
        Me.BtnRunAutomaticUpdate.Size = New System.Drawing.Size(119, 23)
        Me.BtnRunAutomaticUpdate.TabIndex = 24
        Me.BtnRunAutomaticUpdate.Text = "On Demand Update"
        Me.BtnRunAutomaticUpdate.UseVisualStyleBackColor = True
        '
        'LblTraceEnabled
        '
        Me.LblTraceEnabled.AutoSize = True
        Me.LblTraceEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraceEnabled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTraceEnabled.Location = New System.Drawing.Point(119, 91)
        Me.LblTraceEnabled.Name = "LblTraceEnabled"
        Me.LblTraceEnabled.Size = New System.Drawing.Size(77, 13)
        Me.LblTraceEnabled.TabIndex = 22
        Me.LblTraceEnabled.Text = "Trace Enabled"
        '
        'ChkTraceEnabled
        '
        Me.ChkTraceEnabled.AutoSize = True
        Me.ChkTraceEnabled.Location = New System.Drawing.Point(202, 91)
        Me.ChkTraceEnabled.Name = "ChkTraceEnabled"
        Me.ChkTraceEnabled.Size = New System.Drawing.Size(15, 14)
        Me.ChkTraceEnabled.TabIndex = 21
        Me.ChkTraceEnabled.UseVisualStyleBackColor = True
        '
        'TxtTraceFilePath
        '
        Me.TxtTraceFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTraceFilePath.Location = New System.Drawing.Point(202, 111)
        Me.TxtTraceFilePath.Name = "TxtTraceFilePath"
        Me.TxtTraceFilePath.ReadOnly = True
        Me.TxtTraceFilePath.Size = New System.Drawing.Size(321, 20)
        Me.TxtTraceFilePath.TabIndex = 19
        '
        'BtnSetTraceDirectory
        '
        Me.BtnSetTraceDirectory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetTraceDirectory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnSetTraceDirectory.Location = New System.Drawing.Point(74, 109)
        Me.BtnSetTraceDirectory.Name = "BtnSetTraceDirectory"
        Me.BtnSetTraceDirectory.Size = New System.Drawing.Size(122, 23)
        Me.BtnSetTraceDirectory.TabIndex = 18
        Me.BtnSetTraceDirectory.Text = "Select Trace Directory"
        Me.BtnSetTraceDirectory.UseVisualStyleBackColor = True
        '
        'LblAutoSeconds
        '
        Me.LblAutoSeconds.AutoSize = True
        Me.LblAutoSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoSeconds.Location = New System.Drawing.Point(262, 66)
        Me.LblAutoSeconds.Name = "LblAutoSeconds"
        Me.LblAutoSeconds.Size = New System.Drawing.Size(49, 13)
        Me.LblAutoSeconds.TabIndex = 17
        Me.LblAutoSeconds.Text = "Seconds"
        '
        'TxtLastRun
        '
        Me.TxtLastRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLastRun.Location = New System.Drawing.Point(202, 169)
        Me.TxtLastRun.Name = "TxtLastRun"
        Me.TxtLastRun.ReadOnly = True
        Me.TxtLastRun.Size = New System.Drawing.Size(321, 20)
        Me.TxtLastRun.TabIndex = 19
        '
        'LblLastRun
        '
        Me.LblLastRun.AutoSize = True
        Me.LblLastRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastRun.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblLastRun.Location = New System.Drawing.Point(133, 172)
        Me.LblLastRun.Name = "LblLastRun"
        Me.LblLastRun.Size = New System.Drawing.Size(63, 13)
        Me.LblLastRun.TabIndex = 18
        Me.LblLastRun.Text = "Last update"
        '
        'LblAutoTimeout
        '
        Me.LblAutoTimeout.AutoSize = True
        Me.LblAutoTimeout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoTimeout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoTimeout.Location = New System.Drawing.Point(104, 66)
        Me.LblAutoTimeout.Name = "LblAutoTimeout"
        Me.LblAutoTimeout.Size = New System.Drawing.Size(92, 13)
        Me.LblAutoTimeout.TabIndex = 13
        Me.LblAutoTimeout.Text = "Download timeout"
        '
        'LblAutoDataSource
        '
        Me.LblAutoDataSource.AutoSize = True
        Me.LblAutoDataSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoDataSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoDataSource.Location = New System.Drawing.Point(90, 39)
        Me.LblAutoDataSource.Name = "LblAutoDataSource"
        Me.LblAutoDataSource.Size = New System.Drawing.Size(106, 13)
        Me.LblAutoDataSource.TabIndex = 14
        Me.LblAutoDataSource.Text = "Internet Data Source"
        '
        'TxtDownloadTimeout
        '
        Me.TxtDownloadTimeout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDownloadTimeout.Location = New System.Drawing.Point(202, 63)
        Me.TxtDownloadTimeout.Name = "TxtDownloadTimeout"
        Me.TxtDownloadTimeout.Size = New System.Drawing.Size(45, 20)
        Me.TxtDownloadTimeout.TabIndex = 4
        '
        'LblNextLeapSecondsDate
        '
        Me.LblNextLeapSecondsDate.AutoSize = True
        Me.LblNextLeapSecondsDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNextLeapSecondsDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNextLeapSecondsDate.Location = New System.Drawing.Point(28, 139)
        Me.LblNextLeapSecondsDate.Name = "LblNextLeapSecondsDate"
        Me.LblNextLeapSecondsDate.Size = New System.Drawing.Size(130, 13)
        Me.LblNextLeapSecondsDate.TabIndex = 21
        Me.LblNextLeapSecondsDate.Text = "Start of next leap seconds"
        '
        'LblNextLeapSeconds
        '
        Me.LblNextLeapSeconds.AutoSize = True
        Me.LblNextLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNextLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNextLeapSeconds.Location = New System.Drawing.Point(63, 113)
        Me.LblNextLeapSeconds.Name = "LblNextLeapSeconds"
        Me.LblNextLeapSeconds.Size = New System.Drawing.Size(95, 13)
        Me.LblNextLeapSeconds.TabIndex = 23
        Me.LblNextLeapSeconds.Text = "Next leap seconds"
        '
        'TxtNextLeapSeconds
        '
        Me.TxtNextLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNextLeapSeconds.Location = New System.Drawing.Point(164, 110)
        Me.TxtNextLeapSeconds.Name = "TxtNextLeapSeconds"
        Me.TxtNextLeapSeconds.ReadOnly = True
        Me.TxtNextLeapSeconds.Size = New System.Drawing.Size(100, 20)
        Me.TxtNextLeapSeconds.TabIndex = 22
        '
        'TxtNextLeapSecondsDate
        '
        Me.TxtNextLeapSecondsDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNextLeapSecondsDate.Location = New System.Drawing.Point(164, 136)
        Me.TxtNextLeapSecondsDate.Name = "TxtNextLeapSecondsDate"
        Me.TxtNextLeapSecondsDate.ReadOnly = True
        Me.TxtNextLeapSecondsDate.Size = New System.Drawing.Size(250, 20)
        Me.TxtNextLeapSecondsDate.TabIndex = 20
        '
        'LblAutoRepeatFrequency
        '
        Me.LblAutoRepeatFrequency.AutoSize = True
        Me.LblAutoRepeatFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoRepeatFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoRepeatFrequency.Location = New System.Drawing.Point(94, 65)
        Me.LblAutoRepeatFrequency.Name = "LblAutoRepeatFrequency"
        Me.LblAutoRepeatFrequency.Size = New System.Drawing.Size(95, 13)
        Me.LblAutoRepeatFrequency.TabIndex = 16
        Me.LblAutoRepeatFrequency.Text = "Repeat Frequency"
        '
        'LblAutoDownloadTime
        '
        Me.LblAutoDownloadTime.AutoSize = True
        Me.LblAutoDownloadTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoDownloadTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoDownloadTime.Location = New System.Drawing.Point(34, 38)
        Me.LblAutoDownloadTime.Name = "LblAutoDownloadTime"
        Me.LblAutoDownloadTime.Size = New System.Drawing.Size(155, 13)
        Me.LblAutoDownloadTime.TabIndex = 15
        Me.LblAutoDownloadTime.Text = "Initial Download Date and Time"
        '
        'CmbScheduleRepeat
        '
        Me.CmbScheduleRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbScheduleRepeat.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbScheduleRepeat.FormattingEnabled = True
        Me.CmbScheduleRepeat.Location = New System.Drawing.Point(195, 62)
        Me.CmbScheduleRepeat.Name = "CmbScheduleRepeat"
        Me.CmbScheduleRepeat.Size = New System.Drawing.Size(111, 21)
        Me.CmbScheduleRepeat.TabIndex = 12
        '
        'DateScheduleRun
        '
        Me.DateScheduleRun.CustomFormat = "dddd dd MMM yyyy  -  HH:mm:ss"
        Me.DateScheduleRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateScheduleRun.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateScheduleRun.Location = New System.Drawing.Point(195, 35)
        Me.DateScheduleRun.Name = "DateScheduleRun"
        Me.DateScheduleRun.Size = New System.Drawing.Size(250, 20)
        Me.DateScheduleRun.TabIndex = 11
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnClose.Location = New System.Drawing.Point(859, 575)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 5
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'CmbUpdateType
        '
        Me.CmbUpdateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbUpdateType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbUpdateType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmbUpdateType.FormattingEnabled = True
        Me.CmbUpdateType.Location = New System.Drawing.Point(17, 31)
        Me.CmbUpdateType.Name = "CmbUpdateType"
        Me.CmbUpdateType.Size = New System.Drawing.Size(278, 21)
        Me.CmbUpdateType.TabIndex = 8
        '
        'GrpUpdateType
        '
        Me.GrpUpdateType.Controls.Add(Me.CmbUpdateType)
        Me.GrpUpdateType.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpUpdateType.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpUpdateType.Location = New System.Drawing.Point(14, 12)
        Me.GrpUpdateType.Name = "GrpUpdateType"
        Me.GrpUpdateType.Size = New System.Drawing.Size(323, 72)
        Me.GrpUpdateType.TabIndex = 9
        Me.GrpUpdateType.TabStop = False
        Me.GrpUpdateType.Text = "ASCOM Platform Data Source"
        '
        'FolderBrowser
        '
        Me.FolderBrowser.Description = "Select a trace file directory (normally Documents\ASCOM)"
        Me.FolderBrowser.RootFolder = System.Environment.SpecialFolder.MyDocuments
        '
        'GrpStatus
        '
        Me.GrpStatus.Controls.Add(Me.LblNow)
        Me.GrpStatus.Controls.Add(Me.TxtNow)
        Me.GrpStatus.Controls.Add(Me.TxtEffectiveDeltaUT1)
        Me.GrpStatus.Controls.Add(Me.LblNextLeapSecondsDate)
        Me.GrpStatus.Controls.Add(Me.TxtEffectiveLeapSeconds)
        Me.GrpStatus.Controls.Add(Me.LblNextLeapSeconds)
        Me.GrpStatus.Controls.Add(Me.Label6)
        Me.GrpStatus.Controls.Add(Me.TxtNextLeapSecondsDate)
        Me.GrpStatus.Controls.Add(Me.TxtNextLeapSeconds)
        Me.GrpStatus.Controls.Add(Me.Label2)
        Me.GrpStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpStatus.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpStatus.Location = New System.Drawing.Point(495, 34)
        Me.GrpStatus.Name = "GrpStatus"
        Me.GrpStatus.Size = New System.Drawing.Size(439, 172)
        Me.GrpStatus.TabIndex = 26
        Me.GrpStatus.TabStop = False
        Me.GrpStatus.Text = "Values in use by the ASCOM Platform"
        '
        'LblNow
        '
        Me.LblNow.AutoSize = True
        Me.LblNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNow.Location = New System.Drawing.Point(96, 87)
        Me.LblNow.Name = "LblNow"
        Me.LblNow.Size = New System.Drawing.Size(63, 13)
        Me.LblNow.TabIndex = 30
        Me.LblNow.Text = "Current time"
        '
        'TxtNow
        '
        Me.TxtNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNow.Location = New System.Drawing.Point(165, 84)
        Me.TxtNow.Name = "TxtNow"
        Me.TxtNow.ReadOnly = True
        Me.TxtNow.Size = New System.Drawing.Size(209, 20)
        Me.TxtNow.TabIndex = 29
        '
        'TxtEffectiveDeltaUT1
        '
        Me.TxtEffectiveDeltaUT1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtEffectiveDeltaUT1.Location = New System.Drawing.Point(165, 58)
        Me.TxtEffectiveDeltaUT1.Name = "TxtEffectiveDeltaUT1"
        Me.TxtEffectiveDeltaUT1.ReadOnly = True
        Me.TxtEffectiveDeltaUT1.Size = New System.Drawing.Size(209, 20)
        Me.TxtEffectiveDeltaUT1.TabIndex = 28
        '
        'TxtEffectiveLeapSeconds
        '
        Me.TxtEffectiveLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtEffectiveLeapSeconds.Location = New System.Drawing.Point(165, 32)
        Me.TxtEffectiveLeapSeconds.Name = "TxtEffectiveLeapSeconds"
        Me.TxtEffectiveLeapSeconds.ReadOnly = True
        Me.TxtEffectiveLeapSeconds.Size = New System.Drawing.Size(209, 20)
        Me.TxtEffectiveLeapSeconds.TabIndex = 28
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(28, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(131, 13)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "TAI - UTC (Leap seconds)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(41, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "UT1 - UTC (Delta UT1)"
        '
        'GrpScheduleTime
        '
        Me.GrpScheduleTime.Controls.Add(Me.DateScheduleRun)
        Me.GrpScheduleTime.Controls.Add(Me.CmbScheduleRepeat)
        Me.GrpScheduleTime.Controls.Add(Me.LblAutoDownloadTime)
        Me.GrpScheduleTime.Controls.Add(Me.LblAutoRepeatFrequency)
        Me.GrpScheduleTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpScheduleTime.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpScheduleTime.Location = New System.Drawing.Point(14, 503)
        Me.GrpScheduleTime.Name = "GrpScheduleTime"
        Me.GrpScheduleTime.Size = New System.Drawing.Size(698, 95)
        Me.GrpScheduleTime.TabIndex = 27
        Me.GrpScheduleTime.TabStop = False
        Me.GrpScheduleTime.Text = "Automatic Update Schedule"
        '
        'BtnHelp
        '
        Me.BtnHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnHelp.Location = New System.Drawing.Point(859, 546)
        Me.BtnHelp.Name = "BtnHelp"
        Me.BtnHelp.Size = New System.Drawing.Size(75, 23)
        Me.BtnHelp.TabIndex = 28
        Me.BtnHelp.Text = "Help"
        Me.BtnHelp.UseVisualStyleBackColor = True
        '
        'EarthRotationDataForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 610)
        Me.Controls.Add(Me.BtnHelp)
        Me.Controls.Add(Me.GrpScheduleTime)
        Me.Controls.Add(Me.GrpStatus)
        Me.Controls.Add(Me.GrpUpdateType)
        Me.Controls.Add(Me.GrpOnDemandAndAutomaticUpdateConfiguration)
        Me.Controls.Add(Me.GrpManualUpdate)
        Me.Controls.Add(Me.BtnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "EarthRotationDataForm"
        Me.Text = "Earth Rotation Data Update Configuration"
        Me.GrpManualUpdate.ResumeLayout(False)
        Me.GrpManualUpdate.PerformLayout()
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.ResumeLayout(False)
        Me.GrpOnDemandAndAutomaticUpdateConfiguration.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpUpdateType.ResumeLayout(False)
        Me.GrpStatus.ResumeLayout(False)
        Me.GrpStatus.PerformLayout()
        Me.GrpScheduleTime.ResumeLayout(False)
        Me.GrpScheduleTime.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CmbDataSource As ComboBox
    Friend WithEvents GrpManualUpdate As GroupBox
    Friend WithEvents GrpOnDemandAndAutomaticUpdateConfiguration As GroupBox
    Friend WithEvents BtnClose As Button
    Friend WithEvents TxtDownloadTimeout As TextBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents TxtManualLeapSeconds As TextBox
    Friend WithEvents GrpUpdateType As GroupBox
    Friend WithEvents CmbUpdateType As ComboBox
    Friend WithEvents TxtManualDeltaUT1 As TextBox
    Friend WithEvents DateScheduleRun As DateTimePicker
    Friend WithEvents CmbScheduleRepeat As ComboBox
    Friend WithEvents LblAutoRepeatFrequency As Label
    Friend WithEvents LblAutoDownloadTime As Label
    Friend WithEvents LblAutoTimeout As Label
    Friend WithEvents LblAutoDataSource As Label
    Friend WithEvents LblManualDeltaUT1 As Label
    Friend WithEvents LblManualLeapSeconds As Label
    Friend WithEvents LblAutoSeconds As Label
    Friend WithEvents TxtLastRun As TextBox
    Friend WithEvents LblLastRun As Label
    Friend WithEvents LblTraceEnabled As Label
    Friend WithEvents ChkTraceEnabled As CheckBox
    Friend WithEvents TxtTraceFilePath As TextBox
    Friend WithEvents BtnSetTraceDirectory As Button
    Friend WithEvents FolderBrowser As FolderBrowserDialog
    Friend WithEvents LblNextLeapSecondsDate As Label
    Friend WithEvents TxtNextLeapSecondsDate As TextBox
    Friend WithEvents LblNextLeapSeconds As Label
    Friend WithEvents TxtNextLeapSeconds As TextBox
    Friend WithEvents GrpStatus As GroupBox
    Friend WithEvents BtnRunAutomaticUpdate As Button
    Friend WithEvents LblRunStatus As Label
    Friend WithEvents TxtRunStatus As TextBox
    Friend WithEvents TxtEffectiveDeltaUT1 As TextBox
    Friend WithEvents TxtEffectiveLeapSeconds As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblNow As Label
    Friend WithEvents TxtNow As TextBox
    Friend WithEvents GrpScheduleTime As GroupBox
    Friend WithEvents BtnHelp As Button
End Class
