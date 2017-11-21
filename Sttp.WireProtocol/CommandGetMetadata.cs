using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class CommandGetMetadata
    {
        public List<MetadataRequest> Requests;

        public void Load(PayloadReader reader)
        {
            Requests = reader.ReadList<MetadataRequest>();
        }


    }
}