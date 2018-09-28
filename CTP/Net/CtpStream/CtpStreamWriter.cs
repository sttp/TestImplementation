using System;
using System.IO;
using System.Threading;

namespace CTP
{
    //ToDo: Final Review: Done

    /// <summary>
    /// Serializes <see cref="CtpPacket"/> messages to a <see cref="Stream"/> in a synchronous manner.
    /// </summary>
    internal class CtpStreamWriter : IDisposable
    {
        public event Action<object, Exception> OnException;

        private object m_writeLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// The buffer used to write packets to the stream.
        /// </summary>
        private byte[] m_writeBuffer;

        private int m_writeTimeout = 0;

        /// <summary>
        /// Creates a <see cref="CtpStreamWriter"/>
        /// </summary>
        /// <param name="stream"></param>
        public CtpStreamWriter(Stream stream)
        {
            m_stream = stream;
            m_writeBuffer = new byte[128];
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
        /// Writes a packet to the underlying stream. Note: this method blocks until a packet has successfully been sent.
        /// </summary>
        public void Write(CtpPacket packet, int timeout)
        {
            try
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
                    if (m_writeTimeout != timeout)
                    {
                        m_writeTimeout = timeout;
                        stream.WriteTimeout = timeout;
                    }
                    stream.Write(m_writeBuffer, 0, totalSize);
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
                throw;
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

        public void Dispose()
        {
        }
    }
}
