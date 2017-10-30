using System;

namespace Sttp.Data
{
    /// <summary>
    /// Identifies a filter on the metadata since not all users will be permitted to see all metadata.
    /// </summary>
    public class MetadataTableFilter
    {
        public bool Permit(MetadataChangeLogRecord record)
        {
            throw new NotImplementedException();
        }

        public bool PermitRow(MetadataRow row)
        {
            throw new NotImplementedException();
        }

        public bool PermitField(int rowRowIndex, short columnIndex, byte[] value)
        {
            throw new NotImplementedException();
        }
    }
}