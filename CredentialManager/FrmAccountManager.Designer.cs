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
            this.btnRemoveAccount = new System.Windows.Forms.Button();
            this.btnRemoveRoles = new System.Windows.Forms.Button();
            this.btnAddRoles = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstRoles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIsAuthenticationService = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkIsPairingAccount = new System.Windows.Forms.CheckBox();
            this.chkIsDisabled = new System.Windows.Forms.CheckBox();
            this.txtVerifier = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPasswordFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddAccount.Location = new System.Drawing.Point(15, 463);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(75, 23);
            this.btnAddAccount.TabIndex = 12;
            this.btnAddAccount.Text = "Add";
            this.btnAddAccount.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lstAccounts);
            this.groupBox3.Location = new System.Drawing.Point(12, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 426);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accounts";
            // 
            // lstAccounts
            // 
            this.lstAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAccounts.FormattingEnabled = true;
            this.lstAccounts.Location = new System.Drawing.Point(3, 16);
            this.lstAccounts.Name = "lstAccounts";
            this.lstAccounts.Size = new System.Drawing.Size(168, 407);
            this.lstAccounts.TabIndex = 0;
            // 
            // btnRemoveAccount
            // 
            this.btnRemoveAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveAccount.Location = new System.Drawing.Point(108, 463);
            this.btnRemoveAccount.Name = "btnRemoveAccount";
            this.btnRemoveAccount.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAccount.TabIndex = 13;
            this.btnRemoveAccount.Text = "Remove";
            this.btnRemoveAccount.UseVisualStyleBackColor = true;
            // 
            // btnRemoveRoles
            // 
            this.btnRemoveRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveRoles.Location = new System.Drawing.Point(299, 463);
            this.btnRemoveRoles.Name = "btnRemoveRoles";
            this.btnRemoveRoles.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveRoles.TabIndex = 16;
            this.btnRemoveRoles.Text = "Remove";
            this.btnRemoveRoles.UseVisualStyleBackColor = true;
            // 
            // btnAddRoles
            // 
            this.btnAddRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRoles.Location = new System.Drawing.Point(206, 463);
            this.btnAddRoles.Name = "btnAddRoles";
            this.btnAddRoles.Size = new System.Drawing.Size(75, 23);
            this.btnAddRoles.TabIndex = 15;
            this.btnAddRoles.Text = "Add";
            this.btnAddRoles.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lstRoles);
            this.groupBox1.Location = new System.Drawing.Point(203, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 426);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Roles";
            // 
            // lstRoles
            // 
            this.lstRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRoles.FormattingEnabled = true;
            this.lstRoles.Location = new System.Drawing.Point(3, 16);
            this.lstRoles.Name = "lstRoles";
            this.lstRoles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstRoles.Size = new System.Drawing.Size(168, 407);
            this.lstRoles.TabIndex = 1;
            this.toolTip1.SetToolTip(this.lstRoles, "The roles spcifically granted to the specified user.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(394, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Verifier";
            // 
            // chkIsAuthenticationService
            // 
            this.chkIsAuthenticationService.AutoSize = true;
            this.chkIsAuthenticationService.Location = new System.Drawing.Point(506, 113);
            this.chkIsAuthenticationService.Name = "chkIsAuthenticationService";
            this.chkIsAuthenticationService.Size = new System.Drawing.Size(144, 17);
            this.chkIsAuthenticationService.TabIndex = 18;
            this.chkIsAuthenticationService.Text = "Is Authentication Service";
            this.toolTip1.SetToolTip(this.chkIsAuthenticationService, "Gets if this user account is a designated authentication service and can delegate" +
        " its roles.");
            this.chkIsAuthenticationService.UseVisualStyleBackColor = true;
            // 
            // chkIsPairingAccount
            // 
            this.chkIsPairingAccount.AutoSize = true;
            this.chkIsPairingAccount.Location = new System.Drawing.Point(666, 113);
            this.chkIsPairingAccount.Name = "chkIsPairingAccount";
            this.chkIsPairingAccount.Size = new System.Drawing.Size(112, 17);
            this.chkIsPairingAccount.TabIndex = 23;
            this.chkIsPairingAccount.Text = "Is Pairing Account";
            this.toolTip1.SetToolTip(this.chkIsPairingAccount, "Pairing accounts are assigned temporary passwords that MUST be turned into keys o" +
        "n their first login attempt. After a single bad password attempt, the account is" +
        " locked out.");
            this.chkIsPairingAccount.UseVisualStyleBackColor = true;
            // 
            // chkIsDisabled
            // 
            this.chkIsDisabled.AutoSize = true;
            this.chkIsDisabled.Location = new System.Drawing.Point(396, 113);
            this.chkIsDisabled.Name = "chkIsDisabled";
            this.chkIsDisabled.Size = new System.Drawing.Size(78, 17);
            this.chkIsDisabled.TabIndex = 24;
            this.chkIsDisabled.Text = "Is Disabled";
            this.toolTip1.SetToolTip(this.chkIsDisabled, "Accounts can automaically be disabled if Must Rotate Key is checked and an invali" +
        "d attempt occurs.");
            this.chkIsDisabled.UseVisualStyleBackColor = true;
            // 
            // txtVerifier
            // 
            this.txtVerifier.Location = new System.Drawing.Point(397, 156);
            this.txtVerifier.Multiline = true;
            this.txtVerifier.Name = "txtVerifier";
            this.txtVerifier.ReadOnly = true;
            this.txtVerifier.Size = new System.Drawing.Size(381, 269);
            this.txtVerifier.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(394, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Alias Name";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(460, 47);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(318, 20);
            this.txtAlias.TabIndex = 26;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(460, 73);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(318, 20);
            this.txtDescription.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(394, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Description";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.createPasswordFileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // createPasswordFileToolStripMenuItem
            // 
            this.createPasswordFileToolStripMenuItem.Name = "createPasswordFileToolStripMenuItem";
            this.createPasswordFileToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.createPasswordFileToolStripMenuItem.Text = "Create Key File";
            this.createPasswordFileToolStripMenuItem.Click += new System.EventHandler(this.createKeyFileToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(396, 431);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "Import";
            this.toolTip1.SetToolTip(this.button1, "Imports the public key/verifier that was generated by another client.");
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(477, 431);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 31;
            this.button2.Text = "Export";
            this.toolTip1.SetToolTip(this.button2, "Exports the public key/verifier so it can be transported to another system.");
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(703, 431);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 32;
            this.button3.Text = "Change";
            this.toolTip1.SetToolTip(this.button3, "Specifies a user supplied password.");
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(622, 431);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 33;
            this.button4.Text = "Generate";
            this.toolTip1.SetToolTip(this.button4, "Generates a new key file and exports the private key data to a file.");
            this.button4.UseVisualStyleBackColor = true;
            // 
            // FrmAccountManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 498);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkIsDisabled);
            this.Controls.Add(this.chkIsPairingAccount);
            this.Controls.Add(this.txtVerifier);
            this.Controls.Add(this.chkIsAuthenticationService);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemoveRoles);
            this.Controls.Add(this.btnAddRoles);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRemoveAccount);
            this.Controls.Add(this.btnAddAccount);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmAccountManager";
            this.Text = "Account Manager";
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstAccounts;
        private System.Windows.Forms.Button btnRemoveAccount;
        private System.Windows.Forms.Button btnRemoveRoles;
        private System.Windows.Forms.Button btnAddRoles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstRoles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkIsAuthenticationService;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtVerifier;
        private System.Windows.Forms.CheckBox chkIsPairingAccount;
        private System.Windows.Forms.CheckBox chkIsDisabled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createPasswordFileToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

