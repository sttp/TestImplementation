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
        /// A token was defined by the API layer when the runtime ID was defined at the protocol level.
        /// This is only present when <see cref="DataPointID"/> is a RuntimeID type. This field is only 
        /// used on the receiving end. The source of the data point does not need to assign this field.
        /// 
        /// Suggestions for this token include the properly mapped point identifier and possibly routing information.
        /// </summary>
        public object PointToken;

        /// <summary>
        /// The unique identifier for this PointID. Can be int32, GUID, string, SttpNamedSet.
        /// </summary>
        public SttpDataPointID DataPointID;

        /// <summary>
        /// A 64-bit timestamp
        /// </summary>
        public SttpTimestamp Timestamp;

        /// <summary>
        /// The value for the data point.
        /// </summary>
        public SttpValue Value = new SttpValue();

        /// <summary>
        /// 16-bits for identifying the quality of the time.
        /// </summary>
        public TimeQualityFlags TimestampQuality;

        /// <summary>
        /// 16-bits for identifying the quality of the value.
        /// </summary>
        public ValueQualityFlags ValueQuality;

        /// <summary>
        /// An array of extra fields that exist in the protocol. Specific implementations of STTP should define these fields.
        /// Examples include:
        ///   An extra timestamp field for more time precision. 
        ///   Some kind of sequence identifier.
        ///   Some kind of pivot field. 
        ///   Extra quality bits.
        /// </summary>
        public SttpValue[] ExtraFields;
        
        public void Write(PacketWriter stream)
        {
            SttpDataPointLayout layout;
        }
    }
}
