using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace CTP.Net
{
    public class CtpStream
    {
        /// <summary>
        /// Options for how commands will be compressed.
        /// </summary>
        private EncoderOptions m_encoderOptions;

        /// <summary>
        /// A buffer to use for all of the packets.
        /// </summary>
        private byte[] m_buffer;

        /// <summary>
        /// A stream for compressing data.
        /// </summary>
        private MemoryStream m_compressionStream = new MemoryStream();

        /// <summary>
        /// A reserved amount of packet overhead
        /// </summary>
        private const int BufferOffset = 1 + 3 + 1 + 20; //20, extra space for compression overhead.

        /// <summary>
        /// raw unprocessed data received from the client
        /// </summary>
        private byte[] m_inboundBuffer;
        /// <summary>
        /// The active position of the inbound buffer.
        /// </summary>
        private int m_inboundBufferCurrentPosition;
        /// <summary>
        /// The number of unconsumed bytes in the inbound buffer.
        /// </summary>
        private int m_inboundBufferLength;

        /// <summary>
        /// A buffer for extracting a compressed packet.
        /// </summary>
        private byte[] m_compressionBuffer;

        public event Action DataReceived;

        public CtpDecoderResults Results = new CtpDecoderResults();

        private Stream m_stream;

        private byte[] m_inBuffer = new byte[3000];
        private bool m_isReading;
        private object m_syncReceive = new object();
        private AsyncCallback m_asyncReadCallback;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public CtpStream()
        {
            m_encoderOptions = new EncoderOptions();
            m_buffer = new byte[128];
            m_compressionBuffer = new byte[128];
            m_inboundBuffer = new byte[128];
            m_onDataReceived = OnDataReceived;
            m_asyncReadCallback = AsyncReadCallback;
        }

        public void SetActiveStream(Stream stream)
        {
            m_stream = stream;
        }

        /// <summary>
        /// Modify certain serialization options.
        /// </summary>
        public EncoderOptions Options => m_encoderOptions;

        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="payloadKind"></param>
        /// <param name="payload"></param>
        public void Send(byte payloadKind, byte[] payload)
        {
            Send(payloadKind, payload, 0, payload.Length);
        }

        /// <summary>
        /// Sends data.
        /// </summary>
        /// <param name="payloadKind"></param>
        /// <param name="payload">the byte payload to send.</param>
        /// <param name="offset">the offset in <see cref="payload"/></param>
        /// <param name="length">the length of the payload.</param>
        public void Send(byte payloadKind, byte[] payload, int offset, int length)
        {
            payload.ValidateParameters(offset, length);
            if (payloadKind > 63)
                throw new ArgumentOutOfRangeException(nameof(payloadKind), "Cannot be greater than 63");

            //In case of an overflow exception.
            if (length > m_encoderOptions.MaximumCommandSize ||
                1 + 3 + 1 + length > m_encoderOptions.MaximumCommandSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            EnsureCapacity(BufferOffset + length);
            Array.Copy(payload, offset, m_buffer, BufferOffset, length);

            CtpHeader header = (CtpHeader)payloadKind;

            int headerOffset = BufferOffset;
            if (m_encoderOptions.SupportsDeflate && length >= m_encoderOptions.DeflateThreshold)
            {
                if (TryCompressPayload(m_buffer, headerOffset, length, out int newSize, out uint uncompressedChecksum))
                {
                    header |= CtpHeader.IsCompressed;
                    headerOffset -= 8;
                    m_buffer[headerOffset + 0] = (byte)(length >> 24);
                    m_buffer[headerOffset + 1] = (byte)(length >> 16);
                    m_buffer[headerOffset + 2] = (byte)(length >> 8);
                    m_buffer[headerOffset + 3] = (byte)(length >> 0);
                    m_buffer[headerOffset + 4] = (byte)(uncompressedChecksum >> 24);
                    m_buffer[headerOffset + 5] = (byte)(uncompressedChecksum >> 16);
                    m_buffer[headerOffset + 6] = (byte)(uncompressedChecksum >> 8);
                    m_buffer[headerOffset + 7] = (byte)(uncompressedChecksum >> 0);
                    length = newSize + 8;
                }
            }

            headerOffset--;
            length++;
            m_buffer[headerOffset] = (byte)header;

            if (length + 1 <= 254)
            {
                length++;
                headerOffset--;
                m_buffer[headerOffset] = (byte)length;
            }
            else
            {
                length += 4;
                headerOffset -= 4;
                if ((length >> 24) != 0)
                    throw new Exception("The compressed size of this packet exceeds the maximum command size.");

                m_buffer[headerOffset + 0] = 255;
                m_buffer[headerOffset + 1] = (byte)(length >> 16);
                m_buffer[headerOffset + 2] = (byte)(length >> 8);
                m_buffer[headerOffset + 3] = (byte)length;
            }
            m_stream.Write(m_buffer, headerOffset, length);
        }

        /// <summary>
        /// Ensures that <see cref="m_buffer"/> has at least the supplied number of bytes
        /// before returning.
        /// </summary>
        /// <param name="bufferSize"></param>
        private void EnsureCapacity(int bufferSize)
        {
            if (m_buffer.Length < bufferSize)
            {
                //12% larger than the requested buffer size.
                byte[] newBuffer = new byte[bufferSize + (bufferSize >> 3)];
                m_buffer.CopyTo(newBuffer, 0);
                m_buffer = newBuffer;
            }
        }

        /// <summary>
        /// Attempts to compress the payload. If the compression is smaller than the original, the compressed
        /// data is copied over the original and <see cref="newLength"/> contains the length of the compressed data.
        /// Otherwise, this method returns false.
        /// </summary>
        /// <param name="buffer">the data to compress</param>
        /// <param name="offset">the offset</param>
        /// <param name="length">the length of the payload</param>
        /// <param name="newLength">the length of the compressed data if successful, -1 if the compression failed.</param>
        /// <param name="checksum">A computed CRC32 of compressed data if compression is successful.</param>
        /// <returns></returns>
        private bool TryCompressPayload(byte[] buffer, int offset, int length, out int newLength, out uint checksum)
        {
            m_compressionStream.SetLength(0);
            using (var deflate = new DeflateStream(m_compressionStream, CompressionMode.Compress, true))
            {
                deflate.Write(buffer, offset, length);
            }

            //Verifies that there was a size reduction with compression.
            if (m_compressionStream.Position + 8 >= length)
            {
                newLength = -1;
                checksum = 0;
                return false;
            }

            checksum = Crc32.Compute(buffer, offset, length);
            newLength = (int)m_compressionStream.Position;
            m_compressionStream.Position = 0;
            m_compressionStream.ReadAll(buffer, offset, newLength);
            return true;
        }


        /// <summary>
        /// Writes the wire protocol data to the decoder. This data does not have to be packet aligned.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void FillBuffer(byte[] data, int position, int length)
        {
            data.ValidateParameters(position, length);

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

            Array.Copy(data, position, m_inboundBuffer, m_inboundBufferLength, length);
            m_inboundBufferLength += length;
        }

        private WaitCallback m_onDataReceived;
        private ManualResetEvent m_waitForDataEvent = new ManualResetEvent(false);

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop. Once all commands have been read, an async read
        /// will occur
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait for a command before returning null. </param>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public bool Read(int timeout = 0)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            tryAgain:
            if (!InternalRead())
            {
                if (AsyncRead())
                {
                    goto tryAgain;
                }

                long timeToWait = timeout - sw.ElapsedMilliseconds;
                if (timeout < 0)
                {
                    m_waitForDataEvent.WaitOne(-1);
                }
                else if (timeout == 0 || timeToWait <= 0)
                {
                    return false;
                }
                else
                {
                    m_waitForDataEvent.WaitOne((int)timeToWait);
                }
                m_waitForDataEvent.Reset();
                goto tryAgain;

            }
            return true;
        }

        private bool AsyncRead()
        {
            if (!m_isReading)
            {
                lock (m_syncReceive)
                {
                    if (!m_isReading)
                    {
                        m_isReading = true;
                        return m_stream.BeginRead(m_inBuffer, 0, m_inBuffer.Length, AsyncReadCallback, null).CompletedSynchronously;
                    }
                }
            }
            return false;
        }

        private void AsyncReadCallback(IAsyncResult ar)
        {
            lock (m_syncReceive)
            {
                m_isReading = false;
                int length = m_stream.EndRead(ar);
                FillBuffer(m_inBuffer, 0, length);
                m_waitForDataEvent.Set();
            }
            if (!ar.CompletedSynchronously)
            {
                ThreadPool.QueueUserWorkItem(m_onDataReceived);
            }
        }

        private void OnDataReceived(object state)
        {
            DataReceived?.Invoke(); //If this call was completed asynchronously, notify the client that it was fulfilled.
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="FillBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        private bool InternalRead()
        {
            Results.SetInvalid();

            if (m_inboundBufferLength < 1)
                return false;

            bool isCompressed;
            int packetLength;
            int position = m_inboundBufferCurrentPosition;
            byte[] buffer = m_inboundBuffer;

            packetLength = buffer[position];
            position++;
            if (packetLength == 255)
            {
                if (m_inboundBufferLength < 4)
                    return false;
                packetLength = buffer[position] << 16 | buffer[position + 1] << 8 | buffer[position + 2];
                position += 3;
            }

            if (packetLength > m_encoderOptions.MaximumCommandSize)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < packetLength)
                return false;

            CtpHeader header = (CtpHeader)buffer[position];
            position++;
            isCompressed = (header & CtpHeader.IsCompressed) != 0;

            int length = packetLength - (position - m_inboundBufferCurrentPosition);
            if (isCompressed)
            {
                //Decompresses the data.
                int inflatedSize = ToInt32(buffer, position);
                uint checksum = (uint)ToInt32(buffer, position + 4);

                //Only the payload is compressed, but some payload has to be copied to the decompressed block.
                if (m_compressionBuffer.Length < inflatedSize)
                {
                    m_compressionBuffer = new byte[inflatedSize];
                }

                var ms = new MemoryStream(buffer);
                ms.Position = position + 8;
                using (var inflate = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    inflate.ReadAll(m_compressionBuffer, 0, inflatedSize);
                }

                if (checksum != Crc32.Compute(m_compressionBuffer, 0, inflatedSize))
                {
                    throw new InvalidOperationException("Decompression checksum failed.");
                }

                buffer = m_compressionBuffer;
                position = 0;
                length = inflatedSize;
            }
            buffer.ValidateParameters(position, length);

            byte[] results;
            results = new byte[length];
            Array.Copy(buffer, position, results, 0, length);
            Results.SetRaw((byte)((byte)header & 63), results);
            m_inboundBufferCurrentPosition += packetLength;
            m_inboundBufferLength -= packetLength;
            return true;
        }

        private static int ToInt32(byte[] buffer, int startIndex)
        {
            return (int)buffer[startIndex + 0] << 24 |
                   (int)buffer[startIndex + 1] << 16 |
                   (int)buffer[startIndex + 2] << 8 |
                   (int)buffer[startIndex + 3];
        }
    }
}
