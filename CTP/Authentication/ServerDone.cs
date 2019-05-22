using System.Security.Cryptography;

namespace CTP
{
    [CommandName("ServerDone")]
    public class ServerDone
        : CommandObject<ServerDone>
    {
        /// <summary>
        /// The requested Service Provider Name
        /// </summary>
        [CommandField()]
        public string SPN { get; private set; }

        public ServerDone(string spn)
        {
            SPN = spn;
        }

        private ServerDone()
        {

        }

        public static explicit operator ServerDone(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}