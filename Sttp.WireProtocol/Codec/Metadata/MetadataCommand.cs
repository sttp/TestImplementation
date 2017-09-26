namespace Sttp.WireProtocol
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum MetadataCommand
    {
        /// <summary>
        /// Clears all metadata for a user.
        /// </summary>
        Clear,

        /// <summary>
        /// Adds or replaces a table.
        /// 
        /// Payload: 
        /// Guid instanceID, 
        /// long transactionID, 
        /// string tableName, 
        /// int tableIndex, 
        /// bool isMappedToDataPoint
        /// </summary>
        AddTable,

        /// <summary>
        /// Deletes a table.
        /// 
        /// Payload: 
        /// int tableIndex
        /// </summary>
        DeleteTable,

        /// <summary>
        /// Updates the transaction version of the table.
        /// 
        /// Payload: 
        /// int tableIndex, 
        /// long transactionID
        /// 
        /// </summary>
        UpdateTable,

        /// <summary>
        /// Adds or replaces a column.
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
        /// Removes a column.
        /// 
        /// Payload: 
        /// int tableIndex, 
        /// int columnIndex
        /// 
        /// </summary>
        DeleteColumn,

        /// <summary>
        /// Adds or updates a value.
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
        /// Gets all of the tables with their columns
        /// 
        /// Payload:
        /// None
        /// </summary>
        SelectAllTablesWithSchema,

        /// <summary>
        /// Requests that the specified table is resynchronized. In other words, send 
        /// only the changes if the specified transactions are still stored on the server
        /// otherwise, resend the entire server.
        /// 
        /// cacheInstanceID should be null if the local data is not cached.
        /// 
        /// Payload: 
        /// int tableIndex;
        /// Guid cacheInstanceId
        /// long transactionID
        /// </summary>
        ResyncTable,



    }
}