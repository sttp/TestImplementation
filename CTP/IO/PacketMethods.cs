using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Collection;

namespace CTP.IO
{
    internal static class PacketMethods
    {
        [ThreadStatic]
        private static CtpObjectWriter s_writer;

        internal static PooledBuffer CreatePacket(PacketContents contentType, int contentFlags, byte[] payload)
        {
            if (s_writer == null)
                s_writer = new CtpObjectWriter();
            var wr = s_writer;
            wr.Clear();
            wr.Write((byte)contentType);
            wr.Write(contentFlags);
            wr.Write(payload);
            return wr.TakeBuffer();
        }

        internal static PooledBuffer CreatePacket(PacketContents contentType, int contentFlags, CtpObjectWriter payload)
        {
            if (s_writer == null)
                s_writer = new CtpObjectWriter();
            var wr = s_writer;
            wr.Clear();
            wr.Write((byte)contentType);
            wr.Write(contentFlags);
            payload.CopyTo(wr);
            return wr.TakeBuffer();

        }
    }
}
