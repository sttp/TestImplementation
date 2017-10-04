using System;
using Sttp.WireProtocol.Codec.DataPointPacket;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class EncoderUDP
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

        private int m_largeSequenceIndex;
        private byte[] m_buffer = new byte[2000];

        /// <summary>
        /// Occurs when a new packet of data must be sent on the wire. This is called immediately
        /// after <see cref="Flush"/> or when the <see cref="m_autoFlushPacketSize"/> has been exceeded.
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        public DataPointEncoder DataPoint { get; private set; }

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        /// <param name="autoflushPacketSize"></param>
        public EncoderUDP(int autoflushPacketSize)
        {
            m_autoFlushPacketSize = autoflushPacketSize;
            DataPoint = new DataPointEncoder(SendPacket);
        }

        internal void SendPacket(byte[] data, int position, int length)
        {
            if (length > m_autoFlushPacketSize)
            {
                //Segment the frame;
                for (int x = 0; x < length / 1000; x++)
                {
                    m_buffer[0] = (byte)CommandCode.Fragment;
                    BigEndian.CopyBytes(1013, m_buffer, 1);
                    BigEndian.CopyBytes(m_largeSequenceIndex++, m_buffer, 5);
                    BigEndian.CopyBytes(length, m_buffer, 9);
                    BigEndian.CopyBytes(x, m_buffer, 13);
                    Array.Copy(data, position + 1000 * x, m_buffer, 13, 1000);
                    NewPacket?.Invoke(data, position, length);
                }
                return;
            }
            NewPacket?.Invoke(data, position, length);
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
