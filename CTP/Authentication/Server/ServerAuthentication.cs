using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using CTP.SRP;

namespace CTP.Net
{
    public class ServerAuthentication
    {
        //Must be sorted because longest match is used to match an IP address
        private SortedList<IpMatchDefinition, TrustedIPUserMapping> m_ipUsers = new SortedList<IpMatchDefinition, TrustedIPUserMapping>();

        private SrpServer<SrpUserMapping> m_srpUserDatabase = new SrpServer<SrpUserMapping>();

        public ServerAuthentication()
        {

        }

        public SrpCredential<SrpUserMapping> LookupCredential(Auth user)
        {
            return m_srpUserDatabase.LookupCredential(user);
        }

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpUserDatabase.SetSrpDefaults(salt, strength);
        }

        public void AddIPUser(IPAddress ip, int bitmask, string loginName, params string[] roles)
        {
            var mask = new IpMatchDefinition(ip, bitmask);
            m_ipUsers[mask] = new TrustedIPUserMapping(mask, loginName, roles);
        }

        public void AddSrpUser(string username, string password, string loginName, params string[] roles)
        {
            var mapping = new SrpUserMapping(loginName, roles);
            m_srpUserDatabase.AddCredential(username, password, mapping);
        }

        public void AddSrpUser(string username, byte[] verification, byte[] salt, SrpStrength strength, string loginName, params string[] roles)
        {
            var mapping = new SrpUserMapping(loginName, roles);
            m_srpUserDatabase.AddCredential(username, verification, salt, strength, mapping);
        }

        public void AuthenticateSession(CtpSession session)
        {
            var ipBytes = session.RemoteEndpoint.Address.GetAddressBytes();

            foreach (var item in m_ipUsers.Values)
            {
                if (item.IP.IsMatch(ipBytes))
                {
                    session.LoginName = item.LoginName;
                    session.GrantedRoles.UnionWith(item.Roles);
                    break;
                }
            }
        }

        //private void SessionPairing(CtpSession session)
        //{
        //    var user = m_srpUserDatabase.Pairing(session.FinalStream, session.RemoteCertificate, session.LocalCertificate, out byte[] privateSessionKey);
        //    session.LoginName = user.Token.LoginName;
        //    session.GrantedRoles.UnionWith(user.Token.Roles);
        //    AddSrpUser(user.AssignedUserName, Convert.ToBase64String(privateSessionKey), user.Token.LoginName, user.Token.Roles);
        //}
    }
}
