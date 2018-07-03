using System.Collections.Generic;

namespace CTP.Net
{
    public class CommandHandler
    {
        private Dictionary<string, ICtpCommandHandlerBase> m_rootCommands = new Dictionary<string, ICtpCommandHandlerBase>();

        private ICtpCommandHandlerBase m_activeCommand;

        public void RegisterCommandHandler(ICtpCommandHandlerBase handler)
        {
            foreach (var command in handler.SupportedRootCommands)
            {
                m_rootCommands.Add(command, handler);
            }
        }

        public bool TryHandle(CtpSession session, CtpDocument command)
        {
            if (m_activeCommand != null)
            {
                m_activeCommand = m_activeCommand.ProcessCommand(session, command);
                return true;
            }
            if (m_rootCommands.TryGetValue(command.RootElement, out var handler))
            {
                m_activeCommand = handler.ProcessCommand(session, command);
                return true;
            }
            return false;
        }

    }
}