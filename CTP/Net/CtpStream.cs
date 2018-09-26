using System;
using System.IO;
using System.Threading;

namespace CTP
{
    //ToDo: Final Review: Done

    /// <summary>
    /// Serializes <see cref="CtpPacket"/> messages to/from a <see cref="Stream"/>.
    /// Note: this class is thread safe, however, concurrent calls to either the Read or Write methods will block each other.
    /// Read and Write will no block each other, and disposing of the stream will not block.
    /// </summary>
    public class CtpStream : IDisposable
    {
        private object m_writeLock = new object();

        private object m_readLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// The buffer used to write packets to the stream.
        /// </summary>
        private byte[] m_writeBuffer;
        /// <summary>
        /// Raw unprocessed data received from the client.
        /// </summary>
        private byte[] m_inboundBuffer;
        /// <summary>
        /// The current position of the inbound buffer.
        /// </summary>
        private int m_inboundBufferCurrentPosition;
        /// <summary>
        /// The number of unconsumed bytes in the inbound buffer.
        /// </summary>
        private int m_inboundBufferLength;
        /// <summary>
        /// A buffer used to read data from the underlying stream.
        /// </summary>
        private byte[] m_readBuffer;

        /// <summary>
        /// Creates a <see cref="CtpStream"/>
        /// </summary>
        /// <param name="stream"></param>
        public CtpStream(Stream stream)
        {
            m_stream = stream;
            m_writeBuffer = new byte[128];
            m_inboundBuffer = new byte[128];
            m_readBuffer = new byte[3000];
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// The payload length field can consume anywhere from 1 to 4 bytes.
        /// </summary>
        /// <param name="payloadLength"></param>
        /// <returns></returns>
        private int LengthOfPayloadLength(int payloadLength)
        {
            if (payloadLength <= 0xFF)
                return 1;
            if (payloadLength <= 0xFFFF)
                return 2;
            if (payloadLength <= 0xFFFFFF)
                return 3;
            return 4;
        }

        /// <summary>
        /// Writes a packet to the underlying stream. Note: this method blocks until a packet has sucessfully been sent.
        /// </summary>
        public void Write(CtpPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            //In case of an overflow exception.
            int payloadLengthBytes = LengthOfPayloadLength(packet.Payload.Length);
            int totalSize = packet.Payload.Length + 1 + payloadLengthBytes;
            if (totalSize > MaximumPacketSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            lock (m_writeLock)
            {
                EnsureCapacity(totalSize);
                Array.Copy(packet.Payload, 0, m_writeBuffer, 1 + payloadLengthBytes, packet.Payload.Length);
                m_writeBuffer[0] = (byte)(packet.Channel + ((payloadLengthBytes - 1) << 4) + ((packet.IsRawData ? 1 : 0) << 6));
                switch (payloadLengthBytes)
                {
                    case 1:
                        m_writeBuffer[1] = (byte)packet.Payload.Length;
                        break;
                    case 2:
                        m_writeBuffer[1] = (byte)(packet.Payload.Length >> 8);
                        m_writeBuffer[2] = (byte)packet.Payload.Length;
                        break;
                    case 3:
                        m_writeBuffer[1] = (byte)(packet.Payload.Length >> 16);
                        m_writeBuffer[2] = (byte)(packet.Payload.Length >> 8);
                        m_writeBuffer[3] = (byte)packet.Payload.Length;
                        break;
                    case 4:
                        m_writeBuffer[1] = (byte)(packet.Payload.Length >> 24);
                        m_writeBuffer[2] = (byte)(packet.Payload.Length >> 16);
                        m_writeBuffer[3] = (byte)(packet.Payload.Length >> 8);
                        m_writeBuffer[4] = (byte)packet.Payload.Length;
                        break;
                }

                var stream = m_stream;
                if (stream == null)
                    throw new ObjectDisposedException("Stream has been closed");
                stream.Write(m_writeBuffer, 0, totalSize);
            }
        }

        /// <summary>
        /// Ensures that <see cref="m_writeBuffer"/> has at least the supplied number of bytes
        /// before returning.
        /// </summary>
        /// <param name="bufferSize"></param>
        private void EnsureCapacity(int bufferSize)
        {
            if (m_writeBuffer.Length < bufferSize)
            {
                //12% larger than the requested buffer size.
                byte[] newBuffer = new byte[bufferSize + (bufferSize >> 3)];
                m_writeBuffer.CopyTo(newBuffer, 0);
                m_writeBuffer = newBuffer;
            }
        }

        /// <summary>
        /// Reads the next packet from the stream. 
        /// </summary>
        /// <returns></returns>
        public CtpPacket Read()
        {
            lock (m_readLock)
            {
                CtpPacket packet;
                while (!InternalRead(out packet))
                {
                    var stream = m_stream;
                    if (stream == null)
                        throw new ObjectDisposedException("Stream has been closed");
                    int length = stream.Read(m_readBuffer, 0, m_readBuffer.Length);
                    if (length == 0)
                        throw new EndOfStreamException("The stream has been shutdown");

                    if (m_inboundBufferCurrentPosition > 0 && m_inboundBufferLength != 0)
                    {
                        // Compact - trims all data before current position if position is in middle of stream
                        Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition, m_inboundBuffer, 0, m_inboundBufferLength);
                    }
                    m_inboundBufferCurrentPosition = 0;

                    int growSize = m_inboundBufferLength + length;
                    if (m_inboundBuffer.Length < growSize)
                    {
                        //12% larger than the requested buffer size.
                        byte[] newBuffer = new byte[growSize + (growSize >> 3)];
                        m_inboundBuffer.CopyTo(newBuffer, 0);
                        m_inboundBuffer = newBuffer;
                    }

                    Array.Copy(m_readBuffer, 0, m_inboundBuffer, m_inboundBufferLength, length);
                    m_inboundBufferLength += length;
                }
                return packet;
            }
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="FillBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        private bool InternalRead(out CtpPacket packet)
        {
            packet = null;
            if (m_inboundBufferLength < 2)
                return false;

            byte header = m_inboundBuffer[m_inboundBufferCurrentPosition];
            if (header > 127)
                throw new Exception("Unknown Packet Header Version");
            bool isRawData = ((header >> 6) & 1) == 1;
            int packetLengthBytes = ((header >> 4) & 3) + 1;
            if (m_inboundBufferLength < packetLengthBytes + 1)
                return false;

            int payloadLength;
            switch (packetLengthBytes)
            {
                case 1:
                    payloadLength = m_inboundBuffer[m_inboundBufferCurrentPosition + 1];
                    break;
                case 2:
                    payloadLength = (m_inboundBuffer[m_inboundBufferCurrentPosition + 1] << 8)
                                    + m_inboundBuffer[m_inboundBufferCurrentPosition + 2];
                    break;
                case 3:
                    payloadLength = (m_inboundBuffer[m_inboundBufferCurrentPosition + 1] << 16)
                                    + (m_inboundBuffer[m_inboundBufferCurrentPosition + 2] << 8)
                                    + m_inboundBuffer[m_inboundBufferCurrentPosition + 3];
                    break;
                case 4:
                    payloadLength = (m_inboundBuffer[m_inboundBufferCurrentPosition + 1] << 24)
                                    + (m_inboundBuffer[m_inboundBufferCurrentPosition + 2] << 16)
                                    + (m_inboundBuffer[m_inboundBufferCurrentPosition + 3] << 8)
                                    + m_inboundBuffer[m_inboundBufferCurrentPosition + 4];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (packetLengthBytes + 1 + payloadLength > MaximumPacketSize + 5)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < packetLengthBytes + 1 + payloadLength)
                return false;

            byte channel = (byte)(header & 15);
            byte[] payload;
            payload = new byte[payloadLength];
            Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition + 1 + packetLengthBytes, payload, 0, payloadLength);

            packet = new CtpPacket(channel, isRawData, payload);
            m_inboundBufferCurrentPosition += packetLengthBytes + 1 + payloadLength;
            m_inboundBufferLength -= packetLengthBytes + 1 + payloadLength;
            return true;
        }

        /// <summary>
        /// Disposes the underlying stream, and throws exceptions at any writer/readers.
        /// </summary>
        public void Dispose()
        {
            var stream = Interlocked.Exchange(ref m_stream, null);
            stream?.Dispose();
        }
    }
}
