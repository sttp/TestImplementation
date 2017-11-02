using System;
using System.Runtime.InteropServices;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public class SttpDataPointNew
    {
        /// <summary>
        /// The runtimeID associated with the PointID. This will map to a <see cref="SttpPointID"/> that is either globally defined (if positive) or session defined (if negative)
        /// </summary>
        public int RuntimePointID;
        /// <summary>
        /// A 64 or 128 bit timestamp. This combined with RuntimeID is intended to always uniquely define a measurement.
        /// </summary>
        public SttpTimestamp Time;

        public SttpValue Value;

        /// <summary>
        /// 32-bits for identifying the quality of the time.
        /// </summary>
        public uint TimeQuality;
        /// <summary>
        /// 32-bits for identifying the quality of the value.
        /// </summary>
        public uint ValueQuality;
    }
}
