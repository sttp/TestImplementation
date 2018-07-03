using System.Collections.Generic;

namespace CTP.Net
{
    public interface ICtpCommandHandlerBase
    {
        /// <summary>
        /// Gets a list of all supported root commands. If no root commands are supported by this handler, return null or an empty set.
        /// </summary>
        IEnumerable<string> SupportedRootCommands { get; }

        ICtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command);
    }

    public interface ICtpDataChannelHandler
    {
        void ProcessData(CtpSession session, byte[] data);
    }

}