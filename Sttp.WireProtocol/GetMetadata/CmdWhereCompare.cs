namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereCompare : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereCompare;
        public short TableIndex;
        public short ColumnIndex;
        public CompareMethod CompareMethod;
        public SttpValue Item;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            CompareMethod = reader.Read<CompareMethod>();
            Item = reader.Read<SttpValue>();
        }

    }
}