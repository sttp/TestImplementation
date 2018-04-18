using System;
using CTP;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaVersion : DocumentCommandBase
    {
        public readonly Guid RuntimeID;
        public readonly long VersionNumber;

        public CommandMetadataSchemaVersion(Guid runtimeID, long versionNumber)
            : base("MetadataSchemaVersion")
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
        }

        public CommandMetadataSchemaVersion(CtpDocumentReader reader)
            : base("MetadataSchemaVersion")
        {
            var element = reader.ReadEntireElement();

            RuntimeID = (Guid)element.GetValue("RuntimeID");
            VersionNumber = (long)element.GetValue("VersionNumber");

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("RuntimeID", RuntimeID);
            writer.WriteValue("VersionNumber", VersionNumber);
        }
    }
}