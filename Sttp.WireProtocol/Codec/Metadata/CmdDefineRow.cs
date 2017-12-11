using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdDefineRow 
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineRow;
        public SttpValue PrimaryKey;
        public List<SttpValue> Values;

        public void Load(PayloadReader reader)
        {
            PrimaryKey = reader.ReadSttpValue();
            Values = reader.ReadListSttpValue();
        }


    }
}