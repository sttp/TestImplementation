namespace Sttp.WireProtocol.SyncMetadata
{
    public class CmdSelect : ICmd
    {
        public SubCommand SubCommand => SyncMetadata.SubCommand.Select;
        public short TableIndex;
        public short ColumnIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
        }

    }
}