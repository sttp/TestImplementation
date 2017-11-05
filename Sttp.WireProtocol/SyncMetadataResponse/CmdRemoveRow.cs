namespace Sttp.WireProtocol.SyncMetadataResponse
{
    public class CmdRemoveRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.RemoveRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

    }
}