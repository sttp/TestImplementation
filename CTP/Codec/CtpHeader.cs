using System;
using System.CodeDom;
using System.ComponentModel;

namespace CTP
{
    [Flags]
    internal enum CtpHeader : byte
    {
        /// <summary>
        /// The header that is used. Must be 0
        /// </summary>
        HeaderVersion = 1 << 7,

        /// <summary>
        /// Indicates if this packet is compressed. 
        /// 0: None
        /// 1: Yes
        /// </summary>
        IsCompressed = 1 << 6,

        /// <summary>
        /// Indicates that this message is a document payload message
        /// </summary>
        IsDocumentPayload = 1 << 5,

        /// <summary>
        /// Indicates that this message must interrupt the existing flow of the command.
        /// </summary>
        IsException = 1 << 4,

        /// <summary>
        /// Indicates if this channel is owned by the server or the client.
        /// 0=Client
        /// 1=Server
        /// </summary>
        ChannelOwner = 1 << 3,

        /// <summary>
        /// Indicates the content type.
        /// </summary>
        ChannelFlags = 1 << 2 | 1 << 1 | 1 << 0,

        /// <summary>
        /// None of the flags are set. Note, this state is valid.
        /// </summary>
        None = 0
    }

    internal static class CtpHeaderExtensions
    {
        public static CtpHeader SetChannelCode(this CtpHeader header, CtpContentFlags contentFlags)
        {
            if ((header & CtpHeader.HeaderVersion) != 0)
                throw new Exception("Header version not recognized");
            if ((byte)contentFlags > 4)
                throw new InvalidEnumArgumentException(nameof(contentFlags), (int)contentFlags, typeof(CtpContentFlags));
            return (header & ~CtpHeader.ChannelFlags) | (CtpHeader)(byte)contentFlags;
        }

        public static CtpContentFlags GetChannelCode(this CtpHeader header)
        {
            if ((header & CtpHeader.HeaderVersion) != 0)
                throw new Exception("Header version not recognized");
            return (CtpContentFlags)(byte)(header & CtpHeader.ChannelFlags);
        }
    }
}