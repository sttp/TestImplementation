using System;

namespace CTP
{
    /// <summary>
    /// Marks a field or property indicating this value may be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CommandFieldAttribute
        : Attribute
    {
        public readonly string RecordName;

        /// <summary>
        /// Creates a <see cref="CommandFieldAttribute"/>.
        /// </summary>
        /// <param name="recordName">The distinct name that maps this field to the document. If null, the name of the field will be used.</param>
        public CommandFieldAttribute(string recordName = null)
        {
            RecordName = recordName;
        }
    }
}
