using CTP;

namespace Sttp
{
    /// <summary>
    /// A key/value pair of metadata.
    /// </summary>
    public class AttributeValues
    {
        [CommandField()]
        public string Name { get; private set; }

        [CommandField()]
        public CtpObject Value { get; private set; }

        private AttributeValues()
        {

        }

        public AttributeValues(string name, CtpObject value)
        {
            Name = name;
            Value = value;
        }
    }
}