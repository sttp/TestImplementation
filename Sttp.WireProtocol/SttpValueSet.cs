using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueSet
    {
        public List<SttpValue> Values = new List<SttpValue>();

        public void Write(PayloadWriter payloadWriter)
        {
            throw new NotImplementedException();
        }
    }
}
