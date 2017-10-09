namespace Sttp.WireProtocol.Data
{
    public interface IMetadataDecoder : IPacketDecoder
    {
        void Fill(StreamReader buffer);

        /// <summary>
        /// Gets the next command. Null if the end of the command string has occurred.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Note: IMPORTANT. The object returned here is a reusable object and should be dereferenced 
        /// the next time this method is called.
        /// </remarks>
        IMetadataParams NextCommand();
    }
}