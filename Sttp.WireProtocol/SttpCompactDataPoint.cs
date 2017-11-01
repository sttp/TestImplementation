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
  
}
