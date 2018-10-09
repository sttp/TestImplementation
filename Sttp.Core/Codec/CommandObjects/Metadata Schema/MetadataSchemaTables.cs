using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTable
        : DocumentObject<MetadataSchemaTable>
    {
        [DocumentField()]
        public string TableName;
        [DocumentField()]
        public long LastModifiedVersionNumber;
        [DocumentField()]
        public List<MetadataColumn> Columns = new List<MetadataColumn>();

        public MetadataSchemaTable()
        {

        }

        public MetadataSchemaTable Clone(long lastModifiedVersionNumber)
        {
            if (LastModifiedVersionNumber == lastModifiedVersionNumber)
                return this;

            var item = (MetadataSchemaTable)MemberwiseClone();
            item.LastModifiedVersionNumber = LastModifiedVersionNumber;
            return item;
        }
       
    }

}