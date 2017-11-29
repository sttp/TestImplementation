using System;
using System.Collections.Generic;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueSet
    {
        public List<SttpValue> Values = new List<SttpValue>();

        public SttpValueSet()
        {
            
        }
        public SttpValueSet(PayloadReader payloadReader)
        {
            throw new NotImplementedException();
        }

        public void Write(PayloadWriter payloadWriter)
        {
            throw new NotImplementedException();
        }
    }
}
