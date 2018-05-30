using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;
using Sttp.Codec;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class MetadataSchemaTable
    {
        [CtpSerializeField()]
        public string TableName;
        [CtpSerializeField()]
        public long LastModifiedVersionNumber;
        [CtpSerializeField()]
        public List<MetadataColumn> Columns = new List<MetadataColumn>();

        public MetadataSchemaTable()
        {

        }

        public MetadataSchemaTable(CtpDocumentElement documentElement)
        {
            TableName = (string)documentElement.GetValue("TableName");
            LastModifiedVersionNumber = (long)documentElement.GetValue("LastModifiedVersionNumber");

            foreach (var query in documentElement.ForEachElement("Column"))
            {
                Columns.Add(new MetadataColumn(query));
            }
            documentElement.ErrorIfNotHandled();
        }

        public void Save(CtpDocumentWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedVersionNumber", LastModifiedVersionNumber);
            foreach (var item in Columns)
            {
                using (sml.StartElement("Column"))
                {
                    item.Save(sml);
                }
            }
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