namespace Sttp.WireProtocol
{
    /// <summary>
    /// Considering an option for allowing custom derived types to be send on the wire.
    /// </summary>
    public interface ISttpUdfType
    {
        byte[] Save();
        void Load(byte[] data);
    }
}