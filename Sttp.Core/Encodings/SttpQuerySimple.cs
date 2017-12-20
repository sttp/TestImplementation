using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using Sttp.Codec;

namespace Sttp
{
    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQuerySimple : SttpQueryBase
    {
        public string Table;
        public List<string> Columns = new List<string>();

        public SttpQuerySimple()
            : base("SttpQuerySimple")
        {

        }

        public SttpQuerySimple(SttpMarkupElement reader)
            : base("SttpQuerySimple")
        {
            foreach (var item in reader.ChildValues)
            {
                switch (item.ValueName)
                {
                    case "Table":
                        Table = (string)item.Value;
                        break;
                    case "Column":
                        Columns.Add((string)item.Value);
                        break;
                    default:
                        throw new Exception("Unknown value");
                }
                item.Handled = true;
            }

            reader.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("Table", Table);

            foreach (var item in Columns)
            {
                writer.WriteValue("Column", item);
            }
        }

        public SttpMarkup ToSttpMarkup()
        {
            var sml = new SttpMarkupWriter("SttpQuerySimple");
            Save(sml);
            return sml.ToSttpMarkup();
        }

        public SttpQueryStatement ToSttpQuery()
        {
            var qs = new SttpQueryStatement();
            qs.DirectTable = Table;
            for (int x = 0; x < Columns.Count; x++)
            {
                qs.ColumnInputs.Add(new SttpQueryColumn(0, Columns[0], x));
                qs.Outputs.Add(new SttpQueryOutputColumns(x, Columns[x]));
            }
            return qs;
        }

        public static SttpQuerySimple Parse(string table, IEnumerable<string> columns)
        {
            var qry = new SttpQuerySimple();
            qry.Table = table;
            qry.Columns.AddRange(columns);
            return qry;
        }
    }
}
