namespace Sttp.WireProtocol.SyncMetadataResponse
{
    public class CmdDefineValue : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineValue;
        public short TableIndex;
        public short ColumnIndex;
        public int RowIndex;
        public SttpValue Value;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
            Value = reader.Read<SttpValue>();
        }
        
    }
}