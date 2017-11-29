using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpQueryRaw
    {
        public string QueryText;
        public string SyntaxLanguage;
        public List<Tuple<string, SttpValue>> Literals = new List<Tuple<string, SttpValue>>();


    }
}
