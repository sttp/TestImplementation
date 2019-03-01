using System;
using System.IO;
using GSF;

namespace CTP
{
    internal enum PacketContents : byte
    {
        CommandSchema = 0,
        CommandData = 1,
        CommandSchemaWithData = 2,
        CompressedDeflate = 3,
        CompressedZlib = 4,
    }
}