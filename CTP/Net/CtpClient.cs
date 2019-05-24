using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using GSF.Diagnostics;

namespace CTP.Net
{
    /// <summary>
    /// A client that can connect to a <see cref="CtpServer"/>. Call <see cref="Connect"/> to grab a <see cref="CtpNetStream"/>.
    /// <see cref="Connect"/> can be called multiple times to create new connections and either blocks, errors, or times out.
    /// </summary>
    public class CtpClient
    {
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);
        private readonly IPEndPoint m_remoteEndpoint;
        private readonly IClientAuthentication m_authentication;

        /// <summary>
        /// Creates a means of connecting to a <see cref="CtpServer"/>.
        /// </summary>
        /// <param name="host">The host IP</param>
        /// <param name="authentication">The authentication ticket to use. Specify null to not use SSL.</param>
        public CtpClient(IPEndPoint host, IClientAuthentication authentication = null)
        {
            m_remoteEndpoint = host;
            m_authentication = authentication ?? new ClientAuthenticationNone();
        }

        public CtpNetStream Connect()
        {
            Log.Publish(MessageLevel.Info, "Attempting Connection");

            var socket = new TcpClient();
            socket.SendTimeout = 3000;
            socket.ReceiveTimeout = 3000;
            var connect = socket.BeginConnect(m_remoteEndpoint.Address, m_remoteEndpoint.Port, null, null);
            var success = connect.AsyncWaitHandle.WaitOne(3000);
            if (!success)
            {
                socket.Close();
                throw new TimeoutException("Failed to connect");
            }
            socket.EndConnect(connect);
            var netStream = socket.GetStream();

            SslStream sslStream = null;
            using (var handshake = m_authentication.StartHandshake())
            {
                if (handshake.UseSSL)
                {
                    sslStream = new SslStream(netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                    sslStream.AuthenticateAsClient(string.Empty, handshake.GetCertificateCollection(), SslProtocols.Tls12, false);
                    var session = new CtpNetStream(socket, netStream, sslStream);

                    var cmd = session.Read();
                    if (cmd.CommandName == "ServerDone")
                    {
                        if (!handshake.IsCertificateTrusted(sslStream.RemoteCertificate, (ServerDone)cmd))
                        {
                            m_authentication.AuthenticationFailed();
                            session.Send(new AuthFailure("Negotiated Certificate is not trusted: " + sslStream.RemoteCertificate.ToString()));
                            session.Dispose();
                            throw new Exception("Certificate is not trusted");
                        }
                    }
                    else if (cmd.CommandName == "CertificateProof")
                    {
                        if (!handshake.IsCertificateTrusted(sslStream.RemoteCertificate, (CertificateProof)cmd))
                        {
                            m_authentication.AuthenticationFailed();
                            session.Send(new AuthFailure("Negotiated Certificate is not trusted: " + sslStream.RemoteCertificate.ToString()));
                            session.Dispose();
                            throw new Exception("Certificate is not trusted");
                        }
                    }
                    else
                    {
                        m_authentication.AuthenticationFailed();
                        session.Send(new AuthFailure("Unrecognized Auth Command: " + cmd.CommandName));
                        session.Dispose();
                        throw new Exception("Unrecognized Auth Command: " + cmd.CommandName);
                    }

                    if (handshake.IsEphemeralCertificate)
                    {
                        session.Send(handshake.GetCertificateProof());
                    }
                    else
                    {
                        session.Send(handshake.GetClientDone());
                    }

                    cmd = session.Read();
                    if (cmd.CommandName != "AuthSuccess")
                    {
                        m_authentication.AuthenticationFailed();
                        session.Send(new AuthFailure("Expecting AuthSuccess, received: " + cmd.CommandName));
                        session.Dispose();
                        throw new Exception("Expecting AuthSuccess, received: " + cmd.CommandName);
                    }
                    Log.Publish(MessageLevel.Info, "SSL Session Completed");
                    return session;
                }
                else
                {
                    var session = new CtpNetStream(socket, netStream, null);
                    Log.Publish(MessageLevel.Info, "Non SSL Session Completed");
                    return session;
                }
            }
        }

        bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
