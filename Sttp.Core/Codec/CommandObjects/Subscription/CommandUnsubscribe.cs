using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandUnsubscribe : CommandBase
    {
        public CommandUnsubscribe()
            : base("Unsubscribe")
        {
        }

        public CommandUnsubscribe(CtpMarkupReader reader)
            : base("Unsubscribe")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
        }
    }
}
