using System.Collections.Generic;

namespace CTP.Net
{
    public interface ICtpCommandSequence
    {
        ICtpCommandSequence ProcessCommand(CtpSession session, CtpDocument command);
    }
}