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
        /// An alternative name for a document.
        /// </summary>
        public readonly string DocumentName;

        public CommandNameAttribute(string documentName = null)
        {
            DocumentName = documentName;
        }
    }
}
