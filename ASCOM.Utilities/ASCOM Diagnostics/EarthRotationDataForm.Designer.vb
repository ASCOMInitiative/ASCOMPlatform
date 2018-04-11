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
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.GrpManualUpdate = New System.Windows.Forms.GroupBox()
        Me.LblManualDeltaUT1 = New System.Windows.Forms.Label()
        Me.LblManualLeapSeconds = New System.Windows.Forms.Label()
        Me.TxtManualLeapSeconds = New System.Windows.Forms.TextBox()
        Me.TxtManualDeltaUT1 = New System.Windows.Forms.TextBox()
        Me.GrpAutomaticUpdate = New System.Windows.Forms.GroupBox()
        Me.LblRunStatus = New System.Windows.Forms.Label()
        Me.TxtRunStatus = New System.Windows.Forms.TextBox()
        Me.BtnRunAutomaticUpdate = New System.Windows.Forms.Button()
        Me.LblNextLeapSecondsDate = New System.Windows.Forms.Label()
        Me.LblNextLeapSeconds = New System.Windows.Forms.Label()
        Me.LblCurrentLeapSeconds = New System.Windows.Forms.Label()
        Me.TxtNextLeapSeconds = New System.Windows.Forms.TextBox()
        Me.TxtNextLeapSecondsDate = New System.Windows.Forms.TextBox()
        Me.LblTraceEnabled = New System.Windows.Forms.Label()
        Me.TxtCurrentLeapSeconds = New System.Windows.Forms.TextBox()
        Me.ChkTraceEnabled = New System.Windows.Forms.CheckBox()
        Me.TxtTraceFilePath = New System.Windows.Forms.TextBox()
        Me.BtnSetTraceDirectory = New System.Windows.Forms.Button()
        Me.LblAutoSeconds = New System.Windows.Forms.Label()
        Me.TxtLastRun = New System.Windows.Forms.TextBox()
        Me.LblLastRun = New System.Windows.Forms.Label()
        Me.LblAutoRepeatFrequency = New System.Windows.Forms.Label()
        Me.LblAutoDownloadTime = New System.Windows.Forms.Label()
        Me.CmbScheduleRepeat = New System.Windows.Forms.ComboBox()
        Me.LblAutoTimeout = New System.Windows.Forms.Label()
        Me.DateScheduleRun = New System.Windows.Forms.DateTimePicker()
        Me.LblAutoDataSource = New System.Windows.Forms.Label()
        Me.TxtDownloadTimeout = New System.Windows.Forms.TextBox()
        Me.BtnOK = New System.Windows.Forms.Button()
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
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.GrpManualUpdate.SuspendLayout()
        Me.GrpAutomaticUpdate.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpUpdateType.SuspendLayout()
        Me.GrpStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmbDataSource
        '
        Me.CmbDataSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbDataSource.FormattingEnabled = True
        Me.CmbDataSource.Location = New System.Drawing.Point(195, 36)
        Me.CmbDataSource.Name = "CmbDataSource"
        Me.CmbDataSource.Size = New System.Drawing.Size(250, 21)
        Me.CmbDataSource.TabIndex = 2
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnCancel.Location = New System.Drawing.Point(686, 500)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 1
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
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
        Me.GrpManualUpdate.Size = New System.Drawing.Size(323, 93)
        Me.GrpManualUpdate.TabIndex = 3
        Me.GrpManualUpdate.TabStop = False
        Me.GrpManualUpdate.Text = "Manual Entry"
        '
        'LblManualDeltaUT1
        '
        Me.LblManualDeltaUT1.AutoSize = True
        Me.LblManualDeltaUT1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblManualDeltaUT1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblManualDeltaUT1.Location = New System.Drawing.Point(133, 64)
        Me.LblManualDeltaUT1.Name = "LblManualDeltaUT1"
        Me.LblManualDeltaUT1.Size = New System.Drawing.Size(56, 13)
        Me.LblManualDeltaUT1.TabIndex = 15
        Me.LblManualDeltaUT1.Text = "Delta UT1"
        '
        'LblManualLeapSeconds
        '
        Me.LblManualLeapSeconds.AutoSize = True
        Me.LblManualLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblManualLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblManualLeapSeconds.Location = New System.Drawing.Point(113, 38)
        Me.LblManualLeapSeconds.Name = "LblManualLeapSeconds"
        Me.LblManualLeapSeconds.Size = New System.Drawing.Size(76, 13)
        Me.LblManualLeapSeconds.TabIndex = 16
        Me.LblManualLeapSeconds.Text = "Leap Seconds"
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
        'GrpAutomaticUpdate
        '
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblRunStatus)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtRunStatus)
        Me.GrpAutomaticUpdate.Controls.Add(Me.BtnRunAutomaticUpdate)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblNextLeapSecondsDate)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblNextLeapSeconds)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblCurrentLeapSeconds)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtNextLeapSeconds)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtNextLeapSecondsDate)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblTraceEnabled)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtCurrentLeapSeconds)
        Me.GrpAutomaticUpdate.Controls.Add(Me.ChkTraceEnabled)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtTraceFilePath)
        Me.GrpAutomaticUpdate.Controls.Add(Me.BtnSetTraceDirectory)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblAutoSeconds)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtLastRun)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblLastRun)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblAutoRepeatFrequency)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblAutoDownloadTime)
        Me.GrpAutomaticUpdate.Controls.Add(Me.CmbScheduleRepeat)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblAutoTimeout)
        Me.GrpAutomaticUpdate.Controls.Add(Me.DateScheduleRun)
        Me.GrpAutomaticUpdate.Controls.Add(Me.LblAutoDataSource)
        Me.GrpAutomaticUpdate.Controls.Add(Me.CmbDataSource)
        Me.GrpAutomaticUpdate.Controls.Add(Me.TxtDownloadTimeout)
        Me.GrpAutomaticUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpAutomaticUpdate.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpAutomaticUpdate.Location = New System.Drawing.Point(14, 238)
        Me.GrpAutomaticUpdate.Name = "GrpAutomaticUpdate"
        Me.GrpAutomaticUpdate.Size = New System.Drawing.Size(909, 256)
        Me.GrpAutomaticUpdate.TabIndex = 7
        Me.GrpAutomaticUpdate.TabStop = False
        Me.GrpAutomaticUpdate.Text = "Automatic Update"
        '
        'LblRunStatus
        '
        Me.LblRunStatus.AutoSize = True
        Me.LblRunStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRunStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblRunStatus.Location = New System.Drawing.Point(502, 161)
        Me.LblRunStatus.Name = "LblRunStatus"
        Me.LblRunStatus.Size = New System.Drawing.Size(73, 13)
        Me.LblRunStatus.TabIndex = 27
        Me.LblRunStatus.Text = "Update status"
        '
        'TxtRunStatus
        '
        Me.TxtRunStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRunStatus.Location = New System.Drawing.Point(581, 158)
        Me.TxtRunStatus.Name = "TxtRunStatus"
        Me.TxtRunStatus.ReadOnly = True
        Me.TxtRunStatus.Size = New System.Drawing.Size(301, 20)
        Me.TxtRunStatus.TabIndex = 26
        '
        'BtnRunAutomaticUpdate
        '
        Me.BtnRunAutomaticUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRunAutomaticUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnRunAutomaticUpdate.Location = New System.Drawing.Point(807, 182)
        Me.BtnRunAutomaticUpdate.Name = "BtnRunAutomaticUpdate"
        Me.BtnRunAutomaticUpdate.Size = New System.Drawing.Size(75, 23)
        Me.BtnRunAutomaticUpdate.TabIndex = 24
        Me.BtnRunAutomaticUpdate.Text = "Update Now"
        Me.BtnRunAutomaticUpdate.UseVisualStyleBackColor = True
        '
        'LblNextLeapSecondsDate
        '
        Me.LblNextLeapSecondsDate.AutoSize = True
        Me.LblNextLeapSecondsDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNextLeapSecondsDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNextLeapSecondsDate.Location = New System.Drawing.Point(25, 213)
        Me.LblNextLeapSecondsDate.Name = "LblNextLeapSecondsDate"
        Me.LblNextLeapSecondsDate.Size = New System.Drawing.Size(164, 13)
        Me.LblNextLeapSecondsDate.TabIndex = 21
        Me.LblNextLeapSecondsDate.Text = "Next leap seconds take effect on"
        '
        'LblNextLeapSeconds
        '
        Me.LblNextLeapSeconds.AutoSize = True
        Me.LblNextLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNextLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNextLeapSeconds.Location = New System.Drawing.Point(94, 187)
        Me.LblNextLeapSeconds.Name = "LblNextLeapSeconds"
        Me.LblNextLeapSeconds.Size = New System.Drawing.Size(95, 13)
        Me.LblNextLeapSeconds.TabIndex = 23
        Me.LblNextLeapSeconds.Text = "Next leap seconds"
        '
        'LblCurrentLeapSeconds
        '
        Me.LblCurrentLeapSeconds.AutoSize = True
        Me.LblCurrentLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCurrentLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblCurrentLeapSeconds.Location = New System.Drawing.Point(82, 161)
        Me.LblCurrentLeapSeconds.Name = "LblCurrentLeapSeconds"
        Me.LblCurrentLeapSeconds.Size = New System.Drawing.Size(107, 13)
        Me.LblCurrentLeapSeconds.TabIndex = 25
        Me.LblCurrentLeapSeconds.Text = "Current leap seconds"
        '
        'TxtNextLeapSeconds
        '
        Me.TxtNextLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNextLeapSeconds.Location = New System.Drawing.Point(195, 184)
        Me.TxtNextLeapSeconds.Name = "TxtNextLeapSeconds"
        Me.TxtNextLeapSeconds.ReadOnly = True
        Me.TxtNextLeapSeconds.Size = New System.Drawing.Size(100, 20)
        Me.TxtNextLeapSeconds.TabIndex = 22
        '
        'TxtNextLeapSecondsDate
        '
        Me.TxtNextLeapSecondsDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNextLeapSecondsDate.Location = New System.Drawing.Point(195, 210)
        Me.TxtNextLeapSecondsDate.Name = "TxtNextLeapSecondsDate"
        Me.TxtNextLeapSecondsDate.ReadOnly = True
        Me.TxtNextLeapSecondsDate.Size = New System.Drawing.Size(250, 20)
        Me.TxtNextLeapSecondsDate.TabIndex = 20
        '
        'LblTraceEnabled
        '
        Me.LblTraceEnabled.AutoSize = True
        Me.LblTraceEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraceEnabled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTraceEnabled.Location = New System.Drawing.Point(498, 92)
        Me.LblTraceEnabled.Name = "LblTraceEnabled"
        Me.LblTraceEnabled.Size = New System.Drawing.Size(77, 13)
        Me.LblTraceEnabled.TabIndex = 22
        Me.LblTraceEnabled.Text = "Trace Enabled"
        '
        'TxtCurrentLeapSeconds
        '
        Me.TxtCurrentLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCurrentLeapSeconds.Location = New System.Drawing.Point(195, 158)
        Me.TxtCurrentLeapSeconds.Name = "TxtCurrentLeapSeconds"
        Me.TxtCurrentLeapSeconds.ReadOnly = True
        Me.TxtCurrentLeapSeconds.Size = New System.Drawing.Size(45, 20)
        Me.TxtCurrentLeapSeconds.TabIndex = 24
        '
        'ChkTraceEnabled
        '
        Me.ChkTraceEnabled.AutoSize = True
        Me.ChkTraceEnabled.Location = New System.Drawing.Point(581, 92)
        Me.ChkTraceEnabled.Name = "ChkTraceEnabled"
        Me.ChkTraceEnabled.Size = New System.Drawing.Size(15, 14)
        Me.ChkTraceEnabled.TabIndex = 21
        Me.ChkTraceEnabled.UseVisualStyleBackColor = True
        '
        'TxtTraceFilePath
        '
        Me.TxtTraceFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTraceFilePath.Location = New System.Drawing.Point(195, 89)
        Me.TxtTraceFilePath.Name = "TxtTraceFilePath"
        Me.TxtTraceFilePath.ReadOnly = True
        Me.TxtTraceFilePath.Size = New System.Drawing.Size(250, 20)
        Me.TxtTraceFilePath.TabIndex = 19
        '
        'BtnSetTraceDirectory
        '
        Me.BtnSetTraceDirectory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetTraceDirectory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnSetTraceDirectory.Location = New System.Drawing.Point(67, 87)
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
        Me.LblAutoSeconds.Location = New System.Drawing.Point(641, 39)
        Me.LblAutoSeconds.Name = "LblAutoSeconds"
        Me.LblAutoSeconds.Size = New System.Drawing.Size(49, 13)
        Me.LblAutoSeconds.TabIndex = 17
        Me.LblAutoSeconds.Text = "Seconds"
        '
        'TxtLastRun
        '
        Me.TxtLastRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLastRun.Location = New System.Drawing.Point(581, 184)
        Me.TxtLastRun.Name = "TxtLastRun"
        Me.TxtLastRun.ReadOnly = True
        Me.TxtLastRun.Size = New System.Drawing.Size(220, 20)
        Me.TxtLastRun.TabIndex = 19
        '
        'LblLastRun
        '
        Me.LblLastRun.AutoSize = True
        Me.LblLastRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastRun.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblLastRun.Location = New System.Drawing.Point(530, 187)
        Me.LblLastRun.Name = "LblLastRun"
        Me.LblLastRun.Size = New System.Drawing.Size(45, 13)
        Me.LblLastRun.TabIndex = 18
        Me.LblLastRun.Text = "Last run"
        '
        'LblAutoRepeatFrequency
        '
        Me.LblAutoRepeatFrequency.AutoSize = True
        Me.LblAutoRepeatFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoRepeatFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoRepeatFrequency.Location = New System.Drawing.Point(480, 66)
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
        Me.LblAutoDownloadTime.Location = New System.Drawing.Point(34, 66)
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
        Me.CmbScheduleRepeat.Location = New System.Drawing.Point(581, 63)
        Me.CmbScheduleRepeat.Name = "CmbScheduleRepeat"
        Me.CmbScheduleRepeat.Size = New System.Drawing.Size(100, 21)
        Me.CmbScheduleRepeat.TabIndex = 12
        '
        'LblAutoTimeout
        '
        Me.LblAutoTimeout.AutoSize = True
        Me.LblAutoTimeout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoTimeout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoTimeout.Location = New System.Drawing.Point(483, 39)
        Me.LblAutoTimeout.Name = "LblAutoTimeout"
        Me.LblAutoTimeout.Size = New System.Drawing.Size(92, 13)
        Me.LblAutoTimeout.TabIndex = 13
        Me.LblAutoTimeout.Text = "Download timeout"
        '
        'DateScheduleRun
        '
        Me.DateScheduleRun.CustomFormat = "dddd dd MMM yyyy  -  HH:mm:ss"
        Me.DateScheduleRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateScheduleRun.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateScheduleRun.Location = New System.Drawing.Point(195, 63)
        Me.DateScheduleRun.Name = "DateScheduleRun"
        Me.DateScheduleRun.Size = New System.Drawing.Size(250, 20)
        Me.DateScheduleRun.TabIndex = 11
        '
        'LblAutoDataSource
        '
        Me.LblAutoDataSource.AutoSize = True
        Me.LblAutoDataSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAutoDataSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblAutoDataSource.Location = New System.Drawing.Point(51, 39)
        Me.LblAutoDataSource.Name = "LblAutoDataSource"
        Me.LblAutoDataSource.Size = New System.Drawing.Size(138, 13)
        Me.LblAutoDataSource.TabIndex = 14
        Me.LblAutoDataSource.Text = "Earth Rotation Data Source"
        '
        'TxtDownloadTimeout
        '
        Me.TxtDownloadTimeout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDownloadTimeout.Location = New System.Drawing.Point(581, 36)
        Me.TxtDownloadTimeout.Name = "TxtDownloadTimeout"
        Me.TxtDownloadTimeout.Size = New System.Drawing.Size(45, 20)
        Me.TxtDownloadTimeout.TabIndex = 4
        '
        'BtnOK
        '
        Me.BtnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnOK.Location = New System.Drawing.Point(848, 500)
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.Size = New System.Drawing.Size(75, 23)
        Me.BtnOK.TabIndex = 5
        Me.BtnOK.Text = "OK"
        Me.BtnOK.UseVisualStyleBackColor = True
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
        Me.GrpUpdateType.Text = "Update Type"
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
        Me.GrpStatus.Controls.Add(Me.TxtEffectiveLeapSeconds)
        Me.GrpStatus.Controls.Add(Me.Label6)
        Me.GrpStatus.Controls.Add(Me.Label2)
        Me.GrpStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpStatus.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GrpStatus.Location = New System.Drawing.Point(587, 113)
        Me.GrpStatus.Name = "GrpStatus"
        Me.GrpStatus.Size = New System.Drawing.Size(336, 119)
        Me.GrpStatus.TabIndex = 26
        Me.GrpStatus.TabStop = False
        Me.GrpStatus.Text = "Values in use by the ASCOM Platform"
        '
        'LblNow
        '
        Me.LblNow.AutoSize = True
        Me.LblNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblNow.Location = New System.Drawing.Point(30, 87)
        Me.LblNow.Name = "LblNow"
        Me.LblNow.Size = New System.Drawing.Size(63, 13)
        Me.LblNow.TabIndex = 30
        Me.LblNow.Text = "Current time"
        '
        'TxtNow
        '
        Me.TxtNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNow.Location = New System.Drawing.Point(99, 84)
        Me.TxtNow.Name = "TxtNow"
        Me.TxtNow.ReadOnly = True
        Me.TxtNow.Size = New System.Drawing.Size(209, 20)
        Me.TxtNow.TabIndex = 29
        '
        'TxtEffectiveDeltaUT1
        '
        Me.TxtEffectiveDeltaUT1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtEffectiveDeltaUT1.Location = New System.Drawing.Point(99, 58)
        Me.TxtEffectiveDeltaUT1.Name = "TxtEffectiveDeltaUT1"
        Me.TxtEffectiveDeltaUT1.ReadOnly = True
        Me.TxtEffectiveDeltaUT1.Size = New System.Drawing.Size(209, 20)
        Me.TxtEffectiveDeltaUT1.TabIndex = 28
        '
        'TxtEffectiveLeapSeconds
        '
        Me.TxtEffectiveLeapSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtEffectiveLeapSeconds.Location = New System.Drawing.Point(99, 32)
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
        Me.Label6.Location = New System.Drawing.Point(19, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 13)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "Leap seconds"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(37, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Delta UT1"
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnApply.Location = New System.Drawing.Point(767, 500)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(75, 23)
        Me.BtnApply.TabIndex = 27
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'EarthRotationDataForm
        '
        Me.AcceptButton = Me.BtnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(938, 533)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.GrpStatus)
        Me.Controls.Add(Me.GrpUpdateType)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.GrpAutomaticUpdate)
        Me.Controls.Add(Me.GrpManualUpdate)
        Me.Controls.Add(Me.BtnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "EarthRotationDataForm"
        Me.Text = "Earth Rotation Data Update Configuration"
        Me.GrpManualUpdate.ResumeLayout(False)
        Me.GrpManualUpdate.PerformLayout()
        Me.GrpAutomaticUpdate.ResumeLayout(False)
        Me.GrpAutomaticUpdate.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpUpdateType.ResumeLayout(False)
        Me.GrpStatus.ResumeLayout(False)
        Me.GrpStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CmbDataSource As ComboBox
    Friend WithEvents BtnCancel As Button
    Friend WithEvents GrpManualUpdate As GroupBox
    Friend WithEvents GrpAutomaticUpdate As GroupBox
    Friend WithEvents BtnOK As Button
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
    Friend WithEvents LblCurrentLeapSeconds As Label
    Friend WithEvents TxtCurrentLeapSeconds As TextBox
    Friend WithEvents GrpStatus As GroupBox
    Friend WithEvents BtnRunAutomaticUpdate As Button
    Friend WithEvents LblRunStatus As Label
    Friend WithEvents TxtRunStatus As TextBox
    Friend WithEvents BtnApply As Button
    Friend WithEvents TxtEffectiveDeltaUT1 As TextBox
    Friend WithEvents TxtEffectiveLeapSeconds As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblNow As Label
    Friend WithEvents TxtNow As TextBox
End Class
