using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    public class MetadataProcedureParameters
    {
        [DocumentField()]
        public string Name { get; private set; }
        [DocumentField()]
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