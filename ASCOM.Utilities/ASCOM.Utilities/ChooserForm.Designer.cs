using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    internal partial class ChooserForm
    {
        #region Windows Form Designer generated code 
        // Required by the Windows Form Designer
#pragma warning disable CS0649 // Field 'ChooserForm.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Field 'ChooserForm.components' is never assigned to, and will always have its default value null
        public PictureBox picASCOM;
        public Button BtnCancel;
        public Button BtnOK;
        public Button BtnProperties;
        public ComboBox CmbDriverSelector;
        public Label Label1;
        public Label lblTitle;
        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooserForm));
            picASCOM = new PictureBox();
            picASCOM.Click += new EventHandler(PicASCOM_Click);
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(CmdCancel_Click);
            BtnOK = new Button();
            BtnOK.Click += new EventHandler(CmdOK_Click);
            BtnProperties = new Button();
            BtnProperties.Click += new EventHandler(CmdProperties_Click);
            CmbDriverSelector = new ComboBox();
            CmbDriverSelector.SelectionChangeCommitted += new EventHandler(CbDriverSelector_SelectionChangeCommitted);
            Label1 = new Label();
            lblTitle = new Label();
            ChooserMenu = new MenuStrip();
            MnuTrace = new ToolStripMenuItem();
            MnuTrace.DropDownOpening += new EventHandler(MenuTrace_DropDownOpening);
            NormallyLeaveTheseDisabledToolStripMenuItem = new ToolStripMenuItem();
            ToolStripSeparator1 = new ToolStripSeparator();
            MenuSerialTraceEnabled = new ToolStripMenuItem();
            MenuSerialTraceEnabled.Click += new EventHandler(MenuSerialTraceEnabled_Click);
            MenuProfileTraceEnabled = new ToolStripMenuItem();
            MenuProfileTraceEnabled.Click += new EventHandler(MenuProfileTraceEnabled_Click_1);
            MenuRegistryTraceEnabled = new ToolStripMenuItem();
            MenuRegistryTraceEnabled.Click += new EventHandler(MenuRegistryTraceEnabled_Click);
            MenuUtilTraceEnabled = new ToolStripMenuItem();
            MenuUtilTraceEnabled.Click += new EventHandler(MenuUtilTraceEnabled_Click_1);
            MenuSimulatorTraceEnabled = new ToolStripMenuItem();
            MenuSimulatorTraceEnabled.Click += new EventHandler(MenuSimulatorTraceEnabled_Click);
            MenuDriverAccessTraceEnabled = new ToolStripMenuItem();
            MenuDriverAccessTraceEnabled.Click += new EventHandler(MenuDriverAccessTraceEnabled_Click);
            MenuTransformTraceEnabled = new ToolStripMenuItem();
            MenuTransformTraceEnabled.Click += new EventHandler(MenuTransformTraceEnabled_Click);
            MenuNovasTraceEnabled = new ToolStripMenuItem();
            MenuNovasTraceEnabled.Click += new EventHandler(MenuNovasTraceEnabled_Click);
            MenuAstroUtilsTraceEnabled = new ToolStripMenuItem();
            MenuAstroUtilsTraceEnabled.Click += new EventHandler(MenuAstroUtilsTraceEnabled_Click);
            MenuCacheTraceEnabled = new ToolStripMenuItem();
            MenuCacheTraceEnabled.Click += new EventHandler(MenuCacheTraceEnabled_Click);
            MenuEarthRotationDataFormTraceEnabled = new ToolStripMenuItem();
            MenuEarthRotationDataFormTraceEnabled.Click += new EventHandler(MenuEarthRotationDataTraceEnabled_Click);
            MnuAlpaca = new ToolStripMenuItem();
            ToolStripSeparator3 = new ToolStripSeparator();
            MnuDiscoverNow = new ToolStripMenuItem();
            MnuDiscoverNow.Click += new EventHandler(MnuDiscoverNow_Click);
            ToolStripSeparator4 = new ToolStripSeparator();
            MnuEnableDiscovery = new ToolStripMenuItem();
            MnuEnableDiscovery.Click += new EventHandler(MnuEnableDiscovery_Click);
            MnuDisableDiscovery = new ToolStripMenuItem();
            MnuDisableDiscovery.Click += new EventHandler(MnuDisableDiscovery_Click);
            MnuManageAlpacaDevices = new ToolStripMenuItem();
            MnuManageAlpacaDevices.Click += new EventHandler(MnuManageAlpacaDevices_Click);
            MnuConfigureChooser = new ToolStripMenuItem();
            MnuConfigureChooser.Click += new EventHandler(MnuConfigureDiscovery_Click);
            SerialTraceFileName = new SaveFileDialog();
            LblAlpacaDiscovery = new Label();
            AlpacaStatus = new PictureBox();
            DividerLine = new Panel();
            MnuCreateAlpacaDriver = new ToolStripMenuItem();
            MnuCreateAlpacaDriver.Click += new EventHandler(MnuCreateAlpacaDriver_Click);
            ((System.ComponentModel.ISupportInitialize)picASCOM).BeginInit();
            ChooserMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AlpacaStatus).BeginInit();
            SuspendLayout();
            // 
            // picASCOM
            // 
            picASCOM.BackColor = SystemColors.Control;
            picASCOM.ForeColor = SystemColors.ControlText;
            picASCOM.Image = (Image)resources.GetObject("picASCOM.Image");
            picASCOM.Location = new Point(15, 115);
            picASCOM.Name = "picASCOM";
            picASCOM.RightToLeft = RightToLeft.No;
            picASCOM.Size = new Size(48, 56);
            picASCOM.SizeMode = PictureBoxSizeMode.AutoSize;
            picASCOM.TabIndex = 5;
            picASCOM.TabStop = false;
            // 
            // BtnCancel
            // 
            BtnCancel.BackColor = SystemColors.Control;
            BtnCancel.Cursor = Cursors.Default;
            BtnCancel.ForeColor = SystemColors.ControlText;
            BtnCancel.Location = new Point(242, 144);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.RightToLeft = RightToLeft.No;
            BtnCancel.Size = new Size(79, 23);
            BtnCancel.TabIndex = 4;
            BtnCancel.Text = "&Cancel";
            BtnCancel.UseVisualStyleBackColor = false;
            // 
            // BtnOK
            // 
            BtnOK.BackColor = SystemColors.Control;
            BtnOK.Cursor = Cursors.Default;
            BtnOK.Enabled = false;
            BtnOK.ForeColor = SystemColors.ControlText;
            BtnOK.Location = new Point(242, 115);
            BtnOK.Name = "BtnOK";
            BtnOK.RightToLeft = RightToLeft.No;
            BtnOK.Size = new Size(79, 23);
            BtnOK.TabIndex = 3;
            BtnOK.Text = "&OK";
            BtnOK.UseVisualStyleBackColor = false;
            // 
            // BtnProperties
            // 
            BtnProperties.BackColor = SystemColors.Control;
            BtnProperties.Cursor = Cursors.Default;
            BtnProperties.Enabled = false;
            BtnProperties.ForeColor = SystemColors.ControlText;
            BtnProperties.Location = new Point(242, 69);
            BtnProperties.Name = "BtnProperties";
            BtnProperties.RightToLeft = RightToLeft.No;
            BtnProperties.Size = new Size(79, 23);
            BtnProperties.TabIndex = 1;
            BtnProperties.Text = "&Properties...";
            BtnProperties.UseVisualStyleBackColor = false;
            // 
            // CmbDriverSelector
            // 
            CmbDriverSelector.BackColor = SystemColors.Window;
            CmbDriverSelector.Cursor = Cursors.Default;
            CmbDriverSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbDriverSelector.ForeColor = SystemColors.WindowText;
            CmbDriverSelector.Location = new Point(15, 71);
            CmbDriverSelector.Name = "CmbDriverSelector";
            CmbDriverSelector.RightToLeft = RightToLeft.No;
            CmbDriverSelector.Size = new Size(214, 21);
            CmbDriverSelector.Sorted = true;
            CmbDriverSelector.TabIndex = 0;
            // 
            // Label1
            // 
            Label1.BackColor = SystemColors.Control;
            Label1.Cursor = Cursors.Default;
            Label1.ForeColor = SystemColors.ControlText;
            Label1.Location = new Point(69, 117);
            Label1.Name = "Label1";
            Label1.RightToLeft = RightToLeft.No;
            Label1.Size = new Size(160, 54);
            Label1.TabIndex = 6;
            Label1.Text = "Click the logo to learn more about ASCOM, a set of standards for inter-operation " + "of astronomy software.";
            // 
            // lblTitle
            // 
            lblTitle.BackColor = SystemColors.Control;
            lblTitle.Cursor = Cursors.Default;
            lblTitle.ForeColor = SystemColors.ControlText;
            lblTitle.Location = new Point(12, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.RightToLeft = RightToLeft.No;
            lblTitle.Size = new Size(321, 42);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Line 1" + '\r' + '\n' + "Line 2" + '\r' + '\n' + "Line 3";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ChooserMenu
            // 
            ChooserMenu.Items.AddRange(new ToolStripItem[] { MnuTrace, MnuAlpaca });
            ChooserMenu.Location = new Point(0, 0);
            ChooserMenu.Name = "ChooserMenu";
            ChooserMenu.Size = new Size(333, 24);
            ChooserMenu.TabIndex = 7;
            ChooserMenu.Text = "ChooserMenu";
            // 
            // MnuTrace
            // 
            MnuTrace.DropDownItems.AddRange(new ToolStripItem[] { NormallyLeaveTheseDisabledToolStripMenuItem, ToolStripSeparator1, MenuSerialTraceEnabled, MenuProfileTraceEnabled, MenuRegistryTraceEnabled, MenuUtilTraceEnabled, MenuSimulatorTraceEnabled, MenuDriverAccessTraceEnabled, MenuTransformTraceEnabled, MenuNovasTraceEnabled, MenuAstroUtilsTraceEnabled, MenuCacheTraceEnabled, MenuEarthRotationDataFormTraceEnabled });
            MnuTrace.Name = "MnuTrace";
            MnuTrace.Size = new Size(46, 20);
            MnuTrace.Text = "Trace";
            // 
            // NormallyLeaveTheseDisabledToolStripMenuItem
            // 
            NormallyLeaveTheseDisabledToolStripMenuItem.Name = "NormallyLeaveTheseDisabledToolStripMenuItem";
            NormallyLeaveTheseDisabledToolStripMenuItem.Size = new Size(282, 22);
            NormallyLeaveTheseDisabledToolStripMenuItem.Text = "Normally leave these disabled";
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(279, 6);
            // 
            // MenuSerialTraceEnabled
            // 
            MenuSerialTraceEnabled.Name = "MenuSerialTraceEnabled";
            MenuSerialTraceEnabled.Size = new Size(282, 22);
            MenuSerialTraceEnabled.Text = "Serial Trace Enabled";
            // 
            // MenuProfileTraceEnabled
            // 
            MenuProfileTraceEnabled.Name = "MenuProfileTraceEnabled";
            MenuProfileTraceEnabled.Size = new Size(282, 22);
            MenuProfileTraceEnabled.Text = "Profile Trace Enabled";
            // 
            // MenuRegistryTraceEnabled
            // 
            MenuRegistryTraceEnabled.Name = "MenuRegistryTraceEnabled";
            MenuRegistryTraceEnabled.Size = new Size(282, 22);
            MenuRegistryTraceEnabled.Text = "Registry Trace Enabled";
            // 
            // MenuUtilTraceEnabled
            // 
            MenuUtilTraceEnabled.Name = "MenuUtilTraceEnabled";
            MenuUtilTraceEnabled.Size = new Size(282, 22);
            MenuUtilTraceEnabled.Text = "Util Trace Enabled";
            // 
            // MenuSimulatorTraceEnabled
            // 
            MenuSimulatorTraceEnabled.Name = "MenuSimulatorTraceEnabled";
            MenuSimulatorTraceEnabled.Size = new Size(282, 22);
            MenuSimulatorTraceEnabled.Text = "Simulator Trace Enabled";
            // 
            // MenuDriverAccessTraceEnabled
            // 
            MenuDriverAccessTraceEnabled.Name = "MenuDriverAccessTraceEnabled";
            MenuDriverAccessTraceEnabled.Size = new Size(282, 22);
            MenuDriverAccessTraceEnabled.Text = "DriverAccess Trace Enabled";
            // 
            // MenuTransformTraceEnabled
            // 
            MenuTransformTraceEnabled.Name = "MenuTransformTraceEnabled";
            MenuTransformTraceEnabled.Size = new Size(282, 22);
            MenuTransformTraceEnabled.Text = "Transform Trace Enabled";
            // 
            // MenuNovasTraceEnabled
            // 
            MenuNovasTraceEnabled.Name = "MenuNovasTraceEnabled";
            MenuNovasTraceEnabled.Size = new Size(282, 22);
            MenuNovasTraceEnabled.Text = "NOVAS (Partial) Trace Enabled";
            // 
            // MenuAstroUtilsTraceEnabled
            // 
            MenuAstroUtilsTraceEnabled.Name = "MenuAstroUtilsTraceEnabled";
            MenuAstroUtilsTraceEnabled.Size = new Size(282, 22);
            MenuAstroUtilsTraceEnabled.Text = "AstroUtils Trace Enabled";
            // 
            // MenuCacheTraceEnabled
            // 
            MenuCacheTraceEnabled.Name = "MenuCacheTraceEnabled";
            MenuCacheTraceEnabled.Size = new Size(282, 22);
            MenuCacheTraceEnabled.Text = "Cache Trace Enabled";
            // 
            // MenuEarthRotationDataFormTraceEnabled
            // 
            MenuEarthRotationDataFormTraceEnabled.Name = "MenuEarthRotationDataFormTraceEnabled";
            MenuEarthRotationDataFormTraceEnabled.Size = new Size(282, 22);
            MenuEarthRotationDataFormTraceEnabled.Text = "Earth Rotation Data Form Trace Enabled";
            // 
            // MnuAlpaca
            // 
            MnuAlpaca.DropDownItems.AddRange(new ToolStripItem[] { ToolStripSeparator3, MnuDiscoverNow, ToolStripSeparator4, MnuEnableDiscovery, MnuDisableDiscovery, MnuManageAlpacaDevices, MnuCreateAlpacaDriver, MnuConfigureChooser });
            MnuAlpaca.Name = "MnuAlpaca";
            MnuAlpaca.Size = new Size(55, 20);
            MnuAlpaca.Text = "Alpaca";
            // 
            // ToolStripSeparator3
            // 
            ToolStripSeparator3.Name = "ToolStripSeparator3";
            ToolStripSeparator3.Size = new Size(248, 6);
            // 
            // MnuDiscoverNow
            // 
            MnuDiscoverNow.Name = "MnuDiscoverNow";
            MnuDiscoverNow.Size = new Size(251, 22);
            MnuDiscoverNow.Text = "Discover Now";
            // 
            // ToolStripSeparator4
            // 
            ToolStripSeparator4.Name = "ToolStripSeparator4";
            ToolStripSeparator4.Size = new Size(248, 6);
            // 
            // MnuEnableDiscovery
            // 
            MnuEnableDiscovery.Name = "MnuEnableDiscovery";
            MnuEnableDiscovery.Size = new Size(251, 22);
            MnuEnableDiscovery.Text = "Enable DIscovery";
            // 
            // MnuDisableDiscovery
            // 
            MnuDisableDiscovery.Name = "MnuDisableDiscovery";
            MnuDisableDiscovery.Size = new Size(251, 22);
            MnuDisableDiscovery.Text = "Disable Discovery";
            // 
            // MnuManageAlpacaDevices
            // 
            MnuManageAlpacaDevices.Name = "MnuManageAlpacaDevices";
            MnuManageAlpacaDevices.Size = new Size(251, 22);
            MnuManageAlpacaDevices.Text = "Manage Devices (Admin)";
            // 
            // MnuConfigureChooser
            // 
            MnuConfigureChooser.Name = "MnuConfigureChooser";
            MnuConfigureChooser.Size = new Size(251, 22);
            MnuConfigureChooser.Text = "Configure Chooser and Discovery";
            // 
            // LblAlpacaDiscovery
            // 
            LblAlpacaDiscovery.AutoSize = true;
            LblAlpacaDiscovery.BackColor = SystemColors.ControlLightLight;
            LblAlpacaDiscovery.Location = new Point(211, 5);
            LblAlpacaDiscovery.Name = "LblAlpacaDiscovery";
            LblAlpacaDiscovery.Size = new Size(90, 13);
            LblAlpacaDiscovery.TabIndex = 9;
            LblAlpacaDiscovery.Text = "Alpaca Discovery";
            // 
            // AlpacaStatus
            // 
            AlpacaStatus.BackColor = Color.CornflowerBlue;
            AlpacaStatus.BorderStyle = BorderStyle.FixedSingle;
            AlpacaStatus.Location = new Point(305, 8);
            AlpacaStatus.Name = "AlpacaStatus";
            AlpacaStatus.Size = new Size(16, 8);
            AlpacaStatus.TabIndex = 10;
            AlpacaStatus.TabStop = false;
            // 
            // DividerLine
            // 
            DividerLine.BackColor = SystemColors.ActiveCaptionText;
            DividerLine.Location = new Point(15, 102);
            DividerLine.Name = "DividerLine";
            DividerLine.Size = new Size(306, 1);
            DividerLine.TabIndex = 11;
            // 
            // MnuCreateAlpacaDriver
            // 
            MnuCreateAlpacaDriver.Name = "MnuCreateAlpacaDriver";
            MnuCreateAlpacaDriver.Size = new Size(251, 22);
            MnuCreateAlpacaDriver.Text = "Create Alpaca Driver (Admin)";
            // 
            // ChooserForm
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(333, 181);
            Controls.Add(DividerLine);
            Controls.Add(LblAlpacaDiscovery);
            Controls.Add(AlpacaStatus);
            Controls.Add(picASCOM);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOK);
            Controls.Add(BtnProperties);
            Controls.Add(CmbDriverSelector);
            Controls.Add(Label1);
            Controls.Add(lblTitle);
            Controls.Add(ChooserMenu);
            Cursor = Cursors.Default;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Location = new Point(3, 22);
            MainMenuStrip = ChooserMenu;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ChooserForm";
            RightToLeft = RightToLeft.No;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ASCOM <runtime> Chooser";
            ((System.ComponentModel.ISupportInitialize)picASCOM).EndInit();
            ChooserMenu.ResumeLayout(false);
            ChooserMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AlpacaStatus).EndInit();
            Load += new EventHandler(ChooserForm_Load);
            FormClosed += new FormClosedEventHandler(ChooserForm_FormClosed);
            Move += new EventHandler(ChooserFormMoveEventHandler);
            ResumeLayout(false);
            PerformLayout();

        }
        internal MenuStrip ChooserMenu;
        internal ToolStripMenuItem MnuTrace;
        internal SaveFileDialog SerialTraceFileName;
        internal ToolStripMenuItem MenuSerialTraceEnabled;
        internal ToolStripMenuItem MenuProfileTraceEnabled;
        internal ToolStripMenuItem NormallyLeaveTheseDisabledToolStripMenuItem;
        internal ToolStripSeparator ToolStripSeparator1;
        internal ToolStripMenuItem MenuTransformTraceEnabled;
        internal ToolStripMenuItem MenuUtilTraceEnabled;
        internal ToolStripMenuItem MenuSimulatorTraceEnabled;
        internal ToolStripMenuItem MenuDriverAccessTraceEnabled;
        internal ToolStripMenuItem MenuAstroUtilsTraceEnabled;
        internal ToolStripMenuItem MenuNovasTraceEnabled;
        internal ToolStripMenuItem MenuCacheTraceEnabled;
        internal ToolStripMenuItem MenuEarthRotationDataFormTraceEnabled;
        internal ToolStripMenuItem MnuAlpaca;
        internal ToolStripMenuItem MnuEnableDiscovery;
        internal ToolStripMenuItem MnuDiscoverNow;
        internal ToolStripMenuItem MnuConfigureChooser;
        internal Label LblAlpacaDiscovery;
        internal ToolStripSeparator ToolStripSeparator3;
        internal ToolStripSeparator ToolStripSeparator4;
        internal ToolStripMenuItem MnuDisableDiscovery;
        internal PictureBox AlpacaStatus;
        internal ToolStripMenuItem MenuRegistryTraceEnabled;
        internal Panel DividerLine;
        internal ToolStripMenuItem MnuManageAlpacaDevices;
        internal ToolStripMenuItem MnuCreateAlpacaDriver;
        #endregion
    }
}