using System;
using CTP;

namespace Sttp.Codec
{
    public class MetadataRowDecoder
    {
        private CtpObjectReader m_stream;
        private CommandBeginMetadataResponse m_response;
        public MetadataRowDecoder(CommandBeginMetadataResponse metadataResponse)
        {
            m_response = metadataResponse;
            m_stream = new CtpObjectReader();
        }

        public void Load(byte[] data)
        {
            m_stream.SetBuffer(data, 0, data.Length);
        }

        public bool Read(CtpObject[] row)
        {
            if (row.Length != m_response.Columns.Count)
                throw new ArgumentException("The number of elements in array does not match the number of columns in the response", nameof(row));

            if (m_stream.IsEmpty)
            {
                //It's possible that this is not enough since items might eventually be stored with a few bits, so I need some kind of extra escape sequence.
                return false;
            }

            for (int x = 0; x < row.Length; x++)
            {
                row[x] = m_stream.Read();
            }

            return true;
        }



    }
}
