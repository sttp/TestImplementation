﻿using System;
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
        private long m_rawChannelID;
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
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public CtpReadResults Read()
        {
            return TryRead(-1);
        }

        public CtpReadResults TryRead(int timeout)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            tryAgain:
            if (!m_packetDecoder.ReadCommand())
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
            return m_packetDecoder.Results.Clone();
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

        public ulong GetNextRawChannelID()
        {
            ulong id = 0;
            while (id < 16)
            {
                id = (ulong)Interlocked.Increment(ref m_rawChannelID);
            }
            return (ulong)id;
        }

        public void SendDocumentCommand(ulong channelNumber, CtpDocument command)
        {
            SendRaw(channelNumber, command.ToArray());
        }

        public void SendRaw(ulong channelNumber, byte[] payload)
        {
            m_encoder.Send(channelNumber, payload, 0, payload.Length);
        }

        public void SendRaw(ulong channelNumber, byte[] payload, int offset, int length)
        {
            m_encoder.Send(channelNumber, payload, offset, length);
        }


    }
}
