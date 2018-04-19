using System.Collections.Generic;
using CTP;

namespace Sttp.Codec
{
    /// <summary>
    /// Requests metadata. Specifies the table and optional columns to retrieve. 
    /// If the columns are empty, all columns will be returned for the specified table.
    /// 
    /// If there's an error, <see cref="CommandMetadataRequestFailed"/> will be returned.
    /// If successful, the following series of commands will occur:
    ///     <see cref="CommandBeginMetadataResponse"/> - Opening the raw channel to send the rows, and defining the response.
    ///     <see cref="CommandRaw"/> - The rows.
    ///     <see cref="CommandEndMetadataResponse"/> - Closing the raw channel.
    /// </summary>
    public class CommandGetMetadata : DocumentCommandBase
    {
        public string Table;
        public List<string> Columns = new List<string>();

        public CommandGetMetadata(string table, IEnumerable<string> columns)
            : base("GetMetadata")
        {
            Table = table;
            Columns.AddRange(columns);
        }

        public CommandGetMetadata(CtpDocumentReader reader)
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

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("Table", Table);
            foreach (var column in Columns)
            {
                writer.WriteValue("Column", column);
            }
        }
    }
}