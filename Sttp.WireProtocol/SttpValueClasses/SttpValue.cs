using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;
using Sttp.SttpValueClasses;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public abstract class SttpValue : IEquatable<SttpValue>
    {
        public static readonly SttpValue Null = new SttpValueNull();

        protected SttpValue()
        {
        }

        public abstract sbyte AsSByte { get; }
        public abstract short AsInt16 { get; }
        public abstract int AsInt32 { get; }
        public abstract long AsInt64 { get; }
        public abstract byte AsByte { get; }
        public abstract ushort AsUInt16 { get; }
        public abstract uint AsUInt32 { get; }
        public abstract ulong AsUInt64 { get; }
        public abstract float AsSingle { get; }
        public abstract double AsDouble { get; }
        public abstract decimal AsDecimal { get; }
        public abstract DateTime AsDateTime { get; }
        public abstract DateTimeOffset AsDateTimeOffset { get; }
        public abstract SttpTime AsSttpTime { get; }
        public abstract SttpTimeOffset AsSttpTimeOffset { get; }
        public abstract TimeSpan AsTimeSpan { get; }
        public abstract bool AsBool { get; }
        public abstract char AsChar { get; }
        public abstract Guid AsGuid { get; }
        public abstract string AsString { get; }
        public abstract SttpBuffer AsBuffer { get; }
        public abstract SttpValueSet AsSttpValueSet { get; }
        public abstract SttpNamedSet AsSttpNamedSet { get; }
        public abstract SttpMarkup AsSttpMarkup { get; }
        public abstract Guid AsBulkTransportGuid { get; }

        public abstract object ToNativeType { get; }
        public abstract string ToTypeString { get; }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        public bool IsNull => ValueTypeCode == SttpValueTypeCode.Null;

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public abstract SttpValueTypeCode ValueTypeCode { get; }

        public bool IsImmutable => !(this is SttpValueMutable);

        /// <summary>
        /// Clones the value
        /// </summary>
        /// <returns></returns>
        public SttpValue Clone()
        {
            if (IsImmutable)
                return this;
            return ((SttpValueMutable)this).CloneAsImmutable();
        }

        //public static bool operator ==(SttpValue a, SttpValue b)
        //{
        //    if (ReferenceEquals(a, b))
        //        return true;
        //    if (ReferenceEquals(a, null))
        //        return false;
        //    if (ReferenceEquals(b, null))
        //        return false;
        //    if (a.ValueTypeCode != b.ValueTypeCode)
        //        return false;
        //    return true;
        //}

        //public static bool operator !=(SttpValue a, SttpValue b)
        //{
        //    return !(a == b);
        //}

        //public object ToNativeType(SttpValueTypeCode typeCode)
        //{
        //    throw new NotImplementedException();
        //}

        public static explicit operator SttpValue(bool v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator SttpValue(double v)
        {
            throw new NotImplementedException();
            //var rv = new SttpValue();
            //rv.AsDouble = v;
            //return rv;
        }
        public static explicit operator SttpValue(string v)
        {
            throw new NotImplementedException();
            //var rv = new SttpValue();
            //rv.AsString = v;
            //return rv;
        }


        public override string ToString()
        {
            return ToTypeString;
        }

        public bool Equals(SttpValue other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (ValueTypeCode == other.ValueTypeCode)
            {

            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SttpValue)obj);
        }

        public void Save(PayloadWriter wr)
        {

        }

        //public abstract override int GetHashCode();

        //public void Save(PayloadWriter payloadWriter, bool includeTypeCode)
        //{
        //    throw new NotImplementedException();
        //}

        public static SttpValue Load(PayloadReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
