namespace Sttp.WireProtocol.SyncMetadata
{
    public class CmdWhereOperator : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereOperator;
        public OperatorMethod OperatorCode;

        public void Load(PacketReader reader)
        {
            OperatorCode = reader.Read<OperatorMethod>();
        }
        
    }
}