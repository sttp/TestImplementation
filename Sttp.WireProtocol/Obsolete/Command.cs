using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public class Command : IEncode
    {
        public CommandCode CommandCode;
        public byte[] Payload;

        public byte[] Encode()
        {
            MemoryStream stream = new MemoryStream();

            stream.WriteByte((byte)CommandCode);

            int length = Payload?.Length ?? 0;

            if (length > ushort.MaxValue)
                throw new OverflowException("Command payload to large");

            stream.Write(BigEndian.GetBytes((ushort)length), 0, 2);

            if (length > 0)
                stream.Write(Payload, 0, length);

            return stream.ToArray();
        }

        public static Command Decode(byte[] buffer, int startIndex, int length)
        {
            buffer.ValidateParameters(startIndex, length);

            // TODO: Do more buffer length validation!

            Command command = new Command
            {
                CommandCode = (CommandCode)buffer[startIndex + 1]
            };

            ushort payloadLength = BigEndian.ToUInt16(buffer, startIndex + 2);

            if (payloadLength > 0)
                command.Payload = buffer.BlockCopy(startIndex + 4, payloadLength);

            return command;
        }
    }
}
