using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CTP;
using CTP.Net;

namespace Sttp.Codec
{
    public class WireCodec
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
        public WireCodec(Stream session)
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
        /// Writes the wire protocol data to the decoder.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void FillBuffer(byte[] data, int position, int length)
        {
            m_packetDecoder.FillBuffer(data, position, length);
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop. Once all commands have been read, an async read
        /// will occur
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait for a command before returning null. </param>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public CommandObjects NextCommand(int timeout = 0)
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
            return new CommandObjects(m_packetDecoder);
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
            return id;
        }

        public void GetMetadataSchema(Guid? lastKnownRuntimeID = null, long? lastKnownVersionNumber = null)
        {
            m_encoder.Send(0, new CommandGetMetadataSchema(lastKnownRuntimeID, lastKnownVersionNumber));
        }

        public void MetadataSchema(Guid runtimeID, long versionNumber, List<MetadataSchemaTable> tables)
        {
            m_encoder.Send(0, new CommandMetadataSchema(runtimeID, versionNumber, tables));
        }

        public void MetadataSchemaUpdate(Guid runtimeID, long versionNumber, List<MetadataSchemaTableUpdate> tables)
        {
            m_encoder.Send(0, new CommandMetadataSchemaUpdate(runtimeID, versionNumber, tables));
        }

        public void MetadataSchemaVersion(Guid runtimeID, long versionNumber)
        {
            m_encoder.Send(0, new CommandMetadataSchemaVersion(runtimeID, versionNumber));
        }

        public void GetMetadata(string table, IEnumerable<string> columns)
        {
            m_encoder.Send(0, new CommandGetMetadata(table, columns));
        }

        public void MetadataRequestFailed(string reason, string details)
        {
            m_encoder.Send(0, new CommandMetadataRequestFailed(reason, details));
        }

        public void BeginMetadataResponse(ulong rawChannelID, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
        {
            m_encoder.Send(0, new CommandBeginMetadataResponse(rawChannelID, encodingMethod, runtimeID, versionNumber, tableName, columns));
        }

        public void EndMetadataResponse(ulong rawChannelID, int rowCount)
        {
            m_encoder.Send(0, new CommandEndMetadataResponse(rawChannelID, rowCount));
        }

        public void SendCustomCommand(CtpDocument command)
        {
            m_encoder.Send(0, command);
        }

        public void Raw(ulong rawCommandCode, byte[] payload)
        {
            m_encoder.Send(rawCommandCode, payload, 0, payload.Length);
        }

        public void RequestFailed(string origionalCommand, string reason, string details)
        {
            m_encoder.Send(0, new CommandRequestFailed(origionalCommand, reason, details));
        }


    }
}
