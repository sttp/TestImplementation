
using System;
using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdUndefineRow
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.UndefineRow;
        public SttpValue PrimaryKey;

        public void Load(SttpMarkupReader reader)
        {
            var element = reader.ReadEntireElement();

            PrimaryKey = element.GetValue("PrimaryKey");
            element.ErrorIfNotHandled();
        }

    }
}