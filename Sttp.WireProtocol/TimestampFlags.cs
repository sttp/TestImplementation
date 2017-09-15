using System;

namespace Sttp.WireProtocol
{
    [Flags]
    public enum TimestampFlags
    {
        None = 0,
        TimeQualityMask = 0xF,        // Mask for TimeQuality
        //SEC: I'm not sure we should complicate the LeapSecond issue by requiring all of this information about leap seconds.
        //     With how poorly the industry has handled leap seconds, I'd be happy if they just set the LeapSecondOccurring flag 
        //     when they repeat the leap second. Forget the Forward/Reverse flag. If an application really cares to know this information,
        //     they should maintain their own table of when leap seconds occur and use that as flags. I'd venture to say that this would
        //     be more accurate then relying on a vendor to remember to set the LeapSecondOccurred flag and hold it high for 24 hours.
        LeapsecondPending = 1 << 4,   // Set before a leap second occurs and then cleared after
        LeapsecondOccurred = 1 << 5,  // Set in the first second after the leap second occurs and remains set for 24 hours
        LeapsecondDirection = 1 << 6, // Clear for add, set for delete
        NoAccurateTimeSource = 1 << 7 // Accurate time source is unavailable
    }
    // sizeof(uint8), 1-byte
}
