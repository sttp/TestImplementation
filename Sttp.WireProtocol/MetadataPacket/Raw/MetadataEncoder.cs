using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data.Raw
{
    /// <summary>
    /// Encodes a metadata packet.
    /// </summary>
    public class MetadataEncoder : IMetadataEncoder
    {
        private StreamWriter m_stream;
        private int m_autoFlushLevel;
        private Action<byte[], int, int> m_sendPacket;

        public MetadataEncoder(Action<byte[], int, int> sendPacket, int autoFlushLevel)
        {
            m_stream = new StreamWriter();
            m_sendPacket = sendPacket;
            m_autoFlushLevel = autoFlushLevel;
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Clear();
            m_stream.Write(CommandCode.Metadata);
            m_stream.Write((ushort)0); //Packet Length
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            if (m_stream.Position <= 3) //3 bytes means nothing will be sent.
                return;
            int length = m_stream.Length;
            m_stream.Position = 1;
            m_stream.Write((ushort)length);
            m_sendPacket(m_stream.Buffer, 0, length);
        }

        private void EnsureCapacity(int length)
        {
            if (m_stream.Length + length > m_autoFlushLevel && m_stream.Position > 3)
            {
                EndCommand();
                BeginCommand();
            }
        }

        #region [ Response Publisher to Subscriber ]

        public void Clear()
        {
            EnsureCapacity(1);
            m_stream.Write(MetadataCommand.Clear);
        }

        public void AddTable(int tableIndex, string tableName, TableFlags tableFlags)
        {
            EnsureCapacity(1 + 2 + 3 + tableName.Length * 2 + 1);
            m_stream.Write(MetadataCommand.AddTable);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(tableName);
            m_stream.Write(tableFlags);
        }

        public void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType)
        {
            EnsureCapacity(1 + 2 + 2 + 3 + columnName.Length * 2 + 1);

            m_stream.Write(MetadataCommand.AddColumn);
            m_stream.WriteInt15(tableIndex);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write(columnType);
        }

        public void AddRow(int tableIndex, int rowIndex)
        {
            EnsureCapacity(5 + 2);
            m_stream.Write(MetadataCommand.AddRow);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
        {
            EnsureCapacity(1 + 2 + 2 + 4 + 3 + (value?.Length ?? 0) * 2);

            m_stream.Write(MetadataCommand.AddValue);
            m_stream.WriteInt15(tableIndex);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.Write(value);
        }

        public void DeleteRow(int tableIndex, int rowIndex)
        {
            EnsureCapacity(5 + 2);
            m_stream.Write(MetadataCommand.DeleteRow);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            EnsureCapacity(1 + 16 + 8);
            m_stream.Write(MetadataCommand.DatabaseVersion);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(int tableIndex, int[] columnList, List<Tuple<int, string>> filterExpression)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.GetTable);
            m_stream.WriteInt15(tableIndex);
            m_stream.WriteArray(columnList);
            m_stream.WriteList(filterExpression);
        }

        public void GetQuery(List<Tuple<int, int>> columnList, List<Tuple<int, int, int>> joinFields, List<Tuple<int, int, string>> filterExpression)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.GetTable);
            m_stream.WriteList(columnList);
            m_stream.WriteList(joinFields);
            m_stream.WriteList(filterExpression);
        }

        public void SyncDatabase(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.SyncDatabase);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteList(columnList);
        }

        public void SyncTableOrQuery(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList, List<Tuple<int, int>> criticalColumnList)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.SyncTableOrQuery);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteList(columnList);
            m_stream.WriteList(criticalColumnList);
        }

        public void GetDatabaseSchema()
        {
            EnsureCapacity(1);
            m_stream.Write((byte)MetadataCommand.GetDatabaseSchema);
        }

        public void GetDatabaseVersion()
        {
            EnsureCapacity(1);
            m_stream.Write((byte)MetadataCommand.GetDatabaseVersion);
        }

        #endregion

    }
}
