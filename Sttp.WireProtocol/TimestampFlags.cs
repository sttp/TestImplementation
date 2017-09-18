using System;

namespace Sttp.WireProtocol
{
    [Flags]
    public enum TimestampFlags : byte
    {
        None = 0,
        TimeQualityMask = 0xF,          // Mask for TimeQuality
        LeapsecondInProgress = 1 << 4,  // Set when leap second is occurring, i.e., current second is duplicated
        NoAccurateTimeSource = 1 << 5   // Accurate time source is unavailable
    }
    // sizeof(uint8), 1-byte
}
