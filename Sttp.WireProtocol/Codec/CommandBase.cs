using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public abstract class CommandBase
    {
        public readonly string CommandName;

        protected CommandBase(string name)
        {
            CommandName = name;
        }

        public abstract CommandBase Load(SttpMarkupReader reader);

        public abstract void Save(SttpMarkupWriter writer);

        private static ConcurrentDictionary<string, CommandBase> m_commands;

        static CommandBase()
        {
            m_commands = new ConcurrentDictionary<string, CommandBase>();
        }

        public static CommandBase Create(string commandName, SttpMarkup reader)
        {
            if (!m_commands.TryGetValue(commandName, out CommandBase command))
                throw new Exception("Command type has not been registered. " + commandName);
            return command.Load(reader.MakeReader());
        }

        public static void Register(CommandBase command)
        {
            m_commands[command.CommandName] = command;
        }

    }
}
