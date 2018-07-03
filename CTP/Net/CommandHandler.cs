using System.Collections.Generic;

namespace CTP.Net
{
    public class CommandHandler
    {
        private Dictionary<string, ICtpCommandHandler> m_rootCommands = new Dictionary<string, ICtpCommandHandler>();

        public void RegisterCommandHandler(ICtpCommandHandler handler)
        {
            foreach (var command in handler.SupportedCommands)
            {
                m_rootCommands.Add(command, handler);
            }
        }
        public void UnRegisterCommandHandler(ICtpCommandHandler handler)
        {
            foreach (var command in handler.SupportedCommands)
            {
                m_rootCommands.Remove(command);
            }
        }

        public bool TryHandle(CtpSession session, CtpDocument command)
        {
            if (m_rootCommands.TryGetValue(command.RootElement, out var handler))
            {
                handler.ProcessCommand(session, command);
                return true;
            }
            return false;
        }

    }
}