using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Stream.Write(SubCommand.Select);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
        }

        public void Join(short tableIndex, short columnIndex, short foreignTableIndex, bool isLeftJoin)
        {
            Stream.Write(SubCommand.Join);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(foreignTableIndex);
            Stream.Write(isLeftJoin);
        }

        public void WhereInString(short tableIndex, short columnIndex, bool areRegularExpressions, string[] items)
        {
            Stream.Write(SubCommand.WhereInString);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(areRegularExpressions);
            Stream.Write(items);
        }

        public void WhereInValue(short tableIndex, short columnIndex, SttpValue[] items)
        {
            Stream.Write(SubCommand.WhereInValue);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(items);
        }

        public void WhereCompare(short tableIndex, short columnIndex, CompareMethod compareOperator, SttpValue item)
        {
            Stream.Write(SubCommand.WhereCompare);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(compareOperator);
            Stream.Write(item);
        }

        public void WhereOperator(OperatorMethod operatorCode)
        {
            Stream.Write(SubCommand.WhereOperator);
            Stream.Write(operatorCode);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(majorVersion);
            Stream.Write(minorVersion);
        }

    }
}
