using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    public struct SttpPointSerializationInfo
    {
        //ToDo: Fix this class

        /// <summary>
        /// Details enough information to serialize the incoming value
        /// 
        /// bit 0:      PointID is Session Generated
        /// bits 1-3:   Base Value Type Code
        /// bits 4-7:   ExtraFlagsLength
        /// bits 8-11:  TimeQualityFlagsLength
        /// bits 12-15: ValueQualityFlagsLength 
        /// bits 16-63: PointRuntimeID 
        /// 
        /// </summary>
        public ulong RawValue;

        public SttpPointSerializationInfo(ulong value)
        {
            RawValue = value;
        }

        public static implicit operator ulong(SttpPointSerializationInfo value)
        {
            return value.RawValue;
        }

        public static implicit operator SttpPointSerializationInfo(ulong value)
        {
            return new SttpPointSerializationInfo(value);
        }

        public long PointRuntimeID
        {
            get
            {
                return (long)(RawValue >> 9);
            }
            set
            {
                RawValue = (RawValue & 521) | ((ulong)value << 9);
            }
        }

        public bool IsSessionID
        {
            get
            {
                return (RawValue & 1) == 1;
            }
            set
            {
                if (value)
                    RawValue = RawValue | 1;
                else
                    RawValue = RawValue & ~1ul;
            }
        }

        public SttpBaseValueTypeCode BaseValueTypeCode
        {
            get
            {
                return (SttpBaseValueTypeCode)(RawValue & 7);
            }
            set
            {
                RawValue = RawValue & (~7uL) | ((uint)value & 7);
            }
        }

        public int ExtraFlagsLength
        {
            get
            {
                return (int)((RawValue >> 3) & 7);
            }
            set
            {
                RawValue = RawValue & (~(7uL << 3)) | ((uint)value & 7) << 3;
            }
        }

        public int TimeQualityLength
        {
            get
            {
                return (int)((RawValue >> 3) & 7);
            }
            set
            {
                RawValue = RawValue & (~(7uL << 3)) | ((uint)value & 7) << 3;
            }
        }

        public int ValueQualityLength
        {
            get
            {
                return (int)((RawValue >> 3) & 7);
            }
            set
            {
                RawValue = RawValue & (~(7uL << 3)) | ((uint)value & 7) << 3;
            }
        }

    }
}
