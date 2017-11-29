using System.Collections.Generic;

namespace Sttp.Data
{
    public class MetadataForeignKey
    {
        public readonly string ColumnName;
        public readonly string ForeignTableName;
        /// <summary>
        /// The index of the table. -1 if the table has not yet been found.
        /// </summary>
        public int TableIndex = -1;
        public int LocalColumnIndex = -1;

        public MetadataForeignKey(string columnName, string foreignTableName)
        {
            ColumnName = columnName;
            ForeignTableName = foreignTableName;
        }
    }
}