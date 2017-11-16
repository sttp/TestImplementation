using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Limits are in place to prevent misuse of the protocol. It also limits the exposure to a server
    /// whose client's are abusing the server's resources. 
    /// </summary>
    public class ProtocolLimits
    {
        private int m_metadataRequestSizeLimit = 100_000;
        private int m_metadataResponseSizeLimit = 10_000_000;
        private int m_metadataSchemaSizeLimit = 100_000;

        /// <summary>
        /// The maximum size of a GetTable,GetQuery,SyncDatabase or SyncTableOrQuery command.
        /// This limit is before compression.
        /// </summary>
        public int MetadataRequestSizeLimit
        {
            get
            {
                return m_metadataRequestSizeLimit;
            }
            set
            {
                ValidateLimit(0, 10_000_000, value);
                m_metadataRequestSizeLimit = value;
            }
        }

        /// <summary>
        /// Responses to metadata commands cannot exceed this threshold.
        /// This limit is before compression.
        /// </summary>
        public int MetadataResponseSizeLimit
        {
            get
            {
                return m_metadataResponseSizeLimit;
            }
            set
            {
                ValidateLimit(0, 100_000_000, value);
                m_metadataResponseSizeLimit = value;
            }
        }

        /// <summary>
        /// Responses to metadata commands cannot exceed this threshold.
        /// This limit is before compression.
        /// </summary>
        public int MetadataSchemaSizeLimit
        {
            get
            {
                return m_metadataSchemaSizeLimit;
            }
            set
            {
                ValidateLimit(0, 1_000_000, value);
                m_metadataSchemaSizeLimit = value;
            }
        }

        private void ValidateLimit(int min, int max, int value)
        {
            if (value < min)
                throw new ArgumentOutOfRangeException(nameof(value), $"Specified value of {value} is not in the range of {min} to {max} (inclusive).");
        }
    }
}
