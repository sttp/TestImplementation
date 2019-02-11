namespace CTP
{
    /// <summary>
    /// The valid header codes for a <see cref="CtpCommand"/>
    /// </summary>
    internal enum CtpCommandHeader
    {
        StartElement = 0,
        EndElement = 1,
        /// <summary>
        /// Encoded as (Nothing)
        /// </summary>
        ValueNull = 2,
        /// <summary>
        /// Encoded as 7BitInt(Value)
        /// </summary>
        ValueInt64 = 3,
        /// <summary>
        /// Encoded as 7BitInt(~Value)
        /// </summary>
        ValueInvertedInt64 = 4,
        ValueSingle = 5,
        ValueDouble = 6,
        ValueNumeric = 7,
        ValueCtpTime = 8,
        ValueBooleanTrue = 9,
        ValueBooleanFalse =10,
        ValueGuid = 11,
        /// <summary>
        /// Encoded as 7BitInt(DataLength(Value)), Value.
        /// </summary>
        ValueString = 12,
        /// <summary>
        /// Encoded as 7BitInt(DataLength(Value)), Value.
        /// </summary>
        ValueCtpBuffer = 13,
        /// <summary>
        /// Encoded as 7BitInt(DataLength(Value)), Value.
        /// </summary>
        ValueCtpCommand = 14,
    }
}