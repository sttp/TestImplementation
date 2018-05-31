using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    /// <summary>
    /// A command to request the schema of the metadata. 
    /// <see cref="LastKnownRuntimeID"/> and <see cref="LastKnownVersionNumber"/> can be optionally
    /// specified so the entire metadata object is not sent unless it has changed.
    /// 
    /// Responds with <see cref="CommandMetadataSchema"/> if the RuntimeIDs do no match.
    /// Responds with <see cref="CommandMetadataSchemaUpdate"/> if the version numbers do not match.
    /// Responds with <see cref="CommandMetadataSchemaVersion"/> if there are no changes in the metadata.
    /// </summary>
    [CtpSerializable("GetMetadataSchema")]
    public class CommandGetMetadataSchema
    {
        [CtpSerializeField()]
        public Guid? LastKnownRuntimeID { get; private set; }
        [CtpSerializeField()]
        public long? LastKnownVersionNumber { get; private set; }

        //Exists to support CtpSerializable
        private CommandGetMetadataSchema() { }

        public CommandGetMetadataSchema(Guid? lastKnownRuntimeID, long? lastKnownVersionNumber)
        {
            LastKnownRuntimeID = lastKnownRuntimeID;
            LastKnownVersionNumber = lastKnownVersionNumber;
        }
    }
}