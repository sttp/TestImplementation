using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Sttp.IO;

namespace Sttp.WireProtocol
{
    public class PacketDecoder
    {
        private byte[] m_pendingData;
        private byte[] m_pendingData2;
        private PacketReader m_reader;
        private byte[] m_buffer;
        private int m_position;
        private int m_length;
        private SessionDetails m_sessionDetails;

        private bool m_isProcessingFragments;
        private int m_fragmentTotalSize;
        private int m_fragmentTotalRawSize;
        private CommandCode m_fragmentCommandCode;
        private byte m_fragmentCompressionMode;
        private int m_fragmentBytesReceived;

        public PacketDecoder(SessionDetails sessionDetails)
        {
            m_pendingData = new byte[512];
            m_pendingData2 = new byte[512];
            m_sessionDetails = sessionDetails;
            m_buffer = new byte[512];
            m_reader = new PacketReader(sessionDetails);
        }

        public PacketReader NextPacket()
        {
            TryAgain:

            if (m_length < 3)
                return null;

            CommandCode code = (CommandCode)m_buffer[m_position];
            int packetLegth = BigEndian.ToInt16(m_buffer, m_position + 1);
            if (m_length < packetLegth)
            {
                return null;
            }
            if (code == CommandCode.BeginFragment)
            {
                if (m_isProcessingFragments)
                    throw new Exception("A previous fragment has not finished.");

                m_isProcessingFragments = true;

                m_fragmentTotalSize = BigEndian.ToInt32(m_buffer, m_position + 3);
                m_fragmentTotalRawSize = BigEndian.ToInt32(m_buffer, m_position + 7);
                m_fragmentCommandCode = (CommandCode)m_buffer[m_position + 11];
                m_fragmentCompressionMode = m_buffer[m_position + 12];

                if (m_pendingData.Length < m_fragmentTotalSize)
                {
                    m_pendingData = new byte[m_fragmentTotalSize];
                }

                Array.Copy(m_buffer, m_position + 13, m_pendingData, 0, packetLegth - 13);
                m_fragmentBytesReceived = packetLegth - 13;
                m_position += packetLegth;
                m_length -= packetLegth;

                if (m_fragmentBytesReceived == m_fragmentTotalSize)
                {
                    return SendFragmentedPacket();
                }
                goto TryAgain;
            }
            else if (code == CommandCode.NextFragment)
            {
                if (!m_isProcessingFragments)
                    throw new Exception("A fragment has not been defined.");

                Array.Copy(m_buffer, m_position + 3, m_pendingData, m_fragmentBytesReceived, packetLegth - 3);
                m_fragmentBytesReceived += packetLegth - 3;
                m_position += packetLegth;
                m_length -= packetLegth;

                if (m_fragmentBytesReceived == m_fragmentTotalSize)
                {
                    return SendFragmentedPacket();
                }
                goto TryAgain;
            }
            else if (code == CommandCode.CompressedPacket)
            {
                int totalRawSize = BigEndian.ToInt32(m_buffer, m_position + 3);
                CommandCode commandCode = (CommandCode)m_buffer[m_position + 7];
                byte mode = m_buffer[m_position + 8];

                if (m_pendingData.Length < totalRawSize)
                {
                    m_pendingData = new byte[totalRawSize];
                }
                var ms = new MemoryStream(m_buffer);
                ms.Position = m_position + 9;
                using (var inflate = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    inflate.ReadAll(m_pendingData, 0, totalRawSize);
                }
                m_reader.SetBuffer(commandCode, m_pendingData, 0, totalRawSize);

                m_position += packetLegth;
                m_length -= packetLegth;

                return m_reader;
            }
            else
            {
                if (m_isProcessingFragments)
                    throw new Exception("Expecting the next fragment");
                m_reader.SetBuffer(code, m_buffer, m_position, packetLegth);
                m_position += packetLegth;
                m_length -= packetLegth;
                return m_reader;
            }
        }

        private PacketReader SendFragmentedPacket()
        {
            if (m_pendingData2.Length < m_fragmentTotalRawSize)
            {
                m_pendingData2 = new byte[m_fragmentTotalRawSize];
            }
            var ms = new MemoryStream(m_pendingData);
            using (var inflate = new DeflateStream(ms, CompressionMode.Decompress, true))
            {
                inflate.ReadAll(m_pendingData2, 0, m_fragmentTotalRawSize);
            }
            m_reader.SetBuffer(m_fragmentCommandCode, m_pendingData2, 0, m_fragmentTotalRawSize);
            return m_reader;
        }

        public void WriteData(byte[] data, int position, int length)
        {
            Compact();
            while (length + m_length >= m_buffer.Length)
            {
                Grow();
            }
            Array.Copy(data, position, m_buffer, m_length, length);
            m_length += length;
        }

        public void Compact()
        {
            if (m_position > 0 && m_position != m_length)
            {
                // Compact - trims all data before current position if position is in middle of stream
                Array.Copy(m_buffer, m_position, m_buffer, 0, m_length - m_position);
            }
            m_length = m_length - m_position;
            m_position = 0;
        }

        void Grow()
        {
            byte[] newBuffer = new byte[m_buffer.Length * 2];
            m_buffer.CopyTo(newBuffer, 0);
            m_buffer = newBuffer;
        }

    }
}
