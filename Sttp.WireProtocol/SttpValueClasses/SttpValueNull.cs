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
        public override SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Null;
        public override string ToTypeString => "(Null)";
        public override object ToNativeType => null;

        public override long AsInt64 => throw new InvalidCastException();
        public override ulong AsUInt64 => throw new InvalidCastException();
        public override decimal AsDecimal => throw new InvalidCastException();
        public override double AsDouble => throw new InvalidCastException();
        public override float AsSingle => throw new InvalidCastException();
        public override SttpTime AsSttpTime => throw new InvalidCastException();
        public override char AsChar => throw new InvalidCastException();
        public override bool AsBoolean => throw new InvalidCastException();
        public override Guid AsGuid => throw new InvalidCastException();
        public override string AsString => throw new InvalidCastException();
        public override SttpBuffer AsSttpBuffer => throw new InvalidCastException();
        public override SttpMarkup AsSttpMarkup => throw new InvalidCastException();
        public override Guid AsBulkTransportGuid => throw new InvalidCastException();
    }


    internal static class SttpValueNullMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Null;

        public static string ToTypeString()
        {
            return $"(Null)";
        }

        public static object ToNativeType()
        {
            return null;
        }

    }
}
