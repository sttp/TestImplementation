using System;
using System.Collections.Concurrent;

namespace CTP
{
    /// <summary>
    /// This base class assists in serializing <see cref="CommandCode.DocumentCommand"/> into 
    /// concrete objects from their corresponding <see cref="CtpDocument"/> data.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// The name of the command, this corresponds to the CtpDocument's Root Element.
        /// </summary>
        public readonly string CommandName;

        protected CommandBase(string commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        /// Saves this command object to a <see cref="CtpDocument"/>.
        /// </summary>
        /// <param name="writer">The writer to save the command to.</param>
        public abstract void Save(CtpDocumentWriter writer);


        public CtpDocument ToCtpDocument()
        {
            var wr = new CtpDocumentWriter(CommandName);
            Save(wr);
            return wr.ToCtpDocument();
        }

        public override string ToString()
        {
            return ToCtpDocument().ToYAML();
        }

        #region [ Static ]

        /// <summary>
        /// Contains all commands and their corresponding initializers.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Func<CtpDocumentReader, CommandBase>> CommandsInitializers;

        static CommandBase()
        {
            CommandsInitializers = new ConcurrentDictionary<string, Func<CtpDocumentReader, CommandBase>>();
        }

        /// <summary>
        /// Creates a command object from the supplied <see pref="reader"/>. If the command has not been registered, 
        /// an <see cref="CommandUnknown"/> object will be returned.
        /// </summary>
        /// <param name="reader">the serialized data to extract from this reader.</param>
        /// <returns></returns>
        public static CommandBase Create(CtpDocument reader)
        {
            string rootElement = reader.MakeReader().RootElement;
            if (!CommandsInitializers.TryGetValue(rootElement, out Func<CtpDocumentReader, CommandBase> command))
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
        public static void Register(string commandName, Func<CtpDocumentReader, CommandBase> initializer)
        {
            CommandsInitializers[commandName] = initializer;
        }

        #endregion

    }
}
