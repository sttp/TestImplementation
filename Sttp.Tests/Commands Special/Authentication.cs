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
    public class DictionaryTest
    {
        [CommandName("TestObject")]
        private class TestObject
            : CommandObject<TestObject>
        {
            [CommandField()]
            public Dictionary<int, TypeCode?> Data;

            public TestObject()
            {
                Data = new Dictionary<int, TypeCode?>();
                Data[1] = null;
                Data[2] = TypeCode.DateTime;
            }

            public static explicit operator TestObject(CtpCommand obj)
            {
                return FromCommand(obj);
            }
        }

        [TestMethod]
        public void Test()
        {
            var t = new TestObject();
            t.DebugToConsole();
            var obj = (TestObject)t.ToCommand();
            if (obj.ToString() != t.ToString())
                throw new Exception();
        }



    }
}
