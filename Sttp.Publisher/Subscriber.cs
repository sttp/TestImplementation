using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using CTP;
using Sttp.Codec;
using Sttp.Data;

namespace Sttp.Publisher
{
    public class Subscriber
    {
        //internal event EventHandler<EventArgs<Response>> ReceivedResponse;

        internal int NegotiationStep;

        private readonly dynamic m_tcpSocket;
        private dynamic m_udpSocket;
        private readonly Guid m_id;
        private byte[] m_key;
        private byte[] m_iv;
        private readonly List<DataPoint> m_dataPointQueue;
        //private readonly List<Command> m_commandQueue;
        //private readonly List<Response> m_responseQueue;
        private readonly Thread m_dataThread;
        private readonly Thread m_sendThread;
        private bool m_enabled;
        private readonly WireEncoder m_wireEncoder;
        private readonly WireDecoder m_wireDecoder;
        private MetadataRepository m_metadata;

        internal Subscriber(dynamic tcpSocket, MetadataRepository metadata)
        {
            m_metadata = metadata;
            m_tcpSocket = tcpSocket;
            m_id = Guid.NewGuid();
            m_dataPointQueue = new List<DataPoint>();
            //m_commandQueue = new List<Command>();
            //m_responseQueue = new List<Response>();
            m_enabled = true;

            m_dataThread = new Thread(CollateData);
            m_dataThread.Start();

            m_wireEncoder = new WireEncoder();
            m_wireDecoder = new WireDecoder();
            m_wireEncoder.NewPacket += WireEncoderNewPacket;
            // Setup data reception event or handler, etc...
            //m_tcpSocket.OnDataReceived += m_tcpSocket_OnDataReceived;
        }

        private void WireEncoderNewPacket(byte[] data, int position, int length)
        {

            //ToDo: possibly queue data packets so this method will never block.
            m_tcpSocket.Send(data, position, length);
            //          } 
            //            else
            //            {
            //                // Encrypt payload
            //                if ((object) m_key != null)
            //                {
            //                    using (AesManaged aes = new AesManaged())
            //                    {
            //                        aes.KeySize = 256;
            //                        aes.Key = m_key;
            //                        aes.IV = m_iv;
            //                        data = aes.Encrypt(data, 0, data.Length, m_key, m_iv);
            //                        length = data.Length;
            //                    }
            //}
            //m_udpSocket.Send(data, length);
            //            }
        }

        private void m_encoderUDP_NewPacket(byte[] data, int position, int length)
        {
            // Encrypt payload
            if ((object)m_key != null)
            {
                using (AesManaged aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.Key = m_key;
                    aes.IV = m_iv;
                    data = aes.Encrypt(data, position, data.Length, m_key, m_iv);
                    position = 0;
                    length = data.Length;
                }
            }
            m_udpSocket.Send(data, position, length);
        }

        public void Start()
        {
            // Start session negotiation - first step is to declare supported protocol versions...
            //ProtocolVersions versions = new ProtocolVersions { Versions = new[] { OnePointZero } };
            //m_encoder.NegotiateSession.SupportedVersions(versions);
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
            //m_encoder.NegotiateSession.SecureUdpDataChannel(key, iv);
        }

        internal void QueueDataPoint(DataPoint dataPoint)
        {
            lock (m_dataPointQueue)
                m_dataPointQueue.Add(dataPoint);
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

                SttpDataPoint wire = new SttpDataPoint();

                PatchSignalMapping(dataPoints);

                //m_encoder.DataPoint.BeginCommand();

                //foreach (var point in dataPoints)
                //{
                //    wire.ID = m_signalMapping[point.Key.UniqueID].RuntimeID;
                //    Array.Copy(point.Value, 0, wire.Value, 0, point.ValueLength);
                //    wire.BulkDataValueID = 0;
                //    wire.ValueLength = (uint)point.ValueLength;
                //    wire.Time = new SttpTimestamp(point.Time);
                //    wire.TimeQualityFlags = point.Flags;
                //    wire.DataQualityFlags = point.QualityFlags;

                //    //m_encoder.DataPoint.SendDataPoint(wire);
                //}

                //m_encoder.DataPoint.EndCommand();
            }
        }

        //private Dictionary<Guid, DataPointKeyWire> m_signalMapping = new Dictionary<Guid, DataPointKeyWire>();
        private uint m_nextRuntimeIDIndex = 0;
        private uint m_nextBulkValueID = 1;

        private void PatchSignalMapping(DataPoint[] dataPoints)
        {
            foreach (var point in dataPoints)
            {
                //DataPointKeyWire map;
                //if (!(m_signalMapping.TryGetValue(point.Key.UniqueID, out map) && map.Type == point.Key.Type))
                //{
                //    map = new DataPointKeyWire();
                //    map.UniqueID = point.Key.UniqueID;
                //    map.Flags = StateFlags.Quality;
                //    map.Type = point.Key.Type;
                //    map.RuntimeID = m_nextRuntimeIDIndex;
                //    m_nextRuntimeIDIndex++;
                //    //m_encoder.DataPoint.MapRuntimeID(map);
                //}
            }
        }

