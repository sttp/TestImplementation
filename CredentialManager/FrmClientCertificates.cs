using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CTP.Net;

namespace CredentialManager
{
    public partial class FrmClientCertificates : Form
    {
        public FrmClientCertificates(CtpClientCert data)
        {
            InitializeComponent();

            chkIsEnabled.Checked = data.IsEnabled;
            chkSupportsTickets.Checked = data.SupportsTickets;
            txtName.Text = data.CertificateName;
            txtCertPath.Text = data.CertificatePath;
            TxtPairingPinPath.Text = data.PairingPinPath;
            if (File.Exists(TxtPairingPinPath.Text))
            {
                lblPin.Text = "PIN: Valid";
            }
            else
            {
                lblPin.Text = "PIN: Missing";
            }
            if (data.AccessList != null)
            {
                foreach (var item in data.AccessList)
                {
                    lstTrustedIPs.Items.Add(item);
                }
            }
        }

        public CtpClientCert SaveData()
        {
            var data = new CtpClientCert();
            data.IsEnabled = chkIsEnabled.Checked;
            data.SupportsTickets = chkSupportsTickets.Checked;
            data.CertificateName = txtName.Text;
            data.CertificatePath = txtCertPath.Text;
            data.PairingPinPath = TxtPairingPinPath.Text;
            data.AccessList = new List<IpAndMask>(lstTrustedIPs.Items.Cast<IpAndMask>());
            return data;
        }

        private void btnAddTrustedIP_Click(object sender, EventArgs e)
        {
            lstTrustedIPs.Items.Add(new IpAndMask() { IpAddress = "0.0.0.0", MaskBits = 0 });
        }

        private void btnEditTrustedIP_Click(object sender, EventArgs e)
        {
            if (lstTrustedIPs.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }

            using (var frm = new EditAccessList((IpAndMask)lstTrustedIPs.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstTrustedIPs.Items[lstTrustedIPs.SelectedIndex] = frm.SaveData();
                }
            }
        }

        private void btnRemoveTrustedIPs_Click(object sender, EventArgs e)
        {
            if (lstTrustedIPs.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }
            lstTrustedIPs.Items.Remove(lstTrustedIPs.SelectedItem);
        }

        private void btnBrowseCertificate_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Certificate|*.cer";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtCertPath.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowsePin_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "STTP Pairing Pin|*.pin";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    TxtPairingPinPath.Text = dlg.FileName;
                }
            }
        }

        private void btnGeneratePin_Click(object sender, EventArgs e)
        {
            string validSymbols = "FHILPQRSUWXY0123456789";

            StringBuilder pin = new StringBuilder();

            while (pin.Length < 6)
            {
                byte[] code = new byte[100];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(code);
                    foreach (var c in code)
                    {
                        if (c < validSymbols.Length)
                        {
                            pin.Append(validSymbols[c]);
                            if (pin.Length == 6)
                                break;
                        }
                    }
                }
            }

            lblPin.Text = "PIN: " + pin.ToString() + Environment.NewLine
                          + "Valid for 5 minutes" + Environment.NewLine
                          + "PIN Entropy: " + Math.Log(Math.Pow(validSymbols.Length, pin.Length), 2).ToString("N0") + "bits";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
