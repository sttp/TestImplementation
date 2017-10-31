using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;

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
            m_stream.Write((byte)MetadataSubCommand.GetDatabaseSchema);
        }

        public void GetDatabaseVersion()
        {
            m_stream.Write((byte)MetadataSubCommand.GetDatabaseVersion);
        }

    }
}
