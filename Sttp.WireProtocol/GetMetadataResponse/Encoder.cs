﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
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

    }
}