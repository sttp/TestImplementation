using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    public struct SttpDataPointLayout
    {
        /// <summary>
        /// Defines how a Data Point is internally structured.
        /// 
        /// Bits 0,1:   0 - ID is Bits32
        ///             1 - ID is Bits64
        ///             2 - ID is Bits128
        ///             3 - ID is String
        /// 
        /// Bit 2:      0 - Time: Bits64
        ///             1 - Time: Bits64 + Buffer
        /// 
        /// Bit 3:      0 - TimeQuality is Bits32
        ///             1 - TimeQuality is Bits64
        ///
        /// Bit 4:      0 - ValueQuality is Bits32
        ///             1 - ValueQuality is Bits64
        /// 
        /// Bits 5,6,7: 0 - Value is Null
        ///             1 - Value is Bits8
        ///             2 - Value is Bits16
        ///             3 - Value is Bits32
        ///             4 - Value is Bits64
        ///             5 - Value is Buffer
        /// 
        /// </summary>
        public byte RawValue;


    }

}
