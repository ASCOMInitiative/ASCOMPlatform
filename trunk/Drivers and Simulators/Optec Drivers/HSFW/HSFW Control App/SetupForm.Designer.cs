namespace HSFWControlApp
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.Ok_Btn = new System.Windows.Forms.Button();
            this.ObjectNameErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.RestDefaults_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectNameErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // Ok_Btn
            // 
            this.Ok_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok_Btn.Location = new System.Drawing.Point(237, 423);
            this.Ok_Btn.Name = "Ok_Btn";
            this.Ok_Btn.Size = new System.Drawing.Size(75, 23);
            this.Ok_Btn.TabIndex = 4;
            this.Ok_Btn.Text = "OK";
            this.Ok_Btn.UseVisualStyleBackColor = true;
            this.Ok_Btn.Click += new System.EventHandler(this.OK_Btn_Click);
            // 
            // ObjectNameErrorProvider
            // 
            this.ObjectNameErrorProvider.ContainerControl = this;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(300, 394);
            this.propertyGrid1.TabIndex = 5;
            // 
            // RestDefaults_Btn
            // 
            this.RestDefaults_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RestDefaults_Btn.Location = new System.Drawing.Point(12, 423);
            this.RestDefaults_Btn.Name = "RestDefaults_Btn";
            this.RestDefaults_Btn.Size = new System.Drawing.Size(132, 23);
            this.RestDefaults_Btn.TabIndex = 6;
            this.RestDefaults_Btn.Text = "Restore Default Names";
            this.RestDefaults_Btn.UseVisualStyleBackColor = true;
            this.RestDefaults_Btn.Click += new System.EventHandler(this.RestDefaults_Btn_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 457);
            this.Controls.Add(this.RestDefaults_Btn);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.Ok_Btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Filter Wheel ";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectNameErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Ok_Btn;
        private System.Windows.Forms.ErrorProvider ObjectNameErrorProvider;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button RestDefaults_Btn;
    }
}