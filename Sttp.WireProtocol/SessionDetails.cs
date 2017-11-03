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
        public bool SupportsDeflate = false;
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
    }
}
