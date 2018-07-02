using System;

namespace CTP
{
    public class CtpDecoderResults
    {
        private bool m_isValid;
        private byte m_payloadKind;
        private byte[] m_payload;

        public void SetInvalid()
        {
            m_isValid = false;
            m_payload = null;
        }

        internal void SetRaw(byte payloadKind, byte[] payload)
        {
            m_isValid = true;
            m_payloadKind = payloadKind;
            m_payload = payload;
        }

        /// <summary>
        /// Indicates if a command has successfully been decoded. 
        /// This is equal to the return value of the most recent 
        /// <see cref="CtpDecoder.ReadCommand"/> method call.
        /// </summary>
        public bool IsValid => m_isValid;

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

        public byte PayloadKind
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return m_payloadKind;
            }
        } 

    }
}