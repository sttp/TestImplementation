using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        private MemoryStream m_stream = new MemoryStream();

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Position = 0;
            m_stream.SetLength(0);
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
            throw new NotImplementedException();
        }

        public void AddTable(Guid majorVersion, long minorVersion, string tableName, bool isMappedToDataPoint)
        {
            m_stream.Write((byte)MetadataCommand.AddTable);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.Write(tableName);
            m_stream.Write(isMappedToDataPoint);
        }

        public void AddColumn(int columnIndex, string columnName, ValueType columnType)
        {
            m_stream.Write((byte)MetadataCommand.AddColumn);
            m_stream.Write(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write((byte)columnType);
        }

        public void AddValue(int columnIndex, int rowIndex, byte[] value)
        {
            m_stream.Write((byte)MetadataCommand.AddValue);
            m_stream.Write(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.WriteWithLength(value);
        }

        public void DeleteRow(int rowIndex)
        {
            m_stream.Write((byte)MetadataCommand.DeleteRow);
            m_stream.Write(rowIndex);
        }

        public void TableVersion(int tableIndex, Guid majorVersion, long minorVersion)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(int tableIndex, int[] columnList, string[] filterExpression)
        {
            m_stream.Write((byte)MetadataCommand.GetTable);
            m_stream.Write(tableIndex);
            m_stream.Write(columnList.Length);
            foreach (var item in columnList)
            {
                m_stream.Write(item);
            }
        }

        public void SyncTable(int tableIndex, Guid majorVersion, long minorVersion, int[] columnList)
        {
            m_stream.Write((byte)MetadataCommand.SyncTable);
            m_stream.Write(tableIndex);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.Write(columnList.Length);
            foreach (var item in columnList)
            {
                m_stream.Write(item);
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
