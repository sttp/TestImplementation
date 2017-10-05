namespace Sttp.WireProtocol.Data
{
    public interface IMetadataDecoder : IPacketDecoder
    {

        void Fill(StreamReader buffer);

        /// <summary>
        /// Gets the next command. Null if the end of the command string has occurred.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Note: IMPORTANT. The object returned here is a reusable object and should be dereferenced 
        /// the next time this method is called.
        /// </remarks>
        IMetadataParams NextCommand();

        #region [ Response Publisher to Subscriber ]

        //void UseTable(out int tableIndex);
        //void AddTable(out Guid majorVersion, out long minorVersion, out string tableName, out TableFlags tableFlags);
        //void AddColumn(out int columnIndex, out string columnName, out ValueType columnType);
        //void AddValue(out int columnIndex, out int rowIndex, out byte[] value);
        //void DeleteRow(out int rowIndex);
        //void TableVersion(out int tableIndex, out Guid majorVersion, out long minorVersion);
        //void AddRelationship(out int tableIndex, out int columnIndex, out int foreignTableIndex);

        #endregion

        #region [ Request Subscriber to Publisher ]

        //void GetTable(out int tableIndex, out int[] columnList, out string[] filterExpression);
        //void SyncTable(out int tableIndex, out Guid majorVersion, out long minorVersion, out int[] columnList);
        //void SelectAllTablesWithSchema();
        //void GetAllTableVersions();

        #endregion
    }
}