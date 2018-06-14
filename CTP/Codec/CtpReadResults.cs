using System;

namespace CTP
{
    public class CtpReadResults
    {
        private bool m_isValid;
        private uint m_requestID;
        private byte[] m_payload;
        private CtpContentFlags m_contentFlags;

        public void SetInvalid()
        {
            m_contentFlags = CtpContentFlags.None;
            m_isValid = false;
            m_payload = null;
        }

        internal void SetRaw(CtpContentFlags contentFlags, uint requestID, byte[] payload)
        {
            m_contentFlags = contentFlags;
            m_isValid = true;
            m_requestID = requestID;
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

        public CtpContentFlags ContentFlags
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return m_contentFlags;
            }
        }

        public uint RequestID
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return m_requestID;
            }
        }

    }
}