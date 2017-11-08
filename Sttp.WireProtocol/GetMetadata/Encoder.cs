using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadata
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void RequestQuery(Guid schemaVersion, long revision, bool isUpdateQuery, SttpQueryExpression expression)
        {
            Stream.Write(SubCommand.RequestQuery);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
            Stream.Write(isUpdateQuery);
            Stream.Write(expression);
        }

    }
}
