using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.SqlServer.Server;

namespace SqlServerAuthorizationCLR
{
    public static class SignEphemeralCertificate
    {
        [SqlProcedure()]
        public static void Sign(out byte[] ephemeralCertificate, byte[] signingCertificate, string contentsQuery)
        {
            var ticketValues = new DataTable();

            if (string.IsNullOrWhiteSpace(contentsQuery))
                throw new Exception("Ticket cannot be null");

            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                var ta = new SqlDataAdapter(contentsQuery, connection);
                ta.Fill(ticketValues);
                connection.Close();
            }

            var cert = Find(signingCertificate);
            ephemeralCertificate = EphemeralCertificate.SignCertificate(cert, ticketValues);
        }


        [SqlProcedure()]
        public static void OpenCertificate(out byte[] data, out byte[] thumbprint, out string contents, string certificatePath)
        {
            data = File.ReadAllBytes(certificatePath);
            var cert = new X509Certificate2(data);
            thumbprint = cert.GetCertHash();
            contents = cert.ToString(true);
        }

        private static X509Certificate2 Find(byte[] signingCertificate)
        {
            var crt = new X509Certificate2(signingCertificate);
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            foreach (var cert in store.Certificates)
            {
                if (cert.HasPrivateKey)
                {
                    if (cert.Thumbprint == crt.Thumbprint)
                    {
                        return cert;
                    }
                }
            }
            throw new Exception("Certificate Not Found");
        }
      
    }





}
