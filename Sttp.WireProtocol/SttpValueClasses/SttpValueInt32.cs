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

        public override sbyte AsSByte => SttpValueInt32Methods.AsSByte(Value);

        public override short AsInt16 => SttpValueInt32Methods.AsInt16(Value);

        public override int AsInt32 => SttpValueInt32Methods.AsInt32(Value);

        public override long AsInt64 => SttpValueInt32Methods.AsInt64(Value);

        public override byte AsByte => SttpValueInt32Methods.AsByte(Value);

        public override ushort AsUInt16 => SttpValueInt32Methods.AsUInt16(Value);

        public override uint AsUInt32 => SttpValueInt32Methods.AsUInt32(Value);

        public override ulong AsUInt64 => SttpValueInt32Methods.AsUInt64(Value);

        public override decimal AsDecimal => SttpValueInt32Methods.AsDecimal(Value);

        public override double AsDouble => SttpValueInt32Methods.AsDouble(Value);

        public override float AsSingle => SttpValueInt32Methods.AsSingle(Value);

        public override DateTime AsDateTime => SttpValueInt32Methods.AsDateTime(Value);

        public override DateTimeOffset AsDateTimeOffset => SttpValueInt32Methods.AsDateTimeOffset(Value);

        public override SttpTime AsSttpTime => SttpValueInt32Methods.AsSttpTime(Value);

        public override SttpTimeOffset AsSttpTimeOffset => SttpValueInt32Methods.AsSttpTimeOffset(Value);

        public override TimeSpan AsTimeSpan => SttpValueInt32Methods.AsTimeSpan(Value);

        public override char AsChar => SttpValueInt32Methods.AsChar(Value);

        public override bool AsBool => SttpValueInt32Methods.AsBool(Value);

        public override Guid AsGuid => SttpValueInt32Methods.AsGuid(Value);

        public override string AsString => SttpValueInt32Methods.AsString(Value);

        public override string ToTypeString => SttpValueInt32Methods.ToTypeString(Value);

        public override byte[] AsBuffer => SttpValueInt32Methods.AsBuffer(Value);

        public override SttpValueSet AsValueSet => SttpValueInt32Methods.AsValueSet(Value);

        public override SttpNamedSet AsNamedSet => SttpValueInt32Methods.AsNamedSet(Value);

        public override object ToNativeType => SttpValueInt32Methods.ToNativeType(Value);

        public override bool IsNull => SttpValueInt32Methods.IsNull(Value);

        public override SttpValueTypeCode ValueTypeCode => SttpValueInt32Methods.ValueTypeCode;

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
            return SttpValueInt32Methods.GetHashCode(Value);
        }

        public override void Save(Stream value)
        {

        }
    }
}
