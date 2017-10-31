namespace Sttp.WireProtocol.GetMetadataSchema
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
        /// Gets the schema for the database, This is all tables and columns
        /// 
        /// Payload:
        /// None
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
    }
}