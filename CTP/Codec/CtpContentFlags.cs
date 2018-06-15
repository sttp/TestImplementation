using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    /// <summary>
    /// Indicates the content type of the protocol.
    /// </summary>
    [Flags]
    public enum CtpContentFlags : byte
    {
        /// <summary>
        /// Indicates that this message is the first of a new request. 
        /// In response, the remote end will open a new handler for this request. 
        /// If one already exists, the remote end will terminate the existing 
        /// handler and open another one.
        /// 
        /// The initial request must be a Document command.
        /// </summary>
        InitialRequest = 1 << 0,

        /// <summary>
        /// Indicates that this requests is to be closed. This can be sent by either end. 
        /// </summary>
        CloseRequest = 1 << 1,

        /// <summary>
        /// Indicates that the payload is a CtpDocument type.
        /// </summary>
        IsDocument = 1 << 2,

        /// <summary>
        /// Marks this command as optional. If the receiver gets this command,
        /// but cannot interpret it, it can be simply ignored.
        /// </summary>
        IsOptional = 1 << 4,

        None = 0,
    }
}
