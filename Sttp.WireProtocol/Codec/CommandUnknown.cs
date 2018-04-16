using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP.Codec
{
    /// <summary>
    /// If a command code is not registered by this API layer, it is therefore reported in it's raw format. 
    /// To register a command, call <see cref="CommandBase.Register"/>
    /// </summary>
    public class CommandUnknown : CommandBase
    {
        /// <summary>
        /// The Markup data for this command.
        /// </summary>
        public readonly CtpMarkup Markup;


        public CommandUnknown(string commandName, CtpMarkup markup)
            : base(commandName)
        {
            Markup = markup;
        }

        /// <summary>
        /// Saves this command object to a <see cref="CtpMarkup"/>.
        /// </summary>
        /// <param name="writer">The writer to save the command to.</param>
        public override void Save(CtpMarkupWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
