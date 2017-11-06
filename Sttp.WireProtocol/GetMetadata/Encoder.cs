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

        public void Select(string tableName, string columnName, string aliasName)
        {
            Stream.Write(SubCommand.Select);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(aliasName);
        }

        public void Join(string tableName, string columnName, string foreignTableName, string tableAlias, bool isLeftJoin)
        {
            Stream.Write(SubCommand.Join);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(foreignTableName);
            Stream.Write(tableAlias);
            Stream.Write(isLeftJoin);
        }

        public void WhereInString(string tableName, string columnName, bool areRegularExpressions, string[] items)
        {
            Stream.Write(SubCommand.WhereInString);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(areRegularExpressions);
            Stream.Write(items);
        }

        public void WhereInValue(string tableName, string columnName, SttpValueSet items)
        {
            Stream.Write(SubCommand.WhereInValue);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(items);
        }

        public void WhereCompare(string tableName, string columnName, CompareMethod compareOperator, SttpValue item)
        {
            Stream.Write(SubCommand.WhereCompare);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(compareOperator);
            Stream.Write(item);
        }

        public void WhereOperator(OperatorMethod operatorCode)
        {
            Stream.Write(SubCommand.WhereOperator);
            Stream.Write(operatorCode);
        }

        public void DatabaseVersion(Guid schemaVersion, long revision)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
        }

    }
}
