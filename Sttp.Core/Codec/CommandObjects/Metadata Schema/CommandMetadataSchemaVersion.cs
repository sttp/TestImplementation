using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandMetadataSchemaVersion : DocumentCommandBase
    {
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }

        private CommandMetadataSchemaVersion()
            : base("MetadataSchemaVersion")
        {

        }

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