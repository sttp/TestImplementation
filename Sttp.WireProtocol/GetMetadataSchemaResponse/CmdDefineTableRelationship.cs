namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdDefineTableRelationship : ICmd
    {
        public SubCommand SubCommand => GetMetadataSchemaResponse.SubCommand.DefineTableRelationship;
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