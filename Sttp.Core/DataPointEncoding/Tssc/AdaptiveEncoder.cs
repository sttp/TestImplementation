using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class AdaptiveEncoder : EncoderBase
    {
        private MetadataChannelMapEncoder m_channelMap;

        private TsscEncoder m_encoder;

        public AdaptiveEncoder()
        {
            m_encoder = new TsscEncoder();
            m_channelMap = new MetadataChannelMapEncoder();
        }

        public override int Length => m_encoder.Writer.Length;

        public override void Clear(bool clearMapping)
        {
            m_encoder.Reset(clearMapping);
            if (clearMapping)
            {
                m_channelMap.Clear();
            }
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_channelMap.GetChannelID(point.Metadata, out bool isNew);
            m_encoder.AddMeasurement(channelID, point.Time.Ticks, (ulong)point.Quality, point.Value);

            if (isNew)
            {
                CtpValueEncodingNative.Save(m_encoder.Writer, point.Metadata.DataPointID);
            }
        }

        public override byte[] ToArray()
        {
            return m_encoder.Writer.ToArray();
        }
    }
}
