using System;
using System.IO;
using System.IO.Compression;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace CTP
{
    /// <summary>
    /// Decodes an incoming byte stream into a series of command objects. This class will align packets, reassemble fragments, and decompress packets. 
    /// </summary>
    public class CtpDecoder
    {
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

        private FragmentReassembly m_fragmentReassembly = new FragmentReassembly();

        private CommandCode m_commandCode;
        private CtpDocument m_documentPayload;
        private byte[] m_binaryCommandPayload;
        private int m_binaryChannelCode;

        public CtpDecoder()
        {
            m_compressionBuffer = new byte[512];
            m_inboundBuffer = new byte[512];
        }

        /// <summary>
        /// Indicates if a command has successfully been decoded. 
        /// This is equal to the return value of the most recent 
        /// <see cref="NextCommand"/> method call.
        /// </summary>
        public bool IsValid => m_commandCode != CommandCode.Invalid;

        /// <summary>
        /// Indicates what kind of commmand was decoded.
        /// </summary>
        public CommandCode CommandCode => m_commandCode;

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. And the command 
        /// This is the command that was decoded.
        /// </summary>
        public CtpDocument DocumentPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_commandCode != CommandCode.Document)
                    throw new InvalidOperationException("Command is not a Document Command.");
                return m_documentPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public byte[] BinaryPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_commandCode != CommandCode.Binary)
                    throw new InvalidOperationException("Command is not a Binary Command.");
                return m_binaryCommandPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public int BinaryChannelID
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_commandCode != CommandCode.Binary)
                    throw new InvalidOperationException("Command is not a Binary Command.");
                return m_binaryChannelCode;
            }
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

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="FillBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        public bool NextCommand()
        {
            m_commandCode = CommandCode.Invalid;
            m_documentPayload = null;
            m_binaryCommandPayload = null;
            m_binaryChannelCode = 0;

            TryAgain:
            if (m_inboundBufferLength < 2)
                return false;

            CtpHeader header = (CtpHeader)ToUInt16(m_inboundBuffer, m_inboundBufferCurrentPosition);
            int packetLength = (int)(header & CtpHeader.PacketLengthMask);
            if (m_inboundBufferLength < packetLength)
                return false;

            if ((header & CtpHeader.IsFragmented) == CtpHeader.IsFragmented)
            {
                m_fragmentReassembly.ProcessFragment(header, m_inboundBuffer, m_inboundBufferCurrentPosition + 2, packetLength - 2);
                m_inboundBufferCurrentPosition += packetLength;
                m_inboundBufferLength -= packetLength;

                if (m_fragmentReassembly.IsComplete)
                {
                    ProcessPacket(m_fragmentReassembly.Header, m_fragmentReassembly.Buffer, 0, m_fragmentReassembly.TotalSize);
                    return true;
                }
                goto TryAgain;
            }
            else
            {
                ProcessPacket(header, m_inboundBuffer, m_inboundBufferCurrentPosition + 2, packetLength - 2);
                m_inboundBufferCurrentPosition += packetLength;
                m_inboundBufferLength -= packetLength;
                return true;
            }
        }

        private void ProcessPacket(CtpHeader header, byte[] buffer, int position, int length)
        {
            if ((header & CtpHeader.IsCompressed) == CtpHeader.IsCompressed)
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
            if ((header & CtpHeader.CommandMask) == CtpHeader.CommandDocument)
            {
                m_commandCode = CommandCode.Document;
                int markupLength = length;
                int markupStart = position;
                results = new byte[markupLength];
                Array.Copy(buffer, markupStart, results, 0, markupLength);
                m_documentPayload = new CtpDocument(results);
            }
            else
            {
                m_commandCode = CommandCode.Binary;

                if ((header & CtpHeader.CommandMask) == CtpHeader.CommandBinary0)
                {
                    m_binaryChannelCode = 0;
                }
                else if ((header & CtpHeader.CommandMask) == CtpHeader.CommandBinary1)
                {
                    m_binaryChannelCode = 1;
                }
                else
                {
                    m_binaryChannelCode = ToInt32(buffer, position);
                    position += 4;
                    length -= 4;
                }

                results = new byte[length];
                Array.Copy(buffer, position, results, 0, length);
                m_binaryChannelCode = buffer[position];
                m_binaryCommandPayload = results;
            }
        }

        private static ushort ToUInt16(byte[] buffer, int startIndex)
        {
            return (ushort)((uint)buffer[startIndex] << 8 | (uint)buffer[startIndex + 1]);
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
