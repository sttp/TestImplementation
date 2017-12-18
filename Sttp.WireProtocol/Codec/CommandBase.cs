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

        public abstract void Save(SttpMarkupWriter writer);

        private static ConcurrentDictionary<string, Func<SttpMarkupReader, CommandBase>> m_commands;

        static CommandBase()
        {
            m_commands = new ConcurrentDictionary<string, Func<SttpMarkupReader, CommandBase>>();
        }

        public static CommandBase Create(string commandName, SttpMarkup reader)
        {
            if (!m_commands.TryGetValue(commandName, out Func<SttpMarkupReader, CommandBase> command))
                throw new Exception("Command type has not been registered. " + commandName);
            return command(reader.MakeReader());
        }

        public static void Register(string commandName, Func<SttpMarkupReader, CommandBase> constructor)
        {
            m_commands[commandName] = constructor;
        }

    }
}
