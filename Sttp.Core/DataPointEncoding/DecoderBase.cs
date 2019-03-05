using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public abstract class DecoderBase
    {
        public abstract bool Read(SttpDataPoint dataPoint);
    }
}
