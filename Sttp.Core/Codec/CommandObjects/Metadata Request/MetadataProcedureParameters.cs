using CTP;

namespace Sttp.Codec
{
    public class MetadataProcedureParameters
    {
        public readonly string Name;
        public readonly CtpObject Value;
        public MetadataProcedureParameters(CtpDocumentElement documentElement)
        {
            Name = (string)documentElement.GetValue("Name");
            Value = documentElement.GetValue("Value");
            documentElement.ErrorIfNotHandled();
        }

        public MetadataProcedureParameters(string name, CtpObject value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}: ({Value})";
        }

        public void Save(CtpDocumentWriter sml)
        {
            sml.WriteValue("Name", Name);
            sml.WriteValue("Value", Value);
        }
    }
}