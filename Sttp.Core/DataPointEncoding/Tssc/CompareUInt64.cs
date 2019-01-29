using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.DataPointEncoding
{
    public static class CompareUInt64
    {
        //public static void Test()
        //{
        //    for (uint x = 0; x < uint.MaxValue; x++)
        //    {
        //        if (x-10 != UnCompare(Compare(x-10, x), x))
        //        {
        //            throw new Exception(x.ToString());
        //        }
        //    }
        //}

        public static ulong Compare(ulong value1, ulong value2)
        {
            return ChangeSign(value1 - value2);
        }

        private const ulong SignChangeValue = (1ul << 63);

        private static ulong ChangeSign(ulong value)
        {
            //if bit 64 is high, bits 1-63 will be inverted
            //Then all bits will be rotated 1 to the left
            if (value >= SignChangeValue)
            {
                return ((~value) << 1) | 1;
            }
            return (value << 1);
        }

        public static ulong UnCompare(ulong comparedResult, ulong value2)
        {
            comparedResult = UnChangeSign(comparedResult);
            return comparedResult + value2;
        }

        private static ulong UnChangeSign(ulong value)
        {
            if ((value & 1) == 0)
            {
                return value >> 1;
            }
            return ((~value) >> 1) + SignChangeValue;
        }


    }
}
