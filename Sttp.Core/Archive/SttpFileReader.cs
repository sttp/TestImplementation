using System;
using System.Collections.Generic;
using System.IO;
using CTP;
using CTP.IO;
using Sttp.Codec;
using Sttp.DataPointEncoding;

namespace Sttp
{
    public enum FileReaderItem
    {
        ProducerMetadata,
        DataPoint,
        EndOfStream
    }

    public class SttpFileReader : IDisposable
    {
        private CtpFileStream m_stream;
        private NormalDecoder m_decoderNormal;
        private DecoderBase m_currentDecoder;
        private CtpCommand m_nextPacket;

        private SttpProducerMetadata m_metadata;

        private Dictionary<CtpObject, SttpDataPointMetadata> m_metadataLookup;

        public SttpFileReader(Stream stream, bool ownsStream)
        {
            m_stream = new CtpFileStream(stream, CtpCompressionMode.None, ownsStream);
            m_metadataLookup = new Dictionary<CtpObject, SttpDataPointMetadata>();
        }

        private SttpDataPointMetadata Lookup(CtpObject dataPointID)
        {
            if (m_metadataLookup.TryGetValue(dataPointID, out var value))
            {
                return value;
            }
            var dp = new SttpDataPointMetadata(null);
            dp.DataPointID = dataPointID;
            return dp;
        }

        public FileReaderItem Next()
        {
            TryAgain:
            m_nextPacket = m_stream.Read();
            if ((object)m_nextPacket == null)
                return FileReaderItem.EndOfStream;
            
            if (m_nextPacket.RootElement == "DataStreamNormal")
            {
                if (m_decoderNormal == null)
                    m_decoderNormal = new NormalDecoder(Lookup);
                m_decoderNormal.Load(((CommandDataStreamNormal)m_nextPacket));
                m_currentDecoder = m_decoderNormal;
                return FileReaderItem.DataPoint;
            }
            goto TryAgain;
        }

        public bool ReadDataPoint(SttpDataPoint dataPoint)
        {
            return m_currentDecoder.Read(dataPoint);
        }

        public SttpProducerMetadata GetMetadata()
        {
            return m_metadata;
        }

        public void Dispose()
        {
            m_stream?.Dispose();
        }
    }
}
