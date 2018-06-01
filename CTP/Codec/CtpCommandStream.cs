using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class CtpCommandStream
    {
        /// <summary>
        /// Occurs when data has been received from the socket. 
        /// This does not mean an entire command has been received and is ready for processing. Call NextCommand 
        /// to determine if a command object is ready to be received.
        /// </summary>
        public event Action DataReceived;

        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private int m_rawChannelID;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];
        private AsyncCallback m_readCallback;
        private WaitCallback m_onDataReceived;
        private ManualResetEvent m_waitForDataEvent = new ManualResetEvent(false);

        public CtpCommandStream(Stream session)
        {
            m_onDataReceived = OnDataReceived;
            m_readCallback = AsyncReadCallback;
            m_packetDecoder = new CtpDecoder();
            m_encoder = new CtpEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
            m_stream = session;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            m_stream.Write(data, position, length);
        }
        
        /// <summary>
        /// Gets the next data packet. This method should be in a while loop. Once all commands have been read, an async read
        /// will occur
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait for a command before returning null. </param>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public CommandObjects2 NextCommand(int timeout = 0)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            tryAgain:
            if (!m_packetDecoder.NextCommand())
            {
                if (AsyncRead())
                {
                    goto tryAgain;
                }

                long timeToWait = timeout - sw.ElapsedMilliseconds;
                if (timeout < 0)
                {
                    m_waitForDataEvent.WaitOne(-1);
                }
                else if (timeout == 0 || timeToWait <= 0)
                {
                    return null;
                }
                else
                {
                    m_waitForDataEvent.WaitOne((int)timeToWait);
                }
                m_waitForDataEvent.Reset();
                goto tryAgain;

            }

            return new CommandObjects2(m_packetDecoder);
        }

        private bool AsyncRead()
        {
            if (!m_isReading)
            {
                lock (m_syncReceive)
                {
                    if (!m_isReading)
                    {
                        m_isReading = true;
                        return m_stream.BeginRead(m_inBuffer, 0, m_inBuffer.Length, AsyncReadCallback, null).CompletedSynchronously;
                    }
                }
            }
            return false;
        }

        private void AsyncReadCallback(IAsyncResult ar)
        {
            lock (m_syncReceive)
            {
                m_isReading = false;
                int length = m_stream.EndRead(ar);
                m_packetDecoder.FillBuffer(m_inBuffer, 0, length);
                m_waitForDataEvent.Set();
            }
            if (!ar.CompletedSynchronously)
            {
                ThreadPool.QueueUserWorkItem(m_onDataReceived);
            }
        }

        private void OnDataReceived(object state)
        {
            DataReceived?.Invoke(); //If this call was completed asynchronously, notify the client that it was fulfilled.
        }

        public int GetNextRawChannelID()
        {
            int id = 0;
            while (id == 0 || id == 1)
            {
                id = Interlocked.Increment(ref m_rawChannelID);
            }
            return id;
        }

        public void SendDocumentCommand(CtpDocument command)
        {
            m_encoder.SendDocumentCommands(command);
        }

        public void SendRaw(int rawCommandCode, byte[] payload)
        {
            m_encoder.SendBinaryCommand(rawCommandCode, payload, 0, payload.Length);
        }


    }
}
