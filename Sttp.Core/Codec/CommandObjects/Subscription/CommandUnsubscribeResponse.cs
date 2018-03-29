using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandUnsubscribeResponse : CommandBase
    {
        public CommandUnsubscribeResponse()
            : base("UnsubscribeResponse")
        {
        }

        public CommandUnsubscribeResponse(SttpMarkupReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {

        }
    }
}
