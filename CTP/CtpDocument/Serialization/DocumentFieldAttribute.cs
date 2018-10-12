using System;

namespace CTP
{
    /// <summary>
    /// Marks a field or property indicating this value may be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DocumentFieldAttribute
        : Attribute
    {
        public readonly string RecordName;

        /// <summary>
        /// Creates a <see cref="DocumentFieldAttribute"/>.
        /// </summary>
        /// <param name="recordName">The distinct name that maps this field to the document. If null, the name of the field will be used.</param>
        public DocumentFieldAttribute(string recordName = null)
        {
            RecordName = recordName;
        }
    }
}
