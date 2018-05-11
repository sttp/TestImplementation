using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CTP.IO;
using CTP.SRP;

namespace CTP.Net
{
    public enum AuthenticationProtocols : byte
    {
        None = 0,
        SRP = 1,
        NegotiateStream = 2,
        OAUTH = 3,
        LDAP = 4,
    }

    public delegate void SessionCompletedEventHandler(SessionToken token);

    public class UserCredentialServices
    {
        private class SSLUserCertificateValidation
        {
            public UserCredentialServices Users;
            public SessionToken Token;

            public SSLUserCertificateValidation(UserCredentialServices users, SessionToken token)
            {
                Users = users;
                Token = token;
            }

            public bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
            {
                if (certificate == null)
                    return true;

                if (Users.m_selfSignCertificateUsers.TryGetValue(certificate, out SelfSignCertificateUserMapping user))
                {
                    Token.LoginName = user.LoginName;
                    Token.GrantedRoles.UnionWith(user.Roles);
                    return true;
                }

                foreach (var item in Users.m_certificateUsers)
                {
                    string[] subjects = certificate.Subject.Split(',');
                    foreach (var subject in subjects)
                    {
                        if (!item.NameRecord.Contains(subject.Trim(), StringComparer.OrdinalIgnoreCase))
                        {
                            goto Next;
                        }
                    }

                    foreach (var chainCerts in chain.ChainElements)
                    {
                        if (item.TrustedRootCertificates.Contains(chainCerts.Certificate))
                        {
                            Token.LoginName = item.LoginName;
                            Token.GrantedRoles.UnionWith(item.Roles);
                            return true;
                        }
                    }
                    Next:
                    ;
                }

                return true;
            }
        }

        //Must be sorted because longest match is used to match an IP address
        private SortedList<IpMatchDefinition, EncryptionOptions> m_encryptionOptions = new SortedList<IpMatchDefinition, EncryptionOptions>();
        private SortedList<IpMatchDefinition, TrustedIPUserMapping> m_ipUsers = new SortedList<IpMatchDefinition, TrustedIPUserMapping>();

        private Dictionary<X509Certificate, SelfSignCertificateUserMapping> m_selfSignCertificateUsers = new Dictionary<X509Certificate, SelfSignCertificateUserMapping>();
        private Dictionary<string, SrpUserMapping> m_srpUsers = new Dictionary<string, SrpUserMapping>();
        private SortedSet<CertificateUserMapping> m_certificateUsers = new SortedSet<CertificateUserMapping>();
        private SortedSet<WindowsGroupMapping> m_windowsGroupUsers = new SortedSet<WindowsGroupMapping>();
        private SortedSet<WindowsUserMapping> m_windowsUsers = new SortedSet<WindowsUserMapping>();
        private byte[] m_srpDefaultSalt = RNG.CreateSalt(64);
        private SrpStrength m_srpStrength = SrpStrength.Bits1024;

        public event SessionCompletedEventHandler SessionCompleted;

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpDefaultSalt = salt;
            m_srpStrength = strength;
        }

        public void AssignEncriptionOptions(IPAddress remoteIP, int bitmask, X509Certificate localCertificate)
        {
            var mask = new IpMatchDefinition(remoteIP, bitmask);
            m_encryptionOptions[mask] = new EncryptionOptions(mask, localCertificate);
        }

        public void AddIPUser(IPAddress ip, int bitmask, string loginName, params string[] roles)
        {
            var mask = new IpMatchDefinition(ip, bitmask);
            m_ipUsers[mask] = new TrustedIPUserMapping(mask, loginName, roles);
        }

        public void AddSelfSignedCertificateUser(X509Certificate user, string loginName, params string[] roles)
        {
            m_selfSignCertificateUsers[user] = new SelfSignCertificateUserMapping(user, loginName, roles);
        }

        public void AddSrpUser(string username, string password, string loginName, params string[] roles)
        {
            using (var sha = SHA512.Create())
            {
                string un = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
                byte[] data = Encoding.UTF8.GetBytes(un);
                byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                data.CopyTo(rv, 0);
                m_srpDefaultSalt.CopyTo(rv, data.Length);
                byte[] salt = sha.ComputeHash(m_srpDefaultSalt);
                var credentials = new SrpUserCredential(username, password, salt, m_srpStrength);
                m_srpUsers[credentials.UserName] = new SrpUserMapping(credentials, loginName, roles);
            }
        }

        public void AddSrpUser(string username, byte[] verification, byte[] salt, SrpStrength strength, string loginName, params string[] roles)
        {
            var credentials = new SrpUserCredential(username, verification, salt, strength);
            m_srpUsers[credentials.UserName] = new SrpUserMapping(credentials, loginName, roles);
        }

