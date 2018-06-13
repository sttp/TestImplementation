using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class CtpSocket : IDisposable
    {
        public readonly byte ChannelID;
        private CtpEncoder m_encoder;
        private Action m_onDispose;

        public event Action<CtpReadResults> NewData;

        internal CtpSocket(Action onDispose, byte channelID, CtpEncoder encoder)
        {
            m_onDispose = onDispose;
            ChannelID = channelID;
            m_encoder = encoder;
        }

        internal void ProcessRead(CtpReadResults readResults)
        {
            NewData(readResults);
        }

        public void SendStream(byte[] data, int offset, int length)
        {
            m_encoder.Send(CtpChannelCode.Stream, ChannelID, data, offset, length);
        }

        public void SendPayload(byte[] payload)
        {
            SendPayload(payload, 0, payload.Length);
        }

        public void SendPayload(byte[] payload, int offset, int length)
        {
            m_encoder.Send(CtpChannelCode.Block, ChannelID, payload, offset, length);
        }

        public void SendDocument(CtpDocument command)
        {
            m_encoder.Send(CtpChannelCode.Document, ChannelID, command.ToArray());
        }

        public void Dispose()
        {
            m_onDispose?.Invoke();
            m_onDispose = null;
        }
    }

    public class CtpCommandStream
    {
        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];
        private CtpSocket[] m_sockets = new CtpSocket[256];
        private bool m_isClient;
        private Action<CtpSocket> m_onNewInboundSession;

        private WaitCallback m_asyncRead;
        private AsyncCallback m_asyncReadCallback;

        public CtpCommandStream(Stream session, bool isClient, Action<CtpSocket> onNewInboundSession)
        {
            m_onNewInboundSession = onNewInboundSession;
            m_isClient = isClient;
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

        public CtpSocket StartNewSession()
        {
            int start = 2;
            if (m_isClient)
            {
                start = 1;
            }

            for (int x = start; x < m_sockets.Length; x += 2)
            {
                if (m_sockets[x] == null)
                {
                    m_sockets[x] = new CtpSocket(() => m_sockets[x] = null, (byte)x, m_encoder);
                    return m_sockets[x];
                }
            }
            throw new Exception("Out of channels, too many simultaneous streams over a single socket.");
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

            while (m_packetDecoder.ReadCommand())
            {
                if (m_packetDecoder.Results.ChannelNumber == 0)
                {
                    //Handle differently
                }
                else if (m_sockets[m_packetDecoder.Results.ChannelNumber] == null)
                {
                    int channelNumber = m_packetDecoder.Results.ChannelNumber;
                    if (m_isClient)
                    {
                        if ((channelNumber & 1) == 0)
                            throw new Exception("Invalid channel number");
                    }
                    else
                    {
                        if ((channelNumber & 1) == 1)
                            throw new Exception("Invalid channel number");
                    }
                    m_sockets[m_packetDecoder.Results.ChannelNumber] = new CtpSocket(() => m_sockets[channelNumber] = null, m_packetDecoder.Results.ChannelNumber, m_encoder);
                    m_onNewInboundSession(m_sockets[m_packetDecoder.Results.ChannelNumber]);
                }
                m_sockets[m_packetDecoder.Results.ChannelNumber].ProcessRead(m_packetDecoder.Results);
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
