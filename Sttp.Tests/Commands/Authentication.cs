using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CTP;
using GSF.Security.Cryptography.X509;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;

namespace Sttp.Tests.Commands
{
    [TestClass]
    public class Authentication
    {
        [TestMethod]
        public void AuthNone()
        {
            var cmd = new AuthNone();
            cmd = (AuthNone)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void Ticket()
        {
            var cmd = new AuthorizationTicket(DateTime.Parse("1/2/3456 7:08:09.1234567"),
                                 DateTime.Parse("2/2/3456 7:08:09.1234567"),
                                 "Login",
                                 new List<string>(new string[] { "Admin", "User" }),
                                 "Cert1");
            cmd = new AuthorizationTicket(cmd.ToArray());
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void Ticket2()
        {
            var cmd = new AuthorizationTicket(DateTime.Parse("1/2/3456 7:08:09.1234567"),
                                 DateTime.Parse("2/2/3456 7:08:09.1234567"),
                                 "Login",
                                 null,
                                 "Cert1");
            cmd = new AuthorizationTicket(cmd.ToArray());
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void AuthFailure()
        {
            var cmd = new AuthFailure("Access Denied");
            cmd = (AuthFailure)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void AuthSuccess()
        {
            var cmd = new AuthSuccess();
            cmd = (AuthSuccess)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        public static X509Certificate2 Cert = CertificateMaker.GenerateSelfSignedCertificate(CertificateSigningMode.RSA_3072_SHA2_256, "CN=TempCert");

        [TestMethod]
        public void Auth()
        {
            var ticket = new AuthorizationTicket(DateTime.Parse("1/2/3456 7:08:09.1234567"),
                                 DateTime.Parse("2/2/3456 7:08:09.1234567"),
                                 "Login",
                                 new List<string>(new string[] { "Admin", "User" }),
                                 "Cert1");

            var cmd = new Auth(ticket, Cert);
            cmd = (Auth)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

    }
}
