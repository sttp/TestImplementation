using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadataSimple : CommandBase
    {
        public Guid? SchemaVersion;
        public long? LastModifiedVersion;
        public string Table;
        public List<string> Columns = new List<string>();

        public CommandGetMetadataSimple(Guid? schemaVersion, long? lastModifiedVersion, string table, IEnumerable<string> columns)
            : base("GetMetadataSimple")
        {
            SchemaVersion = schemaVersion;
            LastModifiedVersion = lastModifiedVersion;
            Table = table;
            Columns.AddRange(columns);
        }

        public CommandGetMetadataSimple(SttpMarkupReader reader)
            : base("GetMetadataSimple")
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid?)element.GetValue("SchemaVersion");
            LastModifiedVersion = (long)element.GetValue("LastModifiedVersion");
            Table = (string)element.GetValue("Table");
            foreach (string c in element.ForEachValue("Column"))
            {
                Columns.Add(c);
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("LastModifiedVersion", LastModifiedVersion);
            writer.WriteValue("Table", Table);
            foreach (var column in Columns)
            {
                writer.WriteValue("Column", column);
            }
        }

        public CommandGetMetadataStatement ToSttpQuery()
        {
            var qs = new CommandGetMetadataStatement();
            qs.DirectTable = Table;
            for (int x = 0; x < Columns.Count; x++)
            {
                qs.ColumnInputs.Add(new SttpQueryColumn(0, Columns[0], x));
                qs.Outputs.Add(new SttpQueryOutputColumns(x, Columns[x]));
            }
            return qs;
        }
    }
}