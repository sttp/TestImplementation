using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CTP.Net;
using Microsoft.VisualBasic;

namespace CredentialManager
{
    public partial class FrmClientCertificates : Form
    {
        public FrmClientCertificates(CtpClientCert data)
        {
            InitializeComponent();

            txtName.Text = data.CertificateName;
            txtCertificateDirectory.Text = data.CertificateDirectory ?? string.Empty;
            lstTrustedIPs.Items.Clear();
            if (data.AllowedRemoteIPs != null)
            {
                foreach (var item in data.AllowedRemoteIPs)
                {
                    lstTrustedIPs.Items.Add(item);
                }
            }

            txtMappedAccount.Text = data.MappedAccount;
        }

        public CtpClientCert SaveData()
        {
            var data = new CtpClientCert();
            data.CertificateName = txtName.Text;
            data.CertificateDirectory = txtCertificateDirectory.Text;
            data.AllowedRemoteIPs = new List<IpAndMask>(lstTrustedIPs.Items.Cast<IpAndMask>());
            data.MappedAccount = txtMappedAccount.Text;

            return data;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnAddTrustedIP_Click(object sender, EventArgs e)
        {
            lstTrustedIPs.Items.Add(new IpAndMask() { IpAddress = "127.0.0.1", MaskBits = 32 });
        }

        private void lstTrustedIPs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstTrustedIPs.SelectedItem == null)
                {
                    MessageBox.Show("Select and item");
                    return;
                }
                lstTrustedIPs.Items.Remove(lstTrustedIPs.SelectedItem);

            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstTrustedIPs_MouseDoubleClick(null, null);
            }
        }

        private void lstTrustedIPs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstTrustedIPs.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }

            using (var frm = new FrmAccessList((IpAndMask)lstTrustedIPs.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstTrustedIPs.Items[lstTrustedIPs.SelectedIndex] = frm.SaveData();
                }
            }
        }
        
    }
}
