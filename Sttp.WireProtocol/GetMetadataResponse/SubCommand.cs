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
        /// Specified version is not compatible. Recompile query with the latest schema.
        /// </summary>
        VersionNotCompatible,

        /// <summary>
        /// Defines the table structure of the requested table
        /// 
        /// Payload: 
        /// Guid schemaVersion, 
        /// long revision, 
        /// string tableName,
        /// TableFlags flags
        /// Array columns{string columnName, SttpValueTypeCode columnType}
        /// 
        /// </summary>
        DefineTable,

        /// <summary>
        /// Defines a row to an existing table. This will either be a new row, or replacing an exiting one.
        /// 
        /// Payload: 
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

        /// <summary>
        /// Indicates that the requested metadata has finished.
        /// </summary>
        Finished,
    }
}