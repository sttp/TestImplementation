using System;

namespace Sttp.WireProtocol
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        public string TableName;
        /// <summary>
        /// This will change if the implementation does not support versioning metadata.
        /// </summary>
        public Guid BaseVersionID;
        /// <summary>
        /// This is the version number of the last time this individual table was modified.
        /// </summary>
        public int LastVersionNumber;
        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public MetadataColumn[] Columns;
    }
}
