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
        Me.txtTraffic = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ButtonDisable = New System.Windows.Forms.Button
        Me.ButtonClear = New System.Windows.Forms.Button
        Me.picASCOM = New System.Windows.Forms.PictureBox
        Me.chkSlew = New System.Windows.Forms.CheckBox
        Me.chkSlewing = New System.Windows.Forms.CheckBox
        Me.chkShutter = New System.Windows.Forms.CheckBox
        Me.chkConnected = New System.Windows.Forms.CheckBox
        Me.chkOther = New System.Windows.Forms.CheckBox
        Me.chkHW = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTraffic
        '
        Me.txtTraffic.BackColor = System.Drawing.SystemColors.Info
        Me.txtTraffic.CausesValidation = False
        Me.txtTraffic.Location = New System.Drawing.Point(10, 151)
        Me.txtTraffic.Multiline = True
        Me.txtTraffic.Name = "txtTraffic"
        Me.txtTraffic.ReadOnly = True
        Me.txtTraffic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTraffic.Size = New System.Drawing.Size(347, 313)
        Me.txtTraffic.TabIndex = 3
        Me.txtTraffic.Text = "Hello"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonDisable, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonClear, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(227, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(76, 56)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'ButtonDisable
        '
        Me.ButtonDisable.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonDisable.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonDisable.ForeColor = System.Drawing.Color.Black
        Me.ButtonDisable.Location = New System.Drawing.Point(4, 3)
        Me.ButtonDisable.Name = "ButtonDisable"
        Me.ButtonDisable.Size = New System.Drawing.Size(67, 22)
        Me.ButtonDisable.TabIndex = 0
        Me.ButtonDisable.Text = "Disable"
        Me.ButtonDisable.UseVisualStyleBackColor = False
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonClear.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonClear.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClear.ForeColor = System.Drawing.Color.Black
        Me.ButtonClear.Location = New System.Drawing.Point(4, 31)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(67, 22)
        Me.ButtonClear.TabIndex = 1
        Me.ButtonClear.Text = "Clear"
        Me.ButtonClear.UseVisualStyleBackColor = False
        '
        'picASCOM
        '
        Me.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picASCOM.ErrorImage = Nothing
        Me.picASCOM.Image = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.InitialImage = Nothing
        Me.picASCOM.Location = New System.Drawing.Point(309, 12)
        Me.picASCOM.Name = "picASCOM"
        Me.picASCOM.Size = New System.Drawing.Size(48, 56)
        Me.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picASCOM.TabIndex = 5
        Me.picASCOM.TabStop = False
        '
        'chkSlew
        '
        Me.chkSlew.AutoSize = True
        Me.chkSlew.ForeColor = System.Drawing.Color.White
        Me.chkSlew.Location = New System.Drawing.Point(13, 12)
        Me.chkSlew.Name = "chkSlew"
        Me.chkSlew.Size = New System.Drawing.Size(164, 17)
        Me.chkSlew.TabIndex = 6
        Me.chkSlew.Text = "Slew, Sync, Park, Find Home"
        Me.chkSlew.UseVisualStyleBackColor = True
        '
        'chkSlewing
        '
        Me.chkSlewing.AutoSize = True
        Me.chkSlewing.ForeColor = System.Drawing.Color.White
        Me.chkSlewing.Location = New System.Drawing.Point(13, 35)
        Me.chkSlewing.Name = "chkSlewing"
        Me.chkSlewing.Size = New System.Drawing.Size(74, 17)
        Me.chkSlewing.TabIndex = 7
        Me.chkSlewing.Text = "Is Slewing"
        Me.chkSlewing.UseVisualStyleBackColor = True
        '
        'chkShutter
        '
        Me.chkShutter.AutoSize = True
        Me.chkShutter.ForeColor = System.Drawing.Color.White
        Me.chkShutter.Location = New System.Drawing.Point(13, 58)
        Me.chkShutter.Name = "chkShutter"
        Me.chkShutter.Size = New System.Drawing.Size(99, 17)
        Me.chkShutter.TabIndex = 8
        Me.chkShutter.Text = "All Shutter Calls"
        Me.chkShutter.UseVisualStyleBackColor = True
        '
        'chkConnected
        '
        Me.chkConnected.AutoSize = True
        Me.chkConnected.ForeColor = System.Drawing.Color.White
        Me.chkConnected.Location = New System.Drawing.Point(13, 81)
        Me.chkConnected.Name = "chkConnected"
        Me.chkConnected.Size = New System.Drawing.Size(78, 17)
        Me.chkConnected.TabIndex = 9
        Me.chkConnected.Text = "Connected"
        Me.chkConnected.UseVisualStyleBackColor = True
        '
        'chkOther
        '
        Me.chkOther.AutoSize = True
        Me.chkOther.ForeColor = System.Drawing.Color.White
        Me.chkOther.Location = New System.Drawing.Point(13, 104)
        Me.chkOther.Name = "chkOther"
        Me.chkOther.Size = New System.Drawing.Size(118, 17)
        Me.chkOther.TabIndex = 10
        Me.chkOther.Text = "Other ASCOM Calls"
        Me.chkOther.UseVisualStyleBackColor = True
        '
        'chkHW
        '
        Me.chkHW.AutoSize = True
        Me.chkHW.ForeColor = System.Drawing.Color.White
        Me.chkHW.Location = New System.Drawing.Point(13, 128)
        Me.chkHW.Name = "chkHW"
        Me.chkHW.Size = New System.Drawing.Size(118, 17)
        Me.chkHW.TabIndex = 11
        Me.chkHW.Text = "Monitor ""HW"" Calls"
        Me.chkHW.UseVisualStyleBackColor = True
        '
        'ShowTrafficForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(369, 476)
        Me.Controls.Add(Me.chkHW)
        Me.Controls.Add(Me.chkOther)
        Me.Controls.Add(Me.chkConnected)
        Me.Controls.Add(Me.chkShutter)
        Me.Controls.Add(Me.chkSlewing)
        Me.Controls.Add(Me.chkSlew)
        Me.Controls.Add(Me.picASCOM)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.txtTraffic)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShowTrafficForm"
        Me.Text = "Show ASCOM Traffic"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTraffic As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonDisable As System.Windows.Forms.Button
    Friend WithEvents ButtonClear As System.Windows.Forms.Button
    Friend WithEvents picASCOM As System.Windows.Forms.PictureBox
    Friend WithEvents chkSlew As System.Windows.Forms.CheckBox
    Friend WithEvents chkSlewing As System.Windows.Forms.CheckBox
    Friend WithEvents chkShutter As System.Windows.Forms.CheckBox
    Friend WithEvents chkConnected As System.Windows.Forms.CheckBox
    Friend WithEvents chkOther As System.Windows.Forms.CheckBox
    Friend WithEvents chkHW As System.Windows.Forms.CheckBox
End Class
