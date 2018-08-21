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
            this.chkSupportsTickets = new System.Windows.Forms.CheckBox();
            this.btnBrowseCertificate = new System.Windows.Forms.Button();
            this.txtCertPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkIsEnabled = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowsePin = new System.Windows.Forms.Button();
            this.TxtPairingPinPath = new System.Windows.Forms.TextBox();
            this.btnGeneratePin = new System.Windows.Forms.Button();
            this.lblPin = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkSupportsTickets
            // 
            this.chkSupportsTickets.AutoSize = true;
            this.chkSupportsTickets.Location = new System.Drawing.Point(117, 12);
            this.chkSupportsTickets.Name = "chkSupportsTickets";
            this.chkSupportsTickets.Size = new System.Drawing.Size(155, 17);
            this.chkSupportsTickets.TabIndex = 52;
            this.chkSupportsTickets.Text = "Permit Proxy Authentication";
            this.chkSupportsTickets.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCertificate
            // 
            this.btnBrowseCertificate.Location = new System.Drawing.Point(386, 61);
            this.btnBrowseCertificate.Name = "btnBrowseCertificate";
            this.btnBrowseCertificate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCertificate.TabIndex = 51;
            this.btnBrowseCertificate.Text = "Browse";
            this.btnBrowseCertificate.UseVisualStyleBackColor = true;
            this.btnBrowseCertificate.Click += new System.EventHandler(this.btnBrowseCertificate_Click);
            // 
            // txtCertPath
            // 
            this.txtCertPath.Location = new System.Drawing.Point(102, 63);
            this.txtCertPath.Name = "txtCertPath";
            this.txtCertPath.Size = new System.Drawing.Size(278, 20);
            this.txtCertPath.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Certificate Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(102, 35);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(278, 20);
            this.txtName.TabIndex = 47;
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.AutoSize = true;
            this.chkIsEnabled.Location = new System.Drawing.Point(12, 12);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(76, 17);
            this.chkIsEnabled.TabIndex = 45;
            this.chkIsEnabled.Text = "Is Enabled";
            this.chkIsEnabled.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Pairing Pin Path";
            // 
            // btnBrowsePin
            // 
            this.btnBrowsePin.Location = new System.Drawing.Point(386, 89);
            this.btnBrowsePin.Name = "btnBrowsePin";
            this.btnBrowsePin.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePin.TabIndex = 55;
            this.btnBrowsePin.Text = "Browse";
            this.btnBrowsePin.UseVisualStyleBackColor = true;
            this.btnBrowsePin.Click += new System.EventHandler(this.btnBrowsePin_Click);
            // 
            // TxtPairingPinPath
            // 
            this.TxtPairingPinPath.Location = new System.Drawing.Point(102, 91);
            this.TxtPairingPinPath.Name = "TxtPairingPinPath";
            this.TxtPairingPinPath.Size = new System.Drawing.Size(278, 20);
            this.TxtPairingPinPath.TabIndex = 54;
            // 
            // btnGeneratePin
            // 
            this.btnGeneratePin.Location = new System.Drawing.Point(467, 88);
            this.btnGeneratePin.Name = "btnGeneratePin";
            this.btnGeneratePin.Size = new System.Drawing.Size(75, 23);
            this.btnGeneratePin.TabIndex = 56;
            this.btnGeneratePin.Text = "Generate";
            this.btnGeneratePin.UseVisualStyleBackColor = true;
            this.btnGeneratePin.Click += new System.EventHandler(this.btnGeneratePin_Click);
            // 
            // lblPin
            // 
            this.lblPin.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPin.Location = new System.Drawing.Point(12, 130);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(521, 156);
            this.lblPin.TabIndex = 57;
            this.lblPin.Text = "PIN: Missing";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(458, 289);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmClientCertificates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 324);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPin);
            this.Controls.Add(this.btnGeneratePin);
            this.Controls.Add(this.btnBrowsePin);
            this.Controls.Add(this.TxtPairingPinPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkSupportsTickets);
            this.Controls.Add(this.btnBrowseCertificate);
            this.Controls.Add(this.txtCertPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.chkIsEnabled);
            this.Name = "FrmClientCertificates";
            this.Text = "Client Certificate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSupportsTickets;
        private System.Windows.Forms.Button btnBrowseCertificate;
        private System.Windows.Forms.TextBox txtCertPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkIsEnabled;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowsePin;
        private System.Windows.Forms.TextBox TxtPairingPinPath;
        private System.Windows.Forms.Button btnGeneratePin;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Button btnOK;
    }
}