using System;
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

        public void UseTable(int tableIndex)
        {
            EnsureCapacity(3);
            m_stream.Write(MetadataCommand.UseTable);
            m_stream.WriteInt15(tableIndex);
        }

        public void AddTable(Guid majorVersion, long minorVersion, string tableName, TableFlags tableFlags)
        {
            EnsureCapacity(1 + 16 + 8 + 3 + tableName.Length * 2 + 1);

            m_stream.Write(MetadataCommand.AddTable);
            m_stream.Write(tableName);
            m_stream.Write(tableFlags);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        public void AddColumn(int columnIndex, string columnName, ValueType columnType)
        {
            EnsureCapacity(1 + 2 + 3 + columnName.Length * 2 + 1);

            m_stream.Write(MetadataCommand.AddColumn);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write(columnType);
        }

        public void AddValue(int columnIndex, int rowIndex, byte[] value)
        {
            EnsureCapacity(1 + 2 + 4 + 3 + (value?.Length ?? 0) * 2);

            m_stream.Write(MetadataCommand.AddValue);
            m_stream.WriteInt15(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.Write(value);
        }

        public void DeleteRow(int rowIndex)
        {
            EnsureCapacity(5);
            m_stream.Write(MetadataCommand.DeleteRow);
            m_stream.Write(rowIndex);
        }

        public void TableVersion(int tableIndex, Guid majorVersion, long minorVersion)
        {
            EnsureCapacity(1 + 2 + 16 + 8);
            m_stream.Write(MetadataCommand.TableVersion);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        public void AddRelationship(int tableIndex, int columnIndex, int foreignTableIndex)
        {
            EnsureCapacity(1 + 2 + 2 + 2);
            m_stream.Write(MetadataCommand.TableVersion);
            m_stream.WriteInt15(tableIndex);
            m_stream.WriteInt15(columnIndex);
            m_stream.WriteInt15(foreignTableIndex);
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(int tableIndex, int[] columnList, string[] filterExpression)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.GetTable);
            m_stream.WriteInt15(tableIndex);
            m_stream.WriteArray(columnList);
            m_stream.WriteArray(filterExpression);
        }

        public void SyncTable(int tableIndex, Guid majorVersion, long minorVersion, int[] columnList)
        {
            EnsureCapacity(500); //Overflowing this packet size isn't that big of a deal.

            m_stream.Write(MetadataCommand.SyncTable);
            m_stream.WriteInt15(tableIndex);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteArray(columnList);
        }

        public void SelectAllTablesWithSchema()
        {
            EnsureCapacity(1);
            m_stream.Write((byte)MetadataCommand.SelectAllTablesWithSchema);
        }

        public void GetAllTableVersions()
        {
            EnsureCapacity(1);
            m_stream.Write((byte)MetadataCommand.GetAllTableVersions);
        }

        #endregion

    }
}
