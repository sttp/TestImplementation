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
        MetadataClear,

        /// <summary>
        /// Adds or replaces a table.
        /// </summary>
        MetadataAddTable,

        /// <summary>
        /// Deletes a table.
        /// </summary>
        MetadataDeleteTable,

        /// <summary>
        /// Updates the transaction version of the table.
        /// </summary>
        MetadataUpdateTable,
        
        /// <summary>
        /// Adds or replaces a column.
        /// </summary>
        MetadataAddColumn,

        /// <summary>
        /// Removes a column.
        /// </summary>
        MetadataDeleteColumn,

        /// <summary>
        /// Adds or updates a value.
        /// </summary>
        MetadataAddValue,

        /// <summary>
        /// Removes an entire row of data.
        /// </summary>
        MetadataDeleteRow,

    }
}