using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public abstract class CommandBaseLoader
    {
        private static ConcurrentDictionary<KeyValuePair<string, CommandCode>, CommandBase> m_commands;

        static CommandBaseLoader()
        {
            m_commands = new ConcurrentDictionary<KeyValuePair<string, CommandCode>, CommandBase>();
        }

        public static CommandBase Create(CommandCode code, SttpMarkupReader element)
        {
            if (!m_commands.TryGetValue(new KeyValuePair<string, CommandCode>(element.ElementName, code), out CommandBase command))
                throw new Exception("Command type has not been registered. " + element.ElementName);
            return command.Load(element);
        }

        public static void Register(CommandBase command)
        {
            m_commands[new KeyValuePair<string, CommandCode>(command.CommandName, command.Code)] = command;
        }

    }
}