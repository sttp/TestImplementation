using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandPollCompleted : CommandBase
    {
        public CommandPollCompleted()
            : base("PollCompleted")
        {
        }

        public CommandPollCompleted(SttpMarkupReader reader)
            : base("PollCompleted")
        {
            var element = reader.ReadEntireElement();
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
        }
    }
}
