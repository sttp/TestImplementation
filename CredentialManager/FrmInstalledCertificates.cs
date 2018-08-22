using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTP.Net;

namespace CredentialManager
{
    public partial class FrmInstalledCertificates : Form
    {
        public FrmInstalledCertificates(CtpInstalledCertificates option)
        {
            InitializeComponent();

            chkIsEnabled.Checked = option.IsEnabled;
            chkDisableSSL.Checked = option.EnableSSL;
            txtName.Text = option.Name;
            txtCertPath.Text = option.CertificatePath;
            if (option.RemoteIPs != null)
            {
                foreach (var ip in option.RemoteIPs)
                {
                    lstTrustedIPs.Items.Add(ip);
                }
            }
        }

        public CtpInstalledCertificates SaveData()
        {
            var rv = new CtpInstalledCertificates();
            rv.IsEnabled = chkIsEnabled.Checked;
            rv.EnableSSL = chkDisableSSL.Checked;
            rv.Name = txtName.Text;
            rv.CertificatePath = txtCertPath.Text;
            rv.RemoteIPs = new List<IpAndMask>(lstTrustedIPs.Items.Cast<IpAndMask>());
            return rv;
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

        private void btnAddTrustedIP_Click(object sender, EventArgs e)
        {
            lstTrustedIPs.Items.Add(new IpAndMask() { IpAddress = "0.0.0.0" });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmGenerateCertificate())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtCertPath.Text = frm.CertFile;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
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

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

       
    }
}
