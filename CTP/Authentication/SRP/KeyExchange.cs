using System;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using CTP.SRP;

namespace CTP.Net
{
    public class KeyExchange
    {
        public static void AuthenticateAsClient(string accountName, string password, CtpStream stream, SslStream sslStream)
        {
            var param = SrpConstants.Lookup(SrpStrength.Bits4096);
            var privateA = Security.CreateSalt(32).ToUnsignedBigInteger();
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            WriteDocument(stream, new CertExchange(accountName, publicA.ToUnsignedByteArray()));

            CertExchangeResponse certExchangeResponse = (CertExchangeResponse)ReadDocument(stream);

            var publicB = certExchangeResponse.PublicB.ToUnsignedBigInteger();
            var x = certExchangeResponse.ComputeX(password, sslStream.LocalCertificate.GetPublicKey(), sslStream.RemoteCertificate.GetPublicKey());
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N).ToUnsignedByteArray();

            var proof = new CertExchangeClientProof(Security.ComputeHMAC(sessionKey, "Client Proof"));
            WriteDocument(stream, proof);
            CertExchangeServerProof cr = (CertExchangeServerProof)ReadDocument(stream);

            byte[] serverProof = Security.ComputeHMAC(sessionKey, "Server Proof");
            if (!serverProof.SequenceEqual(cr.ServerProof))
                throw new Exception("Failed server challenge");
        }

        public static void AuthenticateAsServer(CertExchange request, string password, CtpStream stream, SslStream sslStream)
        {
            var param = SrpConstants.Lookup(SrpStrength.Bits4096);

            BigInteger verifier;
            BigInteger privateB;
            BigInteger publicB;

            var publicA = request.PublicA.ToUnsignedBigInteger();

            var X = SrpMethods.ComputeX(sslStream.RemoteCertificate.GetPublicKey().Concat(sslStream.LocalCertificate.GetPublicKey()), password);
            verifier = SrpMethods.ComputeV(SrpStrength.Bits4096, X).ToUnsignedBigInteger();
            privateB = Security.CreateSalt(32).ToUnsignedBigInteger();
            publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

            WriteDocument(stream, new CertExchangeResponse(publicB.ToUnsignedByteArray()));

            var clientProof = (CertExchangeClientProof)ReadDocument(stream);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N).ToUnsignedByteArray();

            var cproof = Security.ComputeHMAC(sessionKey, "Client Proof");
            var sproof = Security.ComputeHMAC(sessionKey, "Server Proof");

            if (!clientProof.ClientProof.SequenceEqual(cproof))
            {
                throw new Exception("Authorization failed");
            }
            var serverProof = new CertExchangeServerProof(sproof);
            WriteDocument(stream, serverProof);
        }

        private static void WriteDocument(CtpStream stream, DocumentObject command)
        {
            stream.Send(0, command.ToDocument().ToArray());
        }

        private static CtpDocument ReadDocument(CtpStream stream)
        {
            stream.Read(-1);
            return new CtpDocument(stream.Results.Payload);
        }

    }
}