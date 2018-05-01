using System;
using System.Collections.Generic;
using System.Net;

namespace Sttp.Adapters
{
    public class ListenerConfig
    {
        public IPEndPoint ListenEndpoint;
        private object m_syncRoot;
        private List<IpMatchDefinition> m_list;

        public ListenerConfig()
        {
            m_syncRoot = new object();
            m_list = new List<IpMatchDefinition>();
        }

        public void Add(IPAddress ip, int maskBits, List<Guid> sids = null)
        {
            var client = new IpMatchDefinition(ip, maskBits, sids);
            lock (m_syncRoot)
            {
                if (!m_list.Contains(client))
                {
                    m_list.Add(client);
                    m_list.Sort();
                }
            }
        }

        public bool HasAccess(IPAddress ipAddress, HashSet<Guid> sids)
        {
            bool isMatch = false;
            var src = ipAddress.GetAddressBytes();
            lock (m_syncRoot)
            {
                if (m_list.Count == 0)
                    return true;

                foreach (var remote in m_list)
                {
                    if (remote.IsMatch(src))
                    {
                        isMatch = true;
                        sids.UnionWith(remote.SID);
                    }
                }

                return isMatch;
            }
        }
    }

}