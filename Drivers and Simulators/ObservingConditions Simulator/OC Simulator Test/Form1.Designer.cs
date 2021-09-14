namespace ASCOM.Simulator
{
    partial class Form1
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
            this.buttonToggleConnected = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonAutoRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonToggleConnected
            // 
            this.buttonToggleConnected.Location = new System.Drawing.Point(646, 10);
            this.buttonToggleConnected.Name = "buttonToggleConnected";
            this.buttonToggleConnected.Size = new System.Drawing.Size(86, 23);
            this.buttonToggleConnected.TabIndex = 0;
            this.buttonToggleConnected.Text = "Connect";
            this.buttonToggleConnected.UseVisualStyleBackColor = true;
            this.buttonToggleConnected.Click += new System.EventHandler(this.buttonToggleConnected_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.Location = new System.Drawing.Point(349, 11);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(291, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(12, 41);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(628, 609);
            this.txtStatus.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(646, 627);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(86, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(646, 68);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(86, 23);
            this.btnSetup.TabIndex = 5;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(646, 97);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(86, 23);
            this.buttonRefresh.TabIndex = 6;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonAutoRefresh
            // 
            this.buttonAutoRefresh.Location = new System.Drawing.Point(646, 126);
            this.buttonAutoRefresh.Name = "buttonAutoRefresh";
            this.buttonAutoRefresh.Size = new System.Drawing.Size(86, 23);
            this.buttonAutoRefresh.TabIndex = 7;
            this.buttonAutoRefresh.Text = "Auto Refresh";
            this.buttonAutoRefresh.UseVisualStyleBackColor = true;
            this.buttonAutoRefresh.Click += new System.EventHandler(this.buttonAutoRefresh_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonToggleConnected;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(739, 661);
            this.Controls.Add(this.buttonAutoRefresh);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonToggleConnected);
            this.Name = "Form1";
            this.Text = "Observing conditions Simulator test harness";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonToggleConnected;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonAutoRefresh;
    }
}

