using System;

namespace CTP
{
    /// <summary>
    /// A single value that is contained in a <see cref="CtpDocumentElement"/>. 
    /// </summary>
    public class CtpDocumentValue
    {
        /// <summary>
        /// The name of the value.
        /// </summary>
        public readonly string ValueName;
        /// <summary>
        /// The immutable value.
        /// </summary>
        public readonly CtpObject Value;
        /// <summary>
        /// Gets if this value has been handled. See <see cref="CtpDocumentElement.ErrorIfNotHandled"/> for more details.
        /// </summary>
        public bool Handled;

        /// <summary>
        /// Creates this class from the provided reader.
        /// </summary>
        /// <param name="reader">where to read this from.</param>
        public CtpDocumentValue(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Expecting a Value type for the current node.");
            ValueName = reader.ValueName;
            Value = reader.Value.Clone();
        }

        /// <summary>
        /// Name: {ValueName} Value: {Value.ToTypeString} Handled: {Handled}
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name: {ValueName} Value: {Value.ToTypeString} Handled: {Handled}";
        }
    }
}
