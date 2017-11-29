using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpQueryRaw
    {
        public string QueryText;
        public string SyntaxLanguage;
        public List<Tuple<string, SttpValue>> Literals = new List<Tuple<string, SttpValue>>();

        public void Save(PayloadWriter payloadWriter)
        {
            throw new NotImplementedException();
        }
    }
}
