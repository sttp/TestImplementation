using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.Commands
{
    [TestClass]
    public class General
    {
        [TestMethod]
        public void Error()
        {
            try
            {
                throw new ArgumentException("Out of range", "index");
            }
            catch (Exception e)
            {
                var cmd = new CtpError("An error has occurred", e.ToString());
                cmd = (CtpError)(CtpCommand)cmd;
                Console.WriteLine(cmd.ToString());
            }
        }

        [TestMethod]
        public void Raw()
        {
            var cmd = new CtpRaw(Guid.NewGuid().ToByteArray(), 1);
            cmd = (CtpRaw)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void RequestFailed()
        {
            try
            {
                throw new ArgumentException("Out of range", "index");
            }
            catch (Exception e)
            {
                var cmd = new CtpRequestFailed("GetMetadata", "An error has occurred", e.ToString());
                cmd = (CtpRequestFailed)(CtpCommand)cmd;
                Console.WriteLine(cmd.ToString());
            }
        }

        [TestMethod]
        public void RequestSuccess()
        {
            var cmd = new CtpRequestSuccess("GetMetadata");
            cmd = (CtpRequestSuccess)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void KeepAlive()
        {
            var cmd = new CtpKeepAlive(DateTime.Parse("1/12/2008 1:03 AM"), DateTime.Parse("1/12/2008 1:02 AM"));
            cmd = (CtpKeepAlive)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }
    }
}
