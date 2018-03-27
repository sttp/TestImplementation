using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandDataPointResponseCompleted : CommandBase
    {
        public CommandDataPointResponseCompleted()
            : base("DataPointResponseCompleted")
        {
        }

        public CommandDataPointResponseCompleted(SttpMarkupReader reader)
            : base("DataPointResponseCompleted")
        {
            var element = reader.ReadEntireElement();
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
