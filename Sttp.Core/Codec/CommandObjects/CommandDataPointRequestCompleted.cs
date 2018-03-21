using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandDataPointRequestCompleted : CommandBase
    {
        public CommandDataPointRequestCompleted()
            : base("DataPointRequestCompleted")
        {
        }

        public CommandDataPointRequestCompleted(SttpMarkupReader reader)
            : base("DataPointRequestCompleted")
        {
            var element = reader.ReadEntireElement();
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
