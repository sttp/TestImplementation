using System;
using System.Collections.Generic;

namespace CTP.Net
{
    public interface ICtpRootHandler
    {
        List<string> RootCommands();

        /// <summary>
        /// Note: After this method returns, the internal events will be raised, so you must register
        /// the event handlers properly.
        /// </summary>
        /// <param name="request">the object that can respond to this request.</param>
        /// <param name="command">the command to handle.</param>
        void HandleRequest(CtpRequest request, CtpDocument command);
    }

}