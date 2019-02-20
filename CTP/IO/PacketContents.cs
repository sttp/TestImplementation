using System;
using System.IO;
using GSF;

namespace CTP
{
    internal enum PacketContents
    {
        CommandSchema,
        CommandData,
        CompressedDeflate,
        CompressedZlib,
    }
}