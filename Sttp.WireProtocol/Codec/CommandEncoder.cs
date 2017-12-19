using System;
using System.IO;
using System.Text;
using Sttp.IO;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace Sttp.Codec
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class CommandEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing a Packet;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        //private DataPointEncoder m_dataPoint;
        private SessionDetails m_sessionDetails;

        private byte[] m_buffer;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public CommandEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_buffer = new byte[64];
        }

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

        public void SubscriptionStream(byte encodingMethod, byte[] buffer, int position, int length)
        {
            buffer.ValidateParameters(position, length);
            EnsureCapacity(15 + 1 + length);
            m_buffer[15] = encodingMethod;
            Array.Copy(buffer, position, m_buffer, 16, length);
            EncodeAndSend(CommandCode.SubscriptionStream, m_buffer, 15, length + 1);
        }

        public void SendMarkupCommand(SttpMarkupWriter markup)
        {
            byte[] commandBytes = Encoding.ASCII.GetBytes(markup.RootElement);
            if (commandBytes.Length > 255)
                throw new Exception("Command cannot be more than 255 characters");
            SttpMarkup data = markup.ToSttpMarkup();
            EnsureCapacity(15 + 1 + commandBytes.Length + data.Length);
            m_buffer[15] = (byte)commandBytes.Length;
            commandBytes.CopyTo(m_buffer, 15 + 1);
            data.CopyTo(m_buffer, 15 + 1 + commandBytes.Length);
            EncodeAndSend(CommandCode.MarkupCommand, m_buffer, 15, data.Length + 1 + commandBytes.Length);
        }
        public void SendMarkupCommand(CommandBase command)
        {
            byte[] commandBytes = Encoding.ASCII.GetBytes(command.CommandName);
            if (commandBytes.Length > 255)
                throw new Exception("Command cannot be more than 255 characters");

            var writer = new SttpMarkupWriter(command.CommandName);
            command.Save(writer);
            SttpMarkup data = writer.ToSttpMarkup();
            EnsureCapacity(15 + 1 + commandBytes.Length + data.Length);
            m_buffer[15] = (byte)commandBytes.Length;
            commandBytes.CopyTo(m_buffer, 15 + 1);
            data.CopyTo(m_buffer, 15 + 1 + commandBytes.Length);
            EncodeAndSend(CommandCode.MarkupCommand, m_buffer, 15, data.Length + 1 + commandBytes.Length);
        }

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
        /// Sends a command of data over the wire. Note: This class modifies the input stream, 
        /// so after sending the data, the buffer must be discarded. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="payload">There must be at least 15 bytes before the start of the payload 
        /// since this class modifies the input to put the header before the payload.</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        private void EncodeAndSend(CommandCode command, byte[] payload, int offset, int length)
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
                SendNewPacket(payload, offset - 3, length + 3);
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
                //byte[] raw = new byte[length];
                //Array.Copy(data, offset, raw, 0, length);
                //File.WriteAllBytes("C:\\temp\\openPDC-sttp.raw", raw);
                //byte[] aced = AcedDeflator.Instance.Compress(data, offset, length, AcedCompressionLevel.Maximum, 0, 0);
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

            SendNewPacket(data, offset - Overhead - 3, fragmentLength + Overhead + 3);

            offset += fragmentLength;
            length -= fragmentLength;

            while (length > 0)
            {
                fragmentLength = Math.Min(m_sessionDetails.MaximumSegmentSize - 3, length);

                data[offset - 3] = (byte)CommandCode.NextFragment;
                data[offset - 2] = (byte)(fragmentLength >> 8);
                data[offset - 1] = (byte)(fragmentLength);

                SendNewPacket(data, offset - 3, fragmentLength + 3);
                offset += fragmentLength;
                length -= fragmentLength;
            }
        }

    }
}
