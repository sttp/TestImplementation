using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public class BufferValue
    {
        public byte[] Data;
    }

    public static class BufferValueExtensions
    {
        public static byte[] Encode(this BufferValue bufferValue)
        {
            if ((object)bufferValue == null)
                throw new ArgumentNullException(nameof(bufferValue));

            MemoryStream stream = new MemoryStream();

            int length = bufferValue.Data?.Length ?? 0;

            if (length > 15)
                throw new OverflowException("Buffer payload to large");

            stream.WriteByte((byte)length);

            if (length > 0)
                stream.Write(bufferValue.Data, 0, length);

            return stream.ToArray();
        }

        public static BufferValue DecodeBufferValue(this byte[] buffer, int startIndex, int length)
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
