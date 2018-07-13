using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class RotateKeys
    {
        public byte[] X;
        public byte[] Salt;
        public X509Certificate RemoteCertificate;

        public RotateKeys(byte[] x, byte[] salt, X509Certificate remoteCertificate)
        {
            X = x;
            Salt = salt;
            RemoteCertificate = remoteCertificate;
        }
    }
}