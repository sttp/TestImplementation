using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("MetadataSchemaVersion")]
    public class CommandMetadataSchemaVersion
        : DocumentObject<CommandMetadataSchemaVersion>
    {
        [DocumentField()]
        public Guid RuntimeID { get; private set; }

        [DocumentField()]
        public long VersionNumber { get; private set; }

        //Exists to support CtpSerializable
        private CommandMetadataSchemaVersion()
        {

        }

        public CommandMetadataSchemaVersion(Guid runtimeID, long versionNumber)
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
        }

        public static explicit operator CommandMetadataSchemaVersion(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}