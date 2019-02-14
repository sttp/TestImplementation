using System;

namespace CTP
{
    /// <summary>
    /// Explicitly names the root of a <see cref="CtpCommand"/>. 
    /// Without this attribute, the default name will be the name of the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandNameAttribute
        : Attribute
    {
        /// <summary>
        /// An alternative name for a command.
        /// </summary>
        public readonly string CommandName;

        /// <summary>
        /// The name of this command.
        /// </summary>
        /// <param name="commandName"></param>
        public CommandNameAttribute(string commandName = null)
        {
            CommandName = commandName;
        }
    }
}
