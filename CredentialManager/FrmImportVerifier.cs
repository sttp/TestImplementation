using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTP;
using CTP.SRP;

namespace AuthenticationServer
{
    public partial class FrmImportVerifier : Form
    {
        public FrmImportVerifier()
        {
            InitializeComponent();
        }

        private void btnFromPassword_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() != txtPassword.Text)
            {
                MessageBox.Show("Password must not contain leading or trailing spaces");
                return;
            }
            if (txtPassword.Text.Normalize(NormalizationForm.FormKC) != txtPassword.Text)
            {
                MessageBox.Show("Password is ambiguous since it's typed form is not a normalized form");
                return;
            }



        }

        private void btnFromFile_Click(object sender, EventArgs e)
        {

        }

    }

  
}
