using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Sttp.IO;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes. One instance should be created per Request.
    /// </summary>
    public class BulkTransportEncoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.BulkTransport;

        public BulkTransportEncoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void SendBegin(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, long position, int length)
        {
            BeginCommand();
            Stream.Write(BulkTransportCommand.BeginSend);
            Stream.Write(id);
            Stream.Write(mode);
            Stream.Write(compression);
            Stream.Write(originalSize);
            Stream.Write(source, position, length);
            EndCommand();
        }

        public void SendFragment(Guid id, long bytesRemaining, byte[] content, long position, int length)
        {
            BeginCommand();
            Stream.Write(BulkTransportCommand.SendFragment);
            Stream.Write(id);
            Stream.Write(bytesRemaining);
            Stream.Write(content, position, length);
            EndCommand();
        }

        public void CancelSend(Guid id)
        {
            BeginCommand();
            Stream.Write(BulkTransportCommand.CancelSend);
            Stream.Write(id);
            EndCommand();
        }
    }

}
