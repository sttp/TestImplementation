namespace Sttp.WireProtocol
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum MetadataCommand
    {
        #region [ Notification Publisher to Subscriber ]

        /// <summary>
        /// Indicates that the metadata for a table has recently changed.
        /// It's up to the client to resync the metadata.
        /// 
        /// Payload:
        /// int tableIndex
        /// Guid instanceID, 
        /// long transactionID, 
        /// </summary>
        MetadataChanged,

        #endregion

        #region [ Response Publisher to Subscriber]

        /// <summary>
        /// Changes the active table
        /// 
        /// Payload:
        /// int tableIndex
        /// </summary>
        UseTable,

        /// <summary>
        /// Adds or replaces a table.
        /// 
        /// Payload: 
        /// Guid instanceID, 
        /// long transactionID, 
        /// string tableName, 
        /// bool isMappedToDataPoint
        /// </summary>
        AddTable,

        /// <summary>
        /// Updates the transaction version of the table.
        /// 
        /// Payload: 
        /// long transactionID
        /// 
        /// </summary>
        UpdateTable,

        /// <summary>
        /// Adds or replaces a column.
        /// 
        /// Payload: 
        /// int columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        AddColumn,

        /// <summary>
        /// Adds or updates a value.
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

        #endregion

        #region [Data Request, Subscriber to Publisher]

        /// <summary>
        /// Requests metadata from the active table.
        ///  
        /// Payload: 
        /// 
        /// int tableIndex
        /// int columnListCount
        /// int[] columnIndexes
        /// int filterExpressions
        /// string[] filterExpressionStrings
        /// </summary>
        GetTable,

        /// <summary>
        /// Requests that the active table is resynchronized with the local copy.
        /// 
        /// cacheInstanceID should be null the first time a request happens
        /// 
        /// Payload: 
        /// int tableIndex
        /// Guid cacheInstanceId
        /// long transactionID
        /// </summary>
        SyncTable,

        /// <summary>
        /// Gets all of the tables with their columns
        /// 
        /// Payload:
        /// None
        /// </summary>
        SelectAllTablesWithSchema,

        #endregion


    }
}