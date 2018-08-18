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
using Microsoft.VisualBasic;

namespace CredentialManager
{
    public partial class FrmEditAccount : Form
    {
        public FrmEditAccount(CtpAccount account)
        {
            InitializeComponent();
            LoadData(account);
        }


        private void LoadData(CtpAccount account)
        {
            chkIsEnabled.Checked = account.IsEnabled;
            txtName.Text = account.Name;
            txtDescription.Text = account.Description;
            lstTrustedIPs.Items.Clear();
            if (account.TrustedIPs != null)
            {
                foreach (var item in account.TrustedIPs)
                {
                    lstTrustedIPs.Items.Add(item);
                }
            }
            lstRoles.Items.Clear();
            if (account.Roles != null)
            {
                foreach (var item in account.Roles)
                {
                    lstRoles.Items.Add(item);
                }
            }
            lstCertificates.Items.Clear();
            if (account.ClientCerts != null)
            {
                foreach (var item in account.ClientCerts)
                {
                    lstCertificates.Items.Add(item);
                }
            }
        }

        public CtpAccount SaveData()
        {
            var rv = new CtpAccount();
            rv.IsEnabled = chkIsEnabled.Checked;
            rv.Name = txtName.Text;
            rv.Description = txtDescription.Text;
            rv.TrustedIPs = new List<IpAndMask>(lstTrustedIPs.Items.Cast<IpAndMask>());
            rv.Roles = new List<string>(lstRoles.Items.Cast<string>());
            rv.ClientCerts = new List<CtpClientCert>(lstCertificates.Items.Cast<CtpClientCert>());
            return rv;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnAddTrustedIP_Click(object sender, EventArgs e)
        {
            lstTrustedIPs.Items.Add(new IpAndMask() { IpAddress = "127.0.0.1", MaskBits = 32 });

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

        private void btnAddRoles_Click(object sender, EventArgs e)
        {
            string data = Interaction.InputBox("Role Name", "Role");
            if (data.Length > 0)
                lstRoles.Items.Add(data);
        }

        private void btnEditRoles_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }
            string data = Interaction.InputBox("Edit Role Name", "Role", (string)lstRoles.SelectedItem);
            if (data.Length > 0)
                lstRoles.Items[lstRoles.SelectedIndex] = data;
        }

        private void btnRemoveRoles_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }
            lstRoles.Items.Remove(lstRoles.SelectedItem);
        }

        private void btnAddCertificates_Click(object sender, EventArgs e)
        {
            lstCertificates.Items.Add(new CtpClientCert());
        }

        private void btnEditCertificates_Click(object sender, EventArgs e)
        {
            if (lstCertificates.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }

            using (var dlg = new FrmClients((CtpClientCert)lstCertificates.SelectedItem))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    lstCertificates.Items[lstCertificates.SelectedIndex] = dlg.SaveData();
                }
            }

        }

        private void btnRemoveCertificates_Click(object sender, EventArgs e)
        {
            if (lstCertificates.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }
            lstCertificates.Items.Remove(lstCertificates.SelectedItem);
        }
    }
}

