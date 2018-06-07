using System;
using System.Collections.Generic;
using CTP.Net;

namespace CTP
{
    public interface ICtpCommandHandler
    {
        List<string> CommandsHandled();
        void HandleCommand(CtpSession session, CtpReadResults readResults);
    }
}