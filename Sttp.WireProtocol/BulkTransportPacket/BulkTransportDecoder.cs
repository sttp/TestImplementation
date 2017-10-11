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

        public IBulkTransportParams Read(StreamReader buffer)
        {
            if (buffer.Position == buffer.Length)
                return null;

            BulkTransportCommand command = buffer.Read<BulkTransportCommand>();

            switch (command)
            {
                case BulkTransportCommand.BeginBulkTransport:
                    m_begin.Id = buffer.ReadGuid();
                    m_begin.OriginalSize = buffer.ReadInt64();
                    m_begin.Mode = buffer.Read<BulkTransportMode>();
                    m_begin.IsGZip = buffer.ReadBoolean();
                    m_begin.Content = buffer.ReadBytes();
                    return m_begin;
                case BulkTransportCommand.CancelBulkTransport:
                    m_cancel.Id = buffer.ReadGuid();
                    return m_cancel;
                case BulkTransportCommand.SendFragment:
                    m_fragment.Id = buffer.ReadGuid();
                    m_fragment.Offset = buffer.ReadInt64();
                    m_fragment.Content = buffer.ReadBytes();
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
