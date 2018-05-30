using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;

namespace Sttp.Tests.Serialization
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Test1()
        {
            var cmd = new CommandMetadataRequestFailed("Failed", "True");
            var doc1 = cmd.ToCtpDocument();
            Console.WriteLine(doc1.ToYAML());

            var doc2 = CtpDocument.Serialize(cmd, "Root Element");
            Console.WriteLine(doc2.ToYAML());
        }

        [TestMethod]
        public void Test2()
        {
            var cmd = new CommandMetadataRequestFailed("Failed", "True");
            var doc1 = cmd.ToCtpDocument();
            Console.WriteLine(doc1.ToYAML());

            var doc2 = CtpDocument.Serialize(cmd, "Root Element");
            Console.WriteLine(doc2.ToYAML());
        }
    }
}
