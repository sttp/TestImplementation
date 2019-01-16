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

        /// <summary>
        /// An extra field in the event that one is needed. It's not recommended to use this field for complex value types, but rather
        /// if data must be transported with this value that does not fit in one of the other categories (PointID, Time, Value, Quality)
        /// Examples include:
        ///   Some kind of sequence identifier.
        ///   Some kind of pivot field. 
        ///   Extra quality data or analysis.
        ///   Metadata that must be transported with every measurement. (Caution, this will introduce substantial overhead)
        /// 
        /// Note: Since this field is considered ancillary assigning something other than null will introduce a penalty that may be considerable in some circumstances.
        /// </summary>
        public CtpObject ExtendedData;

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
