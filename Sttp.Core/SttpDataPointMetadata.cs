using System.Collections.Generic;
using CTP;

namespace Sttp
{
    /// <summary>
    /// The metadata associated with a single data point.
    /// </summary>
    public class SttpDataPointMetadata
        : CommandObject<SttpDataPointMetadata>
    {
        /// <summary>
        /// A token was defined by the API layer when the runtime ID was defined at the protocol level.
        /// This is only present when <see cref="DataPointID"/> is a RuntimeID type. This field is only 
        /// used on the receiving end. The source of the data point does not need to assign this field.
        /// 
        /// Suggestions for this token include the properly mapped point identifier and possibly routing information.
        /// </summary>
        [CommandField()]
        public object DataPointAPIToken;

        /// <summary>
        /// A runtime ID for the data point. A negative value designates that this runtime ID is not valid. 
        /// 
        /// Ideally, all measurements will be mapped to a runtime ID, however, for systems that 
        /// contains millions or billions of measurements, this is not a practical expectation.
        /// </summary>
        [CommandField()]
        public int? DataPointRuntimeID { get; set; }

        /// <summary>
        /// The unique identifier for this PointID.
        /// </summary>
        [CommandField()]
        public CtpObject DataPointID { get; set; }

        [CommandField()]
        public List<AttributeValues> Attributes { get; set; }

        public SttpDataPointMetadata()
        {

        }

        public static explicit operator SttpDataPointMetadata(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}