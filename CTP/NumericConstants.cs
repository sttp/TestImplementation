using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    internal static class NumericConstants
    {

        //-1 = 0xBFF0000000000000;
        //0 = 0x0000000000000000;
        //1 = 0x3FF0000000000000;

        public const ulong DoubleNeg1 = 0xBFF0000000000000;
        public const ulong Double0 = 0x0000000000000000;
        public const ulong Double1 = 0x3FF0000000000000;


        //-1 = 0xBF800000;
        //0 = 0x00000000;
        //1 = 0x3F800000;

        public const uint SingleNeg1 = 0xBF800000;
        public const uint Single0 = 0x00000000;
        public const uint Single1 = 0x3F800000;

    }
}
