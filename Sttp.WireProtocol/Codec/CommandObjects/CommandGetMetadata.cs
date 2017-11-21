using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadata
    {
        public readonly List<MetadataRequest> Requests;

        public CommandGetMetadata(PayloadReader reader)
        {
            Requests = reader.ReadList<MetadataRequest>();
        }


    }
}