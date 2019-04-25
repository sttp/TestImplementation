namespace CredentialManager
{
    partial class FrmClientCertificates
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddTrustedIP = new System.Windows.Forms.Button();
            this.lstTrustedIPs = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMappedAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCertificateDirectory = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(117, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(263, 20);
            this.txtName.TabIndex = 47;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(387, 208);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAddTrustedIP);
            this.groupBox2.Controls.Add(this.lstTrustedIPs);
            this.groupBox2.Location = new System.Drawing.Point(9, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(453, 108);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Allowed Remote IPs";
            // 
            // btnAddTrustedIP
            // 
            this.btnAddTrustedIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTrustedIP.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddTrustedIP.Location = new System.Drawing.Point(417, 17);
            this.btnAddTrustedIP.Name = "btnAddTrustedIP";
            this.btnAddTrustedIP.Size = new System.Drawing.Size(28, 23);
            this.btnAddTrustedIP.TabIndex = 18;
            this.btnAddTrustedIP.Text = "+";
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
            this.lstTrustedIPs.Size = new System.Drawing.Size(405, 69);
            this.lstTrustedIPs.TabIndex = 1;
            this.lstTrustedIPs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstTrustedIPs_KeyUp);
            this.lstTrustedIPs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstTrustedIPs_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Account";
            // 
            // txtMappedAccount
            // 
            this.txtMappedAccount.Location = new System.Drawing.Point(117, 38);
            this.txtMappedAccount.Name = "txtMappedAccount";
            this.txtMappedAccount.Size = new System.Drawing.Size(263, 20);
            this.txtMappedAccount.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "Certificate Directory";
            // 
            // txtCertificateDirectory
            // 
            this.txtCertificateDirectory.Location = new System.Drawing.Point(117, 64);
            this.txtCertificateDirectory.Name = "txtCertificateDirectory";
            this.txtCertificateDirectory.Size = new System.Drawing.Size(263, 20);
            this.txtCertificateDirectory.TabIndex = 67;
            // 
            // FrmClientCertificates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 243);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCertificateDirectory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMappedAccount);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Name = "FrmClientCertificates";
            this.Text = "Client Certificate";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddTrustedIP;
        private System.Windows.Forms.ListBox lstTrustedIPs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMappedAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCertificateDirectory;
    }
}