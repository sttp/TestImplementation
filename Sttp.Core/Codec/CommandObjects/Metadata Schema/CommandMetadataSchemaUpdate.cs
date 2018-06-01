using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("MetadataSchemaUpdate")]
    public class CommandMetadataSchemaUpdate
        : CtpDocumentObject<CommandMetadataSchemaUpdate>
    {
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }
        [CtpSerializeField()]
        public List<MetadataSchemaTableUpdate> Tables { get; private set; }

        //Exists to support CtpSerializable
        private CommandMetadataSchemaUpdate()
        { }

        public CommandMetadataSchemaUpdate(Guid runtimeID, long versionNumber, List<MetadataSchemaTableUpdate> tables)
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
            Tables = new List<MetadataSchemaTableUpdate>(tables);
        }

        public static explicit operator CommandMetadataSchemaUpdate(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}