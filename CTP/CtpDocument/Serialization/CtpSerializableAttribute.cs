using System;
using CTP.Serialization;

namespace CTP.Serialization
{
    /// <summary>
    /// Marks a field or property indicating this value may be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CtpCommandAttribute
        : Attribute
    {
        public readonly string CommandName;

        public CtpCommandAttribute(string commandName = null)
        {
            CommandName = commandName;
        }
    }
}
