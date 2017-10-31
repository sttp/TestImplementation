namespace Sttp.WireProtocol.RegisterDataPoint
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }

        void Load(PacketReader reader);

        CmdNewPoint NewPoint { get; }
    }
}