namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdUndefineRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.UndefineRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

    }
}