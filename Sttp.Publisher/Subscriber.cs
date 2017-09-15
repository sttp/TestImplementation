using System;
using System.Collections.Generic;
using System.Threading;
using Sttp.WireProtocol;

namespace Sttp.Publisher
{
    public class Subscriber
    {
        internal event EventHandler<EventArgs<Response>> ReceivedResponse;

        internal int NegotiationStep;

        private readonly API m_api;
        private readonly dynamic m_tcpSocket;
        private dynamic m_udpSocket;
        private readonly Guid m_id;
        private Func<byte[], int, int, byte[]> m_compression;
        private byte[] m_key;
        private byte[] m_iv;
        private readonly List<DataPoint> m_dataPointQueue;
        private readonly List<Command> m_commandQueue;
        private readonly List<Response> m_responseQueue;
        private readonly Thread m_dataThread;
        private readonly Thread m_sendThread;
        private bool m_enabled;

        internal Subscriber(API api, dynamic tcpSocket)
        {
            m_api = api;
            m_tcpSocket = tcpSocket;
            m_id = Guid.NewGuid();
            m_dataPointQueue = new List<DataPoint>();
            m_commandQueue = new List<Command>();
            m_responseQueue = new List<Response>();
            m_enabled = true;

            m_dataThread = new Thread(CollateData);
            m_dataThread.Start();

            m_sendThread = new Thread(SendData);
            m_sendThread.Start();

            // Setup data reception event or handler, etc...
            //m_tcpSocket.OnDataReceived += m_tcpSocket_OnDataReceived;
        }

        public Guid ID => m_id;

        // TODO: Add other subscriber info and goodies

        internal void Disconnect()
        {
            m_enabled = false;
            m_tcpSocket?.Disconnect();
            m_udpSocket?.Disconnect();
        }

        internal void SetCompressionAlgorithm(Func<byte[], int, int, byte[]> compression)
        {
            m_compression = compression;
        }

        internal void SetCryptoParameters(byte[] key, byte[] iv)
        {
            m_key = key;
            m_iv = iv;

            // TODO: Payload needs to be SymmetricSecurity structure
            SendResponse(new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.SecureDataChannel, Payload = null });
        }

        internal void QueueDataPoint(DataPoint dataPoint)
        {
            lock (m_dataPointQueue)
                m_dataPointQueue.Add(dataPoint);
        }

        internal void SendCommand(Command command)
        {
            lock (m_commandQueue)
                m_commandQueue.Add(command);
        }

        internal void SendResponse(Response response)
        {
            lock (m_responseQueue)
                m_responseQueue.Add(response);
        }

        private void CollateData()
        {
            // TODO: This is prototype example code - use thread signaling and/or handle in a better way...
            while (m_enabled)
            {
                Thread.Sleep(100);

                dynamic[] dataPoints;

                lock (m_dataPointQueue)
                {
                    dataPoints = m_dataPointQueue.ToArray();
                    m_dataPointQueue.Clear();
                }

                // Combine data points into command payload / fragment as needed into 16K chunks

                Command dataPointPacket = new Command { CommandCode = CommandCode.DataPointPacket, Payload = null };

                byte[] payload = dataPointPacket.Payload;

                // Compress payload
                if ((object)m_compression != null)
                    payload = m_compression(payload, 0, payload.Length);

                // Encrypt payload
                //if ((object)m_key != null)
                //    payload = aes.Encrypt(payload, m_key, m_iv);

                dataPointPacket.Payload = payload;


                SendCommand(dataPointPacket);
            }
        }

        private void SendData()
        {
            // TODO: This is prototype example code - use thread signaling and/or handle in a better way...
            while (m_enabled)
            {
                Thread.Sleep(500);

                Command[] commands;
                Response[] responses;

                lock (m_commandQueue)
                {
                    commands = m_commandQueue.ToArray();
                    m_commandQueue.Clear();
                }

                lock (m_responseQueue)
                {
                    responses = m_responseQueue.ToArray();
                    m_responseQueue.Clear();
                }

                foreach (Command command in commands)
                    m_tcpSocket.Send(command.Encode());

                foreach (Response response in responses)
                    m_tcpSocket.Send(response.Encode());
            }
        }

        private void m_tcpSocket_OnDataReceived(byte[] buffer, int startIndex, int length)
        {
            Response response = buffer.DecodeResponse(startIndex, length);
            ReceivedResponse?.Invoke(this, new EventArgs<Response>(response));
        }
    }
}
