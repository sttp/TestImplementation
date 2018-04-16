using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandSubscribeToAll : CommandBase
    {
        public CommandSubscribeToAll()
            : base("SubscribeToAll")
        {
        }

        public CommandSubscribeToAll(SttpMarkupReader reader)
            : base("SubscribeToAll")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
