using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sttp;

namespace SampleCode.Encoding_Layer
{
    //------------------------
    //
    //  Provides a means to serialize a tabular data set.
    //
    //------------------------

    public class TabularMetadataSchema
    {
        public int SchemeRuntimeID;
        public string TableName;
        public string[] ColumnNames;
        public ValueTypeCode[] ColumnTypes;
    }

    public class TabularMetadataTable
    {
        public int TableRuntimeID;
        public string TableName;
        public int MetadataSchemaID;
    }

    public class TabularMetadataRecord
    {
        public int TableRuntimeID;
        public int RowNumber;
        public byte[][] Fields;
    }

}
