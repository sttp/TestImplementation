namespace Sttp.WireProtocol.Data
{
    public class MetadataWhereOperatorParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.WhereOperator;
        public MetadataOperatorMethod OperatorCode;

        public void Load(PacketReader reader)
        {
            OperatorCode = reader.Read<MetadataOperatorMethod>();
        }

    }
}