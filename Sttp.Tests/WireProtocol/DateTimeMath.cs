using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class DateTimeMath
    {
        [TestMethod]
        public void Test()
        {
            long max62Bit = (1L << 62) - 1;
            long maxDateTime = DateTime.MaxValue.Ticks;
            long extraSeconds = (max62Bit - maxDateTime) / TimeSpan.TicksPerSecond;
            Console.WriteLine("Max 62 bit value:" + max62Bit);
            Console.WriteLine("MaxTicks:" + (maxDateTime + 1));
            Console.WriteLine("Extra Seconds:" + extraSeconds.ToString("N0"));

            Console.WriteLine("DaysPerEpoc:" + (DateTime.MaxValue.Ticks / TimeSpan.TicksPerDay).ToString("N0"));
            Console.WriteLine("HoursPerEpoc:" + (DateTime.MaxValue.Ticks / TimeSpan.TicksPerHour).ToString("N0"));
            Console.WriteLine("MinutesPerEpoc:" + (DateTime.MaxValue.Ticks / TimeSpan.TicksPerMinute).ToString("N0"));
            Console.WriteLine("SecondsPerEpoc:" + (DateTime.MaxValue.Ticks / TimeSpan.TicksPerSecond).ToString("N0"));
        }

    }
}
