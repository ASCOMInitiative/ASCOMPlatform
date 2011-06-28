<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SerialForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SerialForm))
        Me.lstSerialASCOM = New System.Windows.Forms.ListBox
        Me.btnSerialExit = New System.Windows.Forms.Button
        Me.lblSerial = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lstSerialASCOM
        '
        Me.lstSerialASCOM.FormattingEnabled = True
        Me.lstSerialASCOM.Location = New System.Drawing.Point(15, 25)
        Me.lstSerialASCOM.Name = "lstSerialASCOM"
        Me.lstSerialASCOM.Size = New System.Drawing.Size(295, 264)
        Me.lstSerialASCOM.TabIndex = 0
        '
        'btnSerialExit
        '
        Me.btnSerialExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSerialExit.Location = New System.Drawing.Point(331, 266)
        Me.btnSerialExit.Name = "btnSerialExit"
        Me.btnSerialExit.Size = New System.Drawing.Size(75, 23)
        Me.btnSerialExit.TabIndex = 1
        Me.btnSerialExit.Text = "Exit"
        Me.btnSerialExit.UseVisualStyleBackColor = True
        '
        'lblSerial
        '
        Me.lblSerial.AutoSize = True
        Me.lblSerial.Location = New System.Drawing.Point(12, 9)
        Me.lblSerial.Name = "lblSerial"
        Me.lblSerial.Size = New System.Drawing.Size(143, 13)
        Me.lblSerial.TabIndex = 2
        Me.lblSerial.Text = "COM Ports visible to ASCOM"
        '
        'SerialForm
        '
        Me.AcceptButton = Me.btnSerialExit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSerialExit
        Me.ClientSize = New System.Drawing.Size(424, 306)
        Me.Controls.Add(Me.lblSerial)
        Me.Controls.Add(Me.btnSerialExit)
        Me.Controls.Add(Me.lstSerialASCOM)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SerialForm"
        Me.Text = "COM Ports"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstSerialASCOM As System.Windows.Forms.ListBox
    Friend WithEvents btnSerialExit As System.Windows.Forms.Button
    Friend WithEvents lblSerial As System.Windows.Forms.Label
End Class
