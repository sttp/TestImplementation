
namespace Sttp.WireProtocol
{
    public enum CommandCode : byte
    {
        NegotiateSession = 0x00,
        MetadataRefresh = 0x01,
        Subscribe = 0x02,
        Unsubscribe = 0x03,
        SecureDataChannel = 0x04,
        RuntimeIDMapping = 0x05,
        DataPointPacket = 0x06,
        /// <summary>
        /// Indicates that a fragmented packet is being sent
        /// </summary>
        Fragment = 0x07,
        NoOp = 0xFF,
    }
}