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
        private LookupMetadata m_lookup;

        protected DecoderBase(LookupMetadata lookup)
        {
            m_lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
        }


        public abstract void Load(byte[] data);

        public abstract bool Read(SttpDataPoint dataPoint);

        public SttpDataPointMetadata LookupMetadata(CtpObject dataPointID)
        {
            return m_lookup(dataPointID);
        }

    }
}
