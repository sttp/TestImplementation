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
using Sttp.DataPointEncoding;

namespace Sttp
{
    public enum SttpCompressionMode
    {
        None,
        Deflate,
        Zlib
    }

    public enum EncodingMethod
    {
        Basic,
        Raw,
        Adaptive
    }

    public class SttpFileWriter : IDisposable
    {
        private CtpFileStream m_stream;
        private EncoderBase m_encoder;
        private DefalteHelper m_comp;

        public SttpFileWriter(Stream stream, bool ownsStream, SttpCompressionMode mode, EncodingMethod encoding)
        {
            m_stream = new CtpFileStream(stream, ownsStream);
            if (encoding == EncodingMethod.Raw)
            {
                m_encoder = new RawEncoder();
                m_stream.Write(new CommandBeginDataStream(0, "Raw"));
            }
            if (encoding == EncodingMethod.Basic)
            {
                m_encoder = new BasicEncoder();
                m_stream.Write(new CommandBeginDataStream(0, "Basic"));
            }
            if (encoding == EncodingMethod.Adaptive)
            {
                m_encoder = new AdaptiveEncoder();
                m_stream.Write(new CommandBeginDataStream(0, "Adaptive"));
            }

            if (mode == SttpCompressionMode.Deflate)
            {
                m_stream.Write(new CommandBeginCompressionStream(1, "Deflate"));
                m_comp = new DefalteHelper(new CommandBeginCompressionStream(1, "Deflate"));
            }
            if (mode == SttpCompressionMode.Zlib)
            {
                m_stream.Write(new CommandBeginCompressionStream(1, "zlib"));
                m_comp = new DefalteHelper(new CommandBeginCompressionStream(1, "zlib"));
            }
        }

        public void ProducerMetadata(SttpProducerMetadata metadata)
        {
            if (m_encoder.Length > 0)
            {
                WriteComp(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear(false);
            }

            WriteComp(metadata);
        }

        private void WriteComp(CtpCommand command)
        {
            if (m_comp == null)
            {
                m_stream.Write(command);
            }
            else
            {
                m_stream.Write(m_comp.Deflate(command));
            }
        }

        public void AddDataPoint(SttpDataPoint dataPoint)
        {
            m_encoder.AddDataPoint(dataPoint);
            if (m_encoder.Length > 1000)
            {
                WriteComp(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear(false);
            }
        }

        public void Dispose()
        {
            if (m_encoder.Length > 0)
            {
                WriteComp(new CtpRaw(m_encoder.ToArray(), 0));
                m_encoder.Clear(false);
            }

            m_stream?.Dispose();
        }
    }
}
