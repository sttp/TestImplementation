
namespace Sttp.WireProtocol
{
    public enum CommandCode : byte
    {
        NegotiateSession = 0x01,
        Subscribe = 0x03,
        SecureDataChannel = 0x04,
        RuntimeIDMapping = 0x05,
        DataPointPacket = 0x06,
        /// <summary>
        /// Indicates that a fragmented packet is being sent
        /// </summary>
        Fragment = 0x07,
        NoOp = 0xFF,

        //Finalized Names;

        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,
        
        /// <summary>
        /// Contains Metadata related commands.
        /// </summary>
        Metadata = 0x02,
        /// <summary>
        /// Capable of sending large blocks of data over STTP.
        /// </summary>
        BulkTransport = 0x03,
    }
}