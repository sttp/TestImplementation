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
        public SttpValueSet(ByteReader rd)
        {
            Values = rd.ReadListSttpValue();
        }

        public void Save(ByteWriter wr)
        {
            wr.Write(Values);
        }
    }
}
