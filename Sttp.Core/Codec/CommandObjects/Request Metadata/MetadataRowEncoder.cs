using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataRowEncoder
    {
        private readonly List<MetadataColumn> m_columns;
        private ByteWriter m_stream;

        public MetadataRowEncoder(List<MetadataColumn> columns)
        {
            m_columns = columns;
            m_stream = new ByteWriter();
        }

        public void Clear()
        {
            m_stream.Clear();
        }

        public int Size => m_stream.Length;

        public void AddRow(CtpObject[] row)
        {
            if (row.Length != m_columns.Count)
                throw new ArgumentException("The number of elements in array does not match the number of columns in the response", nameof(row));

            for (int x = 0; x < row.Length; x++)
            {
                CtpValueEncodingNative.Save(m_stream, row[x]);
            }
        }

        public byte[] ToArray()
        {
            return m_stream.ToArray();
        }
    }
}
