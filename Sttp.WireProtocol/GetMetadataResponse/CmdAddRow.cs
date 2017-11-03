namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdAddRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }


    }
}