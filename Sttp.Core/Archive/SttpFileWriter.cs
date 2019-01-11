using System;
using System.Collections.Generic;
using System.Data;
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
    public class SttpFileWriter : IDisposable
    {
        private CtpFileStream m_stream;
        private bool m_initializedDataStream;

        private BasicEncoder m_encoder;

        public SttpFileWriter(Stream stream, bool ownsStream)
        {
            m_stream = new CtpFileStream(stream, ownsStream);
            m_encoder = new BasicEncoder(100_000);
        }

        public void ProducerMetadata(SttpProducerMetadata metadata)
        {
            if (m_encoder.Length > 0)
            {
                m_stream.Write(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear();
            }
            m_stream.Write(metadata);
        }

        public void AddDataPoint(SttpDataPoint dataPoint)
        {
            if (!m_initializedDataStream)
            {
                StartDataStream();
            }
            m_encoder.AddDataPoint(dataPoint);
            if (m_encoder.Length > 1000)
            {
                m_stream.Write(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear();
            }
        }

        private void StartDataStream()
        {
            m_initializedDataStream = true;
            m_stream.Write(new CommandBeginDataStream(0, "Basic"));
        }

        public void Dispose()
        {
            if (m_encoder.Length > 0)
            {
                m_stream.Write(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear();
            }

            m_stream?.Dispose();
        }
    }
}
