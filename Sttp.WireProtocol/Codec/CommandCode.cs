
using System;

namespace Sttp.Codec
{
    // A custom 2 byte header exists if the 7th bit if the first byte is 0.
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
        PacketTypeMask = 7<<5,

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


    /// <summary>
    /// The lowest level command codes supported by the protocol. Nearly all commands exposed to higher level APIs will be in the form of a MarkupCommand.
    /// </summary>
    public enum CommandCode : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        ///// <summary>
        ///// Indicates that a fragmented packet is being sent. Fragmented packets 
        ///// are for the wire protocol to ensure every packet fits the desired transport size. 
        ///// This functionality is handled internally and not exposed for use. To send large packets
        ///// over the wire, the user must send their data with the BulkTransport command.
        ///// 
        ///// Fragmented packets must be sent one at a time in sequence and cannot be 
        ///// interwoven with any other kind of packet.
        ///// 
        ///// Fragments are limited to a size configure in the SessionDetails, but is on the order 
        ///// of MB's.
        ///// 
        ///// Fragmented packets can have a compression=none.
        ///// 
        ///// If TotalFragmentSize == [Length of first fragment], this first fragment is the only fragment.
        ///// This occurs when the packet is compressed, but not fragmented.
        ///// 
        ///// Layout:
        ///// int TotalFragmentSize     - The size of all fragments.
        ///// int TotalRawSize          - The uncompressed data size.
        ///// byte CommandCode          - The Command of the data that is encapsulated.
        ///// byte CompressionMode      - The algorithm that is used to compress the data.
        ///// (Implied) Length of first fragment
        ///// byte[] firstFragment
        ///// 
        ///// </summary>
        //BeginFragment,

        ///// <summary>
        ///// Specifies the next fragment of data. When Offset + Length of Fragment == TotalFragmentSize, the fragment is completed.
        ///// Since fragments are sequential, the offset is known, and the length is computed from the packet overhead.
        ///// 
        ///// Layout:
        ///// (Implied) Offset position in Fragment
        ///// (Implied) Length of fragment
        ///// byte[] Fragment               
        ///// </summary>
        //NextFragment,

        /// <summary>
        /// Streaming of real-time data. This command is extremely simplified since 
        /// the payload for this kind of data is small, so overhead is more costly.
        /// 
        /// Payload:
        /// byte encodingMethod 
        /// byte[] Data;
        /// 
        /// </summary>
        SubscriptionStream,

        /// <summary>
        /// All other commands fall under the classification of Markup commands. These use the SttpMarkup Language
        /// that has been optimized to exchange data in a binary format rather than strings. It also follows a more strict
        /// format than XML, YAML, or JSON. 
        /// 
        /// The benefit of Markup Commands over raw binary streams is that all commands can be properly serialized, even if the command is not
        /// recognized. It also greatly simplifies the wire protocol level and keeps the lowest level from changing when additional commands are added in the future.
        /// 
        /// Payload:
        /// SttpMarkup Command; (Included as the first byte of this command is an ASCII length prefixed string as the name of the command)
        /// </summary>
        MarkupCommand,

    }
}