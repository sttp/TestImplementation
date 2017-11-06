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
        /// The specified request failed. 
        /// 
        /// Payload: 
        /// string Reason      - A user friendly message for the failure, can be null.
        /// string Details     - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestFailed,

        /// <summary>
        /// Indicates the current version of the database.
        /// 
        /// Payload:
        /// Guid SchemaVersion, 
        /// long Revision, 
        /// </summary>
        DatabaseVersion,

        /// <summary>
        /// Defines a table.
        /// 
        /// Payload: 
        /// string tableName,
        /// TableFlags flags
        /// Array columns{string columnName, SttpValueTypeCode columnType}
        /// 
        /// </summary>
        DefineTable,

        /// <summary>
        /// Indicates that the specified column is a pointer to a record in another table.
        /// 
        /// Payload:
        /// string TableName         - The table that has the column with the foreign key.
        /// string ColumnName        - The name of the column with the foreign key.
        /// string ForeignTableName  - The foreign table that has the key. It could be itself of course.
        /// 
        /// </summary>
        DefineTableRelationship,
    }
}