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
                    SubscriberDisconnected?.Invoke(this, new EventArgs<Subscriber>(subscriber));
                    m_subscribers.Remove(id);
                    return true;
                }
            }

            return false;
        }

        public void SendData(DataPoint dataPoint)
        {
            // Route to needed subscribers
            foreach (Subscriber subscriber in FindAllFor(dataPoint.Key.UniqueID))
            {
                subscriber.SendData(dataPoint);
            }
        }

        private Subscriber[] FindAllFor(Guid pointID)
        {
            // TODO: look up subscribers that have subscribed to this point
            return null;
        }

        private void OnClientConnected(dynamic tcpSocket)
        {
            Subscriber subscriber = new Subscriber(null, tcpSocket);
            subscriber.SubscriberSessionEstablished += (sender, args) => SubscriberSessionEstablished?.Invoke(sender, args);
            subscriber.SubscriberDisconnected += (sender, args) => DisconnectSubscriber(args.Argument);

            lock (m_subscribers)
                m_subscribers.Add(subscriber.ID, subscriber);

            SubscriberConnected?.Invoke(this, new EventArgs<Subscriber>(subscriber));

            subscriber.Start();
        }

    }
}