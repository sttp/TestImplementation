using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Sttp.WireProtocol;
using Decoder = Sttp.WireProtocol.Decoder;
using Encoder = Sttp.WireProtocol.Encoder;
using Version = Sttp.WireProtocol.Version;

namespace Sttp.Publisher
{
    public class Subscriber
    {
        internal event EventHandler<EventArgs<Response>> ReceivedResponse;

        internal int NegotiationStep;

        private readonly dynamic m_tcpSocket;
        private dynamic m_udpSocket;
        private readonly Guid m_id;
        private byte[] m_key;
        private byte[] m_iv;
        private readonly List<DataPoint> m_dataPointQueue;
        private readonly List<Command> m_commandQueue;
        private readonly List<Response> m_responseQueue;
        private readonly Thread m_dataThread;
        private readonly Thread m_sendThread;
        private bool m_enabled;
        private readonly Encoder m_encoder;
        private readonly Decoder m_decoder;

        internal Subscriber(dynamic tcpSocket)
        {
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

            m_encoder = new Encoder();
            m_decoder = new Decoder();

            // Setup data reception event or handler, etc...
            //m_tcpSocket.OnDataReceived += m_tcpSocket_OnDataReceived;
        }

        public void Start()
        {
            // Start session negotiation - first step is to declare supported protocol versions...
            ProtocolVersions versions = new ProtocolVersions { Versions = new[] { OnePointZero } };
            SendCommand(new Command { CommandCode = CommandCode.NegotiateSession, Payload = versions.Encode() });
            // Start timer to wait on subscriber response - then disconnect otherwise
        }

        public Guid ID => m_id;

        // TODO: Add other subscriber info and goodies

        internal void Disconnect()
        {
            m_enabled = false;
            m_tcpSocket?.Disconnect();
            m_udpSocket?.Disconnect();
            SubscriberDisconnected?.Invoke(this, new EventArgs<Subscriber>(this));
        }

        internal void SetCryptoParameters(byte[] key, byte[] iv)
        {
            m_key = key;
            m_iv = iv;

            // TODO: Payload needs to be SymmetricSecurity structure
            SendResponse(new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.SecureDataChannel, Payload = null });
        }

