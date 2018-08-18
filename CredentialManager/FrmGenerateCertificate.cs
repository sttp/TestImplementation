using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSF.Security.Cryptography.X509;

namespace CredentialManager
{
    public partial class FrmGenerateCertificate : Form
    {
        private class CertificateOption
        {
            public string CommonName { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime StartDate { get; set; }
            public CertificateSigningMode KeyType { get; set; }
            public short SignatureBit { get; set; }

            public CertificateOption()
            {
                KeyType = CertificateSigningMode.RSA_2048_SHA2_256;
                CommonName = Environment.MachineName;
                EndDate = DateTime.Now.Date.AddYears(3);
                StartDate = DateTime.Now.Date.AddDays(-1);
            }

            public bool Validate()
            {
                return ValidateStartEndDate() && ValidateCommonName();
            }

            private bool ValidateCommonName()
            {
                if (string.IsNullOrEmpty(CommonName))
                {
                    MessageBox.Show("Common name is empty.");
                    return false;
                }
                return true;
            }


            private bool ValidateStartEndDate()
            {
                if (EndDate < StartDate)
                {
                    MessageBox.Show("Start date is before end date.");
                    return false;
                }
                return true;
            }

        }

        CertificateOption m_opt = new CertificateOption();

        public FrmGenerateCertificate()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetBindings();
        }

        private void SetBindings()
        {
            textBoxCommonName.DataBindings.Add("Text", m_opt, "CommonName", false, DataSourceUpdateMode.OnPropertyChanged);

            dateTimePickerStartDate.DataBindings.Add("Value", m_opt, "StartDate", false, DataSourceUpdateMode.OnPropertyChanged);
            dateTimePickerEndDate.DataBindings.Add("Value", m_opt, "EndDate", false, DataSourceUpdateMode.OnPropertyChanged);

            CmbKeyStrengths.DataSource = new BindingSource { DataSource = Enum.GetValues(typeof(CertificateSigningMode)) };
            CmbKeyStrengths.DataBindings.Add("SelectedItem", m_opt, "KeyType", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public string CertFile;

        private void buttonCreateCertificate_Click(object sender, EventArgs e)
        {
            if (m_opt.Validate())
            {
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "Key File|*.pfx";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (var cert = CertificateMaker.GenerateSelfSignedCertificate(m_opt.KeyType, m_opt.CommonName, m_opt.StartDate, m_opt.EndDate))
                        {
                            File.WriteAllBytes(dlg.FileName, cert.Export(X509ContentType.Pkcs12, txtPassword.Text));
                            CertFile = Path.ChangeExtension(dlg.FileName, ".cer");
                            File.WriteAllBytes(CertFile, cert.Export(X509ContentType.Cert));
                        }
                        MessageBox.Show("Done!");
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid");
            }
        }



        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
