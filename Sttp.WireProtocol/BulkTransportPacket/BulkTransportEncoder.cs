using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Sttp.IO;
using Sttp.WireProtocol.BulkTransportPacket;
using Sttp.WireProtocol.Data.Raw;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes. One instance should be created per Request.
    /// </summary>
    public class BulkTransportEncoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.BulkTransport;

        public BulkTransportEncoder(Action<byte[], int, int> sendPacket) 
            : base(sendPacket, 1500)
        {

        }

        public void SendBegin(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, long position, int length)
        {
            BeginCommand();
            m_stream.Write(BulkTransportCommand.BeginSend);
            m_stream.Write(id);
            m_stream.Write(mode);
            m_stream.Write(compression);
            m_stream.Write(originalSize);
            m_stream.Write(source, position, length);
            EndCommand();
        }

        public void SendFragment(Guid id, long bytesRemaining, byte[] content, long position, int length)
        {
            BeginCommand();
            m_stream.Write(BulkTransportCommand.SendFragment);
            m_stream.Write(id);
            m_stream.Write(bytesRemaining);
            m_stream.Write(content, position, length);
            EndCommand();
        }

        public void CancelSend(Guid id)
        {
            BeginCommand();
            m_stream.Write(BulkTransportCommand.CancelSend);
            m_stream.Write(id);
            EndCommand();
        }
    }

}
