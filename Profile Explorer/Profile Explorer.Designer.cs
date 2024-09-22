using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Profile explorer form class.
    /// </summary>
    public partial class frmProfileExplorer : Form
    {

        // Form overrides dispose to clean up the component list.
        /// <summary>
        /// Dispose of the form.
        /// </summary>
        /// <param name="disposing"></param>
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfileExplorer));
            this.KeyTree = new System.Windows.Forms.TreeView();
            this.KeyValues = new System.Windows.Forms.DataGridView();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mnuCtxKeys = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNewKey = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDeleteKey = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRenameKey = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRootEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCtxValues = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuEditData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRenameValue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuValueSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNewValue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteValue = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.KeyValues)).BeginInit();
            this.mnuCtxKeys.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.mnuCtxValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // KeyTree
            // 
            this.KeyTree.Location = new System.Drawing.Point(13, 27);
            this.KeyTree.Name = "KeyTree";
            this.KeyTree.Size = new System.Drawing.Size(299, 501);
            this.KeyTree.TabIndex = 0;
            this.KeyTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.KeyTree_AfterLabelEdit);
            this.KeyTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.KeyTree_NodeMouseClick);
            this.KeyTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyTree_KeyUp);
            this.KeyTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyTree_MouseUp);
            // 
            // KeyValues
            // 
            this.KeyValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KeyValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Value,
            this.Data});
            this.KeyValues.Location = new System.Drawing.Point(328, 27);
            this.KeyValues.Name = "KeyValues";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.KeyValues.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyValues.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.KeyValues.RowTemplate.Height = 17;
            this.KeyValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.KeyValues.Size = new System.Drawing.Size(628, 501);
            this.KeyValues.TabIndex = 1;
            this.KeyValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.KeyValues_CellValueChanged);
            this.KeyValues.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.KeyValues_ColumnWidthChanged);
            this.KeyValues.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.KeyValues_UserDeletingRow);
            this.KeyValues.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyValues_KeyUp);
            this.KeyValues.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyValues_MouseUp);
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // Data
            // 
            this.Data.HeaderText = "Data";
            this.Data.Name = "Data";
            // 
            // mnuCtxKeys
            // 
            this.mnuCtxKeys.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewKey,
            this.ToolStripSeparator1,
            this.mnuDeleteKey,
            this.mnuRenameKey});
            this.mnuCtxKeys.Name = "mnuCtx";
            this.mnuCtxKeys.Size = new System.Drawing.Size(159, 76);
            // 
            // mnuNewKey
            // 
            this.mnuNewKey.Name = "mnuNewKey";
            this.mnuNewKey.ShortcutKeyDisplayString = "Ins";
            this.mnuNewKey.Size = new System.Drawing.Size(158, 22);
            this.mnuNewKey.Text = "New Key";
            this.mnuNewKey.ToolTipText = "Create a new key";
            this.mnuNewKey.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuNewKey_MouseUp);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // mnuDeleteKey
            // 
            this.mnuDeleteKey.Name = "mnuDeleteKey";
            this.mnuDeleteKey.ShortcutKeyDisplayString = "Del";
            this.mnuDeleteKey.Size = new System.Drawing.Size(158, 22);
            this.mnuDeleteKey.Text = "Delete Key";
            this.mnuDeleteKey.ToolTipText = "Delete a key";
            this.mnuDeleteKey.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuDeleteKey_MouseUp);
            // 
            // mnuRenameKey
            // 
            this.mnuRenameKey.Name = "mnuRenameKey";
            this.mnuRenameKey.ShortcutKeyDisplayString = "F2";
            this.mnuRenameKey.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.mnuRenameKey.Size = new System.Drawing.Size(158, 22);
            this.mnuRenameKey.Text = "Rename Key";
            this.mnuRenameKey.ToolTipText = "Rename a key";
            this.mnuRenameKey.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuRenameKey_MouseUp);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuOptions,
            this.mnuHelp});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(968, 24);
            this.MenuStrip1.TabIndex = 2;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(93, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRootEdit});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(61, 20);
            this.mnuOptions.Text = "Options";
            // 
            // mnuRootEdit
            // 
            this.mnuRootEdit.Name = "mnuRootEdit";
            this.mnuRootEdit.Size = new System.Drawing.Size(160, 22);
            this.mnuRootEdit.Text = "Enable Root Edit";
            this.mnuRootEdit.Click += new System.EventHandler(this.mnuRootEdit_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuCtxValues
            // 
            this.mnuCtxValues.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditData,
            this.mnuClearData,
            this.mnuRenameValue,
            this.mnuValueSeparator,
            this.mnuNewValue,
            this.mnuDeleteValue});
            this.mnuCtxValues.Name = "mnuCtxValues";
            this.mnuCtxValues.Size = new System.Drawing.Size(149, 120);
            // 
            // mnuEditData
            // 
            this.mnuEditData.Name = "mnuEditData";
            this.mnuEditData.Size = new System.Drawing.Size(148, 22);
            this.mnuEditData.Text = "Edit Data";
            this.mnuEditData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuEditData_MouseUp);
            // 
            // mnuClearData
            // 
            this.mnuClearData.Name = "mnuClearData";
            this.mnuClearData.Size = new System.Drawing.Size(148, 22);
            this.mnuClearData.Text = "Clear Data";
            this.mnuClearData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuClearData_MouseUp);
            // 
            // mnuRenameValue
            // 
            this.mnuRenameValue.Name = "mnuRenameValue";
            this.mnuRenameValue.Size = new System.Drawing.Size(148, 22);
            this.mnuRenameValue.Text = "Rename Value";
            this.mnuRenameValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuRenameValue_MouseUp);
            // 
            // mnuValueSeparator
            // 
            this.mnuValueSeparator.Name = "mnuValueSeparator";
            this.mnuValueSeparator.Size = new System.Drawing.Size(145, 6);
            // 
            // mnuNewValue
            // 
            this.mnuNewValue.Name = "mnuNewValue";
            this.mnuNewValue.Size = new System.Drawing.Size(148, 22);
            this.mnuNewValue.Text = "New Value";
            this.mnuNewValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuNewValue_MouseUp);
            // 
            // mnuDeleteValue
            // 
            this.mnuDeleteValue.Name = "mnuDeleteValue";
            this.mnuDeleteValue.Size = new System.Drawing.Size(148, 22);
            this.mnuDeleteValue.Text = "Delete Value";
            this.mnuDeleteValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnuDeleteValue_MouseUp);
            // 
            // frmProfileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 651);
            this.Controls.Add(this.MenuStrip1);
            this.Controls.Add(this.KeyValues);
            this.Controls.Add(this.KeyTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip1;
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "frmProfileExplorer";
            this.Text = "Profile Explorer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.KeyValues)).EndInit();
            this.mnuCtxKeys.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.mnuCtxValues.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal TreeView KeyTree;
        internal DataGridView KeyValues;
        internal DataGridViewTextBoxColumn Value;
        internal DataGridViewTextBoxColumn Data;
        internal ContextMenuStrip mnuCtxKeys;
        internal ToolStripMenuItem mnuNewKey;
        internal ToolStripMenuItem mnuDeleteKey;
        internal ToolStripMenuItem mnuRenameKey;
        internal ToolStripSeparator ToolStripSeparator1;
        internal MenuStrip MenuStrip1;
        internal ToolStripMenuItem mnuFile;
        internal ToolStripMenuItem mnuExit;
        internal ToolStripMenuItem mnuOptions;
        internal ToolStripMenuItem mnuRootEdit;
        internal ToolStripMenuItem mnuHelp;
        internal ToolStripMenuItem mnuAbout;
        internal ContextMenuStrip mnuCtxValues;
        internal ToolStripMenuItem mnuNewValue;
        internal ToolStripMenuItem mnuEditData;
        internal ToolStripMenuItem mnuDeleteValue;
        internal ToolStripMenuItem mnuRenameValue;
        internal ToolStripSeparator mnuValueSeparator;
        internal ToolStripMenuItem mnuClearData;

    }
}