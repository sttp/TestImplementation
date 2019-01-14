using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using CTP;

namespace Sttp
{
    /// <summary>
    /// The smallest unit of metadata exchange that contains a list of produced data points
    /// </summary>
    [CommandName("ProducerMetadata")]
    public class SttpProducerMetadata
        : CommandObject<SttpProducerMetadata>
    {
        /// <summary>
        /// The unique identifier for this Producer.
        /// </summary>
        [CommandField()]
        public CtpObject ProducerID { get; set; }

        /// <summary>
        /// A device record that has child records.
        /// </summary>
        [CommandField()]
        public List<AttributeValues> Attributes { get; set; }

        /// <summary>
        /// All of the Measurements associated with a specific device.
        /// </summary>
        [CommandField()]
        public List<SttpDataPointMetadata> DataPoints { get; set; }

        public SttpProducerMetadata()
        {
            Attributes = new List<AttributeValues>();
            DataPoints = new List<SttpDataPointMetadata>();
        }

        protected override void AfterLoad()
        {
            if (DataPoints == null)
                DataPoints = new List<SttpDataPointMetadata>();
            if (Attributes == null)
                Attributes = new List<AttributeValues>();
            if ((object)ProducerID == null)
            {
                ProducerID = CtpObject.Null;
            }
            foreach (var item in DataPoints)
            {
                item.AssignProducer(this);
            }
        }

        public static explicit operator SttpProducerMetadata(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
