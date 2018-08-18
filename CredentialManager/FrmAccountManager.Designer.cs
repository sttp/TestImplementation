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
            this.btnRemoveAccounts = new System.Windows.Forms.Button();
            this.btnEditAccounts = new System.Windows.Forms.Button();
            this.lstAccounts = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lstInterfaceOptions = new System.Windows.Forms.ListBox();
            this.btnInterfaceOptions = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddAccount.Location = new System.Drawing.Point(10, 130);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(75, 23);
            this.btnAddAccount.TabIndex = 12;
            this.btnAddAccount.Text = "Add";
            this.btnAddAccount.UseVisualStyleBackColor = true;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRemoveAccounts);
            this.groupBox3.Controls.Add(this.btnEditAccounts);
            this.groupBox3.Controls.Add(this.lstAccounts);
            this.groupBox3.Controls.Add(this.btnAddAccount);
            this.groupBox3.Location = new System.Drawing.Point(12, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(690, 165);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accounts";
            // 
            // btnRemoveAccounts
            // 
            this.btnRemoveAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveAccounts.Location = new System.Drawing.Point(172, 130);
            this.btnRemoveAccounts.Name = "btnRemoveAccounts";
            this.btnRemoveAccounts.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAccounts.TabIndex = 14;
            this.btnRemoveAccounts.Text = "Remove";
            this.btnRemoveAccounts.UseVisualStyleBackColor = true;
            this.btnRemoveAccounts.Click += new System.EventHandler(this.btnRemoveAccounts_Click);
            // 
            // btnEditAccounts
            // 
            this.btnEditAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditAccounts.Location = new System.Drawing.Point(91, 130);
            this.btnEditAccounts.Name = "btnEditAccounts";
            this.btnEditAccounts.Size = new System.Drawing.Size(75, 23);
            this.btnEditAccounts.TabIndex = 13;
            this.btnEditAccounts.Text = "Edit";
            this.btnEditAccounts.UseVisualStyleBackColor = true;
            this.btnEditAccounts.Click += new System.EventHandler(this.btnEditAccounts_Click);
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
            this.lstAccounts.Size = new System.Drawing.Size(661, 108);
            this.lstAccounts.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnRemove);
            this.groupBox6.Controls.Add(this.btnEdit);
            this.groupBox6.Controls.Add(this.lstInterfaceOptions);
            this.groupBox6.Controls.Add(this.btnInterfaceOptions);
            this.groupBox6.Location = new System.Drawing.Point(12, 212);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(690, 165);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Interface Options";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(172, 130);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 14;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Location = new System.Drawing.Point(91, 130);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 13;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
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
            this.lstInterfaceOptions.Size = new System.Drawing.Size(661, 108);
            this.lstInterfaceOptions.TabIndex = 0;
            // 
            // btnInterfaceOptions
            // 
            this.btnInterfaceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInterfaceOptions.Location = new System.Drawing.Point(10, 130);
            this.btnInterfaceOptions.Name = "btnInterfaceOptions";
            this.btnInterfaceOptions.Size = new System.Drawing.Size(75, 23);
            this.btnInterfaceOptions.TabIndex = 12;
            this.btnInterfaceOptions.Text = "Add";
            this.btnInterfaceOptions.UseVisualStyleBackColor = true;
            this.btnInterfaceOptions.Click += new System.EventHandler(this.btnInterfaceOptions_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(24, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 23);
            this.btnOpen.TabIndex = 14;
            this.btnOpen.Text = "Open Config";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save Config";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(236, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 23);
            this.btnNew.TabIndex = 16;
            this.btnNew.Text = "New Config";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // FrmAccountManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 393);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Name = "FrmAccountManager";
            this.Text = "Account Manager";
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstAccounts;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRemoveAccounts;
        private System.Windows.Forms.Button btnEditAccounts;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ListBox lstInterfaceOptions;
        private System.Windows.Forms.Button btnInterfaceOptions;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
    }
}

