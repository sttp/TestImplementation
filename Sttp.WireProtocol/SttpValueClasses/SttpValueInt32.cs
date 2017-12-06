using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueInt32 : SttpValue
    {
        public readonly int Value;

        public SttpValueInt32(int value)
        {
            Value = value;
        }

        public override sbyte AsSByte
        {
            get
            {
                checked
                {
                    return (sbyte)Value;
                }
            }
        }
        public override short AsInt16
        {
            get
            {
                checked
                {
                    return (short)Value;
                }
            }
        }

        public override int AsInt32
        {
            get
            {
                checked
                {
                    return (int)Value;
                }
            }
        }

        public override long AsInt64
        {
            get
            {
                checked
                {
                    return (long)Value;
                }
            }
        }

        public override byte AsByte
        {
            get
            {
                checked
                {
                    return (byte)Value;
                }
            }
        }

        public override ushort AsUInt16
        {
            get
            {
                checked
                {
                    return (ushort)Value;
                }
            }
        }

        public override uint AsUInt32
        {
            get
            {
                checked
                {
                    return (uint)Value;
                }
            }
        }

        public override ulong AsUInt64
        {
            get
            {
                checked
                {
                    return (ulong)Value;
                }
            }
        }

        public override decimal AsDecimal
        {
            get
            {
                checked
                {
                    return (decimal)Value;
                }
            }
        }

        public override double AsDouble
        {
            get
            {
                checked
                {
                    return (double)Value;
                }
            }
        }

        public override float AsSingle
        {
            get
            {
                checked
                {
                    return (float)Value;
                }
            }
        }

        public override DateTime AsDateTime
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override DateTimeOffset AsDateTimeOffset
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override SttpTimestamp AsSttpTimestamp
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override SttpTimestampOffset AsSttpTimestampOffset
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override TimeSpan AsTimeSpan
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override char AsChar
        {
            get
            {
                checked
                {
                    return (char)Value;
                }
            }
        }

        public override bool AsBool
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override Guid AsGuid
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override string AsString
        {
            get
            {
                return Value.ToString();
            }
        }

        public override string AsTypeString
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override byte[] AsBuffer
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override SttpValueSet AsValueSet
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override SttpNamedSet AsNamedSet
        {
            get
            {
                throw new InvalidCastException();
            }
        }

        public override object AsNativeType
        {
            get
            {
                return Value;
            }
        }

        public override bool IsNull
        {
            get
            {
                return true;
            }
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Int32;

        public override SttpValue Clone()
        {
            return this;
        }

        public override void Save(PayloadWriter wr)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return SttpValueTypeCode.Int32.GetHashCode() ^ Value.GetHashCode();
        }

        public override void Save(Stream value)
        {

        }
    }
}
