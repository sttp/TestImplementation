using System;
using System.IO;
using System.Threading;

namespace CTP
{
    //ToDo: Final Review: Done
    /// <summary>
    /// Serializes <see cref="CtpPacket"/> messages from a <see cref="Stream"/>.
    /// </summary>
    internal class CtpStreamReaderAsync : IDisposable
    {
        public event Action<object, Exception> OnException;

        public event Action<CtpPacket> NewPacket;

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

        private AsyncCallback m_endRead;
        private WaitCallback m_beginRead;

        private bool m_started;

        /// <summary>
        /// Creates a <see cref="CtpStreamReader"/>
        /// </summary>
        /// <param name="stream"></param>
        public CtpStreamReaderAsync(Stream stream)
        {
            m_stream = stream;
            m_inboundBuffer = new byte[128];
            m_readBuffer = new byte[3000];
            m_beginRead = BeginRead;
            m_endRead = EndRead;
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        public void Start()
        {
            if (m_started)
                throw new Exception("Already started");
            m_started = true;
            ThreadPool.QueueUserWorkItem(m_beginRead, null);
        }

        private void BeginRead(object state)
        {
            try
            {
                IAsyncResult async = m_stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, m_endRead, null);
            }
            catch (Exception e)
            {
                OnException?.Invoke(this, e);
            }
        }

        private void EndRead(IAsyncResult ar)
        {
            try
            {
                int length = m_stream.EndRead(ar);
                if (length == 0)
                    throw new EndOfStreamException("The stream has been closed");

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

                while (InternalRead(out var packet))
                {
                    try
                    {
                        NewPacket?.Invoke(packet);
                    }
                    catch (Exception e)
                    {

                    }
                }

                if (ar.CompletedSynchronously)
                    ThreadPool.QueueUserWorkItem(m_beginRead, null);
                else
                    BeginRead(null);
            }
            catch (Exception e)
            {
                OnException?.Invoke(this, e);
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

        public void Dispose()
        {
        }

    }
}
