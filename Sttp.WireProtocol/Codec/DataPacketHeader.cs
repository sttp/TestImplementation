using System;

namespace Sttp.Codec
{
    // A custom 2 byte header exists if the 7th bit of the first byte is 0.
    // This header is a 2 byte big-endian value where bits:
    //  0-9 Payload Length (<= 1023 bytes)
    //  10-14 RawPacketCode (<= 31)
    //  15: By definition, always 0
    [Flags]
    public enum DataPacketHeader : byte
    {
        ///
        /// Header Format:
        /// 
        /// (byte) DataPacketHeader
        /// 
        /// If (Fragment Type = 'First Fragment')
        ///     (int) Total Fragment Size     - The size of all of the fragments that will be received. (Not uncompressed size)
        /// 
        /// If (Fragment Type = 'First Fragment' || Fragment Type = 'Not Fragmented')
        ///     If (IsCompressed = 1)
        ///        (int) Total Raw Size     - The size of the uncompressed data. Including all fragments.
        ///  
        ///     If (Packet Type = Raw Payload)
        ///         (byte) Payload Identifier
        ///     else 
        ///         (byte) Metadata Root Command String Length
        ///         (Ascii) Metadata Root Command
        ///
        ///  
        /// 
        /// (byte[Payload Length]) Payload
        /// 
        /// 
        /// First 3 bits are reserved to identify the packet type.
        /// 
        /// values:
        /// 0: Special 2 byte header.
        /// 1: Special 2 byte header.
        /// 2: Special 2 byte header.
        /// 3: Special 2 byte header.
        /// 4: Not Fragmented
        /// 5: First Fragment
        /// 6: Next Fragment
        /// 7: Unused
        Special2ByteHeader = 0 << 5,
        NotFragmented = 4 << 5,
        BeginFragment = 5 << 5,
        NextFragment = 6 << 5,
        PacketTypeMask = 7 << 5,

        /// <summary>
        /// A flag designating the type of packet. 
        /// 0: Raw Payload
        /// 1: MarkupCommand
        /// </summary>
        IsMarkupCommand = 16,

        /// <summary>
        /// Indicates if this packet is compressed. 
        /// 0: None
        /// 1: Deflate
        /// </summary>
        IsCompressed = 8,

        None = 0,
    }
}