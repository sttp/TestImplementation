using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.WireProtocol.Data
{
    public class MetadataEncoder
    {
        private MemoryStream m_stream = new MemoryStream();

        public void Clear()
        {
            m_stream.Write((byte)CommandCode3.MetadataClear);
        }

        public void DeleteTable(int tableIndex)
        {
            m_stream.Write((byte)CommandCode3.MetadataDeleteTable);
            m_stream.Write(tableIndex);
        }

        public void UpdateTable(int tableIndex, long transactionID)
        {
            m_stream.Write((byte)CommandCode3.MetadataUpdateTable);
            m_stream.Write(tableIndex);
            m_stream.Write(transactionID);
        }

        public void AddTable(Guid instanceID, long transactionID, string tableName, int tableIndex, bool isMappedToDataPoint)
        {
            m_stream.Write((byte)CommandCode3.MetadataAddTable);
            m_stream.Write(instanceID);
            m_stream.Write(transactionID);
            m_stream.Write(tableName);
            m_stream.Write(tableIndex);
            m_stream.Write(isMappedToDataPoint);
        }

        public void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType)
        {
            m_stream.Write((byte)CommandCode3.MetadataAddColumn);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write((byte)columnType);
        }

        public void DeleteColumn(int tableIndex, int columnIndex)
        {
            m_stream.Write((byte)CommandCode3.MetadataDeleteColumn);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
        }

        public void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
        {
            m_stream.Write((byte)CommandCode3.MetadataAddValue);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.WriteWithLength(value);
        }

        public void DeleteRow(int tableIndex, int rowIndex)
        {
            m_stream.Write((byte)CommandCode3.MetadataDeleteRow);
            m_stream.Write(tableIndex);
            m_stream.Write(rowIndex);
        }
    }
}
