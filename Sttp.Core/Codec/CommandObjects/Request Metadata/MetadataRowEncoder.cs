using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataRowEncoder
    {
        private ByteWriter m_stream;
        private CommandBeginMetadataResponse m_response;

        public MetadataRowEncoder(CommandBeginMetadataResponse metadataResponse)
        {
            m_response = metadataResponse;
            m_stream = new ByteWriter();
        }

        public void Clear()
        {
            m_stream.Clear();
        }

        public void AddRow(SttpValueMutable[] row)
        {
            if (row.Length != m_response.Columns.Count)
                throw new ArgumentException("The number of elements in array does not match the number of columns in the response", nameof(row));

            for (int x = 0; x < row.Length; x++)
            {
                SttpValueEncodingNative.Save(m_stream, row[x]);
            }
        }

        public byte[] ToArray()
        {
            return m_stream.ToArray();
        }
    }
}
