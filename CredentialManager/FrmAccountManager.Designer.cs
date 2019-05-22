namespace CredentialManager
{
    partial class FrmAccountManager
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
            this.btnAddAccount = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstAccounts = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLocalCertPath = new System.Windows.Forms.TextBox();
            this.btnBrowseLocalCertificate = new System.Windows.Forms.Button();
            this.chkEnableSSL = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLocalCertPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkUseEphemeralCertificates = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAccount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAccount.Location = new System.Drawing.Point(652, 15);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(28, 23);
            this.btnAddAccount.TabIndex = 12;
            this.btnAddAccount.Text = "+";
            this.btnAddAccount.UseVisualStyleBackColor = true;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lstAccounts);
            this.groupBox3.Controls.Add(this.btnAddAccount);
            this.groupBox3.Location = new System.Drawing.Point(12, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(689, 183);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accounts";
            // 
            // lstAccounts
            // 
            this.lstAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAccounts.DisplayMember = "DisplayMember";
            this.lstAccounts.FormattingEnabled = true;
            this.lstAccounts.Location = new System.Drawing.Point(12, 15);
            this.lstAccounts.Name = "lstAccounts";
            this.lstAccounts.Size = new System.Drawing.Size(634, 147);
            this.lstAccounts.TabIndex = 0;
            this.lstAccounts.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstAccounts_KeyUp);
            this.lstAccounts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstAccounts_MouseDoubleClick);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(118, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 23);
            this.btnOpen.TabIndex = 14;
            this.btnOpen.Text = "Open Config";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save Config";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(224, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 23);
            this.btnNew.TabIndex = 16;
            this.btnNew.Text = "New Config";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Local Certificate Path";
            // 
            // txtLocalCertPath
            // 
            this.txtLocalCertPath.Location = new System.Drawing.Point(123, 49);
            this.txtLocalCertPath.Name = "txtLocalCertPath";
            this.txtLocalCertPath.Size = new System.Drawing.Size(477, 20);
            this.txtLocalCertPath.TabIndex = 40;
            // 
            // btnBrowseLocalCertificate
            // 
            this.btnBrowseLocalCertificate.Location = new System.Drawing.Point(606, 47);
            this.btnBrowseLocalCertificate.Name = "btnBrowseLocalCertificate";
            this.btnBrowseLocalCertificate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLocalCertificate.TabIndex = 41;
            this.btnBrowseLocalCertificate.Text = "Browse";
            this.btnBrowseLocalCertificate.UseVisualStyleBackColor = true;
            this.btnBrowseLocalCertificate.Click += new System.EventHandler(this.btnBrowseLocalCertificate_Click);
            // 
            // chkEnableSSL
            // 
            this.chkEnableSSL.AutoSize = true;
            this.chkEnableSSL.Location = new System.Drawing.Point(12, 19);
            this.chkEnableSSL.Name = "chkEnableSSL";
            this.chkEnableSSL.Size = new System.Drawing.Size(82, 17);
            this.chkEnableSSL.TabIndex = 42;
            this.chkEnableSSL.Text = "Enable SSL";
            this.chkEnableSSL.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkUseEphemeralCertificates);
            this.groupBox2.Controls.Add(this.txtLocalCertPassword);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkEnableSSL);
            this.groupBox2.Controls.Add(this.btnBrowseLocalCertificate);
            this.groupBox2.Controls.Add(this.txtLocalCertPath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 230);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(690, 112);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Listener Options";
            // 
            // txtLocalCertPassword
            // 
            this.txtLocalCertPassword.Location = new System.Drawing.Point(153, 75);
            this.txtLocalCertPassword.Name = "txtLocalCertPassword";
            this.txtLocalCertPassword.Size = new System.Drawing.Size(200, 20);
            this.txtLocalCertPassword.TabIndex = 44;
            this.txtLocalCertPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "PFX Password (Optional)";
            // 
            // chkUseEphemeralCertificates
            // 
            this.chkUseEphemeralCertificates.AutoSize = true;
            this.chkUseEphemeralCertificates.Location = new System.Drawing.Point(106, 19);
            this.chkUseEphemeralCertificates.Name = "chkUseEphemeralCertificates";
            this.chkUseEphemeralCertificates.Size = new System.Drawing.Size(153, 17);
            this.chkUseEphemeralCertificates.TabIndex = 45;
            this.chkUseEphemeralCertificates.Text = "Use Ephemeral Certificates";
            this.chkUseEphemeralCertificates.UseVisualStyleBackColor = true;
            // 
            // FrmAccountManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 351);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Name = "FrmAccountManager";
            this.Text = "Account Manager";
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstAccounts;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLocalCertPath;
        private System.Windows.Forms.Button btnBrowseLocalCertificate;
        private System.Windows.Forms.CheckBox chkEnableSSL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtLocalCertPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkUseEphemeralCertificates;
    }
}

