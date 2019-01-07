using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("GetMetadataProcedure")]
    public class CommandGetMetadataProcedure
        : CommandObject<CommandGetMetadataProcedure>
    {
        [CommandField()]
        public string Name;

        [CommandField()]
        public List<MetadataProcedureParameters> Parameters;

        public CommandGetMetadataProcedure(string name, List<MetadataProcedureParameters> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        //Exists to support CtpSerializable
        private CommandGetMetadataProcedure()
        { }

        public static explicit operator CommandGetMetadataProcedure(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}