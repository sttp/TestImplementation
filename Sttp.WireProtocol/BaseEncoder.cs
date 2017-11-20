using System;

namespace Sttp.WireProtocol
{
    public abstract class BaseEncoder
    {
        protected abstract CommandCode Code { get; }

        protected PayloadWriter Stream;

        protected BaseEncoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
        {
            Stream = new PayloadWriter(sessionDetails, commandEncoder);
            Stream.Clear();
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            Stream.Clear();
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            Stream.Send(Code);
        }
    }
}