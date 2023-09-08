using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EarthRotationDataForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(EarthRotationDataForm));
            CmbDataSource = new ComboBox();
            CmbDataSource.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
            CmbDataSource.KeyUp += new KeyEventHandler(CmbDataSource_Validating);
            GrpManualUpdate = new GroupBox();
            GrpManualUpdate.Paint += new PaintEventHandler(GroupBox_Paint);
            LblManualDeltaUT1 = new Label();
            LblManualLeapSeconds = new Label();
            TxtManualLeapSeconds = new TextBox();
            TxtManualLeapSeconds.KeyUp += new KeyEventHandler(TxtManualLeapSeconds_Validating);
            TxtManualDeltaUT1 = new TextBox();
            TxtManualDeltaUT1.KeyUp += new KeyEventHandler(TxtDeltaUT1Manuals_Validating);
            GrpOnDemandAndAutomaticUpdateConfiguration = new GroupBox();
            GrpOnDemandAndAutomaticUpdateConfiguration.Paint += new PaintEventHandler(GroupBox_Paint);
            LblRunStatus = new Label();
            TxtRunStatus = new TextBox();
            BtnRunAutomaticUpdate = new Button();
            BtnRunAutomaticUpdate.Click += new EventHandler(BtnRunAutomaticUpdate_Click);
            LblTraceEnabled = new Label();
            ChkTraceEnabled = new CheckBox();
            ChkTraceEnabled.CheckedChanged += new EventHandler(ChkTraceEnabled_CheckedChanged);
            TxtTraceFilePath = new TextBox();
            BtnSetTraceDirectory = new Button();
            BtnSetTraceDirectory.Click += new EventHandler(BtnSetTraceDirectory_Click);
            LblAutoSeconds = new Label();
            TxtLastRun = new TextBox();
            LblLastRun = new Label();
            LblAutoTimeout = new Label();
            LblAutoDataSource = new Label();
            TxtDownloadTimeout = new TextBox();
            TxtDownloadTimeout.KeyUp += new KeyEventHandler(TxtDownloadTimeout_Validating);
            LblNextLeapSecondsDate = new Label();
            LblNextLeapSeconds = new Label();
            TxtNextLeapSeconds = new TextBox();
            TxtNextLeapSecondsDate = new TextBox();
            LblAutoRepeatFrequency = new Label();
            LblAutoDownloadTime = new Label();
            CmbScheduleRepeat = new ComboBox();
            CmbScheduleRepeat.SelectedIndexChanged += new EventHandler(CmbSchedulRepeat_Changed);
            CmbScheduleRepeat.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            DateScheduleRun = new DateTimePicker();
            DateScheduleRun.ValueChanged += new EventHandler(DateScheduleRun_ValueChanged);
            BtnClose = new Button();
            BtnClose.Click += new EventHandler(BtnClose_Click);
            ErrorProvider1 = new ErrorProvider(components);
            CmbUpdateType = new ComboBox();
            CmbUpdateType.SelectedIndexChanged += new EventHandler(CmbUpdateType_Changed);
            CmbUpdateType.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            GrpUpdateType = new GroupBox();
            GrpUpdateType.Paint += new PaintEventHandler(GroupBox_Paint);
            FolderBrowser = new FolderBrowserDialog();
            GrpStatus = new GroupBox();
            GrpStatus.Paint += new PaintEventHandler(GroupBox_Paint);
            LblNow = new Label();
            TxtNow = new TextBox();
            TxtEffectiveDeltaUT1 = new TextBox();
            TxtEffectiveLeapSeconds = new TextBox();
            Label6 = new Label();
            Label2 = new Label();
            GrpScheduleTime = new GroupBox();
            GrpScheduleTime.Paint += new PaintEventHandler(GroupBox_Paint);
            BtnHelp = new Button();
            BtnHelp.Click += new EventHandler(BtnHelp_Click);
            GrpManualUpdate.SuspendLayout();
            GrpOnDemandAndAutomaticUpdateConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ErrorProvider1).BeginInit();
            GrpUpdateType.SuspendLayout();
            GrpStatus.SuspendLayout();
            GrpScheduleTime.SuspendLayout();
            SuspendLayout();
            // 
            // CmbDataSource
            // 
            CmbDataSource.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            CmbDataSource.FormattingEnabled = true;
            CmbDataSource.Location = new Point(202, 36);
            CmbDataSource.Name = "CmbDataSource";
            CmbDataSource.Size = new Size(250, 21);
            CmbDataSource.TabIndex = 2;
            // 
            // GrpManualUpdate
            // 
            GrpManualUpdate.Controls.Add(LblManualDeltaUT1);
            GrpManualUpdate.Controls.Add(LblManualLeapSeconds);
            GrpManualUpdate.Controls.Add(TxtManualLeapSeconds);
            GrpManualUpdate.Controls.Add(TxtManualDeltaUT1);
            GrpManualUpdate.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            GrpManualUpdate.ForeColor = SystemColors.Highlight;
            GrpManualUpdate.Location = new Point(14, 113);
            GrpManualUpdate.Name = "GrpManualUpdate";
            GrpManualUpdate.Size = new Size(434, 93);
            GrpManualUpdate.TabIndex = 3;
            GrpManualUpdate.TabStop = false;
            GrpManualUpdate.Text = "Specified values";
            // 
            // LblManualDeltaUT1
            // 
            LblManualDeltaUT1.AutoSize = true;
            LblManualDeltaUT1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblManualDeltaUT1.ForeColor = SystemColors.ControlText;
            LblManualDeltaUT1.Location = new Point(71, 64);
            LblManualDeltaUT1.Name = "LblManualDeltaUT1";
            LblManualDeltaUT1.Size = new Size(117, 13);
            LblManualDeltaUT1.TabIndex = 15;
            LblManualDeltaUT1.Text = "UT1 - UTC (Delta UT1)";
            // 
            // LblManualLeapSeconds
            // 
            LblManualLeapSeconds.AutoSize = true;
            LblManualLeapSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblManualLeapSeconds.ForeColor = SystemColors.ControlText;
            LblManualLeapSeconds.Location = new Point(55, 38);
            LblManualLeapSeconds.Name = "LblManualLeapSeconds";
            LblManualLeapSeconds.Size = new Size(133, 13);
            LblManualLeapSeconds.TabIndex = 16;
            LblManualLeapSeconds.Text = "TAI - UTC (Leap Seconds)";
            // 
            // TxtManualLeapSeconds
            // 
            TxtManualLeapSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtManualLeapSeconds.Location = new Point(195, 35);
            TxtManualLeapSeconds.Name = "TxtManualLeapSeconds";
            TxtManualLeapSeconds.Size = new Size(45, 20);
            TxtManualLeapSeconds.TabIndex = 8;
            // 
            // TxtManualDeltaUT1
            // 
            TxtManualDeltaUT1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtManualDeltaUT1.ForeColor = SystemColors.ControlText;
            TxtManualDeltaUT1.Location = new Point(195, 61);
            TxtManualDeltaUT1.Name = "TxtManualDeltaUT1";
            TxtManualDeltaUT1.Size = new Size(100, 20);
            TxtManualDeltaUT1.TabIndex = 10;
            // 
            // GrpOnDemandAndAutomaticUpdateConfiguration
            // 
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblRunStatus);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(TxtRunStatus);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(BtnRunAutomaticUpdate);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblTraceEnabled);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(ChkTraceEnabled);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(TxtTraceFilePath);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(BtnSetTraceDirectory);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblAutoSeconds);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(TxtLastRun);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblLastRun);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblAutoTimeout);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(LblAutoDataSource);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(CmbDataSource);
            GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(TxtDownloadTimeout);
            GrpOnDemandAndAutomaticUpdateConfiguration.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            GrpOnDemandAndAutomaticUpdateConfiguration.ForeColor = SystemColors.Highlight;
            GrpOnDemandAndAutomaticUpdateConfiguration.Location = new Point(14, 238);
            GrpOnDemandAndAutomaticUpdateConfiguration.Name = "GrpOnDemandAndAutomaticUpdateConfiguration";
            GrpOnDemandAndAutomaticUpdateConfiguration.Size = new Size(698, 228);
            GrpOnDemandAndAutomaticUpdateConfiguration.TabIndex = 7;
            GrpOnDemandAndAutomaticUpdateConfiguration.TabStop = false;
            GrpOnDemandAndAutomaticUpdateConfiguration.Text = "On Demand and Automatic Update Configuration";
            // 
            // LblRunStatus
            // 
            LblRunStatus.AutoSize = true;
            LblRunStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblRunStatus.ForeColor = SystemColors.ControlText;
            LblRunStatus.Location = new Point(123, 198);
            LblRunStatus.Name = "LblRunStatus";
            LblRunStatus.Size = new Size(73, 13);
            LblRunStatus.TabIndex = 27;
            LblRunStatus.Text = "Update status";
            // 
            // TxtRunStatus
            // 
            TxtRunStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtRunStatus.Location = new Point(202, 195);
            TxtRunStatus.Name = "TxtRunStatus";
            TxtRunStatus.ReadOnly = true;
            TxtRunStatus.Size = new Size(321, 20);
            TxtRunStatus.TabIndex = 26;
            // 
            // BtnRunAutomaticUpdate
            // 
            BtnRunAutomaticUpdate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnRunAutomaticUpdate.ForeColor = SystemColors.ControlText;
            BtnRunAutomaticUpdate.Location = new Point(550, 167);
            BtnRunAutomaticUpdate.Name = "BtnRunAutomaticUpdate";
            BtnRunAutomaticUpdate.Size = new Size(119, 23);
            BtnRunAutomaticUpdate.TabIndex = 24;
            BtnRunAutomaticUpdate.Text = "On Demand Update";
            BtnRunAutomaticUpdate.UseVisualStyleBackColor = true;
            // 
            // LblTraceEnabled
            // 
            LblTraceEnabled.AutoSize = true;
            LblTraceEnabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblTraceEnabled.ForeColor = SystemColors.ControlText;
            LblTraceEnabled.Location = new Point(119, 91);
            LblTraceEnabled.Name = "LblTraceEnabled";
            LblTraceEnabled.Size = new Size(77, 13);
            LblTraceEnabled.TabIndex = 22;
            LblTraceEnabled.Text = "Trace Enabled";
            // 
            // ChkTraceEnabled
            // 
            ChkTraceEnabled.AutoSize = true;
            ChkTraceEnabled.Location = new Point(202, 91);
            ChkTraceEnabled.Name = "ChkTraceEnabled";
            ChkTraceEnabled.Size = new Size(15, 14);
            ChkTraceEnabled.TabIndex = 21;
            ChkTraceEnabled.UseVisualStyleBackColor = true;
            // 
            // TxtTraceFilePath
            // 
            TxtTraceFilePath.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtTraceFilePath.Location = new Point(202, 111);
            TxtTraceFilePath.Name = "TxtTraceFilePath";
            TxtTraceFilePath.ReadOnly = true;
            TxtTraceFilePath.Size = new Size(321, 20);
            TxtTraceFilePath.TabIndex = 19;
            // 
            // BtnSetTraceDirectory
            // 
            BtnSetTraceDirectory.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnSetTraceDirectory.ForeColor = SystemColors.ControlText;
            BtnSetTraceDirectory.Location = new Point(74, 109);
            BtnSetTraceDirectory.Name = "BtnSetTraceDirectory";
            BtnSetTraceDirectory.Size = new Size(122, 23);
            BtnSetTraceDirectory.TabIndex = 18;
            BtnSetTraceDirectory.Text = "Select Trace Directory";
            BtnSetTraceDirectory.UseVisualStyleBackColor = true;
            // 
            // LblAutoSeconds
            // 
            LblAutoSeconds.AutoSize = true;
            LblAutoSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblAutoSeconds.ForeColor = SystemColors.ControlText;
            LblAutoSeconds.Location = new Point(262, 66);
            LblAutoSeconds.Name = "LblAutoSeconds";
            LblAutoSeconds.Size = new Size(49, 13);
            LblAutoSeconds.TabIndex = 17;
            LblAutoSeconds.Text = "Seconds";
            // 
            // TxtLastRun
            // 
            TxtLastRun.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtLastRun.Location = new Point(202, 169);
            TxtLastRun.Name = "TxtLastRun";
            TxtLastRun.ReadOnly = true;
            TxtLastRun.Size = new Size(321, 20);
            TxtLastRun.TabIndex = 19;
            // 
            // LblLastRun
            // 
            LblLastRun.AutoSize = true;
            LblLastRun.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblLastRun.ForeColor = SystemColors.ControlText;
            LblLastRun.Location = new Point(133, 172);
            LblLastRun.Name = "LblLastRun";
            LblLastRun.Size = new Size(63, 13);
            LblLastRun.TabIndex = 18;
            LblLastRun.Text = "Last update";
            // 
            // LblAutoTimeout
            // 
            LblAutoTimeout.AutoSize = true;
            LblAutoTimeout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblAutoTimeout.ForeColor = SystemColors.ControlText;
            LblAutoTimeout.Location = new Point(104, 66);
            LblAutoTimeout.Name = "LblAutoTimeout";
            LblAutoTimeout.Size = new Size(92, 13);
            LblAutoTimeout.TabIndex = 13;
            LblAutoTimeout.Text = "Download timeout";
            // 
            // LblAutoDataSource
            // 
            LblAutoDataSource.AutoSize = true;
            LblAutoDataSource.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblAutoDataSource.ForeColor = SystemColors.ControlText;
            LblAutoDataSource.Location = new Point(90, 39);
            LblAutoDataSource.Name = "LblAutoDataSource";
            LblAutoDataSource.Size = new Size(106, 13);
            LblAutoDataSource.TabIndex = 14;
            LblAutoDataSource.Text = "Internet Data Source";
            // 
            // TxtDownloadTimeout
            // 
            TxtDownloadTimeout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtDownloadTimeout.Location = new Point(202, 63);
            TxtDownloadTimeout.Name = "TxtDownloadTimeout";
            TxtDownloadTimeout.Size = new Size(45, 20);
            TxtDownloadTimeout.TabIndex = 4;
            // 
            // LblNextLeapSecondsDate
            // 
            LblNextLeapSecondsDate.AutoSize = true;
            LblNextLeapSecondsDate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblNextLeapSecondsDate.ForeColor = SystemColors.ControlText;
            LblNextLeapSecondsDate.Location = new Point(28, 139);
            LblNextLeapSecondsDate.Name = "LblNextLeapSecondsDate";
            LblNextLeapSecondsDate.Size = new Size(130, 13);
            LblNextLeapSecondsDate.TabIndex = 21;
            LblNextLeapSecondsDate.Text = "Start of next leap seconds";
            // 
            // LblNextLeapSeconds
            // 
            LblNextLeapSeconds.AutoSize = true;
            LblNextLeapSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblNextLeapSeconds.ForeColor = SystemColors.ControlText;
            LblNextLeapSeconds.Location = new Point(63, 113);
            LblNextLeapSeconds.Name = "LblNextLeapSeconds";
            LblNextLeapSeconds.Size = new Size(95, 13);
            LblNextLeapSeconds.TabIndex = 23;
            LblNextLeapSeconds.Text = "Next leap seconds";
            // 
            // TxtNextLeapSeconds
            // 
            TxtNextLeapSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtNextLeapSeconds.Location = new Point(164, 110);
            TxtNextLeapSeconds.Name = "TxtNextLeapSeconds";
            TxtNextLeapSeconds.ReadOnly = true;
            TxtNextLeapSeconds.Size = new Size(100, 20);
            TxtNextLeapSeconds.TabIndex = 22;
            // 
            // TxtNextLeapSecondsDate
            // 
            TxtNextLeapSecondsDate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtNextLeapSecondsDate.Location = new Point(164, 136);
            TxtNextLeapSecondsDate.Name = "TxtNextLeapSecondsDate";
            TxtNextLeapSecondsDate.ReadOnly = true;
            TxtNextLeapSecondsDate.Size = new Size(250, 20);
            TxtNextLeapSecondsDate.TabIndex = 20;
            // 
            // LblAutoRepeatFrequency
            // 
            LblAutoRepeatFrequency.AutoSize = true;
            LblAutoRepeatFrequency.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblAutoRepeatFrequency.ForeColor = SystemColors.ControlText;
            LblAutoRepeatFrequency.Location = new Point(94, 65);
            LblAutoRepeatFrequency.Name = "LblAutoRepeatFrequency";
            LblAutoRepeatFrequency.Size = new Size(95, 13);
            LblAutoRepeatFrequency.TabIndex = 16;
            LblAutoRepeatFrequency.Text = "Repeat Frequency";
            // 
            // LblAutoDownloadTime
            // 
            LblAutoDownloadTime.AutoSize = true;
            LblAutoDownloadTime.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblAutoDownloadTime.ForeColor = SystemColors.ControlText;
            LblAutoDownloadTime.Location = new Point(34, 38);
            LblAutoDownloadTime.Name = "LblAutoDownloadTime";
            LblAutoDownloadTime.Size = new Size(155, 13);
            LblAutoDownloadTime.TabIndex = 15;
            LblAutoDownloadTime.Text = "Initial Download Date and Time";
            // 
            // CmbScheduleRepeat
            // 
            CmbScheduleRepeat.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbScheduleRepeat.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            CmbScheduleRepeat.FormattingEnabled = true;
            CmbScheduleRepeat.Location = new Point(195, 62);
            CmbScheduleRepeat.Name = "CmbScheduleRepeat";
            CmbScheduleRepeat.Size = new Size(111, 21);
            CmbScheduleRepeat.TabIndex = 12;
            // 
            // DateScheduleRun
            // 
            DateScheduleRun.CustomFormat = "dddd dd MMM yyyy  -  HH:mm:ss";
            DateScheduleRun.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DateScheduleRun.Format = DateTimePickerFormat.Custom;
            DateScheduleRun.Location = new Point(195, 35);
            DateScheduleRun.Name = "DateScheduleRun";
            DateScheduleRun.Size = new Size(250, 20);
            DateScheduleRun.TabIndex = 11;
            // 
            // BtnClose
            // 
            BtnClose.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnClose.ForeColor = SystemColors.ControlText;
            BtnClose.Location = new Point(859, 575);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(75, 23);
            BtnClose.TabIndex = 5;
            BtnClose.Text = "Close";
            BtnClose.UseVisualStyleBackColor = true;
            // 
            // ErrorProvider1
            // 
            ErrorProvider1.ContainerControl = this;
            // 
            // CmbUpdateType
            // 
            CmbUpdateType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbUpdateType.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            CmbUpdateType.ForeColor = SystemColors.ControlText;
            CmbUpdateType.FormattingEnabled = true;
            CmbUpdateType.Location = new Point(17, 31);
            CmbUpdateType.Name = "CmbUpdateType";
            CmbUpdateType.Size = new Size(278, 21);
            CmbUpdateType.TabIndex = 8;
            // 
            // GrpUpdateType
            // 
            GrpUpdateType.Controls.Add(CmbUpdateType);
            GrpUpdateType.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            GrpUpdateType.ForeColor = SystemColors.Highlight;
            GrpUpdateType.Location = new Point(14, 12);
            GrpUpdateType.Name = "GrpUpdateType";
            GrpUpdateType.Size = new Size(323, 72);
            GrpUpdateType.TabIndex = 9;
            GrpUpdateType.TabStop = false;
            GrpUpdateType.Text = "ASCOM Platform Data Source";
            // 
            // FolderBrowser
            // 
            FolderBrowser.Description = @"Select a trace file directory (normally Documents\ASCOM)";
            FolderBrowser.RootFolder = Environment.SpecialFolder.MyDocuments;
            // 
            // GrpStatus
            // 
            GrpStatus.Controls.Add(LblNow);
            GrpStatus.Controls.Add(TxtNow);
            GrpStatus.Controls.Add(TxtEffectiveDeltaUT1);
            GrpStatus.Controls.Add(LblNextLeapSecondsDate);
            GrpStatus.Controls.Add(TxtEffectiveLeapSeconds);
            GrpStatus.Controls.Add(LblNextLeapSeconds);
            GrpStatus.Controls.Add(Label6);
            GrpStatus.Controls.Add(TxtNextLeapSecondsDate);
            GrpStatus.Controls.Add(TxtNextLeapSeconds);
            GrpStatus.Controls.Add(Label2);
            GrpStatus.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            GrpStatus.ForeColor = SystemColors.Highlight;
            GrpStatus.Location = new Point(495, 34);
            GrpStatus.Name = "GrpStatus";
            GrpStatus.Size = new Size(439, 172);
            GrpStatus.TabIndex = 26;
            GrpStatus.TabStop = false;
            GrpStatus.Text = "Values in use by the ASCOM Platform";
            // 
            // LblNow
            // 
            LblNow.AutoSize = true;
            LblNow.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblNow.ForeColor = SystemColors.ControlText;
            LblNow.Location = new Point(96, 87);
            LblNow.Name = "LblNow";
            LblNow.Size = new Size(63, 13);
            LblNow.TabIndex = 30;
            LblNow.Text = "Current time";
            // 
            // TxtNow
            // 
            TxtNow.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtNow.Location = new Point(165, 84);
            TxtNow.Name = "TxtNow";
            TxtNow.ReadOnly = true;
            TxtNow.Size = new Size(209, 20);
            TxtNow.TabIndex = 29;
            // 
            // TxtEffectiveDeltaUT1
            // 
            TxtEffectiveDeltaUT1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtEffectiveDeltaUT1.Location = new Point(165, 58);
            TxtEffectiveDeltaUT1.Name = "TxtEffectiveDeltaUT1";
            TxtEffectiveDeltaUT1.ReadOnly = true;
            TxtEffectiveDeltaUT1.Size = new Size(209, 20);
            TxtEffectiveDeltaUT1.TabIndex = 28;
            // 
            // TxtEffectiveLeapSeconds
            // 
            TxtEffectiveLeapSeconds.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtEffectiveLeapSeconds.Location = new Point(165, 32);
            TxtEffectiveLeapSeconds.Name = "TxtEffectiveLeapSeconds";
            TxtEffectiveLeapSeconds.ReadOnly = true;
            TxtEffectiveLeapSeconds.Size = new Size(209, 20);
            TxtEffectiveLeapSeconds.TabIndex = 28;
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label6.ForeColor = SystemColors.ControlText;
            Label6.Location = new Point(28, 35);
            Label6.Name = "Label6";
            Label6.Size = new Size(131, 13);
            Label6.TabIndex = 27;
            Label6.Text = "TAI - UTC (Leap seconds)";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label2.ForeColor = SystemColors.ControlText;
            Label2.Location = new Point(41, 61);
            Label2.Name = "Label2";
            Label2.Size = new Size(117, 13);
            Label2.TabIndex = 26;
            Label2.Text = "UT1 - UTC (Delta UT1)";
            // 
            // GrpScheduleTime
            // 
            GrpScheduleTime.Controls.Add(DateScheduleRun);
            GrpScheduleTime.Controls.Add(CmbScheduleRepeat);
            GrpScheduleTime.Controls.Add(LblAutoDownloadTime);
            GrpScheduleTime.Controls.Add(LblAutoRepeatFrequency);
            GrpScheduleTime.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            GrpScheduleTime.ForeColor = SystemColors.Highlight;
            GrpScheduleTime.Location = new Point(14, 503);
            GrpScheduleTime.Name = "GrpScheduleTime";
            GrpScheduleTime.Size = new Size(698, 95);
            GrpScheduleTime.TabIndex = 27;
            GrpScheduleTime.TabStop = false;
            GrpScheduleTime.Text = "Automatic Update Schedule";
            // 
            // BtnHelp
            // 
            BtnHelp.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnHelp.ForeColor = SystemColors.ControlText;
            BtnHelp.Location = new Point(859, 546);
            BtnHelp.Name = "BtnHelp";
            BtnHelp.Size = new Size(75, 23);
            BtnHelp.TabIndex = 28;
            BtnHelp.Text = "Help";
            BtnHelp.UseVisualStyleBackColor = true;
            // 
            // EarthRotationDataForm
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(949, 610);
            Controls.Add(BtnHelp);
            Controls.Add(GrpScheduleTime);
            Controls.Add(GrpStatus);
            Controls.Add(GrpUpdateType);
            Controls.Add(GrpOnDemandAndAutomaticUpdateConfiguration);
            Controls.Add(GrpManualUpdate);
            Controls.Add(BtnClose);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "EarthRotationDataForm";
            Text = "Earth Rotation Data Update Configuration";
            GrpManualUpdate.ResumeLayout(false);
            GrpManualUpdate.PerformLayout();
            GrpOnDemandAndAutomaticUpdateConfiguration.ResumeLayout(false);
            GrpOnDemandAndAutomaticUpdateConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ErrorProvider1).EndInit();
            GrpUpdateType.ResumeLayout(false);
            GrpStatus.ResumeLayout(false);
            GrpStatus.PerformLayout();
            GrpScheduleTime.ResumeLayout(false);
            GrpScheduleTime.PerformLayout();
            Load += new EventHandler(EarthRotationDataForm_Load);
            ResumeLayout(false);

        }

        internal ComboBox CmbDataSource;
        internal GroupBox GrpManualUpdate;
        internal GroupBox GrpOnDemandAndAutomaticUpdateConfiguration;
        internal Button BtnClose;
        internal TextBox TxtDownloadTimeout;
        internal ErrorProvider ErrorProvider1;
        internal TextBox TxtManualLeapSeconds;
        internal GroupBox GrpUpdateType;
        internal ComboBox CmbUpdateType;
        internal TextBox TxtManualDeltaUT1;
        internal DateTimePicker DateScheduleRun;
        internal ComboBox CmbScheduleRepeat;
        internal Label LblAutoRepeatFrequency;
        internal Label LblAutoDownloadTime;
        internal Label LblAutoTimeout;
        internal Label LblAutoDataSource;
        internal Label LblManualDeltaUT1;
        internal Label LblManualLeapSeconds;
        internal Label LblAutoSeconds;
        internal TextBox TxtLastRun;
        internal Label LblLastRun;
        internal Label LblTraceEnabled;
        internal CheckBox ChkTraceEnabled;
        internal TextBox TxtTraceFilePath;
        internal Button BtnSetTraceDirectory;
        internal FolderBrowserDialog FolderBrowser;
        internal Label LblNextLeapSecondsDate;
        internal TextBox TxtNextLeapSecondsDate;
        internal Label LblNextLeapSeconds;
        internal TextBox TxtNextLeapSeconds;
        internal GroupBox GrpStatus;
        internal Button BtnRunAutomaticUpdate;
        internal Label LblRunStatus;
        internal TextBox TxtRunStatus;
        internal TextBox TxtEffectiveDeltaUT1;
        internal TextBox TxtEffectiveLeapSeconds;
        internal Label Label6;
        internal Label Label2;
        internal Label LblNow;
        internal TextBox TxtNow;
        internal GroupBox GrpScheduleTime;
        internal Button BtnHelp;
    }
}