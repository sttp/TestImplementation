namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereInValue : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInValue;
        public short TableIndex;
        public short ColumnIndex;
        public SttpValue[] Items;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            Items = reader.ReadArray<SttpValue>();
        }
    }
}