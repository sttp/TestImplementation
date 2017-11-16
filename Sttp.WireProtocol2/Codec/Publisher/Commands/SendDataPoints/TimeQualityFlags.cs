using System;

namespace Sttp
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


    }// sizeof(uint8), 1-byte

    public enum TimeQuality : byte
    {
        Locked = 0x0,                       // Clock locked, Normal operation
        Failure = 0xF,                      // Clock fault, time not reliable
        Unlocked10Seconds = 0xB,            // Clock unlocked, time within 10^1s
        Unlocked1Second = 0xA,              // Clock unlocked, time within 10^0s
        UnlockedPoint1Seconds = 0x9,        // Clock unlocked, time within 10^-1s
        UnlockedPoint01Seconds = 0x8,       // Clock unlocked, time within 10^-2s
        UnlockedPoint001Seconds = 0x7,      // Clock unlocked, time within 10^-3s
        UnlockedPoint0001Seconds = 0x6,     // Clock unlocked, time within 10^-4s
        UnlockedPoint00001Seconds = 0x5,    // Clock unlocked, time within 10^-5s
        UnlockedPoint000001Seconds = 0x4,   // Clock unlocked, time within 10^-6s
        UnlockedPoint0000001Seconds = 0x3,  // Clock unlocked, time within 10^-7s
        UnlockedPoint00000001Seconds = 0x2, // Clock unlocked, time within 10^-8s
        UnlockedPoint000000001Seconds = 0x1 // Clock unlocked, time within 10^-9s
    } // 4-bits, 1-nibble

}
