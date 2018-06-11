using System;

namespace CTP
{
    /// <summary>
    /// This header is only valid if the channel number is less than 16 and the packet length is less than 1024 bytes.
    /// </summary>
    [Flags]
    internal enum CtpHeader0 : ushort
    {
        /// <summary>
        /// The header that is used. 
        /// </summary>
        HeaderVersion = 1 << 15,

        /// <summary>
        /// Indicates if this packet is compressed. 
        /// 0: None
        /// 1: Deflate
        /// </summary>
        IsCompressed = 1 << 14,

        /// <summary>
        /// The mask to decode the channel number
        /// </summary>
        ChannelNumberMask = 1 << 13 | 1 << 12 | 1 << 11 | 1 << 10,

        /// <summary>
        /// The number of bits to shift the channel number mask.
        /// </summary>
        ChannelNumberShiftBits = 10,

        /// <summary>
        /// The mask in the packet length
        /// </summary>
        PacketLengthMask = (1 << 10) - 1,

        /// <summary>
        /// None of the flags are set. Note, this state is valid.
        /// </summary>
        None = 0
    }

    [Flags]
    internal enum CtpHeader1 : byte
    {
        /// <summary>
        /// The header that is used. 
        /// </summary>
        HeaderVersion = 1 << 7,

        /// <summary>
        /// This bit is reserved for future use. It must be 0.
        /// </summary>
        ReservedBit = 1 << 6,

        /// <summary>
        /// Indicates if this packet is compressed. 
        /// 0: None
        /// 1: Deflate
        /// </summary>
        IsCompressed = 1 << 5,

        /// <summary>
        /// A mask indicating the number of bytes required for the Packet Length
        /// Range is 1-8, not 0-7.
        /// </summary>
        PacketLengthMask = 1 << 4 | 1 << 3,

        /// <summary>
        /// A mask indicating the number of bytes required for the Channel Number
        /// Range is 1-8, not 0-7.
        /// </summary>
        ChannelNumberLengthMask = 7,

        /// <summary>
        /// None of the flags are set.
        /// </summary>
        None = 0
    }
}