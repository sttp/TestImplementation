using System;
using GSF;

namespace CTP.Net
{
    public interface IRequestHandler
    {
        void OnNewPayload(CtpRequest request, byte[] payload);
        void OnDocument(CtpRequest request, CtpDocument payload);
    }

    public class CtpRequest : IDisposable
    {
        /// <summary>
        /// An object that maintains state information for this request.
        /// </summary>
        public object State;
        
        /// <summary>
        /// A handler for this request.
        /// </summary>
        public IRequestHandler Handler;

        public readonly uint RequestID;

        private CtpEncoder m_encoder;

        public ShortTime LastSentTime { get; private set; }
        public ShortTime LastReceiveTime { get; private set; }

        internal CtpRequest(uint requestID, CtpEncoder encoder)
        {
            LastSentTime = ShortTime.Now;
            LastReceiveTime = LastSentTime;
            RequestID = requestID;
            m_encoder = encoder;
        }

        internal void OnNewData(CtpDecoderResults results)
        {
            if ((results.ContentFlags & CtpContentFlags.IsDocument) == 0)
            {
                Handler.OnNewPayload(this, results.Payload);
            }
            else
            {
                Handler.OnDocument(this, new CtpDocument(results.Payload));
            }
        }

        public void SendPayload(byte[] payload)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, RequestID, payload);
            LastSentTime = ShortTime.Now;
        }

        public void SendDocument(CtpDocument document)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, RequestID, document.ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void SendDocument(DocumentObject document)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, RequestID, document.ToDocument().ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void Dispose()
        {

        }
    }
}