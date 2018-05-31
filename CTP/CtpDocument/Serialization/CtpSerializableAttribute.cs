using System;

namespace CTP.Serialization
{
    /// <summary>
    /// An object that can be serialized into a CTPDocument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CtpSerializableAttribute
        : Attribute
    {
        public readonly string RootCommandName;

        /// <summary>
        /// Creates a <see cref="CtpSerializableAttribute"/>.
        /// </summary>
        /// <param name="rootCommandName">If this object is the root of a document, this name will be used. Otherwise, it will be ignored.</param>
        public CtpSerializableAttribute(string rootCommandName)
        {
            RootCommandName = rootCommandName;
        }
    }
}