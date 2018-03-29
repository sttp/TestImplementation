using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadataSchema : CommandBase
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

        public CommandGetMetadataSchema(SttpMarkupReader reader)
            : this()
        {
            var element = reader.ReadEntireElement();

            LastKnownRuntimeID = (Guid?)element.GetValue("LastKnownRuntimeID");
            LastKnownVersionNumber = (long?)element.GetValue("LastKnownVersionNumber");
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("LastKnownRuntimeID", LastKnownRuntimeID);
            writer.WriteValue("LastKnownVersionNumber", LastKnownVersionNumber);
        }
    }
}