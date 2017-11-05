namespace Sttp.WireProtocol.SyncMetadata
{
    public class CmdWhereInString : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInString;
        public short TableIndex;
        public short ColumnIndex;
        public string[] Items;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            Items = reader.ReadArray<string>();
        }
    }
}