using System;
using System.Runtime.InteropServices;
using System.Text;
using CTP;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental unit of transmission for STTP.
    /// The intention of this class is to be reusable. 
    /// </summary>
    public class SttpDataPoint
    {
        /// <summary>
        /// The metadata for a measurement
        /// </summary>
        public SttpDataPointMetadata Metadata;

        /// <summary>
        /// A timestamp field.
        /// </summary>
        public CtpTime Time;

        /// <summary>
        /// The value for the data point.
        /// </summary>
        public CtpObject Value;

        /// <summary>
        /// A quality field.
        /// </summary>
        public long Quality;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Metadata.DataPointID.ToString());
            sb.Append('\t');
            sb.Append(Time.ToString());
            sb.Append('\t');
            sb.Append(Value.ToString());
            sb.Append('\t');
            sb.Append(Quality.ToString());
            return sb.ToString();
        }
    }
}
