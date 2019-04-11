using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public abstract class EncoderBase
    {
        public abstract int Length { get; }
        public abstract void Clear();
        public abstract int AddDataPoint(SttpDataPoint point);
        public abstract CommandObject ToArray();
    }
}
