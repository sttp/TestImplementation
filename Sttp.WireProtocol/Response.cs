using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public class Response
    {
        public ResponseCode ResponseCode;
        public CommandCode CommandCode;
        public byte[] Payload;
    }

    public static class ResponseExtensions
    {
        public static byte[] Encode(this Response response)
        {
            if ((object)response == null)
                throw new ArgumentNullException(nameof(response));

            MemoryStream stream = new MemoryStream();

            stream.WriteByte((byte)response.ResponseCode);
            stream.WriteByte((byte)response.CommandCode);

            int length = response.Payload?.Length ?? 0;

            if (length > ushort.MaxValue)
                throw new OverflowException("Response payload to large");

            stream.Write(BigEndian.GetBytes((ushort)length), 0, 2);

            if (length > 0)
                stream.Write(response.Payload, 0, length);

            return stream.ToArray();
        }

        public static Response DecodeResponse(this byte[] buffer, int startIndex, int length)
        {
            buffer.ValidateParameters(startIndex, length);

            // TODO: Do more buffer length validation!

            Response response = new Response
            {
                ResponseCode = (ResponseCode)buffer[startIndex],
                CommandCode = (CommandCode)buffer[startIndex + 1],
            };

            ushort payloadLength = BigEndian.ToUInt16(buffer, startIndex + 2);

            if (payloadLength > 0)
                response.Payload = buffer.BlockCopy(startIndex + 4, payloadLength);

            return response;
        }
    }
}
