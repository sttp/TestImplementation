using System;
using System.IO;
using Sttp.IO;

namespace Sttp.WireProtocol.Data
{
    public class MetadataDecoder
    {
        private MemoryStream m_stream;

        public void WriteData(byte[] buffer, int position, int length)
        {

        }

        public CommandCode3 NextCommand()
        {
            return (CommandCode3)m_stream.ReadNextByte();
        }

        public void Clear()
        {
            //Just a placeholder to be complete.
        }

        public void DeleteTable(out int tableIndex)
        {
            tableIndex = m_stream.ReadInt32();
        }

        public void UpdateTable(out int tableIndex, out long transactionID)
        {
            tableIndex = m_stream.ReadInt32();
            transactionID = m_stream.ReadInt64();
        }

        public void AddTable(out Guid instanceID, out long transactionID, out string tableName, out int tableIndex, out bool isMappedToDataPoint)
        {
            instanceID = m_stream.ReadGuid();
            transactionID = m_stream.ReadInt64();
            tableName = m_stream.ReadString();
            tableIndex = m_stream.ReadInt32();
            isMappedToDataPoint = m_stream.ReadBoolean();
        }

        public void AddColumn(out int tableIndex, out int columnIndex, out string columnName, out ValueType columnType)
        {
            tableIndex = m_stream.ReadInt32();
            columnIndex = m_stream.ReadInt32();
            columnName = m_stream.ReadString();
            columnType = (ValueType)m_stream.ReadNextByte();
        }

        public void DeleteColumn(out int tableIndex, out int columnIndex)
        {
            tableIndex = m_stream.ReadInt32();
            columnIndex = m_stream.ReadInt32();
        }

        public void AddValue(out int tableIndex, out int columnIndex, out int rowIndex, out byte[] value)
        {
            tableIndex = m_stream.ReadInt32();
            columnIndex = m_stream.ReadInt32();
            rowIndex = m_stream.ReadInt32();
            value = m_stream.ReadBytes();
        }

        public void DeleteRow(out int tableIndex, out int rowIndex)
        {
            tableIndex = m_stream.ReadInt32();
            rowIndex = m_stream.ReadInt32();
        }
    }
}