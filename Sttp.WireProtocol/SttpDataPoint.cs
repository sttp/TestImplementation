using System;
using System.Runtime.InteropServices;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This class contains the fundamental unit of transmission for STTP.
    /// The intention of this class is to be reusable. In order to prevent reuse of the class,
    /// it must be marked as Immutable. This will indicate that a new class must be created if
    /// reuse is the normal case.
    /// </summary>
    public class SttpDataPoint
        : ISttpValue
    {
        /// <summary>
        /// The identifying information 
        /// </summary>
        public SttpPointInfo PointInfo;

        /// <summary>
        /// A 64-bit timestamp
        /// </summary>
        public SttpTimestamp Timestamp;

        /// <summary>
        /// User defined flags that combines with PointID and Time to uniquely define a measurement. 
        /// This can contain sequence numbers, or extra time precision. Most use cases will leave this field 0.
        /// </summary>
        public long ExtraFlags;

        /// <summary>
        /// The value for the data point.
        /// </summary>
        private SttpValue m_value = new SttpValue();

        /// <summary>
        /// 64-bits for identifying the quality of the time.
        /// </summary>
        public long TimestampQuality;

        /// <summary>
        /// 64-bits for identifying the quality of the value.
        /// </summary>
        public long ValueQuality;

        public bool IsNull
        {
            get => m_value.IsNull;
            set => m_value.IsNull = value;
        }

        public double AsDouble
        {
            get => m_value.AsDouble;
            set => m_value.AsDouble = value;
        }

        public long AsInt64
        {
            get => m_value.AsInt64;
            set => m_value.AsInt64 = value;
        }

        public ulong AsUInt64
        {
            get => m_value.AsUInt64;
            set => m_value.AsUInt64 = value;
        }
        public float AsSingle
        {
            get => m_value.AsSingle;
            set => m_value.AsSingle = value;
        }

        public byte[] AsBuffer
        {
            get => m_value.AsBuffer;
            set => m_value.AsBuffer = value;
        }

        public object AsObject
        {
            get => m_value.AsObject;
            set => m_value.AsObject = value;
        }

        public SttpFundamentalTypeCode FundamentalTypeCode => m_value.FundamentalTypeCode;

        public void Write(PacketWriter stream)
        {
            stream.Write(PointInfo.RuntimeID);
            stream.Write(Timestamp.RawValue);
            m_value.Save(stream);
            if (PointInfo.ExtraFlagsMap != 0)
                stream.Write(ExtraFlags);
            if (PointInfo.TimeQualityMap != 0)
                stream.Write(TimestampQuality);
            if (PointInfo.ValueQualityMap != 0)
                stream.Write(ValueQuality);
        }
    }
}
