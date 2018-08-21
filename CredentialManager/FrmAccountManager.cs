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
                    var cfg = DocumentObject<CtpServerConfig>.Load(new CtpDocument(allData));
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
                    File.WriteAllBytes(dlg.FileName, cfg.ToDocument().ToArray());
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
            lstInterfaceOptions.Items.Clear();
            if (config.InterfaceOptions != null)
            {
                foreach (var option in config.InterfaceOptions)
                {
                    lstInterfaceOptions.Items.Add(option);
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
        }

        private CtpServerConfig SaveData()
        {
            var cfg = new CtpServerConfig();
            cfg.Accounts = new List<CtpAccount>(lstAccounts.Items.Cast<CtpAccount>());
            cfg.InterfaceOptions = new List<CtpInterfaceOptions>(lstInterfaceOptions.Items.Cast<CtpInterfaceOptions>());
            cfg.AnonymousMappings = new List<CtpAnonymousMapping>(lstAnonymousAccountMapping.Items.Cast<CtpAnonymousMapping>());
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

            using (var frm = new FrmEditAccount((CtpAccount)lstAccounts.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstAccounts.Items[lstAccounts.SelectedIndex] = frm.SaveData();
                }
            }
        }

        private void btnInterfaceOptions_Click(object sender, EventArgs e)
        {
            lstInterfaceOptions.Items.Add(new CtpInterfaceOptions());
        }
       
        private void lstInterfaceOptions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstInterfaceOptions.SelectedItem == null)
            {
                MessageBox.Show("Select an item to edit");
                return;
            }

            using (var frm = new FrmInterfaceOptions((CtpInterfaceOptions)lstInterfaceOptions.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstInterfaceOptions.Items[lstInterfaceOptions.SelectedIndex] = frm.SaveData();
                }
            }
        }

        private void lstInterfaceOptions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstInterfaceOptions.SelectedItem == null)
                {
                    MessageBox.Show("Select an item to remove");
                    return;
                }
                lstInterfaceOptions.Items.Remove(lstInterfaceOptions.SelectedItem);
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstInterfaceOptions_MouseDoubleClick(null, null);
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

            using (var frm = new FrmEditAnonymousMapping((CtpAnonymousMapping)lstAnonymousAccountMapping.SelectedItem))
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
        
    }
}
