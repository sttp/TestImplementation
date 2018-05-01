//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Security;
//using System.Net.Sockets;
//using System.Security.Authentication;
//using System.Text;
//using System.Threading;
//using GSF;
//using GSF.Diagnostics;
//using GSF.Threading;
//using GSF.TimeSeries;

//namespace Sttp.Adapters
//{
//    public class ListenerClient
//    {
//        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(ListenerClient), MessageClass.Application);
//        private ListenerConfig m_options;
//        private TcpClient m_socket;
//        private NetworkStream m_netStream;
//        private SslStream m_sslStream;
//        private ScheduledTask m_task;
//        private ConcurrentQueue<IEnumerable<IMeasurement>> m_measurementQueue;
//        private bool m_disposing;

//        private long m_measurementsDiscarded;
//        private long m_measurementsProcessed;
//        private int m_measurementsInQueue;
//        private ScheduledTask m_statusReport;
//        public ShortTime TimeoutTime;

//        public ListenerClient(TcpClient socket, ListenerConfig options)
//        {
//            if (socket == null)
//                throw new ArgumentNullException(nameof(socket));
//            if (options == null)
//                throw new ArgumentNullException(nameof(options));

//            m_disposing = false;
//            m_measurementQueue = new ConcurrentQueue<IEnumerable<IMeasurement>>();
//            m_task = new ScheduledTask();
//            m_task.Running += m_task_Running;
//            m_task.Disposing += m_task_Disposing;
//            m_task.UnhandledException += m_task_UnhandledException;
//            m_statusReport = new ScheduledTask();
//            m_statusReport.Running += m_statusReport_Running;
//            m_statusReport.Start(10000);
//            TimeoutTime = ShortTime.Now.AddSeconds(10);

//            try
//            {
//                m_socket = socket;
//                m_options = options;
//                m_netStream = m_socket.GetStream();
//                m_sslStream = new SslStream(m_netStream, false);
//                m_sslStream.AuthenticateAsServer(m_options.ServerCertificateX509, false, SslProtocols.Tls, false);

//                if (!m_sslStream.IsAuthenticated)
//                    throw new Exception("Not Authenticated");
//                if (!m_sslStream.IsEncrypted)
//                    throw new Exception("Not Encrypted");
//                if (!m_sslStream.IsSigned)
//                    throw new Exception("Data Not Signed");

//                //m_clientStream = new BufferedBinaryStream(m_sslStream);

//                Log.Publish(MessageLevel.Info, MessageFlags.None, "Processing OGE Protocol Request");

//                ClientOptions user;

//                byte code = m_clientStream.ReadNextByte();
//                if (code == 6)
//                {
//                    Log.Publish(MessageLevel.Info, MessageFlags.None, "code=6");
//                    if (TryAuthenticate(m_clientStream, out user))
//                    {
//                        m_task.Start(100); //Start the processing of measurements.
//                        return;
//                    }
//                    else
//                    {
//                        Log.Publish(MessageLevel.Info, MessageFlags.None, "Not Authenticated");
//                    }
//                }
//                else
//                {
//                    Log.Publish(MessageLevel.Info, MessageFlags.None, "Version not found", code.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Publish(MessageLevel.Error, MessageFlags.None, "Error while processing client", null, null, ex);
//            }
//            m_disposing = true;
//            m_task.Dispose();
//        }

//        private void OnBufferFull()
//        {
//            m_compression.CopyTo(m_clientStream);
//            m_compression.Clear();
//        }

//        void m_statusReport_Running(object sender, GSF.EventArgs<ScheduledTaskRunningReason> e)
//        {
//            m_statusReport.Start(10000);
//            Log.Publish(MessageLevel.Info, MessageFlags.None, "Health Status", String.Format("Discarded: {0} Processed: {1} InQueue: {2}"
//            , m_measurementsDiscarded.ToString("N0")
//            , m_measurementsProcessed.ToString("N0")
//            , m_measurementsInQueue.ToString("N0")));
//        }

