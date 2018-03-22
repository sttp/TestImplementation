using System.Collections.Generic;
using Sttp.Codec;

namespace Sttp.Core
{
    public interface ISttpCommandHandler
    {
        List<string> CommandsHandled();
        void HandleCommand(CommandObjects command, WireEncoder encoder);
    }
}