using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadata
    {
        public readonly Guid RequestID;
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly bool AreUpdateQueries;
        public readonly SttpMarkup Queries;

        public CommandGetMetadata(PayloadReader reader)
        {
            RequestID = reader.ReadGuid();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            AreUpdateQueries = reader.ReadBoolean();
            Queries = reader.ReadSttpMarkup();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(CommandMetadataSchema) + ")");
            builder.Append(linePrefix); builder.AppendLine($"RequestID: {RequestID} ");
            builder.Append(linePrefix); builder.AppendLine($"SchemaVersion: {SchemaVersion} ");
            builder.Append(linePrefix); builder.AppendLine($"Revision: {Revision} ");
            builder.Append(linePrefix); builder.AppendLine($"AreUpdateQueries {AreUpdateQueries} ");
            builder.AppendLine(Queries.ToXML());
        }
    }
}