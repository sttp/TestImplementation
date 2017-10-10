namespace Sttp.WireProtocol
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum MetadataCommand : byte
    {
        #region [ Response Publisher to Subscriber ]

        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Clears the existing database.
        /// </summary>
        Clear,

        /// <summary>
        /// Adds a table.
        /// 
        /// Payload: 
        /// int tableIndex,
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        AddTable,

        /// <summary>
        /// Adds a column.
        /// 
        /// Payload: 
        /// int tableIndex,
        /// int columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        AddColumn,

        /// <summary>
        /// Adds a row to an existing table.
        /// 
        /// Payload: 
        /// int tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        AddRow,

        /// <summary>
        /// Adds or updates a value. Deleting a value would be to assign it with null.
        /// 
        /// Payload: 
        /// int tableIndex,
        /// int columnIndex, 
        /// int rowIndex, 
        /// byte[] value
        /// 
        /// </summary>
        AddValue,

        /// <summary>
        /// Removes an entire row of data.
        /// 
        /// Payload: 
        /// int tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        DeleteRow,

        /// <summary>
        /// Indicates what the current version of the database is.
        /// 
        /// Payload:
        /// Guid majorVersion, 
        /// long minorVersion, 
        /// </summary>
        DatabaseVersion,

        #endregion

        #region [ Request Subscriber to Publisher ]

        //ToDo: Add this in later.
        /// <summary>
        /// Gets a table from the database.
        ///  
        /// Payload: 
        /// int tableIndex
        /// int columnListCount
        /// int[] columnIndexes
        /// int filterExpressions
        /// ForEach {
        ///     int ColumnIndex,
        ///     string Expression
        /// }
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/>
        /// <see cref="AddColumn"/>
        /// <see cref="AddValue"/>
        /// 
        /// </summary>
        GetTable,

        /// <summary>
        /// Gets a custom query from the database.
        ///  
        /// Payload: 
        /// int columnListCount
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        /// }
        /// int joinFieldsCount
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        ///     int ForeignTableIndex
        /// }
        /// int filterExpressions
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        ///     string Expression
        /// }
        /// 
        /// </summary>
        GetQuery,

        /// <summary>
        /// Requests the changes to the database since the specified version.
        /// 
        /// MajorVersion == Guid.Empty if the local database is empty.
        /// 
        /// Payload: 
        /// Guid majorVersion
        /// long minorVersion
        /// int columnListCount
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        /// }
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/> Note: Only if the table cannot be patched and a replacement is required.
        /// <see cref="AddColumn"/>
        /// <see cref="AddValue"/>
        /// <see cref="DeleteRow"/>
        /// <see cref="DatabaseVersion"/>
        /// 
        /// </summary>
        SyncDatabase,

        /// <summary>
        /// Requests all changes to the database for the specified columns. If any of the critical columns
        /// have changes, this request must be denied and the user will have to reacquire the table/query
        /// 
        /// MajorVersion == Guid.Empty if the local database is empty.
        /// 
        /// Payload: 
        /// Guid majorVersion
        /// long minorVersion
        /// int columnListCount
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        /// }
        /// int criticalColumns  //These are columns that exist as filter expressions or join columns.
        /// ForEach {
        ///     int TableIndex,
        ///     int ColumnIndex,
        /// }
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/> Note: Only if the table cannot be patched and a replacement is required.
        /// <see cref="AddColumn"/>
        /// <see cref="AddValue"/>
        /// <see cref="DeleteRow"/>
        /// <see cref="DatabaseVersion"/>
        /// 
        /// </summary>
        SyncTableOrQuery,

        /// <summary>
        /// Gets the schema for the database, This is all tables and columns
        /// 
        /// Payload:
        /// None
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/>
        /// <see cref="AddColumn"/>
        /// 
        /// 
        /// </summary>
        GetDatabaseSchema,

        /// <summary>
        /// Gets the current version of the database.
        /// 
        /// Payload:
        /// None
        /// </summary>
        GetDatabaseVersion,

        #endregion


    }
}