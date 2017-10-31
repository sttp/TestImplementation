using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadataSchema;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetDatabaseSchema()
        {
            m_stream.Write((byte)SubCommand.GetDatabaseSchema);
        }

        public void GetDatabaseVersion()
        {
            m_stream.Write((byte)SubCommand.GetDatabaseVersion);
        }

    }
}
