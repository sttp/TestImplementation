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
                    m_begin.OriginalSize = m_buffer.ReadInt64();
                    m_begin.Mode = m_buffer.Read<BulkTransportMode>();
                    m_begin.IsGZip = m_buffer.ReadBoolean();
                    m_begin.Content = m_buffer.ReadBytes();
                    return m_begin;
                case BulkTransportCommand.CancelBulkTransport:
                    m_cancel.Id = m_buffer.ReadGuid();
                    return m_cancel;
                case BulkTransportCommand.SendFragment:
                    m_fragment.Id = m_buffer.ReadGuid();
                    m_fragment.Offset = m_buffer.ReadInt64();
                    m_fragment.Content = m_buffer.ReadBytes();
                    return m_fragment;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public interface IBulkTransportParams
    {
        BulkTransportCommand Command { get; }
    }

    public class BulkTransportBeginParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.BeginBulkTransport;
        public Guid Id;
        public long OriginalSize;
        public BulkTransportMode Mode;
        public bool IsGZip;
        public byte[] Content;
    }

    public class BulkTransportCancelParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.CancelBulkTransport;
        public Guid Id;
    }

    public class BulkTransportSendFragmentParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.SendFragment;
        public Guid Id;
        public long Offset;
        public byte[] Content;
    }
}
