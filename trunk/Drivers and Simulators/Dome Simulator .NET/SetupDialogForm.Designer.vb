<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetupDialogForm
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
        Me.picASCOM = New System.Windows.Forms.PictureBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAltRate = New System.Windows.Forms.TextBox
        Me.txtOCDelay = New System.Windows.Forms.TextBox
        Me.txtMaxAlt = New System.Windows.Forms.TextBox
        Me.txtMinAlt = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtAzRate = New System.Windows.Forms.TextBox
        Me.txtStepSize = New System.Windows.Forms.TextBox
        Me.txtPark = New System.Windows.Forms.TextBox
        Me.txtHome = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.chkCanSetAzimuth = New System.Windows.Forms.CheckBox
        Me.chkCanSyncAzimuth = New System.Windows.Forms.CheckBox
        Me.chkCanSetAltitude = New System.Windows.Forms.CheckBox
        Me.chkCanSetPark = New System.Windows.Forms.CheckBox
        Me.chkCanPark = New System.Windows.Forms.CheckBox
        Me.chkCanSetShutter = New System.Windows.Forms.CheckBox
        Me.chkCanFindHome = New System.Windows.Forms.CheckBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.chkSlewingOpenClose = New System.Windows.Forms.CheckBox
        Me.chkNonFragileAtPark = New System.Windows.Forms.CheckBox
        Me.chkStartShutterError = New System.Windows.Forms.CheckBox
        Me.chkNonFragileAtHome = New System.Windows.Forms.CheckBox
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'picASCOM
        '
        Me.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picASCOM.ErrorImage = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.Image = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.InitialImage = Global.ASCOM.DomeSimulator.My.Resources.Resources.ASCOM
        Me.picASCOM.Location = New System.Drawing.Point(354, 12)
        Me.picASCOM.Name = "picASCOM"
        Me.picASCOM.Size = New System.Drawing.Size(48, 56)
        Me.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picASCOM.TabIndex = 3
        Me.picASCOM.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox1.ForeColor = System.Drawing.Color.White
        Me.GroupBox1.Location = New System.Drawing.Point(13, 82)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(192, 152)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Shutter Control"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txtAltRate, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txtOCDelay, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtMaxAlt, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.txtMinAlt, 1, 3)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(7, 20)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(179, 123)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 92)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 26)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Minimum" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Altitude (deg):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 26)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Maximum" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Altitude (deg):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 26)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Open/Close" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "time (sec):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Slew Rate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(deg/sec):"
        '
        'txtAltRate
        '
        Me.txtAltRate.Location = New System.Drawing.Point(92, 5)
        Me.txtAltRate.Name = "txtAltRate"
        Me.txtAltRate.Size = New System.Drawing.Size(67, 20)
        Me.txtAltRate.TabIndex = 1
        '
        'txtOCDelay
        '
        Me.txtOCDelay.Location = New System.Drawing.Point(92, 35)
        Me.txtOCDelay.Name = "txtOCDelay"
        Me.txtOCDelay.Size = New System.Drawing.Size(67, 20)
        Me.txtOCDelay.TabIndex = 2
        '
        'txtMaxAlt
        '
        Me.txtMaxAlt.Location = New System.Drawing.Point(92, 65)
        Me.txtMaxAlt.Name = "txtMaxAlt"
        Me.txtMaxAlt.Size = New System.Drawing.Size(67, 20)
        Me.txtMaxAlt.TabIndex = 3
        '
        'txtMinAlt
        '
        Me.txtMinAlt.Location = New System.Drawing.Point(92, 95)
        Me.txtMinAlt.Name = "txtMinAlt"
        Me.txtMinAlt.Size = New System.Drawing.Size(67, 20)
        Me.txtMinAlt.TabIndex = 4
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TableLayoutPanel2)
        Me.GroupBox2.ForeColor = System.Drawing.Color.White
        Me.GroupBox2.Location = New System.Drawing.Point(211, 82)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(191, 152)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Azimuth Control"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Label5, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label6, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label7, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label8, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtAzRate, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtStepSize, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.txtPark, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.txtHome, 1, 3)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(7, 20)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.Padding = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(177, 123)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 92)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 26)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Home Position" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(deg):"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(5, 62)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 26)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Park Position" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(deg):"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(5, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 26)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Step Size" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(deg):"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(5, 2)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 26)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Slew Rate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(deg/sec):"
        '
        'txtAzRate
        '
        Me.txtAzRate.Location = New System.Drawing.Point(91, 5)
        Me.txtAzRate.Name = "txtAzRate"
        Me.txtAzRate.Size = New System.Drawing.Size(67, 20)
        Me.txtAzRate.TabIndex = 1
        '
        'txtStepSize
        '
        Me.txtStepSize.Location = New System.Drawing.Point(91, 35)
        Me.txtStepSize.Name = "txtStepSize"
        Me.txtStepSize.Size = New System.Drawing.Size(67, 20)
        Me.txtStepSize.TabIndex = 2
        '
        'txtPark
        '
        Me.txtPark.Location = New System.Drawing.Point(91, 65)
        Me.txtPark.Name = "txtPark"
        Me.txtPark.Size = New System.Drawing.Size(67, 20)
        Me.txtPark.TabIndex = 3
        '
        'txtHome
        '
        Me.txtHome.Location = New System.Drawing.Point(91, 95)
        Me.txtHome.Name = "txtHome"
        Me.txtHome.Size = New System.Drawing.Size(67, 20)
        Me.txtHome.TabIndex = 4
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TableLayoutPanel3)
        Me.GroupBox3.ForeColor = System.Drawing.Color.White
        Me.GroupBox3.Location = New System.Drawing.Point(12, 240)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(389, 115)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Interface Capabilities"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanSetAzimuth, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanSyncAzimuth, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanSetAltitude, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanSetPark, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanPark, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanSetShutter, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCanFindHome, 0, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(7, 19)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TableLayoutPanel3.RowCount = 4
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(376, 90)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'chkCanSetAzimuth
        '
        Me.chkCanSetAzimuth.AutoSize = True
        Me.chkCanSetAzimuth.Location = New System.Drawing.Point(4, 70)
        Me.chkCanSetAzimuth.Name = "chkCanSetAzimuth"
        Me.chkCanSetAzimuth.Size = New System.Drawing.Size(89, 16)
        Me.chkCanSetAzimuth.TabIndex = 6
        Me.chkCanSetAzimuth.Text = "Slew Azimuth"
        Me.chkCanSetAzimuth.UseVisualStyleBackColor = True
        '
        'chkCanSyncAzimuth
        '
        Me.chkCanSyncAzimuth.AutoSize = True
        Me.chkCanSyncAzimuth.Location = New System.Drawing.Point(191, 48)
        Me.chkCanSyncAzimuth.Name = "chkCanSyncAzimuth"
        Me.chkCanSyncAzimuth.Size = New System.Drawing.Size(90, 16)
        Me.chkCanSyncAzimuth.TabIndex = 5
        Me.chkCanSyncAzimuth.Text = "Sync Azimuth"
        Me.chkCanSyncAzimuth.UseVisualStyleBackColor = True
        '
        'chkCanSetAltitude
        '
        Me.chkCanSetAltitude.AutoSize = True
        Me.chkCanSetAltitude.Location = New System.Drawing.Point(4, 48)
        Me.chkCanSetAltitude.Name = "chkCanSetAltitude"
        Me.chkCanSetAltitude.Size = New System.Drawing.Size(87, 16)
        Me.chkCanSetAltitude.TabIndex = 4
        Me.chkCanSetAltitude.Text = "Slew Altitude"
        Me.chkCanSetAltitude.UseVisualStyleBackColor = True
        '
        'chkCanSetPark
        '
        Me.chkCanSetPark.AutoSize = True
        Me.chkCanSetPark.Location = New System.Drawing.Point(191, 26)
        Me.chkCanSetPark.Name = "chkCanSetPark"
        Me.chkCanSetPark.Size = New System.Drawing.Size(67, 16)
        Me.chkCanSetPark.TabIndex = 3
        Me.chkCanSetPark.Text = "Set Park"
        Me.chkCanSetPark.UseVisualStyleBackColor = True
        '
        'chkCanPark
        '
        Me.chkCanPark.AutoSize = True
        Me.chkCanPark.Location = New System.Drawing.Point(4, 26)
        Me.chkCanPark.Name = "chkCanPark"
        Me.chkCanPark.Size = New System.Drawing.Size(48, 16)
        Me.chkCanPark.TabIndex = 2
        Me.chkCanPark.Text = "Park"
        Me.chkCanPark.UseVisualStyleBackColor = True
        '
        'chkCanSetShutter
        '
        Me.chkCanSetShutter.AutoSize = True
        Me.chkCanSetShutter.Location = New System.Drawing.Point(191, 4)
        Me.chkCanSetShutter.Name = "chkCanSetShutter"
        Me.chkCanSetShutter.Size = New System.Drawing.Size(120, 16)
        Me.chkCanSetShutter.TabIndex = 1
        Me.chkCanSetShutter.Text = "Open/Close Shutter"
        Me.chkCanSetShutter.UseVisualStyleBackColor = True
        '
        'chkCanFindHome
        '
        Me.chkCanFindHome.AutoSize = True
        Me.chkCanFindHome.Location = New System.Drawing.Point(4, 4)
        Me.chkCanFindHome.Name = "chkCanFindHome"
        Me.chkCanFindHome.Size = New System.Drawing.Size(77, 16)
        Me.chkCanFindHome.TabIndex = 0
        Me.chkCanFindHome.Text = "Find Home"
        Me.chkCanFindHome.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdOK.Location = New System.Drawing.Point(246, 441)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdCancel.Location = New System.Drawing.Point(327, 441)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TableLayoutPanel4)
        Me.GroupBox4.ForeColor = System.Drawing.Color.White
        Me.GroupBox4.Location = New System.Drawing.Point(13, 361)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(389, 74)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Nonstandard Behavior"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.chkSlewingOpenClose, 1, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.chkNonFragileAtPark, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.chkStartShutterError, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.chkNonFragileAtHome, 0, 0)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(7, 19)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(376, 49)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'chkSlewingOpenClose
        '
        Me.chkSlewingOpenClose.AutoSize = True
        Me.chkSlewingOpenClose.Location = New System.Drawing.Point(191, 26)
        Me.chkSlewingOpenClose.Name = "chkSlewingOpenClose"
        Me.chkSlewingOpenClose.Size = New System.Drawing.Size(135, 17)
        Me.chkSlewingOpenClose.TabIndex = 3
        Me.chkSlewingOpenClose.Text = "Slewing on open/close"
        Me.chkSlewingOpenClose.UseVisualStyleBackColor = True
        '
        'chkNonFragileAtPark
        '
        Me.chkNonFragileAtPark.AutoSize = True
        Me.chkNonFragileAtPark.Location = New System.Drawing.Point(4, 26)
        Me.chkNonFragileAtPark.Name = "chkNonFragileAtPark"
        Me.chkNonFragileAtPark.Size = New System.Drawing.Size(126, 17)
        Me.chkNonFragileAtPark.TabIndex = 2
        Me.chkNonFragileAtPark.Text = "AtPark without Park()"
        Me.chkNonFragileAtPark.UseVisualStyleBackColor = True
        '
        'chkStartShutterError
        '
        Me.chkStartShutterError.AutoSize = True
        Me.chkStartShutterError.Location = New System.Drawing.Point(191, 4)
        Me.chkStartShutterError.Name = "chkStartShutterError"
        Me.chkStartShutterError.Size = New System.Drawing.Size(144, 16)
        Me.chkStartShutterError.TabIndex = 1
        Me.chkStartShutterError.Text = "Start up with shutter error"
        Me.chkStartShutterError.UseVisualStyleBackColor = True
        '
        'chkNonFragileAtHome
        '
        Me.chkNonFragileAtHome.AutoSize = True
        Me.chkNonFragileAtHome.Location = New System.Drawing.Point(4, 4)
        Me.chkNonFragileAtHome.Name = "chkNonFragileAtHome"
        Me.chkNonFragileAtHome.Size = New System.Drawing.Size(152, 16)
        Me.chkNonFragileAtHome.TabIndex = 0
        Me.chkNonFragileAtHome.Text = "AtHome without FindHome"
        Me.chkNonFragileAtHome.UseVisualStyleBackColor = True
        '
        'SetupDialogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(413, 474)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.picASCOM)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetupDialogForm"
        Me.Text = "Dome Simulator Setup"
        CType(Me.picASCOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picASCOM As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAltRate As System.Windows.Forms.TextBox
    Friend WithEvents txtOCDelay As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxAlt As System.Windows.Forms.TextBox
    Friend WithEvents txtMinAlt As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtAzRate As System.Windows.Forms.TextBox
    Friend WithEvents txtStepSize As System.Windows.Forms.TextBox
    Friend WithEvents txtPark As System.Windows.Forms.TextBox
    Friend WithEvents txtHome As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chkCanFindHome As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanSetAzimuth As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanSyncAzimuth As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanSetAltitude As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanSetPark As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanPark As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanSetShutter As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chkSlewingOpenClose As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonFragileAtPark As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartShutterError As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonFragileAtHome As System.Windows.Forms.CheckBox
End Class
