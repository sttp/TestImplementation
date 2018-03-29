using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandSubscribe : CommandBase
    {
        public CommandSubscribe()
            : base("Subscribe")
        {
        }

        public CommandSubscribe(SttpMarkupReader reader)
            : base("Subscribe")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
