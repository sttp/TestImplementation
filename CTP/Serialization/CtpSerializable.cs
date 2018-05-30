using System;

namespace CTP.Serialization
{
    /// <summary>
    /// An object that can be serialized into a CTPDocument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CtpSerializable
        : Attribute
    {
        /// <summary>
        /// Creates a <see cref="CtpSerializable"/>.
        /// </summary>
        public CtpSerializable()
        {
        }
    }
}