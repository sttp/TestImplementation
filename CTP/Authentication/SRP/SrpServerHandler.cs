using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CTP.Net;

namespace CTP.SRP
{
    public class SrpServerHandler<T>
    {
        private Action<byte[], T> m_userAuthenticated;
        private int m_state = 0;
        private CtpSession m_stream;
        private SrpUserCredential<T> m_user;
        private byte[] privateSessionKey;
        private SrpConstants param;
        private BigInteger verifier;
        private BigInteger privateB;
        private BigInteger publicB;

        public SrpServerHandler(CtpSession stream, SrpUserCredential<T> user, Action<byte[], T> userAuthenticated)
        {
            m_stream = stream;
            m_user = user;
            m_userAuthenticated = userAuthenticated;
        }

        public void Start()
        {
            m_state = 1;
            param = SrpConstants.Lookup(m_user.Verifier.SrpStrength);
            verifier = m_user.Verifier.Verification.ToUnsignedBigInteger();
            privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
            m_stream.SendCommand(new SrpIdentityLookup(m_user.Verifier.SrpStrength, m_user.Verifier.Salt, publicB.ToUnsignedByteArray(), m_user.Verifier.IterationCount));
        }

        public void ProcessCommand(CtpSession session, SrpClientResponse command)
        {
            switch (m_state)
            {
                case 1:
                    m_state++;
                    var clientResponse = command;

                    var publicA = clientResponse.PublicA.ToUnsignedBigInteger();
                    byte[] clientChallenge = clientResponse.ClientChallenge;

                    var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                    var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

                    var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);
                    var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);
                    privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);

                    if (!challengeClient.SequenceEqual(clientChallenge))
                        throw new Exception("Failed client challenge");
                    byte[] serverChallenge = challengeServer;

                    m_userAuthenticated(privateSessionKey, m_user.Token);
                    m_stream.SendCommand(new SrpServerResponse(serverChallenge));
                    return;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}