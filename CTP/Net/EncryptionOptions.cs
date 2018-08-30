﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class EncryptionOptions : IComparable<EncryptionOptions>, IComparable, IEquatable<EncryptionOptions>
    {
        public IpMatchDefinition IP;
        public X509Certificate ServerCertificate;
        public bool EnableSSL => ServerCertificate != null;

        public EncryptionOptions(IpMatchDefinition ip, X509Certificate localCertificate)
        {
            IP = ip;
            ServerCertificate = localCertificate;
        }

        public int CompareTo(EncryptionOptions other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (ReferenceEquals(null, other))
                return 1;
            return IP.CompareTo(other.IP);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (ReferenceEquals(this, obj))
                return 0;
            if (!(obj is EncryptionOptions))
                throw new ArgumentException($"Object must be of type {nameof(EncryptionOptions)}");
            return CompareTo((EncryptionOptions)obj);
        }

        public static bool operator <(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(EncryptionOptions left, EncryptionOptions right)
        {
            return Comparer<EncryptionOptions>.Default.Compare(left, right) >= 0;
        }

        public bool Equals(EncryptionOptions other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return IP.Equals(other.IP);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((EncryptionOptions)obj);
        }

        public override int GetHashCode()
        {
            return IP.GetHashCode();
        }

        public static bool operator ==(EncryptionOptions left, EncryptionOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EncryptionOptions left, EncryptionOptions right)
        {
            return !Equals(left, right);
        }
    }
}