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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProfileExplorer))
        Me.KeyTree = New System.Windows.Forms.TreeView
        Me.KeyValues = New System.Windows.Forms.DataGridView
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Data = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.KeyValues, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'KeyTree
        '
        Me.KeyTree.Location = New System.Drawing.Point(13, 13)
        Me.KeyTree.Name = "KeyTree"
        Me.KeyTree.Size = New System.Drawing.Size(299, 515)
        Me.KeyTree.TabIndex = 0
        '
        'KeyValues
        '
        Me.KeyValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.KeyValues.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Value, Me.Data})
        Me.KeyValues.Location = New System.Drawing.Point(328, 13)
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
        Me.KeyValues.Size = New System.Drawing.Size(628, 515)
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
        'frmProfileExplorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 651)
        Me.Controls.Add(Me.KeyValues)
        Me.Controls.Add(Me.KeyTree)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProfileExplorer"
        Me.Text = "Profile Explorer"
        CType(Me.KeyValues, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KeyTree As System.Windows.Forms.TreeView
    Friend WithEvents KeyValues As System.Windows.Forms.DataGridView
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Data As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
