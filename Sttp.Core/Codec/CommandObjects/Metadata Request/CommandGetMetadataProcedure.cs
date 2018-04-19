using System;
using System.Collections.Generic;
using System.Text;
using CTP;

namespace Sttp.Codec
{
    public class CommandGetMetadataProcedure : DocumentCommandBase
    {
        public string Name;
        public List<MetadataProcedureParameters> Parameters;

        public CommandGetMetadataProcedure(string name, List<MetadataProcedureParameters> parameters)
            : base("GetMetadataProcedure")
        {
            Name = name;
            Parameters = parameters;
        }

        public CommandGetMetadataProcedure(CtpDocumentReader reader)
            : base("GetMetadataProcedure")
        {
            var element = reader.ReadEntireElement();

            Name = (string)element.GetValue("Name");
            Parameters = new List<MetadataProcedureParameters>();
            foreach (var e in element.ForEachElement("Parameters"))
            {
                Parameters.Add(new MetadataProcedureParameters(e));
            }

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("Name", Name);

            foreach (var c in Parameters)
            {
                using (writer.StartElement("Parameter"))
                {
                    c.Save(writer);
                }
            }
        }
    }
}