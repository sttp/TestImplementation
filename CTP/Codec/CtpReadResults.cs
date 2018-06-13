using System;

namespace CTP
{
    public class CtpReadResults
    {
        private bool m_isValid;
        private byte[] m_payload;
        private CtpChannelCode m_channelCode;

        public void SetInvalid()
        {
            m_channelCode = CtpChannelCode.Protocol;
            m_isValid = false;
            m_payload = null;
        }

        internal void SetRaw(CtpChannelCode channelCode, byte[] payload)
        {
            m_channelCode = channelCode;
            m_isValid = true;
            m_payload = payload;
        }

        /// <summary>
        /// Indicates if a command has successfully been decoded. 
        /// This is equal to the return value of the most recent 
        /// <see cref="CtpDecoder.ReadCommand"/> method call.
        /// </summary>
        public bool IsValid => m_isValid;

        public CtpReadResults Clone()
        {
            return (CtpReadResults)MemberwiseClone();
        }

        /// <summary>
        /// Valid if <see cref="CtpDecoder.ReadCommand"/> returned true. And the command 
        /// This is the command that was decoded.
        /// </summary>
        public CtpDocument DocumentPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return new CtpDocument(m_payload);
            }
        }

        /// <summary>
        /// Valid if <see cref="CtpDecoder.ReadCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public byte[] Payload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return m_payload;
            }
        }
    }
}