using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using CTP.SRP;
using GSF;

namespace CTP.Net
{
    public class SrpAuthClient
    {
        public static ClientResumeTicket Authenticate(NetworkCredential credentials, CtpStream stream, SslStream sslStream)
        {
            WriteDocument(stream, new Auth(credentials.UserName, false, false));
            AuthResponse authResponse = (AuthResponse)ReadDocument(stream);
            var credentialName = credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            var privateA = Security.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)authResponse.BitStrength;
            var publicB = authResponse.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = authResponse.ComputeX(credentialName, credentials.SecurePassword);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, sslStream?.RemoteCertificate);
            var proof = new AuthClientProof(publicA.ToByteArray(), Security.ComputeHMAC(privateSessionKey, "Client Proof"), null, null);
            WriteDocument(stream, proof);
            AuthServerProof cr = (AuthServerProof)ReadDocument(stream);

            byte[] serverProof = Security.ComputeHMAC(privateSessionKey, "Server Proof");
            if (!serverProof.SequenceEqual(cr.ServerProof))
                throw new Exception("Failed server challenge");

            if ((cr.EncryptedTicketSigningKey?.Length ?? 0) > 0)
            {
                return cr.CreateResumeTicket(privateSessionKey, credentials.UserName, cr.Roles);
            }

            return null;
        }

        public static RotateKeys RotateKeys(NetworkCredential credentials, CtpStream stream, SslStream sslStream)
        {
            WriteDocument(stream, new Auth(credentials.UserName, true, false));
            AuthResponse authResponse = (AuthResponse)ReadDocument(stream);
            var credentialName = credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            var privateA = Security.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)authResponse.BitStrength;
            var publicB = authResponse.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = authResponse.ComputeX(credentialName, credentials.SecurePassword);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, sslStream?.RemoteCertificate);
            var proof = new AuthClientProof(publicA.ToByteArray(), Security.ComputeHMAC(privateSessionKey, "Client Proof"), null, null);
            WriteDocument(stream, proof);
            AuthServerProof cr = (AuthServerProof)ReadDocument(stream);

            byte[] serverProof = Security.ComputeHMAC(privateSessionKey, "Server Proof");
            if (!serverProof.SequenceEqual(cr.ServerProof))
                throw new Exception("Failed server challenge");

            return new RotateKeys(Security.ComputeHMAC(privateSessionKey, "Key X"), Security.ComputeHMAC(privateSessionKey, "Key Salt"), sslStream?.RemoteCertificate);
        }

        public static void ChangePassword(NetworkCredential credentials, CtpStream stream, SslStream sslStream, SecureString newPassword)
        {
            WriteDocument(stream, new Auth(credentials.UserName, false, true));
            AuthResponse authResponse = (AuthResponse)ReadDocument(stream);

            var credentialName = credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            var privateA = Security.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)authResponse.BitStrength;
            var publicB = authResponse.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = authResponse.ComputeX(credentialName, credentials.SecurePassword);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, sslStream?.RemoteCertificate);

            byte[] clientPassword;
            byte[] dataToEncrypt;

            if (authResponse.RequiresPassword)
            {
                var ms = new MemoryStream();
                byte[] pwd = newPassword.ToUTF8();
                ms.WriteByte((byte)pwd.Length);
                ms.Write(pwd, 0, pwd.Length);
                ms.WriteByte(64);
                ms.Write(Security.CreateSalt(64), 0, 64);
                ms.Write(BigEndian.GetBytes((short)2048), 0, 2);
                dataToEncrypt = ms.ToArray();
            }
            else
            {
                var ms = new MemoryStream();

                byte[] salt = Security.CreateSalt(64);
                byte[] X = SrpMethods.ComputeX(salt, credentialName, newPassword);
                byte[] V = SrpMethods.ComputeV(SrpStrength.Bits2048, X);

                ms.Write(BigEndian.GetBytes((short)V.Length), 0, 2);
                ms.Write(V, 0, V.Length);
                ms.WriteByte(64);
                ms.Write(salt, 0, 64);
                ms.Write(BigEndian.GetBytes((short)2048), 0, 2);
                dataToEncrypt = ms.ToArray();

            }

            using (var aes = Aes.Create())
            {
                aes.IV = Security.ComputeHMAC(privateSessionKey, "Password IV", 16);
                aes.Key = Security.ComputeHMAC(privateSessionKey, "Password Key", 32);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var cipher = aes.CreateEncryptor())
                {
                    clientPassword = cipher.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                }
            }

            var proof = new AuthClientProof(publicA.ToByteArray(), Security.ComputeHMAC(privateSessionKey, "Client Proof"), clientPassword, Security.ComputeHMAC(privateSessionKey, clientPassword));


            WriteDocument(stream, proof);
            AuthServerProof cr = (AuthServerProof)ReadDocument(stream);

            byte[] serverProof = Security.ComputeHMAC(privateSessionKey, "Server Proof");
            if (!serverProof.SequenceEqual(cr.ServerProof))
                throw new Exception("Failed server challenge");
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