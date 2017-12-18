using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadata : CommandBase
    {
        public Guid? RequestID;
        public Guid? SchemaVersion;
        public long? Revision;
        public bool? AreUpdateQueries;
        public List<SttpQueryBase> Queries;

        public CommandGetMetadata(SttpMarkupReader reader)
            : base("GetMetadataSchema", CommandCode.GetMetadata)
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            RequestID = (Guid?)element.GetValue("RequestID");
            SchemaVersion = (Guid?)element.GetValue("SchemaVersion");
            Revision = (long?)element.GetValue("Revision");
            AreUpdateQueries = (bool?)element.GetValue("AreUpdateQueries");
            Queries = new List<SttpQueryBase>();

            foreach (var query in element.GetElement("Queries").ChildElements)
            {
                Queries.Add(SttpQueryBase.Create(query));
            }
            element.ErrorIfNotHandled();
        }

        public override CommandBase Load(SttpMarkupReader reader)
        {
            return new CommandGetMetadata(reader);
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("RequestID", RequestID);
                writer.WriteValue("SchemaVersion", SchemaVersion);
                writer.WriteValue("Revision", Revision);
                writer.WriteValue("AreUpdateQueries", AreUpdateQueries);
                using (writer.StartElement("Queries"))
                {
                    foreach (var q in Queries)
                    {
                        q.Save(writer);
                    }
                }

            }
        }
    }
}