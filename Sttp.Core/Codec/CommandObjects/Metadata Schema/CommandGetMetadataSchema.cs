using System;
using System.Collections.Generic;
using CTP;

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
    public class CommandGetMetadataSchema : DocumentCommandBase
    {
        public readonly Guid? LastKnownRuntimeID;
        public readonly long? LastKnownVersionNumber;

        public CommandGetMetadataSchema()
            : base("GetMetadataSchema")
        {

        }

        public CommandGetMetadataSchema(Guid? lastKnownRuntimeID, long? lastKnownVersionNumber)
            : this()
        {
            LastKnownRuntimeID = lastKnownRuntimeID;
            LastKnownVersionNumber = lastKnownVersionNumber;
        }

        public CommandGetMetadataSchema(CtpDocumentReader reader)
            : this()
        {
            var element = reader.ReadEntireElement();
            LastKnownRuntimeID = (Guid?)element.GetValue("LastKnownRuntimeID");
            LastKnownVersionNumber = (long?)element.GetValue("LastKnownVersionNumber");
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("LastKnownRuntimeID", LastKnownRuntimeID);
            writer.WriteValue("LastKnownVersionNumber", LastKnownVersionNumber);
        }
    }
}