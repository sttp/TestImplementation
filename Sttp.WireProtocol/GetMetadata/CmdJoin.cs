namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdJoin : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Join;
        public short TableIndex;
        public short ColumnIndex;
        public short ForeignTableIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ForeignTableIndex = reader.ReadInt16();
        }
    }
}