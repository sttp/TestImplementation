namespace Sttp.WireProtocol.Metadata
{
    public class CmdUndefineRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.UndefineRow;
        public SttpValue PrimaryKey;

        public void Load(PacketReader reader)
        {
            PrimaryKey = reader.Read<SttpValue>();
        }

    }
}