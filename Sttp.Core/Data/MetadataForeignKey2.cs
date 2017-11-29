using System.Collections.Generic;
using Sttp.Codec;

namespace Sttp.Data
{
    public class MetadataForeignKey2
    {
        public readonly string ColumnName;
        public readonly string ForeignTableName;
        /// <summary>
        /// The index of the table. -1 if the table has not yet been found.
        /// </summary>
        public int TableIndex = -1;
        public int LocalColumnIndex = -1;

        public MetadataForeignKey2(MetadataForeignKey key)
        {
            ColumnName = key.ColumnName;
            ForeignTableName = key.ForeignTableName;
        }
        public MetadataForeignKey2(string columnName, string foreignTableName)
        {
            ColumnName = columnName;
            ForeignTableName = foreignTableName;
        }
    }
}