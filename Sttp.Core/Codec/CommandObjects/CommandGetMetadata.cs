using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadata : CommandBase
    {
        public Guid RequestID;
        public Guid SchemaVersion;
        public long Revision;
        public bool AreUpdateQueries;
        public List<SttpQueryBase> Queries;

        public CommandGetMetadata(Guid requestID, Guid schemaVersion, long revision, bool areUpdateQueries, List<SttpQueryBase> queries)
            : base("GetMetadata")
        {
            RequestID = requestID;
            SchemaVersion = schemaVersion;
            Revision = revision;
            AreUpdateQueries = areUpdateQueries;
            Queries = new List<SttpQueryBase>(queries);
        }

        public CommandGetMetadata(SttpMarkupReader reader)
            : base("GetMetadata")
        {
            var element = reader.ReadEntireElement();

            RequestID = (Guid)element.GetValue("RequestID");
            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            Revision = (long)element.GetValue("Revision");
            AreUpdateQueries = (bool)element.GetValue("AreUpdateQueries");
            Queries = new List<SttpQueryBase>();

            foreach (var query in element.GetElement("Queries").ChildElements)
            {
                Queries.Add(SttpQueryBase.Create(query));
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RequestID", RequestID);
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("Revision", Revision);
            writer.WriteValue("AreUpdateQueries", AreUpdateQueries);
            using (writer.StartElement("Queries"))
            {
                foreach (var q in Queries)
                {
                    using (writer.StartElement("Query"))
                    {
                        q.Save(writer);
                    }
                }
            }

        }
    }
}