using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IO
{
    internal static class PacketMethods
    {
        internal static byte[] CreatePacket(PacketContents contentType, int contentFlags, byte[] payload)
        {
            var wr = new CtpObjectWriter();
            wr.Write((byte)contentType);
            wr.Write(contentFlags);
            wr.Write(payload);
            return wr.ToArray();
        }

        internal static byte[] CreatePacket(PacketContents contentType, int contentFlags, CtpObjectWriter payload)
        {
            var wr = new CtpObjectWriter();
            wr.Write((byte)contentType);
            wr.Write(contentFlags);
            payload.CopyTo(wr);
            return wr.ToArray();
        }
    }
}
