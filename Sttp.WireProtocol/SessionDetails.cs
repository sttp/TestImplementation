using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Contains the details negotiated by the session.
    /// </summary>
    public class SessionDetails
    {

        public readonly ProtocolLimits Limits = new ProtocolLimits();

        /// <summary>
        /// Indicate that the protocol supports deflate.
        /// </summary>
        public bool SupportsDeflate = false;

        /// <summary>
        /// The number of bytes before deflate kicks in.
        /// </summary>
        public int DeflateThreshold = 1000;

        /// <summary>
        /// The maximum size of every packet. After this threshold, the packet will be fragmented.
        /// </summary>
        public int MaximumSegmentSize = 1500;

        /// <summary>
        /// Only points with RuntimeIDs smaller than this will be cached. All IDs greater than this will
        /// have their RuntimeID serialized with each measurement and suffer a bandwidth penalty. Publishers
        /// with more than this many points should consider mapping their most common and most frequent measurements 
        /// into this space.
        /// </summary>
        public int MaxRuntimeIDCache = 5000; 

        /// <summary>
        /// This variable is not negotiated.
        /// </summary>
        public int NextFragmentID = 0;

        /// <summary>
        /// For the default compression method, this identifies the most common first byte serialized.
        /// </summary>
        public byte MostCommonFirstByte = 9;
    }
}
