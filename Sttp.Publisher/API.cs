using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using Sttp.WireProtocol;

namespace Sttp.Publisher
{
    // API layer exists above wire protocol layer whose details are invisible to API user.
    // Goal here is that API is the primary interface any STTP user will ever see and hence
    // needs to remain as simple as possible.
    public class API
    {
        public event EventHandler<EventArgs<Subscriber>> SubscriberConnected;          // Subscriber connected
        public event EventHandler<EventArgs<Subscriber>> SubscriberSessionEstablished; // Successful session negotiations
        public event EventHandler<EventArgs<Subscriber>> SubscriberDisconnected;       // Subscriber disconnected

        private dynamic m_tcpSocket;
        private readonly Dictionary<Guid, Subscriber> m_subscribers;

        public API()
        {
            m_subscribers = new Dictionary<Guid, Subscriber>();
        }

        public ReadOnlyDictionary<Guid, Subscriber> Subscribers => new ReadOnlyDictionary<Guid, Subscriber>(m_subscribers);

        public void Connect(string connectionString)
        {
            // Create socket - note reverse connection needs to be allowed,
            // i.e., where publisher will directly connect to single client
            // Typical use case is that publisher will establish a listening
            // socket that multiple clients will connect to...

            // m_tcpSocket = new Socket(of proper kind);

            // If listening socket, attach to client connected event or handler, etc...
            //m_tcpSocket.ClientConnected += OnClientConnected;
        }

        public void Disconnect()
        {
            m_tcpSocket?.Disconnect();

            lock (m_subscribers)
            {
                foreach (Guid id in m_subscribers.Values.Select(subscriber => subscriber.ID))
                    DisconnectSubscriber(id);

                m_subscribers.Clear();
            }
        }

        public bool DisconnectSubscriber(Subscriber subscriber) => DisconnectSubscriber(subscriber?.ID ?? Guid.Empty);

        public bool DisconnectSubscriber(Guid id)
        {
            lock (m_subscribers)
            {
                Subscriber subscriber;

                if (m_subscribers.TryGetValue(id, out subscriber))
                {
                    subscriber.Disconnect();
                    subscriber.ReceivedResponse -= subscriber_ReceivedResponse;
                    SubscriberDisconnected?.Invoke(this, new EventArgs<Subscriber>(subscriber));
                    m_subscribers.Remove(id);
                    return true;
                }
            }

            return false;
        }

        public void SendData(Guid pointID, ushort value)
        {
            // Route to needed subscribers
            Subscriber[] subscribers = FindAllFor(pointID);

            // Create data point structure for UInt16
            byte[] pointValue = BigEndian.GetBytes(value);

            // Define state: timestamp, data quality, etc
            byte[] pointState = null;

            foreach (Subscriber subscriber in subscribers)
            {
                subscriber.QueueDataPoint(new DataPoint
                {
                    ID = GetRuntimeID(subscriber, pointID),
                    Value = pointValue,
                    State = pointState
                });
            }
        }

        public void SendData(Guid pointID, int value)
        {
            // Route to needed subscribers
            Subscriber[] subscribers = FindAllFor(pointID);

            // Create data point structure for Int32
            byte[] pointValue = BigEndian.GetBytes(value);

            // Define state: timestamp, data quality, etc
            byte[] pointState = null;

            foreach (Subscriber subscriber in subscribers)
            {
                subscriber.QueueDataPoint(new DataPoint
                {
                    ID = GetRuntimeID(subscriber, pointID),
                    Value = pointValue,
                    State = pointState
                });
            }
        }

        public void SendData(Guid pointID, byte[] value)
        {
            // Route to needed subscribers
            Subscriber[] subscribers = FindAllFor(pointID);

            // Fragment value into 15-byte chunks with sequence number
            int fragments = value.Length / 15;

            // Define state: timestamp, data quality, etc
            byte[] pointState = null;

            // Subscriber API can recollate

            // Create data point structure for each chunk and send each chunk to subscribers
            foreach (Subscriber subscriber in subscribers)
            {
                uint runtimeID = GetRuntimeID(subscriber, pointID);

                for (int i = 0; i < fragments; i++)
                {
                    BufferValue bufferValue = new BufferValue
                    {
                        Data = value.BlockCopy(i * 15, 15)
                    };

                    subscriber.QueueDataPoint(new DataPoint
                    {
                        ID = runtimeID,
                        Value = bufferValue.Encode(),
                        State = BigEndian.GetBytes((ushort)i)
                    });
                }
            }
        }

