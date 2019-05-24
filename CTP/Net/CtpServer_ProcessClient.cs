using System;
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
                    IServerHandshake config = m_server.m_config.StartHandshake();
                    CtpNetStream session;

                    if (config.UseSSL)
                    {
                        var ssl = new SslStream(netStream, false, UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
                        ssl.AuthenticateAsServer(config.GetCertificate(), true, SslProtocols.Tls12, false);
                        session = new CtpNetStream(socket, netStream, ssl);

                        if (config.IsEphemeralCertificate)
                        {
                            session.Send(config.GetCertificateProof());
                        }
                        else
                        {
                            session.Send(config.GetServerDone());
                        }

                        var cmd = session.Read();

                        if (cmd.CommandName == "ClientDone")
                        {
                            if (!config.IsCertificateTrusted(session, (ClientDone)cmd))
                            {
                                session.Send(new AuthFailure("Negotiated Certificate is not trusted: " + ssl.RemoteCertificate.ToString()));
                                session.Dispose();
                                throw new Exception("Certificate is not trusted");
                            }
                        }
                        else if (cmd.CommandName == "CertificateProof")
                        {
                            if (!config.IsCertificateTrusted(session, (CertificateProof)cmd))
                            {
                                session.Send(new AuthFailure("Negotiated Certificate is not trusted: " + ssl.RemoteCertificate.ToString()));
                                session.Dispose();
                                throw new Exception("Certificate is not trusted");
                            }
                        }
                        else
                        {
                            session.Send(new AuthFailure("Unrecognized Auth Command: " + cmd.CommandName));
                            session.Dispose();
                            throw new Exception("Unrecognized Auth Command: " + cmd.CommandName);
                        }
                        session.Send(new AuthSuccess());
                    }
                    else
                    {
                        session = new CtpNetStream(socket, netStream, null);
                    }

                    m_server.OnSessionCompleted(session);

                }
                catch (Exception e)
                {
                    Logger.SwallowException(e, "Failed to authenticate client, Safely Disconnecting");
                    //Swallow the exception since a failed connection attempt can safely be ignored by the server.
                }
            }

            private bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
            {
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