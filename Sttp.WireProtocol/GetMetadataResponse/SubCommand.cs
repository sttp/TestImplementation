namespace Sttp.WireProtocol.GetMetadataResponse
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates the current version of the database.
        /// 
        /// Payload:
        /// Guid majorVersion, 
        /// long minorVersion, 
        /// </summary>
        DatabaseVersion,

        /// <summary>
        /// Clears the existing database.
        /// </summary>
        Clear,

        /// <summary>
        /// Adds a table.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        AddTable,

        /// <summary>
        /// Adds a column.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// short columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        AddColumn,

        /// <summary>
        /// Adds a row to an existing table.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        AddRow,

        /// <summary>
        /// Adds or updates a value. Deleting a value would be to assign it with null.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// short columnIndex, 
        /// int rowIndex, 
        /// byte[] value
        /// 
        /// </summary>
        AddValue,

        /// <summary>
        /// Removes an entire row of data.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        DeleteRow,
    }
}