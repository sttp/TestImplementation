using System;
using System.IO;
using GSF;

namespace CTP
{
    internal enum PacketContents : byte
    {
        CommandSchema,
        CommandData,
        CompressedDeflate,
        CompressedZlib,
    }
}