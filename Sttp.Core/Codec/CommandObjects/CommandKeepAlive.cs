using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandKeepAlive : CommandBase
    {
        public CommandKeepAlive()
            : base("KeepAlive")
        {
        }

        public CommandKeepAlive(SttpMarkupReader reader)
            : base("KeepAlive")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
