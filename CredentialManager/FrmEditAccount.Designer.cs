namespace CredentialManager
{
    partial class FrmEditAccount
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkIsEnabled = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveTrustedIPs = new System.Windows.Forms.Button();
            this.btnEditTrustedIP = new System.Windows.Forms.Button();
            this.btnAddTrustedIP = new System.Windows.Forms.Button();
            this.lstTrustedIPs = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemoveRoles = new System.Windows.Forms.Button();
            this.btnEditRoles = new System.Windows.Forms.Button();
            this.lstRoles = new System.Windows.Forms.ListBox();
            this.btnAddRoles = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnRemoveCertificates = new System.Windows.Forms.Button();
            this.btnEditCertificates = new System.Windows.Forms.Button();
            this.btnAddCertificates = new System.Windows.Forms.Button();
            this.lstCertificates = new System.Windows.Forms.ListBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkIsEnabled);
            this.groupBox5.Controls.Add(this.txtDescription);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtName);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(498, 190);
            this.groupBox5.TabIndex = 41;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "General";
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.AutoSize = true;
            this.chkIsEnabled.Location = new System.Drawing.Point(72, 23);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(76, 17);
            this.chkIsEnabled.TabIndex = 29;
            this.chkIsEnabled.Text = "Is Enabled";
            this.chkIsEnabled.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(72, 72);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(408, 100);
            this.txtDescription.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(72, 46);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(408, 20);
            this.txtName.TabIndex = 31;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(12, 12);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(760, 509);
            this.splitContainer2.SplitterDistance = 190;
            this.splitContainer2.TabIndex = 42;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer4.Size = new System.Drawing.Size(760, 190);
            this.splitContainer4.SplitterDistance = 498;
            this.splitContainer4.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemoveTrustedIPs);
            this.groupBox2.Controls.Add(this.btnEditTrustedIP);
            this.groupBox2.Controls.Add(this.btnAddTrustedIP);
            this.groupBox2.Controls.Add(this.lstTrustedIPs);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 190);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Trusted IPs";
            // 
            // btnRemoveTrustedIPs
            // 
            this.btnRemoveTrustedIPs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveTrustedIPs.Location = new System.Drawing.Point(136, 161);
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
            this.btnEditTrustedIP.Location = new System.Drawing.Point(71, 161);
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
            this.btnAddTrustedIP.Location = new System.Drawing.Point(6, 161);
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
            this.lstTrustedIPs.Size = new System.Drawing.Size(244, 134);
            this.lstTrustedIPs.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer3.Size = new System.Drawing.Size(760, 315);
            this.splitContainer3.SplitterDistance = 234;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoveRoles);
            this.groupBox1.Controls.Add(this.btnEditRoles);
            this.groupBox1.Controls.Add(this.lstRoles);
            this.groupBox1.Controls.Add(this.btnAddRoles);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 315);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Granted Roles";
            // 
            // btnRemoveRoles
            // 
            this.btnRemoveRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveRoles.Location = new System.Drawing.Point(139, 283);
            this.btnRemoveRoles.Name = "btnRemoveRoles";
            this.btnRemoveRoles.Size = new System.Drawing.Size(59, 23);
            this.btnRemoveRoles.TabIndex = 17;
            this.btnRemoveRoles.Text = "Remove";
            this.btnRemoveRoles.UseVisualStyleBackColor = true;
            this.btnRemoveRoles.Click += new System.EventHandler(this.btnRemoveRoles_Click);
            // 
            // btnEditRoles
            // 
            this.btnEditRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditRoles.Location = new System.Drawing.Point(74, 283);
            this.btnEditRoles.Name = "btnEditRoles";
            this.btnEditRoles.Size = new System.Drawing.Size(59, 23);
            this.btnEditRoles.TabIndex = 16;
            this.btnEditRoles.Text = "Edit";
            this.btnEditRoles.UseVisualStyleBackColor = true;
            this.btnEditRoles.Click += new System.EventHandler(this.btnEditRoles_Click);
            // 
            // lstRoles
            // 
            this.lstRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRoles.FormattingEnabled = true;
            this.lstRoles.Location = new System.Drawing.Point(8, 16);
            this.lstRoles.Name = "lstRoles";
            this.lstRoles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstRoles.Size = new System.Drawing.Size(220, 251);
            this.lstRoles.TabIndex = 1;
            // 
            // btnAddRoles
            // 
            this.btnAddRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRoles.Location = new System.Drawing.Point(9, 283);
            this.btnAddRoles.Name = "btnAddRoles";
            this.btnAddRoles.Size = new System.Drawing.Size(59, 23);
            this.btnAddRoles.TabIndex = 15;
            this.btnAddRoles.Text = "Add";
            this.btnAddRoles.UseVisualStyleBackColor = true;
            this.btnAddRoles.Click += new System.EventHandler(this.btnAddRoles_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnRemoveCertificates);
            this.groupBox4.Controls.Add(this.btnEditCertificates);
            this.groupBox4.Controls.Add(this.btnAddCertificates);
            this.groupBox4.Controls.Add(this.lstCertificates);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(522, 315);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Client Certificates";
            // 
            // btnRemoveCertificates
            // 
            this.btnRemoveCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveCertificates.Location = new System.Drawing.Point(136, 283);
            this.btnRemoveCertificates.Name = "btnRemoveCertificates";
            this.btnRemoveCertificates.Size = new System.Drawing.Size(59, 23);
            this.btnRemoveCertificates.TabIndex = 20;
            this.btnRemoveCertificates.Text = "Remove";
            this.btnRemoveCertificates.UseVisualStyleBackColor = true;
            this.btnRemoveCertificates.Click += new System.EventHandler(this.btnRemoveCertificates_Click);
            // 
            // btnEditCertificates
            // 
            this.btnEditCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditCertificates.Location = new System.Drawing.Point(71, 283);
            this.btnEditCertificates.Name = "btnEditCertificates";
            this.btnEditCertificates.Size = new System.Drawing.Size(59, 23);
            this.btnEditCertificates.TabIndex = 19;
            this.btnEditCertificates.Text = "Edit";
            this.btnEditCertificates.UseVisualStyleBackColor = true;
            this.btnEditCertificates.Click += new System.EventHandler(this.btnEditCertificates_Click);
            // 
            // btnAddCertificates
            // 
            this.btnAddCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCertificates.Location = new System.Drawing.Point(6, 283);
            this.btnAddCertificates.Name = "btnAddCertificates";
            this.btnAddCertificates.Size = new System.Drawing.Size(59, 23);
            this.btnAddCertificates.TabIndex = 18;
            this.btnAddCertificates.Text = "Add";
            this.btnAddCertificates.UseVisualStyleBackColor = true;
            this.btnAddCertificates.Click += new System.EventHandler(this.btnAddCertificates_Click);
            // 
            // lstCertificates
            // 
            this.lstCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCertificates.DisplayMember = "DisplayMember";
            this.lstCertificates.FormattingEnabled = true;
            this.lstCertificates.Location = new System.Drawing.Point(9, 16);
            this.lstCertificates.Name = "lstCertificates";
            this.lstCertificates.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstCertificates.Size = new System.Drawing.Size(504, 251);
            this.lstCertificates.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(697, 527);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 43;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // FrmEditAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.splitContainer2);
            this.Name = "FrmEditAccount";
            this.Text = "Edit Account Details";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkIsEnabled;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveTrustedIPs;
        private System.Windows.Forms.Button btnEditTrustedIP;
        private System.Windows.Forms.Button btnAddTrustedIP;
        private System.Windows.Forms.ListBox lstTrustedIPs;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRemoveRoles;
        private System.Windows.Forms.Button btnEditRoles;
        private System.Windows.Forms.ListBox lstRoles;
        private System.Windows.Forms.Button btnAddRoles;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnRemoveCertificates;
        private System.Windows.Forms.Button btnEditCertificates;
        private System.Windows.Forms.Button btnAddCertificates;
        private System.Windows.Forms.ListBox lstCertificates;
        private System.Windows.Forms.Button BtnOK;
    }
}