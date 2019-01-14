using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
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
        /// The owner of this data point.
        /// </summary>
        public SttpProducerMetadata Producer { get; private set; }
      
        /// <summary>
        /// A token that is defined by the API layer to simplify mapping.
        /// 
        /// Suggestions for this token include the properly mapped point identifier and possibly routing information.
        /// </summary>
        public object Token;

        /// <summary>
        /// A runtime ID for the data point. This value is the ID associated with the <see cref="SttpDataPoint.RuntimeID"/>
        /// 
        /// Ideally, all measurements will be mapped to a runtime ID, however, for systems that 
        /// contains millions or billions of measurements, this is not a practical expectation.
        /// </summary>
        [CommandField()]
        public int RuntimeID { get; set; }

        /// <summary>
        /// The unique identifier for this PointID.
        /// </summary>
        [CommandField()]
        public CtpObject DataPointID { get; set; }

        [CommandField()]
        public List<AttributeValues> Attributes { get; set; }

        public SttpDataPointMetadata(SttpProducerMetadata producer)
        {
            AssignProducer(producer);
            Attributes = new List<AttributeValues>();
        }

        private SttpDataPointMetadata()
        {

        }

        protected override void AfterLoad()
        {
            if (Attributes == null)
                Attributes = new List<AttributeValues>();
            if ((object)DataPointID == null)
            {
                DataPointID = CtpObject.Null;
            }
        }

        internal void AssignProducer(SttpProducerMetadata producer)
        {
            Producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public static explicit operator SttpDataPointMetadata(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}