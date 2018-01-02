using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    /// <summary>
    /// A single value that is contained in a <see cref="SttpMarkupElement"/>. 
    /// </summary>
    public class SttpMarkupValue
    {
        /// <summary>
        /// The name of the value.
        /// </summary>
        public readonly string ValueName;
        /// <summary>
        /// The immutable value.
        /// </summary>
        public readonly SttpValue Value;
        /// <summary>
        /// Gets if this value has been handled. See <see cref="SttpMarkupElement.ErrorIfNotHandled"/> for more details.
        /// </summary>
        public bool Handled;

        /// <summary>
        /// Creates this class from the provided reader.
        /// </summary>
        /// <param name="reader">where to read this from.</param>
        public SttpMarkupValue(SttpMarkupReader reader)
        {
            if (reader.NodeType != SttpMarkupNodeType.Value)
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
