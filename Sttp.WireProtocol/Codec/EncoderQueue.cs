using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.WireProtocol.Codec
{
    public class DataPacket
    {
        public byte[] Data;
        public int Length;
    }

    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class EncoderQueue
    {
        /// <summary>
        /// Low volume queue has high priority.
        /// </summary>
        public Queue<DataPacket> CommandQueue;
       
        /// <summary>
        /// High volume queue that is less important as the command queue.
        /// </summary>
        public Queue<DataPacket> StreamingQueue;

        /// <summary>
        /// Large blocks of data to be sent. Less than 1MB.
        /// </summary>
        public Queue<DataPacket> BulkQueue;

        /// <summary>
        /// Large blocks of data to be sent. More than 1MB.
        /// </summary>
        public Queue<DataPacket> EnormousQueue;

        public void QueueMessage(DataPacket packet)
        {
            
            
        }

     



    }
}
