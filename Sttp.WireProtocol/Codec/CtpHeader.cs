using System;

namespace CTP
{
    [Flags]
    public enum CtpHeader : ushort
    {
        /// <summary>
        /// Indicates if this packet is compressed. 
        /// 0: None
        /// 1: Deflate
        /// </summary>
        IsCompressed = 1 << 15,
        /// <summary>
        /// A flag designating the type of packet. 
        /// 0: Raw Payload
        /// 1: MarkupCommand
        /// </summary>
        IsFragmented = 1 << 14,
        CommandMask = CommandDocument,
        CommandBinary0 = 0 << 12,
        CommandBinary1 = 1 << 12,
        CommandRawInt32 = 2 << 12,
        CommandDocument = 3 << 12,
        PacketLengthMask = (1 << 12) - 1,

        None = 0,
    }
}