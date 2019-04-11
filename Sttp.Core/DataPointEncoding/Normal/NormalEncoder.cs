using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class NormalEncoder : EncoderBase
    {
        private CtpObjectWriter m_stream;
        private int m_lastChannelID = 0;
        private long m_lastTimestamp;
        private long m_lastQuality = 0;
        private Dictionary<SttpDataPointID, int> m_pointIDToChannelIDMapping = new Dictionary<SttpDataPointID, int>();
        private SttpDataPointID[] m_metadata = new SttpDataPointID[16];
        private int m_metadataCount;

        public NormalEncoder()
        {
            m_stream = new CtpObjectWriter();
            Clear();
        }

        public override int Length => m_stream.Length;

        public sealed override void Clear()
        {
            m_lastChannelID = 0;
            m_lastTimestamp = 0;
            m_lastQuality = 0;
            m_stream.Clear();
        }

        public override CommandObject ToArray()
        {
            return new CommandDataStreamNormal(m_stream.ToArray());
        }

        public override int AddDataPoint(SttpDataPoint point)
        {
            byte code = 0;
            //CtpObject value = point.Value;
            int channelID = m_lastChannelID += 1; //Note the += 1

            //Most of the time, measurements will be sequential, this limits a dictionary lookup
            if (channelID >= m_metadataCount || !ReferenceEquals(m_metadata[channelID], point.DataPoint))
            {
                //A reference equals should be good enough to try
                channelID = GetChannelID(point, out var includeMetadata);
                if (m_lastChannelID != channelID)
                {
                    code |= 1;
                    m_lastChannelID = channelID;
                }
                if (includeMetadata)
                    code |= 2;
            }

            if (point.Quality != m_lastQuality)
            {
                code |= 4;
                m_lastQuality = point.Quality;
            }

            if (point.Time.Ticks != m_lastTimestamp)
            {
                code |= 8;
                m_lastTimestamp = point.Time.Ticks;
            }

            if (code > 0 || (point.Value.ValueTypeCode == CtpTypeCode.Integer && point.Value.UnsafeInteger > long.MaxValue - 16))
            {
                m_stream.WriteExact(code);
                if ((code & 1) != 0)
                    m_stream.WriteExact(channelID);
                if ((code & 2) != 0)
                    m_stream.Write(point.DataPoint.ID);
                if ((code & 4) != 0)
                    m_stream.WriteExact(point.Quality);
                if ((code & 8) != 0)
                    m_stream.Write(point.Time);
            }
            else if (point.Value.ValueTypeCode == CtpTypeCode.Integer)
            {
                //10% of the time, it's an integer
                if (point.Value.UnsafeInteger >= 0)
                    m_stream.WriteExact(point.Value.UnsafeInteger + 16);
                else
                    m_stream.WriteExact(point.Value.UnsafeInteger);
                return m_stream.Length;

            }

            if (point.Value.ValueTypeCode == CtpTypeCode.Single)
            {
                m_stream.WriteExact(point.Value.UnsafeSingle); //90% of the time, its single.
            }
            else
            {
                m_stream.Write(point.Value);
            }
            return m_stream.Length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int GetChannelID(SttpDataPoint point, out bool includeMetadata)
        {
            includeMetadata = false;
            if (!m_pointIDToChannelIDMapping.TryGetValue(point.DataPoint, out var channelID))
            {
                includeMetadata = true;
                channelID = m_metadataCount;
                m_pointIDToChannelIDMapping.Add(point.DataPoint, channelID);

                if (m_metadataCount + 1 == m_metadata.Length)
                {
                    var newList = new SttpDataPointID[m_metadata.Length * 2];
                    m_metadata.CopyTo(newList, 0);
                    m_metadata = newList;
                }

                m_metadata[m_metadataCount] = point.DataPoint;
                m_metadataCount++;
            }
            return channelID;
        }
    }
}
