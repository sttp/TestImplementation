﻿using System.Collections.Generic;

namespace CTP.Net
{
    [DocumentName("CtpServerConfig")]
    public class CtpServerConfig
        : DocumentObject<CtpServerConfig>
    {
        [DocumentField()]
        public List<CtpAccount> Accounts { get; set; }

        [DocumentField()]
        public List<CtpAnonymousMapping> AnonymousMappings { get; set; }

        [DocumentField()]
        public List<CtpClientCert> ClientCerts { get; set; }

        [DocumentField()]
        public bool EnableSSL;

        [DocumentField()]
        public string ServerCertificatePath { get; set; }

        public CtpServerConfig()
        {
            ClientCerts = new List<CtpClientCert>();
            Accounts = new List<CtpAccount>();
            AnonymousMappings = new List<CtpAnonymousMapping>();
        }

        public static explicit operator CtpServerConfig(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public void Validate()
        {


        }
    }
}