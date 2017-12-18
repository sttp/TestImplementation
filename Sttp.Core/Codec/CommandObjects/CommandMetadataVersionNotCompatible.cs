using System;

namespace Sttp.Codec
{
    public class CommandMetadataVersionNotCompatible : CommandBase
    {
        public CommandMetadataVersionNotCompatible()
            : base("MetadataVersionNotCompatible")
        {

        }

        public CommandMetadataVersionNotCompatible(SttpMarkupReader reader)
            : base("MetadataVersionNotCompatible")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
            }
        }
    }
}