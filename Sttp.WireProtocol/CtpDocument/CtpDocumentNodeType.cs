namespace CTP
{
    /// <summary>
    /// The valid node types of <see cref="CtpDocument"/>
    /// </summary>
    internal enum CtpDocumentNodeType : byte
    {
        /// <summary>
        /// Indicates that the node type is an element.
        /// </summary>
        StartElement = 0,
        /// <summary>
        /// Indicates that the node type if a value.
        /// </summary>
        Value = 1,
        /// <summary>
        /// Specifies that the node type is the ending marker of a Element, Values don't have ending markers.
        /// </summary>
        EndElement = 2,
        /// <summary>
        /// Specifies that the end of the document has occurred.
        /// </summary>
        EndOfDocument = 3,
        /// <summary>
        /// Specifies that the current node is at the start of the document. This value will never be serialized 
        /// to the stream, but rather is a placeholder during the reading process.
        /// </summary>
        StartOfDocument = 255, 
    }

    internal enum CtpDocumentHeader
    {
        ValueNull = 0,
        ValueInt64 = 1,
        ValueSingle = 2,
        ValueDouble = 3,
        ValueCtpTime = 4,
        ValueBoolean = 5,
        ValueGuid = 6,
        ValueString = 7,
        ValueCtpBuffer = 8,
        ValueCtpDocument = 9,
        StartElement = 10,
        EndElement = 11,
    }
}