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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lstInterfaceOptions = new System.Windows.Forms.ListBox();
            this.btnInterfaceOptions = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstAnonymousAccountMapping = new System.Windows.Forms.ListBox();
            this.btnAddAnonomousAccountMapping = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAccount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAccount.Location = new System.Drawing.Point(653, 15);
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
            this.groupBox3.Size = new System.Drawing.Size(690, 248);
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
            this.lstAccounts.Size = new System.Drawing.Size(635, 212);
            this.lstAccounts.TabIndex = 0;
            this.lstAccounts.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstAccounts_KeyUp);
            this.lstAccounts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstAccounts_MouseDoubleClick);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.lstInterfaceOptions);
            this.groupBox6.Controls.Add(this.btnInterfaceOptions);
            this.groupBox6.Location = new System.Drawing.Point(12, 465);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(690, 128);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Certificate Selection Criteria";
            // 
            // lstInterfaceOptions
            // 
            this.lstInterfaceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInterfaceOptions.DisplayMember = "DisplayMember";
            this.lstInterfaceOptions.FormattingEnabled = true;
            this.lstInterfaceOptions.Location = new System.Drawing.Point(12, 15);
            this.lstInterfaceOptions.Name = "lstInterfaceOptions";
            this.lstInterfaceOptions.Size = new System.Drawing.Size(635, 95);
            this.lstInterfaceOptions.TabIndex = 0;
            this.lstInterfaceOptions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstInterfaceOptions_KeyUp);
            this.lstInterfaceOptions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstInterfaceOptions_MouseDoubleClick);
            // 
            // btnInterfaceOptions
            // 
            this.btnInterfaceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInterfaceOptions.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnInterfaceOptions.Location = new System.Drawing.Point(653, 15);
            this.btnInterfaceOptions.Name = "btnInterfaceOptions";
            this.btnInterfaceOptions.Size = new System.Drawing.Size(28, 23);
            this.btnInterfaceOptions.TabIndex = 12;
            this.btnInterfaceOptions.Text = "+";
            this.btnInterfaceOptions.UseVisualStyleBackColor = true;
            this.btnInterfaceOptions.Click += new System.EventHandler(this.btnInterfaceOptions_Click);
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
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstAnonymousAccountMapping);
            this.groupBox1.Controls.Add(this.btnAddAnonomousAccountMapping);
            this.groupBox1.Location = new System.Drawing.Point(12, 295);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(690, 164);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Anonymous Account Mapping";
            // 
            // lstAnonymousAccountMapping
            // 
            this.lstAnonymousAccountMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAnonymousAccountMapping.DisplayMember = "DisplayMember";
            this.lstAnonymousAccountMapping.FormattingEnabled = true;
            this.lstAnonymousAccountMapping.Location = new System.Drawing.Point(12, 15);
            this.lstAnonymousAccountMapping.Name = "lstAnonymousAccountMapping";
            this.lstAnonymousAccountMapping.Size = new System.Drawing.Size(635, 134);
            this.lstAnonymousAccountMapping.TabIndex = 0;
            this.lstAnonymousAccountMapping.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstAnonymousAccountMapping_KeyUp);
            this.lstAnonymousAccountMapping.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstAnonymousAccountMapping_MouseDoubleClick);
            // 
            // btnAddAnonomousAccountMapping
            // 
            this.btnAddAnonomousAccountMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAnonomousAccountMapping.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddAnonomousAccountMapping.Location = new System.Drawing.Point(653, 15);
            this.btnAddAnonomousAccountMapping.Name = "btnAddAnonomousAccountMapping";
            this.btnAddAnonomousAccountMapping.Size = new System.Drawing.Size(28, 23);
            this.btnAddAnonomousAccountMapping.TabIndex = 12;
            this.btnAddAnonomousAccountMapping.Text = "+";
            this.btnAddAnonomousAccountMapping.UseVisualStyleBackColor = true;
            this.btnAddAnonomousAccountMapping.Click += new System.EventHandler(this.btnAddAnonomousAccountMapping_Click);
            // 
            // FrmAccountManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 605);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Name = "FrmAccountManager";
            this.Text = "Account Manager";
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstAccounts;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox lstInterfaceOptions;
        private System.Windows.Forms.Button btnInterfaceOptions;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstAnonymousAccountMapping;
        private System.Windows.Forms.Button btnAddAnonomousAccountMapping;
    }
}

