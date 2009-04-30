<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProfileExplorer
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProfileExplorer))
        Me.KeyTree = New System.Windows.Forms.TreeView
        Me.KeyValues = New System.Windows.Forms.DataGridView
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Data = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.mnuCtx = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuNewKey = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRename = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRootEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem
        CType(Me.KeyValues, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuCtx.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'KeyTree
        '
        Me.KeyTree.Location = New System.Drawing.Point(13, 27)
        Me.KeyTree.Name = "KeyTree"
        Me.KeyTree.Size = New System.Drawing.Size(299, 501)
        Me.KeyTree.TabIndex = 0
        '
        'KeyValues
        '
        Me.KeyValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.KeyValues.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Value, Me.Data})
        Me.KeyValues.Location = New System.Drawing.Point(328, 27)
        Me.KeyValues.Name = "KeyValues"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.KeyValues.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyValues.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.KeyValues.RowTemplate.Height = 17
        Me.KeyValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.KeyValues.Size = New System.Drawing.Size(628, 501)
        Me.KeyValues.TabIndex = 1
        '
        'Value
        '
        Me.Value.HeaderText = "Value"
        Me.Value.Name = "Value"
        '
        'Data
        '
        Me.Data.HeaderText = "Data"
        Me.Data.Name = "Data"
        '
        'mnuCtx
        '
        Me.mnuCtx.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewKey, Me.ToolStripSeparator1, Me.mnuDelete, Me.mnuRename})
        Me.mnuCtx.Name = "mnuCtx"
        Me.mnuCtx.Size = New System.Drawing.Size(165, 76)
        '
        'mnuNewKey
        '
        Me.mnuNewKey.Name = "mnuNewKey"
        Me.mnuNewKey.ShortcutKeyDisplayString = "Ins"
        Me.mnuNewKey.Size = New System.Drawing.Size(164, 22)
        Me.mnuNewKey.Text = "New Key"
        Me.mnuNewKey.ToolTipText = "Create a new key"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(161, 6)
        '
        'mnuDelete
        '
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.ShortcutKeyDisplayString = "Del"
        Me.mnuDelete.Size = New System.Drawing.Size(164, 22)
        Me.mnuDelete.Text = "Delete Key"
        Me.mnuDelete.ToolTipText = "Delete a key"
        '
        'mnuRename
        '
        Me.mnuRename.Name = "mnuRename"
        Me.mnuRename.ShortcutKeyDisplayString = "F2"
        Me.mnuRename.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.mnuRename.Size = New System.Drawing.Size(164, 22)
        Me.mnuRename.Text = "Rename Key"
        Me.mnuRename.ToolTipText = "Rename a key"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuOptions, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(968, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(103, 22)
        Me.mnuExit.Text = "Exit"
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRootEdit})
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(56, 20)
        Me.mnuOptions.Text = "Options"
        '
        'mnuRootEdit
        '
        Me.mnuRootEdit.Name = "mnuRootEdit"
        Me.mnuRootEdit.Size = New System.Drawing.Size(164, 22)
        Me.mnuRootEdit.Text = "Enable Root Edit"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(114, 22)
        Me.mnuAbout.Text = "About"
        '
        'frmProfileExplorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 651)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.KeyValues)
        Me.Controls.Add(Me.KeyTree)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmProfileExplorer"
        Me.Text = "Profile Explorer"
        CType(Me.KeyValues, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuCtx.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KeyTree As System.Windows.Forms.TreeView
    Friend WithEvents KeyValues As System.Windows.Forms.DataGridView
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Data As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents mnuCtx As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuNewKey As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRename As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRootEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem

End Class
