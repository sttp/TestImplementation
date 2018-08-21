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
           
            lstRoles.Items.Clear();
            if (account.Roles != null)
            {
                foreach (var item in account.Roles)
                {
                    lstRoles.Items.Add(item);
                }
            }
           
        }

        public CtpAccount SaveData()
        {
            var rv = new CtpAccount();
            rv.IsEnabled = chkIsEnabled.Checked;
            rv.Name = txtName.Text;
            rv.Description = txtDescription.Text;
            rv.Roles = new List<string>(lstRoles.Items.Cast<string>());
            return rv;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
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

       

       
    }
}

