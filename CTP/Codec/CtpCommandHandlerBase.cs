using System.Collections.Generic;

namespace CTP.Net
{
    public abstract class CtpCommandHandlerBase
    {
        public List<string> SupportedCommands = new List<string>();
        public abstract CtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command);
        public abstract void Cancel();
    }
}