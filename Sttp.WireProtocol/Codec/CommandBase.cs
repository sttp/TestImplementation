using System;
using System.Collections.Concurrent;

namespace CTP.Codec
{
    /// <summary>
    /// This base class assists in serializing <see cref="CommandCode.MarkupCommand"/> into 
    /// concrete objects from their corresponding <see cref="CtpMarkup"/> data.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// The name of the command, this corresponds to the SttpMarkup's Root Element.
        /// </summary>
        public readonly string CommandName;

        protected CommandBase(string commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        /// Saves this command object to a <see cref="CtpMarkup"/>.
        /// </summary>
        /// <param name="writer">The writer to save the command to.</param>
        public abstract void Save(CtpMarkupWriter writer);


        public CtpMarkup ToSttpMarkup()
        {
            var wr = new CtpMarkupWriter(CommandName);
            Save(wr);
            return wr.ToSttpMarkup();
        }

        public override string ToString()
        {
            return ToSttpMarkup().ToYAML();
        }

        #region [ Static ]

        /// <summary>
        /// Contains all commands and their corresponding initializers.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Func<CtpMarkupReader, CommandBase>> CommandsInitializers;

        static CommandBase()
        {
            CommandsInitializers = new ConcurrentDictionary<string, Func<CtpMarkupReader, CommandBase>>();
        }

        /// <summary>
        /// Creates a command object from the supplied <see pref="reader"/>. If the command has not been registered, 
        /// an <see cref="CommandUnknown"/> object will be returned.
        /// </summary>
        /// <param name="reader">the serialized data to extract from this reader.</param>
        /// <returns></returns>
        public static CommandBase Create(CtpMarkup reader)
        {
            string rootElement = reader.MakeReader().RootElement;
            if (!CommandsInitializers.TryGetValue(rootElement, out Func<CtpMarkupReader, CommandBase> command))
            {
                return new CommandUnknown(rootElement, reader);
            }
            return command(reader.MakeReader());
        }

        /// <summary>
        /// Registers an initializer for a command. This will be used when receiving a command to attempt to turn it into an object.
        /// If a command is received that is not registered, a <see cref="CommandUnknown"/> will be created in its place.
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <param name="initializer">The initializer</param>
        public static void Register(string commandName, Func<CtpMarkupReader, CommandBase> initializer)
        {
            CommandsInitializers[commandName] = initializer;
        }

        #endregion

    }
}
