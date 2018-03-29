using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadata : CommandBase
    {
        public string Table;
        public List<string> Columns = new List<string>();

        public CommandGetMetadata(string table, IEnumerable<string> columns)
            : base("GetMetadata")
        {
            Table = table;
            Columns.AddRange(columns);
        }

        public CommandGetMetadata(SttpMarkupReader reader)
            : base("GetMetadata")
        {
            var element = reader.ReadEntireElement();

            Table = (string)element.GetValue("Table");
            foreach (string c in element.ForEachValue("Column"))
            {
                Columns.Add(c);
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("Table", Table);
            foreach (var column in Columns)
            {
                writer.WriteValue("Column", column);
            }
        }
    }
}