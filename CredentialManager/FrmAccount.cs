using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CTP.Net;
using Microsoft.VisualBasic;

namespace CredentialManager
{
    public partial class FrmAccount : Form
    {
        public FrmAccount(CtpAccount account)
        {
            InitializeComponent();
            LoadData(account);
        }

        private void LoadData(CtpAccount account)
        {
            chkIsEnabled.Checked = account.IsEnabled;
            txtName.Text = account.Name;
            txtDescription.Text = account.Description;
            txtCertificateDirectory.Text = account.CertificateDirectory ?? string.Empty;
            lstTrustedIPs.Items.Clear();
            if (account.AllowedRemoteIPs != null)
            {
                foreach (var item in account.AllowedRemoteIPs)
                {
                    lstTrustedIPs.Items.Add(item);
                }
            }

            lstImplicitRoles.Items.Clear();
            if (account.ImplicitRoles != null)
            {
                foreach (var item in account.ImplicitRoles)
                {
                    lstImplicitRoles.Items.Add(item);
                }
            }

            lstExplicitRoles.Items.Clear();
            if (account.ExplicitRoles != null)
            {
                foreach (var item in account.ExplicitRoles)
                {
                    lstExplicitRoles.Items.Add(item);
                }
            }

        }

        public CtpAccount SaveData()
        {
            var rv = new CtpAccount();
            rv.IsEnabled = chkIsEnabled.Checked;
            rv.Name = txtName.Text;
            rv.Description = txtDescription.Text;
            rv.CertificateDirectory = txtCertificateDirectory.Text;
            rv.AllowedRemoteIPs = new List<IpAndMask>(lstTrustedIPs.Items.Cast<IpAndMask>());
            rv.ImplicitRoles = new List<string>(lstImplicitRoles.Items.Cast<string>());
            rv.ExplicitRoles = new List<string>(lstExplicitRoles.Items.Cast<string>());
            return rv;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnAddExplicitRoles_Click(object sender, EventArgs e)
        {
            string data = Interaction.InputBox("Role Name", "Role");
            if (data.Length > 0)
                lstExplicitRoles.Items.Add(data);
        }

        private void btnAddRoles_Click(object sender, EventArgs e)
        {
            string data = Interaction.InputBox("Role Name", "Role");
            if (data.Length > 0)
                lstImplicitRoles.Items.Add(data);
        }

        private void lstRoles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox lst = sender as ListBox;
            if (lst.SelectedItem == null)
            {
                MessageBox.Show("Select and item");
                return;
            }
            string data = Interaction.InputBox("Edit Role Name", "Role", (string)lst.SelectedItem);
            if (data.Length > 0)
                lst.Items[lst.SelectedIndex] = data;
        }

        private void lstRoles_KeyUp(object sender, KeyEventArgs e)
        {
            ListBox lst = sender as ListBox;
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (lst.SelectedItem == null)
                {
                    MessageBox.Show("Select and item");
                    return;
                }
                lst.Items.Remove(lst.SelectedItem);

            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                lstRoles_MouseDoubleClick(sender, null);
            }
        }

        private void btnAddTrustedIP_Click(object sender, EventArgs e)
        {
            lstTrustedIPs.Items.Add(new IpAndMask() { IpAddress = "127.0.0.1", MaskBits = 32 });
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

       
    }
}

