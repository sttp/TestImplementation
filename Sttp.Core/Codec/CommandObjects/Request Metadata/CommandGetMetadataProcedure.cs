using System;
using System.Collections.Generic;
using System.Text;
using CTP;

namespace Sttp.Codec
{
    public class MetadataProcedureParameters
    {
        public readonly string Name;
        public readonly CtpValue Value;
        public MetadataProcedureParameters(CtpDocumentElement documentElement)
        {
            Name = (string)documentElement.GetValue("Name");
            Value = documentElement.GetValue("Value");
            documentElement.ErrorIfNotHandled();
        }

        public MetadataProcedureParameters(string name, CtpValue value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}: ({Value})";
        }

        public void Save(CtpDocumentWriter sml)
        {
            sml.WriteValue("Name", Name);
            sml.WriteValue("Value", Value);
        }
    }
    public class CommandGetMetadataProcedure : CommandBase
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
            foreach (var e in element.GetElement("Parameters").ChildElements)
            {
                Parameters.Add(new MetadataProcedureParameters(e));
            }

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("Name", Name);

            using (writer.StartElement("Parameters"))
            {
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
}