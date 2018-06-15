using GSF;
using System;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class CtpCommandStream
    {
        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];

        private WaitCallback m_asyncRead;
        private AsyncCallback m_asyncReadCallback;

        public ShortTime LastSentTime { get; private set; }
        public ShortTime LastReceiveTime { get; private set; }

        private Action<CtpDocument> m_processCommand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="processCommand"></param>
        public CtpCommandStream(Stream session, Action<CtpDocument> processCommand)
        {
            m_processCommand = processCommand;
            m_packetDecoder = new CtpDecoder();
            m_encoder = new CtpEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
            m_stream = session;
            m_asyncRead = AsyncRead;
            m_asyncReadCallback = AsyncReadCallback;
        }

        public void Start()
        {
            AsyncRead(null);
        }

        public void SendCommand(byte[] payload)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, payload);
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(CtpDocument document)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, document.ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(DocumentObject document)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | CtpContentFlags.InitialRequest, document.ToDocument().ToArray());
            LastSentTime = ShortTime.Now;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            m_stream.Write(data, position, length);
        }

        private void AsyncRead(object obj)
        {
            lock (m_syncReceive)
            {
                if (!m_isReading)
                {
                    m_isReading = true;
                    m_stream.BeginRead(m_inBuffer, 0, m_inBuffer.Length, m_asyncReadCallback, null);
                }
            }
        }

        private void AsyncReadCallback(IAsyncResult ar)
        {
            lock (m_syncReceive)
            {
                m_isReading = false;
                int length = m_stream.EndRead(ar);
                m_packetDecoder.FillBuffer(m_inBuffer, 0, length);
            }
            LastReceiveTime = ShortTime.Now;
            while (m_packetDecoder.ReadCommand())
            {
                m_processCommand(new CtpDocument(m_packetDecoder.Results.Payload));
            }
            if (ar.CompletedSynchronously)
            {
                ThreadPool.QueueUserWorkItem(m_asyncRead);
            }
            else
            {
                AsyncRead(null);
            }
        }
    }
}
