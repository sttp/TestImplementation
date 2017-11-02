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
    public class SttpDataPointNew
    {
        public SttpDataPoint PointID;
        public SttpValue Time;
        public uint TimeQuality;
        public SttpValue Value;
        public uint ValueQuality;
    }
}
