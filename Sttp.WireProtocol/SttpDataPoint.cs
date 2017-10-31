using System;
using System.Runtime.InteropServices;

namespace Sttp.WireProtocol.SendDataPoints
{
    /// <summary>
    /// This kind of data point can be extremely compact.
    /// </summary>
    public class SttpCompactDataPoint
    {
        public long ShortTime;     //Same scale as DateTime. Must be universal time. Bit63 is reserved for LeapSecondInProgress. 
        public uint DataPointID;
        public uint Value;         //Must be a 32 bit value or smaller.
        public uint TimeQuality;   //32-bit user defined time quality flags. These must rarely change.
        public uint ValueQuality;
    }   //24 bytes in size.

    /// <summary>
    /// This is the normal fully loaded data point. Not so compact.
    /// </summary>
    public class SttpDataPoint
    {
        public uint DataPointID;
        public SttpTimestamp Time;
        public byte ValueLength;
        public SttpValue Value = new SttpValue();
        public uint TimeQuality;
        public uint ValueQuality;

        /// <summary>
        /// If this data cannot fit in a 64 byte payload, 
        /// it will come out of bounds at a later time.
        /// </summary>
        public uint LargeValueID;
    }

}
