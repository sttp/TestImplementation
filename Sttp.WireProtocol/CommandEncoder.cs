using System;
using System.IO;
using System.IO.Compression;
using Sttp.IO;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Takes a command and payload and encodes it as a packet. This class will ensure that a large command
    /// is compressed and fragmented so the encoded data is never larger than the maximum packet size.
    /// </summary>
    public class CommandEncoder
    {
        private SessionDetails m_sessionDetails;
        private Action<byte[], int, int> m_sendPacket;

        public CommandEncoder(SessionDetails sessionDetails, Action<byte[], int, int> sendPacket)
        {
            m_sessionDetails = sessionDetails;
            m_sendPacket = sendPacket;
        }

        /// <summary>
        /// Sends a command of data over the wire. Note: This class modifies the input stream, 
        /// so after sending the data, the buffer must be discarded. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="payload">There must be at least 15 bytes before the start of the payload 
        /// since this class modifies the input to put the header before the payload.</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void EncodeAndSend(CommandCode command, byte[] payload, int offset, int length)
        {
            if (offset < 15)
                throw new Exception("There must be at least 15 bytes before the start of the byte[] buffer as a working space");

            if (length > m_sessionDetails.Limits.MaxPacketSize)
            {
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");
            }

            //ToDo: Consider if there is a better way to apply different compression algorithms. Deflate works great for large sets of data
            //bit responds poorly if the data segment is small.
            if (m_sessionDetails.SupportsDeflate && length >= m_sessionDetails.DeflateThreshold)
            {
                SendCompressed(command, payload, offset, length);
                return;
            }

            if (length + 3 < m_sessionDetails.MaximumSegmentSize)
            {
                //This packet doesn't have to be fragmented.
                payload[offset - 3] = (byte)command;
                payload[offset - 2] = (byte)(length >> 8);
                payload[offset - 1] = (byte)(length);
                m_sendPacket(payload, offset - 3, length + 3);
            }
            else
            {
                SendFragmentedPacket(command, length, 0, payload, offset, length);
            }
        }

        private void SendCompressed(CommandCode command, byte[] data, int offset, int length)
        {
            //ToDo: Determine if and what kind of compression should occur, Maybe try multiple 
            using (var ms = new MemoryStream())
            {
                ms.Write(new byte[15]); //a 15 byte prefix.
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    deflate.Write(data, offset, length);
                }

                data = ms.ToArray();
                SendFragmentedPacket(command, length, 1, data, 15, data.Length - 15);
            }

        }

        private void SendFragmentedPacket(CommandCode command, int totalRawSize, byte compressionMode, byte[] data, int offset, int length)
        {
            const int Overhead = 4 + 4 + 1 + 1; //10 bytes.

            int fragmentLength = Math.Min(m_sessionDetails.MaximumSegmentSize - Overhead - 3, length);

            data[offset - Overhead - 3] = (byte)CommandCode.BeginFragment;
            data[offset - Overhead - 2] = (byte)((fragmentLength + Overhead) >> 8);
            data[offset - Overhead - 1] = (byte)(fragmentLength + Overhead);
            data[offset - Overhead + 0] = (byte)(length >> 24);
            data[offset - Overhead + 1] = (byte)(length >> 16);
            data[offset - Overhead + 2] = (byte)(length >> 8);
            data[offset - Overhead + 3] = (byte)(length >> 0);
            data[offset - Overhead + 4] = (byte)(totalRawSize >> 24);
            data[offset - Overhead + 5] = (byte)(totalRawSize >> 16);
            data[offset - Overhead + 6] = (byte)(totalRawSize >> 8);
            data[offset - Overhead + 7] = (byte)(totalRawSize >> 0);
            data[offset - Overhead + 8] = (byte)(command);
            data[offset - Overhead + 9] = (byte)(compressionMode);

            m_sendPacket(data, offset - Overhead - 3, fragmentLength + Overhead + 3);

            offset += fragmentLength;
            length -= fragmentLength;

            while (length > 0)
            {
                fragmentLength = Math.Min(m_sessionDetails.MaximumSegmentSize - 3, length);

                data[offset - Overhead - 3] = (byte)CommandCode.NextFragment;
                data[offset - Overhead - 2] = (byte)(fragmentLength >> 8);
                data[offset - Overhead - 1] = (byte)(fragmentLength);

                m_sendPacket(data, offset - 3, fragmentLength + 3);
                offset += fragmentLength;
                length -= fragmentLength;
            }

        }

    }
}
