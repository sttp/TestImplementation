using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GSF.Diagnostics;

namespace CTP.Net
{
    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public partial class CtpServer
    {
        /// <summary>
        /// Responsible for processing new client connections for the <see cref="CtpServer"/>. Use <see cref="AcceptAsync"/>. This method will
        /// callback <see cref="OnSessionCompleted"/> once a connection has been successful.
        /// </summary>
        private class ProcessClient
        {
            private readonly LogPublisher Log;

            //ToDo: For now, a new background thread is started that synchronously authenticates a new client.
            //      There may be opportunity to make this async one day.

            private readonly CtpServer m_server;
            private readonly TcpClient m_client;

            private ProcessClient(CtpServer server, TcpClient client)
            {
                using (Logger.AppendStackMessages("Remote Client", client.Client.RemoteEndPoint.ToString()))
                    Log = Logger.CreatePublisher(typeof(ProcessClient), MessageClass.Framework);

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
                    SslStream ssl = null;
                    if (config.EnableSSL)
                    {
                        ssl = new SslStream(netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                        ssl.AuthenticateAsServer(config.ServerCertificate, true, SslProtocols.Tls12, false);
                    }

                    var session = new CtpNetStream(socket, netStream, ssl);
                    var packet = session.Read();

                    Log.Publish(MessageLevel.Info, "Auth Packet", packet.CommandName);

                    session.GrantedRoles = new HashSet<string>();
                    switch (packet.CommandName)
                    {
                        case "Auth":
                            var auth = (Auth)packet;
                            if (m_server.m_config.CertificateClients.TryGetValue(auth.CertificateThumbprint, out var clientCert))
                            {
                                if (auth.ValidateSignature(clientCert.Certificate))
                                {
                                    var ticket = new AuthorizationTicket(auth.Ticket);
                                    if (string.IsNullOrEmpty(ticket.ApprovedPublicKey) || session.RemoteCertificate.GetPublicKeyString() == ticket.ApprovedPublicKey
                                        && (ticket.ValidFrom < DateTime.UtcNow && DateTime.UtcNow <= ticket.ValidTo))
                                    {
                                        session.AccountName = clientCert.ClientCert.MappedAccount;
                                        session.LoginName = ticket.LoginName;
                                        if (m_server.m_config.Accounts.ContainsKey(session.AccountName))
                                        {
                                            if (ticket.Roles == null)
                                            {
                                                session.GrantedRoles.UnionWith(m_server.m_config.Accounts[session.AccountName]);
                                            }
                                            else
                                            {
                                                session.GrantedRoles.UnionWith(m_server.m_config.Accounts[session.AccountName].Union(ticket.Roles));
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        case "AuthNone":
                            session.LoginName = "";
                            foreach (var item in m_server.m_config.AnonymousMappings)
                            {
                                var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                                if (item.Key.IsMatch(ipBytes))
                                {
                                    session.AccountName = item.Value;
                                    if (m_server.m_config.Accounts.ContainsKey(session.AccountName))
                                    {
                                        session.GrantedRoles.UnionWith(m_server.m_config.Accounts[session.AccountName]);
                                    }

                                    break;
                                }
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                    m_server.OnSessionCompleted(session);
                }
                catch (Exception e)
                {
                    Logger.SwallowException(e, "Failed to authenticate client, Safely Disconnecting");
                    //Swallow the exception since a failed connection attempt can safely be ignored by the server.
                }
            }

            private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
            {
                //Always accept any client certificate, the ticket is used to authenticate it.
                return true;
            }

            public static void AcceptAsync(CtpServer server, TcpClient client)
            {
                var pc = new ProcessClient(server, client);
                pc.Start();
            }

        }
    }
}