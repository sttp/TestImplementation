using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace Sttp.Codec
{
    public class CommandKeepAlive : DocumentCommandBase
    {
        public CommandKeepAlive()
            : base("KeepAlive")
        {
        }

        public CommandKeepAlive(CtpDocumentReader reader)
            : base("KeepAlive")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {

        }
    }
}