        private Subscriber[] FindAllFor(Guid pointID)
        {
            // TODO: look up subscribers that have subscribed to this point
            return null;
        }

        private uint GetRuntimeID(Subscriber subscriber, Guid uniqueID)
        {
            // TODO: Lookup subscriber's runtime ID for Guid
            return 0;
        }

        private void OnClientConnected(dynamic tcpSocket)
        {
            Subscriber subscriber = new Subscriber(this, tcpSocket);
            subscriber.ReceivedResponse += subscriber_ReceivedResponse; ;

            lock (m_subscribers)
                m_subscribers.Add(subscriber.ID, subscriber);

            SubscriberConnected?.Invoke(this, new EventArgs<Subscriber>(subscriber));

            // Start session negotiation - first step is to declare supported protocol versions...
            // TODO: Payload needs to be ProtocolVersions array
            SendCommand(subscriber, new Command { CommandCode = CommandCode.NegotiateSession, Payload = null });

            // Start timer to wait on subscriber response - then disconnect otherwise
        }

        private void subscriber_ReceivedResponse(object sender, EventArgs<Response> e)
        {
            Subscriber subscriber = sender as Subscriber;

            if ((object)subscriber == null)
                throw new InvalidOperationException($"Received response that was not from a Subscriber: {sender?.GetType().FullName}");

            Response response = e.Argument;

            // Check which command this is in response to
            switch (response.CommandCode)
            {
                case CommandCode.NegotiateSession:
                    if (response.ResponseCode == ResponseCode.Succeeded)
                    {
                        subscriber.NegotiationStep++;

                        if (subscriber.NegotiationStep == 1)
                        {
                            // Setup subscriber desired protocol version, validating that publisher can support it
                            // Then send support operational modes
                            // TODO: Payload needs to be OperationalModes structure
                            SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = null });
                        }
                        else if (subscriber.NegotiationStep == 2)
                        {
                            // Setup subscriber desired operational modes, validating that publisher can support them
                            // This also sets up desired compression algorithm:

                            Func<byte[], int, int, byte[]> compression;
                            string name = "TSSC"; // operationalModes.CompressionAlgorithms[0].Name;
                            // Version version = operationalModes.CompressionAlgorithms[0].Version;

                            switch(name)
                            {
                                case "DEFLATE":
                                    compression = null; // DeflateCompress();
                                    break;
                                case "TSSC":
                                    compression = null; // new TsscAlgorithm(subscriber).Compress();
                                    break;
                                default:
                                    compression = null;
                                    break;
                            }

                            subscriber.SetCompressionAlgorithm(compression);

                            SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = null });
                            SubscriberSessionEstablished?.Invoke(this, new EventArgs<Subscriber>(subscriber));
                        }
                        else
                        {
                            DisconnectSubscriber(subscriber);

                            // TODO: Just log instead of throwing exception
                            throw new InvalidOperationException("Too many session negotiation steps encountered - check protocol version");
                        }
                    }
                    else
                    {
                        DisconnectSubscriber(subscriber);
                    }
                    break;
                case CommandCode.MetadataRefresh:
                    break;
                case CommandCode.Subscribe:
                    break;
                case CommandCode.Unsubscribe:
                    break;
                case CommandCode.SecureDataChannel:
                    using (AesManaged aes = new AesManaged())
                    {
                        aes.KeySize = 256;
                        aes.GenerateKey();
                        aes.GenerateIV();

                        subscriber.SetCryptoParameters(aes.Key, aes.IV);
                    }
                    break;
                case CommandCode.RuntimeIDMapping:
                    break;
                case CommandCode.NoOp:
                    break;
            }
        }

        private void SendCommand(Subscriber subscriber, Command command) => subscriber?.SendCommand(command);

        private void SendResponse(Subscriber subscriber, Response response) => subscriber?.SendResponse(response);
    }
}