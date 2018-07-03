using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using CTP.SRP;

namespace CTP.Net
{
    public class ServerAuthentication : CtpCommandHandlerBase
    {
        //Must be sorted because longest match is used to match an IP address
        private SortedList<IpMatchDefinition, TrustedIPUserMapping> m_ipUsers = new SortedList<IpMatchDefinition, TrustedIPUserMapping>();
        private Dictionary<string, SelfSignCertificateUserMapping> m_selfSignCertificateUsers = new Dictionary<string, SelfSignCertificateUserMapping>();
        private SortedSet<WindowsGroupMapping> m_windowsGroupUsers = new SortedSet<WindowsGroupMapping>();
        private SortedSet<WindowsUserMapping> m_windowsUsers = new SortedSet<WindowsUserMapping>();
        private SrpServer<SrpUserMapping> m_srpUserDatabase = new SrpServer<SrpUserMapping>();

        public ServerAuthentication()
        {
            SupportedCommands.Add("SrpIdentity");
            SupportedCommands.Add("AuthNegotiate");
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

        public void AddSelfSignedCertificateUser(X509Certificate user, string loginName, params string[] roles)
        {
            m_selfSignCertificateUsers[user.GetCertHashString()] = new SelfSignCertificateUserMapping(user, loginName, roles);
        }

        public void AddSrpUser(string username, string password, string loginName, params string[] roles)
        {
            var mapping = new SrpUserMapping(loginName, roles);
            m_srpUserDatabase.AddUser(username, password, mapping);
        }

        public void AddSrpUser(string username, byte[] verification, byte[] salt, SrpStrength strength, string loginName, params string[] roles)
        {
            var mapping = new SrpUserMapping(loginName, roles);
            m_srpUserDatabase.AddUser(username, verification, salt, strength, mapping);
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

            if (session.RemoteCertificate != null)
            {
                if (m_selfSignCertificateUsers.TryGetValue(session.RemoteCertificate.GetCertHashString(), out SelfSignCertificateUserMapping user))
                {
                    if (session.RemoteCertificate.GetPublicKeyString() == user.UserCertificate.GetPublicKeyString())
                    {
                        session.LoginName = user.LoginName;
                        session.GrantedRoles.UnionWith(user.Roles);
                    }
                }
            }
        }

        //private void CertificatePairing(CtpSession session)
        //{
        //    if (session.RemoteCertificate != null)
        //        throw new Exception("A remote certificate must be supplied.");
        //    var user = m_srpUserDatabase.Pairing(session.FinalStream, session.RemoteCertificate, session.LocalCertificate, out byte[] privateSessionKey);
        //    AddSelfSignedCertificateUser(session.Ssl.RemoteCertificate, user.Token.LoginName, user.Token.Roles);
        //    session.LoginName = user.Token.LoginName;
        //    session.GrantedRoles.UnionWith(user.Token.Roles);
        //}

        //private void SessionPairing(CtpSession session)
        //{
        //    var user = m_srpUserDatabase.Pairing(session.FinalStream, session.RemoteCertificate, session.LocalCertificate, out byte[] privateSessionKey);
        //    session.LoginName = user.Token.LoginName;
        //    session.GrantedRoles.UnionWith(user.Token.Roles);
        //    AddSrpUser(user.AssignedUserName, Convert.ToBase64String(privateSessionKey), user.Token.LoginName, user.Token.Roles);
        //}

        private void SrpAsServer(CtpSession session)
        {

        }

        private void WinAsServer(CtpSession client, AuthNegotiate command)
        {
            //using (var stream = client.OpenStream(command.StreamID))
            //{
            //    client.Win = new NegotiateStream(stream, true);
            //    try
            //    {
            //        client.Win.AuthenticateAsServer(CredentialCache.DefaultNetworkCredentials, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);

            //        var identity = client.Win.RemoteIdentity as WindowsIdentity; //When called by the server, returns WindowsIdentity. 
            //        //If it returns a GenericIdentity, this is because identifier information was not provided by the client. Therefore assigning null is sufficient.

            //        if (identity == null)
            //        {
            //            return;
            //        }

            //        foreach (var user in m_windowsUsers)
            //        {
            //            var name = identity.Name;
            //        }

            //        return;
            //    }
            //    catch (Exception e)
            //    {
            //        throw;
            //    }
            //}
        }

        public override CtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command)
        {
            switch (command.RootElement)
            {
                case "SrpIdentity":
                    return m_srpUserDatabase.Authenticate(session, (SrpIdentity)command, session.RemoteCertificate, session.LocalCertificate, (x, user) =>
                                                                                                                                              {
                                                                                                                                                  session.LoginName = user.LoginName;
                                                                                                                                                  session.GrantedRoles.UnionWith(user.Roles);
                                                                                                                                              });
                    break;
                case "AuthNegotiate":
                    throw new NotSupportedException();
                    //WinAsServer(session, (AuthNegotiate)readResults.DocumentPayload);
                    break;
                default:
                    throw new Exception("Command invalid");
            }
        }

        public override void Cancel()
        {
        }
    }
}
