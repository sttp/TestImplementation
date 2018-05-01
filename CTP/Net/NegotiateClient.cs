using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace CTP.Net
{
    public class ClientToken
    {
        public readonly TcpClient Client;
        public readonly ConnectionOptions Options;
        public readonly NetworkStream NetStream;
        public SslStream SSL;
        public NegotiateStream Win;

        public ClientToken(TcpClient client, ConnectionOptions options)
        {
            Client = client;
            Options = options;
            NetStream = client.GetStream();
        }

        public bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            switch (Options.ServerAuthentication)
            {
                case ServerAuthenticationMode.None:
                    Options.IsServerTrusted = false;
                    return true;
                case ServerAuthenticationMode.TrustCertificate:
                    Options.IsServerTrusted = Options.TrustedCertificates.Contains(certificate);
                    return true;
                case ServerAuthenticationMode.TrustCA:
                    Options.IsServerTrusted = sslPolicyErrors == SslPolicyErrors.None;
                    return true;
                case ServerAuthenticationMode.TrustedSharedSecret:
                    Options.IsServerTrusted = false;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public class NegotiateClient
    {
        public void ProcessClient(ClientToken client)
        {
            switch (client.Options.Encryption)
            {
                case EncryptionMode.None:

                    break;
                case EncryptionMode.SSL:
                    if (client.Options.EncryptAsServer)
                    {
                        SSLAsServer(client);
                    }
                    else
                    {
                        SSLAsClient(client);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SSLAsServer(ClientToken client)
        {
            client.SSL = new SslStream(client.NetStream, false, client.UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
            client.SSL.BeginAuthenticateAsServer(client.Options.ServerCertificate, true, SslProtocols.Tls12, false, EndAuthenticateAsServer, client);
        }

        private void EndAuthenticateAsServer(IAsyncResult ar)
        {
            ClientToken client = (ClientToken)ar.AsyncState;
            try
            {
                client.SSL.EndAuthenticateAsServer(ar);
                client = client;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void SSLAsClient(ClientToken client)
        {
            client.SSL = new SslStream(client.NetStream, false, client.UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
            client.SSL.BeginAuthenticateAsClient(client.Options.ServerName, null, SslProtocols.Tls12, client.Options.SslCheckCertificateRevocation, EndAuthenticateAsClient, client);
        }

        private void EndAuthenticateAsClient(IAsyncResult ar)
        {
            ClientToken client = (ClientToken)ar.AsyncState;
            try
            {
                client.SSL.EndAuthenticateAsClient(ar);
                client = client;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void WinAsClient(ClientToken client)
        {
            client.Win = new NegotiateStream(client.NetStream, false);
            client.Win.BeginAuthenticateAsClient(CredentialCache.DefaultNetworkCredentials, client.Options.ServerName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, EndWinAsClient, client);
        }

        private void EndWinAsClient(IAsyncResult ar)
        {
            ClientToken client = (ClientToken)ar.AsyncState;
            try
            {
                client.Win.EndAuthenticateAsClient(ar);
                client = client;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void WinAsServer(ClientToken client)
        {
            client.Win = new NegotiateStream(client.NetStream, false);
            client.Win.BeginAuthenticateAsServer(CredentialCache.DefaultNetworkCredentials, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, EndWinAsServer, client);
        }

        private void EndWinAsServer(IAsyncResult ar)
        {
            ClientToken client = (ClientToken)ar.AsyncState;
            try
            {
                client.Win.EndAuthenticateAsServer(ar);
                client = client;
            }
            catch (Exception e)
            {
                throw;
            }
        }



    }

}


