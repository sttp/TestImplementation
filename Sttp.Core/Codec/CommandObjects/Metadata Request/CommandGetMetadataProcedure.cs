using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable("GetMetadataProcedure")]
    public class CommandGetMetadataProcedure
    {
        [CtpSerializeField()]
        public string Name;
        [CtpSerializeField()]
        public List<MetadataProcedureParameters> Parameters;

        public CommandGetMetadataProcedure(string name, List<MetadataProcedureParameters> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        //Exists to support CtpSerializable
        private CommandGetMetadataProcedure() { }
    }
}