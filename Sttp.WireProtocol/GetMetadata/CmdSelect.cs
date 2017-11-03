namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdSelect : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Select;
        public short TableIndex;
        public short ColumnIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
        }

    }
}