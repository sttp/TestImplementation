namespace Sttp.Codec
{
    public struct SttpDatapointLayoutExtraFields
    {
        /// <summary>
        /// Defines how the extra fields exist and are serialized. Note
        /// there is no limit to the number of fields that can be specified.
        /// 
        /// Bit 0:      Set = Contains an extra field at this location.
        /// Bit 1:      Set = Contains an extra field at this location.
        /// Bit 2:      Set = Contains an extra field at this location.
        /// Bit 3:      Set = Contains an extra field at this location.
        /// Bit 4:      Set = Contains an extra field at this location.
        /// Bit 5:      Set = Contains an extra field at this location.
        /// Bit 6:      Set = Contains an extra field at this location.
        /// Bit 7:      Set = MORE. Additional fields exist. Another one of these structs follow.
        /// 
        /// </summary>
        public byte RawValue;
    }
}