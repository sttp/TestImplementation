using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    [Flags]
    public enum TimeQualityFlags : byte
    {
        None = 0,
        TimeQualityMask = 0xF,          // Mask for TimeQuality
        // TODO: Discuss usefulness of adding IEEE bits for leap-seconds
        //LeapsecondPending = 1 << 4,    // Set before a leap second occurs and then cleared after
        //LeapsecondOccurred = 1 << 5,   // Set in the first second after the leap second occurs and remains set for 24 hours
        //LeapsecondDirection = 1 << 6,  // Clear for add, set for delete
        NoAccurateTimeSource = 1 << 7   // Accurate time source is unavailable
    }
    // sizeof(uint8), 1-byte
}
