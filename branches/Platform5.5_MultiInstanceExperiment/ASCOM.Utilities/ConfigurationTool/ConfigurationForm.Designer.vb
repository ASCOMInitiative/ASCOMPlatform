<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigurationForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigurationForm))
        Me.cmbDeviceType = New System.Windows.Forms.ComboBox
        Me.lblDeviceType = New System.Windows.Forms.Label
        Me.lblDevice = New System.Windows.Forms.Label
        Me.cmbDevice = New System.Windows.Forms.ComboBox
        Me.lstAliases = New System.Windows.Forms.CheckedListBox
        Me.lblAliases = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmbDeviceType
        '
        Me.cmbDeviceType.FormattingEnabled = True
        Me.cmbDeviceType.Location = New System.Drawing.Point(87, 12)
        Me.cmbDeviceType.Name = "cmbDeviceType"
        Me.cmbDeviceType.Size = New System.Drawing.Size(244, 21)
        Me.cmbDeviceType.TabIndex = 0
        '
        'lblDeviceType
        '
        Me.lblDeviceType.AutoSize = True
        Me.lblDeviceType.Location = New System.Drawing.Point(13, 15)
        Me.lblDeviceType.Name = "lblDeviceType"
        Me.lblDeviceType.Size = New System.Drawing.Size(68, 13)
        Me.lblDeviceType.TabIndex = 1
        Me.lblDeviceType.Text = "Device Type"
        '
        'lblDevice
        '
        Me.lblDevice.AutoSize = True
        Me.lblDevice.Location = New System.Drawing.Point(40, 57)
        Me.lblDevice.Name = "lblDevice"
        Me.lblDevice.Size = New System.Drawing.Size(41, 13)
        Me.lblDevice.TabIndex = 2
        Me.lblDevice.Text = "Device"
        '
        'cmbDevice
        '
        Me.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDevice.FormattingEnabled = True
        Me.cmbDevice.Location = New System.Drawing.Point(87, 54)
        Me.cmbDevice.Name = "cmbDevice"
        Me.cmbDevice.Size = New System.Drawing.Size(613, 21)
        Me.cmbDevice.TabIndex = 3
        '
        'lstAliases
        '
        Me.lstAliases.FormattingEnabled = True
        Me.lstAliases.Location = New System.Drawing.Point(87, 111)
        Me.lstAliases.Name = "lstAliases"
        Me.lstAliases.Size = New System.Drawing.Size(613, 214)
        Me.lstAliases.TabIndex = 4
        '
        'lblAliases
        '
        Me.lblAliases.AutoSize = True
        Me.lblAliases.Location = New System.Drawing.Point(42, 111)
        Me.lblAliases.Name = "lblAliases"
        Me.lblAliases.Size = New System.Drawing.Size(40, 13)
        Me.lblAliases.TabIndex = 5
        Me.lblAliases.Text = "Aliases"
        '
        'ConfigurationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(733, 419)
        Me.Controls.Add(Me.lblAliases)
        Me.Controls.Add(Me.lstAliases)
        Me.Controls.Add(Me.cmbDevice)
        Me.Controls.Add(Me.lblDevice)
        Me.Controls.Add(Me.lblDeviceType)
        Me.Controls.Add(Me.cmbDeviceType)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ConfigurationForm"
        Me.Text = "Configuration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbDeviceType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDeviceType As System.Windows.Forms.Label
    Friend WithEvents lblDevice As System.Windows.Forms.Label
    Friend WithEvents cmbDevice As System.Windows.Forms.ComboBox
    Friend WithEvents lstAliases As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblAliases As System.Windows.Forms.Label

End Class
