using System;
using System.Collections.Generic;
using CTP;

namespace Sttp.Codec
{
    public class CommandBeginMetadataResponse : DocumentCommandBase
    {
        public readonly int BinaryChannelCode;
        public readonly Guid EncodingMethod;
        public Guid RuntimeID;
        public long VersionNumber;
        public string TableName;
        public List<MetadataColumn> Columns;

        public CommandBeginMetadataResponse(int binaryChannelCode, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
            : base("BeginMetadataResponse")
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
            TableName = tableName;
            Columns = columns;
        }

        public CommandBeginMetadataResponse(CtpDocumentReader reader)
            : base("BeginMetadataResponse")
        {
            var element = reader.ReadEntireElement();

            BinaryChannelCode = (int)element.GetValue("BinaryChannelCode");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");
            RuntimeID = (Guid)element.GetValue("RuntimeID");
            VersionNumber = (long)element.GetValue("VersionNumber");
            TableName = (string)element.GetValue("TableName");
            Columns = new List<MetadataColumn>();
            foreach (var e in element.ForEachElement("Column"))
            {
                Columns.Add(new MetadataColumn(e));
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("BinaryChannelCode", BinaryChannelCode);
            writer.WriteValue("EncodingMethod", EncodingMethod);
            writer.WriteValue("RuntimeID", RuntimeID);
            writer.WriteValue("VersionNumber", VersionNumber);
            writer.WriteValue("TableName", TableName);
            foreach (var c in Columns)
            {
                using (writer.StartElement("Column"))
                {
                    c.Save(writer);
                }
            }
        }
    }
}
