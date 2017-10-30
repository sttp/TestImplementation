using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.MetadataPacket
{
    public enum MetadataSubRequest
    {

    }

    public class MetadataRequest
    {

    }

    public class MetadataRequestGetDatabaseSchema
    {

    }


    public class MetadataResponse
    {
        public MetadataSubRequest Request;

        public List<Tuple<int, string, TableFlags>> Tables;

        public void AddTable(int tableIndex, string tableNames, TableFlags flags)
        {

        }

        public void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType)
        {

        }

        public void AddRow(int tableIndex, int rowIndex)
        {

        }

        public void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
        {

        }

        public void DeleteRow(int tableIndex, int rowIndex)
        {
            
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            
        }
    }


}
