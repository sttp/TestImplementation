using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using CTP.Net;
using GSF.TimeSeries;
using GSF.TimeSeries.Adapters;
using Sttp.Codec.DataPoint;
using Sttp.Services;

namespace Sttp.Adapters
{
    public class SttpPublisher
        : FacileActionAdapterBase
    {
        private class CtpUser
        {
            private SttpPublisher m_home;
            private SessionToken m_token;
            private SttpServer m_server;
            private byte[] m_subscribeList;
            private BasicEncoder m_encoder;
            private int m_encoderID;

            public CtpUser(SttpPublisher home, SessionToken token)
            {
                m_home = home;
                m_token = token;
                m_server = new SttpServer(token);
                m_server.RegisterCommandHandler(home.m_metadata);
                m_server.Start();
            }

            public void ProcessMeasurements(List<SttpDataPoint> dataPoints)
            {
                byte[] subscribeList = m_subscribeList;
                if (subscribeList != null)
                {
                    bool hasData = false;
                    foreach (var datapoint in dataPoints)
                    {
                        if (0 <= datapoint.DataPointRuntimeID && datapoint.DataPointRuntimeID < subscribeList.Length * 8)
                        {
                            byte mask = subscribeList[datapoint.DataPointRuntimeID >> 3];
                            if ((mask & (1 << (datapoint.DataPointRuntimeID & 7))) != 0)
                            {
                                m_encoder.AddDataPoint(datapoint);
                                hasData = true;
                            }
                        }
                    }

                    if (hasData)
                    {
                        m_server.Codec.Raw(m_encoderID, m_encoder.ToArray());
                        m_encoder.Clear();
                    }
                }
            }
        }

        //private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(SttpPublisher), MessageClass.Application);

        private List<CtpUser> Users = new List<CtpUser>();
        private CtpListener m_listener;
        private SttpMetadataServer m_metadata;

        public override void Initialize()
        {
            //Log.Publish(MessageLevel.Info, MessageFlags.None, "Initializing");

            base.Initialize();

            m_listener = new CtpListener(new IPEndPoint(GetMyIPV4(), 48294));
            m_listener.Permissions.AddSrpUser("TrialUser", "P@$$w0rd", "Demo User", "Can Read");
            m_listener.SessionCompleted += M_listener_SessionCompleted;
            m_metadata = new SttpMetadataServer();
            m_metadata.DefineSchema(DataSource);
            m_metadata.FillData(DataSource);
            m_metadata.CommitData();
        }

        private void M_listener_SessionCompleted(SessionToken token)
        {
            lock (m_listener)
            {
                Users.Add(new CtpUser(this, token));
            }
        }

        public static IPAddress GetMyIPV4()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipaddress;
                }
            }
            return IPAddress.Loopback;
        }

        public override void Start()
        {
            //Log.Publish(MessageLevel.Info, MessageFlags.None, "Starting");
            m_listener.Start();
            base.Start();
        }

        public override void Stop()
        {
            //Log.Publish(MessageLevel.Info, MessageFlags.None, "Stopping");
            m_listener.Stop();
            base.Stop();
        }

        public override MeasurementKey[] InputMeasurementKeys
        {
            get
            {
                return null;
            }
            set
            {
                //Do Nothing
            }
        }

        public override string GetShortStatus(int maxLength)
        {
            return string.Empty;
        }

        public override bool SupportsTemporalProcessing
        {
            get
            {
                return false;
            }
        }

        public override void QueueMeasurementsForProcessing(IEnumerable<IMeasurement> measurements)
        {

            List<SttpDataPoint> dataPoints = new List<SttpDataPoint>();
            foreach (var measurement in measurements)
            {
                var dataPoint = new SttpDataPoint();
                dataPoint.Time.SetValue((DateTime)measurement.Timestamp);
                dataPoint.DataPointID.SetValue(measurement.ID);
                dataPoint.DataPointRuntimeID = measurement.Key.RuntimeID;
                dataPoint.Value.SetValue(measurement.AdjustedValue);
                dataPoint.Quality = (int)measurement.StateFlags;
                dataPoints.Add(dataPoint);
            }

            lock (Users)
            {
                foreach (var client in Users)
                {
                    client.ProcessMeasurements(dataPoints);
                }
            }
        }
    }
}
