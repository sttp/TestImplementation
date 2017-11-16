using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadata
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetMetadata(Guid schemaVersion, long revision, bool isUpdateQuery, SttpQueryExpression expression)
        {
            BeginCommand();
            Stream.Write(schemaVersion);
            Stream.Write(revision);
            Stream.Write(isUpdateQuery);
            Stream.Write(expression);
            EndCommand();
        }

    }
}
