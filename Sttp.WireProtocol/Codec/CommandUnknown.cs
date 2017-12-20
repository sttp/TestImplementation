using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    /// <summary>
    /// If a command code is unknown by this API layer, it will be reported in a raw format.
    /// </summary>
    public class CommandUnknown : CommandBase
    {
        public readonly SttpMarkup Markup;

        public CommandUnknown(string name)
            : base(name)
        {
        }


        public CommandUnknown(string commandName, SttpMarkup markup)
            : base(commandName)
        {
            Markup = markup;
        }

        public override void Save(SttpMarkupWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
