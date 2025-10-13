using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// EarthRotationDataForm class
    /// </summary>
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EarthRotationDataForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Dispose(bool disposing)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EarthRotationDataForm));
            this.CmbDataSource = new System.Windows.Forms.ComboBox();
            this.GrpManualUpdate = new System.Windows.Forms.GroupBox();
            this.LblManualDeltaUT1 = new System.Windows.Forms.Label();
            this.LblManualLeapSeconds = new System.Windows.Forms.Label();
            this.TxtManualLeapSeconds = new System.Windows.Forms.TextBox();
            this.TxtManualDeltaUT1 = new System.Windows.Forms.TextBox();
            this.GrpOnDemandAndAutomaticUpdateConfiguration = new System.Windows.Forms.GroupBox();
            this.LblRunStatus = new System.Windows.Forms.Label();
            this.TxtRunStatus = new System.Windows.Forms.TextBox();
            this.BtnRunAutomaticUpdate = new System.Windows.Forms.Button();
            this.LblTraceEnabled = new System.Windows.Forms.Label();
            this.ChkTraceEnabled = new System.Windows.Forms.CheckBox();
            this.TxtTraceFilePath = new System.Windows.Forms.TextBox();
            this.BtnSetTraceDirectory = new System.Windows.Forms.Button();
            this.LblAutoSeconds = new System.Windows.Forms.Label();
            this.TxtLastRun = new System.Windows.Forms.TextBox();
            this.LblLastRun = new System.Windows.Forms.Label();
            this.LblAutoTimeout = new System.Windows.Forms.Label();
            this.LblAutoDataSource = new System.Windows.Forms.Label();
            this.TxtDownloadTimeout = new System.Windows.Forms.TextBox();
            this.LblNextLeapSecondsDate = new System.Windows.Forms.Label();
            this.LblNextLeapSeconds = new System.Windows.Forms.Label();
            this.TxtNextLeapSeconds = new System.Windows.Forms.TextBox();
            this.TxtNextLeapSecondsDate = new System.Windows.Forms.TextBox();
            this.LblAutoRepeatFrequency = new System.Windows.Forms.Label();
            this.LblAutoDownloadTime = new System.Windows.Forms.Label();
            this.CmbScheduleRepeat = new System.Windows.Forms.ComboBox();
            this.DateScheduleRun = new System.Windows.Forms.DateTimePicker();
            this.BtnClose = new System.Windows.Forms.Button();
            this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.CmbUpdateType = new System.Windows.Forms.ComboBox();
            this.GrpUpdateType = new System.Windows.Forms.GroupBox();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.GrpStatus = new System.Windows.Forms.GroupBox();
            this.LblNow = new System.Windows.Forms.Label();
            this.TxtNow = new System.Windows.Forms.TextBox();
            this.TxtEffectiveDeltaUT1 = new System.Windows.Forms.TextBox();
            this.TxtEffectiveLeapSeconds = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.GrpScheduleTime = new System.Windows.Forms.GroupBox();
            this.BtnHelp = new System.Windows.Forms.Button();
            this.GrpManualUpdate.SuspendLayout();
            this.GrpOnDemandAndAutomaticUpdateConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            this.GrpUpdateType.SuspendLayout();
            this.GrpStatus.SuspendLayout();
            this.GrpScheduleTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbDataSource
            // 
            this.CmbDataSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbDataSource.FormattingEnabled = true;
            this.CmbDataSource.Location = new System.Drawing.Point(202, 36);
            this.CmbDataSource.Name = "CmbDataSource";
            this.CmbDataSource.Size = new System.Drawing.Size(321, 21);
            this.CmbDataSource.TabIndex = 2;
            this.CmbDataSource.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            this.CmbDataSource.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CmbDataSource_Validating);
            // 
            // GrpManualUpdate
            // 
            this.GrpManualUpdate.Controls.Add(this.LblManualDeltaUT1);
            this.GrpManualUpdate.Controls.Add(this.LblManualLeapSeconds);
            this.GrpManualUpdate.Controls.Add(this.TxtManualLeapSeconds);
            this.GrpManualUpdate.Controls.Add(this.TxtManualDeltaUT1);
            this.GrpManualUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpManualUpdate.ForeColor = System.Drawing.SystemColors.Highlight;
            this.GrpManualUpdate.Location = new System.Drawing.Point(14, 113);
            this.GrpManualUpdate.Name = "GrpManualUpdate";
            this.GrpManualUpdate.Size = new System.Drawing.Size(434, 93);
            this.GrpManualUpdate.TabIndex = 3;
            this.GrpManualUpdate.TabStop = false;
            this.GrpManualUpdate.Text = "Specified values";
            this.GrpManualUpdate.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
            // 
            // LblManualDeltaUT1
            // 
            this.LblManualDeltaUT1.AutoSize = true;
            this.LblManualDeltaUT1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblManualDeltaUT1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblManualDeltaUT1.Location = new System.Drawing.Point(71, 64);
            this.LblManualDeltaUT1.Name = "LblManualDeltaUT1";
            this.LblManualDeltaUT1.Size = new System.Drawing.Size(117, 13);
            this.LblManualDeltaUT1.TabIndex = 15;
            this.LblManualDeltaUT1.Text = "UT1 - UTC (Delta UT1)";
            // 
            // LblManualLeapSeconds
            // 
            this.LblManualLeapSeconds.AutoSize = true;
            this.LblManualLeapSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblManualLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblManualLeapSeconds.Location = new System.Drawing.Point(55, 38);
            this.LblManualLeapSeconds.Name = "LblManualLeapSeconds";
            this.LblManualLeapSeconds.Size = new System.Drawing.Size(133, 13);
            this.LblManualLeapSeconds.TabIndex = 16;
            this.LblManualLeapSeconds.Text = "TAI - UTC (Leap Seconds)";
            // 
            // TxtManualLeapSeconds
            // 
            this.TxtManualLeapSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtManualLeapSeconds.Location = new System.Drawing.Point(195, 35);
            this.TxtManualLeapSeconds.Name = "TxtManualLeapSeconds";
            this.TxtManualLeapSeconds.Size = new System.Drawing.Size(45, 20);
            this.TxtManualLeapSeconds.TabIndex = 8;
            this.TxtManualLeapSeconds.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtManualLeapSeconds_Validating);
            // 
            // TxtManualDeltaUT1
            // 
            this.TxtManualDeltaUT1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtManualDeltaUT1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TxtManualDeltaUT1.Location = new System.Drawing.Point(195, 61);
            this.TxtManualDeltaUT1.Name = "TxtManualDeltaUT1";
            this.TxtManualDeltaUT1.Size = new System.Drawing.Size(100, 20);
            this.TxtManualDeltaUT1.TabIndex = 10;
            this.TxtManualDeltaUT1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtDeltaUT1Manuals_Validating);
            // 
            // GrpOnDemandAndAutomaticUpdateConfiguration
            // 
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblRunStatus);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.TxtRunStatus);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.BtnRunAutomaticUpdate);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblTraceEnabled);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.ChkTraceEnabled);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.TxtTraceFilePath);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.BtnSetTraceDirectory);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblAutoSeconds);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.TxtLastRun);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblLastRun);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblAutoTimeout);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.LblAutoDataSource);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.CmbDataSource);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Controls.Add(this.TxtDownloadTimeout);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpOnDemandAndAutomaticUpdateConfiguration.ForeColor = System.Drawing.SystemColors.Highlight;
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Location = new System.Drawing.Point(14, 238);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Name = "GrpOnDemandAndAutomaticUpdateConfiguration";
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Size = new System.Drawing.Size(698, 228);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.TabIndex = 7;
            this.GrpOnDemandAndAutomaticUpdateConfiguration.TabStop = false;
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Text = "On Demand and Automatic Update Configuration";
            this.GrpOnDemandAndAutomaticUpdateConfiguration.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
            // 
            // LblRunStatus
            // 
            this.LblRunStatus.AutoSize = true;
            this.LblRunStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblRunStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblRunStatus.Location = new System.Drawing.Point(123, 198);
            this.LblRunStatus.Name = "LblRunStatus";
            this.LblRunStatus.Size = new System.Drawing.Size(73, 13);
            this.LblRunStatus.TabIndex = 27;
            this.LblRunStatus.Text = "Update status";
            // 
            // TxtRunStatus
            // 
            this.TxtRunStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRunStatus.Location = new System.Drawing.Point(202, 195);
            this.TxtRunStatus.Name = "TxtRunStatus";
            this.TxtRunStatus.ReadOnly = true;
            this.TxtRunStatus.Size = new System.Drawing.Size(321, 20);
            this.TxtRunStatus.TabIndex = 26;
            // 
            // BtnRunAutomaticUpdate
            // 
            this.BtnRunAutomaticUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRunAutomaticUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnRunAutomaticUpdate.Location = new System.Drawing.Point(550, 167);
            this.BtnRunAutomaticUpdate.Name = "BtnRunAutomaticUpdate";
            this.BtnRunAutomaticUpdate.Size = new System.Drawing.Size(119, 23);
            this.BtnRunAutomaticUpdate.TabIndex = 24;
            this.BtnRunAutomaticUpdate.Text = "On Demand Update";
            this.BtnRunAutomaticUpdate.UseVisualStyleBackColor = true;
            this.BtnRunAutomaticUpdate.Click += new System.EventHandler(this.BtnRunAutomaticUpdate_Click);
            // 
            // LblTraceEnabled
            // 
            this.LblTraceEnabled.AutoSize = true;
            this.LblTraceEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTraceEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblTraceEnabled.Location = new System.Drawing.Point(119, 91);
            this.LblTraceEnabled.Name = "LblTraceEnabled";
            this.LblTraceEnabled.Size = new System.Drawing.Size(77, 13);
            this.LblTraceEnabled.TabIndex = 22;
            this.LblTraceEnabled.Text = "Trace Enabled";
            // 
            // ChkTraceEnabled
            // 
            this.ChkTraceEnabled.AutoSize = true;
            this.ChkTraceEnabled.Location = new System.Drawing.Point(202, 91);
            this.ChkTraceEnabled.Name = "ChkTraceEnabled";
            this.ChkTraceEnabled.Size = new System.Drawing.Size(15, 14);
            this.ChkTraceEnabled.TabIndex = 21;
            this.ChkTraceEnabled.UseVisualStyleBackColor = true;
            this.ChkTraceEnabled.CheckedChanged += new System.EventHandler(this.ChkTraceEnabled_CheckedChanged);
            // 
            // TxtTraceFilePath
            // 
            this.TxtTraceFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTraceFilePath.Location = new System.Drawing.Point(202, 111);
            this.TxtTraceFilePath.Name = "TxtTraceFilePath";
            this.TxtTraceFilePath.ReadOnly = true;
            this.TxtTraceFilePath.Size = new System.Drawing.Size(321, 20);
            this.TxtTraceFilePath.TabIndex = 19;
            // 
            // BtnSetTraceDirectory
            // 
            this.BtnSetTraceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSetTraceDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnSetTraceDirectory.Location = new System.Drawing.Point(74, 109);
            this.BtnSetTraceDirectory.Name = "BtnSetTraceDirectory";
            this.BtnSetTraceDirectory.Size = new System.Drawing.Size(122, 23);
            this.BtnSetTraceDirectory.TabIndex = 18;
            this.BtnSetTraceDirectory.Text = "Select Trace Directory";
            this.BtnSetTraceDirectory.UseVisualStyleBackColor = true;
            this.BtnSetTraceDirectory.Click += new System.EventHandler(this.BtnSetTraceDirectory_Click);
            // 
            // LblAutoSeconds
            // 
            this.LblAutoSeconds.AutoSize = true;
            this.LblAutoSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAutoSeconds.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblAutoSeconds.Location = new System.Drawing.Point(262, 66);
            this.LblAutoSeconds.Name = "LblAutoSeconds";
            this.LblAutoSeconds.Size = new System.Drawing.Size(49, 13);
            this.LblAutoSeconds.TabIndex = 17;
            this.LblAutoSeconds.Text = "Seconds";
            // 
            // TxtLastRun
            // 
            this.TxtLastRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtLastRun.Location = new System.Drawing.Point(202, 169);
            this.TxtLastRun.Name = "TxtLastRun";
            this.TxtLastRun.ReadOnly = true;
            this.TxtLastRun.Size = new System.Drawing.Size(321, 20);
            this.TxtLastRun.TabIndex = 19;
            // 
            // LblLastRun
            // 
            this.LblLastRun.AutoSize = true;
            this.LblLastRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLastRun.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblLastRun.Location = new System.Drawing.Point(133, 172);
            this.LblLastRun.Name = "LblLastRun";
            this.LblLastRun.Size = new System.Drawing.Size(63, 13);
            this.LblLastRun.TabIndex = 18;
            this.LblLastRun.Text = "Last update";
            // 
            // LblAutoTimeout
            // 
            this.LblAutoTimeout.AutoSize = true;
            this.LblAutoTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAutoTimeout.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblAutoTimeout.Location = new System.Drawing.Point(104, 66);
            this.LblAutoTimeout.Name = "LblAutoTimeout";
            this.LblAutoTimeout.Size = new System.Drawing.Size(92, 13);
            this.LblAutoTimeout.TabIndex = 13;
            this.LblAutoTimeout.Text = "Download timeout";
            // 
            // LblAutoDataSource
            // 
            this.LblAutoDataSource.AutoSize = true;
            this.LblAutoDataSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAutoDataSource.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblAutoDataSource.Location = new System.Drawing.Point(90, 39);
            this.LblAutoDataSource.Name = "LblAutoDataSource";
            this.LblAutoDataSource.Size = new System.Drawing.Size(106, 13);
            this.LblAutoDataSource.TabIndex = 14;
            this.LblAutoDataSource.Text = "Internet Data Source";
            // 
            // TxtDownloadTimeout
            // 
            this.TxtDownloadTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtDownloadTimeout.Location = new System.Drawing.Point(202, 63);
            this.TxtDownloadTimeout.Name = "TxtDownloadTimeout";
            this.TxtDownloadTimeout.Size = new System.Drawing.Size(45, 20);
            this.TxtDownloadTimeout.TabIndex = 4;
            this.TxtDownloadTimeout.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtDownloadTimeout_Validating);
            // 
            // LblNextLeapSecondsDate
            // 
            this.LblNextLeapSecondsDate.AutoSize = true;
            this.LblNextLeapSecondsDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNextLeapSecondsDate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblNextLeapSecondsDate.Location = new System.Drawing.Point(28, 139);
            this.LblNextLeapSecondsDate.Name = "LblNextLeapSecondsDate";
            this.LblNextLeapSecondsDate.Size = new System.Drawing.Size(130, 13);
            this.LblNextLeapSecondsDate.TabIndex = 21;
            this.LblNextLeapSecondsDate.Text = "Start of next leap seconds";
            // 
            // LblNextLeapSeconds
            // 
            this.LblNextLeapSeconds.AutoSize = true;
            this.LblNextLeapSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNextLeapSeconds.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblNextLeapSeconds.Location = new System.Drawing.Point(63, 113);
            this.LblNextLeapSeconds.Name = "LblNextLeapSeconds";
            this.LblNextLeapSeconds.Size = new System.Drawing.Size(95, 13);
            this.LblNextLeapSeconds.TabIndex = 23;
            this.LblNextLeapSeconds.Text = "Next leap seconds";
            // 
            // TxtNextLeapSeconds
            // 
            this.TxtNextLeapSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNextLeapSeconds.Location = new System.Drawing.Point(164, 110);
            this.TxtNextLeapSeconds.Name = "TxtNextLeapSeconds";
            this.TxtNextLeapSeconds.ReadOnly = true;
            this.TxtNextLeapSeconds.Size = new System.Drawing.Size(100, 20);
            this.TxtNextLeapSeconds.TabIndex = 22;
            // 
            // TxtNextLeapSecondsDate
            // 
            this.TxtNextLeapSecondsDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNextLeapSecondsDate.Location = new System.Drawing.Point(164, 136);
            this.TxtNextLeapSecondsDate.Name = "TxtNextLeapSecondsDate";
            this.TxtNextLeapSecondsDate.ReadOnly = true;
            this.TxtNextLeapSecondsDate.Size = new System.Drawing.Size(250, 20);
            this.TxtNextLeapSecondsDate.TabIndex = 20;
            // 
            // LblAutoRepeatFrequency
            // 
            this.LblAutoRepeatFrequency.AutoSize = true;
            this.LblAutoRepeatFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAutoRepeatFrequency.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblAutoRepeatFrequency.Location = new System.Drawing.Point(94, 65);
            this.LblAutoRepeatFrequency.Name = "LblAutoRepeatFrequency";
            this.LblAutoRepeatFrequency.Size = new System.Drawing.Size(95, 13);
            this.LblAutoRepeatFrequency.TabIndex = 16;
            this.LblAutoRepeatFrequency.Text = "Repeat Frequency";
            // 
            // LblAutoDownloadTime
            // 
            this.LblAutoDownloadTime.AutoSize = true;
            this.LblAutoDownloadTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAutoDownloadTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblAutoDownloadTime.Location = new System.Drawing.Point(34, 38);
            this.LblAutoDownloadTime.Name = "LblAutoDownloadTime";
            this.LblAutoDownloadTime.Size = new System.Drawing.Size(155, 13);
            this.LblAutoDownloadTime.TabIndex = 15;
            this.LblAutoDownloadTime.Text = "Initial Download Date and Time";
            // 
            // CmbScheduleRepeat
            // 
            this.CmbScheduleRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbScheduleRepeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbScheduleRepeat.FormattingEnabled = true;
            this.CmbScheduleRepeat.Location = new System.Drawing.Point(195, 62);
            this.CmbScheduleRepeat.Name = "CmbScheduleRepeat";
            this.CmbScheduleRepeat.Size = new System.Drawing.Size(111, 21);
            this.CmbScheduleRepeat.TabIndex = 12;
            this.CmbScheduleRepeat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ComboBox_DrawItem);
            this.CmbScheduleRepeat.SelectedIndexChanged += new System.EventHandler(this.CmbSchedulRepeat_Changed);
            // 
            // DateScheduleRun
            // 
            this.DateScheduleRun.CustomFormat = "dddd dd MMM yyyy  -  HH:mm:ss";
            this.DateScheduleRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateScheduleRun.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateScheduleRun.Location = new System.Drawing.Point(195, 35);
            this.DateScheduleRun.Name = "DateScheduleRun";
            this.DateScheduleRun.Size = new System.Drawing.Size(250, 20);
            this.DateScheduleRun.TabIndex = 11;
            this.DateScheduleRun.ValueChanged += new System.EventHandler(this.DateScheduleRun_ValueChanged);
            // 
            // BtnClose
            // 
            this.BtnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnClose.Location = new System.Drawing.Point(859, 575);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 5;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            // 
            // CmbUpdateType
            // 
            this.CmbUpdateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbUpdateType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbUpdateType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CmbUpdateType.FormattingEnabled = true;
            this.CmbUpdateType.Location = new System.Drawing.Point(17, 31);
            this.CmbUpdateType.Name = "CmbUpdateType";
            this.CmbUpdateType.Size = new System.Drawing.Size(278, 21);
            this.CmbUpdateType.TabIndex = 8;
            this.CmbUpdateType.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ComboBox_DrawItem);
            this.CmbUpdateType.SelectedIndexChanged += new System.EventHandler(this.CmbUpdateType_Changed);
            // 
            // GrpUpdateType
            // 
            this.GrpUpdateType.Controls.Add(this.CmbUpdateType);
            this.GrpUpdateType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpUpdateType.ForeColor = System.Drawing.SystemColors.Highlight;
            this.GrpUpdateType.Location = new System.Drawing.Point(14, 12);
            this.GrpUpdateType.Name = "GrpUpdateType";
            this.GrpUpdateType.Size = new System.Drawing.Size(323, 72);
            this.GrpUpdateType.TabIndex = 9;
            this.GrpUpdateType.TabStop = false;
            this.GrpUpdateType.Text = "ASCOM Platform Data Source";
            this.GrpUpdateType.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "Select a trace file directory (normally Documents\\ASCOM)";
            this.FolderBrowser.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            // 
            // GrpStatus
            // 
            this.GrpStatus.Controls.Add(this.LblNow);
            this.GrpStatus.Controls.Add(this.TxtNow);
            this.GrpStatus.Controls.Add(this.TxtEffectiveDeltaUT1);
            this.GrpStatus.Controls.Add(this.LblNextLeapSecondsDate);
            this.GrpStatus.Controls.Add(this.TxtEffectiveLeapSeconds);
            this.GrpStatus.Controls.Add(this.LblNextLeapSeconds);
            this.GrpStatus.Controls.Add(this.Label6);
            this.GrpStatus.Controls.Add(this.TxtNextLeapSecondsDate);
            this.GrpStatus.Controls.Add(this.TxtNextLeapSeconds);
            this.GrpStatus.Controls.Add(this.Label2);
            this.GrpStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.GrpStatus.Location = new System.Drawing.Point(495, 34);
            this.GrpStatus.Name = "GrpStatus";
            this.GrpStatus.Size = new System.Drawing.Size(439, 172);
            this.GrpStatus.TabIndex = 26;
            this.GrpStatus.TabStop = false;
            this.GrpStatus.Text = "Values in use by the ASCOM Platform";
            this.GrpStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
            // 
            // LblNow
            // 
            this.LblNow.AutoSize = true;
            this.LblNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblNow.Location = new System.Drawing.Point(96, 87);
            this.LblNow.Name = "LblNow";
            this.LblNow.Size = new System.Drawing.Size(63, 13);
            this.LblNow.TabIndex = 30;
            this.LblNow.Text = "Current time";
            // 
            // TxtNow
            // 
            this.TxtNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNow.Location = new System.Drawing.Point(165, 84);
            this.TxtNow.Name = "TxtNow";
            this.TxtNow.ReadOnly = true;
            this.TxtNow.Size = new System.Drawing.Size(209, 20);
            this.TxtNow.TabIndex = 29;
            // 
            // TxtEffectiveDeltaUT1
            // 
            this.TxtEffectiveDeltaUT1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtEffectiveDeltaUT1.Location = new System.Drawing.Point(165, 58);
            this.TxtEffectiveDeltaUT1.Name = "TxtEffectiveDeltaUT1";
            this.TxtEffectiveDeltaUT1.ReadOnly = true;
            this.TxtEffectiveDeltaUT1.Size = new System.Drawing.Size(209, 20);
            this.TxtEffectiveDeltaUT1.TabIndex = 28;
            // 
            // TxtEffectiveLeapSeconds
            // 
            this.TxtEffectiveLeapSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtEffectiveLeapSeconds.Location = new System.Drawing.Point(165, 32);
            this.TxtEffectiveLeapSeconds.Name = "TxtEffectiveLeapSeconds";
            this.TxtEffectiveLeapSeconds.ReadOnly = true;
            this.TxtEffectiveLeapSeconds.Size = new System.Drawing.Size(209, 20);
            this.TxtEffectiveLeapSeconds.TabIndex = 28;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label6.Location = new System.Drawing.Point(28, 35);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(131, 13);
            this.Label6.TabIndex = 27;
            this.Label6.Text = "TAI - UTC (Leap seconds)";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(41, 61);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(117, 13);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "UT1 - UTC (Delta UT1)";
            // 
            // GrpScheduleTime
            // 
            this.GrpScheduleTime.Controls.Add(this.DateScheduleRun);
            this.GrpScheduleTime.Controls.Add(this.CmbScheduleRepeat);
            this.GrpScheduleTime.Controls.Add(this.LblAutoDownloadTime);
            this.GrpScheduleTime.Controls.Add(this.LblAutoRepeatFrequency);
            this.GrpScheduleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpScheduleTime.ForeColor = System.Drawing.SystemColors.Highlight;
            this.GrpScheduleTime.Location = new System.Drawing.Point(14, 503);
            this.GrpScheduleTime.Name = "GrpScheduleTime";
            this.GrpScheduleTime.Size = new System.Drawing.Size(698, 95);
            this.GrpScheduleTime.TabIndex = 27;
            this.GrpScheduleTime.TabStop = false;
            this.GrpScheduleTime.Text = "Automatic Update Schedule";
            this.GrpScheduleTime.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
            // 
            // BtnHelp
            // 
            this.BtnHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnHelp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnHelp.Location = new System.Drawing.Point(859, 546);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.Size = new System.Drawing.Size(75, 23);
            this.BtnHelp.TabIndex = 28;
            this.BtnHelp.Text = "Help";
            this.BtnHelp.UseVisualStyleBackColor = true;
            this.BtnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // EarthRotationDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 610);
            this.Controls.Add(this.BtnHelp);
            this.Controls.Add(this.GrpScheduleTime);
            this.Controls.Add(this.GrpStatus);
            this.Controls.Add(this.GrpUpdateType);
            this.Controls.Add(this.GrpOnDemandAndAutomaticUpdateConfiguration);
            this.Controls.Add(this.GrpManualUpdate);
            this.Controls.Add(this.BtnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EarthRotationDataForm";
            this.Text = "Earth Rotation Data Update Configuration";
            this.Load += new System.EventHandler(this.EarthRotationDataForm_Load);
            this.GrpManualUpdate.ResumeLayout(false);
            this.GrpManualUpdate.PerformLayout();
            this.GrpOnDemandAndAutomaticUpdateConfiguration.ResumeLayout(false);
            this.GrpOnDemandAndAutomaticUpdateConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.GrpUpdateType.ResumeLayout(false);
            this.GrpStatus.ResumeLayout(false);
            this.GrpStatus.PerformLayout();
            this.GrpScheduleTime.ResumeLayout(false);
            this.GrpScheduleTime.PerformLayout();
            this.ResumeLayout(false);

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