using System;

namespace Sttp.WireProtocol.BulkTransport
{
    public class CmdBeginSend : ICmd
    {
        public SubCommand SubCommand => SubCommand.BeginSend;

        public Guid Id;
        public BulkTransportMode Mode;
        public BulkTransportCompression Compression;
        public long OriginalSize;
        public byte[] Content;


        public void Load(PacketReader reader)
        {
            Id = reader.ReadGuid();
            Mode = reader.Read<BulkTransportMode>();
            Compression = reader.Read<BulkTransportCompression>();
            OriginalSize = reader.ReadInt64();
            Content = reader.ReadBytes();
        }


    }
}