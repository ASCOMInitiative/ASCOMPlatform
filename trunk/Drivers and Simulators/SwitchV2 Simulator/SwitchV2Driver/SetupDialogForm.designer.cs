namespace ASCOM.Simulator
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.dataGridViewSwitches = new System.Windows.Forms.DataGridView();
            this.checkBoxSetupSimulator = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelVersion = new System.Windows.Forms.Label();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.switchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCanWrite = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSwitches)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(437, 264);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(502, 263);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "Add switches and set their properties here";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(513, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Switch List";
            // 
            // chkTrace
            // 
            this.chkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(8, 269);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSwitches
            // 
            this.dataGridViewSwitches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSwitches.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSwitches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSwitches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.switchName,
            this.colDescription,
            this.colValue,
            this.colMin,
            this.colMax,
            this.colStep,
            this.colCanWrite});
            this.dataGridViewSwitches.Location = new System.Drawing.Point(8, 71);
            this.dataGridViewSwitches.MultiSelect = false;
            this.dataGridViewSwitches.Name = "dataGridViewSwitches";
            this.dataGridViewSwitches.RowHeadersWidth = 40;
            this.dataGridViewSwitches.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewSwitches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewSwitches.Size = new System.Drawing.Size(553, 186);
            this.dataGridViewSwitches.TabIndex = 7;
            this.toolTip1.SetToolTip(this.dataGridViewSwitches, "This is used to show and edit the switch properties. Set Up Simulator must be che" +
        "cked to allow the switch type properties to be changed.");
            this.dataGridViewSwitches.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewSwitches_CellValidating);
            this.dataGridViewSwitches.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewSwitches_EditingControlShowing);
            this.dataGridViewSwitches.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewSwitches_RowsAdded);
            this.dataGridViewSwitches.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewSwitches_RowsRemoved);
            this.dataGridViewSwitches.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewSwitches_RowValidating);
            // 
            // checkBoxSetupSimulator
            // 
            this.checkBoxSetupSimulator.AutoSize = true;
            this.checkBoxSetupSimulator.Location = new System.Drawing.Point(288, 46);
            this.checkBoxSetupSimulator.Name = "checkBoxSetupSimulator";
            this.checkBoxSetupSimulator.Size = new System.Drawing.Size(103, 17);
            this.checkBoxSetupSimulator.TabIndex = 8;
            this.checkBoxSetupSimulator.Text = "Set up Simulator";
            this.toolTip1.SetToolTip(this.checkBoxSetupSimulator, "Check this to allow the switch properties to be set up.");
            this.checkBoxSetupSimulator.UseVisualStyleBackColor = true;
            this.checkBoxSetupSimulator.CheckedChanged += new System.EventHandler(this.checkBoxSetupSimulator_CheckedChanged);
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(105, 270);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(45, 13);
            this.labelVersion.TabIndex = 9;
            this.labelVersion.Text = "Version:";
            // 
            // colId
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colId.DefaultCellStyle = dataGridViewCellStyle2;
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colId.ToolTipText = "This is the switch number, it is not editable.";
            this.colId.Width = 25;
            // 
            // switchName
            // 
            this.switchName.DataPropertyName = "Name";
            this.switchName.HeaderText = "Name";
            this.switchName.MaxInputLength = 255;
            this.switchName.Name = "switchName";
            this.switchName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.switchName.ToolTipText = "Set the Switch name.";
            this.switchName.Width = 80;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ToolTipText = "Set the Switch description";
            this.colDescription.Width = 200;
            // 
            // colValue
            // 
            this.colValue.DataPropertyName = "Value";
            dataGridViewCellStyle3.NullValue = null;
            this.colValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.colValue.HeaderText = "Value";
            this.colValue.MaxInputLength = 255;
            this.colValue.Name = "colValue";
            this.colValue.ToolTipText = "Set the current switch value.";
            this.colValue.Width = 40;
            // 
            // colMin
            // 
            this.colMin.DataPropertyName = "Minimum";
            this.colMin.HeaderText = "Min";
            this.colMin.MaxInputLength = 255;
            this.colMin.Name = "colMin";
            this.colMin.ToolTipText = "set the minimum switch value, 0 for a binary switch.";
            this.colMin.Width = 40;
            // 
            // colMax
            // 
            this.colMax.DataPropertyName = "Maximum";
            this.colMax.HeaderText = "Max";
            this.colMax.MaxInputLength = 255;
            this.colMax.Name = "colMax";
            this.colMax.ToolTipText = "Set the maximum switch value, 1 for a boolean switch.";
            this.colMax.Width = 40;
            // 
            // colStep
            // 
            this.colStep.DataPropertyName = "StepSize";
            dataGridViewCellStyle4.NullValue = null;
            this.colStep.DefaultCellStyle = dataGridViewCellStyle4;
            this.colStep.HeaderText = "Step Size";
            this.colStep.MaxInputLength = 255;
            this.colStep.Name = "colStep";
            this.colStep.ToolTipText = "Set the switch step size, or minimum change for a sensor.";
            this.colStep.Width = 40;
            // 
            // colCanWrite
            // 
            this.colCanWrite.DataPropertyName = "CanWrite";
            this.colCanWrite.HeaderText = "Can Write";
            this.colCanWrite.Name = "colCanWrite";
            this.colCanWrite.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colCanWrite.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCanWrite.ToolTipText = "The is unchecked for a switch cannot be set, i.e. a sensor.";
            this.colCanWrite.Width = 40;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 296);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.checkBoxSetupSimulator);
            this.Controls.Add(this.dataGridViewSwitches);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulator Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSwitches)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.DataGridView dataGridViewSwitches;
        private System.Windows.Forms.CheckBox checkBoxSetupSimulator;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn switchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStep;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCanWrite;
    }
}