        private void m_tcpSocket_OnDataReceived(byte[] buffer, int startIndex, int length)
        {
            m_wireDecoder.FillBuffer(buffer, startIndex, length);
            var packet = m_wireDecoder.NextCommand();
            while (packet != null)
            {
                switch (packet.CommandCode)
                {
                    //case DecoderCallback.NegotiateSessionStep1:
                    //    // Setup subscriber desired protocol version, validating that publisher can support it
                    //    ProtocolVersions versions;
                    //    m_decoderTcp.NegotiateSessionStep1(out versions);

                    //    // Version 1.0 is the only currently supported version for this implementation
                    //    if (versions.Versions.Any(v => v == OnePointZero))
                    //    {
                    //        // Send supported operational modes
                    //        OperationalModes modes = new OperationalModes
                    //        {
                    //            Stateful = new NamedVersions { Items = new[] { new NamedVersion { Name = "TSSC", Version = OnePointZero } } },
                    //            Stateless = new NamedVersions { Items = new[] { new NamedVersion { Name = "DEFLATE", Version = OnePointZero } } },
                    //            UdpPort = SupportsUDP ? (ushort)1 : (ushort)0
                    //        };

                    //        m_encoder.NegotiateSession.SelectedModes(modes);
                    //    }
                    //    else
                    //    {
                    //        Disconnect();
                    //    }
                    //    break;
                    //case DecoderCallback.NegotiateSessionStep1Reply:
                    //    //m_encoder.CommandFailed(CommandCode.NegotiateSession, "Not expecting a reply");
                    //    Disconnect();
                    //    break;
                    //case DecoderCallback.NegotiateSessionStep2:
                    //    // Setup subscriber desired operational modes, validating that publisher can support them
                    //    OperationalModes subscriberModes;
                    //    m_decoderTcp.NegotiateSessionStep2(out subscriberModes);

                    //    // Validate UDP support
                    //    if (!SupportsUDP && subscriberModes.UdpPort > 0)
                    //    {
                    //        //m_encoder.CommandFailed(CommandCode.NegotiateSession, "Publisher does not support UDP");
                    //        Disconnect();
                    //        return;
                    //    }

                    //    // Set up desired compression algorithm
                    //    Func<byte[], int, int, byte[]> compression = null;
                    //    string name = subscriberModes.Stateful.Items[0].Name;
                    //    Version version = subscriberModes.Stateful.Items[0].Version;
                    //    bool supported = true;

                    //    switch (name)
                    //    {
                    //        case "DEFLATE":
                    //            if (version == OnePointZero)
                    //                compression = null; // DeflateCompress();
                    //            else
                    //                supported = false;
                    //            break;
                    //        case "TSSC":
                    //            if (version == OnePointZero)
                    //                compression = null; // new TsscAlgorithm(subscriber).Compress();
                    //            else
                    //                supported = false;
                    //            break;
                    //        case "NONE":
                    //            supported = version == OnePointZero;
                    //            break;
                    //    }

                    //    if (subscriberModes.UdpPort > 0)
                    //    {
                    //        // TODO: Setup Stateless compression algorithm also
                    //    }

                    //    if (supported)
                    //    {
                    //        //m_encoder.CommandSuccess(CommandCode.NegotiateSession, "");
                    //        //SetCompressionAlgorithm(compression);
                    //        SubscriberSessionEstablished?.Invoke(this, new EventArgs<Subscriber>(this));
                    //    }
                    //    else
                    //    {
                    //        //m_encoder.CommandFailed(CommandCode.NegotiateSession, $"Unsupported compression algorithm: {name}");
                    //        Disconnect();
                    //    }
                    //    break;
                    //case DecoderCallback.CommandSuccess:
                    //    break;
                    //case DecoderCallback.CommandFailed:
                    //    break;
                    //case DecoderCallback.RequestMetadataTables:
                    //    break;
                    //case DecoderCallback.RequestMetadataTablesReply:
                    //    break;
                    //case DecoderCallback.RequestMetadata:
                    //    break;
                    //case DecoderCallback.RequestMetadataReply:
                    //    break;
                    //case DecoderCallback.Subscribe:
                    //    break;
                    //case DecoderCallback.EndOfMessages:
                    //    break;
                    //case CommandCode.NegotiateSession:
                    //    break;
                    //case CommandCode.Subscription:
                    //    break;
                    //case CommandCode.MapRuntimeIDs:
                    //    break;
                    //case CommandCode.NoOp:
                    //    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                packet = m_wireDecoder.NextCommand();
            }
        }

        public const ushort DefaultTargetPacketSize = short.MaxValue / 2;
        public const bool DefaultSupportsUDP = true;

        private static readonly Version OnePointZero = new Version(1, 0);

        public event EventHandler<EventArgs<Subscriber>> SubscriberSessionEstablished; // Successful session negotiations
        public event EventHandler<EventArgs<Subscriber>> SubscriberDisconnected;       // Subscriber disconnected

        public ushort TargetPacketSize { get; set; } = DefaultTargetPacketSize;

        public bool SupportsUDP { get; set; } = DefaultSupportsUDP;

        public void SendData(int runtimeID, DataPoint dataPoint)
        {
            //ToDo: Handle strings and byte arrays differently.
            QueueDataPoint(dataPoint);
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
