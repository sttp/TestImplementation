using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol.RegisterDataPoint
{
    public class CmdNewPoint : ICmd
    {
        public SubCommand SubCommand => SubCommand.NewPoint;
        public uint DataPointID;
        public uint RuntimeID;
        public ValueType Type;
        public StateFlags Flags;

        public void Load(PacketReader reader)
        {
            DataPointID = reader.ReadUInt32();
            RuntimeID = reader.ReadUInt32();
            Type = reader.Read<ValueType>();
            Flags = reader.Read<StateFlags>();
        }

        CmdNewPoint ICmd.NewPoint => this;
    }
}