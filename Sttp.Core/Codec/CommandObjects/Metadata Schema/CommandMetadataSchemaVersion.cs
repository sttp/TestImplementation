using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("MetadataSchemaVersion")]
    public class CommandMetadataSchemaVersion
        : CtpDocumentObject<CommandMetadataSchemaVersion>
    {
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }

        //Exists to support CtpSerializable
        private CommandMetadataSchemaVersion()
        { }

        public CommandMetadataSchemaVersion(Guid runtimeID, long versionNumber)
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
        }

        public static explicit operator CommandMetadataSchemaVersion(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}