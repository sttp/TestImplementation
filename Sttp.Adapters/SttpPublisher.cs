//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
//using GSF.Diagnostics;
//using GSF.TimeSeries;
//using GSF.TimeSeries.Adapters;

//namespace Sttp.Adapters
//{
//    public class SttpPublisher
//        : FacileActionAdapterBase
//    {
//        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(SttpPublisher), MessageClass.Application);

//        private Listener m_listener;
//        private ListenerConfig m_options;

//        public override void Initialize()
//        {
//            Log.Publish(MessageLevel.Info, MessageFlags.None, "Initializing");

//            base.Initialize();

//            m_options = new ListenerConfig();
//            var al = new AccessList();
//            al.IpAddress = IPAddress.Any;
//            al.BitMask = 0;
//            m_options.AccessList.Add(al);
//            var c = new ClientOptions();
//            c.UserName = "test";
//            m_options.UserOptions.Add(c);
//            m_options.ListenEndpoint = new IPEndPoint(GetMyIPV4(), 48294);
//        }

//        public static IPAddress GetMyIPV4()
//        {
//            String strHostName = Dns.GetHostName();
//            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
//            foreach (IPAddress ipaddress in iphostentry.AddressList)
//            {
//                if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
//                {
//                    return ipaddress;
//                }
//            }
//            return IPAddress.Loopback;
//        }

//        public override void Start()
//        {
//            Log.Publish(MessageLevel.Info, MessageFlags.None, "Starting");

//            base.Start();
//            m_listener = new Listener(m_options);
//            m_listener.Start();
//        }

//        public override void Stop()
//        {
//            Log.Publish(MessageLevel.Info, MessageFlags.None, "Stopping");

//            if (m_listener != null)
//                m_listener.Stop();
//            m_listener = null;
//            base.Stop();
//        }

//        public override MeasurementKey[] InputMeasurementKeys
//        {
//            get
//            {
//                return null;
//            }
//            set
//            {
//                //Do Nothing
//            }
//        }

//        public override string GetShortStatus(int maxLength)
//        {
//            return string.Empty;
//        }

//        public override bool SupportsTemporalProcessing
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public override void QueueMeasurementsForProcessing(IEnumerable<IMeasurement> measurements)
//        {
//            if (m_listener != null)
//            {
//                m_listener.QueueMeasurements(measurements);
//            }
//        }
//    }
//}
