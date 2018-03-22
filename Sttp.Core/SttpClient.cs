using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Sttp.Codec;
using Sttp.Core.BulkTransport;
using Sttp.Core.Data;

namespace Sttp.Core
{
    public class SttpClient
    {
        private Stream m_stream;
        private WireEncoder m_encoder;
        private WireDecoder m_decoder;
        private byte[] m_buffer = new byte[4096];

        public SttpClient(Stream networkStream)
        {
            m_stream = networkStream;
            m_encoder = new WireEncoder();
            m_decoder = new WireDecoder();
            m_encoder.NewPacket += M_encoder_NewPacket;
        }

        private void M_encoder_NewPacket(byte[] data, int offset, int length)
        {
            m_stream.Write(data, offset, length);
        }

        public List<string> GetMetaDataTableList()
        {
            m_encoder.GetMetadataSchema();
            var cmd = GetNextCommand();
            return cmd.MetadataSchema.Tables.Select(x => x.TableName).ToList();
        }

        public List<string> GetMetaDataFieldList(string tableName)
        {
            m_encoder.GetMetadataSchema();
            var cmd = GetNextCommand();
            return cmd.MetadataSchema.Tables.First(x => x.TableName == tableName).Columns.Select(x => x.Name).ToList();
        }

        private CommandObjects GetNextCommand()
        {
            TryAgain:
            CommandObjects obj = m_decoder.NextCommand();
            if (obj != null)
                return obj;
            int length = 0;

            if ((length = m_stream.Read(m_buffer, 0, m_buffer.Length)) > 0)
            {
                m_decoder.FillBuffer(m_buffer, 0, length);
                goto TryAgain;
            }

            throw new EndOfStreamException();
        }

    }
}
