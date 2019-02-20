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
        private DecoderBase m_decoderRaw;
        private DecoderBase m_decoderBasic;
        private DecoderBase m_decoderSimple;
        private DecoderBase m_decoderAdvanced;
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
            
            if (m_nextPacket.RootElement == "DataStreamRaw")
            {
                if (m_decoderRaw == null)
                    m_decoderRaw = new RawDecoder(Lookup);
                m_decoderRaw.Load(((CommandDataStreamRaw)m_nextPacket).Data);
                m_currentDecoder = m_decoderRaw;
                return FileReaderItem.DataPoint;
            }
            else if (m_nextPacket.RootElement == "DataStreamBasic")
            {
                if (m_decoderBasic == null)
                    m_decoderBasic = new BasicDecoder(Lookup);
                m_decoderBasic.Load(((CommandDataStreamBasic)m_nextPacket).Data);
                m_currentDecoder = m_decoderBasic;
                return FileReaderItem.DataPoint;
            }
            else if (m_nextPacket.RootElement == "DataStreamSimple")
            {
                if (m_decoderSimple == null)
                    m_decoderSimple = new SimpleDecoder(Lookup);
                m_decoderSimple.Load(((CommandDataStreamSimple)m_nextPacket).Data);
                m_currentDecoder = m_decoderSimple;
                return FileReaderItem.DataPoint;
            }
            else if (m_nextPacket.RootElement == "DataStreamAdvanced")
            {
                if (m_decoderAdvanced == null)
                    m_decoderAdvanced = new AdvancedDecoder(Lookup);
                m_decoderAdvanced.Load(((CommandDataStreamAdvanced)m_nextPacket).Data);
                m_currentDecoder = m_decoderAdvanced;
                return FileReaderItem.DataPoint;

            }
            else if (m_nextPacket.RootElement == "ProducerMetadata")
            {
                m_metadata = (SttpProducerMetadata)m_nextPacket;
                foreach (var item in m_metadata.DataPoints)
                {
                    m_metadataLookup[item.DataPointID] = item;
                }
                return FileReaderItem.ProducerMetadata;
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