        internal void QueueDataPoints(IEnumerable<DataPoint> dataPoints)
        {
            // Add each point individually instead of as a range - this
            // way bulk data points can interleave with other data points
            // that may be being added simultaneously
            foreach (DataPoint dataPoint in dataPoints)
                QueueDataPoint(dataPoint);
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

                DataPoint[] dataPoints;

                lock (m_dataPointQueue)
                {
                    dataPoints = m_dataPointQueue.ToArray();
                    m_dataPointQueue.Clear();
                }

                DataPointWire wire = new DataPointWire();

                PatchSignalMapping(dataPoints);

                foreach (var point in dataPoints)
                {
                    if (point.ValueLength <= 64)
                    {
                        wire.ID = m_signalMapping[point.Key.UniqueID].RuntimeID;
                        Array.Copy(point.Value, 0, wire.Value, 0, point.ValueLength);
                        wire.Sequence = 0;
                        wire.Fragment = 0;
                        wire.Length = (uint)point.ValueLength;
                        wire.Time = new SttpTimestamp(point.Time);
                        wire.Flags = point.Flags;
                        wire.QualityFlags = point.QualityFlags;

                        m_encoder.SendDataPoint(true, wire);
                    }
                    else
                    {
                        uint bulkID = m_nextBulkValueID;
                        if (m_nextBulkValueID == 0)
                            m_nextBulkValueID++;

                        wire.ID = m_signalMapping[point.Key.UniqueID].RuntimeID;
                        wire.Sequence = bulkID;
                        wire.Length = (uint)point.ValueLength;
                        wire.Time = new SttpTimestamp(point.Time);
                        wire.Flags = point.Flags;
                        wire.QualityFlags = point.QualityFlags;

                        for (int x = 0; x < point.ValueLength; x += 64)
                        {
                            Array.Copy(point.Value, 0, wire.Value, 0, point.ValueLength);
                            wire.Fragment = (uint)(x / 64);
                            m_encoder.SendDataPoint(true, wire);
                        }
                    }
                }

                //Send this data packet.

                byte[] payload = new byte[0];

                // Encrypt payload
                if ((object)m_key != null)
                {
                    using (AesManaged aes = new AesManaged())
                    {
                        aes.KeySize = 256;
                        aes.Key = m_key;
                        aes.IV = m_iv;
                        payload = aes.Encrypt(payload, 0, payload.Length, m_key, m_iv);
                    }
                }

                SendCommand(null);
            }
        }

        private Dictionary<Guid, DataPointKeyWire> m_signalMapping = new Dictionary<Guid, DataPointKeyWire>();
        private uint m_nextRuntimeIDIndex = 0;
        private uint m_nextBulkValueID = 1;

        private void PatchSignalMapping(DataPoint[] dataPoints)
        {
            foreach (var point in dataPoints)
            {
                DataPointKeyWire map;
                if (!(m_signalMapping.TryGetValue(point.Key.UniqueID, out map) && map.Type == point.Key.Type))
                {
                    map = new DataPointKeyWire();
                    map.UniqueID = point.Key.UniqueID;
                    map.Flags = StateFlags.Quality;
                    map.Type = point.Key.Type;
                    map.RuntimeID = m_nextRuntimeIDIndex;
                    m_nextRuntimeIDIndex++;
                    m_encoder.RuntimeIDMapping(map);
                }
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
            Response response = Response.Decode(buffer, startIndex, length);
            ReceivedResponse?.Invoke(this, new EventArgs<Response>(response));
        }

        public const ushort DefaultTargetPacketSize = short.MaxValue / 2;
        public const bool DefaultSupportsUDP = true;

        private static readonly Version OnePointZero = new Version(1, 0);

        public event EventHandler<EventArgs<Subscriber>> SubscriberSessionEstablished; // Successful session negotiations
        public event EventHandler<EventArgs<Subscriber>> SubscriberDisconnected;       // Subscriber disconnected

        public ushort TargetPacketSize { get; set; } = DefaultTargetPacketSize;

        public bool SupportsUDP { get; set; } = DefaultSupportsUDP;

        public void SendData(DataPoint dataPoint)
        {
            //ToDo: Handle strings and byte arrays differently.
            QueueDataPoint(dataPoint);
        }

        private void subscriber_ReceivedResponse(object sender, EventArgs<Response> e)
        {
            Response response = e.Argument;

            // Check which command this is in response to
            switch (response.CommandCode)
            {
                case CommandCode.NegotiateSession:
                    NegotiateSession(response);
                    break;
                case CommandCode.MetadataRefresh:
                    break;
                case CommandCode.Subscribe:
                    break;
                case CommandCode.Unsubscribe:
                    break;
                case CommandCode.SecureDataChannel:
                    SecureDataChannel();
                    break;
                case CommandCode.RuntimeIDMapping:
                    break;
                case CommandCode.NoOp:
                    break;
            }
        }

        private void NegotiateSession(Response response)
        {
            if (response.ResponseCode == ResponseCode.Succeeded)
            {
                NegotiationStep++;

                if (NegotiationStep == 1)
                {
                    // Setup subscriber desired protocol version, validating that publisher can support it
                    ProtocolVersions versions = ProtocolVersions.Decode(response.Payload, 0, response.Payload.Length);

                    // Version 1.0 is the only currently supported version for this implementation
                    if (versions.Versions.Any(version => version == OnePointZero))
                    {
                        // Send supported operational modes
                        OperationalModes modes = new OperationalModes
                        {
                            Stateful = new NamedVersions { Items = new[] { new NamedVersion { Name = "TSSC", Version = OnePointZero } } },
                            Stateless = new NamedVersions { Items = new[] { new NamedVersion { Name = "DEFLATE", Version = OnePointZero } } },
                            UdpPort = SupportsUDP ? (ushort)1 : (ushort)0
                        };

                        SendResponse(new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = modes.Encode() });
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                else if (NegotiationStep == 2)
                {
                    // Setup subscriber desired operational modes, validating that publisher can support them
                    OperationalModes subscriberModes = OperationalModes.Decode(response.Payload, 0, response.Payload.Length);

                    // Validate UDP support
                    if (!SupportsUDP && subscriberModes.UdpPort > 0)
                    {
                        SendResponse(new Response { ResponseCode = ResponseCode.Failed, CommandCode = CommandCode.NegotiateSession, Payload = MessageResponse("Publisher does not support UDP") });
                        Disconnect();
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
                        //SetCompressionAlgorithm(compression);
                        SendResponse(new Response { ResponseCode = ResponseCode.Succeeded, CommandCode = CommandCode.NegotiateSession, Payload = null });
                        SubscriberSessionEstablished?.Invoke(this, new EventArgs<Subscriber>(this));
                    }
                    else
                    {
                        SendResponse(new Response { ResponseCode = ResponseCode.Failed, CommandCode = CommandCode.NegotiateSession, Payload = MessageResponse($"Unsupported compression algorithm: {name}") });
                        Disconnect();
                    }
                }
                else
                {
                    Disconnect();

                    // TODO: Just log instead of throwing exception
                    throw new InvalidOperationException("Too many session negotiation steps encountered - check protocol version");
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void SecureDataChannel()
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();

                SetCryptoParameters(aes.Key, aes.IV);
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
    }
}
