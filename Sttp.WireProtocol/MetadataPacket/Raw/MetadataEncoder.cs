using System;

namespace Sttp.WireProtocol.Data.Raw
{
    /// <summary>
    /// Encodes a metadata packet.
    /// </summary>
    public class MetadataEncoder : IMetadataEncoder
    {
        private Action<byte[], int, int> m_baseEncoder;

        public MetadataEncoder(Action<byte[], int, int> baseEncoder)
        {
            m_baseEncoder = baseEncoder;
        }

        private StreamWriter m_stream = new StreamWriter();

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Position = 0;
            m_stream.Length = 0;
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public byte[] EndCommand()
        {
            return m_stream.ToArray();
        }

        #region [ Response Publisher to Subscriber ]

        public void UseTable(int tableIndex)
        {
            m_stream.Write(MetadataCommand.UseTable);
            m_stream.WriteInt15(tableIndex);
        }

        public void AddTable(Guid majorVersion, long minorVersion, string tableName, bool isMappedToDataPoint)
        {
            m_stream.Write(MetadataCommand.AddTable);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.Write(tableName);
            m_stream.Write(isMappedToDataPoint);
        }

        public void AddColumn(int columnIndex, string columnName, ValueType columnType)
        {
            m_stream.Write(MetadataCommand.AddColumn);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write(columnType);
        }

        public void AddValue(int columnIndex, int rowIndex, byte[] value)
        {
            m_stream.Write(MetadataCommand.AddValue);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.Write(value);
        }

        public void DeleteRow(int rowIndex)
        {
            m_stream.Write(MetadataCommand.DeleteRow);
            m_stream.Write(rowIndex);
        }

        public void TableVersion(int tableIndex, Guid majorVersion, long minorVersion)
        {
            m_stream.Write(MetadataCommand.TableVersion);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(int tableIndex, int[] columnList, string[] filterExpression)
        {
            m_stream.Write(MetadataCommand.GetTable);
            m_stream.WriteInt15(tableIndex);

            m_stream.WriteInt15(columnList?.Length ?? 0);
            if (columnList?.Length > 0)
            {
                foreach (var item in columnList)
                {
                    m_stream.Write(item);
                }
            }

            m_stream.WriteInt15(filterExpression?.Length ?? 0);
            if (filterExpression?.Length > 0)
            {
                foreach (var item in filterExpression)
                {
                    m_stream.Write(item);
                }
            }
        }

        public void SyncTable(int tableIndex, Guid majorVersion, long minorVersion, int[] columnList)
        {
            m_stream.Write(MetadataCommand.SyncTable);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteInt15(columnList?.Length ?? 0);
            if (columnList?.Length > 0)
            {
                foreach (var item in columnList)
                {
                    m_stream.Write(item);
                }
            }
        }

        public void SelectAllTablesWithSchema()
        {
            m_stream.Write((byte)MetadataCommand.SelectAllTablesWithSchema);
        }

        public void GetAllTableVersions()
        {
            m_stream.Write((byte)MetadataCommand.GetAllTableVersions);
        }

        #endregion

    }
}
