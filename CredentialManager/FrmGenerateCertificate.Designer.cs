namespace CredentialManager
{
    partial class FrmGenerateCertificate
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.textBoxCommonName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCreateCertificate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.CmbKeyStrengths = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "End Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Start Date";
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(98, 69);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerEndDate.TabIndex = 27;
            this.toolTip.SetToolTip(this.dateTimePickerEndDate, "Certificate End");
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(98, 43);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerStartDate.TabIndex = 26;
            this.toolTip.SetToolTip(this.dateTimePickerStartDate, "Certificate Start");
            // 
            // textBoxCommonName
            // 
            this.textBoxCommonName.Location = new System.Drawing.Point(98, 17);
            this.textBoxCommonName.Name = "textBoxCommonName";
            this.textBoxCommonName.Size = new System.Drawing.Size(200, 20);
            this.textBoxCommonName.TabIndex = 25;
            this.toolTip.SetToolTip(this.textBoxCommonName, "the common name (CN) of the cert. Do not include CN=.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Common Name";
            // 
            // buttonCreateCertificate
            // 
            this.buttonCreateCertificate.Location = new System.Drawing.Point(180, 148);
            this.buttonCreateCertificate.Name = "buttonCreateCertificate";
            this.buttonCreateCertificate.Size = new System.Drawing.Size(118, 23);
            this.buttonCreateCertificate.TabIndex = 23;
            this.buttonCreateCertificate.Text = "Create Certificate";
            this.buttonCreateCertificate.UseVisualStyleBackColor = true;
            this.buttonCreateCertificate.Click += new System.EventHandler(this.buttonCreateCertificate_Click);
            // 
            // CmbKeyStrengths
            // 
            this.CmbKeyStrengths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbKeyStrengths.FormattingEnabled = true;
            this.CmbKeyStrengths.Location = new System.Drawing.Point(98, 95);
            this.CmbKeyStrengths.Name = "CmbKeyStrengths";
            this.CmbKeyStrengths.Size = new System.Drawing.Size(200, 21);
            this.CmbKeyStrengths.TabIndex = 32;
            this.toolTip.SetToolTip(this.CmbKeyStrengths, "Recommended: 2048. \r\n1024 only if in a trusted environment where encryption is op" +
        "tional and high speed key exchange is desired. \r\n4096 will take minutes to gener" +
        "ate.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "PFX Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(98, 122);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.TabIndex = 33;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(114, 148);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 23);
            this.buttonCancel.TabIndex = 31;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Key Strength";
            // 
            // FrmGenerateCertificate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 183);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.dateTimePickerStartDate);
            this.Controls.Add(this.textBoxCommonName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCreateCertificate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.CmbKeyStrengths);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label8);
            this.Name = "FrmGenerateCertificate";
            this.Text = "Generate Certificate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.TextBox textBoxCommonName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCreateCertificate;
        private System.Windows.Forms.ComboBox CmbKeyStrengths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label8;
    }
}