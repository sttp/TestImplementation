using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using CTP.SRP;

namespace CTP.Net
{
    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public partial class CtpServer
    {
        private class ProcessClient
        {
            private readonly CtpServer m_server;
            private readonly TcpClient m_client;
            private Stream m_finalStream;
            private SslStream m_ssl;
            private CtpSession m_session;
            private CtpStream m_ctpStream;
            public ProcessClient(CtpServer server, TcpClient client)
            {
                m_server = server;
                m_client = client;

                var thread = new Thread(Process);
                thread.IsBackground = true;
                thread.Start();
            }

            private void Process()
            {
                try
                {
                    TcpClient socket = m_client;
                    NetworkStream netStream = socket.GetStream();
                    bool requireSSL = m_server.m_useSSL;
                    bool hasAccess = m_server.DefaultAllowConnections;
                    X509Certificate serverCertificate = m_server.DefaultCertificate;

                    var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                    foreach (var item in m_server.m_encryptionOptions.Values)
                    {
                        if (item.IP.IsMatch(ipBytes))
                        {
                            hasAccess = item.HasAccess;
                            serverCertificate = item.ServerCertificate;
                            break;
                        }
                    }

                    if (!hasAccess)
                    {
                        throw new Exception("Client does not have access");
                    }

                    if (requireSSL)
                    {
                        serverCertificate = serverCertificate ?? EmphericalCertificate.Value;
                        m_ssl = new SslStream(netStream, false, null, null, EncryptionPolicy.RequireEncryption);
                        m_ssl.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls12, false);
                        m_finalStream = m_ssl;
                    }
                    else
                    {
                        m_finalStream = netStream;
                    }

                    m_ctpStream = new CtpStream();
                    m_ctpStream.SetActiveStream(m_finalStream);

                    m_session = new CtpSession(m_ctpStream, false, CertificateTrustMode.None, socket, netStream, m_ssl);
                    m_server.Authentication.AuthenticateSessionByIP(m_session);

                    var doc = ReadDocument();
                    switch (doc.RootElement)
                    {
                        case "Auth":
                            var user = SrpAuthServer.AuthSrp(m_server.ResumeKeys, m_server.Authentication, (CertExchange)doc, m_ctpStream, m_ssl);
                            m_session.LoginName = user.LoginName;
                            m_session.GrantedRoles.UnionWith(user.Roles);
                            break;
                        case "AuthResume":

                        case "AuthNone":
                            break;
                        default:
                            throw new Exception();
                    }

                    m_server.OnSessionCompleted(m_session);
                }
                catch (Exception e)
                {

                }
            }

            private CtpDocument ReadDocument()
            {
                m_ctpStream.Read(-1);
                return new CtpDocument(m_ctpStream.Results.Payload);
            }


        }
    }
}