using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadataBasic : CommandBase
    {
        public Guid? SchemaVersion;
        public long? SequenceNumber;
        public string Table;
        public List<string> Columns = new List<string>();

        public CommandGetMetadataBasic(Guid? schemaVersion, long? sequenceNumber, string table, IEnumerable<string> columns)
            : base("GetMetadataBasic")
        {
            SchemaVersion = schemaVersion;
            SequenceNumber = sequenceNumber;
            Table = table;
            Columns.AddRange(columns);
        }

        public CommandGetMetadataBasic(SttpMarkupReader reader)
            : base("GetMetadataBasic")
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid?)element.GetValue("SchemaVersion");
            SequenceNumber = (long?)element.GetValue("SequenceNumber");
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
            writer.WriteValue("SequenceNumber", SequenceNumber);
            writer.WriteValue("Table", Table);
            foreach (var column in Columns)
            {
                writer.WriteValue("Column", column);
            }
        }

        //public CommandGetMetadataAdvance ToSttpQuery()
        //{
        //    var qs = new CommandGetMetadataAdvance();
        //    qs.DirectTable = Table;
        //    for (int x = 0; x < Columns.Count; x++)
        //    {
        //        qs.ColumnInputs.Add(new SttpQueryColumn(0, Columns[0], x));
        //        qs.Outputs.Add(new SttpQueryOutputColumns(x, Columns[x]));
        //    }
        //    return qs;
        //}
    }
}