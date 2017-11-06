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
        /// Guid schemaVersion, 
        /// long revision, 
        /// </summary>
        DatabaseVersion,

        /// <summary>
        /// Specified version is not compatible. Recompile query with the latest schema.
        /// </summary>
        VersionNotCompatible,

        /// <summary>
        /// Defines a table. 
        /// 
        /// Payload: 
        /// short tableIndex,      -For joined data, the table is the left most table.
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        DefineTable,

        /// <summary>
        /// Defines a column.
        /// 
        /// Payload: 
        /// short tableIndex,      -For joined data, the table is the left most table.
        /// short columnIndex,     -For joined data, this is a numeric sequence.
        /// string columnName,     -For joined data, this will be the original column name. Renaming will be a client side activity.
        /// ValueType columnType
        /// 
        /// </summary>
        DefineColumn,

        /// <summary>
        /// Defines a row to an existing table. This will either be a new row, or replacing an exiting one.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// SttpValue primaryKey,  -The primary key for the data being parsed.
        /// SttpValueSet fields    -All of the fields for this row.
        /// 
        /// </summary>
        DefineRow,

        /// <summary>
        /// Indicates that a row of data has been removed. It's possible that the client may not have this row. In which case, ignore it.
        /// This is only valid for queries that are specified as update queries.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        UndefineRow,
    }
}