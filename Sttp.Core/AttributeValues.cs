using CTP;

namespace Sttp
{
    /// <summary>
    /// A key/value pair of metadata.
    /// </summary>
    public class AttributeValues
        : CommandObject<AttributeValues>
    {
        [CommandField()]
        public string Name { get; set; }

        [CommandField()]
        public CtpObject Value { get; set; }

        public AttributeValues()
        {

        }

        public AttributeValues(string name, CtpObject value)
        {
            Name = name;
            Value = value;
        }

        public static explicit operator AttributeValues(CtpCommand obj)
        {
            return FromCommand(obj);
        }

    }
}