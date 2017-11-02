namespace Sttp.WireProtocol
{
    /// <summary>
    /// Represents the fundamental data types supported by STTP.
    /// Higher level types must be derived from these types.
    /// </summary>
    public enum SttpFundamentalTypeCode : byte
    {
        Null = 0,           //All values are nullable.
        Int64 = 1,          //Int64, SByte, Int16, Int32
        //Note: I have UInt64 and Int64 because when I was encoding, I was already having to detect sign and add a bit for it. So no space is saved.
        //but special treatment should occur for signed and unsigned values. Such as overflow detection.
        UInt64 = 2,         //UInt64, Byte, UInt16, UInt32, Bool, Char (According to .NET source code, it's unsigned. Look at the IL Code for Char.CompareTo())
        Single = 3,         //Single
        Double = 4,         //Double
        Buffer = 5,         //ScientificTime, DateTime, DateTimeOffset, Time, TimeSpan, Numeric, Custom User Defined Types
    }
}