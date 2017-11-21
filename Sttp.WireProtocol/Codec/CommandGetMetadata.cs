using System;
using System.Collections.Generic;

namespace Sttp.Codec
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