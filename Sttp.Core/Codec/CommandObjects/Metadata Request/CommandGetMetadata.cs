using System.Collections.Generic;
using CTP;
using CTP.Serialization;

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
    [CtpSerializable("GetMetadata")]
    public class CommandGetMetadata
    {
        [CtpSerializeField()]
        public string Table { get; private set; }
        [CtpSerializeField()]
        public List<string> Columns { get; private set; } = new List<string>();

        public CommandGetMetadata(string table, IEnumerable<string> columns)
        {
            Table = table;
            Columns.AddRange(columns);
        }

        //Exists to support CtpSerializable
        private CommandGetMetadata() { }

    }
}