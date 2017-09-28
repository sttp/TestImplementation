using System;
using Sttp.WireProtocol.Codec.DataPointPacket;
using Sttp.WireProtocol.Data.Raw;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class EncoderTCP
    {
        /// <summary>
        /// Once this size has been reached, the protocol will automatically call 
        /// </summary>
        private int m_autoFlushPacketSize;

        /// <summary>
        /// The bytes that need to be reliably sent. Note, this data is not valid until <see cref="Flush"/> has been called.
        /// </summary>
        private byte[] m_sendBuffer;

        /// <summary>
        /// The length of <see cref="m_sendBufferLength"/>
        /// </summary>
        private int m_sendBufferLength;

        /// <summary>
        /// Occurs when a new packet of data must be sent on the wire. This is called immediately
        /// after <see cref="Flush"/> or when the <see cref="m_autoFlushPacketSize"/> has been exceeded.
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        public MetadataEncoder Metadata { get; private set; }

        public DataPointEncoder DataPoint { get; private set; }

        public SubscriptionEncoder Subscription { get; private set; }

        public NegotiateSessionEncoder NegotiateSession { get; private set; }

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        /// <param name="autoflushPacketSize"></param>
        public EncoderTCP(int autoflushPacketSize)
        {
            m_autoFlushPacketSize = autoflushPacketSize;
            Subscription = new SubscriptionEncoder(SendPacket);
            DataPoint = new DataPointEncoder(SendPacket);
            NegotiateSession = new NegotiateSessionEncoder(SendPacket);
            Metadata = new MetadataEncoder(SendPacket);
        }

        internal void SendPacket(byte[] data, int position, int length)
        {
            NewPacket?.Invoke(data, position, length);

            //If this was UDP, I'd have to split this data into separate packets if it were too big, but since this is TCP, I can just send
            //the entire thing on the wire.
        }

        //public void CommandSuccess(CommandCode command, string response)
        //{
        //  //ToDO: Reconsider
        //}

        //public void CommandFailed(CommandCode command, string response)
        //{
        //  //ToDO: Reconsider
        //}

        //public void NoOp()
        //{
        //  //ToDO: Change to a heartbeat:
        //}


    }
}
