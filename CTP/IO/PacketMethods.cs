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
            payload.CopyToAsCtpBuffer(wr);
            return wr.TakeBuffer();

        }

        internal static bool TryReadPacket(byte[] data, int position, int length, int maximumPacketSize, out PacketContents payloadType, out int payloadFlags, out byte[] payloadBuffer, out int consumedLength)
        {
            if (length > maximumPacketSize)
                throw new Exception("Command size is too large");

            var stream = new CtpObjectReader(data, position, length);
            CtpObject pType;
            CtpObject pFlags;
            CtpObject pData;
            if (stream.TryRead(out pType) && stream.TryRead(out pFlags) && stream.TryRead(out pData))
            {
                payloadType = (PacketContents)(byte)pType;
                payloadFlags = (int)pFlags;
                payloadBuffer = (byte[])pData;
                consumedLength = stream.Position;
                return true;
            }
            payloadType = default(PacketContents);
            payloadFlags = 0;
            payloadBuffer = null;
            consumedLength = 0;
            return false;
        }
    }
}
