using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sttp.Transport
{
    public class UdpTransport
    {
        public void Key()
        {
            var p = new RSAParameters();
            p.Exponent = new byte[0];
            p.D = new byte[0];

            using (var e = new RSACryptoServiceProvider())
            {
                e.ImportParameters(p);
            }
        }

    }
}
