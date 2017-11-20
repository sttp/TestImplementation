namespace Sttp.WireProtocol.Metadata
{
    public class CmdDefineRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineRow;
        public SttpValue PrimaryKey;
        public SttpValueSet Values;

        public void Load(PayloadReader reader)
        {
            PrimaryKey = reader.Read<SttpValue>();
            Values = reader.Read<SttpValueSet>();
        }


    }
}