using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadata
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly bool AreUpdateQueries;
        public readonly List<SttpQueryStatement> Queries;
        public readonly List<SttpQueryRaw> QueriesRaw;

        public CommandGetMetadata(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            AreUpdateQueries = reader.ReadBoolean();
            Queries = reader.ReadListSttpQueryStatement();
            QueriesRaw = reader.ReadListSttpQueryRaw();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(CommandMetadataSchema) + ")");
            builder.Append(linePrefix); builder.AppendLine($"SchemaVersion: {SchemaVersion} ");
            builder.Append(linePrefix); builder.AppendLine($"Revision: {Revision} ");
            builder.Append(linePrefix); builder.AppendLine($"AreUpdateQueries {AreUpdateQueries} ");
            builder.Append(linePrefix); builder.AppendLine($"Queries Count {Queries.Count} ");
            foreach (var table in Queries)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"QueriesRaw Count {QueriesRaw.Count} ");
            foreach (var table in QueriesRaw)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
        }
    }
}