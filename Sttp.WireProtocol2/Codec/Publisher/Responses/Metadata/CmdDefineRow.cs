namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDefineRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineRow;
        public SttpValue PrimaryKey;
        public SttpValueSet Values;

        public void Load(PacketReader reader)
        {
            PrimaryKey = reader.Read<SttpValue>();
            Values = reader.Read<SttpValueSet>();
        }


    }
}