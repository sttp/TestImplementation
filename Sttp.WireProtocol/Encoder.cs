using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class Encoder
    {
        /// <summary>
        /// The bytes that need to be reliably sent. Note, this data is not valid until <see cref="Flush"/> has been called.
        /// </summary>
        public byte[] ReliableSendBuffer { get; private set; }
        /// <summary>
        /// The length of <see cref="ReliableSendBuffer"/>
        /// </summary>
        public int ReliableSendBufferLength { get; private set; }
        /// <summary>
        /// The bytes that can be sent unreliably. Note, this data is not valid until <see cref="Flush"/> has been called.
        /// </summary>
        public byte[] UnreliableSendBuffer { get; private set; }
        /// <summary>
        /// The length of <see cref="UnreliableSendBuffer"/>
        /// </summary>
        public int UnreliableSendBufferLength { get; private set; }

        public void NegotiateSession(bool sendReliably, Version protocolVersionNumber)
        {
         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendReliably"></param>
        /// <param name="responseCode"></param>
        /// <param name="commandCode"></param>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        public void SendCommand(bool sendReliably, byte responseCode, byte commandCode, byte[] data, int position, int length)
        {

        }

        /// <summary>
        /// Begins sending a series of <see cref="DataPointKey"/>, one at a time. After
        /// sending each metadata segment, <see cref="ReliableSendBufferLength"/> or <see cref="UnreliableSendBufferLength"/>
        /// will indicate approximately how much data is pending to be sent. This will allow control of the packet size.
        /// </summary>
        public void RegisterSignalMapping(bool sendReliably, DataPointKey requiredMetadata)
        {
            //Sends this to the client and builds compression state data for it.
        }

        /// <summary>
        /// Begins sending a series of data points, one at a time. After
        /// sending each point, <see cref="ReliableSendBufferLength"/> or <see cref="UnreliableSendBufferLength"/>
        /// will indicate approximately how much data is pending to be sent. This will allow control of the packet size.
        /// </summary>
        /// <param name="sendReliably"></param>
        /// <param name="point"></param>
        public void SendDataPoint(bool sendReliably, DataPoint point)
        {

        }


        /// <summary>
        /// Flushes all pending data to the provided socket.
        /// </summary>
        public void Flush()
        {

        }
    }
}
