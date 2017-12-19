using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpMarkupValue
    {
        public readonly string ValueName;
        public readonly SttpValue Value;
        public bool Handled;

        public SttpMarkupValue(SttpMarkupReader reader)
        {
            if (reader.NodeType != SttpMarkupNodeType.Value)
                throw new Exception("Expecting a Value type for the current node.");
            ValueName = reader.ValueName;
            Value = reader.Value.CloneAsImmutable();
        }

        public override string ToString()
        {
            return $"Name: {ValueName} Value: {Value.ToTypeString} Handled: {Handled}";
        }
    }
}
