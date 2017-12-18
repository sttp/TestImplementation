//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Sttp.Codec
//{
//    public class CommandMetadataSchemaUpdate
//    {
//        public readonly Guid SchemaVersion;
//        public readonly long Revision;
//        public readonly long UpdatedFromVersion;
//        public readonly List<MetadataSchemaTableUpdate> Tables;

//        public CommandMetadataSchemaUpdate(PayloadReader reader)
//        {
//            SchemaVersion = reader.ReadGuid();
//            Revision = reader.ReadInt64();
//            UpdatedFromVersion = reader.ReadInt64();
//            Tables = reader.ReadListMetadataSchemaTableUpdate();
//        }

//        public void GetFullOutputString(string linePrefix, StringBuilder builder)
//        {
//            builder.Append(linePrefix); builder.AppendLine("(" + nameof(CommandMetadataSchema) + ")");
//            builder.Append(linePrefix); builder.AppendLine($"SchemaVersion: {SchemaVersion} ");
//            builder.Append(linePrefix); builder.AppendLine($"Revision: {Revision} ");
//            builder.Append(linePrefix); builder.AppendLine($"UpdatedFromVersion: {UpdatedFromVersion} ");
//            builder.Append(linePrefix); builder.AppendLine($"Tables Count {Tables.Count} ");
//            foreach (var table in Tables)
//            {
//                table.GetFullOutputString(linePrefix + " ", builder);
//            }
//        }
//    }
//}