//        public void QueueMeasurements(IEnumerable<IMeasurement> measurements)
//        {
//            int cnt = measurements.Count();
//            if (m_disposing)
//                return;
//            if (m_measurementsInQueue > 1000000)
//            {
//                Interlocked.Add(ref m_measurementsDiscarded, cnt);
//                return;
//            }

//            int queueSize = Interlocked.Add(ref m_measurementsInQueue, cnt);
//            m_measurementQueue.Enqueue(measurements);

//            if (queueSize > 1000)
//            {
//                m_task.Start();
//            }
//        }

//        void m_task_UnhandledException(object sender, GSF.EventArgs<Exception> e)
//        {
//            Log.Publish(MessageLevel.Error, MessageFlags.None, "Unhandled Exception. Closing", null, null, e.Argument);
//            m_disposing = true;
//            m_task.Dispose();
//        }

//        void m_task_Disposing(object sender, EventArgs e)
//        {
//            m_disposing = true;
//            try
//            {
//                m_clientStream.Dispose();
//            }
//            catch (Exception)
//            {
//            }
//            try
//            {
//                m_sslStream.Dispose();
//            }
//            catch (Exception)
//            {
//            }
//            try
//            {
//                m_netStream.Dispose();
//            }
//            catch (Exception)
//            {
//            }

//            try
//            {
//                ((IDisposable)m_socket).Dispose();
//            }
//            catch (Exception)
//            {
//            }

//        }

//        void m_task_Running(object sender, GSF.EventArgs<ScheduledTaskRunningReason> e)
//        {
//            if (e.Argument == ScheduledTaskRunningReason.Disposing)
//                return;
//            if (m_disposing)
//                return;

//            while (m_clientStream.AvailableBytes > 0)
//            {
//                var nextByte = m_clientStream.ReadNextByte();
//                if (nextByte == (byte)GEPCommands.Quit)
//                {
//                    m_compression.AddCommand((byte)GEPCommands.Quit);
//                    OnBufferFull();
//                    m_clientStream.Flush();

//                    Log.Publish(MessageLevel.Error, MessageFlags.None, "Client sent Quit");
//                    m_disposing = true;
//                    m_task.Dispose();
//                    return;
//                }
//                else
//                {
//                    Log.Publish(MessageLevel.Error, MessageFlags.None, "Unknown Message", nextByte.ToString());
//                    m_disposing = true;
//                    m_task.Dispose();
//                    return;
//                }
//            }

//            int dataSent = 0;
//            IEnumerable<IMeasurement> measurements;
//            while (m_measurementQueue.TryDequeue(out measurements))
//            {
//                foreach (var measurement in measurements)
//                {
//                    dataSent++;
//                    if ((dataSent & 1023) == 0)
//                    {
//                        TimeoutTime = ShortTime.Now.AddSeconds(10);
//                    }
//                    m_compression.AddMeasurement(measurement.Key.SignalID, measurement.Timestamp.Value, (uint)measurement.StateFlags, (float)measurement.AdjustedValue);
//                }
//                TimeoutTime = ShortTime.Now.AddSeconds(10);
//                m_compression.AddCommand((byte)GEPCommands.EndOfFrame);
//                int cnt = measurements.Count();
//                Interlocked.Add(ref m_measurementsInQueue, -cnt);
//                Interlocked.Add(ref m_measurementsProcessed, cnt);
//            }
//            m_compression.AddCommand((byte)GEPCommands.EndOfBatch);
//            OnBufferFull();
//            m_clientStream.Flush();

//            if (dataSent < 1000)
//            {
//                m_task.Start(100);
//            }
//            else
//            {
//                m_task.Start();
//            }
//        }

//        private bool TryAuthenticate(BufferedBinaryStream stream, out ClientOptions user)
//        {
//            var session = m_options.SrpUsers.AuthenticateAsServer(stream, Encoding.UTF8.GetBytes(m_options.ServerCertificateX509.Thumbprint));
//            if (session != null)
//            {
//                user = session.Token;
//                return true;
//            }
//            user = null;
//            return false;
//        }

//        public void Dispose()
//        {
//            m_disposing = true;
//            m_task.Dispose();
//        }
//    }
//}
