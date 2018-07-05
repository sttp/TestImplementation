using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using CTP.SRP;
using GSF.IO;
using GSF.Security.Cryptography.X509;

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
                    bool requireSSL = m_server.DefaultRequireSSL;
                    bool hasAccess = m_server.DefaultAllowConnections;
                    X509Certificate serverCertificate = m_server.DefaultCertificate;

                    var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                    foreach (var item in m_server.m_encryptionOptions.Values)
                    {
                        if (item.IP.IsMatch(ipBytes))
                        {
                            requireSSL = item.RequireSSL;
                            hasAccess = item.HasAccess;
                            serverCertificate = item.ServerCertificate;
                            break;
                        }
                    }

                    if (!hasAccess)
                    {
                        throw new Exception("Client does not have access");
                    }

                    char mode = (char)netStream.ReadNextByte();
                    switch (mode)
                    {
                        case 'N':
                            if (requireSSL)
                            {
                                mode = '1';
                            }
                            else
                            {
                                mode = 'N';
                            }
                            break;
                        case '1':
                            requireSSL = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    netStream.WriteByte((byte)mode);
                    netStream.Flush();

                    CertificateTrustMode certificateTrust = CertificateTrustMode.None;

                    bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
                    {
                        if (sslpolicyerrors == SslPolicyErrors.None || sslpolicyerrors == SslPolicyErrors.RemoteCertificateNotAvailable)
                        {
                            certificateTrust = CertificateTrustMode.Native;
                        }
                        return true;
                    }

                    if (requireSSL)
                    {
                        serverCertificate = serverCertificate ?? EmphericalCertificate.Value;
                        m_ssl = new SslStream(netStream, false, UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
                        m_ssl.AuthenticateAsServer(serverCertificate, true, SslProtocols.Tls12, false);
                        m_finalStream = m_ssl;
                    }
                    else
                    {
                        m_finalStream = netStream;
                    }

                    m_ctpStream = new CtpStream();
                    m_ctpStream.SetActiveStream(m_finalStream);

                    m_session = new CtpSession(m_ctpStream, false, certificateTrust, socket, netStream, m_ssl);
                    m_server.Authentication.AuthenticateSession(m_session);

                    var doc = ReadDocument();
                    switch (doc.RootElement)
                    {
                        case "AuthSrp":
                            AuthSrp((AuthSrp)doc);
                            break;
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

            private void AuthSrp(AuthSrp command)
            {
                var items = m_server.Authentication.FindSrpUser(command);

                int m_state = 0;
                SrpUserCredential<SrpUserMapping> m_user = items;
                byte[] privateSessionKey;
                SrpConstants param;
                BigInteger verifier;
                BigInteger privateB;
                BigInteger publicB;

                param = SrpConstants.Lookup(m_user.Verifier.SrpStrength);
                verifier = m_user.Verifier.Verification.ToUnsignedBigInteger();
                privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
                publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

                WriteDocument(new SrpIdentityLookup(m_user.Verifier.SrpStrength, m_user.Verifier.Salt, publicB.ToUnsignedByteArray(), m_user.Verifier.IterationCount));

                var clientResponse = (SrpClientResponse)ReadDocument();

                var publicA = clientResponse.PublicA.ToUnsignedBigInteger();
                byte[] clientChallenge = clientResponse.ClientChallenge;

                var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

                var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);
                var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);
                privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);

                if (!challengeClient.SequenceEqual(clientChallenge))
                    throw new Exception("Failed client challenge");
                byte[] serverChallenge = challengeServer;


                m_session.LoginName = m_user.Token.LoginName;
                m_session.GrantedRoles.UnionWith(m_user.Token.Roles);
                WriteDocument(new SrpServerResponse(serverChallenge));
            }

            private void WriteDocument(DocumentObject command)
            {
                m_ctpStream.Send(0, command.ToDocument().ToArray());
            }

            private CtpDocument ReadDocument()
            {
                m_ctpStream.Read(-1);
                return new CtpDocument(m_ctpStream.Results.Payload);
            }


        }
    }
}