using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Sttp.Codec;

namespace Sttp.Adapters
{
    public class NetServerClient
    {
        private readonly TcpClient m_client;
        private readonly NetworkStream m_stream;
        private readonly HashSet<Guid> m_sids;
        public WireEncoder Encoder;
        private WireDecoder m_decoder;
        private byte[] m_readBuffer = new byte[4096];
        private CommandObjects m_nextCommand;
        private bool m_isReadPending;
        private AsyncCallback m_onReceive;

        public NetServerClient(TcpClient client, HashSet<Guid> sids)
        {
            m_client = client;
            m_sids = sids;
            m_stream = client.GetStream();
            Encoder = new WireEncoder();
            m_decoder = new WireDecoder();
            Encoder.NewPacket += Encoder_NewPacket;
            m_onReceive = OnReceive;
            BeginRead();
        }

        private void BeginRead()
        {
            if (!m_isReadPending)
            {
                m_isReadPending = true;
                m_stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, m_onReceive, null);
            }
        }

        public bool CommandAvailable => m_nextCommand != null;

        public CommandObjects NextCommand()
        {
            if (m_nextCommand == null)
            {
                BeginRead();
                return null;
            }
            else
            {
                var rv = m_nextCommand;
                m_nextCommand = m_decoder.NextCommand();
                return rv;
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            int length = m_stream.EndRead(ar);
            m_isReadPending = false;
            m_decoder.FillBuffer(m_readBuffer, 0, length);
            m_nextCommand = m_decoder.NextCommand();
        }

        private void Encoder_NewPacket(byte[] buffer, int offset, int length)
        {
            m_stream.Write(buffer, offset, length);
        }
    }
}