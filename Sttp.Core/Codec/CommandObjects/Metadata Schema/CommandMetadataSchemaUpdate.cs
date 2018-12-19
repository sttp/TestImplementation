using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("MetadataSchemaUpdate")]
    public class CommandMetadataSchemaUpdate
        : CommandObject<CommandMetadataSchemaUpdate>
    {
        [CommandField()]
        public Guid RuntimeID { get; private set; }
        [CommandField()]
        public long VersionNumber { get; private set; }
        [CommandField()]
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

        public static explicit operator CommandMetadataSchemaUpdate(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }
}