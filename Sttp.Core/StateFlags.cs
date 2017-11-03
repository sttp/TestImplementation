using System;

namespace Sttp
{
    [Flags]
    public enum StateFlags : ushort
    {
        TimestampTypeMask = 0x3,      // Mask for TimestampType
        Quality = 1 << 2,             // State includes QualityFlags
        Sequence = 1 << 3,            // State includes sequence number as uint16
        PriorityMask = 0x30,          // Mask for Priority, get value with >> 4
        Reliability = 1 << 7,         // When set, data will use lossy communications
        Verification = 1 << 8,        // When set, data delivery will be verified
        Exception = 1 << 9,           // When set, data will be published on change
        Resolution = 1 << 10,         // When set, data will be down-sampled
        ResolutionTypeMask = 0x1800,  // Mask for ResolutionType
        KeyAction = 1 << 13,          // When set key is to be added; otherwise, removed
        ReservedFlag1 = 1 << 14,      // Reserved flag 1
        ReservedFlag2 = 1 << 15       // Reserved flag 2
    }
    // sizeof(uint16), 2-bytes
}
