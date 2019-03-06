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
using Sttp.Metadata;

namespace Sttp
{

    public enum EncodingMethod
    {
        Normal,
        //Adaptive
    }

    public class SttpFileWriter : IDisposable
    {
        private CtpFileStream m_stream;
        private EncoderBase m_encoder;

        public SttpFileWriter(Stream stream, bool ownsStream, CtpCompressionMode mode, EncodingMethod encoding)
        {
            m_stream = new CtpFileStream(stream, mode, ownsStream);
            if (encoding == EncodingMethod.Normal)
            {
                m_encoder = new NormalEncoder();
            }
            m_encoder.Clear();
        }

        public void ProducerMetadata(CommandC37ConfigFrame metadata)
        {
            if (m_encoder.Length > 0)
            {
                m_stream.Write(m_encoder.ToArray());
                m_encoder.Clear();
            }

            m_stream.Write(metadata);
        }

        public void ProducerMetadata(SttpProducerMetadata metadata)
        {
            if (m_encoder.Length > 0)
            {
                m_stream.Write(m_encoder.ToArray());
                m_encoder.Clear();
            }

            m_stream.Write(metadata);
        }

        public void AddDataPoint(SttpDataPoint dataPoint)
        {
            m_encoder.AddDataPoint(dataPoint);
            if (m_encoder.Length > 1500)
            {
                m_stream.Write(m_encoder.ToArray());
                m_encoder.Clear();
            }
        }

        public void Dispose()
        {
            if (m_encoder.Length > 0)
            {
                m_stream.Write(m_encoder.ToArray());
                m_encoder.Clear();
            }

            m_stream?.Dispose();
        }
    }
}
