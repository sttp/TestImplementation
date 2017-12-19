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
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
          
        }
    }
}