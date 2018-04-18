using System;
using System.Collections.Generic;
using System.Linq;
using CTP;

namespace Sttp.Codec
{
    public class CommandSubscribeToAll : DocumentCommandBase
    {
        public CommandSubscribeToAll()
            : base("SubscribeToAll")
        {
        }

        public CommandSubscribeToAll(CtpDocumentReader reader)
            : base("SubscribeToAll")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
        }
    }
}
