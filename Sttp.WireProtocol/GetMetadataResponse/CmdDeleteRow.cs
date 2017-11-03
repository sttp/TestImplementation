namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDeleteRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.DeleteRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

    }
}