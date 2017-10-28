using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class BulkTransportDecoder : IPacketDecoder
    {
        private BulkTransportBeginSendParams m_beginSend = new BulkTransportBeginSendParams();
        private BulkTransportCancelSendParams m_cancelSend = new BulkTransportCancelSendParams();
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
                case BulkTransportCommand.BeginSend:
                    m_beginSend.Id = m_buffer.ReadGuid();
                    m_beginSend.Mode = m_buffer.Read<BulkTransportMode>();
                    m_beginSend.Compression = m_buffer.Read<BulkTransportCompression>();
                    m_beginSend.OriginalSize = m_buffer.ReadInt64();
                    m_beginSend.Content = m_buffer.ReadBytes();
                    return m_beginSend;
                case BulkTransportCommand.CancelSend:
                    m_cancelSend.Id = m_buffer.ReadGuid();
                    return m_cancelSend;
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
