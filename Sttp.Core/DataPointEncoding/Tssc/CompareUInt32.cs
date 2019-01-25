using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.DataPointEncoding
{
    public static class CompareUInt32
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

        public static uint Compare(uint value1, uint value2)
        {
            return ChangeSign(value1 - value2);
        }

        private const uint SignChangeValue = (1u << 31);

        private static uint ChangeSign(uint value)
        {
            //if bit 32 is high, bits 1-31 will be inverted
            //Then all bits will be rotated 1 to the left
            if (value >= SignChangeValue)
            {
                return ((~value) << 1) | 1;
            }
            return (value << 1);
        }

        public static uint UnCompare(uint comparedResult, uint value2)
        {
            comparedResult = UnChangeSign(comparedResult);
            return comparedResult + value2;
        }

        private static uint UnChangeSign(uint value)
        {
            if ((value & 1) == 0)
            {
                return value >> 1;
            }
            return ((~value) >> 1) + SignChangeValue;
        }

        /// <summary>
        /// Counts the number of bits required to store this value if leading zero's are ignored.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Unfortunately, c# cannot call the cpu instruction clz
        /// Example from http://en.wikipedia.org/wiki/Find_first_set
        /// </remarks>
        public static int RequiredBits(uint value)
        {
            int position = 0;
            if (value > 0xFFFFu)
            {
                value >>= 16;
                position += 16;
            }
            if (value > 0xFFu)
            {
                value >>= 8;
                position += 8;
            }
            if (value > 15u)
            {
                value >>= 4;
                position += 4;
            }
            if (value > 3u)
            {
                value >>= 2;
                position += 2;
            }
            if (value > 2u)
            {
                return (int)(position + 1 + (value >> 1));
            }
            return (int)(position + value);
        }

    }
}
