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
        /// None of the flags are set. Note, this state is valid.
        /// </summary>
        None = 0
    }
}