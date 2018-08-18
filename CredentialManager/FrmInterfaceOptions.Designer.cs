namespace CredentialManager
{
    partial class FrmInterfaceOptions
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
            this.chkIsEnabled = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveTrustedIPs = new System.Windows.Forms.Button();
            this.btnEditTrustedIP = new System.Windows.Forms.Button();
            this.btnAddTrustedIP = new System.Windows.Forms.Button();
            this.lstTrustedIPs = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCertPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.chkDisableSSL = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.AutoSize = true;
            this.chkIsEnabled.Location = new System.Drawing.Point(12, 12);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(76, 17);
            this.chkIsEnabled.TabIndex = 30;
            this.chkIsEnabled.Text = "Is Enabled";
            this.chkIsEnabled.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(102, 35);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(278, 20);
            this.txtName.TabIndex = 35;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.btnRemoveTrustedIPs);
            this.groupBox2.Controls.Add(this.btnEditTrustedIP);
            this.groupBox2.Controls.Add(this.btnAddTrustedIP);
            this.groupBox2.Controls.Add(this.lstTrustedIPs);
            this.groupBox2.Location = new System.Drawing.Point(15, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 203);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Trusted IPs";
            // 
            // btnRemoveTrustedIPs
            // 
            this.btnRemoveTrustedIPs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveTrustedIPs.Location = new System.Drawing.Point(136, 174);
            this.btnRemoveTrustedIPs.Name = "btnRemoveTrustedIPs";
            this.btnRemoveTrustedIPs.Size = new System.Drawing.Size(59, 23);
            this.btnRemoveTrustedIPs.TabIndex = 20;
            this.btnRemoveTrustedIPs.Text = "Remove";
            this.btnRemoveTrustedIPs.UseVisualStyleBackColor = true;
            this.btnRemoveTrustedIPs.Click += new System.EventHandler(this.btnRemoveTrustedIPs_Click);
            // 
            // btnEditTrustedIP
            // 
            this.btnEditTrustedIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditTrustedIP.Location = new System.Drawing.Point(71, 174);
            this.btnEditTrustedIP.Name = "btnEditTrustedIP";
            this.btnEditTrustedIP.Size = new System.Drawing.Size(59, 23);
            this.btnEditTrustedIP.TabIndex = 19;
            this.btnEditTrustedIP.Text = "Edit";
            this.btnEditTrustedIP.UseVisualStyleBackColor = true;
            this.btnEditTrustedIP.Click += new System.EventHandler(this.btnEditTrustedIP_Click);
            // 
            // btnAddTrustedIP
            // 
            this.btnAddTrustedIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTrustedIP.Location = new System.Drawing.Point(6, 174);
            this.btnAddTrustedIP.Name = "btnAddTrustedIP";
            this.btnAddTrustedIP.Size = new System.Drawing.Size(59, 23);
            this.btnAddTrustedIP.TabIndex = 18;
            this.btnAddTrustedIP.Text = "Add";
            this.btnAddTrustedIP.UseVisualStyleBackColor = true;
            this.btnAddTrustedIP.Click += new System.EventHandler(this.btnAddTrustedIP_Click);
            // 
            // lstTrustedIPs
            // 
            this.lstTrustedIPs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTrustedIPs.DisplayMember = "DisplayMember";
            this.lstTrustedIPs.FormattingEnabled = true;
            this.lstTrustedIPs.Location = new System.Drawing.Point(6, 19);
            this.lstTrustedIPs.Name = "lstTrustedIPs";
            this.lstTrustedIPs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTrustedIPs.Size = new System.Drawing.Size(242, 147);
            this.lstTrustedIPs.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Certificate Path";
            // 
            // txtCertPath
            // 
            this.txtCertPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCertPath.Location = new System.Drawing.Point(102, 63);
            this.txtCertPath.Name = "txtCertPath";
            this.txtCertPath.Size = new System.Drawing.Size(278, 20);
            this.txtCertPath.TabIndex = 40;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(386, 61);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 41;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(467, 61);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 42;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(467, 288);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 43;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // chkDisableSSL
            // 
            this.chkDisableSSL.AutoSize = true;
            this.chkDisableSSL.Location = new System.Drawing.Point(117, 12);
            this.chkDisableSSL.Name = "chkDisableSSL";
            this.chkDisableSSL.Size = new System.Drawing.Size(84, 17);
            this.chkDisableSSL.TabIndex = 44;
            this.chkDisableSSL.Text = "Disable SSL";
            this.chkDisableSSL.UseVisualStyleBackColor = true;
            // 
            // FrmInterfaceOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 323);
            this.Controls.Add(this.chkDisableSSL);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtCertPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.chkIsEnabled);
            this.Name = "FrmInterfaceOptions";
            this.Text = "FrmInterfaceOptions";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsEnabled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveTrustedIPs;
        private System.Windows.Forms.Button btnEditTrustedIP;
        private System.Windows.Forms.Button btnAddTrustedIP;
        private System.Windows.Forms.ListBox lstTrustedIPs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCertPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.CheckBox chkDisableSSL;
    }
}