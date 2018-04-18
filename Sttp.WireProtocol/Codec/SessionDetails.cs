using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    /// <summary>
    /// Limits are in place to prevent misuse of the protocol. It also limits the exposure to a server
    /// whose clients are abusing the server's resources. 
    /// </summary>
    internal class SessionDetails
    {
        private int m_maximumCommandSize = 10_000_000;
        private int m_deflateThreshold = 1000;
        private int m_maximumPacketSize = 1500;

        /// <summary>
        /// Indicate that the protocol supports deflate.
        /// </summary>
        public bool SupportsDeflate = true;
        private int m_maxRuntimeIDCache = 5000;

        /// <summary>
        /// The number of bytes before deflate kicks in. 
        /// Must be between 100 and 100,000,000
        /// </summary>
        public int DeflateThreshold
        {
            get
            {
                return m_deflateThreshold;
            }
            set
            {
                ValidateLimit(100, 100_000_000, value);
                m_deflateThreshold = value;
            }
        }

        /// <summary>
        /// The maximum size of every atomic packet. After this threshold, the packet must be fragmented.
        /// Must be between 300 and 4095.
        /// </summary>
        public int MaximumPacketSize
        {
            get
            {
                return m_maximumPacketSize;
            }
            set
            {
                ValidateLimit(300, 4095, value);
                m_maximumPacketSize = value;
            }
        }

        /// <summary>
        /// The maximum size of any single command. This size is the uncompressed size.
        /// Must be between 1,000 and 100,000,000.
        /// </summary>
        public int MaximumCommandSize
        {
            get
            {
                return m_maximumCommandSize;
            }
            set
            {
                ValidateLimit(1_000, 100_000_000, value);
                m_maximumCommandSize = value;
            }
        }

        /// <summary>
        /// Only points with RuntimeIDs smaller than this will be cached. All IDs greater than this will
        /// have their RuntimeID serialized with each measurement and suffer a bandwidth penalty. Publishers
        /// with more than this many points should consider mapping their most common and most frequent measurements 
        /// into this space.
        /// 
        /// Must be between -1 and 1,000,000
        /// </summary>
        public int MaxRuntimeIDCache
        {
            get
            {
                return m_maxRuntimeIDCache;
            }
            set
            {
                ValidateLimit(-1, 1_000_000, value);
                m_maxRuntimeIDCache = value;
            }
        }

        private void ValidateLimit(int min, int max, int value)
        {
            if (value < min || value > max)
                throw new ArgumentOutOfRangeException(nameof(value), $"Specified value of {value} is not in the range of {min} to {max} (inclusive).");
        }
    }
}
