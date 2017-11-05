namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdJoin : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Join;
        public short TableIndex;
        public short ColumnIndex;
        public short ForeignTableIndex;
        public bool  IsLeftJoin;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ForeignTableIndex = reader.ReadInt16();
            IsLeftJoin = reader.ReadBoolean();
        }
    }
}