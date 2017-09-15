using System;

namespace Sttp.WireProtocol
{
    [Flags]
    public enum QualityFlags
    {
        //SEC: We should include an IsNull flag for empty values. Maybe an IsMissing could fit.
        Normal = 0,                 // Defines normal state
        BadTime = 1 << 0,           // Defines bad time state
        BadValue = 1 << 1,          // Defines bad value state
        UnreasonableValue = 1 << 2, // Defines unreasonable value state
        CalculatedValue = 1 << 3,   // Defines calculated value state
        ReservedFlag1 = 1 << 4,     // Defines reserved flag 1
        ReservedFlag2 = 1 << 5,     // Defines reserved flag 1
        UserDefinedFlag1 = 1 << 6,  // Defines user defined flag 1
        UserDefinedFlag2 = 1 << 7   // Defines user defined flag 1
    }
    // sizeof(uint8), 1-byte
}
