using CTP;

namespace Sttp
{
    /// <summary>
    /// An ID that is assigned for each <see cref="SttpDataPoint"/>
    /// </summary>
    public class SttpDataPointID
    {
        /// <summary>
        /// The unique identifier for this PointID. This will typically be a GUID, but may also be a string or integer.
        /// </summary>
        public readonly CtpObject ID;

        /// <summary>
        /// An optional token that can be defined by the API layer. The primary purpose of this token is to speed up
        /// mapping since a dictionary lookup on ID is somewhat expensive.
        /// </summary>
        public object MetadataToken;

        public SttpDataPointID(CtpObject id)
        {
            ID = id;
        }
    }
}