namespace Sttp.Codec.Metadata
{
    public class CmdUndefineRow 
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.UndefineRow;
        public SttpValue PrimaryKey;

        public void Load(PayloadReader reader)
        {
            PrimaryKey = reader.Read<SttpValue>();
        }

    }
}