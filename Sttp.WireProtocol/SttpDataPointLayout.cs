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
        /// Bits 6,7:   0 - ID is a 32-bit Runtime ID, it is configurable and will be closer to 16 bits.
        ///             1 - ID is a Guid, or 128-bit integer. It doesn't have to be completely random.
        ///             2 - ID is a String
        ///             3 - ID is a SttpNamedSet
        /// 
        /// Bit 5:      Set = Has Extra Fields. See following byte for details.
        /// 
        /// Bits 0-4:   Sttp Value Type Code. 32 values.
        /// 
        /// </summary>
        public byte RawValue;

        public SttpPointIDTypeCode PointIDType
        {
            get
            {
                return (SttpPointIDTypeCode)(RawValue >> 5);
            }
            set
            {
                RawValue = (byte)(RawValue & ~((1 << 5) - 1) | (((byte)value) << 5));
            }
        }

        public SttpValueTypeCode ValueType
        {
            get
            {
                return (SttpValueTypeCode)(RawValue & 31);
            }
            set
            {
                RawValue = (byte)(RawValue & ~31 | (((byte)value) & 31));
            }
        }

        public bool ExtraFields
        {
            get
            {
                return (RawValue & 32) != 0;
            }
            set
            {
                if (value)
                {
                    RawValue = (byte)(RawValue | 32);
                }
                else
                {
                    RawValue = (byte)(RawValue & ~32);
                }
            }
        }
    }
}
