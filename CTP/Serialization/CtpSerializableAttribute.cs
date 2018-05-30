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
        /// <summary>
        /// Creates a <see cref="CtpSerializableAttribute"/>.
        /// </summary>
        public CtpSerializableAttribute()
        {
        }
    }
}