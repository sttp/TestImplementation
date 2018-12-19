using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("MetadataSchemaVersion")]
    public class CommandMetadataSchemaVersion
        : CommandObject<CommandMetadataSchemaVersion>
    {
        [CommandField()]
        public Guid RuntimeID { get; private set; }

        [CommandField()]
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

        public static explicit operator CommandMetadataSchemaVersion(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }
}