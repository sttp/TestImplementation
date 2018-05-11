using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
                foreach (var item in Users.m_selfSignCertificateUsers)
                {
                    if (item.UserCertificate.Equals(certificate))
                    {
                        Token.LoginName = item.LoginName;
                        Token.GrantedRoles.UnionWith(item.Roles);
                        return true;
                    }
                }

                foreach (var item in Users.m_certificateUsers)
                {
                    string[] subjects = certificate.Subject.Split('x');
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

        private class AsyncReading
        {
            public SessionToken Session;
            public byte[] ReadBuffer;
            public int ValidBytes => m_bytesRead;
            private int m_bytesRead;
            private int m_waitingByteCount;
            private Action<AsyncReading> m_callback;
            private IAsyncResult m_async;

            public AsyncReading(SessionToken session)
            {
                Session = session;
                m_bytesRead = 0;

            }

            public void Reset()
            {
                m_bytesRead = 0;
            }


            public void WaitForBytes(int byteCount, Action<AsyncReading> callback)
            {
                if (ReadBuffer == null)
                {
                    ReadBuffer = new byte[byteCount];
                }
                else if (ReadBuffer.Length < byteCount)
                {
                    byte[] newBuffer = new byte[byteCount];
                    ReadBuffer.CopyTo(newBuffer, 0);
                    ReadBuffer = newBuffer;
                }

                m_waitingByteCount = byteCount;
                m_callback = callback;

                if (byteCount <= m_bytesRead)
                {
                    ThreadPool.QueueUserWorkItem(x => callback(this));
                }
                else
                {
                    m_async = Session.FinalStream.BeginRead(ReadBuffer, m_bytesRead, m_waitingByteCount - m_bytesRead, Callback, null);
                }
            }

            private void Callback(IAsyncResult ar)
            {
                int bytesRead = Session.FinalStream.EndRead(ar);
                if (bytesRead == 0)
                    throw new EndOfStreamException();
                m_bytesRead += bytesRead;
                if (m_waitingByteCount < m_bytesRead)
                {
                    m_async = Session.FinalStream.BeginRead(ReadBuffer, m_bytesRead, m_waitingByteCount - m_bytesRead, Callback, null);
                }
                else
                {
                    m_callback(this);
                }
            }
        }

        private SortedSet<EncryptionOptions> m_encryptionOptions = new SortedSet<EncryptionOptions>();
        private SortedSet<SelfSignCertificateUserMapping> m_selfSignCertificateUsers = new SortedSet<SelfSignCertificateUserMapping>();
        private SortedSet<CertificateUserMapping> m_certificateUsers = new SortedSet<CertificateUserMapping>();
        private SortedSet<SrpUserMapping> m_srpUsers = new SortedSet<SrpUserMapping>();
        private SortedSet<WindowsGroupMapping> m_windowsGroupUsers = new SortedSet<WindowsGroupMapping>();
        private SortedSet<WindowsUserMapping> m_windowsUsers = new SortedSet<WindowsUserMapping>();
        private SortedSet<TrustedIPUserMapping> m_ipUsers = new SortedSet<TrustedIPUserMapping>();

        public event SessionCompletedEventHandler SessionCompleted;

        public void AssignEncriptionOptions(IPAddress remoteIP, int bitmask, X509Certificate localCertificate)
        {
            var mask = new IpMatchDefinition(remoteIP, bitmask);
            m_encryptionOptions.Add(new EncryptionOptions(mask, localCertificate));
        }

        public void AddIPUser(IPAddress ip, int bitmask, string loginName, params string[] roles)
        {
            var mask = new IpMatchDefinition(ip, bitmask);
            m_ipUsers.Add(new TrustedIPUserMapping(mask, loginName, roles));
        }

        public void AddSelfSignedCertificateUser(X509Certificate user, string loginName, params string[] roles)
        {
            m_selfSignCertificateUsers.Add(new SelfSignCertificateUserMapping(user, loginName, roles));
        }

        public void AuthenticateAsServer(SessionToken session)
        {
            var ipBytes = session.RemoteEndpoint.Address.GetAddressBytes();
            X509Certificate localCertificate = null;
            foreach (var item in m_encryptionOptions)
            {
                if (item.IP.IsMatch(ipBytes))
                {
                    localCertificate = item.ServerCertificate;
                    break;
                }
            }

            foreach (var item in m_ipUsers)
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
                    throw new NotSupportedException();
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
