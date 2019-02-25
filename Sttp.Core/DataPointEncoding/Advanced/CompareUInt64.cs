using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.DataPointEncoding
{
    public static class CompareUInt64
    {
        public static ulong Compare(ulong currentValue, ulong prevValue)
        {
            return ChangeSign(currentValue - prevValue);
        }

        private const ulong SignChangeValue = (1ul << 63);

        public static ulong ChangeSign(ulong value)
        {
            //if bit 64 is high, bits 1-63 will be inverted
            //Then all bits will be rotated 1 to the left
            if (value >= SignChangeValue)
            {
                return ((~value) << 1) | 1;
            }
            return (value << 1);
        }

        public static ulong UnCompare(ulong comparedResult, ulong prevValue)
        {
            comparedResult = UnChangeSign(comparedResult);
            return comparedResult + prevValue;
        }

        public static ulong UnChangeSign(ulong value)
        {
            if ((value & 1) == 0)
            {
                return value >> 1;
            }
            return ((~value) >> 1) + SignChangeValue;
        }
    }
}
