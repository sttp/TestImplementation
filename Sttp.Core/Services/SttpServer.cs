using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Sttp.Codec;
using Sttp.Core;

namespace Sttp.Services
{
    public class SttpServer
    {
        private Stream m_stream;
        private WireEncoder m_encoder;
        private WireDecoder m_decoder;
        private Thread m_processing;
        private Dictionary<string, ISttpCommandHandler> m_handler;

        public SttpBulkTransportServer BulkTransport;
        public SttpMetadataServer MetadataServer;

        public SttpServer(Stream networkStream)
        {
            m_handler = new Dictionary<string, ISttpCommandHandler>();
            m_stream = networkStream;
            m_encoder = new WireEncoder();
            m_encoder.NewPacket += M_encoder_NewPacket;
            m_decoder = new WireDecoder();

            BulkTransport = new SttpBulkTransportServer();
            MetadataServer = new SttpMetadataServer();
            RegisterCommandHandler(BulkTransport);
            RegisterCommandHandler(MetadataServer);
        }

        private void M_encoder_NewPacket(byte[] data, int offset, int length)
        {
            m_stream.Write(data, offset, length);
        }

        public void RegisterCommandHandler(ISttpCommandHandler handler)
        {
            foreach (var name in handler.CommandsHandled())
            {
                m_handler[name] = handler;
            }
        }

        private void ProcessRequest()
        {
            byte[] buffer = new byte[4096];
            int length = 0;
            while ((length = m_stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                m_decoder.FillBuffer(buffer, 0, length);
                CommandObjects obj;
                while ((obj = m_decoder.NextCommand()) != null)
                {
                    if (m_handler.TryGetValue(obj.CommandName, out ISttpCommandHandler handler))
                    {
                        handler.HandleCommand(obj, m_encoder);
                    }
                    else
                    {
                        m_encoder.RequestFailed(obj.CommandName, false, "Command Handler does not exist", "");
                    }
                }
            }
        }

        public void Start()
        {
            m_processing = new Thread(ProcessRequest);
            m_processing.IsBackground = true;
            m_processing.Start();
        }





    }
}
