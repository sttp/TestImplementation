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
        }

        private CtpServerConfig SaveData()
        {
            var cfg = new CtpServerConfig();
            cfg.Accounts = new List<CtpAccount>(lstAccounts.Items.Cast<CtpAccount>());
            cfg.InterfaceOptions = new List<CtpInterfaceOptions>(lstInterfaceOptions.Items.Cast<CtpInterfaceOptions>());
            return cfg;
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            lstAccounts.Items.Add(new CtpAccount() { Name = "New Account" });
        }

        private void btnEditAccounts_Click(object sender, EventArgs e)
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

        private void btnRemoveAccounts_Click(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem == null)
            {
                MessageBox.Show("Select an item to remove");
                return;
            }
            lstAccounts.Items.Remove(lstAccounts.SelectedItem);
        }


        private void btnInterfaceOptions_Click(object sender, EventArgs e)
        {
            lstInterfaceOptions.Items.Add(new CtpInterfaceOptions());

        }

        private void btnEdit_Click(object sender, EventArgs e)
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstInterfaceOptions.SelectedItem == null)
            {
                MessageBox.Show("Select an item to remove");
                return;
            }
            lstInterfaceOptions.Items.Remove(lstInterfaceOptions.SelectedItem);
        }


    }
}
