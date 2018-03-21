using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandGetMetadataProcedure : CommandBase
    {
        public string ProcedureName;
        public SttpMarkup Options;

        public CommandGetMetadataProcedure(string procedureName, SttpMarkup options)
            : base("CommandGetMetadataProcedure")
        {
            ProcedureName = procedureName;
            Options = options;
        }

        public CommandGetMetadataProcedure(SttpMarkupReader reader)
            : base("CommandGetMetadataProcedure")
        {
            var element = reader.ReadEntireElement();

            ProcedureName = (string)element.GetValue("ProcedureName");
            Options = (SttpMarkup)element.GetValue("Options");
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ProcedureName", ProcedureName);
            writer.WriteValue("Options", Options);
        }
    }
}