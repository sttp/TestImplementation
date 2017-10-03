using System;
using System.IO;
using Sttp.IO;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data.Raw
{
    public class MetadataDecoder : IMetadataDecoder
    {
        private MemoryStream m_stream = new MemoryStream();

        public void BeginCommand(byte[] buffer, int position, int length)
        {
            m_stream.Position = 0;
            m_stream.SetLength(0);
            m_stream.Write(buffer, position, length);
            m_stream.Position = 0;
        }

        public MetadataCommand NextCommand()
        {
            MetadataCommand command = (MetadataCommand)m_stream.ReadNextByte();
            m_stream.Position -= 1;
            return command;
        }

        #region [ Response Publisher to Subscriber ]

        public void UseTable(out int tableIndex)
        {
            if (MetadataCommand.UseTable != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            tableIndex = m_stream.ReadInt32();
        }

        public void AddTable(out Guid majorVersion, out long minorVersion, out string tableName, out TableFlags tableFlags)
        {
            if (MetadataCommand.AddTable != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            tableName = m_stream.ReadString();
            tableFlags = (TableFlags)m_stream.ReadByte();
            majorVersion = m_stream.ReadGuid();
            minorVersion = m_stream.ReadInt64();
        }

        public void AddColumn(out int columnIndex, out string columnName, out ValueType columnType, out string referenceTable)
        {
            if (MetadataCommand.AddColumn != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            columnIndex = m_stream.ReadInt32();
            columnName = m_stream.ReadString();
            columnType = (ValueType)m_stream.ReadNextByte();
            referenceTable = m_stream.ReadString();
        }

        public void AddValue(out int columnIndex, out int rowIndex, out byte[] value)
        {
            if (MetadataCommand.AddValue != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            columnIndex = m_stream.ReadInt32();
            rowIndex = m_stream.ReadInt32();
            value = m_stream.ReadBytes();
        }

        public void DeleteRow(out int rowIndex)
        {
            if (MetadataCommand.DeleteRow != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            rowIndex = m_stream.ReadInt32();
        }

        public void TableVersion(out int tableIndex, out Guid majorVersion, out long minorVersion)
        {
            if (MetadataCommand.TableVersion != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
            tableIndex = m_stream.ReadInt32();
            majorVersion = m_stream.ReadGuid();
            minorVersion = m_stream.ReadInt64();
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(out int tableIndex, out int[] columnList, out string[] filterExpression)
        {
            throw new NotImplementedException();
        }

        public void SyncTable(out int tableIndex, out Guid majorVersion, out long minorVersion, out int[] columnList)
        {
            throw new NotImplementedException();
        }

        public void SelectAllTablesWithSchema()
        {
            if (MetadataCommand.SelectAllTablesWithSchema != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
        }

        public void GetAllTableVersions()
        {
            if (MetadataCommand.GetAllTableVersions != (MetadataCommand)m_stream.ReadNextByte())
                throw new InvalidOperationException("Wrong Method Called");
        }

        #endregion


    }
}