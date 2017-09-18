using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public class BufferValue : IEncode
    {
        public byte[] Data;

        public byte[] Encode()
        {
            MemoryStream stream = new MemoryStream();

            int length = Data?.Length ?? 0;

            if (length > 15)
                throw new OverflowException("Buffer payload to large");

            stream.WriteByte((byte)length);

            if (length > 0)
                stream.Write(Data, 0, length);

            return stream.ToArray();
        }

        public static BufferValue Decode(byte[] buffer, int startIndex, int length)
        {
            buffer.ValidateParameters(startIndex, length);

            // TODO: Do more buffer length validation!

            BufferValue bufferValue = new BufferValue();

            ushort payloadLength = BigEndian.ToUInt16(buffer, startIndex + 2);

            if (payloadLength > 0)
                bufferValue.Data = buffer.BlockCopy(startIndex + 4, payloadLength);

            return bufferValue;
        }
    }
}
