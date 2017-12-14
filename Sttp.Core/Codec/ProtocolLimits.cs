using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    /// <summary>
    /// Limits are in place to prevent misuse of the protocol. It also limits the exposure to a server
    /// whose client's are abusing the server's resources. 
    /// </summary>
    public class ProtocolLimits
    {
        private int m_maxPacketSize = 10_000_000;
        private int m_maxDataPointSize = 10_000;
        private int m_maxMetadataRecordValue = 1_000_000;

        /// <summary>
        /// The maximum size of a single packet of any type.
        /// </summary>
        public int MaxPacketSize
        {
            get
            {
                return m_maxPacketSize;
            }
            set
            {
                ValidateLimit(0, 100_000_000, value);
                m_maxPacketSize = value;
            }
        }

        /// <summary>
        /// The maximum size of a SttpDataPoint value that can be sent.
        /// </summary>
        public int MaxDataPointSize
        {
            get
            {
                return m_maxDataPointSize;
            }
            set
            {
                ValidateLimit(0, 1_000_000, value);
                m_maxDataPointSize = value;
            }
        }

        /// <summary>
        /// The maximum size of a single field of the metadata.
        /// </summary>
        public int MaxMetadataRecordValue
        {
            get
            {
                return m_maxMetadataRecordValue;
            }
            set
            {
                ValidateLimit(0, 10_000_000, value);
                m_maxMetadataRecordValue = value;
            }
        }

        private void ValidateLimit(int min, int max, int value)
        {
            if (value < min)
                throw new ArgumentOutOfRangeException(nameof(value), $"Specified value of {value} is not in the range of {min} to {max} (inclusive).");
        }
    }
}
