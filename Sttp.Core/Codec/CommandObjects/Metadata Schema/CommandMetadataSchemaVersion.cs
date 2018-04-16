using System;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaVersion : CommandBase
    {
        public readonly Guid RuntimeID;
        public readonly long VersionNumber;

        public CommandMetadataSchemaVersion(Guid runtimeID, long versionNumber)
            : base("MetadataSchemaVersion")
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
        }

        public CommandMetadataSchemaVersion(CtpMarkupReader reader)
            : base("MetadataSchemaVersion")
        {
            var element = reader.ReadEntireElement();

            RuntimeID = (Guid)element.GetValue("RuntimeID");
            VersionNumber = (long)element.GetValue("VersionNumber");

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
            writer.WriteValue("RuntimeID", RuntimeID);
            writer.WriteValue("VersionNumber", VersionNumber);
        }
    }
}