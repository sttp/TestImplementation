using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class CtpSocket : IDisposable
    {
        public readonly uint ChannelID;
        private CtpEncoder m_encoder;
        private Action m_onDispose;
        private CtpContentFlags m_flags;
        public event Action<CtpReadResults> NewData;

        internal CtpSocket(bool isClientRequest, Action onDispose, uint channelID, CtpEncoder encoder)
        {
            if (isClientRequest)
                m_flags = CtpContentFlags.IsClientRequest;
            else
                m_flags = CtpContentFlags.IsServerRequest;

            m_onDispose = onDispose;
            ChannelID = channelID;
            m_encoder = encoder;
        }

        internal void ProcessRead(CtpReadResults readResults)
        {
            NewData(readResults);
        }

        public void SendPayload(byte[] payload)
        {
            SendPayload(payload, 0, payload.Length);
        }

        public void SendPayload(byte[] payload, int offset, int length)
        {
            m_encoder.Send(m_flags, ChannelID, payload, offset, length);
        }

        public void SendDocument(CtpDocument command)
        {
            m_encoder.Send(CtpContentFlags.IsDocument | m_flags, ChannelID, command.ToArray());
        }

        public void Dispose()
        {
            m_onDispose?.Invoke();
            m_onDispose = null;
        }
    }

    public class CtpCommandStream
    {
        private int m_sessionID;
        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];
        private Dictionary<uint, CtpSocket> m_clientRequests = new Dictionary<uint, CtpSocket>();
        private Dictionary<uint, CtpSocket> m_serverRequests = new Dictionary<uint, CtpSocket>();
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

        public CtpSocket BeginRequest()
        {
            var requests = m_isClient ? m_clientRequests : m_serverRequests;

            tryAgain:
            var sessionID = (uint)Interlocked.Increment(ref m_sessionID);

            if (requests.ContainsKey(sessionID))
                goto tryAgain;
            var request = new CtpSocket(m_isClient, () => requests.Remove(sessionID), sessionID, m_encoder);
            requests[sessionID] = request;
            return request;
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
                uint sessionID = m_packetDecoder.Results.RequestID;
                if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.IsClientRequest) != 0)
                {
                    if (m_clientRequests.TryGetValue(m_packetDecoder.Results.RequestID, out var socket))
                    {
                        socket.ProcessRead(m_packetDecoder.Results);
                    }
                    else
                    {
                        if (!m_isClient)
                        {
                            //If this request was made by the client, and I'm the server, begin a new request.
                            socket = new CtpSocket(!m_isClient, () => m_clientRequests.Remove(sessionID), sessionID, m_encoder);
                            m_clientRequests[sessionID] = socket;
                            m_onNewInboundSession(socket);
                        }
                    }

                }
                else if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.IsServerRequest) != 0)
                {
                    if (m_serverRequests.TryGetValue(m_packetDecoder.Results.RequestID, out var socket))
                    {
                        socket.ProcessRead(m_packetDecoder.Results);
                    }
                    else
                    {
                        if (m_isClient)
                        {
                            //If this request was made by the server, and I'm the client, begin a new request.
                            socket = new CtpSocket(m_isClient, () => m_serverRequests.Remove(sessionID), sessionID, m_encoder);
                            m_serverRequests[sessionID] = socket;
                            m_onNewInboundSession(socket);
                        }
                    }
                }
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
