using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpBulkTransport
    {
        public readonly SttpValueTypeCode FundamentalType;
        public readonly Guid BulkTransportID;
        public readonly long Length;

        public SttpBulkTransport(SttpValueTypeCode fundamentalType, Guid bulkTransportID, long length)
        {
            FundamentalType = fundamentalType;
            BulkTransportID = bulkTransportID;
            Length = length;
        }

        public SttpBulkTransport(ByteReader reader)
        {
            FundamentalType = (SttpValueTypeCode)reader.ReadByte();
            BulkTransportID = reader.ReadGuid();
            Length = reader.ReadInt64();
        }

        public void Write(ByteWriter writer)
        {
            writer.Write((byte)FundamentalType);
            writer.Write(BulkTransportID);
            writer.Write(Length);
        }

        public static bool operator ==(SttpBulkTransport a, SttpBulkTransport b)
        {
            return a.FundamentalType == b.FundamentalType && a.BulkTransportID == b.BulkTransportID && a.Length == b.Length;
        }

        public static bool operator !=(SttpBulkTransport a, SttpBulkTransport b)
        {
            return !(a == b);
        }
    }
}
