//using System;
//using System.IO;
//using Sttp.IO;

//namespace Sttp.WireProtocol.Data.Condensed
//{
//    public class MetadataDecoder : IMetadataDecoder
//    {
//        private MemoryStream m_stream = new MemoryStream();

//        public void BeginCommand(byte[] buffer, int position, int length)
//        {
//            m_stream.Position = 0;
//            m_stream.SetLength(0);
//            m_stream.Write(buffer, position, length);
//            m_stream.Position = 0;
//        }

//        public MetadataCommand NextCommand()
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            m_stream.Position -= 1;
//            return command;
//        }

//        public void Clear()
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            //Just a placeholder to be complete.
//        }

//        public void DeleteTable(out int tableIndex)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();

//        }

//        public void UpdateTable(out int tableIndex, out long transactionID)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();
//            transactionID = m_stream.ReadInt64();
//        }

//        public void AddTable(out Guid instanceID, out long transactionID, out string tableName, out int tableIndex, out bool isMappedToDataPoint)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            instanceID = m_stream.ReadGuid();
//            transactionID = m_stream.ReadInt64();
//            tableName = m_stream.ReadString();
//            tableIndex = m_stream.ReadInt32();
//            isMappedToDataPoint = m_stream.ReadBoolean();
//        }

//        public void AddColumn(out int tableIndex, out int columnIndex, out string columnName, out ValueType columnType)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();
//            columnIndex = m_stream.ReadInt32();
//            columnName = m_stream.ReadString();
//            columnType = (ValueType)m_stream.ReadNextByte();
//        }

//        public void DeleteColumn(out int tableIndex, out int columnIndex)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();
//            columnIndex = m_stream.ReadInt32();
//        }

//        public void AddValue(out int tableIndex, out int columnIndex, out int rowIndex, out byte[] value)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();
//            columnIndex = m_stream.ReadInt32();
//            rowIndex = m_stream.ReadInt32();
//            value = m_stream.ReadBytes();
//        }

//        public void DeleteRow(out int tableIndex, out int rowIndex)
//        {
//            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
//            tableIndex = m_stream.ReadInt32();
//            rowIndex = m_stream.ReadInt32();
//        }
//    }
//}