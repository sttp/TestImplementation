namespace Sttp.WireProtocol
{
    /// <summary>
    /// Represents the fundamental data types supported by STTP.
    /// Higher level types must be derived from these types.
    /// </summary>
    public enum SttpFundamentalTypeCode : byte
    {
        Null = 0,     //All values are nullable.
        Int32 = 1,    //SByte, Byte, Int16, Int32, UInt16, UInt32, Bool, Char
        Int64 = 2,    //Int64, UInt64
        Single = 3,   //Single
        Double = 4,   //Double
        Buffer = 5,   //DateTime, TimeSpan, Strings, Guid, Decimal, Custom User Defined Types
    }
}