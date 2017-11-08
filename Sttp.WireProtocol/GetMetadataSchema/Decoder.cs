using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class Decoder
    {
        public bool IncludeSchema;

        public CommandCode CommandCode => CommandCode.GetMetadataSchema;

        public void Fill(PacketReader reader)
        {
            IncludeSchema = reader.ReadBoolean();
        }
    }
}
