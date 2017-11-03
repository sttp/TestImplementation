using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This class contains required metadata for each STTP point. 
    /// </summary>
    public class SttpPointInfo
    {
        /// <summary>
        /// An API definable token that can be used to cache lookup information for this DataPoint's processing.
        /// Since the publisher does not have to map anything, this is for the subscriber.
        /// </summary>
        public object PointToken;

        /// <summary>
        /// A 64-bit RuntimeID. Since the publisher and subscriber will not have the same runtime ID, it's up to the subscriber to map the publisher's runtime ID.
        /// </summary>
        public readonly long RuntimeID;

        /// <summary>
        /// Defines the mapping code for the provided time quality. A value of 0 means this value will be ignored.
        /// The lowest 3 bits define how many bytes are provided for this flag.
        /// </summary>
        public readonly short TimeQualityMap;

        /// <summary>
        /// Defines the mapping code for the provided value quality. A value of 0 means this value will be ignored.
        /// The lowest 3 bits define how many bytes are provided for this flag.
        /// </summary>
        public readonly short ValueQualityMap;

        /// <summary>
        /// Defines the mapping code for the Extra Flags. A value of 0 means this value will be ignored.
        /// The lowest 3 bits define how many bytes are provided for this flag.
        /// </summary>
        public readonly short ExtraFlagsMap;

        /// <summary>
        /// Defines the value type code for every measurement sent on the wire. 
        /// 
        /// If 0, this means 'Value' will not be sent.
        /// If not 0, it's still possible for null to be sent, so all types should support nullable types.
        /// </summary>
        public readonly short ValueTypeCode;

        /// <summary>
        /// Defines the serialization information that must exist with all SttpDataPoint.
        /// </summary>
        /// <param name="runtimeID"></param>
        /// <param name="timeQualityMap"></param>
        /// <param name="valueQualityMap"></param>
        /// <param name="extraFlagsMap"></param>
        /// <param name="valueTypeCode"></param>
        public SttpPointInfo(long runtimeID, short timeQualityMap, short valueQualityMap, short extraFlagsMap, short valueTypeCode)
        {
            RuntimeID = runtimeID;
            TimeQualityMap = timeQualityMap;
            ValueQualityMap = valueQualityMap;
            ExtraFlagsMap = extraFlagsMap;
            ValueTypeCode = valueTypeCode;
        }

        public void Write(PacketWriter stream)
        {
            stream.Write(RuntimeID);
            stream.Write(TimeQualityMap);
            stream.Write(ValueQualityMap);
            stream.Write(ExtraFlagsMap);
            stream.Write(ValueTypeCode);
        }
    }
}
