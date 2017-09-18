using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sttp.WireProtocol;
using Version = Sttp.WireProtocol.Version;

namespace Sttp.Publisher
{
    // API layer exists above wire protocol layer whose details are invisible to API user.
    // Goal here is that API is the primary interface any STTP user will ever see and hence
    // needs to remain as simple as possible.
    public class API
    {
        public const ushort DefaultTargetPacketSize = short.MaxValue / 2;
        public const bool DefaultSupportsUDP = true;

        private static readonly Version OnePointZero = new Version(1, 0);

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

        public ushort TargetPacketSize { get; set; } = DefaultTargetPacketSize;

        public bool SupportsUDP { get; set; } = DefaultSupportsUDP;

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

        public void SendData(Guid pointID, sbyte value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, short value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, int value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, long value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, byte value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, ushort value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, uint value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, ulong value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, decimal value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, double value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, float value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, Guid value, DataPointState state = null)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                DataPoint point = new DataPoint { ID = GetRuntimeID(subscriber, pointID) };

                point.SetValue(value);
                point.SetState(state);

                subscriber.QueueDataPoint(point);
            }
        }

        public void SendData(Guid pointID, byte[] value)
        {
            // Fragment buffer into needed data points
            List<DataPoint> dataPoints = DataPoint.GetDataPoints(value);

            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                uint runtimeID = GetRuntimeID(subscriber, pointID);

                subscriber.QueueDataPoints(dataPoints.Select(point => new DataPoint
                {
                    ID = runtimeID,
                    //Value = point.Value, ToDo: Fix
                    State = point.State
                }));
            }
        }

        public void SendData(Guid pointID, string value, Encoding encoding)
        {
            // Fragment string into needed data points
            List<DataPoint> dataPoints = DataPoint.GetDataPoints(value, encoding);

            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(pointID))
            {
                uint runtimeID = GetRuntimeID(subscriber, pointID);

                subscriber.QueueDataPoints(dataPoints.Select(point => new DataPoint
                {
                    ID = runtimeID,
                    //Value = point.Value, ToDo: Fix
                    State = point.State
                }));
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
            ProtocolVersions versions = new ProtocolVersions { Versions = new[] { OnePointZero } };
            SendCommand(subscriber, new Command { CommandCode = CommandCode.NegotiateSession, Payload = versions.Encode() });

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
                    NegotiateSession(subscriber, response);
                    break;
                case CommandCode.MetadataRefresh:
                    break;
                case CommandCode.Subscribe:
                    break;
                case CommandCode.Unsubscribe:
                    break;
                case CommandCode.SecureDataChannel:
                    SecureDataChannel(subscriber);
                    break;
                case CommandCode.RuntimeIDMapping:
                    break;
                case CommandCode.NoOp:
                    break;
            }
        }

        private void NegotiateSession(Subscriber subscriber, Response response)
        {
            if (response.ResponseCode == ResponseCode.Succeeded)
            {
                subscriber.NegotiationStep++;

                if (subscriber.NegotiationStep == 1)
                {
                    // Setup subscriber desired protocol version, validating that publisher can support it
                    ProtocolVersions versions = ProtocolVersions.Decode(response.Payload, 0, response.Payload.Length);

                    // Version 1.0 is the only currently supported version for this implementation
                    if (versions.Versions.Any(version => version == OnePointZero))
                    {
                        // Send supported operational modes
                        OperationalModes modes = new OperationalModes
                        {
                            Encodings = StringEncodingFlags.ASCII | StringEncodingFlags.ANSI | StringEncodingFlags.UTF8 | StringEncodingFlags.Unicode,
                            Stateful = new NamedVersions { Items = new[] { new NamedVersion { Name = "TSSC", Version = OnePointZero } } },
                            Stateless = new NamedVersions { Items = new[] { new NamedVersion { Name = "DEFLATE", Version = OnePointZero } } },
                            UdpPort = SupportsUDP ? (ushort)1 : (ushort)0
                        };

                        SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = modes.Encode() });
                    }
                    else
                    {
                        DisconnectSubscriber(subscriber);
                    }
                }
                else if (subscriber.NegotiationStep == 2)
                {
                    // Setup subscriber desired operational modes, validating that publisher can support them
                    OperationalModes subscriberModes = OperationalModes.Decode(response.Payload, 0, response.Payload.Length);

                    // Validate UDP support
                    if (!SupportsUDP && subscriberModes.UdpPort > 0)
                    {
                        SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Failed, CommandCode = CommandCode.NegotiateSession, Payload = MessageResponse("Publisher does not support UDP") });
                        DisconnectSubscriber(subscriber);
                        return;
                    }

                    // Set up desired compression algorithm
                    Func<byte[], int, int, byte[]> compression = null;
                    string name = subscriberModes.Stateful.Items[0].Name;
                    Version version = subscriberModes.Stateful.Items[0].Version;
                    bool supported = true;

                    switch (name)
                    {
                        case "DEFLATE":
                            if (version == OnePointZero)
                                compression = null; // DeflateCompress();
                            else
                                supported = false;
                            break;
                        case "TSSC":
                            if (version == OnePointZero)
                                compression = null; // new TsscAlgorithm(subscriber).Compress();
                            else
                                supported = false;
                            break;
                        case "NONE":
                            supported = version == OnePointZero;
                            break;
                    }

                    if (subscriberModes.UdpPort > 0)
                    {
                        // TODO: Setup Stateless compression algorithm also
                    }

                    if (supported)
                    {
                        subscriber.SetCompressionAlgorithm(compression);
                        SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = null });
                        SubscriberSessionEstablished?.Invoke(this, new EventArgs<Subscriber>(subscriber));
                    }
                    else
                    {
                        SendResponse(subscriber, new Response { ResponseCode = ResponseCode.Failed, CommandCode = CommandCode.NegotiateSession, Payload = MessageResponse($"Unsupported compression algorithm: {name}") });
                        DisconnectSubscriber(subscriber);
                    }
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
        }

        private void SecureDataChannel(Subscriber subscriber)
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();

                subscriber.SetCryptoParameters(aes.Key, aes.IV);
            }
        }

        private byte[] MessageResponse(string message)
        {
            if (string.IsNullOrEmpty(message))
                return new byte[] { 0 };

            if (message.Length > byte.MaxValue)
                message = message.Substring(0, byte.MaxValue);

            byte[] data = Encoding.ASCII.GetBytes(message);

            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte((byte)data.Length);
                stream.Write(data, 0, data.Length);
                return stream.ToArray();
            }
        }

        private void SendCommand(Subscriber subscriber, Command command) => subscriber?.SendCommand(command);

        private void SendResponse(Subscriber subscriber, Response response) => subscriber?.SendResponse(response);

    }
}