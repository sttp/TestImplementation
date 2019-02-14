using CTP;

namespace Sttp.Codec
{
    public class MetadataProcedureParameters
        : CommandObject<MetadataProcedureParameters>
    {
        [CommandField()]
        public string Name { get; private set; }
        [CommandField()]
        public CtpObject Value { get; private set; }

        public MetadataProcedureParameters(string name, CtpObject value)
        {
            Name = name;
            Value = value;
        }

        //Exists to support CtpSerializable
        private MetadataProcedureParameters()
        { }

        public override string ToString()
        {
            return $"{Name}: ({Value})";
        }

    }
}