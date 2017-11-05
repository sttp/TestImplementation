namespace Sttp.WireProtocol.GetMetadataSchemaResponse
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
        /// Defines a table.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        DefineTable,

        /// <summary>
        /// Defines a column.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// short columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        DefineColumn,
       
    }
}