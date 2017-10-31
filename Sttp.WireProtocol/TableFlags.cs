namespace Sttp.WireProtocol
{
    /// <summary>
    /// Flags associated with Metadata Tables
    /// </summary>
    public enum TableFlags : byte
    {
        None,
        /// <summary>
        /// Indicates that the RowID equals the RuntimeID of a DataPoint.
        /// </summary>
        MappedToDataPoint,
        /// <summary>
        /// Establishes a many-to-many relationship from one table to another. 
        /// </summary>
        LinkingTable,

    }
}
