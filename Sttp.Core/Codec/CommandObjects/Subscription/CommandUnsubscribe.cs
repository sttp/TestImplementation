using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandUnsubscribe : CommandBase
    {
        public CommandUnsubscribe()
            : base("Unsubscribe")
        {
        }

        public CommandUnsubscribe(SttpMarkupReader reader)
            : base("Unsubscribe")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
