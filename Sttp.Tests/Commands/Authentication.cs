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
            //var cmd = new AuthNone();
            //cmd = (AuthNone)(CtpCommand)cmd;
            //Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void Ticket()
        {
            //var cmd = new AuthorizationTicket("SPN", DateTime.Parse("1/2/3456 7:08:09.1234567"),
            //                     DateTime.Parse("2/2/3456 7:08:09.1234567"),
            //                     "Login",
            //                     new List<string>(new string[] { "Admin", "User" }),
            //                     new byte[20]);
            //cmd = new AuthorizationTicket(cmd.ToArray());
            //Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void Ticket2()
        {
            //var cmd = new AuthorizationTicket("SPN", DateTime.Parse("1/2/3456 7:08:09.1234567"),
            //                     DateTime.Parse("2/2/3456 7:08:09.1234567"),
            //                     "Login",
            //                     null,
            //                     new byte[20]);
            //                     cmd = new AuthorizationTicket(cmd.ToArray());
            //Console.WriteLine(cmd.ToString());
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


        [TestMethod]
        public void Auth()
        {
            //var ticket = new AuthorizationTicket("SPN", DateTime.Parse("1/2/3456 7:08:09.1234567"),
            //                     DateTime.Parse("2/2/3456 7:08:09.1234567"),
            //                     "Login",
            //                     new List<string>(new string[] { "Admin", "User" }),
            //                     new byte[20]);

            //var cmd = new Auth(ticket.ToArray(), Cert);
            //cmd = (Auth)(CtpCommand)cmd;
            //Console.WriteLine(cmd.ToString());
        }

    }
}
