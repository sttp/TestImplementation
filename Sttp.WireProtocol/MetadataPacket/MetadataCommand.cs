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
        /// Changes the active table
        /// 
        /// Payload:
        /// int tableIndex
        /// </summary>
        UseTable,

        /// <summary>
        /// Adds or Replaces a table if it already exists.
        /// 
        /// Payload: 
        /// Guid majorVersion, 
        /// long minorVersion, 
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        AddTable,

        /// <summary>
        /// Adds a column.
        /// 
        /// Payload: 
        /// int columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        AddColumn,

        /// <summary>
        /// Adds or updates a value. Deleting a value would be to assign it with null.
        /// 
        /// Payload: 
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
        /// int rowIndex,
        /// 
        /// </summary>
        DeleteRow,

        /// <summary>
        /// Indicates what the current version of a table is.
        /// 
        /// Payload:
        /// int tableIndex
        /// Guid majorVersion, 
        /// long minorVersion, 
        /// 
        /// This is in response to <see cref="GetAllTableVersions"/>
        /// </summary>
        TableVersion,

        /// <summary>
        /// Adds a table relationship. Sometimes known as a foreign key relationship.
        /// 
        /// Payload:
        /// int tableIndex
        /// int columnIndex,
        /// int foreignTableIndex
        /// 
        /// This is in response to <see cref="GetAllTableVersions"/>
        /// </summary>
        AddRelationship,

        #endregion

        #region [ Request Subscriber to Publisher ]

        /// <summary>
        /// Requests metadata from the specified table.
        ///  
        /// Payload: 
        /// 
        /// int tableIndex
        /// int columnListCount
        /// int[] columnIndexes
        /// int filterExpressions
        /// string[] filterExpressionStrings
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/>
        /// <see cref="UseTable"/>
        /// <see cref="AddColumn"/>
        /// <see cref="AddValue"/>
        /// 
        /// </summary>
        GetTable,

        /// <summary>
        /// Requests that the specified table is synchronized with the local copy.
        /// 
        /// MajorVersion == Guid.Empty if the local table is blank.
        /// 
        /// Payload: 
        /// int tableIndex
        /// Guid majorVersion
        /// long minorVersion
        /// int columnListCount
        /// int[] columnList
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/> Note: Only if the table cannot be patched and a replacement is required.
        /// <see cref="UseTable"/>
        /// <see cref="AddColumn"/>
        /// <see cref="AddValue"/>
        /// <see cref="DeleteRow"/>
        /// <see cref="TableVersion"/>
        /// 
        /// </summary>
        SyncTable,

        /// <summary>
        /// Gets all of the tables with their columns
        /// 
        /// Payload:
        /// None
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/>
        /// <see cref="UseTable"/>
        /// <see cref="AddColumn"/>
        /// 
        /// 
        /// </summary>
        SelectAllTablesWithSchema,

        /// <summary>
        /// Gets the version information for every table the user has access to.
        /// 
        /// Payload:
        /// None
        /// <see cref="AddTable"/>
        /// 
        /// Response is a series of these commands:
        /// <see cref="TableVersion"/>
        /// </summary>
        GetAllTableVersions,


        #endregion


    }
}