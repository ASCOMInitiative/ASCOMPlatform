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
            components = new System.ComponentModel.Container();
            var DataGridViewCellStyle1 = new DataGridViewCellStyle();
            var DataGridViewCellStyle2 = new DataGridViewCellStyle();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfileExplorer));
            KeyTree = new TreeView();
            KeyTree.KeyUp += new KeyEventHandler(KeyTree_KeyUp);
            KeyTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(KeyTree_NodeMouseClick);
            KeyTree.AfterLabelEdit += new NodeLabelEditEventHandler(KeyTree_AfterLabelEdit);
            KeyTree.MouseUp += new MouseEventHandler(KeyTree_MouseUp);
            KeyValues = new DataGridView();
            KeyValues.ColumnWidthChanged += new DataGridViewColumnEventHandler(KeyValues_ColumnWidthChanged);
            KeyValues.CellValueChanged += new DataGridViewCellEventHandler(KeyValues_CellValueChanged);
            KeyValues.UserDeletingRow += new DataGridViewRowCancelEventHandler(KeyValues_UserDeletingRow);
            KeyValues.KeyUp += new KeyEventHandler(KeyValues_KeyUp);
            KeyValues.MouseUp += new MouseEventHandler(KeyValues_MouseUp);
            Value = new DataGridViewTextBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            mnuCtxKeys = new ContextMenuStrip(components);
            mnuNewKey = new ToolStripMenuItem();
            mnuNewKey.MouseUp += new MouseEventHandler(mnuNewKey_MouseUp);
            ToolStripSeparator1 = new ToolStripSeparator();
            mnuDeleteKey = new ToolStripMenuItem();
            mnuDeleteKey.MouseUp += new MouseEventHandler(mnuDeleteKey_MouseUp);
            mnuRenameKey = new ToolStripMenuItem();
            mnuRenameKey.MouseUp += new MouseEventHandler(mnuRenameKey_MouseUp);
            MenuStrip1 = new MenuStrip();
            mnuFile = new ToolStripMenuItem();
            mnuExit = new ToolStripMenuItem();
            mnuExit.Click += new EventHandler(mnuExit_Click);
            mnuOptions = new ToolStripMenuItem();
            mnuRootEdit = new ToolStripMenuItem();
            mnuRootEdit.Click += new EventHandler(mnuRootEdit_Click);
            mnuHelp = new ToolStripMenuItem();
            mnuAbout = new ToolStripMenuItem();
            mnuAbout.Click += new EventHandler(mnuAbout_Click);
            mnuCtxValues = new ContextMenuStrip(components);
            mnuEditData = new ToolStripMenuItem();
            mnuEditData.MouseUp += new MouseEventHandler(mnuEditData_MouseUp);
            mnuClearData = new ToolStripMenuItem();
            mnuClearData.MouseUp += new MouseEventHandler(mnuClearData_MouseUp);
            mnuRenameValue = new ToolStripMenuItem();
            mnuRenameValue.MouseUp += new MouseEventHandler(mnuRenameValue_MouseUp);
            mnuValueSeparator = new ToolStripSeparator();
            mnuNewValue = new ToolStripMenuItem();
            mnuNewValue.MouseUp += new MouseEventHandler(mnuNewValue_MouseUp);
            mnuDeleteValue = new ToolStripMenuItem();
            mnuDeleteValue.MouseUp += new MouseEventHandler(mnuDeleteValue_MouseUp);
            ((System.ComponentModel.ISupportInitialize)KeyValues).BeginInit();
            mnuCtxKeys.SuspendLayout();
            MenuStrip1.SuspendLayout();
            mnuCtxValues.SuspendLayout();
            SuspendLayout();
            // 
            // KeyTree
            // 
            KeyTree.Location = new Point(13, 27);
            KeyTree.Name = "KeyTree";
            KeyTree.Size = new Size(299, 501);
            KeyTree.TabIndex = 0;
            // 
            // KeyValues
            // 
            KeyValues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            KeyValues.Columns.AddRange(new DataGridViewColumn[] { Value, Data });
            KeyValues.Location = new Point(328, 27);
            KeyValues.Name = "KeyValues";
            DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle1.BackColor = SystemColors.Control;
            DataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            KeyValues.RowHeadersDefaultCellStyle = DataGridViewCellStyle1;
            DataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            KeyValues.RowsDefaultCellStyle = DataGridViewCellStyle2;
            KeyValues.RowTemplate.Height = 17;
            KeyValues.ScrollBars = ScrollBars.Vertical;
            KeyValues.Size = new Size(628, 501);
            KeyValues.TabIndex = 1;
            // 
            // Value
            // 
            Value.HeaderText = "Value";
            Value.Name = "Value";
            // 
            // Data
            // 
            Data.HeaderText = "Data";
            Data.Name = "Data";
            // 
            // mnuCtxKeys
            // 
            mnuCtxKeys.Items.AddRange(new ToolStripItem[] { mnuNewKey, ToolStripSeparator1, mnuDeleteKey, mnuRenameKey });
            mnuCtxKeys.Name = "mnuCtx";
            mnuCtxKeys.Size = new Size(159, 76);
            // 
            // mnuNewKey
            // 
            mnuNewKey.Name = "mnuNewKey";
            mnuNewKey.ShortcutKeyDisplayString = "Ins";
            mnuNewKey.Size = new Size(158, 22);
            mnuNewKey.Text = "New Key";
            mnuNewKey.ToolTipText = "Create a new key";
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(155, 6);
            // 
            // mnuDeleteKey
            // 
            mnuDeleteKey.Name = "mnuDeleteKey";
            mnuDeleteKey.ShortcutKeyDisplayString = "Del";
            mnuDeleteKey.Size = new Size(158, 22);
            mnuDeleteKey.Text = "Delete Key";
            mnuDeleteKey.ToolTipText = "Delete a key";
            // 
            // mnuRenameKey
            // 
            mnuRenameKey.Name = "mnuRenameKey";
            mnuRenameKey.ShortcutKeyDisplayString = "F2";
            mnuRenameKey.ShortcutKeys = Keys.F2;
            mnuRenameKey.Size = new Size(158, 22);
            mnuRenameKey.Text = "Rename Key";
            mnuRenameKey.ToolTipText = "Rename a key";
            // 
            // MenuStrip1
            // 
            MenuStrip1.Items.AddRange(new ToolStripItem[] { mnuFile, mnuOptions, mnuHelp });
            MenuStrip1.Location = new Point(0, 0);
            MenuStrip1.Name = "MenuStrip1";
            MenuStrip1.Size = new Size(968, 24);
            MenuStrip1.TabIndex = 2;
            MenuStrip1.Text = "MenuStrip1";
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuExit });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new Size(37, 20);
            mnuFile.Text = "File";
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            mnuExit.Size = new Size(92, 22);
            mnuExit.Text = "Exit";
            // 
            // mnuOptions
            // 
            mnuOptions.DropDownItems.AddRange(new ToolStripItem[] { mnuRootEdit });
            mnuOptions.Name = "mnuOptions";
            mnuOptions.Size = new Size(61, 20);
            mnuOptions.Text = "Options";
            // 
            // mnuRootEdit
            // 
            mnuRootEdit.Name = "mnuRootEdit";
            mnuRootEdit.Size = new Size(160, 22);
            mnuRootEdit.Text = "Enable Root Edit";
            // 
            // mnuHelp
            // 
            mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuAbout });
            mnuHelp.Name = "mnuHelp";
            mnuHelp.Size = new Size(44, 20);
            mnuHelp.Text = "Help";
            // 
            // mnuAbout
            // 
            mnuAbout.Name = "mnuAbout";
            mnuAbout.Size = new Size(152, 22);
            mnuAbout.Text = "About";
            // 
            // mnuCtxValues
            // 
            mnuCtxValues.Items.AddRange(new ToolStripItem[] { mnuEditData, mnuClearData, mnuRenameValue, mnuValueSeparator, mnuNewValue, mnuDeleteValue });
            mnuCtxValues.Name = "mnuCtxValues";
            mnuCtxValues.Size = new Size(153, 142);
            // 
            // mnuEditData
            // 
            mnuEditData.Name = "mnuEditData";
            mnuEditData.Size = new Size(152, 22);
            mnuEditData.Text = "Edit Data";
            // 
            // mnuClearData
            // 
            mnuClearData.Name = "mnuClearData";
            mnuClearData.Size = new Size(152, 22);
            mnuClearData.Text = "Clear Data";
            // 
            // mnuRenameValue
            // 
            mnuRenameValue.Name = "mnuRenameValue";
            mnuRenameValue.Size = new Size(152, 22);
            mnuRenameValue.Text = "Rename Value";
            // 
            // mnuValueSeparator
            // 
            mnuValueSeparator.Name = "mnuValueSeparator";
            mnuValueSeparator.Size = new Size(149, 6);
            // 
            // mnuNewValue
            // 
            mnuNewValue.Name = "mnuNewValue";
            mnuNewValue.Size = new Size(152, 22);
            mnuNewValue.Text = "New Value";
            // 
            // mnuDeleteValue
            // 
            mnuDeleteValue.Name = "mnuDeleteValue";
            mnuDeleteValue.Size = new Size(152, 22);
            mnuDeleteValue.Text = "Delete Value";
            // 
            // frmProfileExplorer
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 651);
            Controls.Add(MenuStrip1);
            Controls.Add(KeyValues);
            Controls.Add(KeyTree);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = MenuStrip1;
            Name = "frmProfileExplorer";
            Text = "Profile Explorer";
            ((System.ComponentModel.ISupportInitialize)KeyValues).EndInit();
            mnuCtxKeys.ResumeLayout(false);
            MenuStrip1.ResumeLayout(false);
            MenuStrip1.PerformLayout();
            mnuCtxValues.ResumeLayout(false);
            Load += new EventHandler(Form1_Load);
            Resize += new EventHandler(Form1_Resize);
            ResumeLayout(false);
            PerformLayout();

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