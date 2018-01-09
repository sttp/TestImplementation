using System;

namespace Sttp
{
    [Flags]
    public enum ValueQualityFlags : byte
    {
        Normal = 0,                 // Defines normal state
        BadTime = 1 << 0,           // Defines bad time state
        BadValue = 1 << 1,          // Defines bad value state
        UnreasonableValue = 1 << 2, // Defines unreasonable value state
        CalculatedValue = 1 << 3,   // Defines calculated value state
        MissingValue = 1 << 4,      // Defines a missing value
        ReservedFlag = 1 << 5,      // Defines a reserved flag
        UserDefinedFlag1 = 1 << 6,  // Defines user defined flag 1
        UserDefinedFlag2 = 1 << 7   // Defines user defined flag 1
    }
    // sizeof(uint8), 1-byte
}
