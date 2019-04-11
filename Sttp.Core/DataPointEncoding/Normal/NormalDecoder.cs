using System.Collections.Generic;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class NormalDecoder : DecoderBase
    {
        private CtpObjectReader m_stream;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private List<SttpDataPointID> m_channelMap = new List<SttpDataPointID>();

        public NormalDecoder()
        {
            m_stream = new CtpObjectReader();
        }

        public void Load(CommandDataStreamNormal data)
        {
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream.SetBuffer(data.ObjectStream);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            TryAgain:
            if (m_stream.IsEmpty)
                return false;

            m_lastChannelID++;
            CtpObject value = m_stream.Read();

            if (value.ValueTypeCode == CtpTypeCode.Integer)
            {
                long code = value.IsInteger;
                if (0 <= code && code <= 15)
                {
                    bool channelIDChanged = (code & 1) != 0;
                    bool hasMetadata = (code & 2) != 0;
                    bool qualityChanged = (code & 4) != 0;
                    bool timeChanged = (code & 8) != 0;

                    if (channelIDChanged)
                        m_lastChannelID = m_stream.Read().AsInt32;

                    if (hasMetadata)
                        Assign(new SttpDataPointID(m_stream.Read()), m_lastChannelID);

                    if (qualityChanged)
                        m_lastQuality = m_stream.Read().AsInt64;

                    if (timeChanged)
                        m_lastTimestamp = m_stream.Read().AsCtpTime;

                    value = m_stream.Read();
                }
                else
                {
                    value = code - 16;
                }
            }

            dataPoint.DataPoint = GetMetadata(m_lastChannelID);
            dataPoint.Quality = m_lastQuality;
            dataPoint.Time = m_lastTimestamp;
            dataPoint.Value = value;

            if (dataPoint.DataPoint == null)
                goto TryAgain;

            return true;
        }

        private SttpDataPointID GetMetadata(int channelID)
        {
            if (channelID >= m_channelMap.Count)
            {
                return null;
            }
            return m_channelMap[channelID];
        }

        private void Assign(SttpDataPointID metadata, int channelID)
        {
            while (m_channelMap.Count <= channelID)
                m_channelMap.Add(null);
            m_channelMap[channelID] = metadata;
        }
    }
}
