using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportBeginSend
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.BulkTransportBeginSend;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
        {

        }

        public void BeginSend(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, long position, int length)
        {
            BeginCommand();
            Stream.Write(CommandCode.BulkTransportBeginSend);
            Stream.Write(id);
            Stream.Write(mode);
            Stream.Write(compression);
            Stream.Write(originalSize);
            Stream.Write(source, position, length);
            EndCommand();
        }

    }
}
