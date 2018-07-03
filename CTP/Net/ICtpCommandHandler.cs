using System.Collections.Generic;

namespace CTP.Net
{
    public interface ICtpCommandHandler
    {
        /// <summary>
        /// Gets a list of all supported root commands. If no root commands are supported by this handler, return null or an empty set.
        /// </summary>
        IEnumerable<string> SupportedCommands { get; }

        void ProcessCommand(CtpSession session, CtpDocument command);
    }
}