<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HandboxForm
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
        Me.txtDomeAz = New System.Windows.Forms.Label
        Me.LabelShutter = New System.Windows.Forms.Label
        Me.LabelAzimuth = New System.Windows.Forms.Label
        Me.txtShutter = New System.Windows.Forms.Label
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lblPARK = New System.Windows.Forms.Label
        Me.lblHOME = New System.Windows.Forms.Label
        Me.lblSlew = New System.Windows.Forms.Label
        Me.ButtonGoto = New System.Windows.Forms.Button
        Me.ButtonSync = New System.Windows.Forms.Button
        Me.txtNewAz = New System.Windows.Forms.TextBox
        Me.ButtonOpen = New System.Windows.Forms.Button
        Me.ButtonClose = New System.Windows.Forms.Button
        Me.ButtonSlewAltitudeUp = New System.Windows.Forms.Button
        Me.ButtonSlewStop = New System.Windows.Forms.Button
        Me.ButtonSlewAltitudeDown = New System.Windows.Forms.Button
        Me.ButtonClockwise = New System.Windows.Forms.Button
        Me.ButtonCounterClockwise = New System.Windows.Forms.Button
        Me.ButtonStepClockwise = New System.Windows.Forms.Button
        Me.ButtonStepCounterClockwise = New System.Windows.Forms.Button
        Me.ButtonPark = New System.Windows.Forms.Button
        Me.ButtonHome = New System.Windows.Forms.Button
        Me.ButtonSetup = New System.Windows.Forms.Button
        Me.ButtonTraffic = New System.Windows.Forms.Button
        Me.picASCOM = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.txtDomeAz, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelShutter, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelAzimuth, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtShutter, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 83)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(160, 40)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'txtDomeAz
        '
        Me.txtDomeAz.AutoSize = True
        Me.txtDomeAz.Dock = System.Windows.Forms.DockStyle.Right
        Me.txtDomeAz.ForeColor = System.Drawing.Color.Red
        Me.txtDomeAz.Location = New System.Drawing.Point(134, 20)
        Me.txtDomeAz.Name = "txtDomeAz"
        Me.txtDomeAz.Size = New System.Drawing.Size(22, 19)
        Me.txtDomeAz.TabIndex = 3
        Me.txtDomeAz.Text = "---.-"
        '
        'LabelShutter
        '
        Me.LabelShutter.AutoSize = True
        Me.LabelShutter.ForeColor = System.Drawing.Color.White
        Me.LabelShutter.Location = New System.Drawing.Point(4, 1)
        Me.LabelShutter.Name = "LabelShutter"
        Me.LabelShutter.Size = New System.Drawing.Size(44, 13)
        Me.LabelShutter.TabIndex = 0
        Me.LabelShutter.Text = "Shutter:"
        '
        'LabelAzimuth
        '
        Me.LabelAzimuth.AutoSize = True
        Me.LabelAzimuth.ForeColor = System.Drawing.Color.White
        Me.LabelAzimuth.Location = New System.Drawing.Point(4, 20)
        Me.LabelAzimuth.Name = "LabelAzimuth"
        Me.LabelAzimuth.Size = New System.Drawing.Size(53, 13)
        Me.LabelAzimuth.TabIndex = 1
        Me.LabelAzimuth.Text = "Dome Az:"
        '
        'txtShutter
        '
        Me.txtShutter.AutoSize = True
        Me.txtShutter.Dock = System.Windows.Forms.DockStyle.Right
        Me.txtShutter.ForeColor = System.Drawing.Color.Red
        Me.txtShutter.Location = New System.Drawing.Point(137, 1)
        Me.txtShutter.Name = "txtShutter"
        Me.txtShutter.Size = New System.Drawing.Size(19, 19)
        Me.txtShutter.TabIndex = 2
        Me.txtShutter.Text = "----"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.Controls.Add(Me.lblPARK, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblHOME, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblSlew, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 129)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(160, 20)
        Me.TableLayoutPanel2.TabIndex = 4
        '
        'lblPARK
        '
        Me.lblPARK.AutoSize = True
        Me.lblPARK.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblPARK.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblPARK.Location = New System.Drawing.Point(109, 0)
        Me.lblPARK.Name = "lblPARK"
        Me.lblPARK.Padding = New System.Windows.Forms.Padding(1, 2, 1, 1)
        Me.lblPARK.Size = New System.Drawing.Size(38, 16)
        Me.lblPARK.TabIndex = 2
        Me.lblPARK.Text = "PARK"
        '
        'lblHOME
        '
        Me.lblHOME.AutoSize = True
        Me.lblHOME.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblHOME.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblHOME.Location = New System.Drawing.Point(56, 0)
        Me.lblHOME.Name = "lblHOME"
        Me.lblHOME.Padding = New System.Windows.Forms.Padding(1, 2, 1, 1)
        Me.lblHOME.Size = New System.Drawing.Size(41, 16)
        Me.lblHOME.TabIndex = 1
        Me.lblHOME.Text = "HOME"
        '
        'lblSlew
        '
        Me.lblSlew.AutoSize = True
        Me.lblSlew.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblSlew.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblSlew.Location = New System.Drawing.Point(3, 0)
        Me.lblSlew.Name = "lblSlew"
        Me.lblSlew.Padding = New System.Windows.Forms.Padding(1, 2, 1, 1)
        Me.lblSlew.Size = New System.Drawing.Size(40, 16)
        Me.lblSlew.TabIndex = 0
        Me.lblSlew.Text = "SLEW"
        '
        'ButtonGoto
        '
        Me.ButtonGoto.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonGoto.Location = New System.Drawing.Point(4, 155)
        Me.ButtonGoto.Name = "ButtonGoto"
        Me.ButtonGoto.Size = New System.Drawing.Size(57, 29)
        Me.ButtonGoto.TabIndex = 5
        Me.ButtonGoto.Text = "Goto:"
        Me.ButtonGoto.UseVisualStyleBackColor = False
        '
        'ButtonSync
        '
        Me.ButtonSync.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonSync.Location = New System.Drawing.Point(4, 190)
        Me.ButtonSync.Name = "ButtonSync"
        Me.ButtonSync.Size = New System.Drawing.Size(57, 29)
        Me.ButtonSync.TabIndex = 6
        Me.ButtonSync.Text = "Sync:"
        Me.ButtonSync.UseVisualStyleBackColor = False
        '
        'txtNewAz
        '
        Me.txtNewAz.Location = New System.Drawing.Point(94, 180)
        Me.txtNewAz.Name = "txtNewAz"
        Me.txtNewAz.Size = New System.Drawing.Size(70, 20)
        Me.txtNewAz.TabIndex = 7
        '
        'ButtonOpen
        '
        Me.ButtonOpen.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonOpen.Location = New System.Drawing.Point(4, 239)
        Me.ButtonOpen.Name = "ButtonOpen"
        Me.ButtonOpen.Size = New System.Drawing.Size(57, 23)
        Me.ButtonOpen.TabIndex = 8
        Me.ButtonOpen.Text = "Open"
        Me.ButtonOpen.UseVisualStyleBackColor = False
        '
        'ButtonClose
        '
        Me.ButtonClose.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonClose.Location = New System.Drawing.Point(107, 239)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(57, 23)
        Me.ButtonClose.TabIndex = 9
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = False
        '
        'ButtonSlewAltitudeUp
        '
        Me.ButtonSlewAltitudeUp.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonSlewAltitudeUp.Font = New System.Drawing.Font("Wingdings 3", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ButtonSlewAltitudeUp.Location = New System.Drawing.Point(67, 240)
        Me.ButtonSlewAltitudeUp.Name = "ButtonSlewAltitudeUp"
        Me.ButtonSlewAltitudeUp.Size = New System.Drawing.Size(34, 46)
        Me.ButtonSlewAltitudeUp.TabIndex = 10
        Me.ButtonSlewAltitudeUp.Text = "£"
        Me.ButtonSlewAltitudeUp.UseVisualStyleBackColor = False
        '
        'ButtonSlewStop
        '
        Me.ButtonSlewStop.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonSlewStop.Font = New System.Drawing.Font("Wingdings 2", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ButtonSlewStop.Location = New System.Drawing.Point(67, 292)
        Me.ButtonSlewStop.Name = "ButtonSlewStop"
        Me.ButtonSlewStop.Size = New System.Drawing.Size(34, 46)
        Me.ButtonSlewStop.TabIndex = 11
        Me.ButtonSlewStop.Text = "Ä"
        Me.ButtonSlewStop.UseVisualStyleBackColor = False
        '
        'ButtonSlewAltitudeDown
        '
        Me.ButtonSlewAltitudeDown.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonSlewAltitudeDown.Font = New System.Drawing.Font("Wingdings 3", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ButtonSlewAltitudeDown.Location = New System.Drawing.Point(67, 344)
        Me.ButtonSlewAltitudeDown.Name = "ButtonSlewAltitudeDown"
        Me.ButtonSlewAltitudeDown.Size = New System.Drawing.Size(34, 46)
        Me.ButtonSlewAltitudeDown.TabIndex = 12
        Me.ButtonSlewAltitudeDown.Text = "¤"
        Me.ButtonSlewAltitudeDown.UseVisualStyleBackColor = False
        '
        'ButtonClockwise
        '
        Me.ButtonClockwise.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonClockwise.Font = New System.Drawing.Font("Wingdings 3", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ButtonClockwise.Location = New System.Drawing.Point(4, 292)
        Me.ButtonClockwise.Name = "ButtonClockwise"
        Me.ButtonClockwise.Size = New System.Drawing.Size(57, 46)
        Me.ButtonClockwise.TabIndex = 13
        Me.ButtonClockwise.Text = "P"
        Me.ButtonClockwise.UseVisualStyleBackColor = False
        '
        'ButtonCounterClockwise
        '
        Me.ButtonCounterClockwise.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonCounterClockwise.Font = New System.Drawing.Font("Wingdings 3", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ButtonCounterClockwise.Location = New System.Drawing.Point(107, 292)
        Me.ButtonCounterClockwise.Name = "ButtonCounterClockwise"
        Me.ButtonCounterClockwise.Size = New System.Drawing.Size(57, 46)
        Me.ButtonCounterClockwise.TabIndex = 14
        Me.ButtonCounterClockwise.Text = "Q"
        Me.ButtonCounterClockwise.UseVisualStyleBackColor = False
        '
        'ButtonStepClockwise
        '
        Me.ButtonStepClockwise.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonStepClockwise.Location = New System.Drawing.Point(4, 344)
        Me.ButtonStepClockwise.Name = "ButtonStepClockwise"
        Me.ButtonStepClockwise.Size = New System.Drawing.Size(57, 23)
        Me.ButtonStepClockwise.TabIndex = 15
        Me.ButtonStepClockwise.Text = "Step"
        Me.ButtonStepClockwise.UseVisualStyleBackColor = False
        '
        'ButtonStepCounterClockwise
        '
        Me.ButtonStepCounterClockwise.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonStepCounterClockwise.Location = New System.Drawing.Point(107, 344)
        Me.ButtonStepCounterClockwise.Name = "ButtonStepCounterClockwise"
        Me.ButtonStepCounterClockwise.Size = New System.Drawing.Size(57, 23)
        Me.ButtonStepCounterClockwise.TabIndex = 16
        Me.ButtonStepCounterClockwise.Text = "Step"
        Me.ButtonStepCounterClockwise.UseVisualStyleBackColor = False
        '
        'ButtonPark
        '
        Me.ButtonPark.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonPark.Location = New System.Drawing.Point(4, 400)
        Me.ButtonPark.Name = "ButtonPark"
        Me.ButtonPark.Size = New System.Drawing.Size(57, 29)
        Me.ButtonPark.TabIndex = 17
        Me.ButtonPark.Text = "Park"
        Me.ButtonPark.UseVisualStyleBackColor = False
        '
        'ButtonHome
        '
        Me.ButtonHome.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonHome.Location = New System.Drawing.Point(106, 400)
        Me.ButtonHome.Name = "ButtonHome"
        Me.ButtonHome.Size = New System.Drawing.Size(57, 29)
        Me.ButtonHome.TabIndex = 18
        Me.ButtonHome.Text = "Home"
        Me.ButtonHome.UseVisualStyleBackColor = False
        '
        'ButtonSetup
        '
        Me.ButtonSetup.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonSetup.Location = New System.Drawing.Point(4, 435)
        Me.ButtonSetup.Name = "ButtonSetup"
        Me.ButtonSetup.Size = New System.Drawing.Size(57, 23)
        Me.ButtonSetup.TabIndex = 19
        Me.ButtonSetup.Text = "Setup"
        Me.ButtonSetup.UseVisualStyleBackColor = False
        '
        'ButtonTraffic
        '
        Me.ButtonTraffic.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonTraffic.Location = New System.Drawing.Point(106, 435)
        Me.ButtonTraffic.Name = "ButtonTraffic"
        Me.ButtonTraffic.Size = New System.Drawing.Size(57, 23)
        Me.ButtonTraffic.TabIndex = 20
        Me.ButtonTraffic.Text = "Traffic"
        Me.ButtonTraffic.UseVisualStyleBackColor = False
        '
        'picASCOM
        '
        Me.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picASCOM.ErrorImage = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.Image = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.InitialImage = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.Location = New System.Drawing.Point(106, 12)
        Me.picASCOM.Name = "picASCOM"
        Me.picASCOM.Size = New System.Drawing.Size(48, 56)
        Me.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picASCOM.TabIndex = 2
        Me.picASCOM.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.ErrorImage = Nothing
        Me.PictureBox1.Image = Global.ASCOM.DomeSimulator.My.Resources.Resources.saturnc
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(10, 21)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(78, 37)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 21
        Me.PictureBox1.TabStop = False
        '
        'HandboxForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(167, 466)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ButtonTraffic)
        Me.Controls.Add(Me.ButtonSetup)
        Me.Controls.Add(Me.ButtonHome)
        Me.Controls.Add(Me.ButtonPark)
        Me.Controls.Add(Me.ButtonStepCounterClockwise)
        Me.Controls.Add(Me.ButtonStepClockwise)
        Me.Controls.Add(Me.ButtonCounterClockwise)
        Me.Controls.Add(Me.ButtonClockwise)
        Me.Controls.Add(Me.ButtonSlewAltitudeDown)
        Me.Controls.Add(Me.ButtonSlewStop)
        Me.Controls.Add(Me.ButtonSlewAltitudeUp)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ButtonOpen)
        Me.Controls.Add(Me.txtNewAz)
        Me.Controls.Add(Me.ButtonSync)
        Me.Controls.Add(Me.ButtonGoto)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.picASCOM)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "HandboxForm"
        Me.Text = "Dome Simulator"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picASCOM As System.Windows.Forms.PictureBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LabelShutter As System.Windows.Forms.Label
    Friend WithEvents LabelAzimuth As System.Windows.Forms.Label
    Friend WithEvents txtShutter As System.Windows.Forms.Label
    Friend WithEvents txtDomeAz As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblSlew As System.Windows.Forms.Label
    Friend WithEvents lblPARK As System.Windows.Forms.Label
    Friend WithEvents lblHOME As System.Windows.Forms.Label
    Friend WithEvents ButtonGoto As System.Windows.Forms.Button
    Friend WithEvents ButtonSync As System.Windows.Forms.Button
    Friend WithEvents txtNewAz As System.Windows.Forms.TextBox
    Friend WithEvents ButtonOpen As System.Windows.Forms.Button
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents ButtonSlewAltitudeUp As System.Windows.Forms.Button
    Friend WithEvents ButtonSlewStop As System.Windows.Forms.Button
    Friend WithEvents ButtonSlewAltitudeDown As System.Windows.Forms.Button
    Friend WithEvents ButtonClockwise As System.Windows.Forms.Button
    Friend WithEvents ButtonCounterClockwise As System.Windows.Forms.Button
    Friend WithEvents ButtonStepClockwise As System.Windows.Forms.Button
    Friend WithEvents ButtonStepCounterClockwise As System.Windows.Forms.Button
    Friend WithEvents ButtonPark As System.Windows.Forms.Button
    Friend WithEvents ButtonHome As System.Windows.Forms.Button
    Friend WithEvents ButtonSetup As System.Windows.Forms.Button
    Friend WithEvents ButtonTraffic As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
