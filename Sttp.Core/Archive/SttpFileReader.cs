using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;
using CTP.IO;
using Sttp.Codec;
using Sttp.Codec.DataPoint;

namespace Sttp.Archive
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
        private BasicDecoder m_decoder;
        private CtpCommand m_nextCommand;

        private CommandBeginDataStream m_dataStream;
        private SttpProducerMetadata m_metadata;

        public SttpFileReader(Stream stream, bool ownsStream)
        {
            m_stream = new CtpFileStream(stream, ownsStream);
            m_decoder = new BasicDecoder();
        }

        public FileReaderItem Next()
        {
            TryAgain:
            m_nextCommand = m_stream.Read();
            if ((object)m_nextCommand == null)
                return FileReaderItem.EndOfStream;
            if (m_nextCommand.IsRaw)
            {
                if (m_dataStream == null)
                    throw new Exception("Data stream is not defined for the specified channel.");

                var raw = (CtpRaw)m_nextCommand;
                if (raw.Channel == m_dataStream.ChannelCode)
                {
                    m_decoder.Load(raw.Payload);
                    return FileReaderItem.DataPoint;
                }
                throw new Exception("Data stream is not defined for the specified channel.");
            }
            else if (m_nextCommand.RootElement == "BeginDataStream")
            {
                m_dataStream = (CommandBeginDataStream)m_nextCommand;
                if (m_dataStream.EncodingMechanism != "Basic")
                {
                    throw new Exception("Data stream encoding is not supported.");
                }
            }
            else if (m_nextCommand.RootElement == "ProducerMetadata")
            {
                m_metadata = (SttpProducerMetadata)m_nextCommand;
                return FileReaderItem.ProducerMetadata;
            }
            goto TryAgain;
        }

        public bool ReadDataPoint(SttpDataPoint dataPoint)
        {
            return m_decoder.Read(dataPoint);
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
