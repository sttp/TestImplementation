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
    public abstract partial class SttpValue : IEquatable<SttpValue>
    {
        public static readonly SttpValue Null = new SttpValueNull();

        protected SttpValue()
        {
        }

        public abstract long AsInt64 { get; }
        public abstract ulong AsUInt64 { get; }
        public abstract float AsSingle { get; }
        public abstract double AsDouble { get; }
        public abstract decimal AsDecimal { get; }
        public abstract SttpTime AsSttpTime { get; }
        public abstract bool AsBoolean { get; }

        public abstract Guid AsGuid { get; }
        public abstract Guid AsBulkTransportGuid { get; }

        public abstract string AsString { get; }
        public abstract SttpBuffer AsSttpBuffer { get; }
        public abstract SttpMarkup AsSttpMarkup { get; }

        public abstract object ToNativeType { get; }
        public abstract string ToTypeString { get; }

        public sbyte AsSByte
        {
            get
            {
                checked
                {
                    return (sbyte)AsInt32; 
                }
            }
        }

        public int AsInt32
        {
            get
            {
                checked
                {
                    return (int)AsInt64;
                }
            }
        }

        public short AsInt16
        {
            get
            {
                checked
                {
                    return (short)AsInt32;
                }
            }
        }

        public byte AsByte
        {
            get
            {
                checked
                {
                    return (byte)AsInt32;
                }
            }
        }

        public ushort AsUInt16
        {
            get
            {
                checked
                {
                    return (ushort)AsInt32;
                }
            }
        }

        public uint AsUInt32
        {
            get
            {
                checked
                {
                    return (uint)AsInt64;
                }
            }
        }

        public DateTime AsDateTime => AsSttpTime.AsDateTime;

        public DateTimeOffset AsDateTimeOffset => AsSttpTime.AsDateTimeOffset;

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


        //public abstract override int GetHashCode();

        //public void Save(PayloadWriter payloadWriter, bool includeTypeCode)
        //{
        //    throw new NotImplementedException();
        //}

        public static SttpValue CreateBulkTransportGuid(Guid guid)
        {
            return new SttpValueBulkTransportGuid(guid);
        }

    }
}
