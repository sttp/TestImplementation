using System;

namespace Sttp.Codec
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class WireDecoder
    {
        //private DataPointDecoder m_dataPointDecoder;
        private CommandDecoder m_packetDecoder;
        private SessionDetails m_sessionDetails;


        public WireDecoder()
        {
            // m_dataPointDecoder = new DataPointDecoder();
            m_sessionDetails = new SessionDetails();
            m_packetDecoder = new CommandDecoder(m_sessionDetails);
        }

        /// <summary>
        /// Writes the wire protocol data to the decoder.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void WriteData(byte[] data, int position, int length)
        {
            m_packetDecoder.WriteData(data, position, length);
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="WriteData"/>
        /// </summary>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public CommandObjects NextCommand()
        {
            PayloadReader reader = m_packetDecoder.NextPacket();
            if (reader == null)
                return null;

            return new CommandObjects(reader);
        }



    }
}
