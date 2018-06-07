using System;
using CTP;
using CTP.Net;

namespace CTP
{
    public class CtpReadResults
    {
        private CommandCode m_commandCode;
        private CtpDocument m_documentPayload;
        private byte[] m_binaryCommandPayload;
        private int m_binaryChannelCode;

        public void SetInvalid()
        {
            m_commandCode = CommandCode.Invalid;
            m_documentPayload = null;
            m_binaryCommandPayload = null;
            m_binaryChannelCode = 0;
        }

        public void SetDocument(CtpDocument document)
        {
            m_commandCode = CommandCode.Document;
            m_documentPayload = document;
        }

        public void SetRaw(int channelCode, byte[] payload)
        {
            m_commandCode = CommandCode.Binary;
            m_binaryChannelCode = channelCode;
            m_binaryCommandPayload = payload;
        }

        /// <summary>
        /// Indicates if a command has successfully been decoded. 
        /// This is equal to the return value of the most recent 
        /// <see cref="CtpDecoder.ReadCommand"/> method call.
        /// </summary>
        public bool IsValid => m_commandCode != CommandCode.Invalid;

        /// <summary>
        /// Indicates what kind of commmand was decoded.
        /// </summary>
        public CommandCode CommandCode => m_commandCode;

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
                if (m_commandCode != CommandCode.Document)
                    throw new InvalidOperationException("Command is not a Document Command.");
                return m_documentPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="CtpDecoder.ReadCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public byte[] BinaryPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_commandCode != CommandCode.Binary)
                    throw new InvalidOperationException("Command is not a Binary Command.");
                return m_binaryCommandPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="CtpDecoder.ReadCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public int BinaryChannelID
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_commandCode != CommandCode.Binary)
                    throw new InvalidOperationException("Command is not a Binary Command.");
                return m_binaryChannelCode;
            }
        }

    }
}