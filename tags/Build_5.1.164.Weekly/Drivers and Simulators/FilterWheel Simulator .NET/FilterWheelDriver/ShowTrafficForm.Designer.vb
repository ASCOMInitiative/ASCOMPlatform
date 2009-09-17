<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowTrafficForm
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.btnDisable = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.picASCOM = New System.Windows.Forms.PictureBox
        Me.txtTraffic = New System.Windows.Forms.TextBox
        Me.chkPosition = New System.Windows.Forms.CheckBox
        Me.chkMoving = New System.Windows.Forms.CheckBox
        Me.chkName = New System.Windows.Forms.CheckBox
        Me.chkOther = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDisable, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClear, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(229, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(76, 56)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnDisable
        '
        Me.btnDisable.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnDisable.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnDisable.ForeColor = System.Drawing.Color.Black
        Me.btnDisable.Location = New System.Drawing.Point(4, 3)
        Me.btnDisable.Name = "btnDisable"
        Me.btnDisable.Size = New System.Drawing.Size(67, 22)
        Me.btnDisable.TabIndex = 0
        Me.btnDisable.Text = "Disable"
        Me.btnDisable.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnClear.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnClear.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClear.ForeColor = System.Drawing.Color.Black
        Me.btnClear.Location = New System.Drawing.Point(4, 31)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(67, 22)
        Me.btnClear.TabIndex = 1
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'picASCOM
        '
        Me.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picASCOM.Image = Global.ASCOM.FilterWheelSim.My.Resources.Resources.ASCOM
        Me.picASCOM.Location = New System.Drawing.Point(311, 12)
        Me.picASCOM.Name = "picASCOM"
        Me.picASCOM.Size = New System.Drawing.Size(48, 56)
        Me.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picASCOM.TabIndex = 1
        Me.picASCOM.TabStop = False
        '
        'txtTraffic
        '
        Me.txtTraffic.BackColor = System.Drawing.SystemColors.Info
        Me.txtTraffic.CausesValidation = False
        Me.txtTraffic.Location = New System.Drawing.Point(12, 74)
        Me.txtTraffic.Multiline = True
        Me.txtTraffic.Name = "txtTraffic"
        Me.txtTraffic.ReadOnly = True
        Me.txtTraffic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTraffic.Size = New System.Drawing.Size(347, 313)
        Me.txtTraffic.TabIndex = 2
        Me.txtTraffic.Text = "Hello"
        '
        'chkPosition
        '
        Me.chkPosition.AutoSize = True
        Me.chkPosition.Checked = True
        Me.chkPosition.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPosition.Location = New System.Drawing.Point(13, 13)
        Me.chkPosition.Name = "chkPosition"
        Me.chkPosition.Size = New System.Drawing.Size(63, 17)
        Me.chkPosition.TabIndex = 3
        Me.chkPosition.Text = "Position"
        Me.chkPosition.UseVisualStyleBackColor = True
        '
        'chkMoving
        '
        Me.chkMoving.AutoSize = True
        Me.chkMoving.Checked = True
        Me.chkMoving.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMoving.Location = New System.Drawing.Point(13, 37)
        Me.chkMoving.Name = "chkMoving"
        Me.chkMoving.Size = New System.Drawing.Size(61, 17)
        Me.chkMoving.TabIndex = 4
        Me.chkMoving.Text = "Moving"
        Me.chkMoving.UseVisualStyleBackColor = True
        '
        'chkName
        '
        Me.chkName.AutoSize = True
        Me.chkName.Checked = True
        Me.chkName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkName.Location = New System.Drawing.Point(110, 13)
        Me.chkName.Name = "chkName"
        Me.chkName.Size = New System.Drawing.Size(87, 17)
        Me.chkName.TabIndex = 5
        Me.chkName.Text = "Name/Offset"
        Me.chkName.UseVisualStyleBackColor = True
        '
        'chkOther
        '
        Me.chkOther.AutoSize = True
        Me.chkOther.Checked = True
        Me.chkOther.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOther.Location = New System.Drawing.Point(110, 37)
        Me.chkOther.Name = "chkOther"
        Me.chkOther.Size = New System.Drawing.Size(76, 17)
        Me.chkOther.TabIndex = 6
        Me.chkOther.Text = "Other calls"
        Me.chkOther.UseVisualStyleBackColor = True
        '
        'ShowTrafficForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(374, 398)
        Me.Controls.Add(Me.chkOther)
        Me.Controls.Add(Me.chkName)
        Me.Controls.Add(Me.chkMoving)
        Me.Controls.Add(Me.chkPosition)
        Me.Controls.Add(Me.txtTraffic)
        Me.Controls.Add(Me.picASCOM)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", Global.ASCOM.FilterWheelSim.My.MySettings.Default, "trafficFormLocation", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = Global.ASCOM.FilterWheelSim.My.MySettings.Default.trafficFormLocation
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShowTrafficForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ASCOM API Calls"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnDisable As System.Windows.Forms.Button
    Friend WithEvents picASCOM As System.Windows.Forms.PictureBox
    Friend WithEvents txtTraffic As System.Windows.Forms.TextBox
    Friend WithEvents chkMoving As System.Windows.Forms.CheckBox
    Friend WithEvents chkName As System.Windows.Forms.CheckBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents chkPosition As System.Windows.Forms.CheckBox
    Friend WithEvents chkOther As System.Windows.Forms.CheckBox

End Class
