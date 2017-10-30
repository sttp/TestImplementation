using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.WireProtocol.Data;

namespace Sttp.WireProtocol.GetMetadata
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void Select(short tableIndex, short columnIndex)
        {
            m_stream.Write(SubCommand.Select);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
        }

        public void Join(short tableIndex, short columnIndex, short foreignTableIndex)
        {
            m_stream.Write(SubCommand.Join);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(foreignTableIndex);
        }

        public void WhereInString(short tableIndex, short columnIndex, bool areRegularExpressions, string[] items)
        {
            m_stream.Write(SubCommand.WhereInString);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(areRegularExpressions);
            m_stream.Write(items);
        }

        public void WhereInValue(short tableIndex, short columnIndex, byte[][] items)
        {
            m_stream.Write(SubCommand.WhereInValue);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(items);
        }

        public void WhereCompare(short tableIndex, short columnIndex, CompareMethod compareOperator, byte[] item)
        {
            m_stream.Write(SubCommand.WhereCompare);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(compareOperator);
            m_stream.Write(item);
        }

        public void WhereOperator(OperatorMethod operatorCode)
        {
            m_stream.Write(SubCommand.WhereOperator);
            m_stream.Write(operatorCode);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            m_stream.Write(MetadataSubCommand.DatabaseVersion);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

    }
}
