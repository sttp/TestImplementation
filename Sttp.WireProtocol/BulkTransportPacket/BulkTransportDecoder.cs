using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class BulkTransportDecoder : IPacketDecoder
    {
        private BulkTransportBeginParams m_begin = new BulkTransportBeginParams();
        private BulkTransportCancelParams m_cancel = new BulkTransportCancelParams();
        private BulkTransportSendFragmentParams m_fragment = new BulkTransportSendFragmentParams();

        public CommandCode CommandCode => CommandCode.BulkTransport;

        private StreamReader m_buffer;

        public void Fill(StreamReader reader)
        {
            m_buffer = reader;
        }

        public IBulkTransportParams Read()
        {
            if (m_buffer.Position == m_buffer.Length)
                return null;

            BulkTransportCommand command = m_buffer.Read<BulkTransportCommand>();

            switch (command)
            {
                case BulkTransportCommand.BeginBulkTransport:
                    m_begin.Id = m_buffer.ReadGuid();
                    m_begin.Mode = m_buffer.Read<BulkTransportMode>();
                    m_begin.Compression = m_buffer.Read<BulkTransportCompression>();
                    m_begin.OriginalSize = m_buffer.ReadInt64();
                    m_begin.Content = m_buffer.ReadBytes();
                    return m_begin;
                case BulkTransportCommand.CancelBulkTransport:
                    m_cancel.Id = m_buffer.ReadGuid();
                    return m_cancel;
                case BulkTransportCommand.SendFragment:
                    m_fragment.Id = m_buffer.ReadGuid();
                    m_fragment.BytesRemaining = m_buffer.ReadInt64();
                    m_fragment.Content = m_buffer.ReadBytes();
                    return m_fragment;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
