using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class TestUnusedBits
    {
        [TestMethod]
        public unsafe void TestBits()
        {
            float value = 28927496529742; //A random number to have bits set.

            for (int x = 0; x < 256; x++)
            {
                ((byte*)&value)[3] = (byte)x;
                Console.WriteLine(x.ToString() + '\t' + value.ToString());
            }

            double value2 = 28927496529742; //A random number to have bits set.

            for (int x = 0; x < 256; x++)
            {
                ((byte*)&value2)[7] = (byte)x;
                Console.WriteLine(x.ToString() + '\t' + value2.ToString());
            }
        }
    }
}
