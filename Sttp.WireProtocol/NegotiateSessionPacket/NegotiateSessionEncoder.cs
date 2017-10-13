using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class NegotiateSessionEncoder
    {
        private Action<byte[], int, int> m_baseEncoder;

        public NegotiateSessionEncoder(Action<byte[], int, int> baseEncoder)
        {
            m_baseEncoder = baseEncoder;
        }

        public enum EncryptionOptions : byte
        {
            /// <summary>
            /// A TLS 1.2 channel, requiring only a client certificate
            /// </summary>
            TLS12Client,
            /// <summary>
            /// A TLS 1.2 channel, requiring only a server certificate
            /// </summary>
            TLS12Server,
            /// <summary>
            /// A TLS 1.2 requiring both client/server certificates
            /// </summary>
            TLS12Mutual,

            /// <summary>
            /// Negotiate Stream
            /// </summary>
            Windows,

            /// <summary>
            /// No encryption is used.
            /// </summary>
            None,
        }

        public enum AuthenticationOptions : byte
        {
            None,

            TrustedCertificates,

            Windows,
            /// <summary>
            /// A string based code used to mutually authenticate two endpoints.
            /// 
            /// Method:
            /// Server To Client: 32-byte nonce
            /// Client To Server: 32-byte nonce
            /// Client To Server: SHA256(ClientNonce | ServerNonce | ClientPublicKey | ServerPublicKey | PBKDF2(API KEY))
            /// Server To Client: SHA256(ServerNonce | ClientNonce | ServerPublicKey | ClientPublicKey | PBKDF2(API KEY))
            /// 
            /// Remarks:
            /// This method ensures against man-in-the-middle attacks for untrusted certificates.
            /// And if one does occur, authentication will fail and a brute force attacks will be much more difficult.
            /// 
            /// If not present, ClientPublicKey and/or ServerPublicKey can be left out. It requires only 1 to ensure against
            /// man in the middle attacks. 
            /// 
            /// If both are absent, this can still be used to authenticate a connection, you just won't know if someone is listening in.
            /// </summary>
            APIKey,
            LDAP,
        }

        public void SupportedEncryption(EncryptionOptions[] encryption)
        {
            
        }

        public void SelectEncryption(EncryptionOptions selectedOption)
        {
            
        }

        public void SupportedAuthentication(AuthenticationOptions[] authentication)
        {
            
        }

        public void SelectAuthentication(AuthenticationOptions authentication)
        {
            
        }

        public void SupportedVersions(ProtocolVersions protocolVersionNumber)
        {
            //Client connects to server and specifies it's protocol versions supported
        }

        public void SelectedModes(OperationalModes modes)
        {
            //The server replies with the supported operational modes
        }

        public void SecureUdpDataChannel(byte[] key, byte[] iv)
        {

        }

        
    }
}
