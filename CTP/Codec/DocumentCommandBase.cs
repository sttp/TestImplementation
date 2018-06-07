using System;
using System.Collections.Concurrent;

namespace CTP
{
    /// <summary>
    /// This base class assists in serializing <see cref="CommandCode.Document"/> into 
    /// concrete objects from their corresponding <see cref="CtpDocument"/> data.
    /// </summary>
    public static class DocumentCommandBase
    {
        /// <summary>
        /// Contains all commands and their corresponding initializers.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Func<CtpDocument, object>> CommandsInitializers;

        static DocumentCommandBase()
        {
            CommandsInitializers = new ConcurrentDictionary<string, Func<CtpDocument, object>>();
        }

        /// <summary>
        /// Creates a command object from the supplied <see pref="reader"/>. If the command has not been registered, 
        /// null will be returned.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="reader">the serialized data to extract from this reader.</param>
        /// <returns></returns>
        public static object Create(string commandName, CtpDocument reader)
        {
            if (!CommandsInitializers.TryGetValue(commandName, out Func<CtpDocument, object> command))
            {
                return null;
            }
            return command(reader);
        }

        /// <summary>
        /// Registers an initializer for a command. This will be used when receiving a command to attempt to turn it into an object.
        /// If a command is received that is not registered, null will be created in its place.
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <param name="initializer">The initializer</param>
        public static void Register(string commandName, Func<CtpDocument, object> initializer)
        {
            CommandsInitializers[commandName] = initializer;
        }
    }
}
