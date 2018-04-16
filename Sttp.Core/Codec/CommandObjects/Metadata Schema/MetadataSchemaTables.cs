using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTable
    {
        public string TableName;
        public long LastModifiedVersionNumber;
        public List<MetadataColumn> Columns = new List<MetadataColumn>();

        public MetadataSchemaTable()
        {

        }

        public MetadataSchemaTable(CtpMarkupElement element)
        {
            TableName = (string)element.GetValue("TableName");
            LastModifiedVersionNumber = (long)element.GetValue("LastModifiedVersionNumber");

            foreach (var query in element.GetElement("Columns").ChildElements)
            {
                Columns.Add(new MetadataColumn(query));
            }
            element.ErrorIfNotHandled();
        }

        public void Save(CtpMarkupWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedVersionNumber", LastModifiedVersionNumber);
            using (sml.StartElement("Columns"))
            {
                foreach (var item in Columns)
                {
                    using (sml.StartElement("Column"))
                    {
                        item.Save(sml);
                    }
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