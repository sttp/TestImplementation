using System;

namespace Sttp.WireProtocol
{
    public class MetadataPatchDetails
    {
        public long Revision;
        public MetadataChangeType ChangeType;
        public byte[] ChangeDetails;

        public MetadataPatchDetails(long revision, MetadataChangeType changeType, byte[] changeDetails)
        {
            Revision = revision;
            ChangeType = changeType;
            ChangeDetails = changeDetails;
        }

        public static MetadataPatchDetails FromFillSchema(long revision, string tableName, string columnName, ValueType columnType)
        {
            return null;
        }

        public static MetadataPatchDetails FromFillData(long revisionSequenceNumber, string tableName, string columnName, int recordID, object fieldValue)
        {
            throw new NotImplementedException();
        }

        public static MetadataPatchDetails AddTable(long transactionID, string tableName, string columnName, ValueType columnType)
        {
            throw new NotImplementedException();
        }
    }
}