using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandMetadataSchema
    {
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }
        [CtpSerializeField()]
        public List<MetadataSchemaTable> Tables { get; private set; }

        public CommandMetadataSchema(Guid runtimeID, long versionNumber, List<MetadataSchemaTable> tables)
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
            Tables = new List<MetadataSchemaTable>(tables);
        }

        //Exists to support CtpSerializable
        private CommandMetadataSchema() { }

        public CommandMetadataSchema Combine(CommandMetadataSchemaUpdate update)
        {
            List<MetadataSchemaTable> newList = new List<MetadataSchemaTable>(Tables);

            //Only support patching if the schemas match and the sequence number is less than the update.
            if (RuntimeID == update.RuntimeID && VersionNumber < update.VersionNumber)
            {
                foreach (var item in update.Tables)
                {
                    int indx = Tables.FindIndex(x => x.TableName == item.TableName);
                    newList[indx] = newList[indx].Clone(item.LastModifiedVersionNumber);
                }
                return new CommandMetadataSchema(update.RuntimeID, update.VersionNumber, newList);
            }
            return this;
        }
    }
}