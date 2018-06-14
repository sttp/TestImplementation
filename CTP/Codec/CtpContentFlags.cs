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
        /// Indicates that the channel should be reset to the root if it's not already.
        /// </summary>
        ResetChannel = 1 << 7,

        /// <summary>
        /// Indicates that the payload is a CtpDocument type.
        /// </summary>
        IsDocument = 1 << 6,

        /// <summary>
        /// Indicates that the client initiated this request.
        /// </summary>
        IsClientRequest = 1 << 5,

        /// <summary>
        /// Indicates that this request was initiated by the server.
        /// </summary>
        IsServerRequest = 1 << 4,

        /// <summary>
        /// Indicates that this message is an exception 
        /// </summary>
        IsError = 1 << 3,

        /// <summary>
        /// Indicates that this message is an optional command.
        /// </summary>
        IsOptional = 1 << 2,

        None = 0,
    }
}
