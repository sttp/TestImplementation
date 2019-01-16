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
        /// The owner of this data point. This is likely an individual equipment station or RTU.
        /// This field may be null.
        /// </summary>
        public SttpProducerMetadata Producer { get; private set; }

        /// <summary>
        /// A token that is defined by the API layer to simplify mapping.
        /// If the API takes advantage of this, it cannot be assumed that this will always be assigned since
        /// any updates to metadata by the protocol may cause this field to be reset.
        /// 
        /// Suggestions for this token include the properly mapped point identifier and possibly routing information.
        /// </summary>
        public object Token;

        /// <summary>
        /// A runtime ID for the data point. If present, this improves the speed of serialization.
        /// However, this ID must be a one-to-one relationship to DataPointID for each instance of the application.
        /// </summary>
        [CommandField()]
        public int? RuntimeID { get; set; }

        /// <summary>
        /// The unique identifier for this PointID. This will typically be a GUID, but may also be a string or integer.
        /// </summary>
        [CommandField()]
        public CtpObject DataPointID { get; private set; }

        /// <summary>
        /// The list of user defined Key/Value pairs of metadata associated with this data point.
        /// </summary>
        [CommandField()]
        public List<AttributeValues> Attributes { get; private set; }

        public SttpDataPointMetadata(SttpProducerMetadata producer)
        {
            Producer = producer;
            Attributes = new List<AttributeValues>();
            DataPointID = CtpObject.Null;
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
            Producer = producer;
        }

        public static explicit operator SttpDataPointMetadata(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}