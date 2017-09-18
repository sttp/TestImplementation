namespace Sttp.WireProtocol
{
    public enum SetType
    {
        FullSet = 0,    // Data point keys represent a new full set of keys
        UpdatedSet = 1, // Data point keys represent keys to be added and removed
    }
    // sizeof(uint8), 1-byte
}
