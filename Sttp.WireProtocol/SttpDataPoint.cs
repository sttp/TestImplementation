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
