using System;
using System.IO;
using System.Threading;
using GSF;

namespace CTP
{
    //ToDo: Final Review: Done
    /// <summary>
    /// Serializes <see cref="CtpPacket"/> messages from a <see cref="Stream"/>.
    /// </summary>
    internal class CtpStreamReader : IDisposable
    {
        public event Action<object, Exception> OnException;

        private object m_readLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

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

        private int m_readTimeout = 0;

        /// <summary>
        /// Creates a <see cref="CtpStreamReader"/>
        /// </summary>
        /// <param name="stream"></param>
        public CtpStreamReader(Stream stream)
        {
            m_stream = stream;
            m_inboundBuffer = new byte[128];
            m_readBuffer = new byte[3000];
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// Reads the next packet from the stream. 
        /// </summary>
        /// <returns></returns>
        public CtpDocument Read(int timeout)
        {
            try
            {
                lock (m_readLock)
                {
                    CtpDocument packet;
                    while (!InternalRead(out packet))
                    {
                        var stream = m_stream;
                        if (stream == null)
                            throw new ObjectDisposedException("Stream has been closed");
                        if (m_readTimeout != timeout)
                        {
                            m_readTimeout = timeout;
                            stream.ReadTimeout = timeout;
                        }
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
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
                throw;
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
        private bool InternalRead(out CtpDocument packet)
        {
            packet = null;
            if (m_inboundBufferLength < 2)
                return false;

            byte header = m_inboundBuffer[m_inboundBufferCurrentPosition];
            if (header > 63)
                throw new Exception("Unknown Packet Header Version");

            bool longPayload = (header & 16) > 0;
            int length = 0;

            if (!longPayload)
            {
                length = BigEndian.ToInt16(m_inboundBuffer, m_inboundBufferCurrentPosition) & ((1 << 12) - 1);
            }
            else
            {
                if (m_inboundBufferLength < 4)
                    return false;
                length = BigEndian.ToInt32(m_inboundBuffer, m_inboundBufferCurrentPosition) & ((1 << 28) - 1);
            }

            if (length > MaximumPacketSize)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < length)
                return false;

            byte[] payload;
            payload = new byte[length];
            Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition, payload, 0, length);

            packet = new CtpDocument(payload, false);
            m_inboundBufferCurrentPosition += length;
            m_inboundBufferLength -= length;
            return true;
        }

        public void Dispose()
        {
        }
    }
}
