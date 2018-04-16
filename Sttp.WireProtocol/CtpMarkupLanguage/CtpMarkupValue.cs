using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    /// <summary>
    /// A single value that is contained in a <see cref="CtpMarkupElement"/>. 
    /// </summary>
    public class CtpMarkupValue
    {
        /// <summary>
        /// The name of the value.
        /// </summary>
        public readonly string ValueName;
        /// <summary>
        /// The immutable value.
        /// </summary>
        public readonly CtpValue Value;
        /// <summary>
        /// Gets if this value has been handled. See <see cref="CtpMarkupElement.ErrorIfNotHandled"/> for more details.
        /// </summary>
        public bool Handled;

        /// <summary>
        /// Creates this class from the provided reader.
        /// </summary>
        /// <param name="reader">where to read this from.</param>
        public CtpMarkupValue(CtpMarkupReader reader)
        {
            if (reader.NodeType != CtpMarkupNodeType.Value)
                throw new Exception("Expecting a Value type for the current node.");
            ValueName = reader.ValueName;
            Value = reader.Value.CloneAsImmutable();
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
