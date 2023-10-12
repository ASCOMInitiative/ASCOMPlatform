using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Alpaca configuration form class
    /// </summary>
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ChooserAlpacaConfigurationForm : Form
    {
        /// <summary>
        /// Dispose of the configuration form
        /// </summary>
        /// <param name="disposing"></param>
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
#pragma warning disable CS0649 // Field 'ChooserAlpacaConfigurationForm.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Field 'ChooserAlpacaConfigurationForm.components' is never assigned to, and will always have its default value null

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooserAlpacaConfigurationForm));
            BtnOK = new Button();
            BtnOK.Click += new EventHandler(BtnOK_Click);
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            NumDiscoveryIpPort = new NumericUpDown();
            ChkDNSResolution = new CheckBox();
            NumDiscoveryBroadcasts = new NumericUpDown();
            NumDiscoveryDuration = new NumericUpDown();
            Label1 = new Label();
            Label2 = new Label();
            Label3 = new Label();
            PictureBox1 = new PictureBox();
            ChkListAllDiscoveredDevices = new CheckBox();
            ChkShowDeviceDetails = new CheckBox();
            NumExtraChooserWidth = new NumericUpDown();
            Label4 = new Label();
            ChkShowCreateNewAlpacaDriverMessage = new CheckBox();
            GrpIpVersion = new GroupBox();
            RadIpV4AndV6 = new RadioButton();
            RadIpV6 = new RadioButton();
            RadIpV4 = new RadioButton();
            ChkMultiThreadedChooser = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryIpPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryBroadcasts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumExtraChooserWidth).BeginInit();
            GrpIpVersion.SuspendLayout();
            SuspendLayout();
            // 
            // BtnOK
            // 
            BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOK.Location = new Point(516, 311);
            BtnOK.Name = "BtnOK";
            BtnOK.Size = new Size(75, 23);
            BtnOK.TabIndex = 5;
            BtnOK.Text = "OK";
            BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(598, 311);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 6;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // NumDiscoveryIpPort
            // 
            NumDiscoveryIpPort.Location = new Point(165, 73);
            NumDiscoveryIpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            NumDiscoveryIpPort.Minimum = new decimal(new int[] { 32227, 0, 0, 0 });
            NumDiscoveryIpPort.Name = "NumDiscoveryIpPort";
            NumDiscoveryIpPort.Size = new Size(120, 20);
            NumDiscoveryIpPort.TabIndex = 1;
            NumDiscoveryIpPort.TextAlign = HorizontalAlignment.Right;
            NumDiscoveryIpPort.Value = new decimal(new int[] { 32227, 0, 0, 0 });
            // 
            // ChkDNSResolution
            // 
            ChkDNSResolution.AutoSize = true;
            ChkDNSResolution.Location = new Point(269, 193);
            ChkDNSResolution.Name = "ChkDNSResolution";
            ChkDNSResolution.Size = new Size(233, 17);
            ChkDNSResolution.TabIndex = 4;
            ChkDNSResolution.Text = "Attempt DNS name resolution (Default false)";
            ChkDNSResolution.UseVisualStyleBackColor = true;
            // 
            // NumDiscoveryBroadcasts
            // 
            NumDiscoveryBroadcasts.Location = new Point(165, 99);
            NumDiscoveryBroadcasts.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumDiscoveryBroadcasts.Name = "NumDiscoveryBroadcasts";
            NumDiscoveryBroadcasts.Size = new Size(120, 20);
            NumDiscoveryBroadcasts.TabIndex = 2;
            NumDiscoveryBroadcasts.TextAlign = HorizontalAlignment.Right;
            NumDiscoveryBroadcasts.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // NumDiscoveryDuration
            // 
            NumDiscoveryDuration.Location = new Point(165, 125);
            NumDiscoveryDuration.Maximum = new decimal(new int[] { 10000, 0, 0, 65536 });
            NumDiscoveryDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumDiscoveryDuration.Name = "NumDiscoveryDuration";
            NumDiscoveryDuration.Size = new Size(120, 20);
            NumDiscoveryDuration.TabIndex = 3;
            NumDiscoveryDuration.TextAlign = HorizontalAlignment.Right;
            NumDiscoveryDuration.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(291, 75);
            Label1.Name = "Label1";
            Label1.Size = new Size(205, 13);
            Label1.TabIndex = 6;
            Label1.Text = "Discovery IP Port Number (Default 32227)";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(291, 101);
            Label2.Name = "Label2";
            Label2.Size = new Size(214, 13);
            Label2.TabIndex = 7;
            Label2.Text = "Number of Discovery Broadcasts (Default 2)";
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(291, 127);
            Label3.Name = "Label3";
            Label3.Size = new Size(187, 13);
            Label3.TabIndex = 8;
            Label3.Text = "Discovery Duration (Default 1 second)";
            // 
            // PictureBox1
            // 
            PictureBox1.Image = My.Resources.Resources.ASCOMAlpacaMidRes;
            PictureBox1.ImageLocation = "";
            PictureBox1.Location = new Point(12, 12);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(100, 76);
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBox1.TabIndex = 9;
            PictureBox1.TabStop = false;
            // 
            // ChkListAllDiscoveredDevices
            // 
            ChkListAllDiscoveredDevices.AutoSize = true;
            ChkListAllDiscoveredDevices.Location = new Point(269, 216);
            ChkListAllDiscoveredDevices.Name = "ChkListAllDiscoveredDevices";
            ChkListAllDiscoveredDevices.Size = new Size(218, 17);
            ChkListAllDiscoveredDevices.TabIndex = 10;
            ChkListAllDiscoveredDevices.Text = "List all discovered devices (Default false)";
            ChkListAllDiscoveredDevices.UseVisualStyleBackColor = true;
            // 
            // ChkShowDeviceDetails
            // 
            ChkShowDeviceDetails.AutoSize = true;
            ChkShowDeviceDetails.Location = new Point(269, 239);
            ChkShowDeviceDetails.Name = "ChkShowDeviceDetails";
            ChkShowDeviceDetails.Size = new Size(189, 17);
            ChkShowDeviceDetails.TabIndex = 11;
            ChkShowDeviceDetails.Text = "Show device details (Default false)";
            ChkShowDeviceDetails.UseVisualStyleBackColor = true;
            // 
            // NumExtraChooserWidth
            // 
            NumExtraChooserWidth.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            NumExtraChooserWidth.Location = new Point(165, 151);
            NumExtraChooserWidth.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            NumExtraChooserWidth.Name = "NumExtraChooserWidth";
            NumExtraChooserWidth.Size = new Size(120, 20);
            NumExtraChooserWidth.TabIndex = 12;
            NumExtraChooserWidth.TextAlign = HorizontalAlignment.Right;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(291, 153);
            Label4.Name = "Label4";
            Label4.Size = new Size(175, 13);
            Label4.TabIndex = 13;
            Label4.Text = "Additional Chooser width (Default 0)";
            // 
            // ChkShowCreateNewAlpacaDriverMessage
            // 
            ChkShowCreateNewAlpacaDriverMessage.AutoSize = true;
            ChkShowCreateNewAlpacaDriverMessage.Location = new Point(269, 262);
            ChkShowCreateNewAlpacaDriverMessage.Name = "ChkShowCreateNewAlpacaDriverMessage";
            ChkShowCreateNewAlpacaDriverMessage.Size = new Size(235, 17);
            ChkShowCreateNewAlpacaDriverMessage.TabIndex = 14;
            ChkShowCreateNewAlpacaDriverMessage.Text = "Show 'Create new Alpaca driver' instructions";
            ChkShowCreateNewAlpacaDriverMessage.UseVisualStyleBackColor = true;
            // 
            // GrpIpVersion
            // 
            GrpIpVersion.Controls.Add(RadIpV4AndV6);
            GrpIpVersion.Controls.Add(RadIpV6);
            GrpIpVersion.Controls.Add(RadIpV4);
            GrpIpVersion.Location = new Point(531, 73);
            GrpIpVersion.Name = "GrpIpVersion";
            GrpIpVersion.Size = new Size(136, 107);
            GrpIpVersion.TabIndex = 15;
            GrpIpVersion.TabStop = false;
            GrpIpVersion.Text = "Supported IP Version(s)";
            // 
            // RadIpV4AndV6
            // 
            RadIpV4AndV6.AutoSize = true;
            RadIpV4AndV6.Location = new Point(6, 78);
            RadIpV4AndV6.Name = "RadIpV4AndV6";
            RadIpV4AndV6.Size = new Size(88, 17);
            RadIpV4AndV6.TabIndex = 2;
            RadIpV4AndV6.TabStop = true;
            RadIpV4AndV6.Text = "IP V4 and V6";
            RadIpV4AndV6.UseVisualStyleBackColor = true;
            // 
            // RadIpV6
            // 
            RadIpV6.AutoSize = true;
            RadIpV6.Location = new Point(6, 52);
            RadIpV6.Name = "RadIpV6";
            RadIpV6.Size = new Size(75, 17);
            RadIpV6.TabIndex = 1;
            RadIpV6.TabStop = true;
            RadIpV6.Text = "IP V6 Only";
            RadIpV6.UseVisualStyleBackColor = true;
            // 
            // RadIpV4
            // 
            RadIpV4.AutoSize = true;
            RadIpV4.Location = new Point(6, 26);
            RadIpV4.Name = "RadIpV4";
            RadIpV4.Size = new Size(75, 17);
            RadIpV4.TabIndex = 0;
            RadIpV4.TabStop = true;
            RadIpV4.Text = "IP V4 Only";
            RadIpV4.UseVisualStyleBackColor = true;
            // 
            // ChkMultiThreadedChooser
            // 
            ChkMultiThreadedChooser.AutoSize = true;
            ChkMultiThreadedChooser.Location = new Point(269, 285);
            ChkMultiThreadedChooser.Name = "ChkMultiThreadedChooser";
            ChkMultiThreadedChooser.Size = new Size(304, 17);
            ChkMultiThreadedChooser.TabIndex = 16;
            ChkMultiThreadedChooser.Text = "DIsplay Chooser while discovery is underway (Default true))";
            ChkMultiThreadedChooser.UseVisualStyleBackColor = true;
            // 
            // ChooserAlpacaConfigurationForm
            // 
            AcceptButton = BtnOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(685, 346);
            Controls.Add(ChkMultiThreadedChooser);
            Controls.Add(GrpIpVersion);
            Controls.Add(ChkShowCreateNewAlpacaDriverMessage);
            Controls.Add(Label4);
            Controls.Add(NumExtraChooserWidth);
            Controls.Add(ChkShowDeviceDetails);
            Controls.Add(ChkListAllDiscoveredDevices);
            Controls.Add(PictureBox1);
            Controls.Add(Label3);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(NumDiscoveryDuration);
            Controls.Add(NumDiscoveryBroadcasts);
            Controls.Add(ChkDNSResolution);
            Controls.Add(NumDiscoveryIpPort);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOK);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ChooserAlpacaConfigurationForm";
            Text = "Alpaca Discovery Configuration";
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryIpPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryBroadcasts).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumDiscoveryDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumExtraChooserWidth).EndInit();
            GrpIpVersion.ResumeLayout(false);
            GrpIpVersion.PerformLayout();
            Load += new EventHandler(ChooserAlpacaConfigurationForm_Load);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Button BtnOK;
        internal Button BtnCancel;
        internal NumericUpDown NumDiscoveryIpPort;
        internal CheckBox ChkDNSResolution;
        internal NumericUpDown NumDiscoveryBroadcasts;
        internal NumericUpDown NumDiscoveryDuration;
        internal Label Label1;
        internal Label Label2;
        internal Label Label3;
        internal PictureBox PictureBox1;
        internal CheckBox ChkListAllDiscoveredDevices;
        internal CheckBox ChkShowDeviceDetails;
        internal NumericUpDown NumExtraChooserWidth;
        internal Label Label4;
        internal CheckBox ChkShowCreateNewAlpacaDriverMessage;
        internal GroupBox GrpIpVersion;
        internal RadioButton RadIpV4AndV6;
        internal RadioButton RadIpV6;
        internal RadioButton RadIpV4;
        internal CheckBox ChkMultiThreadedChooser;
    }
}