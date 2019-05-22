using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CTP;
using CTP.Net;

namespace CredentialManager
{
    public partial class FrmAccountManager : Form
    {
        public FrmAccountManager()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            LoadData(new CtpServerConfig());
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Sttp Config File|*.cdf";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    byte[] allData = File.ReadAllBytes(dlg.FileName);
                    var cfg = (CtpServerConfig)new CtpCommand(allData);
                    LoadData(cfg);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Sttp Config File|*.cdf";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var cfg = SaveData();
                    File.WriteAllBytes(dlg.FileName, cfg.ToCommand().ToArray());
                }
            }
        }

        private void LoadData(CtpServerConfig config)
        {
            lstAccounts.Items.Clear();
            if (config.Accounts != null)
            {
                foreach (var account in config.Accounts)
                {
                    lstAccounts.Items.Add(account);
                }
            }
            
            chkEnableSSL.Checked = config.EnableSSL;
            txtLocalCertPath.Text = config.ServerCertificatePath;
            txtLocalCertPassword.Text = config.CertificatePassword;
            chkUseEphemeralCertificates.Checked = config.UseEphemeralCertificates;

        }

        private CtpServerConfig SaveData()
        {
            var cfg = new CtpServerConfig();
            cfg.Accounts = new List<CtpAccount>(lstAccounts.Items.Cast<CtpAccount>());
            cfg.EnableSSL = chkEnableSSL.Checked;
            cfg.ServerCertificatePath = txtLocalCertPath.Text;
            cfg.CertificatePassword = txtLocalCertPassword.Text;
            cfg.UseEphemeralCertificates = chkUseEphemeralCertificates.Checked;
            return cfg;
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            lstAccounts.Items.Add(new CtpAccount() { Name = "New Account" });
        }

        private void lstAccounts_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstAccounts.SelectedItem == null)
                {
                    MessageBox.Show("Select an item to remove");
                    return;
                }
                lstAccounts.Items.Remove(lstAccounts.SelectedItem);
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstAccounts_MouseDoubleClick(null, null);
            }
        }

        private void lstAccounts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstAccounts.SelectedItem == null)
            {
                MessageBox.Show("Select an item to edit");
                return;
            }

            using (var frm = new FrmAccount((CtpAccount)lstAccounts.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstAccounts.Items[lstAccounts.SelectedIndex] = frm.SaveData();
                }
            }
        }

        private void btnBrowseLocalCertificate_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "*.cer|Certificate|*.pfx|Certificate Container PFX";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLocalCertPath.Text = dlg.FileName;
                }
            }
        }
    }
}
