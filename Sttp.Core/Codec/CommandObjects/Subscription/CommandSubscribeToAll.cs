using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandSubscribeToAll : CommandBase
    {
        public CommandSubscribeToAll()
            : base("SubscribeToAll")
        {
        }

        public CommandSubscribeToAll(CtpMarkupReader reader)
            : base("SubscribeToAll")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
        }
    }
}
