using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This data point is what is used to communicating with the <see cref="EncoderTCP"/>/<see cref="DecoderTCP"/>. 
    /// </summary>
    public class DataPointWire
    {
        /// <summary>
        /// Maps to DataPointKeyWire.RuntimeID
        /// </summary>
        public uint ID;

        /// <summary>
        /// Contains the Value.
        /// </summary>
        public readonly byte[] Value = new byte[64];

        /// <summary>
        /// Increments every time that a new value larger than 64 bytes is created. 0 if the data doesn't need to be fragmented.
        /// </summary>
        public uint Sequence;
        /// <summary>
        /// The fragment index of data that is being sent.
        /// </summary>
        public uint Fragment;
        /// <summary>
        /// The total size of all fragments.
        /// </summary>
        public uint Length;

        public SttpTimestamp Time;

        public TimeQualityFlags Flags;

        public DataQualityFlags QualityFlags;

        //This code goes elsewhere
        //public static List<DataPoint> GetDataPoints(byte[] value)
        //{
        //    //ToDo: Need to correct this
        //    const int MaxBufferSize = MaxValueSize - 1;

        //    List<DataPoint> dataPoints = new List<DataPoint>();

        //    // Fragment value into 15-byte chunks with sequence number
        //    int remainder = value.Length % MaxBufferSize;
        //    int fragments = value.Length / MaxBufferSize + remainder > 0 ? 1 : 0;

        //    for (int i = 0; i < fragments; i++)
        //    {
        //        int length = MaxBufferSize;

        //        if (i == fragments - 1 && remainder > 0)
        //            length = remainder;

        //        var point = new DataPoint();
        //        Array.Copy(value, i * MaxBufferSize, point.Value, 0, length);
        //        point.State = BigEndian.GetBytes((ushort)i);
        //        dataPoints.Add(point);
        //    }

        //    return dataPoints;
        //}
    }
}
