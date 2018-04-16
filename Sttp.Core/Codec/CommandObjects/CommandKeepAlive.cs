using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandKeepAlive : CommandBase
    {
        public CommandKeepAlive()
            : base("KeepAlive")
        {
        }

        public CommandKeepAlive(CtpMarkupReader reader)
            : base("KeepAlive")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
        }
    }
}
