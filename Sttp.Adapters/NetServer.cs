using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.Adapters
{
    public class NetServer
    {
        private Listener m_listeners;

        public NetServer()
        {
            var options = new ListenerConfig();
            options.ListenEndpoint = new IPEndPoint(IPAddress.IPv6Any, 25832);
            options.Add(IPAddress.Any, 32);
            options.Add(IPAddress.IPv6Any, 128);
            m_listeners = new Listener(options);
            m_listeners.NewClient += M_listeners_NewClient;
        }

        public void Start()
        {
            m_listeners.Start();
        }

        public void Stop()
        {
            m_listeners.Stop();
        }

        private void M_listeners_NewClient(TcpClient client, HashSet<Guid> sids)
        {

        }
    }
}
