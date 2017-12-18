using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public abstract class CommandBase
    {
        public readonly string CommandName;

        protected CommandBase(string name)
        {
            CommandName = name;
        }

        public abstract CommandBase Load(SttpMarkupReader reader);
        public abstract void Save(SttpMarkupWriter writer);
    }
}
