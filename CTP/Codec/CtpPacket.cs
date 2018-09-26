using System;

namespace CTP
{
    /// <summary>
    /// A packet that can be serialized.
    /// </summary>
    public class CtpPacket
    {
        public readonly byte Channel;
        public readonly bool IsRawData;
        public readonly byte[] Payload;

        public CtpPacket(byte channel, bool isRawData, byte[] payload)
        {
            if (channel > 16)
                throw new ArgumentOutOfRangeException(nameof(channel), "Cannot be greater than 16");
            Channel = channel;
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            IsRawData = isRawData;
        }

        public CtpPacket(byte channel, CtpDocument document)
        {
            if (channel > 16)
                throw new ArgumentOutOfRangeException(nameof(channel), "Cannot be greater than 16");
            if (document.RootElement == "Raw")
            {
                var raw = (CtpRaw)document;
                IsRawData = true;
                Payload = raw.Payload;
            }
            else
            {
                IsRawData = false;
                Payload = document.ToArray();
            }
        }

        public CtpPacket(byte channel, DocumentObject document)
        {
            if (channel > 16)
                throw new ArgumentOutOfRangeException(nameof(channel), "Cannot be greater than 16");
            if (document is CtpRaw)
            {
                var raw = (CtpRaw)document;
                IsRawData = true;
                Payload = raw.Payload;
            }
            else
            {
                IsRawData = false;
                Payload = document.ToDocument().ToArray();
            }
        }

    }
}