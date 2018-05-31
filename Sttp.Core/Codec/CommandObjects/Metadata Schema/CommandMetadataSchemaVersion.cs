using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable("MetadataSchemaVersion")]
    public class CommandMetadataSchemaVersion
    {
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }

        //Exists to support CtpSerializable
        private CommandMetadataSchemaVersion() { }

        public CommandMetadataSchemaVersion(Guid runtimeID, long versionNumber)
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
        }
    }
}