using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchema
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly List<MetadataSchemaTables> Tables;

        public CommandMetadataSchema(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            Tables = reader.ReadList<MetadataSchemaTables>();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(CommandMetadataSchema) + ")");
            builder.Append(linePrefix); builder.AppendLine($"SchemaVersion: {SchemaVersion} ");
            builder.Append(linePrefix); builder.AppendLine($"Revision: {SchemaVersion} ");
            builder.Append(linePrefix); builder.AppendLine($"Tables Count {Tables.Count} ");
            foreach (var table in Tables)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
        }
    }
}