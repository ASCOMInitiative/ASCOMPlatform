﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConnectForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConnectForm))
        Me.cmbDeviceType = New System.Windows.Forms.ComboBox
        Me.btnChoose = New System.Windows.Forms.Button
        Me.txtDevice = New System.Windows.Forms.TextBox
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.btnConnect = New System.Windows.Forms.Button
        Me.btnProperties = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cmbDeviceType
        '
        Me.cmbDeviceType.FormattingEnabled = True
        Me.cmbDeviceType.Location = New System.Drawing.Point(12, 29)
        Me.cmbDeviceType.Name = "cmbDeviceType"
        Me.cmbDeviceType.Size = New System.Drawing.Size(246, 21)
        Me.cmbDeviceType.TabIndex = 0
        '
        'btnChoose
        '
        Me.btnChoose.Location = New System.Drawing.Point(275, 27)
        Me.btnChoose.Name = "btnChoose"
        Me.btnChoose.Size = New System.Drawing.Size(75, 23)
        Me.btnChoose.TabIndex = 1
        Me.btnChoose.Text = "Choose"
        Me.btnChoose.UseVisualStyleBackColor = True
        '
        'txtDevice
        '
        Me.txtDevice.Enabled = False
        Me.txtDevice.Location = New System.Drawing.Point(12, 74)
        Me.txtDevice.Name = "txtDevice"
        Me.txtDevice.Size = New System.Drawing.Size(246, 20)
        Me.txtDevice.TabIndex = 2
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(12, 122)
        Me.txtStatus.Multiline = True
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtStatus.Size = New System.Drawing.Size(770, 253)
        Me.txtStatus.TabIndex = 3
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(275, 72)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 4
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'btnProperties
        '
        Me.btnProperties.Location = New System.Drawing.Point(356, 72)
        Me.btnProperties.Name = "btnProperties"
        Me.btnProperties.Size = New System.Drawing.Size(75, 23)
        Me.btnProperties.TabIndex = 5
        Me.btnProperties.Text = "Properties"
        Me.btnProperties.UseVisualStyleBackColor = True
        '
        'ConnectForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 442)
        Me.Controls.Add(Me.btnProperties)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtDevice)
        Me.Controls.Add(Me.btnChoose)
        Me.Controls.Add(Me.cmbDeviceType)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ConnectForm"
        Me.Text = "Diagnostics Connect"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbDeviceType As System.Windows.Forms.ComboBox
    Friend WithEvents btnChoose As System.Windows.Forms.Button
    Friend WithEvents txtDevice As System.Windows.Forms.TextBox
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnProperties As System.Windows.Forms.Button
End Class
