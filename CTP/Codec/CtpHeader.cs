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
        /// Indicates the kind of payload. 
        /// Examples include: 
        /// Payload 0 is CtpDocument Commands. 
        /// Payload 1 is a measurement stream
        /// Payload 2 is a metadata stream
        /// Payload 3 is an authentication stream, 
        /// </summary>
        PayloadKind = (1 << 6) - 1,

        /// <summary>
        /// None of the flags are set. Note, this state is valid.
        /// </summary>
        None = 0
    }
}