<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChooserAlpacaConfigurationForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChooserAlpacaConfigurationForm))
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.NumDiscoveryIpPort = New System.Windows.Forms.NumericUpDown()
        Me.ChkDNSResolution = New System.Windows.Forms.CheckBox()
        Me.NumDiscoveryBroadcasts = New System.Windows.Forms.NumericUpDown()
        Me.NumDiscoveryDuration = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.NumDiscoveryIpPort, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumDiscoveryBroadcasts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumDiscoveryDuration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnOK
        '
        Me.BtnOK.Location = New System.Drawing.Point(446, 220)
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.Size = New System.Drawing.Size(75, 23)
        Me.BtnOK.TabIndex = 5
        Me.BtnOK.Text = "OK"
        Me.BtnOK.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.Location = New System.Drawing.Point(527, 220)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 6
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'NumDiscoveryIpPort
        '
        Me.NumDiscoveryIpPort.Location = New System.Drawing.Point(165, 73)
        Me.NumDiscoveryIpPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.NumDiscoveryIpPort.Minimum = New Decimal(New Integer() {1024, 0, 0, 0})
        Me.NumDiscoveryIpPort.Name = "NumDiscoveryIpPort"
        Me.NumDiscoveryIpPort.Size = New System.Drawing.Size(120, 20)
        Me.NumDiscoveryIpPort.TabIndex = 1
        Me.NumDiscoveryIpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumDiscoveryIpPort.Value = New Decimal(New Integer() {1024, 0, 0, 0})
        '
        'ChkDNSResolution
        '
        Me.ChkDNSResolution.AutoSize = True
        Me.ChkDNSResolution.Location = New System.Drawing.Point(273, 151)
        Me.ChkDNSResolution.Name = "ChkDNSResolution"
        Me.ChkDNSResolution.Size = New System.Drawing.Size(233, 17)
        Me.ChkDNSResolution.TabIndex = 4
        Me.ChkDNSResolution.Text = "Attempt DNS name resolution (Default false)"
        Me.ChkDNSResolution.UseVisualStyleBackColor = True
        '
        'NumDiscoveryBroadcasts
        '
        Me.NumDiscoveryBroadcasts.Location = New System.Drawing.Point(165, 99)
        Me.NumDiscoveryBroadcasts.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumDiscoveryBroadcasts.Name = "NumDiscoveryBroadcasts"
        Me.NumDiscoveryBroadcasts.Size = New System.Drawing.Size(120, 20)
        Me.NumDiscoveryBroadcasts.TabIndex = 2
        Me.NumDiscoveryBroadcasts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumDiscoveryBroadcasts.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'NumDiscoveryDuration
        '
        Me.NumDiscoveryDuration.DecimalPlaces = 1
        Me.NumDiscoveryDuration.Location = New System.Drawing.Point(165, 125)
        Me.NumDiscoveryDuration.Maximum = New Decimal(New Integer() {10000, 0, 0, 65536})
        Me.NumDiscoveryDuration.Minimum = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.NumDiscoveryDuration.Name = "NumDiscoveryDuration"
        Me.NumDiscoveryDuration.Size = New System.Drawing.Size(120, 20)
        Me.NumDiscoveryDuration.TabIndex = 3
        Me.NumDiscoveryDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumDiscoveryDuration.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(291, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(215, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Discovery IP Port Number (Default is 32227)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(291, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Number of Discovery Broadcasts (Default is 1)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(291, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(211, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Discovery Duration (Default is 2.0 seconds)"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ASCOM.Utilities.My.Resources.Resources.ASCOMAlpacaMidRes
        Me.PictureBox1.ImageLocation = ""
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 76)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'ChooserAlpacaConfigurationForm
        '
        Me.AcceptButton = Me.BtnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(614, 255)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.NumDiscoveryDuration)
        Me.Controls.Add(Me.NumDiscoveryBroadcasts)
        Me.Controls.Add(Me.ChkDNSResolution)
        Me.Controls.Add(Me.NumDiscoveryIpPort)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ChooserAlpacaConfigurationForm"
        Me.Text = "Alpaca Discovery Configuration"
        CType(Me.NumDiscoveryIpPort, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumDiscoveryBroadcasts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumDiscoveryDuration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnOK As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents NumDiscoveryIpPort As NumericUpDown
    Friend WithEvents ChkDNSResolution As CheckBox
    Friend WithEvents NumDiscoveryBroadcasts As NumericUpDown
    Friend WithEvents NumDiscoveryDuration As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents PictureBox1 As PictureBox
End Class
