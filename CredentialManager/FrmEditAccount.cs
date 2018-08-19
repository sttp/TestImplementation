using System;
using System.Collections.Generic;
using System.Linq;
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

            using (var frm = new EditAccessList((IpAndMask)lstTrustedIPs.SelectedItem))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lstTrustedIPs.Items[lstTrustedIPs.SelectedIndex] = frm.SaveData();
                }
            }
        }

      private void btnAddRoles_Click(object sender, EventArgs e)
        {
            string data = Interaction.InputBox("Role Name", "Role");
            if (data.Length > 0)
                lstRoles.Items.Add(data);
        }

        private void lstRoles_MouseDoubleClick(object sender, MouseEventArgs e)
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

        private void lstRoles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lstRoles.SelectedItem == null)
                {
                    MessageBox.Show("Select and item");
                    return;
                }
                lstRoles.Items.Remove(lstRoles.SelectedItem);

            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstRoles_MouseDoubleClick(null, null);
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

     

       
       
    }
}

