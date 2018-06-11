using System;
using System.IO;
using System.IO.Compression;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace CTP
{

    /// <summary>
    /// Responsible for encoding each command into bytes that can be sent on the socket. 
    /// This class will fragment and compress packets to ensure that all packets fit inside the desired maximum fragment size.
    /// </summary>
    public class CtpEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing each packet or fragment;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;
        private EncoderOptions m_encoderOptions;

        /// <summary>
        /// A buffer to use to for all of the packets.
        /// </summary>
        private byte[] m_buffer;
        private MemoryStream m_compressionStream = new MemoryStream();
        private const int BufferOffset = 55;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public CtpEncoder()
        {
            m_encoderOptions = new EncoderOptions();
            m_buffer = new byte[128];
        }

        /// <summary>
        /// Modify certain serialization options.
        /// </summary>
        public EncoderOptions Options => m_encoderOptions;

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }


        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <param name="payload"></param>
        public void Send(ulong channelNumber, CtpDocument payload)
        {
            Send(channelNumber, payload.ToArray());
        }

        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <param name="payload"></param>
        public void Send(ulong channelNumber, byte[] payload)
        {
            Send(channelNumber, payload, 0, payload.Length);
        }

        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="channelNumber">a user code for this raw stream</param>
        /// <param name="payload">the byte payload to send.</param>
        /// <param name="offset">the offset in <see cref="payload"/></param>
        /// <param name="length">the length of the payload.</param>
        public void Send(ulong channelNumber, byte[] payload, int offset, int length)
        {
            payload.ValidateParameters(offset, length);
            if (length + 1 + 8 + 4 + 8 > m_encoderOptions.MaximumCommandSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            EnsureCapacity(BufferOffset + length);
            Array.Copy(payload, offset, m_buffer, BufferOffset, length);

            bool isCompressed = false;

            int headerOffset = BufferOffset;
            if (m_encoderOptions.SupportsDeflate && length >= m_encoderOptions.DeflateThreshold)
            {
                if (TryCompressPayload(m_buffer, headerOffset, length, out int newSize, out uint uncompressedChecksum))
                {
                    isCompressed = true;
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

            if (2 + length < 1024 && channelNumber < 16)
            {
                //Header 0 can be used.
                CtpHeader0 header0 = (CtpHeader0)length;
                if (isCompressed)
                    header0 |= CtpHeader0.IsCompressed;
                headerOffset -= 2;
                length += 2;
                m_buffer[headerOffset + 0] = (byte)((ushort)header0 >> 8);
                m_buffer[headerOffset + 1] = (byte)(header0);
            }
            else
            {
                //Header1 must be used.
                CtpHeader1 header1 = CtpHeader1.HeaderVersion;
                if (isCompressed)
                    header1 |= CtpHeader1.IsCompressed;


                byte[] lengthBytes = ToBigEndianBytesTrimmed((uint)length);
                byte[] channelNumberBytes = ToBigEndianBytesTrimmed(channelNumber);
                header1 |= (CtpHeader1)Math.Max(0, channelNumberBytes.Length - 1);
                header1 |= (CtpHeader1)(Math.Max(0, lengthBytes.Length - 1) << 3);

                headerOffset -= lengthBytes.Length;
                length += lengthBytes.Length;
                lengthBytes.CopyTo(m_buffer, headerOffset);

                headerOffset -= channelNumberBytes.Length;
                length += channelNumberBytes.Length;
                channelNumberBytes.CopyTo(m_buffer, headerOffset);

                headerOffset--;
                length++;
                m_buffer[headerOffset + 1] = (byte)(header1);
            }
            SendNewPacket(m_buffer, headerOffset, length);

        }

        private static byte[] ToBigEndianBytesTrimmed(ulong value)
        {
            int requiredBytes = (64 - BitMath.CountLeadingZeros(value) + 7) >> 3;
            byte[] rv = new byte[Math.Max(1, requiredBytes)];

            for (int x = rv.Length - 1; x >= 0; x--)
            {
                rv[x] = (byte)value;
                value >>= 8;
            }
            return rv;
        }
        private static byte[] ToBigEndianBytesTrimmed(uint value)
        {
            int requiredBytes = (32 - BitMath.CountLeadingZeros(value) + 7) >> 3;
            byte[] rv = new byte[Math.Max(1, requiredBytes)];

            for (int x = rv.Length - 1; x >= 0; x--)
            {
                rv[x] = (byte)value;
                value >>= 8;
            }
            return rv;
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
    }
}