        public void AuthenticateAsServer(SessionToken session)
        {
            var ipBytes = session.RemoteEndpoint.Address.GetAddressBytes();
            X509Certificate localCertificate = null;
            foreach (var item in m_encryptionOptions.Values)
            {
                if (item.IP.IsMatch(ipBytes))
                {
                    localCertificate = item.ServerCertificate;
                    break;
                }
            }

            foreach (var item in m_ipUsers.Values)
            {
                if (item.IP.IsMatch(ipBytes))
                {
                    session.LoginName = item.LoginName;
                    session.GrantedRoles.UnionWith(item.Roles);
                    break;
                }
            }

            if (localCertificate != null)
            {
                SSLAsServer(localCertificate, session);
            }
            switch ((AuthenticationProtocols)session.FinalStream.ReadNextByte())
            {
                case AuthenticationProtocols.None:
                    Finish(session);
                    break;
                case AuthenticationProtocols.SRP:
                    SrpAsServer(session);
                    break;
                case AuthenticationProtocols.NegotiateStream:
                    WinAsServer(session);
                    break;
                case AuthenticationProtocols.OAUTH:
                    throw new NotSupportedException();
                    break;
                case AuthenticationProtocols.LDAP:
                    throw new NotSupportedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void SrpAsServer(SessionToken session)
        {
            string userName = session.FinalStream.ReadString();
            userName = userName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            if (!m_srpUsers.TryGetValue(userName, out SrpUserMapping user))
            {
                using (var sha = SHA512.Create())
                {
                    byte[] data = Encoding.UTF8.GetBytes(userName);
                    byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                    data.CopyTo(rv, 0);
                    m_srpDefaultSalt.CopyTo(rv, data.Length);
                    byte[] salt = sha.ComputeHash(m_srpDefaultSalt);
                    var credentials = new SrpUserCredential(userName, Guid.NewGuid().ToString(), salt, m_srpStrength);
                    user = new SrpUserMapping(credentials, null, new string[0]);
                }
            }

            var server = new Srp6aServer(user.Credentials);
            server.AuthenticateAsServer(session.FinalStream);
        }

        private void SSLAsServer(X509Certificate certificate, SessionToken session)
        {
            var obj = new SSLUserCertificateValidation(this, session);
            session.SSL = new SslStream(session.NetworkStream, false, obj.UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
            session.SSL.AuthenticateAsServer(certificate, true, SslProtocols.Tls12, false);
        }

        private void WinAsServer(SessionToken client)
        {
            client.Win = new NegotiateStream(client.FinalStream, true);
            try
            {
                client.Win.AuthenticateAsServer(CredentialCache.DefaultNetworkCredentials, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);

                var identity = client.Win.RemoteIdentity as WindowsIdentity; //When called by the server, returns WindowsIdentity. 
                //If it returns a GenericIdentity, this is because identifier information was not provided by the client. Therefore assigning null is sufficient.

                if (identity == null)
                {
                    Finish(client);
                    return;
                }

                foreach (var user in m_windowsUsers)
                {
                    var name = identity.Name;



                }

                Finish(client);
                return;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private void Finish(SessionToken client)
        {
            SessionCompleted?.Invoke(client);
        }
    }

    public class EncryptionOptions : IComparable<EncryptionOptions>, IComparable, IEquatable<EncryptionOptions>
    {
        public IpMatchDefinition IP;
        public X509Certificate ServerCertificate;

        public EncryptionOptions(IpMatchDefinition ip, X509Certificate serverCertificate)
        {
            IP = ip;
            ServerCertificate = serverCertificate;
        }

        public int CompareTo(EncryptionOptions other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (ReferenceEquals(null, other))
                return 1;
            return IP.CompareTo(other.IP);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (ReferenceEquals(this, obj))
                return 0;
            if (!(obj is EncryptionOptions))
                throw new ArgumentException($"Object must be of type {nameof(EncryptionOptions)}");
            return CompareTo((EncryptionOptions)obj);
        }

        public static bool operator <(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) >= 0;
        }

        public bool Equals(EncryptionOptions other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return IP.Equals(other.IP);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((EncryptionOptions)obj);
        }

        public override int GetHashCode()
        {
            return IP.GetHashCode();
        }

        public static bool operator ==(EncryptionOptions left, EncryptionOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EncryptionOptions left, EncryptionOptions right)
        {
            return !Equals(left, right);
        }
    }

    public class SelfSignCertificateUserMapping
    {
        public X509Certificate UserCertificate;
        public string LoginName;
        public string[] Roles;

        public SelfSignCertificateUserMapping(X509Certificate userCertificate, string loginName, string[] roles)
        {
            UserCertificate = userCertificate;
            LoginName = loginName;
            Roles = roles;
        }
    }

    public class CertificateUserMapping
    {
        public List<string> NameRecord;
        public X509CertificateCollection TrustedRootCertificates;

        public string LoginName;
        public string[] Roles;
    }

    public class SrpUserMapping
    {
        public SrpUserCredential Credentials;
        public string LoginName;
        public string[] Roles;

        public SrpUserMapping(SrpUserCredential credentials, string loginName, string[] roles)
        {
            Credentials = credentials;
            LoginName = loginName;
            Roles = roles;
        }
    }

    public class WindowsUserMapping
    {
        public string Domain;
        public string Username;
        public string LoginName;
        public string[] Roles;
    }

    public class WindowsGroupMapping
    {
        public string Domain;
        public string Group;
        public string LoginName;
        public string[] Roles;
    }

    public class TrustedIPUserMapping
    {
        public IpMatchDefinition IP;
        public string LoginName;
        public string[] Roles;

        public TrustedIPUserMapping(IpMatchDefinition ip, string loginName, string[] roles)
        {
            IP = ip;
            LoginName = loginName;
            Roles = roles;
        }
    }

    public class OAuthUserMapping
    {
        public string LoginName;
        public string[] Roles;

    }

}
