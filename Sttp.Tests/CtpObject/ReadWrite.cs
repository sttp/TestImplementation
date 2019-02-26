using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using GSF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp;

namespace Sttp.Tests.Object_Serialzation
{
    [TestClass]
    public class ReadWrite
    {
        [TestMethod]
        public void TestValues()
        {
            Test(new Random(0), CtpTypeCode.Null);
            Test(new Random(0), CtpTypeCode.Int8);
            Test(new Random(0), CtpTypeCode.Int16);
            Test(new Random(0), CtpTypeCode.Int32);
            Test(new Random(0), CtpTypeCode.Int64);
            Test(new Random(0), CtpTypeCode.Single);
            Test(new Random(0), CtpTypeCode.Double);
            Test(new Random(0), CtpTypeCode.Numeric);
            Test(new Random(0), CtpTypeCode.CtpTime);
            Test(new Random(0), CtpTypeCode.Boolean);
            Test(new Random(0), CtpTypeCode.Guid);
            Test(new Random(0), CtpTypeCode.String);
            Test(new Random(0), CtpTypeCode.CtpBuffer);
            Test(new Random(0), CtpTypeCode.CtpCommand);

            Test(new Random());
        }

        private void Test(Random r)
        {
            for (int cnt = 0; cnt < 100; cnt++)
            {
                int length = r.Next(1000);
                var list = new List<CtpObject>();
                for (int x = 0; x < length; x++)
                {
                    list.Add(CreateRandom(r, (CtpTypeCode)r.Next(14)));
                }
                Test(list);
            }
            
        } 

        private void Test(Random r, CtpTypeCode code)
        {
            var list = new List<CtpObject>();
            for (int x = 0; x < 1000; x++)
            {
                list.Add(CreateRandom(r, code));
            }
            Test(list);
        }

        private void Test(List<CtpObject> data)
        {
            var wr = new CtpObjectWriter();
            for (var x = 0; x < data.Count; x++)
            {
                var o = data[x];
                wr.Write(o);
            }

            var rd = new CtpObjectReader(wr.ToArray());
            for (var x = 0; x < data.Count; x++)
            {
                var o = data[x];
                var oo = rd.Read();
                if (!o.Equals(oo))
                {
                    throw new Exception($"Not Equal {o} {oo} on record {x}");
                }
            }
        }

        private CtpObject CreateRandom(Random r, CtpTypeCode typeCode)
        {
            byte[] buffer = new byte[8];
            r.NextBytes(buffer);
            switch (typeCode)
            {
                case CtpTypeCode.Null:
                    return CtpObject.Null;
                case CtpTypeCode.Int8:
                    switch (r.Next(100))
                    {
                        case 0: return -1;
                        case 1: return -2;
                        case 2: return 0;
                        case 3: return 1;
                        case 4: return 99;
                        case 5: return 100;
                        case 6: return 101;
                        case 7: return sbyte.MaxValue;
                        case 8: return sbyte.MinValue;
                        default: return ((sbyte)buffer[0]) >> r.Next(8);
                    }
                case CtpTypeCode.Int16:
                    switch (r.Next(100))
                    {
                        case 0: return short.MaxValue;
                        case 1: return short.MinValue;
                        default: return BigEndian.ToInt16(buffer, 0) >> r.Next(16);
                    }
                case CtpTypeCode.Int32:
                    switch (r.Next(100))
                    {
                        case 0: return int.MaxValue;
                        case 1: return int.MinValue;
                        default: return BigEndian.ToInt64(buffer, 0) >> r.Next(32);
                    }
                case CtpTypeCode.Int64:
                    switch (r.Next(100))
                    {
                        case 0: return long.MaxValue;
                        case 1: return long.MinValue;
                        default: return BigEndian.ToInt64(buffer, 0) >> r.Next(64);
                    }
                case CtpTypeCode.Single:
                    switch (r.Next(100))
                    {
                        case 0: return -1f;
                        case 1: return 0f;
                        case 2: return 1f;
                        case 3: return float.NaN;
                        case 4: return float.MaxValue;
                        case 5: return float.MinValue;
                        case 6: return float.Epsilon;
                        case 7: return float.PositiveInfinity;
                        case 8: return float.NegativeInfinity;
                        default: return BigEndian.ToSingle(buffer, 0);
                    }
                case CtpTypeCode.Double:
                    switch (r.Next(100))
                    {
                        case 0: return -1.0;
                        case 1: return 0.0;
                        case 2: return 1.0;
                        case 3: return double.NaN;
                        case 4: return double.MaxValue;
                        case 5: return double.MinValue;
                        case 6: return double.Epsilon;
                        case 7: return double.PositiveInfinity;
                        case 8: return double.NegativeInfinity;
                        default: return BigEndian.ToDouble(buffer, 0);
                    }
                case CtpTypeCode.Numeric:
                    switch (r.Next(100))
                    {
                        case 0: return decimal.MinusOne;
                        case 1: return decimal.Zero;
                        case 2: return decimal.One;
                        case 3: return decimal.MaxValue;
                        case 4: return decimal.MinValue;
                        default:
                            {
                                uint high = BigEndian.ToUInt32(buffer, 0);
                                uint mid = BigEndian.ToUInt32(buffer, 2);
                                uint low = BigEndian.ToUInt32(buffer, 4);
                                int shift = r.Next(97);
                                if (shift >= 32)
                                {
                                    high = 0;
                                    shift = -32;
                                    if (shift >= 32)
                                    {
                                        mid = 0;
                                        shift = -32;
                                        low >>= shift;
                                    }
                                    else
                                    {
                                        mid >>= shift;
                                    }
                                }
                                else
                                {
                                    high >>= shift;
                                }
                                return new decimal((int)low, (int)mid, (int)high, r.Next(2) == 0, (byte)r.Next(29));
                            }
                    }
                case CtpTypeCode.CtpTime:
                    switch (r.Next(100))
                    {
                        case 0: return CtpTime.MaxValue;
                        case 1: return CtpTime.MinValue;
                        case 2: return new CtpTime(0);
                        default: return new CtpTime(DateTime.FromBinary(Math.Min(DateTime.MaxValue.Ticks, Math.Max(0, BigEndian.ToInt64(buffer, 0)))));
                    }
                case CtpTypeCode.Boolean:
                    return (buffer[0] & 1) == 1;
                case CtpTypeCode.Guid:
                    return (buffer[0] % 30 == 0) ? Guid.Empty : Guid.NewGuid();
                case CtpTypeCode.String:
                    {
                        var bytes = (byte[])CreateRandom(r, CtpTypeCode.CtpBuffer);
                        return Encoding.Unicode.GetString(bytes);
                    }
                case CtpTypeCode.CtpBuffer:
                    {
                        var data = new byte[buffer[0]];
                        r.NextBytes(data);
                        return data;
                    }
                case CtpTypeCode.CtpCommand:
                    return new CtpError(CreateRandom(r, CtpTypeCode.String).AsString, CreateRandom(r, CtpTypeCode.String).AsString).ToCommand();
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }

    }
}
