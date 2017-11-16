using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class Decoder
    {
        public Guid SchemaVersion;
        public long Revision;

        public CommandCode CommandCode => CommandCode.GetMetadataSchema;

        public void Fill(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
        }
    }
}
