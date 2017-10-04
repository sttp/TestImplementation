using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data.Raw
{
    public class MetadataDecoder : IMetadataDecoder
    {
        private StreamReader m_stream = new StreamReader();

        public void BeginCommand(byte[] buffer, int position, int length)
        {
            m_stream.Clear();
            m_stream.Fill(buffer, position, length);
        }

        public MetadataCommand NextCommand()
        {
            MetadataCommand command = m_stream.ReadMetadataCommand();
            m_stream.Position -= 1;
            return command;
        }

        #region [ Response Publisher to Subscriber ]

        public void UseTable(out int tableIndex)
        {
            if (MetadataCommand.UseTable != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            tableIndex = m_stream.ReadInt32();
        }

        public void AddTable(out Guid majorVersion, out long minorVersion, out string tableName, out TableFlags tableFlags)
        {
            if (MetadataCommand.AddTable != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            tableName = m_stream.ReadString();
            tableFlags = (TableFlags)m_stream.ReadByte();
            majorVersion = m_stream.ReadGuid();
            minorVersion = m_stream.ReadInt64();
        }

        public void AddColumn(out int columnIndex, out string columnName, out ValueType columnType)
        {
            if (MetadataCommand.AddColumn != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            columnIndex = m_stream.ReadInt32();
            columnName = m_stream.ReadString();
            columnType = m_stream.ReadValueType();
        }

        public void AddValue(out int columnIndex, out int rowIndex, out byte[] value)
        {
            if (MetadataCommand.AddValue != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            columnIndex = m_stream.ReadInt32();
            rowIndex = m_stream.ReadInt32();
            value = m_stream.ReadBytes();
        }

        public void DeleteRow(out int rowIndex)
        {
            if (MetadataCommand.DeleteRow != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            rowIndex = m_stream.ReadInt32();
        }

        public void TableVersion(out int tableIndex, out Guid majorVersion, out long minorVersion)
        {
            if (MetadataCommand.TableVersion != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            tableIndex = m_stream.ReadInt32();
            majorVersion = m_stream.ReadGuid();
            minorVersion = m_stream.ReadInt64();
        }

        public void AddRelationship(out int tableIndex, out int columnIndex, out int foreignTableIndex)
        {
            if (MetadataCommand.AddRelationship != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
            tableIndex = m_stream.ReadInt32();
            columnIndex = m_stream.ReadInt32();
            foreignTableIndex = m_stream.ReadInt32();
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
            if (MetadataCommand.SelectAllTablesWithSchema != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
        }

        public void GetAllTableVersions()
        {
            if (MetadataCommand.GetAllTableVersions != m_stream.ReadMetadataCommand())
                throw new InvalidOperationException("Wrong Method Called");
        }

        #endregion


    }
}