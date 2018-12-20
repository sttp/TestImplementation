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
                    var cfg = CommandObject<CtpServerConfig>.Load(new CtpCommand(allData));
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
            
            lstAnonymousAccountMapping.Items.Clear();
            if (config.AnonymousMappings != null)
            {
                foreach (var option in config.AnonymousMappings)
                {
                    lstAnonymousAccountMapping.Items.Add(option);
                }
            }
            lstCertificates.Items.Clear();
            if (config.ClientCerts != null)
            {
                foreach (var item in config.ClientCerts)
                {
                    lstCertificates.Items.Add(item);
                }
            }


            chkEnableSSL.Checked = config.EnableSSL;
            txtLocalCertPath.Text = config.ServerCertificatePath;
            txtLocalCertPassword.Text = config.CertificatePassword;
        }

        private CtpServerConfig SaveData()
        {
            var cfg = new CtpServerConfig();
            cfg.Accounts = new List<CtpAccount>(lstAccounts.Items.Cast<CtpAccount>());
            cfg.AnonymousMappings = new List<CtpAnonymousMapping>(lstAnonymousAccountMapping.Items.Cast<CtpAnonymousMapping>());
            cfg.ClientCerts = new List<CtpClientCert>(lstCertificates.Items.Cast<CtpClientCert>());
            cfg.EnableSSL = chkEnableSSL.Checked;
            cfg.ServerCertificatePath = txtLocalCertPath.Text;
            cfg.CertificatePassword = txtLocalCertPassword.Text;
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

        private void btnAddAnonomousAccountMapping_Click(object sender, EventArgs e)
        {
            lstAnonymousAccountMapping.Items.Add(new CtpAnonymousMapping());

        }

        private void lstAnonymousAccountMapping_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstAnonymousAccountMapping.SelectedItem == null)
            {
                MessageBox.Show("Select an item to edit");
                return;
            }

            using (var frm = new FrmAnonymousMapping((CtpAnonymousMapping)lstAnonymousAccountMapping.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstAnonymousAccountMapping.Items[lstAnonymousAccountMapping.SelectedIndex] = frm.SaveData();
                }
            }
        }

        private void lstAnonymousAccountMapping_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstAnonymousAccountMapping.SelectedItem == null)
                {
                    MessageBox.Show("Select an item to remove");
                    return;
                }
                lstAnonymousAccountMapping.Items.Remove(lstAnonymousAccountMapping.SelectedItem);
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstAnonymousAccountMapping_MouseDoubleClick(null, null);
            }
        }

        private void btnAddCertificates_Click(object sender, EventArgs e)
        {
            lstCertificates.Items.Add(new CtpClientCert());
        }

        private void lstCertificates_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstCertificates.SelectedItem == null)
                {
                    MessageBox.Show("Select and item");
                    return;
                }
                lstCertificates.Items.Remove(lstCertificates.SelectedItem);

            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstCertificates_MouseDoubleClick(null, null);
            }
        }

        private void lstCertificates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstCertificates.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }

            using (var dlg = new FrmClientCertificates((CtpClientCert)lstCertificates.SelectedItem))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    lstCertificates.Items[lstCertificates.SelectedIndex] = dlg.SaveData();
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
