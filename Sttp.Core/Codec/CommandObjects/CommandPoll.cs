using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandPoll : CommandBase
    {
        public CommandPoll()
            : base("Poll")
        {
        }

        public CommandPoll(SttpMarkupReader reader)
            : base("Poll")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
