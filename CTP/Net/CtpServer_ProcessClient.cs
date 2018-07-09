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
using System.Security.Principal;
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
                    m_server.Authentication.AuthenticateSession(m_session);

                    var doc = ReadDocument();
                    switch (doc.RootElement)
                    {
                        case "AuthSrp":
                            AuthSrp((Auth)doc);
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

            private void AuthSrp(Auth command)
            {
                SrpCredential<SrpUserMapping> credential = m_server.Authentication.LookupCredential(command);
                SrpConstants param;
                BigInteger verifier;
                BigInteger privateB;
                BigInteger publicB;

                param = SrpConstants.Lookup(credential.Verifier.SrpStrength);
                verifier = credential.Verifier.VerifierCode.ToUnsignedBigInteger();
                privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
                publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

                WriteDocument(new AuthResponse(credential.Verifier.SrpStrength, credential.Verifier.Salt, publicB.ToUnsignedByteArray()));

                var clientProof = (AuthClientProof)ReadDocument();
                var publicA = clientProof.PublicA.ToUnsignedBigInteger();
                var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);
                var privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_ssl?.LocalCertificate);

                var proof = clientProof.Decrypt(privateSessionKey);

                m_session.LoginName = credential.Token.LoginName;
                m_session.SessionToken = proof.UserToken;
                if (m_session.GrantedRoles == null)
                    m_session.GrantedRoles.UnionWith(credential.Token.Roles);
                else
                    m_session.GrantedRoles.UnionWith(credential.Token.Roles.Intersect(proof.RequestedAccess));
                WriteDocument(new AuthServerProof(proof.ServerProof));
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