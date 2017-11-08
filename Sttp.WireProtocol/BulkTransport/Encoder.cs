using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransport
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.BulkTransport;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void BeginSend(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, long position, int length)
        {
            BeginCommand();
            Stream.Write(SubCommand.BeginSend);
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
            Stream.Write(SubCommand.SendFragment);
            Stream.Write(id);
            Stream.Write(bytesRemaining);
            Stream.Write(content, position, length);
            EndCommand();
        }

        public void CancelSend(Guid id)
        {
            BeginCommand();
            Stream.Write(SubCommand.CancelSend);
            Stream.Write(id);
            EndCommand();
        }

    }
}
