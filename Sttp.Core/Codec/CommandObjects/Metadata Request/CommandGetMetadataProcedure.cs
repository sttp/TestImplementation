using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("GetMetadataProcedure")]
    public class CommandGetMetadataProcedure
        : DocumentObject<CommandGetMetadataProcedure>
    {
        [DocumentField()]
        public string Name;

        [DocumentField()]
        public List<MetadataProcedureParameters> Parameters;

        public CommandGetMetadataProcedure(string name, List<MetadataProcedureParameters> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        //Exists to support CtpSerializable
        private CommandGetMetadataProcedure()
        { }

        public static explicit operator CommandGetMetadataProcedure(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}