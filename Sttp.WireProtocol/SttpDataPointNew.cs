using System;
using System.Runtime.InteropServices;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public class SttpDataPointNew
    {
        /// <summary>
        /// The identifying information 
        /// </summary>
        public SttpPointID PointID;
       
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
        public SttpValue Value;

        /// <summary>
        /// 32-bits for identifying the quality of the time.
        /// </summary>
        public uint TimestampQuality;
        /// <summary>
        /// 32-bits for identifying the quality of the value.
        /// </summary>
        public uint ValueQuality;
    }
}
