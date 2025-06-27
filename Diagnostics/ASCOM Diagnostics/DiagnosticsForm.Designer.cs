using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    public partial class DiagnosticsForm
    {
        /// <summary>
        /// Form overrides dispose to clean up the component list. 
        /// </summary>
        /// <param name="disposing"></param>
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
        private System.ComponentModel.IContainer components = null;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosticsForm));
            this.btnRunDiagnostics = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuChooseDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooserNETToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListAvailableCOMPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EarthRotationDataUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLeaveUnset = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuUseTraceAutoFilenames = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUseTraceManualFilename = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSerialTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuIncludeSerialTraceDebugInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuProfileTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRegistryTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUtilTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTimerTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSimulatorTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDriverAccessTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTransformTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNovasTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAstroUtilsTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCacheTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEarthRotationDataFormTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuThrowAbandonedMutexExceptions = new System.Windows.Forms.ToolStripMenuItem();
            this.SerialWaitTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuWaitTypeManualResetEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuWaitTypeSleep = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuWaitTypeWaitForSingleObject = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDiagnosticsTraceEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAutoViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsCheckForPlatformReleases = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsCheckForPlatformPreReleases = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsUseOmniSimulators = new System.Windows.Forms.ToolStripMenuItem();
            this.SetLogFileLocationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayUnicodeInTraceLoggerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAction = new System.Windows.Forms.Label();
            this.btnViewLastLog = new System.Windows.Forms.Button();
            this.SerialTraceFileName = new System.Windows.Forms.SaveFileDialog();
            this.BtnUpdateAvailable = new System.Windows.Forms.Button();
            this.ReportNET35ComponentUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunDiagnostics
            // 
            this.btnRunDiagnostics.Location = new System.Drawing.Point(408, 202);
            this.btnRunDiagnostics.Name = "btnRunDiagnostics";
            this.btnRunDiagnostics.Size = new System.Drawing.Size(110, 23);
            this.btnRunDiagnostics.TabIndex = 0;
            this.btnRunDiagnostics.Text = "Run Diagnostics";
            this.btnRunDiagnostics.UseVisualStyleBackColor = true;
            this.btnRunDiagnostics.Click += new System.EventHandler(this.RunDiagnostics);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(408, 231);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(110, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(11, 104);
            this.lblMessage.MinimumSize = new System.Drawing.Size(505, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(505, 13);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(11, 41);
            this.lblTitle.MinimumSize = new System.Drawing.Size(505, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(505, 19);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "ASCOM Diagnostics";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(2, 206);
            this.lblResult.MaximumSize = new System.Drawing.Size(400, 13);
            this.lblResult.MinimumSize = new System.Drawing.Size(400, 13);
            this.lblResult.Name = "lblResult";
            this.lblResult.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblResult.Size = new System.Drawing.Size(400, 13);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChooseDevice,
            this.mnuTools,
            this.mnuTrace,
            this.OptionsToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(530, 24);
            this.MenuStrip1.TabIndex = 5;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // mnuChooseDevice
            // 
            this.mnuChooseDevice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem,
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem});
            this.mnuChooseDevice.Name = "mnuChooseDevice";
            this.mnuChooseDevice.Size = new System.Drawing.Size(97, 20);
            this.mnuChooseDevice.Text = "Choose Device";
            // 
            // ChooseAndConnectToDevice64bitApplicationToolStripMenuItem
            // 
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Name = "ChooseAndConnectToDevice64bitApplicationToolStripMenuItem";
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Size = new System.Drawing.Size(336, 22);
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Text = "Choose and Connect to Device";
            this.ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Click += new System.EventHandler(this.ChooseAndConncectToDevice64bitApplicationToolStripMenuItem_Click);
            // 
            // ChooseAndConnectToDevice32bitApplicationToolStripMenuItem
            // 
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Name = "ChooseAndConnectToDevice32bitApplicationToolStripMenuItem";
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Size = new System.Drawing.Size(336, 22);
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Text = "Choose and Connect to Device (32bit application)";
            this.ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Click += new System.EventHandler(this.ChooseAndConnectToDevice32BitApplicationToolStripMenuItem_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooserToolStripMenuItem1,
            this.ChooserNETToolStripMenuItem,
            this.ListAvailableCOMPortsToolStripMenuItem,
            this.EarthRotationDataUpdateToolStripMenuItem,
            this.ReportNET35ComponentUseToolStripMenuItem});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(47, 20);
            this.mnuTools.Text = "Tools";
            // 
            // ChooserToolStripMenuItem1
            // 
            this.ChooserToolStripMenuItem1.Name = "ChooserToolStripMenuItem1";
            this.ChooserToolStripMenuItem1.Size = new System.Drawing.Size(244, 22);
            this.ChooserToolStripMenuItem1.Text = "Telescope Chooser (using COM)";
            this.ChooserToolStripMenuItem1.Click += new System.EventHandler(this.ChooserToolStripMenuItem1_Click);
            // 
            // ChooserNETToolStripMenuItem
            // 
            this.ChooserNETToolStripMenuItem.Name = "ChooserNETToolStripMenuItem";
            this.ChooserNETToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.ChooserNETToolStripMenuItem.Text = "Telescope Chooser (using .NET)";
            this.ChooserNETToolStripMenuItem.Click += new System.EventHandler(this.ChooserNETToolStripMenuItem_Click);
            // 
            // ListAvailableCOMPortsToolStripMenuItem
            // 
            this.ListAvailableCOMPortsToolStripMenuItem.Name = "ListAvailableCOMPortsToolStripMenuItem";
            this.ListAvailableCOMPortsToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.ListAvailableCOMPortsToolStripMenuItem.Text = "List Available COM Ports";
            this.ListAvailableCOMPortsToolStripMenuItem.Click += new System.EventHandler(this.ListAvailableCOMPortsToolStripMenuItem_Click);
            // 
            // EarthRotationDataUpdateToolStripMenuItem
            // 
            this.EarthRotationDataUpdateToolStripMenuItem.Name = "EarthRotationDataUpdateToolStripMenuItem";
            this.EarthRotationDataUpdateToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.EarthRotationDataUpdateToolStripMenuItem.Text = "Manage Earth Rotation Data";
            this.EarthRotationDataUpdateToolStripMenuItem.Click += new System.EventHandler(this.EarthRotationDataUpdateToolStripMenuItem_Click);
            // 
            // mnuTrace
            // 
            this.mnuTrace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLeaveUnset,
            this.ToolStripSeparator1,
            this.MenuUseTraceAutoFilenames,
            this.MenuUseTraceManualFilename,
            this.MenuSerialTraceEnabled,
            this.MenuIncludeSerialTraceDebugInformation,
            this.MenuProfileTraceEnabled,
            this.MenuRegistryTraceEnabled,
            this.MenuUtilTraceEnabled,
            this.MenuTimerTraceEnabled,
            this.MenuSimulatorTraceEnabled,
            this.MenuDriverAccessTraceEnabled,
            this.MenuTransformTraceEnabled,
            this.MenuNovasTraceEnabled,
            this.MenuAstroUtilsTraceEnabled,
            this.MenuCacheTraceEnabled,
            this.MenuEarthRotationDataFormTraceEnabled,
            this.MenuThrowAbandonedMutexExceptions,
            this.SerialWaitTypeToolStripMenuItem,
            this.MenuDiagnosticsTraceEnabled});
            this.mnuTrace.Name = "mnuTrace";
            this.mnuTrace.Size = new System.Drawing.Size(47, 20);
            this.mnuTrace.Text = "Trace";
            this.mnuTrace.DropDownOpening += new System.EventHandler(this.MnuTrace_DropDownOpening);
            // 
            // mnuLeaveUnset
            // 
            this.mnuLeaveUnset.Name = "mnuLeaveUnset";
            this.mnuLeaveUnset.Size = new System.Drawing.Size(283, 22);
            this.mnuLeaveUnset.Text = "Normally leave these options disabled";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(280, 6);
            // 
            // MenuUseTraceAutoFilenames
            // 
            this.MenuUseTraceAutoFilenames.Name = "MenuUseTraceAutoFilenames";
            this.MenuUseTraceAutoFilenames.Size = new System.Drawing.Size(283, 22);
            this.MenuUseTraceAutoFilenames.Text = "Use Automatic Serial Trace Filenames";
            this.MenuUseTraceAutoFilenames.Click += new System.EventHandler(this.MenuAutoTraceFilenames_Click);
            // 
            // MenuUseTraceManualFilename
            // 
            this.MenuUseTraceManualFilename.Name = "MenuUseTraceManualFilename";
            this.MenuUseTraceManualFilename.Size = new System.Drawing.Size(283, 22);
            this.MenuUseTraceManualFilename.Text = "Use a Manual Serial Trace Filename";
            this.MenuUseTraceManualFilename.Click += new System.EventHandler(this.MenuUseTraceManualFilename_Click);
            // 
            // MenuSerialTraceEnabled
            // 
            this.MenuSerialTraceEnabled.Name = "MenuSerialTraceEnabled";
            this.MenuSerialTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuSerialTraceEnabled.Text = "Serial Trace Enabled";
            this.MenuSerialTraceEnabled.Click += new System.EventHandler(this.MenuSerialTraceEnabled_Click);
            // 
            // MenuIncludeSerialTraceDebugInformation
            // 
            this.MenuIncludeSerialTraceDebugInformation.Name = "MenuIncludeSerialTraceDebugInformation";
            this.MenuIncludeSerialTraceDebugInformation.Size = new System.Drawing.Size(283, 22);
            this.MenuIncludeSerialTraceDebugInformation.Text = "Include Serial Trace Debug Information";
            this.MenuIncludeSerialTraceDebugInformation.Click += new System.EventHandler(this.MenuIncludeSerialTraceDebugInformation_Click);
            // 
            // MenuProfileTraceEnabled
            // 
            this.MenuProfileTraceEnabled.Name = "MenuProfileTraceEnabled";
            this.MenuProfileTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuProfileTraceEnabled.Text = "Profile Trace Enabled";
            this.MenuProfileTraceEnabled.Click += new System.EventHandler(this.MenuProfileTraceEnabled_Click_1);
            // 
            // MenuRegistryTraceEnabled
            // 
            this.MenuRegistryTraceEnabled.Name = "MenuRegistryTraceEnabled";
            this.MenuRegistryTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuRegistryTraceEnabled.Text = "Registry Trace Enabled";
            this.MenuRegistryTraceEnabled.Click += new System.EventHandler(this.MenuRegistryTraceEnabled_Click);
            // 
            // MenuUtilTraceEnabled
            // 
            this.MenuUtilTraceEnabled.Name = "MenuUtilTraceEnabled";
            this.MenuUtilTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuUtilTraceEnabled.Text = "Util Trace Enabled";
            this.MenuUtilTraceEnabled.Click += new System.EventHandler(this.MenuUtilTraceEnabled_Click_1);
            // 
            // MenuTimerTraceEnabled
            // 
            this.MenuTimerTraceEnabled.Name = "MenuTimerTraceEnabled";
            this.MenuTimerTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuTimerTraceEnabled.Text = "Timer Timer Enabled";
            this.MenuTimerTraceEnabled.Click += new System.EventHandler(this.MenuTimerTraceEnabled_Click);
            // 
            // MenuSimulatorTraceEnabled
            // 
            this.MenuSimulatorTraceEnabled.Name = "MenuSimulatorTraceEnabled";
            this.MenuSimulatorTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuSimulatorTraceEnabled.Text = "Simulator Trace Enabled";
            this.MenuSimulatorTraceEnabled.Click += new System.EventHandler(this.MenuSimulatorTraceEnabled_Click);
            // 
            // MenuDriverAccessTraceEnabled
            // 
            this.MenuDriverAccessTraceEnabled.Name = "MenuDriverAccessTraceEnabled";
            this.MenuDriverAccessTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuDriverAccessTraceEnabled.Text = "DriverAccess Trace Enabled";
            this.MenuDriverAccessTraceEnabled.Click += new System.EventHandler(this.MenuDriverAccessTraceEnabled_Click);
            // 
            // MenuTransformTraceEnabled
            // 
            this.MenuTransformTraceEnabled.Name = "MenuTransformTraceEnabled";
            this.MenuTransformTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuTransformTraceEnabled.Text = "Transform Trace Enabled";
            this.MenuTransformTraceEnabled.Click += new System.EventHandler(this.MenuTransformTraceEnabled_Click);
            // 
            // MenuNovasTraceEnabled
            // 
            this.MenuNovasTraceEnabled.Name = "MenuNovasTraceEnabled";
            this.MenuNovasTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuNovasTraceEnabled.Text = "NOVAS (Partial) Trace Enabled";
            this.MenuNovasTraceEnabled.Click += new System.EventHandler(this.MenuNovasTraceEnabled_Click);
            // 
            // MenuAstroUtilsTraceEnabled
            // 
            this.MenuAstroUtilsTraceEnabled.Name = "MenuAstroUtilsTraceEnabled";
            this.MenuAstroUtilsTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuAstroUtilsTraceEnabled.Text = "AstroUtils Trace Enabled";
            this.MenuAstroUtilsTraceEnabled.Click += new System.EventHandler(this.MenuAstroUtilsTraceEnabled_Click);
            // 
            // MenuCacheTraceEnabled
            // 
            this.MenuCacheTraceEnabled.Name = "MenuCacheTraceEnabled";
            this.MenuCacheTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuCacheTraceEnabled.Text = "Cache Trace Enabled";
            this.MenuCacheTraceEnabled.Click += new System.EventHandler(this.MenuCacheTraceEnabled_Click);
            // 
            // MenuEarthRotationDataFormTraceEnabled
            // 
            this.MenuEarthRotationDataFormTraceEnabled.Name = "MenuEarthRotationDataFormTraceEnabled";
            this.MenuEarthRotationDataFormTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuEarthRotationDataFormTraceEnabled.Text = "Earth Rotation Data Form Trace Enabled";
            this.MenuEarthRotationDataFormTraceEnabled.Click += new System.EventHandler(this.MenuEarthRotationScheduledJobTraceEnabled_Click);
            // 
            // MenuThrowAbandonedMutexExceptions
            // 
            this.MenuThrowAbandonedMutexExceptions.Name = "MenuThrowAbandonedMutexExceptions";
            this.MenuThrowAbandonedMutexExceptions.Size = new System.Drawing.Size(283, 22);
            this.MenuThrowAbandonedMutexExceptions.Text = "Throw Abandoned Mutex Exceptions";
            this.MenuThrowAbandonedMutexExceptions.Visible = false;
            this.MenuThrowAbandonedMutexExceptions.Click += new System.EventHandler(this.MenuThrowAbandonedMutexExceptions_Click);
            // 
            // SerialWaitTypeToolStripMenuItem
            // 
            this.SerialWaitTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuWaitTypeManualResetEvent,
            this.MenuWaitTypeSleep,
            this.MenuWaitTypeWaitForSingleObject});
            this.SerialWaitTypeToolStripMenuItem.Name = "SerialWaitTypeToolStripMenuItem";
            this.SerialWaitTypeToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
            this.SerialWaitTypeToolStripMenuItem.Text = "Serial Wait Type";
            this.SerialWaitTypeToolStripMenuItem.Visible = false;
            // 
            // MenuWaitTypeManualResetEvent
            // 
            this.MenuWaitTypeManualResetEvent.Name = "MenuWaitTypeManualResetEvent";
            this.MenuWaitTypeManualResetEvent.Size = new System.Drawing.Size(182, 22);
            this.MenuWaitTypeManualResetEvent.Text = "ManualResetEvent";
            this.MenuWaitTypeManualResetEvent.Click += new System.EventHandler(this.MenuWaitTypeManualResetEvent_Click);
            // 
            // MenuWaitTypeSleep
            // 
            this.MenuWaitTypeSleep.Name = "MenuWaitTypeSleep";
            this.MenuWaitTypeSleep.Size = new System.Drawing.Size(182, 22);
            this.MenuWaitTypeSleep.Text = "Sleep";
            this.MenuWaitTypeSleep.Click += new System.EventHandler(this.MenuWaitTypeSleep_Click);
            // 
            // MenuWaitTypeWaitForSingleObject
            // 
            this.MenuWaitTypeWaitForSingleObject.Name = "MenuWaitTypeWaitForSingleObject";
            this.MenuWaitTypeWaitForSingleObject.Size = new System.Drawing.Size(182, 22);
            this.MenuWaitTypeWaitForSingleObject.Text = "WaitForSingleObject";
            this.MenuWaitTypeWaitForSingleObject.Click += new System.EventHandler(this.MenuWaitTypeWaitForSingleObject_Click);
            // 
            // MenuDiagnosticsTraceEnabled
            // 
            this.MenuDiagnosticsTraceEnabled.Name = "MenuDiagnosticsTraceEnabled";
            this.MenuDiagnosticsTraceEnabled.Size = new System.Drawing.Size(283, 22);
            this.MenuDiagnosticsTraceEnabled.Text = "Diagnostics Trace Enabled";
            this.MenuDiagnosticsTraceEnabled.Click += new System.EventHandler(this.MenuDiagnosticsTraceEnabled_Click);
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAutoViewLog,
            this.OptionsCheckForPlatformReleases,
            this.OptionsCheckForPlatformPreReleases,
            this.OptionsUseOmniSimulators,
            this.SetLogFileLocationToolStripMenuItem1,
            this.DisplayUnicodeInTraceLoggerMenuItem});
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "Options";
            // 
            // MenuAutoViewLog
            // 
            this.MenuAutoViewLog.Name = "MenuAutoViewLog";
            this.MenuAutoViewLog.Size = new System.Drawing.Size(321, 22);
            this.MenuAutoViewLog.Text = "Automatically view log after run";
            this.MenuAutoViewLog.Click += new System.EventHandler(this.MenuAutoViewLog_Click);
            // 
            // OptionsCheckForPlatformReleases
            // 
            this.OptionsCheckForPlatformReleases.Name = "OptionsCheckForPlatformReleases";
            this.OptionsCheckForPlatformReleases.Size = new System.Drawing.Size(321, 22);
            this.OptionsCheckForPlatformReleases.Text = "Check for Platform updates";
            this.OptionsCheckForPlatformReleases.Click += new System.EventHandler(this.OptionsCheckForPlatformReleases_Click);
            // 
            // OptionsCheckForPlatformPreReleases
            // 
            this.OptionsCheckForPlatformPreReleases.Name = "OptionsCheckForPlatformPreReleases";
            this.OptionsCheckForPlatformPreReleases.Size = new System.Drawing.Size(321, 22);
            this.OptionsCheckForPlatformPreReleases.Text = "Check for Platform pre-release updates";
            this.OptionsCheckForPlatformPreReleases.Click += new System.EventHandler(this.OptionsCheckForPlatformPreReleases_Click);
            // 
            // OptionsUseOmniSimulators
            // 
            this.OptionsUseOmniSimulators.Name = "OptionsUseOmniSimulators";
            this.OptionsUseOmniSimulators.Size = new System.Drawing.Size(321, 22);
            this.OptionsUseOmniSimulators.Text = "Use Omni-Simulators as Platform simulators";
            this.OptionsUseOmniSimulators.Click += new System.EventHandler(this.OptionsUseOmniSimulators_Click);
            // 
            // SetLogFileLocationToolStripMenuItem1
            // 
            this.SetLogFileLocationToolStripMenuItem1.Name = "SetLogFileLocationToolStripMenuItem1";
            this.SetLogFileLocationToolStripMenuItem1.Size = new System.Drawing.Size(321, 22);
            this.SetLogFileLocationToolStripMenuItem1.Text = "Set Log File Root Folder";
            this.SetLogFileLocationToolStripMenuItem1.Click += new System.EventHandler(this.SetLogFileLocationToolStripMenuItem_Click);
            // 
            // DisplayUnicodeInTraceLoggerMenuItem
            // 
            this.DisplayUnicodeInTraceLoggerMenuItem.Name = "DisplayUnicodeInTraceLoggerMenuItem";
            this.DisplayUnicodeInTraceLoggerMenuItem.Size = new System.Drawing.Size(321, 22);
            this.DisplayUnicodeInTraceLoggerMenuItem.Text = "Display Unicode characters in TraceLogger files";
            this.DisplayUnicodeInTraceLoggerMenuItem.Click += new System.EventHandler(this.DisplayUnicodeInTraceLoggerMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(22, 236);
            this.lblAction.MaximumSize = new System.Drawing.Size(380, 13);
            this.lblAction.MinimumSize = new System.Drawing.Size(380, 13);
            this.lblAction.Name = "lblAction";
            this.lblAction.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblAction.Size = new System.Drawing.Size(380, 13);
            this.lblAction.TabIndex = 6;
            this.lblAction.Text = "Action";
            this.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnViewLastLog
            // 
            this.btnViewLastLog.Enabled = false;
            this.btnViewLastLog.Location = new System.Drawing.Point(408, 173);
            this.btnViewLastLog.Name = "btnViewLastLog";
            this.btnViewLastLog.Size = new System.Drawing.Size(110, 23);
            this.btnViewLastLog.TabIndex = 7;
            this.btnViewLastLog.Text = "View Last Log";
            this.btnViewLastLog.UseVisualStyleBackColor = true;
            this.btnViewLastLog.Click += new System.EventHandler(this.BtnLastLog_Click);
            // 
            // BtnUpdateAvailable
            // 
            this.BtnUpdateAvailable.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnUpdateAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpdateAvailable.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.BtnUpdateAvailable.Location = new System.Drawing.Point(408, 71);
            this.BtnUpdateAvailable.Name = "BtnUpdateAvailable";
            this.BtnUpdateAvailable.Size = new System.Drawing.Size(110, 42);
            this.BtnUpdateAvailable.TabIndex = 8;
            this.BtnUpdateAvailable.Text = "Platform Update Available";
            this.BtnUpdateAvailable.UseVisualStyleBackColor = false;
            this.BtnUpdateAvailable.Visible = false;
            this.BtnUpdateAvailable.Click += new System.EventHandler(this.BtnUpdateAvailable_Click);
            // 
            // ReportNET35ComponentUseToolStripMenuItem
            // 
            this.ReportNET35ComponentUseToolStripMenuItem.Name = "ReportNET35ComponentUseToolStripMenuItem";
            this.ReportNET35ComponentUseToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.ReportNET35ComponentUseToolStripMenuItem.Text = "Report .NET 3.5 Component Use";
            this.ReportNET35ComponentUseToolStripMenuItem.Click += new System.EventHandler(this.ReportNET35ComponentUseToolStripMenuItem_Click);
            // 
            // DiagnosticsForm
            // 
            this.AcceptButton = this.btnRunDiagnostics;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(530, 266);
            this.Controls.Add(this.BtnUpdateAvailable);
            this.Controls.Add(this.btnViewLastLog);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRunDiagnostics);
            this.Controls.Add(this.MenuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "DiagnosticsForm";
            this.Text = "ASCOM Diagnostics";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiagnosticsForm_KeyDown);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal Button btnRunDiagnostics;
        internal Button btnExit;
        internal Label lblMessage;
        internal Label lblTitle;
        internal Label lblResult;
        internal MenuStrip MenuStrip1;
        internal ToolStripMenuItem mnuTools;
        internal ToolStripMenuItem ChooserToolStripMenuItem1;
        internal ToolStripMenuItem ChooserNETToolStripMenuItem;
        internal Label lblAction;
        internal ToolStripMenuItem ListAvailableCOMPortsToolStripMenuItem;
        internal Button btnViewLastLog;
        internal ToolStripMenuItem mnuChooseDevice;
        internal ToolStripMenuItem ChooseAndConnectToDevice64bitApplicationToolStripMenuItem;
        internal ToolStripMenuItem mnuTrace;
        internal ToolStripMenuItem mnuLeaveUnset;
        internal ToolStripSeparator ToolStripSeparator1;
        internal ToolStripMenuItem MenuUseTraceAutoFilenames;
        internal ToolStripMenuItem MenuUseTraceManualFilename;
        internal ToolStripMenuItem MenuSerialTraceEnabled;
        internal ToolStripMenuItem MenuIncludeSerialTraceDebugInformation;
        internal ToolStripMenuItem MenuProfileTraceEnabled;
        internal ToolStripMenuItem MenuTransformTraceEnabled;
        internal ToolStripMenuItem MenuUtilTraceEnabled;
        internal ToolStripMenuItem MenuTimerTraceEnabled;
        internal ToolStripMenuItem MenuSimulatorTraceEnabled;
        internal ToolStripMenuItem MenuDriverAccessTraceEnabled;
        internal ToolStripMenuItem AboutToolStripMenuItem;
        internal ToolStripMenuItem MenuThrowAbandonedMutexExceptions;
        internal ToolStripMenuItem MenuAstroUtilsTraceEnabled;
        internal ToolStripMenuItem MenuNovasTraceEnabled;
        internal ToolStripMenuItem ChooseAndConnectToDevice32bitApplicationToolStripMenuItem;
        internal SaveFileDialog SerialTraceFileName;
        internal ToolStripMenuItem SerialWaitTypeToolStripMenuItem;
        internal ToolStripMenuItem MenuWaitTypeSleep;
        internal ToolStripMenuItem MenuWaitTypeManualResetEvent;
        internal ToolStripMenuItem MenuWaitTypeWaitForSingleObject;
        internal ToolStripMenuItem MenuCacheTraceEnabled;
        internal ToolStripMenuItem OptionsToolStripMenuItem;
        internal ToolStripMenuItem MenuAutoViewLog;
        internal ToolStripMenuItem EarthRotationDataUpdateToolStripMenuItem;
        internal ToolStripMenuItem MenuEarthRotationDataFormTraceEnabled;
        internal ToolStripMenuItem MenuRegistryTraceEnabled;
        private ToolStripMenuItem OptionsCheckForPlatformReleases;
        private ToolStripMenuItem OptionsCheckForPlatformPreReleases;
        internal Button BtnUpdateAvailable;
        private ToolStripMenuItem OptionsUseOmniSimulators;
        private ToolStripMenuItem MenuDiagnosticsTraceEnabled;
        private ToolStripMenuItem SetLogFileLocationToolStripMenuItem1;
        private ToolStripMenuItem DisplayUnicodeInTraceLoggerMenuItem;
        private ToolStripMenuItem ReportNET35ComponentUseToolStripMenuItem;
    }
}