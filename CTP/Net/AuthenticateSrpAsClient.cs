using System;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading;
using CTP.SRP;

namespace CTP.Net
{
    public class AuthenticateSrpAsClient : CtpCommandHandlerBase
    {
        private int m_state = 0;
        private CtpSession m_stream;
        private readonly string m_identity;
        private readonly SecureString m_password;
        public byte[] privateSessionKey;
        private byte[] challengeServer;
        public ManualResetEvent Wait = new ManualResetEvent(false);

        public AuthenticateSrpAsClient(CtpSession stream, string identity, SecureString password)
        {
            m_stream = stream;
            m_password = password;
            m_identity = identity.Normalize(NormalizationForm.FormKC).Trim().ToLower();
        }

        public void Start()
        {
            m_state = 1;
            m_stream.SendCommand(0, new SrpIdentity(m_identity));
        }

        public override CtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command)
        {
            switch (m_state)
            {
                case 1:
                    m_state++;
                    SrpIdentityLookup lookup = (SrpIdentityLookup)command;

                    var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
                    var strength = (SrpStrength)lookup.SrpStrength;
                    var publicB = lookup.PublicB.ToUnsignedBigInteger();
                    var param = SrpConstants.Lookup(strength);
                    var publicA = BigInteger.ModPow(param.g, privateA, param.N);

                    var x = lookup.ComputePassword(m_identity, m_password);
                    var verifier = param.g.ModPow(x, param.N);

                    var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                    var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
                    var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
                    var sessionKey = base1.ModPow(exp1, param.N);
                    challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    byte[] clientChallenge = challengeClient;

                    session.SendCommand(0, new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge));
                    return this;
                case 2:
                    m_state++;

                    SrpServerResponse cr = (SrpServerResponse)command;
                    if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                        throw new Exception("Failed server challenge");
                    Wait.Set();
                    return null;
                default:
                    throw new NotSupportedException();
            }
        }

        public override void Cancel()
        {

        }
    }
}