namespace Sttp.WireProtocol
{
    /// <summary>
    /// Represents the fundamental data types supported by STTP.
    /// Higher level types must be derived from these types.
    /// </summary>
    public enum SttpFundamentalTypeCode : byte
    {
        Null = 0,     //All values are nullable.
        Int64 = 1,    //Int64, UInt64, SByte, Byte, Int16, Int32, UInt16, UInt32, Bool, Char
        Single = 2,   //Single
        Double = 3,   //Double
        String = 4,   //Strings
        Buffer = 5,   //DateTime, TimeSpan, Guid, Decimal, Custom User Defined Types
    }
}