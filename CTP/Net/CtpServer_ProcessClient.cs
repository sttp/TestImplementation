using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;

namespace CTP.Net
{
    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public partial class CtpServer
    {
        /// <summary>
        /// Responsible for processing new client connections for the <see cref="CtpServer"/>. Use <see cref="AcceptAsync"/>. This method will
        /// callback <see cref="CtpServer.OnSessionCompleted"/> once a connection has been successful.
        /// </summary>
        private class ProcessClient
        {
            //ToDo: For now, a new background thread is started that synchronously authenticates a new client.
            //      There may be opportunity to make this async one day.

            private readonly CtpServer m_server;
            private readonly TcpClient m_client;
            private Stream m_finalStream;
            private SslStream m_ssl;
            private CtpSession m_session;

            private ProcessClient(CtpServer server, TcpClient client)
            {
                m_server = server;
                m_client = client;
            }

            private void Start()
            {
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

                    var config = m_server.m_config.EncryptionOptions;

                    if (config.EnableSSL)
                    {
                        m_ssl = new SslStream(netStream, false, null, null, EncryptionPolicy.RequireEncryption);
                        m_ssl.AuthenticateAsServer(config.ServerCertificate, false, SslProtocols.Tls12, false);
                        m_finalStream = m_ssl;
                    }
                    else
                    {
                        m_finalStream = netStream;
                    }

                    m_session = new CtpSession(m_finalStream, false, socket, netStream, m_ssl);

                    var doc = m_session.Read();
                    string loginName = null;
                    string accountName = null;
                    List<string> roles = null;
                    switch (doc.RootElement)
                    {
                        case "Auth":
                            var auth = (Auth)doc;
                            if (m_server.m_config.CertificateClients.TryGetValue(auth.AuthorizationCertificate, out var clientCert))
                            {
                                if (auth.ValidateSignature(clientCert.Certificate))
                                {
                                    var ticket = (Ticket)auth.Ticket;
                                    if (ticket.ValidFrom < DateTime.UtcNow && DateTime.UtcNow <= ticket.ValidTo)
                                    {
                                        accountName = clientCert.ClientCert.MappedAccount;
                                        loginName = ticket.LoginName;
                                        roles = m_server.m_config.Accounts[accountName].Union(ticket.Roles).ToList();
                                    }
                                    break;
                                }
                            }
                            break;
                        case "AuthNone":
                            foreach (var item in m_server.m_config.AnonymousMappings)
                            {
                                var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                                if (item.Key.IsMatch(ipBytes))
                                {
                                    accountName = item.Value;
                                    roles = m_server.m_config.Accounts[accountName];
                                    break;
                                }
                            }
                            break;
                        default:
                            throw new Exception();
                    }

                    m_session.GrantedRoles = new HashSet<string>(roles);
                    m_session.LoginName = loginName;
                    m_server.OnSessionCompleted(m_session);
                }
                catch (Exception)
                {
                    //Swallow the exception since a failed connection attempt can safely be ignored by the server.
                }
            }

            public static void AcceptAsync(CtpServer server, TcpClient client)
            {
                var pc = new ProcessClient(server, client);
                pc.Start();
            }

        }
    }
}