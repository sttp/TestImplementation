using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class CtpCommandStream
    {
        private int m_requestID;
        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];
        private Dictionary<uint, CtpRequest> m_activeRequests = new Dictionary<uint, CtpRequest>();
        private Dictionary<string, ICtpRootHandler> m_rootHandlers = new Dictionary<string, ICtpRootHandler>();
        private bool m_isClient;

        private WaitCallback m_asyncRead;
        private AsyncCallback m_asyncReadCallback;

        public CtpCommandStream(Stream session, bool isClient)
        {
            m_isClient = isClient;
            if (m_isClient)
                m_requestID = 0;
            else
                m_requestID = 1;

            m_packetDecoder = new CtpDecoder();
            m_encoder = new CtpEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
            m_stream = session;
            m_asyncRead = AsyncRead;
            m_asyncReadCallback = AsyncReadCallback;
        }

        public void AddHandler(ICtpRootHandler rootHandler)
        {
            foreach (var commands in rootHandler.RootCommands())
            {
                m_rootHandlers.Add(commands, rootHandler);
            }
        }

        public void Start()
        {
            AsyncRead(null);
        }

        public void BeginRequest(CtpDocument document, IRequestHandler handler, object state)
        {
            var req = BeginRequest();
            req.Handler = handler;
            req.State = state;
            req.SendDocument(document);
        }

        public CtpRequest BeginRequest()
        {
            tryAgain:
            var requestID = (uint)Interlocked.Add(ref m_requestID, 2);

            if (m_activeRequests.ContainsKey(requestID))
                goto tryAgain;
            var request = new CtpRequest(requestID, m_encoder);
            m_activeRequests[requestID] = request;
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
                if ((m_packetDecoder.Results.RequestID & 1) != 0)
                {
                    if (m_clientRequests.TryGetValue(m_packetDecoder.Results.RequestID, out var socket))
                    {
                        if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.InitialRequest) != 0)
                        {
                            socket.Dispose();
                            socket = new CtpRequestHandler(!m_isClient, () => m_clientRequests.Remove(sessionID), sessionID, m_encoder);
                            m_clientRequests[sessionID] = socket;
                            m_onNewInboundSession(socket);
                        }
                        socket.ProcessRead(m_packetDecoder.Results);
                    }
                    else
                    {
                        if (!m_isClient)
                        {
                            if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.InitialRequest) != 0)
                            {
                                m_encoder.Send(CtpContentFlags.CloseRequest | CtpContentFlags.WasRequestInitiatedByServer, sessionID, new CtpError("Request Error", "A new request must be flagged as an initial request.").ToDocument().ToArray());
                                return;
                            }
                            //If this request was made by the client, and I'm the server, begin a new request.
                            socket = new CtpRequestHandler(!m_isClient, () => m_clientRequests.Remove(sessionID), sessionID, m_encoder);
                            m_clientRequests[sessionID] = socket;
                            m_onNewInboundSession(socket);
                        }
                    }
                }
                else
                {
                    if (m_serverRequests.TryGetValue(m_packetDecoder.Results.RequestID, out var socket))
                    {
                        if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.InitialRequest) != 0)
                        {
                            socket.Dispose();
                            socket = new CtpRequestHandler(!m_isClient, () => m_serverRequests.Remove(sessionID), sessionID, m_encoder);
                            m_serverRequests[sessionID] = socket;
                            m_onNewInboundSession(socket);
                        }
                        socket.ProcessRead(m_packetDecoder.Results);
                    }
                    else
                    {
                        if (m_isClient)
                        {
                            if ((m_packetDecoder.Results.ContentFlags & CtpContentFlags.InitialRequest) != 0)
                            {
                                m_encoder.Send(CtpContentFlags.CloseRequest | CtpContentFlags.WasRequestInitiatedByServer, sessionID, new CtpError("Request Error", "A new request must be flagged as an initial request.").ToDocument().ToArray());
                                return;
                            }

                            //If this request was made by the server, and I'm the client, begin a new request.
                            socket = new CtpRequestHandler(m_isClient, () => m_serverRequests.Remove(sessionID), sessionID, m_encoder);
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
