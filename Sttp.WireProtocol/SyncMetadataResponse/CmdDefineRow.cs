namespace Sttp.WireProtocol.SyncMetadataResponse
{
    public class CmdDefineRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }


    }
}