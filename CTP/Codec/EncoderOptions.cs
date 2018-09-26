using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    /// <summary>
    /// Limits are in place to prevent misuse of the protocol.
    /// </summary>
    public class EncoderOptions
    {
        private int m_maximumCommandSize = 10_000_000;
        private int m_deflateThreshold = 300;

        /// <summary>
        /// The maximum size of any single command. This size is the uncompressed size.
        /// Must be between 10,000 and 100,000,000 (Inclusive)
        /// </summary>
        public int MaximumCommandSize
        {
            get
            {
                return m_maximumCommandSize;
            }
            set
            {
                ValidateLimit(10_000, 100_000_000, value);
                m_maximumCommandSize = value;
            }
        }

        private void ValidateLimit(int min, int max, int value)
        {
            if (value < min || value > max)
                throw new ArgumentOutOfRangeException(nameof(value), $"Specified value of {value} is not in the range of {min} to {max} (inclusive).");
        }
    }
}
