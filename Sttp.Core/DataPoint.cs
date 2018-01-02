using System;
using System.Text;

namespace Sttp
{
    /// <summary>
    /// Represents a single point of data.
    /// 
    /// ToDo: Get rid of this class.
    /// </summary>
    public class DataPoint
    {
        public DataPointKey Key;
        public byte[] Value;
        public int ValueLength;
        public DateTime Time;
        public TimeQualityFlags Flags;
        public ValueQualityFlags QualityFlags;
    }
}
