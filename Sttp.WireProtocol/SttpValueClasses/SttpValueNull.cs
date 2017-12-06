using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueNull : SttpValue
    {
        public override sbyte AsSByte => throw new InvalidCastException();
        public override short AsInt16 => throw new InvalidCastException();
        public override int AsInt32 => throw new InvalidCastException();
        public override long AsInt64 => throw new InvalidCastException();
        public override byte AsByte => throw new InvalidCastException();
        public override ushort AsUInt16 => throw new InvalidCastException();
        public override uint AsUInt32 => throw new InvalidCastException();
        public override ulong AsUInt64 => throw new InvalidCastException();
        public override decimal AsDecimal => throw new InvalidCastException();
        public override double AsDouble => throw new InvalidCastException();
        public override float AsSingle => throw new InvalidCastException();
        public override DateTime AsDateTime => throw new InvalidCastException();
        public override DateTimeOffset AsDateTimeOffset => throw new InvalidCastException();
        public override SttpTime AsSttpTime => throw new InvalidCastException();
        public override SttpTimeOffset AsSttpTimeOffset => throw new InvalidCastException();
        public override TimeSpan AsTimeSpan => throw new InvalidCastException();
        public override char AsChar => throw new InvalidCastException();
        public override bool AsBool => throw new InvalidCastException();
        public override Guid AsGuid => throw new InvalidCastException();
        public override string AsString => throw new InvalidCastException();
        public override string ToTypeString => "(Null)";
        public override byte[] AsBuffer => throw new InvalidCastException();
        public override SttpValueSet AsValueSet => throw new InvalidCastException();
        public override SttpNamedSet AsNamedSet => throw new InvalidCastException();
        public override object ToNativeType => null;
        public override bool IsNull => true;

        public override SttpValueTypeCode ValueTypeCode { get; }
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
            throw new NotImplementedException();
        }

        public override void Save(Stream value)
        {
            throw new NotImplementedException();
        }
    }
}
