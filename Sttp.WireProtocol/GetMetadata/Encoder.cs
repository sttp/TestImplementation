using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadata
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
        {

        }

        public void Schema(Guid schemaVersion, long revision)
        {
            BeginCommand();
            Stream.Write(schemaVersion);
            Stream.Write(revision);
            EndCommand();
        }

        public void Query(Guid schemaVersion, long revision, bool isUpdateQuery, SttpQueryExpression expression)
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
