using System;
using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdDefineRow
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineRow;
        public SttpValue PrimaryKey;
        public List<SttpValue> Values = new List<SttpValue>();

        public void Load(SttpMarkupReader reader)
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != "DefineRow")
                throw new Exception("Invalid command");

            PrimaryKey = element.GetValue("PrimaryKey");

            foreach (var query in element.GetElement("Fields").ForEachValue("Field"))
            {
                Values.Add(query);
            }
            element.ErrorIfNotHandled();
        }


    }
}