using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    /// <summary>
    /// Encodes a metadata packet.
    /// </summary>
    public class MetadataEncoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.Metadata;
        private SessionDetails m_sessionDetails;

        public MetadataEncoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void Clear()
        {
            m_stream.Write(MetadataSubCommand.Clear);
        }

        public void AddTable(short tableIndex, string tableName, TableFlags tableFlags)
        {
            m_stream.Write(MetadataSubCommand.AddTable);
            m_stream.Write(tableIndex);
            m_stream.Write(tableName);
            m_stream.Write(tableFlags);
        }

        public void AddColumn(short tableIndex, short columnIndex, string columnName, ValueType columnType)
        {
            m_stream.Write(MetadataSubCommand.AddColumn);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write(columnType);
        }

        public void AddRow(short tableIndex, int rowIndex)
        {
            m_stream.Write(MetadataSubCommand.AddRow);
            m_stream.Write(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void AddValue(short tableIndex, short columnIndex, int rowIndex, byte[] value)
        {
            m_stream.Write(MetadataSubCommand.AddValue);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.Write(value);
        }

        public void DeleteRow(short tableIndex, int rowIndex)
        {
            m_stream.Write(MetadataSubCommand.DeleteRow);
            m_stream.Write(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            m_stream.Write(MetadataSubCommand.DatabaseVersion);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        public void Select(short tableIndex, short columnIndex)
        {
            m_stream.Write(MetadataSubCommand.Select);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
        }

        public void Join(short tableIndex, short columnIndex, short foreignTableIndex)
        {
            m_stream.Write(MetadataSubCommand.Join);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(foreignTableIndex);
        }

        public void WhereInString(short tableIndex, short columnIndex, bool areRegularExpressions, string[] items)
        {
            m_stream.Write(MetadataSubCommand.WhereInString);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(areRegularExpressions);
            m_stream.Write(items);
        }

        public void WhereInValue(short tableIndex, short columnIndex, byte[][] items)
        {
            m_stream.Write(MetadataSubCommand.WhereInValue);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(items);
        }

       
        public void WhereCompare(short tableIndex, short columnIndex, MetadataCompareMethod compareOperator, byte[] item)
        {
            m_stream.Write(MetadataSubCommand.WhereCompare);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(compareOperator);
            m_stream.Write(item);
        }

        

        public void WhereOperator(MetadataOperatorMethod operatorCode)
        {
            m_stream.Write(MetadataSubCommand.WhereOperator);
            m_stream.Write(operatorCode);
        }

        public void GetDatabaseSchema()
        {
            m_stream.Write((byte)MetadataSubCommand.GetDatabaseSchema);
        }

        public void GetDatabaseVersion()
        {
            m_stream.Write((byte)MetadataSubCommand.GetDatabaseVersion);
        }

    }
}
