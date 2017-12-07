using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpNamedSet
    {
        public List<Tuple<string, SttpValue>> Values;

        public SttpNamedSet(ByteReader payloadReader)
        {
            throw new NotImplementedException();
        }

        public void Save(ByteWriter payloadWriter)
        {
            throw new NotImplementedException();
        }
    }
}
